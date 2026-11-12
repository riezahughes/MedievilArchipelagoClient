using Archipelago.Core;
using Archipelago.Core.Util;

namespace MedievilArchipelago.Helpers
{
    // Knights of Gallowmere: the cutscene that sends Dan to the arena in Zarok's Lair only starts once he's
    // received knights_to_find knights. Until then, walking into the circle around the trigger freezes him
    // (forward speed 0, X/Square/Start off), shows a popup, then puts him back where he enters the level and
    // hands control back.
    //
    // This runs on its own fast loop rather than the GPS handler: the GPS ticks every 500ms and reports the
    // respawn position, which lags behind Dan, so he could get onto the trigger between ticks.
    internal static class ZaroksLairGateHandler
    {
        public const string KnightItemName = "Knight of Gallowmere";
        private const byte ZaroksLairId = 0x17;

        // Blocked circle around the trigger, in Addresses.DanPosition units (X, Z; Y is height). The centre is the
        // middle of four points measured on the trigger's edge in game (X -871..774, Z -968..1314). The radius was
        // tuned in game.
        // Build in DEBUG and walk around to check it: the loop prints Dan's position.
        public static int GateX { get; set; } = -48;
        public static int GateZ { get; set; } = 173;
        public static int GateRadius { get; set; } = 900;

        public static TimeSpan PollInterval { get; set; } = TimeSpan.FromMilliseconds(50);
        public static TimeSpan PopupCooldown { get; set; } = TimeSpan.FromSeconds(5);

        // how long Dan stands frozen before he's moved
        public static TimeSpan FreezeTime { get; set; } = TimeSpan.FromMilliseconds(600);

        // where Dan enters Zarok's Lair (Addresses.DanPosition, read from a savestate taken on arrival)
        public static (short X, short Y, short Z) Entrance { get; set; } = (3, 0, 2268);

        // Writing only the s16 copies (Addresses.DanPosition / DanPositionCopy) doesn't move Dan: the game rebuilds
        // them every frame. His real position is three s32 16.16 fixed-point values (X, Y, Z) in an object on the
        // heap, which sits somewhere different each time the level loads. So when he's frozen, the heap is
        // searched for X, Y, Z triples matching his s16 position, and those are written too.
        private const uint HeapStart = 0x00180000;
        private const int HeapLength = 0x00070000;

        // how many ticks the entrance position is written for, so it sticks
        public static int HoldTicks { get; set; } = 4;

        // set while Dan is frozen, waiting to be moved
        private class PushBack
        {
            public DateTime Started;
            public List<uint> FixedPositions = new();
            public int TicksHeld;
            public byte[] Speed = Array.Empty<byte>();
            public byte X, Square, Start;
        }
        private static PushBack? _push;

        // after a push-back, don't freeze him again until he's been seen outside the circle (or this long has
        // passed), so if moving him ever fails he isn't stuck frozen on the spot
        public static TimeSpan RearmTime { get; set; } = TimeSpan.FromSeconds(3);
        private static bool _armed = true;
        private static DateTime _released = DateTime.MinValue;

        private static DateTime _lastPopup = DateTime.MinValue;
#if DEBUG
        private static DateTime _lastDebugPrint = DateTime.MinValue;
#endif

        public static int KnightsRequired(ArchipelagoClient client) =>
            Int32.Parse(client.Options?.GetValueOrDefault("knights_to_find", "0").ToString());

        public static int KnightsReceived(ArchipelagoClient client) =>
            client.CurrentSession.Items.AllItemsReceived.Count(item => item.ItemName == KnightItemName);

        public static Task Start(ArchipelagoClient client, CancellationToken token) => Task.Run(async () =>
        {
            while (!token.IsCancellationRequested)
            {
                try
                {
                    Check(client);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Zarok's Lair gate check failed: {ex.Message}");
                }
                await Task.Delay(PollInterval, token);
            }
        }, token);

        private static bool IsInGateArea(int x, int z)
        {
            long dx = x - GateX, dz = z - GateZ;
            return dx * dx + dz * dz <= (long)GateRadius * GateRadius;
        }

