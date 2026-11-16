using System.Text;
using Archipelago.Core.Util;

namespace MedievilArchipelago.Helpers
{
    internal static class ExitToMapHandler
    {
        private const uint LevelMode = 3;
        private const uint MapMode = 4;
        private const uint ExitMessageId = 303;
        private const uint ExitStateHandler = 0x8006DE18;
        private const uint VanillaMessageId = 301;
        private const uint VanillaHandler = 0x8006DD08;
        private const uint VanillaFlags = 0x200;
        private const uint AsylumGroundsLevel = 0x0D;
        private const ushort InsideTheAsylumLevel = 0x0E;
        private const uint ZarokLevel = 0x17;
        private const uint ZarokEndingExiting = 0x8005A464;
        private const uint ZarokEndingDefault = 0x8005A3D4;
        private const byte CompletedBit = 0x01;
        private const byte SuppressCompletionBit = 0x08;
        private static readonly byte[] Label = Encoding.ASCII.GetBytes("Go To Map\0");
        private static readonly HashSet<uint> UnsupportedLevels = new() { 0x12, 0x18 };

        private static readonly TimeSpan PatchDelay = TimeSpan.FromSeconds(5);
        private static readonly TimeSpan PatchRefresh = TimeSpan.FromSeconds(5);
        private static readonly TimeSpan CloseDebounce = TimeSpan.FromMilliseconds(150);
        private static readonly TimeSpan MapPromptGuard = TimeSpan.FromSeconds(15);

        private static Task? _worker;
        private static uint _lastMode;
        private static DateTime _levelStarted;
        private static DateTime _lastPatch;

        private static bool _armed;
        private static bool _ownsBit;
        private static bool _redirectedExit;
        private static uint _armedLevel;
        private static uint _armedRecords;
        private static uint _savedClearedMask;
        private static byte _savedVisited;
        private static byte _savedFlags;
        private const byte ClearedLocationBit = 0x10;
        private static DateTime _closedSince;
        private static DateTime _guardMapUntil;

        public static void Start(CancellationToken token)
        {
            if (_worker != null) return;
            _worker = Task.Run(async () =>
            {
                while (!token.IsCancellationRequested)
                {
                    try
                    {
                        await Task.Delay(50, token);
                        Tick();
                    }
                    catch (OperationCanceledException)
                    {
                        return;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Exit to map failed: {ex.Message}");
                        await Task.Delay(1000, CancellationToken.None);
                    }
                }
            }, token);
        }

        private static void Tick()
        {
            if (!PlayerStateHandler.isInTheGame()) return;

            uint mode = Memory.ReadUInt(Addresses.GameMode);
            if (mode == LevelMode && _lastMode != LevelMode) _levelStarted = DateTime.UtcNow;
            _lastMode = mode;

            if (_armed)
            {
                if (mode == MapMode) FinishExit();
                else if (mode != LevelMode) Disarm();
            }

            if (mode == MapMode && DateTime.UtcNow < _guardMapUntil) SuppressMapPrompt();

            if (mode != LevelMode) return;

            uint level = Memory.ReadUInt(Addresses.LevelBeingPlayed);
            if (UnsupportedLevels.Contains(level))
            {
                if (_armed) Disarm();
                RestoreVanillaRecord();
                return;
            }

            // a save state can swap the level under us while armed
            if (_armed && level != _armedLevel) Disarm();

            // Arm before the PatchDelay check: the pause record stays patched from the last level, so "Go To Map" can be
            // picked straight after a level start (e.g. loading a save state from the map). Unarmed, Zarok's Lair rolls the credits.
            bool paused = ShopHandler.IsMenuOpen(Addresses.PauseMenuId);
            if (paused)
            {
                _closedSince = default;
                if (!_armed) Arm(level);
                else Reassert();
            }
            else if (_armed)
            {
                if (_closedSince == default) _closedSince = DateTime.UtcNow;
                if (DateTime.UtcNow - _closedSince >= CloseDebounce && Memory.ReadUInt(Addresses.PendingExitState) == 0) Disarm();
            }

            if (DateTime.UtcNow - _levelStarted < PatchDelay) return;
            if (DateTime.UtcNow - _lastPatch >= PatchRefresh) ApplyRecord();
        }

        // Save states undo our RAM writes, so while armed keep the Zarok redirect, the Asylum redirect and our bit 3 in place
        private static void Reassert()
        {
            if (_armedLevel == ZarokLevel && Memory.ReadUInt(Addresses.ZarokEndingPointer) != ZarokEndingExiting)
                Memory.Write(Addresses.ZarokEndingPointer, ZarokEndingExiting);
            if (_redirectedExit && Memory.ReadUShort(Addresses.AsylumGroundsExitDestination) != 0)
                Memory.Write(Addresses.AsylumGroundsExitDestination, (ushort)0);
            if (_ownsBit)
            {
                uint flagsAddress = _armedRecords + _armedLevel + 0x34;
                byte flags = Memory.ReadByte(flagsAddress);
                if ((flags & SuppressCompletionBit) == 0) Memory.WriteByte(flagsAddress, (byte)(flags | SuppressCompletionBit));
            }
        }

