using Archipelago.Core;
using Archipelago.Core.Models;

namespace MedievilArchipelago.Helpers
{
    // KeySanity: each level (bar Dan's Crypt, the Ant Hill and Zarok's Lair) has a key in the item pool.
    // The apworld's keysanity_naming option picks one of two names for every key, so both are listed here.
    // Level IDs are the game's CurrentLevel values (see LevelHandlers.GetLevelNameFromId).
    internal static class LevelKeyHandler
    {
        private static readonly Dictionary<string, byte> LevelKeys = new Dictionary<string, byte>
        {
            // keysanity_naming: level_name
            ["The Graveyard Unlock"] = 0x01,
            ["Return to the Graveyard Unlock"] = 0x02,
            ["Cemetery Hill Unlock"] = 0x03,
            ["The Hilltop Mausoleum Unlock"] = 0x04,
            ["Scarecrow Fields Unlock"] = 0x05,
            ["The Crystal Caves Unlock"] = 0x08,
            ["Pumpkin Gorge Unlock"] = 0x09,
            ["Pumpkin Serpent Unlock"] = 0x0A,
            ["The Sleeping Village Unlock"] = 0x0B,
            ["Pools of the Ancient Dead Unlock"] = 0x0C,
            ["Asylum Grounds Unlock"] = 0x0D,
            ["Inside the Asylum Unlock"] = 0x0E,
            ["Enchanted Earth Unlock"] = 0x0F,
            ["The Gallows Gauntlet Unlock"] = 0x10,
            ["The Haunted Ruins Unlock"] = 0x11,
            ["The Ghost Ship Unlock"] = 0x13,
            ["The Entrance Hall Unlock"] = 0x14,
            ["The Time Device Unlock"] = 0x15,
            ["The Lake Unlock"] = 0x16,

            // keysanity_naming: unique_name
            ["Angelic Call"] = 0x01,
            ["Moldy Dog Food"] = 0x02,
            ["Hillwalking Boots"] = 0x03,
            ["Musical Staff"] = 0x04,
            ["Farm Deed"] = 0x05,
            ["Ancient Horn"] = 0x08,
            ["Smashed Pumpkin"] = 0x09,
            ["Witches Request"] = 0x0A,
            ["Key to the City"] = 0x0B,
            ["Soul Detector"] = 0x0C,
            ["Jack's Invitation"] = 0x0D,
            ["Fight Club"] = 0x0E,
            ["Demonic Amulet"] = 0x0F,
            ["Serpent Effigy"] = 0x10,
            ["Royal Decree"] = 0x11,
            ["Stowaway’s Spirit"] = 0x13, // curly apostrophe, matching the apworld's item name
            ["Ancient Knocker"] = 0x14,
            ["Alarm Clock"] = 0x15,
            ["Boating Licence"] = 0x16,
        };

        public static bool IsLevelKey(string itemName) => itemName != null && LevelKeys.ContainsKey(itemName);

        public static byte? GetLevelIdForKey(string itemName) =>
            itemName != null && LevelKeys.TryGetValue(itemName, out var levelId) ? levelId : null;

        // Keys aren't written to game memory: the received item list is the record of which levels are open.
        public static void ReceiveLevelKey(Item item)
        {
            var levelId = GetLevelIdForKey(item.Name);
            if (levelId == null) return;
            Console.WriteLine($"Level key received: {item.Name} ({LevelHandlers.GetLevelNameFromId(levelId.Value)})");
        }

        // True when KeySanity is off, the level has no key, or the key for it has been received.
        public static bool HasLevelKey(ArchipelagoClient client, byte levelId)
        {
            int keySanityOption = Int32.Parse(client.Options?.GetValueOrDefault("keysanity", "0").ToString());
            if (keySanityOption != 1) return true;
            if (!LevelKeys.ContainsValue(levelId)) return true;

            return client.CurrentSession.Items.AllItemsReceived
                .Any(item => GetLevelIdForKey(item.ItemName) == levelId);
        }
    }
}
