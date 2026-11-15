using Archipelago.Core.Util;

namespace MedievilArchipelago.Helpers
{
    // "To Map" pause option: the pause menu's Restart row is pointed at the Complete Level cheat (exit code 5), so
    // choosing it leaves the level for the map. The client then puts back the map fog and the level's cleared flag
    // that Complete Level changes.
    //
    // Zarok's Lair: exit code 5 there plays the ending (the game checks level == 0x17 after a level is completed).
    // While To Map is used there, the level-complete jump-table entry is pointed past that check, then put back.
    // See ~/coding/ap/docs/medievil/level-exit-flow.md.
    //
    // One loop running a small state machine:
    // InLevel -> (pause opened) Paused -> (To Map chosen) Exiting -> (level clear cleanup ran) InLevel
    internal static class ToMapHandler
    {
        private const byte HallOfHeroesId = 0x12;
        private const byte ZaroksLairId = 0x17;
        private const byte PauseMenuOpen = 0x01;
        private const byte InLevelState = 0x03;

        public static TimeSpan PollInterval { get; set; } = TimeSpan.FromMilliseconds(50);
        // the X press only lasts a frame or two, so poll faster while the pause menu is open
        public static TimeSpan PausedPollInterval { get; set; } = TimeSpan.FromMilliseconds(10);
        // how long after To Map is chosen the map fog is put back
        public static TimeSpan MapFogRestoreDelay { get; set; } = TimeSpan.FromMilliseconds(100);

        private enum State { InLevel, Paused, Exiting }
        private static State _state = State.InLevel;

        private static byte _menuLevel;     // level the pause menu was last set up for (0 = not set up)
        private static bool _toMapEnabled;  // whether Restart is currently swapped for To Map
        private static uint _selectedRowAddress; // where the pause menu stores the selected row
        private static byte[] _mapFog = Array.Empty<byte>(); // map fog when the pause menu opened
        private static bool _zarokRedirected;
        private static bool _leftLevel; // seen out of the level since To Map was chosen

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
                await Task.Delay(_state == State.Paused ? PausedPollInterval : PollInterval, token);
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

                    if (_toMapEnabled && Memory.ReadByte(Addresses.OpenPopup) == PauseMenuOpen)
                    {
                        // menu pointer is 0x80xxxxxx; the selected row is at +0xC
                        _selectedRowAddress = Memory.ReadUInt(Addresses.CurrentMenu) - 0x7FFFFFF4;
                        _mapFog = BitConverter.GetBytes(Memory.ReadUInt(Addresses.CopyProtection));
                        _state = State.Paused;
                    }
                    break;

                case State.Paused:
                    if (Memory.ReadByte(Addresses.DanFrozen) != 1 || !inLevel)
                    {
                        _state = State.InLevel; // closed the pause menu without To Map
                        return;
                    }
                    if (Memory.ReadUInt(_selectedRowAddress) == Addresses.PauseMenu_RestartRow
                        && Memory.ReadUShort(Addresses.ButtonX_MenuPress) == 0xFFFF)
                    {
                        if (level == ZaroksLairId)
                            SetZarokRedirect(true);

                        await Task.Delay(MapFogRestoreDelay);
                        Memory.WriteByteArray(Addresses.CopyProtection, _mapFog);
                        _leftLevel = false;
                        _state = State.Exiting;
                    }
                    break;

                case State.Exiting:
                    if (Memory.ReadUInt(Addresses.Level_Clear_Reset) == 0x01)
                    {
                        // undo the cleared flag Complete Level set
                        Memory.WriteByte(Addresses.Cleared_Level_Byte, 0x00);
                        FinishExit();
                    }
                    else if (!PlayerStateHandler.isInTheGame() || (_leftLevel && inLevel))
                    {
                        // quit to the title, or a level loaded without the cleanup being seen:
                        // don't leave the Zarok redirect in place
                        FinishExit();
                    }
                    else if (!inLevel)
                    {
                        // the game is still "in level" while the pause menu fades out, so a level only counts as
                        // newly loaded once it has been left first
                        _leftLevel = true;
                    }
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

        private static void SetZarokRedirect(bool redirect)
        {
            if (redirect == _zarokRedirected)
                return;
            uint target = redirect ? Addresses.LevelComplete_SkipZarokEnding : Addresses.LevelComplete_Default;
            Memory.WriteByteArray(Addresses.Zarok_Complete, BitConverter.GetBytes(target));
            _zarokRedirected = redirect;
        }
    }
}