        private static void ApplyRecord()
        {
            _lastPatch = DateTime.UtcNow;
            Memory.Write(Addresses.PauseRestartRecord, ExitMessageId);
            Memory.Write(Addresses.PauseRestartRecord + 4, ExitStateHandler);
            Memory.Write(Addresses.PauseRestartRecord + 0x14, 0u);
            Memory.Write(Addresses.ExitMessageText, Label);
        }

        private static void RestoreVanillaRecord()
        {
            if (Memory.ReadUInt(Addresses.PauseRestartRecord + 4) == VanillaHandler) return;
            Memory.Write(Addresses.PauseRestartRecord, VanillaMessageId);
            Memory.Write(Addresses.PauseRestartRecord + 4, VanillaHandler);
            Memory.Write(Addresses.PauseRestartRecord + 0x14, VanillaFlags);
        }

        private static uint RecordsBase() => Memory.ReadUInt(Addresses.LevelRecordsPointer) & 0x1FFFFF;

        private static void Arm(uint level)
        {
            uint records = RecordsBase();
            uint flagsAddress = records + level + 0x34;
            byte flags = Memory.ReadByte(flagsAddress);

            _armedLevel = level;
            _armedRecords = records;
            _savedClearedMask = Memory.ReadUInt(records + 0x30);
            _savedVisited = Memory.ReadByte(records + level + 0x50);
            _savedFlags = flags;
            _ownsBit = (flags & SuppressCompletionBit) == 0;
            if (_ownsBit) Memory.WriteByte(flagsAddress, (byte)(flags | SuppressCompletionBit));
            if (level == AsylumGroundsLevel)
            {
                Memory.Write(Addresses.AsylumGroundsExitDestination, (ushort)0);
                _redirectedExit = true;
            }
            if (level == ZarokLevel) Memory.Write(Addresses.ZarokEndingPointer, ZarokEndingExiting);
            _armed = true;
        }

        private static void Disarm()
        {
            if (_ownsBit)
            {
                uint flagsAddress = _armedRecords + _armedLevel + 0x34;
                byte flags = Memory.ReadByte(flagsAddress);
                if ((flags & SuppressCompletionBit) != 0) Memory.WriteByte(flagsAddress, (byte)(flags & ~SuppressCompletionBit));
            }
            UndoRedirect();
            RestoreZarokEnding();
            _armed = false;
            _ownsBit = false;
            _closedSince = default;
        }

        private static void RestoreZarokEnding()
        {
            if (_armedLevel == ZarokLevel) Memory.Write(Addresses.ZarokEndingPointer, ZarokEndingDefault);
        }

        private static void UndoRedirect()
        {
            if (!_redirectedExit) return;
            Memory.Write(Addresses.AsylumGroundsExitDestination, InsideTheAsylumLevel);
            _redirectedExit = false;
        }

        private static void SuppressMapPrompt()
        {
            Memory.Write(Addresses.NewlyClearedFlag, 0u);
            if (PopupHandler.IsOnWorldMap() && Memory.ReadUInt(Addresses.MapPromptPending) != 0) Memory.Write(Addresses.MapPromptPending, 0u);
        }

        private static void FinishExit()
        {
            uint flagsAddress = _armedRecords + _armedLevel + 0x34;
            byte flags = Memory.ReadByte(flagsAddress);
            // the level-complete handler clears our bit 3 itself, but Zarok's redirect skips that handler, so drop it here
            // (left set, the next Arm wouldn't own it)
            if (_ownsBit && (flags & SuppressCompletionBit) != 0)
            {
                flags = (byte)(flags & ~SuppressCompletionBit);
                Memory.WriteByte(flagsAddress, flags);
            }
            // Armed means the pause menu was open, so reaching the map can only be "Go To Map": always undo what the exit did.
            // Don't make this depend on owning bit 3: some levels set it themselves (Enchanted Earth does), and skipping this
            // leaves the "newly cleared" flag up, so the map marks the level cleared.
            Memory.Write(_armedRecords + 0x30, _savedClearedMask);
            Memory.WriteByte(_armedRecords + _armedLevel + 0x50, _savedVisited);
            // the "Cleared:" locations watch bit 4 of this same flags byte, so put it back as it was
            Memory.WriteByte(flagsAddress, (byte)((flags & ~ClearedLocationBit) | (_savedFlags & ClearedLocationBit)));
            Memory.Write(Addresses.NewlyClearedFlag, 0u);
            _guardMapUntil = DateTime.UtcNow + MapPromptGuard;
            if ((_savedFlags & CompletedBit) == 0)
                Console.WriteLine($"Left {LevelHandlers.GetLevelNameFromId((byte)_armedLevel)} for the map without clearing it.");
            UndoRedirect();
            RestoreZarokEnding();
            _armed = false;
            _ownsBit = false;
            _closedSince = default;
        }
    }
}
