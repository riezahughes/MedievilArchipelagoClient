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
        public static Dictionary<string, Tuple<int, uint>> GetSkillStatuses()
        {
            return new Dictionary<string, Tuple<int, uint>>
            {
                {"Daring Dash: Skill",new Tuple<int, uint>(Memory.ReadInt(Addresses.DaringDashSkill), Addresses.DaringDashSkill)},
            };
        }

        public static Dictionary<string, Tuple<int, uint>> GetWeaponLocationStatuses()
        {
            return new Dictionary<string, Tuple<int, uint>>
            {
                // uses fake locations at the moment
                {"Small Sword: Dans Crypt",new Tuple<int, uint>(Memory.ReadInt(Addresses.FakeAddress), Addresses.FakeAddress)},
                {"Broadsword: Hall of Heroes",new Tuple<int, uint>(Memory.ReadInt(Addresses.FakeAddress), Addresses.FakeAddress)},
                {"Magic Sword: Hall of Heroes",new Tuple<int, uint>(Memory.ReadInt(Addresses.FakeAddress), Addresses.FakeAddress)},
                {"Club: Cemetery Hill",new Tuple<int, uint>(Memory.ReadInt(Addresses.FakeAddress), Addresses.FakeAddress)},
                {"Hammer: Hall Of Heroes",new Tuple<int, uint>(Memory.ReadInt(Addresses.FakeAddress), Addresses.FakeAddress)},
                {"Daggers: Dan's Crypt",new Tuple<int, uint>(Memory.ReadInt(Addresses.FakeAddress), Addresses.FakeAddress)},
                {"Axe: Hall Of Heroes",new Tuple<int, uint>(Memory.ReadInt(Addresses.FakeAddress), Addresses.FakeAddress)},
                {"Chicken Drumsticks: The Enchanted Earth",new Tuple<int, uint>(Memory.ReadInt(Addresses.FakeAddress), Addresses.FakeAddress)},
                {"Crossbow: Hall Of Heroes",new Tuple<int, uint>(Memory.ReadInt(Addresses.FakeAddress), Addresses.FakeAddress)},
                {"Longbow: Hall Of Heroes",new Tuple<int, uint>(Memory.ReadInt(Addresses.FakeAddress), Addresses.FakeAddress)},
                {"Fire Longbow: Hall Of Heroes",new Tuple<int, uint>(Memory.ReadInt(Addresses.FakeAddress), Addresses.FakeAddress)},
                {"Magic Longbow: Hall Of Heroes",new Tuple<int, uint>(Memory.ReadInt(Addresses.FakeAddress), Addresses.FakeAddress)},
                {"Spear: Hall Of Heroes",new Tuple<int, uint>(Memory.ReadInt(Addresses.FakeAddress), Addresses.FakeAddress)},
                {"Lightning: Hall Of Heroes",new Tuple<int, uint>(Memory.ReadInt(Addresses.FakeAddress), Addresses.FakeAddress)},
                {"Good Lightning: Zarok's Lair",new Tuple<int, uint>(Memory.ReadInt(Addresses.FakeAddress), Addresses.FakeAddress)},
                {"Copper Shield: Dan's Crypt",new Tuple<int, uint>(Memory.ReadInt(Addresses.FakeAddress), Addresses.FakeAddress)},
                {"Silver Shield: Return to the Graveyard",new Tuple<int, uint>(Memory.ReadInt(Addresses.FakeAddress), Addresses.FakeAddress)},
                {"Gold Shield: Hall Of Heroes",new Tuple<int, uint>(Memory.ReadInt(Addresses.FakeAddress), Addresses.FakeAddress)},
                {"Dragon Armour: The Crystal Caves",new Tuple<int, uint>(Memory.ReadInt(Addresses.FakeAddress), Addresses.FakeAddress)},
            };
        }

        public static Dictionary<string, Tuple<int, uint>> GetHallOfHeroesStatuses()
        {
            return new Dictionary<string, Tuple<int, uint>>
            {
                // these locations need updated.

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

        public static Dictionary<string, Tuple<int, uint>> GetLifeBottleLocationStatuses()
        {
            return new Dictionary<string, Tuple<int, uint>>
            {
                // uses fake locations at the moment
                {"Life Bottle: Dan's Crypt", new Tuple<int, uint>(Memory.ReadInt(Addresses.FakeAddress), Addresses.FakeAddress)},
                {"Life Bottle: The Graveyard", new Tuple<int, uint>(Memory.ReadInt(Addresses.FakeAddress), Addresses.FakeAddress)},
                {"Life Bottle: Hall of Heroes (Canny Tim)", new Tuple<int, uint>(Memory.ReadInt(Addresses.FakeAddress), Addresses.FakeAddress)},
                {"Life Bottle: Dan's Crypt - Behind Wall", new Tuple<int, uint>(Memory.ReadInt(Addresses.FakeAddress), Addresses.FakeAddress)},
                {"Life Bottle: Scarecrow Fields", new Tuple<int, uint>(Memory.ReadInt(Addresses.FakeAddress), Addresses.FakeAddress)},
                {"Life Bottle: Pools of the Ancient Dead", new Tuple<int, uint>(Memory.ReadInt(Addresses.FakeAddress), Addresses.FakeAddress)},
                {"Life Bottle: Hall of Heroes (Ravenhooves The Archer)", new Tuple<int, uint>(Memory.ReadInt(Addresses.FakeAddress), Addresses.FakeAddress)},
                {"Life Bottle: Hall of Heroes (Dirk Steadfast)", new Tuple<int, uint>(Memory.ReadInt(Addresses.FakeAddress), Addresses.FakeAddress)},
                {"Life Bottle: The Time Device", new Tuple<int, uint>(Memory.ReadInt(Addresses.FakeAddress), Addresses.FakeAddress)}
            };
        }

        public static Dictionary<string, Tuple<int, uint>> GetRuneLocationStatuses()
        {
            return new Dictionary<string, Tuple<int, uint>>
            {
                // uses fake locations at the moment

                // Chaos Runes
                {"Chaos Rune: The Graveyard", new Tuple<int, uint>(Memory.ReadInt(Addresses.FakeAddress), Addresses.FakeAddress)},
                {"Chaos Rune: The Hilltop Mausoleum", new Tuple<int, uint>(Memory.ReadInt(Addresses.FakeAddress), Addresses.FakeAddress)},
                {"Chaos Rune: Scarecrow Fields", new Tuple<int, uint>(Memory.ReadInt(Addresses.FakeAddress), Addresses.FakeAddress)},
                {"Chaos Rune: The Lake", new Tuple<int, uint>(Memory.ReadInt(Addresses.FakeAddress), Addresses.FakeAddress)},
                {"Chaos Rune: Pumpkin Gorge", new Tuple<int, uint>(Memory.ReadInt(Addresses.FakeAddress), Addresses.FakeAddress)},
                {"Chaos Rune: Sleeping Village", new Tuple<int, uint>(Memory.ReadInt(Addresses.FakeAddress), Addresses.FakeAddress)},
                {"Chaos Rune: Pools of the Ancient Dead", new Tuple<int, uint>(Memory.ReadInt(Addresses.FakeAddress), Addresses.FakeAddress)},
                {"Chaos Rune: Asylum Grounds", new Tuple<int, uint>(Memory.ReadInt(Addresses.FakeAddress), Addresses.FakeAddress)},
                {"Chaos Rune: The Haunted Ruins", new Tuple<int, uint>(Memory.ReadInt(Addresses.FakeAddress), Addresses.FakeAddress)},
                {"Chaos Rune: Ghost Ship", new Tuple<int, uint>(Memory.ReadInt(Addresses.FakeAddress), Addresses.FakeAddress)},
                {"Chaos Rune: The Time Device", new Tuple<int, uint>(Memory.ReadInt(Addresses.FakeAddress), Addresses.FakeAddress)},

                // Earth Runes
                {"Earth Rune: The Graveyard", new Tuple<int, uint>(Memory.ReadInt(Addresses.FakeAddress), Addresses.FakeAddress)},
                {"Earth Rune: The Hilltop Mausoleum", new Tuple<int, uint>(Memory.ReadInt(Addresses.FakeAddress), Addresses.FakeAddress)},
                {"Earth Rune: Scarecrow Fields", new Tuple<int, uint>(Memory.ReadInt(Addresses.FakeAddress), Addresses.FakeAddress)},
                {"Earth Rune: The Crystal Caves", new Tuple<int, uint>(Memory.ReadInt(Addresses.FakeAddress), Addresses.FakeAddress)},
                {"Earth Rune: The Lake", new Tuple<int, uint>(Memory.ReadInt(Addresses.FakeAddress), Addresses.FakeAddress)},
                {"Earth Rune: Pumpkin Gorge", new Tuple<int, uint>(Memory.ReadInt(Addresses.FakeAddress), Addresses.FakeAddress)},
                {"Earth Rune: Sleeping Village", new Tuple<int, uint>(Memory.ReadInt(Addresses.FakeAddress), Addresses.FakeAddress)},
                {"Earth Rune: Inside the Asylum", new Tuple<int, uint>(Memory.ReadInt(Addresses.FakeAddress), Addresses.FakeAddress)},
                {"Earth Rune: Enchanted Earth", new Tuple<int, uint>(Memory.ReadInt(Addresses.FakeAddress), Addresses.FakeAddress)},
                {"Earth Rune: The Haunted Ruins", new Tuple<int, uint>(Memory.ReadInt(Addresses.FakeAddress), Addresses.FakeAddress)},
                {"Earth Rune: The Entrance Hall", new Tuple<int, uint>(Memory.ReadInt(Addresses.FakeAddress), Addresses.FakeAddress)},
                {"Earth Rune: The Time Device", new Tuple<int, uint>(Memory.ReadInt(Addresses.FakeAddress), Addresses.FakeAddress)},

                // Moon Runes
                {"Moon Rune: The Hilltop Mausoleum", new Tuple<int, uint>(Memory.ReadInt(Addresses.FakeAddress), Addresses.FakeAddress)},
                {"Moon Rune: Scarecrow Fields", new Tuple<int, uint>(Memory.ReadInt(Addresses.FakeAddress), Addresses.FakeAddress)},
                {"Moon Rune: Pumpkin Gorge", new Tuple<int, uint>(Memory.ReadInt(Addresses.FakeAddress), Addresses.FakeAddress)},
                {"Moon Rune: Ghost Ship", new Tuple<int, uint>(Memory.ReadInt(Addresses.FakeAddress), Addresses.FakeAddress)},
                {"Moon Rune: The Time Device", new Tuple<int, uint>(Memory.ReadInt(Addresses.FakeAddress), Addresses.FakeAddress)},

                // Star Runes
                {"Star Rune: Return to the Graveyard", new Tuple<int, uint>(Memory.ReadInt(Addresses.FakeAddress), Addresses.FakeAddress)},
                {"Star Rune: Dan's Crypt", new Tuple<int, uint>(Memory.ReadInt(Addresses.FakeAddress), Addresses.FakeAddress)},
                {"Star Rune: The Crystal Caves", new Tuple<int, uint>(Memory.ReadInt(Addresses.FakeAddress), Addresses.FakeAddress)},
                {"Star Rune: The Lake", new Tuple<int, uint>(Memory.ReadInt(Addresses.FakeAddress), Addresses.FakeAddress)},
                {"Star Rune: Enchanted Earth", new Tuple<int, uint>(Memory.ReadInt(Addresses.FakeAddress), Addresses.FakeAddress)},
                {"Star Rune: The Gallows Gauntlet", new Tuple<int, uint>(Memory.ReadInt(Addresses.FakeAddress), Addresses.FakeAddress)},
                {"Star Rune: Ghost Ship", new Tuple<int, uint>(Memory.ReadInt(Addresses.FakeAddress), Addresses.FakeAddress)},

                // Time Runes
                {"Time Rune: The Lake", new Tuple<int, uint>(Memory.ReadInt(Addresses.FakeAddress), Addresses.FakeAddress)},
                {"Time Rune: Pumpkin Gorge", new Tuple<int, uint>(Memory.ReadInt(Addresses.FakeAddress), Addresses.FakeAddress)},
                {"Time Rune: The Time Device", new Tuple<int, uint>(Memory.ReadInt(Addresses.FakeAddress), Addresses.FakeAddress)}
            };
        }



        public static List<Location> BuildLocationList()
        {
            int baseId = 99251000;
            List<Location> locations = new List<Location>();

            List<LevelData> levels = GetLevelData();
            List<HallOfHeroesData> hallOfHeroesVisits = GetHallOfHeroesData();
            List<SkillData> skills = GetSkillData();
            List<WeaponLocationsData> weapons = GetWeaponLocationsData();
            List<LifeBottlesData> bottles = GetLifeBottleLocationsData();
            List<RuneData> runes = GetRuneLocationsData();

            var levelDict = GetLevelCompleteStatuses();
            var skillDict = GetSkillStatuses();
            var hallOfHeroesDict = GetHallOfHeroesStatuses();
            var weaponLocationDict = GetWeaponLocationStatuses();
            var bottleLocationDict = GetLifeBottleLocationStatuses();
            var runeLocationDict = GetRuneLocationStatuses();


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
            foreach (var skill in skills)
            {
                {
                    Location location = new Location()
                    {
                        Name = skill.Name,
                        Address = skillDict[skill.Name].Item2,
                        Id = baseId + skill.LevelId,
                        CheckType = LocationCheckType.Byte,
                        CompareType = LocationCheckCompareType.Match,
                        CheckValue = skill.Name = "1"
                    };

                    locations.Add(location);

                };
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
            foreach (var weaponLocation in weapons)
            {
                {
                    Location location = new Location()
                    {
                        Name = weaponLocation.Name,
                        Address = weaponLocationDict[weaponLocation.Name].Item2,
                        Id = baseId + weaponLocation.LevelId,
                        CheckType = LocationCheckType.Byte,
                        CompareType = LocationCheckCompareType.GreaterThan,
                        CheckValue = weaponLocation.Name = "999" // fake number as this is just a dummy location for now
                    };

                    locations.Add(location);

                };
            }
            foreach (var bottleLocation in bottles)
            {
                {
                    Location location = new Location()
                    {
                        Name = bottleLocation.Name,
                        Address = bottleLocationDict[bottleLocation.Name].Item2,
                        Id = baseId + bottleLocation.LevelId,
                        CheckType = LocationCheckType.Byte,
                        CompareType = LocationCheckCompareType.GreaterThan,
                        CheckValue = bottleLocation.Name = "999" // fake number as this is just a dummy location for now
                    };

                    locations.Add(location);

                };
            }
            foreach (var runeLocation in runes)
            {
                {
                    Location location = new Location()
                    {
                        Name = runeLocation.Name,
                        Address = runeLocationDict[runeLocation.Name].Item2,
                        Id = baseId + runeLocation.LevelId,
                        CheckType = LocationCheckType.Byte,
                        CompareType = LocationCheckCompareType.GreaterThan,
                        CheckValue = runeLocation.Name = "999" // fake number as this is just a dummy location for now
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

        private static List<SkillData> GetSkillData()
        {
            List<SkillData> skills = new List<SkillData>()
            {
                new SkillData("Daring Dash: Skill", 43)
            };
            return skills;
        }

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
        private static List<WeaponLocationsData> GetWeaponLocationsData()
        {
            List<WeaponLocationsData> weaponLocations = new List<WeaponLocationsData>()
            {
                new WeaponLocationsData("Small Sword: Dans Crypt",64),
                new WeaponLocationsData("Broadsword: Hall of Heroes",65),
                new WeaponLocationsData("Magic Sword: Hall of Heroes",66),
                new WeaponLocationsData("Club: Cemetery Hill",67),
                new WeaponLocationsData("Hammer: Hall Of Heroes",68),
                new WeaponLocationsData("Daggers: Dan's Crypt",69),
                new WeaponLocationsData("Axe: Hall Of Heroes",70),
                new WeaponLocationsData("Chicken Drumsticks: The Enchanted Earth",71),
                new WeaponLocationsData("Crossbow: Hall Of Heroes",72),
                new WeaponLocationsData("Longbow: Hall Of Heroes",73),
                new WeaponLocationsData("Fire Longbow: Hall Of Heroes",74),
                new WeaponLocationsData("Magic Longbow: Hall Of Heroes",75),
                new WeaponLocationsData("Spear: Hall Of Heroes",76),
                new WeaponLocationsData("Lightning: Hall Of Heroes",77),
                new WeaponLocationsData("Good Lightning: Zarok's Lair",78),
                new WeaponLocationsData("Copper Shield: Dan's Crypt",79),
                new WeaponLocationsData("Silver Shield: Return to the Graveyard",80),
                new WeaponLocationsData("Gold Shield: Hall Of Heroes",81),
                new WeaponLocationsData("Dragon Armour: The Crystal Caves",82),
            };
            return weaponLocations;
        }

        private static List<LifeBottlesData> GetLifeBottleLocationsData()
        {
            List<LifeBottlesData> bottleLocations = new List<LifeBottlesData>()
            {
                new LifeBottlesData("Life Bottle: Dan's Crypt",83),
                new LifeBottlesData("Life Bottle: The Graveyard",84),
                new LifeBottlesData("Life Bottle: Hall of Heroes (Canny Tim)",85),
                new LifeBottlesData("Life Bottle: Dan's Crypt - Behind Wall",86),
                new LifeBottlesData("Life Bottle: Scarecrow Fields",87),
                new LifeBottlesData("Life Bottle: Pools of the Ancient Dead",88),
                new LifeBottlesData("Life Bottle: Hall of Heroes (Ravenhooves The Archer)",89),
                new LifeBottlesData("Life Bottle: Hall of Heroes (Dirk Steadfast)",90),
                new LifeBottlesData("Life Bottle: The Time Device",91),
            };
            return bottleLocations;
        }

        private static List<RuneData> GetRuneLocationsData()
        {
            List<RuneData> runeLocations = new List<RuneData>()
            {
                new RuneData("Chaos Rune: The Graveyard",92),
                new RuneData("Chaos Rune: The Hilltop Mausoleum",93),
                new RuneData("Chaos Rune: Scarecrow Fields",94),
                new RuneData("Chaos Rune: The Lake",95),
                new RuneData("Chaos Rune: Pumpkin Gorge",96),
                new RuneData("Chaos Rune: Sleeping Village",97),
                new RuneData("Chaos Rune: Pools of the Ancient Dead",98),
                new RuneData("Chaos Rune: Asylum Grounds",99),
                new RuneData("Chaos Rune: The Haunted Ruins",100),
                new RuneData("Chaos Rune: Ghost Ship",101),
                new RuneData("Chaos Rune: The Time Device",102),
                new RuneData("Earth Rune: The Graveyard",103),
                new RuneData("Earth Rune: The Hilltop Mausoleum",104),
                new RuneData("Earth Rune: Scarecrow Fields",105),
                new RuneData("Earth Rune: The Crystal Caves",106),
                new RuneData("Earth Rune: The Lake",107),
                new RuneData("Earth Rune: Pumpkin Gorge",108),
                new RuneData("Earth Rune: Sleeping Village",109),
                new RuneData("Earth Rune: Inside the Asylum",110),
                new RuneData("Earth Rune: Enchanted Earth",111),
                new RuneData("Earth Rune: The Haunted Ruins",112),
                new RuneData("Earth Rune: The Entrance Hall",113),
                new RuneData("Earth Rune: The Time Device",114),
                new RuneData("Moon Rune: The Hilltop Mausoleum",115),
                new RuneData("Moon Rune: Scarecrow Fields",116),
                new RuneData("Moon Rune: Pumpkin Gorge",117),
                new RuneData("Moon Rune: Ghost Ship",118),
                new RuneData("Moon Rune: The Time Device",119),
                new RuneData("Star Rune: Return to the Graveyard",120),
                new RuneData("Star Rune: Dan's Crypt",121),
                new RuneData("Star Rune: The Crystal Caves",122),
                new RuneData("Star Rune: The Lake",123),
                new RuneData("Star Rune: Enchanted Earth",124),
                new RuneData("Star Rune: The Gallows Gauntlet",125),
                new RuneData("Star Rune: Ghost Ship",126),
                new RuneData("Time Rune: The Lake",127),
                new RuneData("Time Rune: Pumpkin Gorge",128),
                new RuneData("Time Rune: The Time Device",129),
            };
            return runeLocations;
        }

    }
}