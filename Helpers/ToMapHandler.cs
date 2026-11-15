using Archipelago.Core.Util;

namespace MedievilArchipelago.Helpers
{
    // "To Map" pause option: the pause menu's Restart row is pointed at the Complete Level cheat (exit code 5), so
    // choosing it leaves the level for the map. The client then puts back the map fog and the level's cleared flag
    // that Complete Level changes.
    //
    // Zarok's Lair: exit code 5 there plays the ending (the game checks level == 0x17 after a level is completed).
    // While the pause menu is open there, the level-complete jump-table entry is pointed past that check, then put
    // back. It's set on opening the pause menu, not on the X press: the game handles the exit within a frame or two
    // of the press, before the client can react. Gameplay is frozen while paused, so the real Zarok kill can't be
    // redirected.
    // See ~/coding/ap/docs/medievil/level-exit-flow.md.
    //
    // One loop running a small state machine:
    // InLevel -> (pause opened) Paused -> (To Map chosen) Exiting -> (15 seconds of holding the cleared flags at 0) InLevel
    internal static class ToMapHandler
    {
        private const byte HallOfHeroesId = 0x12;
        private const byte ZaroksLairId = 0x17;
        private const byte PauseMenuOpen = 0x01;
        private const byte InLevelState = 0x03;
        private const byte MapState = 0x04;
        private const uint LevelCompleteRequest = 5; // exit request written by Complete Level (and a real level clear)

        public static TimeSpan PollInterval { get; set; } = TimeSpan.FromMilliseconds(50);
        // poll faster while the pause menu is open (so To Map is seen quickly) and while leaving through To Map
        // (so the cleared flags are held at 0 between the game's writes)
        public static TimeSpan FastPollInterval { get; set; } = TimeSpan.FromMilliseconds(10);
        // how long after To Map is chosen the cleared flags are held at 0
        public static TimeSpan ClearSuppressTime { get; set; } = TimeSpan.FromSeconds(15);
        // how long after To Map is chosen the map fog is put back
        public static TimeSpan MapFogRestoreDelay { get; set; } = TimeSpan.FromMilliseconds(100);

        private enum State { InLevel, Paused, Exiting }
        private static State _state = State.InLevel;

        private static byte _menuLevel;     // level the pause menu was last set up for (0 = not set up)
        private static bool _toMapEnabled;  // whether Restart is currently swapped for To Map
        private static byte[] _mapFog = Array.Empty<byte>(); // map fog when the pause menu opened
        private static DateTime _exitStarted; // when To Map was chosen

        public static Task Start(CancellationToken token) => Task.Run(async () =>
        {
            while (!token.IsCancellationRequested)
            {
                try
                {
                    await Tick();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"To Map check failed: {ex.Message}");
                }
                await Task.Delay(_state == State.InLevel ? PollInterval : FastPollInterval, token);
            }
        }, token);

