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
        public static Dictionary<string, Tuple<int, uint>> GetLevelCompleteStatuses ()
        {
            return new Dictionary<string, Tuple<int, uint>>
            {
               {"Dan's Crypt Cleared", new Tuple<int, uint>(Memory.ReadInt(Addresses.DansCryptLevelStatus), Addresses.DansCryptLevelStatus)},
               {"The Graveyard Cleared", new Tuple<int, uint>(Memory.ReadInt(Addresses.TheGraveyardLevelStatus), Addresses.TheGraveyardLevelStatus)},
               {"Return to the Graveyard Cleared", new Tuple<int, uint>(Memory.ReadInt(Addresses.ReturnToTheGraveyardLevelStatus), Addresses.ReturnToTheGraveyardLevelStatus)},
               {"Cemetery Hill Cleared", new Tuple<int, uint>(Memory.ReadInt(Addresses.CemeteryHillLevelStatus), Addresses.CemeteryHillLevelStatus)},
               {"The Hilltop Mausoleum Cleared", new Tuple<int, uint>(Memory.ReadInt(Addresses.TheHilltopMausoleumLevelStatus), Addresses.TheHilltopMausoleumLevelStatus)},
               {"Scarecrow Fields Cleared", new Tuple<int, uint>(Memory.ReadInt(Addresses.ScarecrowFieldsLevelStatus), Addresses.ScarecrowFieldsLevelStatus)},
               {"Ant Hill Cleared", new Tuple<int, uint>(Memory.ReadInt(Addresses.AntHillLevelStatus), Addresses.AntHillLevelStatus)},
               {"The Crystal Caves Cleared", new Tuple<int, uint>(Memory.ReadInt(Addresses.TheCrystalCavesLevelStatus), Addresses.TheCrystalCavesLevelStatus)},
               {"The Lake Cleared", new Tuple<int, uint>(Memory.ReadInt(Addresses.TheLakeLevelStatus), Addresses.TheLakeLevelStatus)},
               {"Pumpkin Gorge Cleared", new Tuple<int, uint>(Memory.ReadInt(Addresses.PumpkinGorgeLevelStatus), Addresses.PumpkinGorgeLevelStatus)},
               {"Pumpkin Serpent Cleared", new Tuple<int, uint>(Memory.ReadInt(Addresses.PumpkinSerpentLevelStatus), Addresses.PumpkinSerpentLevelStatus)},
               {"Sleeping Village Cleared", new Tuple<int, uint>(Memory.ReadInt(Addresses.SleepingVillageLevelStatus), Addresses.SleepingVillageLevelStatus)},
               {"Pools of the Ancient Dead Cleared", new Tuple<int, uint>(Memory.ReadInt(Addresses.PoolsOfTheAncientDeadLevelStatus), Addresses.PoolsOfTheAncientDeadLevelStatus)},
               {"Asylum Grounds Cleared", new Tuple<int, uint>(Memory.ReadInt(Addresses.AsylumGroundsLevelStatus), Addresses.AsylumGroundsLevelStatus)},
               {"Inside the Asylum Cleared", new Tuple<int, uint>(Memory.ReadInt(Addresses.InsideTheAsylumLevelStatus), Addresses.InsideTheAsylumLevelStatus)},
               {"Enchanted Earth Cleared", new Tuple<int, uint>(Memory.ReadInt(Addresses.EnchantedEarthLevelStatus), Addresses.EnchantedEarthLevelStatus)},
               {"The Gallows Gauntlet Cleared", new Tuple<int, uint>(Memory.ReadInt(Addresses.TheGallowsGauntletLevelStatus), Addresses.TheGallowsGauntletLevelStatus)},
               {"The Haunted Ruins Cleared", new Tuple<int, uint>(Memory.ReadInt(Addresses.TheHauntedRuinsLevelStatus), Addresses.TheHauntedRuinsLevelStatus)},
               {"Ghost Ship Cleared", new Tuple<int, uint>(Memory.ReadInt(Addresses.GhostShipLevelStatus), Addresses.GhostShipLevelStatus)},
               {"The Entrance Hall Cleared", new Tuple<int, uint>(Memory.ReadInt(Addresses.TheEntranceHallLevelStatus), Addresses.TheEntranceHallLevelStatus)},
               {"The Time Device Cleared", new Tuple<int, uint>(Memory.ReadInt(Addresses.TheTimeDeviceLevelStatus), Addresses.TheTimeDeviceLevelStatus)},
               {"Zaroks Lair Cleared", new Tuple<int, uint>(Memory.ReadInt(Addresses.ZaroksLairLevelStatus), Addresses.ZaroksLairLevelStatus)},
            };
        }

        public static List<Location> BuildLocationList()
        {
            int baseId = 99251000;
            List<LevelData> levels = GetLevelData();
            List<Location> locations = new List<Location>();
            var levelDict = GetLevelCompleteStatuses();
            foreach (var level in levels) { 
                {
                    Location location = new Location()
                    {
                        Name = level.Name,
                        Address = levelDict[level.Name].Item2,
                        Id = baseId + level.LevelId,
                        CheckType = LocationCheckType.Byte,
                        CheckValue = level.Name == "Dan's Crypt Cleared" ? "1" : "17"
                    };
                    locations.Add(location);

                };
            }
            return locations;
        }
        internal static readonly Dictionary<string, uint> AmmoAddressDictionary = new()
        {
            ["Gold Coins"] = Addresses.CurrentGold,
            ["Health"] = Addresses.CurrentEnergy,
            ["Health Vial"] = Addresses.CurrentEnergy,
            ["Small Sword"] = Addresses.SmallSword,
            ["Magic Sword"] = Addresses.MagicSword,
            ["Hammer"] = Addresses.Hammer,
            ["Axe"] = Addresses.Axe,
            ["Good Lightning"] = Addresses.GoodLightning,
            ["Dragon Armour"] = Addresses.DragonArmour,
            ["Life Bottle"] = Addresses.CurrentLifePotions,
            ["Energy"] = Addresses.CurrentEnergy,
            ["Dagger"] = Addresses.DaggerAmmo,
            ["Dagger Ammo"] = Addresses.DaggerAmmo,
            ["BroadSword"] = Addresses.BroadswordCharge,
            ["BroadSword Charge"] = Addresses.BroadswordCharge,
            ["Club"] = Addresses.ClubCharge,
            ["Club Charge"] = Addresses.ClubCharge,
            ["Chicken Drumsticks"] = Addresses.ChickenDrumsticksAmmo,
            ["Chicken Drumsticks Ammo"] = Addresses.ChickenDrumsticksAmmo,
            ["Crossbow"] = Addresses.CrossbowAmmo,
            ["Crossbow Ammo"] = Addresses.CrossbowAmmo,
            ["Longbow"] = Addresses.LongbowAmmo,
            ["Longbow Ammo"] = Addresses.LongbowAmmo,
            ["Fire Longbow"] = Addresses.FireLongbowAmmo,
            ["Fire Longbow Ammo"] = Addresses.FireLongbowAmmo,
            ["Magic Longbow"] = Addresses.MagicLongbowAmmo,
            ["Magic Longbow Ammo"] = Addresses.MagicLongbowAmmo,
            ["Spear"] = Addresses.SpearAmmo,
            ["Spear Ammo"] = Addresses.SpearAmmo,
            ["Copper Shield"] = Addresses.CopperShieldAmmo,
            ["Copper Shield Ammo"] = Addresses.CopperShieldAmmo,
            ["Silver Shield"] = Addresses.SilverShieldAmmo,
            ["Silver Shield Ammo"] = Addresses.SilverShieldAmmo,
            ["Gold Shield"] = Addresses.GoldShieldAmmo,
            ["Gold Shield Ammo"] = Addresses.GoldShieldAmmo,
            ["Lightning"] = Addresses.LightningCharge,
            ["Lightning Charge"] = Addresses.LightningCharge


        };


        private static List<LevelData> GetLevelData()
        {
            List<LevelData> levels = new List<LevelData>()
            {
                new LevelData("Dan's Crypt Cleared", 1, [], []),
                new LevelData("The Graveyard Cleared", 2, [], []),
                new LevelData("Return to the Graveyard Cleared",3, [], []),
                new LevelData("Cemetery Hill Cleared",4, [], []),
                new LevelData("The Hilltop Mausoleum Cleared",5, [], []),
                new LevelData("Scarecrow Fields Cleared",6, [], []),
                new LevelData("Ant Hill Cleared",7, [], []),
                new LevelData("The Crystal Caves Cleared",8, [], []),
                new LevelData("The Lake Cleared",9, [], []),
                new LevelData("Pumpkin Gorge Cleared",10, [], []),
                new LevelData("Pumpkin Serpent Cleared",11, [], []),
                new LevelData("Sleeping Village Cleared",12, [], []),
                new LevelData("Pools of the Ancient Dead Cleared",13, [], []),
                new LevelData("Asylum Grounds Cleared",14, [], []),
                new LevelData("Inside the Asylum Cleared",15, [], []),
                new LevelData("Enchanted Earth Cleared",16, [], []),
                new LevelData("The Gallows Gauntlet Cleared" ,17, [], []),
                new LevelData("The Haunted Ruins Cleared",18, [], []),
                new LevelData("Ghost Ship Cleared",19, [], []),
                new LevelData("The Entrance Hall Cleared",20, [], []),
                new LevelData("The Time Device Cleared",21, [], []),
                new LevelData("Zaroks Lair Cleared",22, [], []) // not sure if this is considered a level as it's the clear condition. Putting it here for now
            };
            return levels;
        }
    }
}