using Archipelago.Core.Models;
using Archipelago.Core.Util;
using MedievilArchipelago;
using MedievilArchipelago.Models;
using Newtonsoft.Json;
using Serilog;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.InteropServices.JavaScript;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Location = Archipelago.Core.Models.Location;

namespace MedievilArchipelago
{
    public class Helpers
    {
        public static Dictionary<string, Tuple<int, uint>> GetLevelCompleteStatuses ()
        {
            return new Dictionary<string, Tuple<int, uint>>
            {
               {"Dan's Crypt: Cleared", new Tuple<int, uint>(Memory.ReadInt(Addresses.DansCryptLevelStatus), Addresses.DansCryptLevelStatus)},
               {"The Graveyard: Cleared", new Tuple<int, uint>(Memory.ReadInt(Addresses.TheGraveyardLevelStatus), Addresses.TheGraveyardLevelStatus)},
               {"Return to the Graveyard: Cleared", new Tuple<int, uint>(Memory.ReadInt(Addresses.ReturnToTheGraveyardLevelStatus), Addresses.ReturnToTheGraveyardLevelStatus)},
               {"Cemetery Hill: Cleared", new Tuple<int, uint>(Memory.ReadInt(Addresses.CemeteryHillLevelStatus), Addresses.CemeteryHillLevelStatus)},
               {"The Hilltop Mausoleum: Cleared", new Tuple<int, uint>(Memory.ReadInt(Addresses.TheHilltopMausoleumLevelStatus), Addresses.TheHilltopMausoleumLevelStatus)},
               {"Scarecrow Fields: Cleared", new Tuple<int, uint>(Memory.ReadInt(Addresses.ScarecrowFieldsLevelStatus), Addresses.ScarecrowFieldsLevelStatus)},
               {"Ant Hill: Cleared", new Tuple<int, uint>(Memory.ReadInt(Addresses.AntHillLevelStatus), Addresses.AntHillLevelStatus)},
               {"The Crystal Caves: Cleared", new Tuple<int, uint>(Memory.ReadInt(Addresses.TheCrystalCavesLevelStatus), Addresses.TheCrystalCavesLevelStatus)},
               {"The Lake: Cleared", new Tuple<int, uint>(Memory.ReadInt(Addresses.TheLakeLevelStatus), Addresses.TheLakeLevelStatus)},
               {"Pumpkin Gorge: Cleared", new Tuple<int, uint>(Memory.ReadInt(Addresses.PumpkinGorgeLevelStatus), Addresses.PumpkinGorgeLevelStatus)},
               {"Pumpkin Serpent: Cleared", new Tuple<int, uint>(Memory.ReadInt(Addresses.PumpkinSerpentLevelStatus), Addresses.PumpkinSerpentLevelStatus)},
               {"Sleeping Village: Cleared", new Tuple<int, uint>(Memory.ReadInt(Addresses.SleepingVillageLevelStatus), Addresses.SleepingVillageLevelStatus)},
               {"Pools of the Ancient Dead: Cleared", new Tuple<int, uint>(Memory.ReadInt(Addresses.PoolsOfTheAncientDeadLevelStatus), Addresses.PoolsOfTheAncientDeadLevelStatus)},
               {"Asylum Grounds: Cleared", new Tuple<int, uint>(Memory.ReadInt(Addresses.AsylumGroundsLevelStatus), Addresses.AsylumGroundsLevelStatus)},
               {"Inside the Asylum: Cleared", new Tuple<int, uint>(Memory.ReadInt(Addresses.InsideTheAsylumLevelStatus), Addresses.InsideTheAsylumLevelStatus)},
               {"Enchanted Earth: Cleared", new Tuple<int, uint>(Memory.ReadInt(Addresses.EnchantedEarthLevelStatus), Addresses.EnchantedEarthLevelStatus)},
               {"The Gallows Gauntlet: Cleared", new Tuple<int, uint>(Memory.ReadInt(Addresses.TheGallowsGauntletLevelStatus), Addresses.TheGallowsGauntletLevelStatus)},
               {"The Haunted Ruins: Cleared", new Tuple<int, uint>(Memory.ReadInt(Addresses.TheHauntedRuinsLevelStatus), Addresses.TheHauntedRuinsLevelStatus)},
               {"Ghost Ship: Cleared", new Tuple<int, uint>(Memory.ReadInt(Addresses.GhostShipLevelStatus), Addresses.GhostShipLevelStatus)},
               {"The Entrance Hall: Cleared", new Tuple<int, uint>(Memory.ReadInt(Addresses.TheEntranceHallLevelStatus), Addresses.TheEntranceHallLevelStatus)},
               {"The Time Device: Cleared", new Tuple<int, uint>(Memory.ReadInt(Addresses.TheTimeDeviceLevelStatus), Addresses.TheTimeDeviceLevelStatus)},
               {"Zaroks Lair: Cleared", new Tuple<int, uint>(Memory.ReadInt(Addresses.ZaroksLairLevelStatus), Addresses.ZaroksLairLevelStatus)},
               {"The Graveyard: Chalice", new Tuple<int, uint>(Memory.ReadInt(Addresses.TheGraveyardLevelStatus), Addresses.TheGraveyardLevelStatus)},
               {"Return to the Graveyard: Chalice", new Tuple<int, uint>(Memory.ReadInt(Addresses.ReturnToTheGraveyardLevelStatus), Addresses.ReturnToTheGraveyardLevelStatus)},
               {"Cemetery Hill: Chalice", new Tuple<int, uint>(Memory.ReadInt(Addresses.CemeteryHillLevelStatus), Addresses.CemeteryHillLevelStatus)},
               {"The Hilltop Mausoleum: Chalice", new Tuple<int, uint>(Memory.ReadInt(Addresses.TheHilltopMausoleumLevelStatus), Addresses.TheHilltopMausoleumLevelStatus)},
               {"Scarecrow Fields: Chalice", new Tuple<int, uint>(Memory.ReadInt(Addresses.ScarecrowFieldsLevelStatus), Addresses.ScarecrowFieldsLevelStatus)},
               {"Ant Hill: Chalice", new Tuple<int, uint>(Memory.ReadInt(Addresses.AntHillLevelStatus), Addresses.AntHillLevelStatus)},
               {"The Crystal Caves: Chalice", new Tuple<int, uint>(Memory.ReadInt(Addresses.TheCrystalCavesLevelStatus), Addresses.TheCrystalCavesLevelStatus)},
               {"The Lake: Chalice", new Tuple<int, uint>(Memory.ReadInt(Addresses.TheLakeLevelStatus), Addresses.TheLakeLevelStatus)},
               {"Pumpkin Gorge: Chalice", new Tuple<int, uint>(Memory.ReadInt(Addresses.PumpkinGorgeLevelStatus), Addresses.PumpkinGorgeLevelStatus)},
               {"Pumpkin Serpent: Chalice", new Tuple<int, uint>(Memory.ReadInt(Addresses.PumpkinSerpentLevelStatus), Addresses.PumpkinSerpentLevelStatus)},
               {"Sleeping Village: Chalice", new Tuple<int, uint>(Memory.ReadInt(Addresses.SleepingVillageLevelStatus), Addresses.SleepingVillageLevelStatus)},
               {"Pools of the Ancient Dead: Chalice", new Tuple<int, uint>(Memory.ReadInt(Addresses.PoolsOfTheAncientDeadLevelStatus), Addresses.PoolsOfTheAncientDeadLevelStatus)},
               {"Asylum Grounds: Chalice", new Tuple<int, uint>(Memory.ReadInt(Addresses.AsylumGroundsLevelStatus), Addresses.AsylumGroundsLevelStatus)},
               {"Inside the Asylum: Chalice", new Tuple<int, uint>(Memory.ReadInt(Addresses.InsideTheAsylumLevelStatus), Addresses.InsideTheAsylumLevelStatus)},
               {"Enchanted Earth: Chalice", new Tuple<int, uint>(Memory.ReadInt(Addresses.EnchantedEarthLevelStatus), Addresses.EnchantedEarthLevelStatus)},
               {"The Gallows Gauntlet: Chalice", new Tuple<int, uint>(Memory.ReadInt(Addresses.TheGallowsGauntletLevelStatus), Addresses.TheGallowsGauntletLevelStatus)},
               {"The Haunted Ruins: Chalice", new Tuple<int, uint>(Memory.ReadInt(Addresses.TheHauntedRuinsLevelStatus), Addresses.TheHauntedRuinsLevelStatus)},
               {"Ghost Ship: Chalice", new Tuple<int, uint>(Memory.ReadInt(Addresses.GhostShipLevelStatus), Addresses.GhostShipLevelStatus)},
               {"The Entrance Hall: Chalice", new Tuple<int, uint>(Memory.ReadInt(Addresses.TheEntranceHallLevelStatus), Addresses.TheEntranceHallLevelStatus)},
               {"The Time Device: Chalice", new Tuple<int, uint>(Memory.ReadInt(Addresses.TheTimeDeviceLevelStatus), Addresses.TheTimeDeviceLevelStatus)},
            };
        }