        private static async Task Tick()
        {
            byte level = Memory.ReadByte(Addresses.CurrentLevel);
            bool inLevel = PlayerStateHandler.isInTheGame()
                && level > 0 && level < 24
                && Memory.ReadByte(Addresses.Game_State) == InLevelState;

            switch (_state)
            {
                case State.InLevel:
                    if (!inLevel)
                    {
                        _menuLevel = 0; // set the menu up again when the next level loads
                        return;
                    }
                    if (level != _menuLevel)
                        SetUpPauseMenu(level);
                    // not paused, so the real level-complete handler must be in place
                    SetZarokRedirect(false);

                    if (_toMapEnabled && Memory.ReadByte(Addresses.OpenPopup) == PauseMenuOpen)
                    {
                        _mapFog = BitConverter.GetBytes(Memory.ReadUInt(Addresses.CopyProtection));
                        _state = State.Paused;
                        if (level == ZaroksLairId)
                            SetZarokRedirect(true);
                    }
                    break;

                case State.Paused:
                    // Choosing To Map closes the pause menu, so "unpaused" alone doesn't mean To Map wasn't chosen.
                    // Read the unpause first, then the exit request: To Map writes the request no later than the
                    // menu closes, so if it was chosen the request is already there.
                    bool closed = Memory.ReadByte(Addresses.DanFrozen) != 1 || !inLevel;
                    // While a level is being played the exit request is always 0 (the game leaves the level as
                    // soon as it isn't), so 5 here means To Map (Complete Level) was just chosen.
                    bool toMapChosen = Memory.ReadUInt(Addresses.LevelExitRequest) == LevelCompleteRequest;

                    if (toMapChosen)
                    {
                        await Task.Delay(MapFogRestoreDelay);
                        Memory.WriteByteArray(Addresses.CopyProtection, _mapFog);
                        _exitStarted = DateTime.Now;
                        _state = State.Exiting;
                    }
                    else if (closed)
                    {
                        // closed the pause menu without To Map
                        SetZarokRedirect(false);
                        _state = State.InLevel;
                    }
                    break;

                case State.Exiting:
                    // Complete Level sets the "newly cleared" flag when it runs its exit, and the map copies that flag
                    // into Cleared_Level_Byte when it loads. Hold both at 0 for a while so To Map never shows a clear.
                    // A vanilla level clear never gets here, so it still shows one.
                    Memory.WriteByteArray(Addresses.LevelNewlyCleared, BitConverter.GetBytes(0u));
                    // Cleared_Level_Byte is in the overlay area: only the map's while on the map. Writing it in a
                    // level would overwrite part of that level's overlay.
                    if (Memory.ReadByte(Addresses.Game_State) == MapState)
                        Memory.WriteByte(Addresses.Cleared_Level_Byte, 0x00);

                    // once the level has been left, the game has handled the exit, so the Zarok redirect can go
                    if (!inLevel)
                        SetZarokRedirect(false);

                    if (DateTime.Now - _exitStarted >= ClearSuppressTime)
                        FinishExit();
                    break;
            }
        }

        private static void FinishExit()
        {
            SetZarokRedirect(false);
            _menuLevel = 0;
            _state = State.InLevel;
        }

        // Restart is swapped for To Map unless Dan's Crypt hasn't been cleared (new game) or Dan is in the Hall of Heroes
        private static void SetUpPauseMenu(byte level)
        {
            _menuLevel = level;
            _toMapEnabled = Memory.ReadBit(Addresses.DC_LevelStatus, 4) && level != HallOfHeroesId;

            if (_toMapEnabled)
            {
                Memory.WriteByte(Addresses.Restart_Visibility, 0x00);
                Memory.WriteByte(Addresses.Restart_Function, 0x44); // Complete Level's handler
                Memory.WriteByteArray(Addresses.Restart_Text, BitConverter.GetBytes(Addresses.Text_ToMap));
            }
            else
            {
                Memory.WriteByte(Addresses.Restart_Visibility, 0x02);
                Memory.WriteByte(Addresses.Restart_Function, 0x08); // Restart's handler
                Memory.WriteByteArray(Addresses.Restart_Text, BitConverter.GetBytes(Addresses.Text_Restart));
            }

            // Set Ant Caves as visited so you don't end up there from Enchanted Earth
            Memory.WriteBit(Addresses.TA_LevelStatus, 0, true);
        }

        // read back rather than tracked in a flag: loading a save state puts the old value back
        private static void SetZarokRedirect(bool redirect)
        {
            uint target = redirect ? Addresses.LevelComplete_SkipZarokEnding : Addresses.LevelComplete_Default;
            if (Memory.ReadUInt(Addresses.Zarok_Complete) != target)
                Memory.WriteByteArray(Addresses.Zarok_Complete, BitConverter.GetBytes(target));

            // the redirect skips the code that zeroes the "newly cleared" flag, so the map would get whatever the last
            // completed level left there and show a clear. Only exits read it, and each exit sets it again, so
            // zeroing it early is harmless.
            if (redirect)
                Memory.WriteByteArray(Addresses.LevelNewlyCleared, BitConverter.GetBytes(0u));
        }
    }
}