        private static void Check(ArchipelagoClient client)
        {
            if (!PlayerStateHandler.isInTheGame() || Memory.ReadByte(Addresses.CurrentLevel) != ZaroksLairId)
            {
                Release(); // left the level (or died, or quit) mid push-back: give control straight back
                return;
            }

            short x = Memory.ReadShort(Addresses.DanPositionX);
            short z = Memory.ReadShort(Addresses.DanPositionZ);

#if DEBUG
            if (DateTime.Now - _lastDebugPrint > TimeSpan.FromSeconds(1))
            {
                _lastDebugPrint = DateTime.Now;
                Console.WriteLine($"Zarok's Lair: Dan at X {x}, Y {Memory.ReadShort(Addresses.DanPositionY)}, Z {z}");
            }
#endif

            if (_push != null)
            {
                ContinuePushBack(_push);
                return;
            }

            if (!IsInGateArea(x, z))
            {
                _armed = true;
                return;
            }
            if (!_armed && DateTime.Now - _released < RearmTime) return;

            int required = KnightsRequired(client);
            int received = KnightsReceived(client);
            if (required <= 0 || received >= required) return;

            BeginPushBack(x, z);

            if (DateTime.Now - _lastPopup > PopupCooldown)
            {
                _lastPopup = DateTime.Now;
                PopupHandler.Show($"You need {required} Knights of Gallowmere. You have {received}.");
                Console.WriteLine($"Zarok's Lair: blocked, {received}/{required} Knights of Gallowmere.");
            }
        }

        private static void BeginPushBack(short x, short z)
        {
            _push = new PushBack
            {
                Started = DateTime.Now,
                FixedPositions = FindFixedPositions(x, Memory.ReadShort(Addresses.DanPositionY), z),
                Speed = Memory.ReadByteArray(Addresses.DanForwardSpeed, 2),
                X = Memory.ReadByte(Addresses.ButtonX),
                Square = Memory.ReadByte(Addresses.ButtonSquare),
                Start = Memory.ReadByte(Addresses.ButtonStart),
            };
#if DEBUG
            Console.WriteLine($"Zarok's Lair: Dan's 16.16 position found at " +
                $"{string.Join(", ", _push.FixedPositions.Select(a => $"0x{0x80000000 + a:X8}"))}");
#endif

            Memory.Write(Addresses.DanForwardSpeed, new byte[] { 0, 0 });
            Memory.WriteByte(Addresses.ButtonX, 0);
            Memory.WriteByte(Addresses.ButtonSquare, 0);
            Memory.WriteByte(Addresses.ButtonStart, 0);
        }

        private static void ContinuePushBack(PushBack push)
        {
            // frozen where he stopped (forward speed is 0)
            if (DateTime.Now - push.Started < FreezeTime) return;

            WritePosition(push, Entrance.X, Entrance.Y, Entrance.Z);
            if (++push.TicksHeld >= HoldTicks) Release();
        }

        // heap addresses of s32 16.16 X, Y, Z triples whose whole parts match Dan's s16 position (to within 1,
        // since the s16 copy can round the other way)
        private static List<uint> FindFixedPositions(short x, short y, short z)
        {
            var found = new List<uint>();
            byte[] heap = Memory.ReadByteArray(HeapStart, HeapLength);
            for (int o = 0; o + 12 <= heap.Length; o += 4)
            {
                if (Math.Abs((BitConverter.ToInt32(heap, o) >> 16) - x) > 1) continue;
                if (Math.Abs((BitConverter.ToInt32(heap, o + 4) >> 16) - y) > 1) continue;
                if (Math.Abs((BitConverter.ToInt32(heap, o + 8) >> 16) - z) > 1) continue;
                found.Add(HeapStart + (uint)o);
            }
            return found;
        }

        // writes Dan's position everywhere it's kept
        private static void WritePosition(PushBack push, short x, short y, short z)
        {
            Memory.Write(Addresses.DanPositionX, x);
            Memory.Write(Addresses.DanPositionY, y);
            Memory.Write(Addresses.DanPositionZ, z);
            Memory.Write(Addresses.DanPositionCopyX, x);
            Memory.Write(Addresses.DanPositionCopyY, (short)(y + 6)); // the copy's height sits 6 higher
            Memory.Write(Addresses.DanPositionCopyZ, z);

            byte[] fixedXyz = new byte[12];
            BitConverter.GetBytes(x << 16).CopyTo(fixedXyz, 0);
            BitConverter.GetBytes(y << 16).CopyTo(fixedXyz, 4);
            BitConverter.GetBytes(z << 16).CopyTo(fixedXyz, 8);
            foreach (uint address in push.FixedPositions)
                Memory.Write(address, fixedXyz);
        }

        // puts back the speed and buttons saved when the push-back started
        private static void Release()
        {
            if (_push is not { } push) return;
            _push = null;
            _armed = false;
            _released = DateTime.Now;

            Memory.Write(Addresses.DanForwardSpeed, push.Speed);
            Memory.WriteByte(Addresses.ButtonX, push.X);
            Memory.WriteByte(Addresses.ButtonSquare, push.Square);
            Memory.WriteByte(Addresses.ButtonStart, push.Start);
        }
    }
}