        public static Dictionary<string, Tuple<int, uint>> GetHallOfHeroesStatuses()
        {
            return new Dictionary<string, Tuple<int, uint>>
            {
                {"Canny Tim 1: Hall Of Heroes",new Tuple<int, uint>(Memory.ReadInt(Addresses.CannyTim1), Addresses.CannyTim1)},
                {"Canny Tim 2: Hall Of Heroes",new Tuple<int, uint>(Memory.ReadInt(Addresses.CannyTim2), Addresses.CannyTim2)},
                {"Stanyer Iron Hewer 1: Hall Of Heroes",new Tuple<int, uint>(Memory.ReadInt(Addresses.StanyerIronHewer1), Addresses.StanyerIronHewer1)},
                {"Stanyer Iron Hewer 2: Hall Of Heroes",new Tuple<int, uint>(Memory.ReadInt(Addresses.StanyerIronHewer2), Addresses.StanyerIronHewer2)},
                {"Woden The Mighty 1: Hall Of Heroes",new Tuple<int, uint>(Memory.ReadInt(Addresses.WodenTheMighty1), Addresses.WodenTheMighty1)},
                {"Woden The Mighty 2: Hall Of Heroes",new Tuple<int, uint>(Memory.ReadInt(Addresses.WodenTheMighty2), Addresses.WodenTheMighty2)},
                {"RavenHooves The Archer 1: Hall Of Heroes",new Tuple<int, uint>(Memory.ReadInt(Addresses.RavenHoovesTheArcher1), Addresses.RavenHoovesTheArcher1)},
                {"RavenHooves The Archer 2: Hall Of Heroes",new Tuple<int, uint>(Memory.ReadInt(Addresses.RavenHoovesTheArcher2), Addresses.RavenHoovesTheArcher2)},
                {"RavenHooves The Archer 3: Hall Of Heroes",new Tuple<int, uint>(Memory.ReadInt(Addresses.RavenHoovesTheArcher3), Addresses.RavenHoovesTheArcher3)},
                {"RavenHooves The Archer 4: Hall Of Heroes",new Tuple<int, uint>(Memory.ReadInt(Addresses.RavenHoovesTheArcher4), Addresses.RavenHoovesTheArcher4)},
                {"Imanzi 1: Hall Of Heroes",new Tuple<int, uint>(Memory.ReadInt(Addresses.Imanzi1), Addresses.Imanzi1)},
                {"Imanzi 2: Hall Of Heroes",new Tuple<int, uint>(Memory.ReadInt(Addresses.Imanzi2), Addresses.Imanzi2)},
                {"Dark Steadfast 1: Hall Of Heroes",new Tuple<int, uint>(Memory.ReadInt(Addresses.DarkSteadfast1), Addresses.DarkSteadfast1)},
                {"Dark Steadfast 2: Hall Of Heroes",new Tuple<int, uint>(Memory.ReadInt(Addresses.DarkSteadfast2), Addresses.DarkSteadfast2)},
                {"Karl Stungard 1: Hall Of Heroes",new Tuple<int, uint>(Memory.ReadInt(Addresses.KarlStungard1), Addresses.KarlStungard1)},
                {"Karl Stungard 2: Hall Of Heroes",new Tuple<int, uint>(Memory.ReadInt(Addresses.KarlStungard2), Addresses.KarlStungard2)},
                {"Bloodmonath Skill Cleaver 1: Hall Of Heroes",new Tuple<int, uint>(Memory.ReadInt(Addresses.BloodmonathSkillCleaver1), Addresses.BloodmonathSkillCleaver1)},
                {"Bloodmonath Skill Cleaver 2: Hall Of Heroes",new Tuple<int, uint>(Memory.ReadInt(Addresses.BloodmonathSkillCleaver2), Addresses.BloodmonathSkillCleaver2)},
                {"Megwynne Stormbinder 1: Hall Of Heroes",new Tuple<int, uint>(Memory.ReadInt(Addresses.MegwynneStormbinder1), Addresses.MegwynneStormbinder1)},
                {"Megwynne Stormbinder 2: Hall Of Heroes",new Tuple<int, uint>(Memory.ReadInt(Addresses.MegwynneStormbinder2), Addresses.MegwynneStormbinder2)},
            };
        }

