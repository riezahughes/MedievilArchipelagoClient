using Archipelago.Core;
using Archipelago.Core.Models;
using Archipelago.Core.Util;

namespace MedievilArchipelago.Helpers
{
    // Dan's Crypt has two memory layouts. Every run starts in it with the new-game layout; on every visit after you've
    // left it (cleared or not) its pickups and chest sit 0xC38 lower (see Addresses, "Dans Crypt").
    // Once the level's "been on the map" bit is set, swap those locations for their after-leaving versions for good.
    internal static class DansCryptLayoutHandler
    {
        public static bool Returned { get; private set; }

        // Call before building the starting location list, so a save that has already left Dan's Crypt starts on the right addresses
        public static bool CheckAlreadyLeft()
        {
            Returned = PlayerStateHandler.isInTheGame() && (Memory.ReadByte(Addresses.DC_LevelStatus) & Addresses.DC_LeftLevelBit) != 0;
            return Returned;
        }

        public static void Start(ArchipelagoClient client)
        {
            if (Returned) return;
            Memory.MonitorAddressForAction<byte>(
                Addresses.DC_LevelStatus,
                () => SwapToReturnLayout(client),
                value => (value & Addresses.DC_LeftLevelBit) != 0 && PlayerStateHandler.isInTheGame());
        }

        private static void SwapToReturnLayout(ArchipelagoClient client)
        {
            if (Returned) return;
            Returned = true;

            var firstVisit = LocationHandlers.BuildLocationList(client.Options).ToDictionary(l => l.Id);
            var checkedIds = new HashSet<long>(client.CurrentSession.Locations.AllLocationsChecked);
            int swapped = 0;

            foreach (var returned in LocationHandlers.BuildLocationList(client.Options, dansCryptReturned: true))
            {
                if (!firstVisit.TryGetValue(returned.Id, out var original) || SameAddresses(original, returned)) continue;

                // RemoveLocation/AddLocation match by Id, and AddLocation skips an Id that's still monitored
                client.LocationManager.RemoveLocation(original);
                if (!checkedIds.Contains(returned.Id)) client.LocationManager.AddLocation(returned);
                swapped++;
            }

            Console.WriteLine($"Dan's Crypt: switched {swapped} locations to the layout used after leaving it.");
        }

        private static bool SameAddresses(ILocation a, ILocation b) =>
            a is CompositeLocation ca && b is CompositeLocation cb
                ? ca.Conditions.Select(Address).SequenceEqual(cb.Conditions.Select(Address))
                : Address(a) == Address(b);

        private static ulong Address(ILocation location) => location is Location l ? l.Address : 0;
    }
}
