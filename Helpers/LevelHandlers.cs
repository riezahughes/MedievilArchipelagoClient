using Archipelago.Core.Models;
using Archipelago.Core;
using Archipelago.Core.Util;

namespace MedievilArchipelago.Helpers
{
    internal class LevelHandlers
    {
        public static void CheckPositionalLocations(ArchipelagoClient client, List<ILocation> builtLocations)
        {

            //

            // A large pile of custom pieces are here. These are mostly things that are in dynamic memory/are hard to work out. 
            // By doing it this way, it allows us to create a plane and custom logic from it, which, i'm really fucking greatful for.
            // Bit thanks to Arson for the GPS btw. Holy shit. What a godsend.

            //
            if (builtLocations?.Count == null)
            {
                return;
            }

            int gargoyleSanity = int.Parse(client.Options?.GetValueOrDefault("gargoylesanity", "0").ToString());

            // starting gargoyles
            if (client.GPSHandler.MapId == 6 && client.GPSHandler.X >= 64189 && client.GPSHandler.X >= 60484 && client.GPSHandler.Y == 0 && client.GPSHandler.Z >= 94 && client.GPSHandler.Z >= 65436 && gargoyleSanity == 1) {
                var location1 = builtLocations.FirstOrDefault(loc => loc.Name == "Gargoyle: Left - DC");
                var location2 = builtLocations.FirstOrDefault(loc => loc.Name == "Gargoyle: Right - DC");
                if (location1 != null && location2 != null)
                {
                    {
                        client.SendLocation(location1);
                        client.SendLocation(location2);
                    }
                }
            }

            // HoH Gargoyle

            if (client.GPSHandler.MapId == 18 && client.GPSHandler.Y == 0 && client.GPSHandler.Z >= 65166 && client.GPSHandler.Z <= 65465 && gargoyleSanity == 1)
            {
                var location = builtLocations.FirstOrDefault(loc => loc.Name == "Gargoyle: Entrance - HH");
                if (location != null)
                {
                    client.SendLocation(location);
                }

            }

            // RtG Gargoyle + Daring Dash
            if (client.GPSHandler.MapId == 2 && client.GPSHandler.X >= 19333 && client.GPSHandler.X <= 19455 && client.GPSHandler.Y >= 65057 && client.GPSHandler.Y <= 65072 && client.GPSHandler.Z >= 5925 && client.GPSHandler.Z <= 6667)
            {
                var location1 = builtLocations.FirstOrDefault(loc => loc.Name == "Skill: Daring Dash");
                if (location1 != null)
                {
                    client.SendLocation(location1);
                }

                var location2 = builtLocations.FirstOrDefault(loc => loc.Name == "Gargoyle: Exit - RTG");


                if (location2 != null && gargoyleSanity == 1)
                {
                    client.SendLocation(location2);
                }

            }

            // EE Level Complete

            if (client.GPSHandler.MapId == 15 && client.GPSHandler.X >= 61307 && client.GPSHandler.X <= 61632 && client.GPSHandler.Y >= 64576 && client.GPSHandler.Y <= 64586 && client.GPSHandler.Z >= 1784 && client.GPSHandler.Z <= 2235)
            {
                var location = builtLocations.FirstOrDefault(loc => loc.Name == "Cleared: Enchanted Earth");
                if (location != null)
                {
                    client.SendLocation(location);
                }

            }

            // Anthill Checks Complete

            if (client.GPSHandler.MapId == 7 && client.GPSHandler.X >= 38238 && client.GPSHandler.X <= 38366 && client.GPSHandler.Y >= 955 && client.GPSHandler.Y <= 975 && client.GPSHandler.Z >= 8374 && client.GPSHandler.Z <= 8630)
            {
                var amber = Memory.ReadByte(Addresses.AmberPiece);
                var fairies = Memory.ReadByte(Addresses.FairyCount);



                if (amber >= 7)
                {
                    var location = builtLocations.FirstOrDefault(loc => loc.Name == "Equipment: Chicken Drumsticks - TA");
                    if (location != null)
                    {
                        client.SendLocation(location);
                    }
                }

                else if(fairies == 6)
                {
                    var location = builtLocations.FirstOrDefault(loc => loc.Name == "Chalice: Ant Hill");
                    if (location != null)
                    {
                        client.SendLocation(location);
                    }
                }

                var completeLocation = builtLocations.FirstOrDefault(loc => loc.Name == "Cleared: Ant Hill");
                if (completeLocation != null)
                {
                    client.SendLocation(completeLocation);
                }

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