        public static List<Location> BuildLocationList()
        {
            int baseId = 99251000;
            List<LevelData> levels = GetLevelData();
            List<HallOfHeroesData> hallOfHeroesVisits = GetHallOfHeroesData();
            List<Location> locations = new List<Location>();
            var levelDict = GetLevelCompleteStatuses();
            var hallOfHeroesDict = GetHallOfHeroesStatuses();

            // in order to do chalices, this needs to be able to put in more than one address to check to update th elocatoin


            // level and Chalice locations
            foreach (var level in levels) {
                if (level.Name.Contains("Cleared")) {
                    {
                        Location location = new Location()
                        {
                            Name = level.Name,
                            Address = levelDict[level.Name].Item2,
                            Id = baseId + level.LevelId,
                            CheckType = LocationCheckType.Byte,
                            CompareType = level.Name.Contains("Zarok") ? LocationCheckCompareType.Match : LocationCheckCompareType.GreaterThan,
                            CheckValue = level.Name = "16"
                        };

                        locations.Add(location);

                    };
                }
                if (level.Name.Contains("Chalice"))
                {
                    {
                        Location location = new Location()
                        {
                            Name = level.Name,
                            Address = levelDict[level.Name].Item2,
                            Id = baseId + level.LevelId,
                            CheckType = LocationCheckType.Byte,
                            CheckValue = "19"
                        };

                        locations.Add(location);

                    };
                }
            }
            foreach (var hallOfHeroesVisit in hallOfHeroesVisits)
            {
                {
                    Location location = new Location()
                    {
                        Name = hallOfHeroesVisit.Name,
                        Address = hallOfHeroesDict[hallOfHeroesVisit.Name].Item2,
                        Id = baseId + hallOfHeroesVisit.LevelId,
                        CheckType = LocationCheckType.Byte,
                        CompareType = LocationCheckCompareType.GreaterThan,
                        CheckValue = hallOfHeroesVisit.Name = "0"
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
            ["Broadsword"] = Addresses.BroadswordCharge,
            ["Broadsword Charge"] = Addresses.BroadswordCharge,
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

        internal static readonly Dictionary<string, uint> SkillDictionary = new()
        {
            ["Daring Dash"] = Addresses.DaringDashSkill,
        };


        private static List<HallOfHeroesData> GetHallOfHeroesData()
        {
            List<HallOfHeroesData> hallOfHeroeVisits = new List<HallOfHeroesData>()
            {
                new HallOfHeroesData("Canny Tim 1: Hall Of Heroes",44),
                new HallOfHeroesData("Canny Tim 2: Hall Of Heroes",45),
                new HallOfHeroesData("Stanyer Iron Hewer 1: Hall Of Heroes",46),
                new HallOfHeroesData("Stanyer Iron Hewer 2: Hall Of Heroes",47),
                new HallOfHeroesData("Woden The Mighty 1: Hall Of Heroes",48),
                new HallOfHeroesData("Woden The Mighty 2: Hall Of Heroes",49),
                new HallOfHeroesData("RavenHooves The Archer 1: Hall Of Heroes",50),
                new HallOfHeroesData("RavenHooves The Archer 2: Hall Of Heroes",51),
                new HallOfHeroesData("RavenHooves The Archer 3: Hall Of Heroes",52),
                new HallOfHeroesData("RavenHooves The Archer 4: Hall Of Heroes",53),
                new HallOfHeroesData("Imanzi 1: Hall Of Heroes",54),
                new HallOfHeroesData("Imanzi 2: Hall Of Heroes",55),
                new HallOfHeroesData("Dark Steadfast 1: Hall Of Heroes",56),
                new HallOfHeroesData("Dark Steadfast 2: Hall Of Heroes",57),
                new HallOfHeroesData("Karl Stungard 1: Hall Of Heroes",58),
                new HallOfHeroesData("Karl Stungard 2: Hall Of Heroes",59),
                new HallOfHeroesData("Bloodmonath Skill Cleaver 1: Hall Of Heroes",60),
                new HallOfHeroesData("Bloodmonath Skill Cleaver 2: Hall Of Heroes",61),
                new HallOfHeroesData("Megwynne Stormbinder 1: Hall Of Heroes",62),
                new HallOfHeroesData("Megwynne Stormbinder 2: Hall Of Heroes",63),
            };
            return hallOfHeroeVisits;
        }
        private static List<LevelData> GetLevelData()
        {
            List<LevelData> levels = new List<LevelData>()
            {
                new LevelData("Dan's Crypt: Cleared", 1, [], []),
                new LevelData("The Graveyard: Cleared", 2, [], []),
                new LevelData("Return to the Graveyard: Cleared",3, [], []),
                new LevelData("Cemetery Hill: Cleared",4, [], []),
                new LevelData("The Hilltop Mausoleum: Cleared",5, [], []),
                new LevelData("Scarecrow Fields: Cleared",6, [], []),
                new LevelData("Ant Hill: Cleared",7, [], []),
                new LevelData("The Crystal Caves: Cleared",6, [], []),
                new LevelData("The Lake: Cleared",9, [], []),
                new LevelData("Pumpkin Gorge: Cleared",10, [], []),
                new LevelData("Pumpkin Serpent: Cleared",11, [], []),
                new LevelData("Sleeping Village: Cleared",12, [], []),
                new LevelData("Pools of the Ancient Dead: Cleared",13, [], []),
                new LevelData("Asylum Grounds: Cleared",14, [], []),
                new LevelData("Inside the Asylum: Cleared",15, [], []),
                new LevelData("Enchanted Earth: Cleared",16, [], []),
                new LevelData("The Gallows Gauntlet: Cleared" ,17, [], []),
                new LevelData("The Haunted Ruins: Cleared",18, [], []),
                new LevelData("Ghost Ship: Cleared",19, [], []),
                new LevelData("The Entrance Hall: Cleared",20, [], []),
                new LevelData("The Time Device: Cleared",21, [], []),
                new LevelData("Zaroks Lair: Cleared",22, [], []), // not sure if this is considered a level as it's the clear condition. Putting it here for now
                new LevelData("The Graveyard: Chalice", 23, [], []),
                new LevelData("Return to the Graveyard: Chalice", 24, [], []),
                new LevelData("Cemetery Hill: Chalice", 25, [], []),
                new LevelData("The Hilltop Mausoleum: Chalice", 26, [], []),
                new LevelData("Scarecrow Fields: Chalice", 27, [], []),
                new LevelData("Ant Hill: Chalice", 28, [], []),
                new LevelData("The Crystal Caves: Chalice", 29, [], []),
                new LevelData("The Lake: Chalice", 30, [], []),
                new LevelData("Pumpkin Gorge: Chalice", 31, [], []),
                new LevelData("Pumpkin Serpent: Chalice", 32, [], []),
                new LevelData("Sleeping Village: Chalice", 33, [], []),
                new LevelData("Pools of the Ancient Dead: Chalice", 34, [], []),
                new LevelData("Asylum Grounds: Chalice", 35, [], []),
                new LevelData("Inside the Asylum: Chalice", 36, [], []),
                new LevelData("Enchanted Earth: Chalice", 37, [], []),
                new LevelData("The Gallows Gauntlet: Chalice", 38, [], []),
                new LevelData("The Haunted Ruins: Chalice", 39, [], []),
                new LevelData("Ghost Ship: Chalice", 40, [], []),
                new LevelData("The Entrance Hall: Chalice", 41, [], []),
                new LevelData("The Time Device: Chalice", 42, [], [])
            };
            return levels;
        }
    }
}