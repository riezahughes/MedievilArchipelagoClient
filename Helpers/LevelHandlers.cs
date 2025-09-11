using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Archipelago.Core.Models;
using Archipelago.Core;

namespace MedievilArchipelago.Helpers
{
    internal class LevelHandlers
    {
        public static void CheckPositionalLocations(ArchipelagoClient client, List<ILocation> builtLocations)
        {
            if (builtLocations?.Count == null)
            {
                return;
            }


            // Crystal 1
            if (client.GPSHandler.MapId == 8 && client.GPSHandler.X >= 1186 && client.GPSHandler.X <= 1550 && client.GPSHandler.Y >= 65449 && client.GPSHandler.Y <= 65465 && client.GPSHandler.Z >= 63075 && client.GPSHandler.Z <= 63351)
            {
                var location = builtLocations.FirstOrDefault(loc => loc.Name == "Gold Coins: Bag in Crystal at Start - CC");
                if (location != null)
                {
                    {
                        client.SendLocation(location);
                    }
                }
            }

            // Crystal 2
            if (client.GPSHandler.MapId == 8 && client.GPSHandler.X >= 60389 && client.GPSHandler.X <= 60768 && client.GPSHandler.Y >= 65127 && client.GPSHandler.Y <= 65150 && client.GPSHandler.Z >= 165 && client.GPSHandler.Z <= 301)
            {
                var location = builtLocations.FirstOrDefault(loc => loc.Name == "Equipment: Silver Shield in Crystal - CC");
                if (location != null)
                {
                    {
                        client.SendLocation(location);
                    }
                }
            }

            // Crystal 3
            if (client.GPSHandler.MapId == 8 && client.GPSHandler.X >= 59734 && client.GPSHandler.X <= 60010 && client.GPSHandler.Y >= 64629 && client.GPSHandler.Y <= 64643 && client.GPSHandler.Z >= 7163 && client.GPSHandler.Z <= 7516)
            {
                var location = builtLocations.FirstOrDefault(loc => loc.Name == "Gold Coins: Chest in Crystal After Earth Door - CC");

                if (location != null)
                {
                    {
                        client.SendLocation(location);
                    }
                }
            }

            // Crystal 4
            if (client.GPSHandler.MapId == 8 && client.GPSHandler.X >= 373 && client.GPSHandler.X <= 639 && client.GPSHandler.Y >= 64288 && client.GPSHandler.Y <= 64291 && client.GPSHandler.Z >= 3149 && client.GPSHandler.Z <= 3295)
            {
                var location = builtLocations.FirstOrDefault(loc => loc.Name == "Gold Coins: Chest in Crystal after Pool - CC");
                if (location != null)
                {
                    {
                        client.SendLocation(location);
                    }
                }
            }


            // Zarok Chest 1
            if (client.GPSHandler.MapId == 23 && client.GPSHandler.X >= 64639 && client.GPSHandler.X <= 64693 && client.GPSHandler.Y >= 9 && client.GPSHandler.Y <= 13 && client.GPSHandler.Z >= 2820 && client.GPSHandler.Z <= 2939)
            {
                var location = builtLocations.FirstOrDefault(loc => loc.Name == "Equipment: Good Lightning - ZL");
                if (location != null)
                {
                    {
                        client.SendLocation(location);
                    }
                }
            }

            // Zarok Chest 2
            if (client.GPSHandler.MapId == 23 && client.GPSHandler.X >= 768 && client.GPSHandler.X <= 999 && client.GPSHandler.Y >= 9 && client.GPSHandler.Y <= 13 && client.GPSHandler.Z >= 2830 && client.GPSHandler.Z <= 2965)
            {
                var location = builtLocations.FirstOrDefault(loc => loc.Name == "Equipment: Silver Shield Arena - ZL");
                if (location != null)
                {
                    {
                        client.SendLocation(location);
                    }
                }
            }
        }
        public static string GetLevelNameFromId(byte levelId)
        {
            var dict = new Dictionary<byte, string>
            {
                [0x00] = "None/Out of Game",
                [0x01] = "The Graveyard",
                [0x02] = "Return to the Graveyard",
                [0x03] = "Cemetery Hill",
                [0x04] = "The Hilltop Mausoleum",
                [0x05] = "Scarecrow Fields",
                [0x06] = "Dan's Crypt",
                [0x07] = "Ant Hill",
                [0x08] = "The Crystal Caves",
                [0x09] = "Pumpkin Gorge",
                [0x0A] = "The Pumpkin Serpent",
                [0x0B] = "The Sleeping Village",
                [0x0C] = "Pools of the Ancient Dead",
                [0x0D] = "The Asylum Grounds",
                [0x0E] = "Inside the Asylum",
                [0x0F] = "Enchanted Earth",
                [0x10] = "The Gallows Gauntlet",
                [0x11] = "The Haunted Ruins",
                [0x12] = "Hall of Heroes",
                [0x13] = "Ghost Ship",
                [0x14] = "The Entrance Hall",
                [0x15] = "The Time Device",
                [0x16] = "The Lake",
                [0x17] = "Zarok's Lair",
            };

            return dict[levelId];
        }

        public static string GetLevelNameFromMapId(byte levelId)
        {
            var dict = new Dictionary<byte, string>
            {
                [0] = "Dan's Crypt",
                [1] = "The Graveyard",
                [2] = "Return to the Graveyard",
                [3] = "Cemetery Hill",
                [4] = "The Hilltop Mausoleum",
                [5] = "Scarecrow Fields",
                [6] = "Enchanted Earth",
                [7] = "Pumpkin Gorge",
                [8] = "The Pumpkin Serpent",
                [9] = "The Sleeping Village",
                [10] = "The Asylum Grounds",
                [11] = "Inside the Asylum",
                [12] = "Pools of the Ancient Dead",
                [13] = "The Lake",
                [14] = "The Crystal Caves",
                [15] = "The Haunted Ruins",
                [16] = "Ghost Ship",
                [17] = "The Gallows Gauntlet",
                [18] = "The Entrance Hall",
                [19] = "The Time Device",
                [20] = "Zarok's Lair"
            };

            return dict[levelId];
        }
    }
}
