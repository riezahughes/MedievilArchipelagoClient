using Archipelago.Core.Models;
using Archipelago.Core.Util;
using MedievilArchipelago;
using MedievilArchipelago.Models;
using Newtonsoft.Json;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Location = Archipelago.Core.Models.Location;

namespace MedievilArchipelago
{
    public class Helpers
    {
        public static string OpenEmbeddedResource(string resourceName)
        {
            var assembly = Assembly.GetExecutingAssembly();
            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            using (StreamReader reader = new StreamReader(stream))
            {
                string jsonFile = reader.ReadToEnd();
                return jsonFile;
            }
        }
        public static ulong GetDuckstationOffset()
        {
            var baseAddress = Memory.GetBaseAddress("duckstation-qt-x64-ReleaseLTCG");
            var offset = Memory.ReadULong(baseAddress + 0x008C4FA8);
            return offset;
        }

        public static Dictionary<string, Tuple<int, uint>> GetLevelCompleteStatuses ()
        {
            return new Dictionary<string, Tuple<int, uint>>
            {
               {"Dans Crypt Completed", new Tuple<int, uint>(Memory.ReadInt(Addresses.DansCryptLevelStatus), Addresses.DansCryptLevelStatus)},
               {"The Graveyard Completed", new Tuple<int, uint>(Memory.ReadInt(Addresses.TheGraveyardLevelStatus), Addresses.TheGraveyardLevelStatus)},
               {"Return to the Graveyard Completed", new Tuple<int, uint>(Memory.ReadInt(Addresses.ReturnToTheGraveyardLevelStatus), Addresses.ReturnToTheGraveyardLevelStatus)},
               {"Cemetery Hill Completed", new Tuple<int, uint>(Memory.ReadInt(Addresses.CemeteryHillLevelStatus), Addresses.CemeteryHillLevelStatus)},
               {"The Hilltop Mausoleum Completed", new Tuple<int, uint>(Memory.ReadInt(Addresses.TheHilltopMausoleumLevelStatus), Addresses.TheHilltopMausoleumLevelStatus)},
               {"Scarecrow Fields Completed", new Tuple<int, uint>(Memory.ReadInt(Addresses.ScarecrowFieldsLevelStatus), Addresses.ScarecrowFieldsLevelStatus)},
               {"Ant Hill Completed", new Tuple<int, uint>(Memory.ReadInt(Addresses.AntHillLevelStatus), Addresses.AntHillLevelStatus)},
               {"The Crystal Caves Completed", new Tuple<int, uint>(Memory.ReadInt(Addresses.TheCrystalCavesLevelStatus), Addresses.TheCrystalCavesLevelStatus)},
               {"The Lake Completed", new Tuple<int, uint>(Memory.ReadInt(Addresses.TheLakeLevelStatus), Addresses.TheLakeLevelStatus)},
               {"Pumpkin Gorge Completed", new Tuple<int, uint>(Memory.ReadInt(Addresses.PumpkinGorgeLevelStatus), Addresses.PumpkinGorgeLevelStatus)},
               {"Pumpkin Serpent Completed", new Tuple<int, uint>(Memory.ReadInt(Addresses.PumpkinSerpentLevelStatus), Addresses.PumpkinSerpentLevelStatus)},
               {"Sleeping Village Completed", new Tuple<int, uint>(Memory.ReadInt(Addresses.SleepingVillageLevelStatus), Addresses.SleepingVillageLevelStatus)},
               {"Pools of the Ancient Dead Completed", new Tuple<int, uint>(Memory.ReadInt(Addresses.PoolsOfTheAncientDeadLevelStatus), Addresses.PoolsOfTheAncientDeadLevelStatus)},
               {"Asylum Grounds Completed", new Tuple<int, uint>(Memory.ReadInt(Addresses.AsylumGroundsLevelStatus), Addresses.AsylumGroundsLevelStatus)},
               {"Inside the Asylum Completed", new Tuple<int, uint>(Memory.ReadInt(Addresses.InsideTheAsylumLevelStatus), Addresses.InsideTheAsylumLevelStatus)},
               {"Enchanted Earth Completed", new Tuple<int, uint>(Memory.ReadInt(Addresses.EnchantedEarthLevelStatus), Addresses.EnchantedEarthLevelStatus)},
               {"The Gallows Gauntlet Completed", new Tuple<int, uint>(Memory.ReadInt(Addresses.TheGallowsGauntletLevelStatus), Addresses.TheGallowsGauntletLevelStatus)},
               {"The Haunted Ruins Completed", new Tuple<int, uint>(Memory.ReadInt(Addresses.TheHauntedRuinsLevelStatus), Addresses.TheHauntedRuinsLevelStatus)},
               {"Ghost Ship Completed", new Tuple<int, uint>(Memory.ReadInt(Addresses.GhostShipLevelStatus), Addresses.GhostShipLevelStatus)},
               {"The Entrance Hall Completed", new Tuple<int, uint>(Memory.ReadInt(Addresses.TheEntranceHallLevelStatus), Addresses.TheEntranceHallLevelStatus)},
               {"The Time Device Completed", new Tuple<int, uint>(Memory.ReadInt(Addresses.TheTimeDeviceLevelStatus), Addresses.TheTimeDeviceLevelStatus)},
               {"Zaroks Lair Completed", new Tuple<int, uint>(Memory.ReadInt(Addresses.ZaroksLairLevelStatus), Addresses.ZaroksLairLevelStatus)},
            };
        }

        public static List<Location> BuildLocationList()
        {
            int baseId = 1230000;
            int levelOffset = 1000;
            List<LevelData> levels = GetLevelData();
            List<Location> locations = new List<Location>();
            var gemDict = GetLevelCompleteStatuses();
            foreach (var level in levels) { 
                {
                    Location location = new Location()
                    {
                        Name = level.Name,
                        AddressBit = 0x00,
                        Address = level.Address,
                        Category = "Level_End",
                        Id = baseId + (levelOffset * 0),
                        CheckType = LocationCheckType.Bit,
                        CheckValue = "0x11",
                        CompareType = LocationCheckCompareType.Match,
                    };
                    locations.Add(location);

                };
            }
            return locations;
        }

        private static List<LevelData> GetLevelData()
        {
            List<LevelData> levels = new List<LevelData>()
            {
                new LevelData("The Graveyard", 0x000f81e1, [], []),
            };
            return levels;
        }
    }
}