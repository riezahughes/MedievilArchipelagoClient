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
        public static Dictionary<string, Tuple<int, uint>> GetLevelCompleteStatuses()
        {
            return new Dictionary<string, Tuple<int, uint>>
            {
               {"Cleared: Dan's Crypt", new Tuple<int, uint>(Memory.ReadInt(Addresses.DansCryptLevelStatus), Addresses.DansCryptLevelStatus)},
               {"Cleared: The Graveyard", new Tuple<int, uint>(Memory.ReadInt(Addresses.TheGraveyardLevelStatus), Addresses.TheGraveyardLevelStatus)},
               {"Cleared: Return to the Graveyard", new Tuple<int, uint>(Memory.ReadInt(Addresses.ReturnToTheGraveyardLevelStatus), Addresses.ReturnToTheGraveyardLevelStatus)},
               {"Cleared: Cemetery Hill", new Tuple<int, uint>(Memory.ReadInt(Addresses.CemeteryHillLevelStatus), Addresses.CemeteryHillLevelStatus)},
               {"Cleared: The Hilltop Mausoleum", new Tuple<int, uint>(Memory.ReadInt(Addresses.TheHilltopMausoleumLevelStatus), Addresses.TheHilltopMausoleumLevelStatus)},
               {"Cleared: Scarecrow Fields", new Tuple<int, uint>(Memory.ReadInt(Addresses.ScarecrowFieldsLevelStatus), Addresses.ScarecrowFieldsLevelStatus)},
               {"Cleared: Ant Hill", new Tuple<int, uint>(Memory.ReadInt(Addresses.AntHillLevelStatus), Addresses.AntHillLevelStatus)},
               {"Cleared: The Crystal Caves", new Tuple<int, uint>(Memory.ReadInt(Addresses.TheCrystalCavesLevelStatus), Addresses.TheCrystalCavesLevelStatus)},
               {"Cleared: The Lake", new Tuple<int, uint>(Memory.ReadInt(Addresses.TheLakeLevelStatus), Addresses.TheLakeLevelStatus)},
               {"Cleared: Pumpkin Gorge", new Tuple<int, uint>(Memory.ReadInt(Addresses.PumpkinGorgeLevelStatus), Addresses.PumpkinGorgeLevelStatus)},
               {"Cleared: Pumpkin Serpent", new Tuple<int, uint>(Memory.ReadInt(Addresses.PumpkinSerpentLevelStatus), Addresses.PumpkinSerpentLevelStatus)},
               {"Cleared: Sleeping Village", new Tuple<int, uint>(Memory.ReadInt(Addresses.SleepingVillageLevelStatus), Addresses.SleepingVillageLevelStatus)},
               {"Cleared: Pools of the Ancient Dead", new Tuple<int, uint>(Memory.ReadInt(Addresses.PoolsOfTheAncientDeadLevelStatus), Addresses.PoolsOfTheAncientDeadLevelStatus)},
               {"Cleared: Asylum Grounds", new Tuple<int, uint>(Memory.ReadInt(Addresses.AsylumGroundsLevelStatus), Addresses.AsylumGroundsLevelStatus)},
               {"Cleared: Inside the Asylum", new Tuple<int, uint>(Memory.ReadInt(Addresses.InsideTheAsylumLevelStatus), Addresses.InsideTheAsylumLevelStatus)},
               {"Cleared: Enchanted Earth", new Tuple<int, uint>(Memory.ReadInt(Addresses.EnchantedEarthLevelStatus), Addresses.EnchantedEarthLevelStatus)},
               {"Cleared: The Gallows Gauntlet", new Tuple<int, uint>(Memory.ReadInt(Addresses.TheGallowsGauntletLevelStatus), Addresses.TheGallowsGauntletLevelStatus)},
               {"Cleared: The Haunted Ruins", new Tuple<int, uint>(Memory.ReadInt(Addresses.TheHauntedRuinsLevelStatus), Addresses.TheHauntedRuinsLevelStatus)},
               {"Cleared: Ghost Ship", new Tuple<int, uint>(Memory.ReadInt(Addresses.GhostShipLevelStatus), Addresses.GhostShipLevelStatus)},
               {"Cleared: The Entrance Hall", new Tuple<int, uint>(Memory.ReadInt(Addresses.TheEntranceHallLevelStatus), Addresses.TheEntranceHallLevelStatus)},
               {"Cleared: The Time Device", new Tuple<int, uint>(Memory.ReadInt(Addresses.TheTimeDeviceLevelStatus), Addresses.TheTimeDeviceLevelStatus)},
               {"Cleared: Zaroks Lair", new Tuple<int, uint>(Memory.ReadInt(Addresses.ZaroksLairLevelStatus), Addresses.ZaroksLairLevelStatus)},
            };
        }
        public static Dictionary<string, Tuple<int, uint>> GetChaliceLocationStatuses()
        {
            return new Dictionary<string, Tuple<int, uint>>
            {
               {"Chalice: The Graveyard", new Tuple<int, uint>(Memory.ReadInt(Addresses.TheGraveyardLevelStatus), Addresses.TheGraveyardLevelStatus)},
               {"Chalice: Return to the Graveyard", new Tuple<int, uint>(Memory.ReadInt(Addresses.ReturnToTheGraveyardLevelStatus), Addresses.ReturnToTheGraveyardLevelStatus)},
               {"Chalice: Cemetery Hill", new Tuple<int, uint>(Memory.ReadInt(Addresses.CemeteryHillLevelStatus), Addresses.CemeteryHillLevelStatus)},
               {"Chalice: The Hilltop Mausoleum", new Tuple<int, uint>(Memory.ReadInt(Addresses.TheHilltopMausoleumLevelStatus), Addresses.TheHilltopMausoleumLevelStatus)},
               {"Chalice: Scarecrow Fields", new Tuple<int, uint>(Memory.ReadInt(Addresses.ScarecrowFieldsLevelStatus), Addresses.ScarecrowFieldsLevelStatus)},
               {"Chalice: Ant Hill", new Tuple<int, uint>(Memory.ReadInt(Addresses.AntHillLevelStatus), Addresses.AntHillLevelStatus)},
               {"Chalice: The Crystal Caves", new Tuple<int, uint>(Memory.ReadInt(Addresses.TheCrystalCavesLevelStatus), Addresses.TheCrystalCavesLevelStatus)},
               {"Chalice: The Lake", new Tuple<int, uint>(Memory.ReadInt(Addresses.TheLakeLevelStatus), Addresses.TheLakeLevelStatus)},
               {"Chalice: Pumpkin Gorge", new Tuple<int, uint>(Memory.ReadInt(Addresses.PumpkinGorgeLevelStatus), Addresses.PumpkinGorgeLevelStatus)},
               {"Chalice: Pumpkin Serpent", new Tuple<int, uint>(Memory.ReadInt(Addresses.PumpkinSerpentLevelStatus), Addresses.PumpkinSerpentLevelStatus)},
               {"Chalice: Sleeping Village", new Tuple<int, uint>(Memory.ReadInt(Addresses.SleepingVillageLevelStatus), Addresses.SleepingVillageLevelStatus)},
               {"Chalice: Pools of the Ancient Dead", new Tuple<int, uint>(Memory.ReadInt(Addresses.PoolsOfTheAncientDeadLevelStatus), Addresses.PoolsOfTheAncientDeadLevelStatus)},
               {"Chalice: Asylum Grounds", new Tuple<int, uint>(Memory.ReadInt(Addresses.AsylumGroundsLevelStatus), Addresses.AsylumGroundsLevelStatus)},
               {"Chalice: Inside the Asylum", new Tuple<int, uint>(Memory.ReadInt(Addresses.InsideTheAsylumLevelStatus), Addresses.InsideTheAsylumLevelStatus)},
               {"Chalice: Enchanted Earth", new Tuple<int, uint>(Memory.ReadInt(Addresses.EnchantedEarthLevelStatus), Addresses.EnchantedEarthLevelStatus)},
               {"Chalice: The Gallows Gauntlet", new Tuple<int, uint>(Memory.ReadInt(Addresses.TheGallowsGauntletLevelStatus), Addresses.TheGallowsGauntletLevelStatus)},
               {"Chalice: The Haunted Ruins", new Tuple<int, uint>(Memory.ReadInt(Addresses.TheHauntedRuinsLevelStatus), Addresses.TheHauntedRuinsLevelStatus)},
               {"Chalice: Ghost Ship", new Tuple<int, uint>(Memory.ReadInt(Addresses.GhostShipLevelStatus), Addresses.GhostShipLevelStatus)},
               {"Chalice: The Entrance Hall", new Tuple<int, uint>(Memory.ReadInt(Addresses.TheEntranceHallLevelStatus), Addresses.TheEntranceHallLevelStatus)},
               {"Chalice: The Time Device", new Tuple<int, uint>(Memory.ReadInt(Addresses.TheTimeDeviceLevelStatus), Addresses.TheTimeDeviceLevelStatus)},
            };
        }
        public static Dictionary<string, Tuple<int, uint>> GetSkillStatuses()
        {
            return new Dictionary<string, Tuple<int, uint>>
            {
                {"Skill: Daring Dash",new Tuple<int, uint>(Memory.ReadInt(Addresses.DaringDashSkill), Addresses.DaringDashSkill)},
            };
        }

        public static Dictionary<string, Tuple<int, uint>> GetWeaponLocationStatuses()
        {
            return new Dictionary<string, Tuple<int, uint>>
            {
                // uses fake locations at the moment
                {"Equipment: Small Sword",new Tuple<int, uint>(Memory.ReadInt(Addresses.FakeAddress), Addresses.FakeAddress)},
                {"Equipment: Broadsword",new Tuple<int, uint>(Memory.ReadInt(Addresses.FakeAddress), Addresses.FakeAddress)},
                {"Equipment: Magic Sword",new Tuple<int, uint>(Memory.ReadInt(Addresses.FakeAddress), Addresses.FakeAddress)},
                {"Equipment: Club",new Tuple<int, uint>(Memory.ReadInt(Addresses.FakeAddress), Addresses.FakeAddress)},
                {"Equipment: Hammer",new Tuple<int, uint>(Memory.ReadInt(Addresses.FakeAddress), Addresses.FakeAddress)},
                {"Equipment: Daggers",new Tuple<int, uint>(Memory.ReadInt(Addresses.FakeAddress), Addresses.FakeAddress)},
                {"Equipment: Axe",new Tuple<int, uint>(Memory.ReadInt(Addresses.FakeAddress), Addresses.FakeAddress)},
                {"Equipment: Chicken Drumsticks",new Tuple<int, uint>(Memory.ReadInt(Addresses.FakeAddress), Addresses.FakeAddress)},
                {"Equipment: Crossbow",new Tuple<int, uint>(Memory.ReadInt(Addresses.FakeAddress), Addresses.FakeAddress)},
                {"Equipment: Longbow",new Tuple<int, uint>(Memory.ReadInt(Addresses.FakeAddress), Addresses.FakeAddress)},
                {"Equipment: Fire Longbow",new Tuple<int, uint>(Memory.ReadInt(Addresses.FakeAddress), Addresses.FakeAddress)},
                {"Equipment: Magic Longbow",new Tuple<int, uint>(Memory.ReadInt(Addresses.FakeAddress), Addresses.FakeAddress)},
                {"Equipment: Spear",new Tuple<int, uint>(Memory.ReadInt(Addresses.FakeAddress), Addresses.FakeAddress)},
                {"Equipment: Lightning",new Tuple<int, uint>(Memory.ReadInt(Addresses.FakeAddress), Addresses.FakeAddress)},
                {"Equipment: Good Lightning",new Tuple<int, uint>(Memory.ReadInt(Addresses.FakeAddress), Addresses.FakeAddress)},
                {"Equipment: Copper Shield",new Tuple<int, uint>(Memory.ReadInt(Addresses.FakeAddress), Addresses.FakeAddress)},
                {"Equipment: Silver Shield",new Tuple<int, uint>(Memory.ReadInt(Addresses.FakeAddress), Addresses.FakeAddress)},
                {"Equipment: Gold Shield",new Tuple<int, uint>(Memory.ReadInt(Addresses.FakeAddress), Addresses.FakeAddress)},
                {"Equipment: Dragon Armour",new Tuple<int, uint>(Memory.ReadInt(Addresses.FakeAddress), Addresses.FakeAddress)},
            };
        }

        public static Dictionary<string, Tuple<int, uint>> GetHallOfHeroesStatuses()
        {
            return new Dictionary<string, Tuple<int, uint>>
            {
                // these locations need updated.

                {"Hall of Heroes: Canny Tim 1",new Tuple<int, uint>(Memory.ReadInt(Addresses.CannyTim1), Addresses.CannyTim1)},
                {"Hall of Heroes: Canny Tim 2",new Tuple<int, uint>(Memory.ReadInt(Addresses.CannyTim2), Addresses.CannyTim2)},
                {"Hall of Heroes: Stanyer Iron Hewer 1",new Tuple<int, uint>(Memory.ReadInt(Addresses.StanyerIronHewer1), Addresses.StanyerIronHewer1)},
                {"Hall of Heroes: Stanyer Iron Hewer 2",new Tuple<int, uint>(Memory.ReadInt(Addresses.StanyerIronHewer2), Addresses.StanyerIronHewer2)},
                {"Hall of Heroes: Woden The Mighty 1",new Tuple<int, uint>(Memory.ReadInt(Addresses.WodenTheMighty1), Addresses.WodenTheMighty1)},
                {"Hall of Heroes: Woden The Mighty 2",new Tuple<int, uint>(Memory.ReadInt(Addresses.WodenTheMighty2), Addresses.WodenTheMighty2)},
                {"Hall of Heroes: RavenHooves The Archer 1",new Tuple<int, uint>(Memory.ReadInt(Addresses.RavenHoovesTheArcher1), Addresses.RavenHoovesTheArcher1)},
                {"Hall of Heroes: RavenHooves The Archer 2",new Tuple<int, uint>(Memory.ReadInt(Addresses.RavenHoovesTheArcher2), Addresses.RavenHoovesTheArcher2)},
                {"Hall of Heroes: RavenHooves The Archer 3",new Tuple<int, uint>(Memory.ReadInt(Addresses.RavenHoovesTheArcher3), Addresses.RavenHoovesTheArcher3)},
                {"Hall of Heroes: RavenHooves The Archer 4",new Tuple<int, uint>(Memory.ReadInt(Addresses.RavenHoovesTheArcher4), Addresses.RavenHoovesTheArcher4)},
                {"Hall of Heroes: Imanzi 1",new Tuple<int, uint>(Memory.ReadInt(Addresses.Imanzi1), Addresses.Imanzi1)},
                {"Hall of Heroes: Imanzi 2",new Tuple<int, uint>(Memory.ReadInt(Addresses.Imanzi2), Addresses.Imanzi2)},
                {"Hall of Heroes: Dark Steadfast 1",new Tuple<int, uint>(Memory.ReadInt(Addresses.DarkSteadfast1), Addresses.DarkSteadfast1)},
                {"Hall of Heroes: Dark Steadfast 2",new Tuple<int, uint>(Memory.ReadInt(Addresses.DarkSteadfast2), Addresses.DarkSteadfast2)},
                {"Hall of Heroes: Karl Stungard 1",new Tuple<int, uint>(Memory.ReadInt(Addresses.KarlStungard1), Addresses.KarlStungard1)},
                {"Hall of Heroes: Karl Stungard 2",new Tuple<int, uint>(Memory.ReadInt(Addresses.KarlStungard2), Addresses.KarlStungard2)},
                {"Hall of Heroes: Bloodmonath Skill Cleaver 1",new Tuple<int, uint>(Memory.ReadInt(Addresses.BloodmonathSkillCleaver1), Addresses.BloodmonathSkillCleaver1)},
                {"Hall of Heroes: Bloodmonath Skill Cleaver 2",new Tuple<int, uint>(Memory.ReadInt(Addresses.BloodmonathSkillCleaver2), Addresses.BloodmonathSkillCleaver2)},
                {"Hall of Heroes: Megwynne Stormbinder 1",new Tuple<int, uint>(Memory.ReadInt(Addresses.MegwynneStormbinder1), Addresses.MegwynneStormbinder1)},
                {"Hall of Heroes: Megwynne Stormbinder 2",new Tuple<int, uint>(Memory.ReadInt(Addresses.MegwynneStormbinder2), Addresses.MegwynneStormbinder2)},
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
            List<ChaliceData> chalices = GetChaliceData();
            List<HallOfHeroesData> hallOfHeroesVisits = GetHallOfHeroesData();
            List<SkillData> skills = GetSkillData();
            List<WeaponLocationsData> weapons = GetWeaponLocationsData();
            List<LifeBottlesData> bottles = GetLifeBottleLocationsData();
            List<RuneData> runes = GetRuneLocationsData();

            var levelDict = GetLevelCompleteStatuses();
            var chaliceDict = GetChaliceLocationStatuses();
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
            }
            foreach (var chalice in chalices)
            {
                if (chalice.Name.Contains("Chalice"))
                {
                    {
                        Location location = new Location()
                        {
                            Name = chalice.Name,
                            Address = chaliceDict[chalice.Name].Item2,
                            Id = baseId + chalice.LevelId,
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
                new LevelData("Cleared: Dan's Crypt", 1, [], []),
                new LevelData("Cleared: The Graveyard", 2, [], []),
                new LevelData("Cleared: Return to the Graveyard",3, [], []),
                new LevelData("Cleared: Cemetery Hill",4, [], []),
                new LevelData("Cleared: The Hilltop Mausoleum",5, [], []),
                new LevelData("Cleared: Scarecrow Fields",6, [], []),
                new LevelData("Cleared: Ant Hill",7, [], []),
                new LevelData("Cleared: The Crystal Caves",6, [], []),
                new LevelData("Cleared: The Lake",9, [], []),
                new LevelData("Cleared: Pumpkin Gorge",10, [], []),
                new LevelData("Cleared: Pumpkin Serpent",11, [], []),
                new LevelData("Cleared: Sleeping Village",12, [], []),
                new LevelData("Cleared: Pools of the Ancient Dead",13, [], []),
                new LevelData("Cleared: Asylum Grounds",14, [], []),
                new LevelData("Cleared: Inside the Asylum",15, [], []),
                new LevelData("Cleared: Enchanted Earth",16, [], []),
                new LevelData("Cleared: The Gallows Gauntlet" ,17, [], []),
                new LevelData("Cleared: The Haunted Ruins",18, [], []),
                new LevelData("Cleared: Ghost Ship",19, [], []),
                new LevelData("Cleared: The Entrance Hall",20, [], []),
                new LevelData("Cleared: The Time Device",21, [], []),
                new LevelData("Cleared: Zaroks Lair",22, [], []), // not sure if this is considered a level as it's the clear condition. Putting it here for now

            };
            return levels;
        }
        private static List<ChaliceData> GetChaliceData()
        {
            List<ChaliceData> chalices = new List<ChaliceData>()
            {
                new ChaliceData("Chalice: The Graveyard", 23),
                new ChaliceData("Chalice: Return to the Graveyard", 24),
                new ChaliceData("Chalice: Cemetery Hill", 25),
                new ChaliceData("Chalice: The Hilltop Mausoleum", 26),
                new ChaliceData("Chalice: Scarecrow Fields", 27),
                new ChaliceData("Chalice: Ant Hill", 28),
                new ChaliceData("Chalice: The Crystal Caves", 29),
                new ChaliceData("Chalice: The Lake", 30),
                new ChaliceData("Chalice: Pumpkin Gorge", 31),
                new ChaliceData("Chalice: Pumpkin Serpent", 32),
                new ChaliceData("Chalice: Sleeping Village", 33),
                new ChaliceData("Chalice: Pools of the Ancient Dead", 34),
                new ChaliceData("Chalice: Asylum Grounds", 35),
                new ChaliceData("Chalice: Inside the Asylum", 36),
                new ChaliceData("Chalice: Enchanted Earth", 37),
                new ChaliceData("Chalice: The Gallows Gauntlet", 38),
                new ChaliceData("Chalice: The Haunted Ruins", 39),
                new ChaliceData("Chalice: Ghost Ship", 40),
                new ChaliceData("Chalice: The Entrance Hall", 41),
                new ChaliceData("Chalice: The Time Device", 42)
            };
            return chalices;
        }

        private static List<SkillData> GetSkillData()
        {
            List<SkillData> skills = new List<SkillData>()
            {
                new SkillData("Skill: Daring Dash", 43)
            };
            return skills;
        }

        private static List<HallOfHeroesData> GetHallOfHeroesData()
        {
            List<HallOfHeroesData> hallOfHeroeVisits = new List<HallOfHeroesData>()
            {
                new HallOfHeroesData("Hall of Heroes: Canny Tim 1",44),
                new HallOfHeroesData("Hall of Heroes: Canny Tim 2",45),
                new HallOfHeroesData("Hall of Heroes: Stanyer Iron Hewer 1",46),
                new HallOfHeroesData("Hall of Heroes: Stanyer Iron Hewer 2",47),
                new HallOfHeroesData("Hall of Heroes: Woden The Mighty 1",48),
                new HallOfHeroesData("Hall of Heroes: Woden The Mighty 2",49),
                new HallOfHeroesData("Hall of Heroes: RavenHooves The Archer 1",50),
                new HallOfHeroesData("Hall of Heroes: RavenHooves The Archer 2",51),
                new HallOfHeroesData("Hall of Heroes: RavenHooves The Archer 3",52),
                new HallOfHeroesData("Hall of Heroes: RavenHooves The Archer 4",53),
                new HallOfHeroesData("Hall of Heroes: Imanzi 1",54),
                new HallOfHeroesData("Hall of Heroes: Imanzi 2",55),
                new HallOfHeroesData("Hall of Heroes: Dark Steadfast 1",56),
                new HallOfHeroesData("Hall of Heroes: Dark Steadfast 2",57),
                new HallOfHeroesData("Hall of Heroes: Karl Stungard 1",58),
                new HallOfHeroesData("Hall of Heroes: Karl Stungard 2",59),
                new HallOfHeroesData("Hall of Heroes: Bloodmonath Skill Cleaver 1",60),
                new HallOfHeroesData("Hall of Heroes: Bloodmonath Skill Cleaver 2",61),
                new HallOfHeroesData("Hall of Heroes: Megwynne Stormbinder 1",62),
                new HallOfHeroesData("Hall of Heroes: Megwynne Stormbinder 2",63),
            };
            return hallOfHeroeVisits;
        }
        private static List<WeaponLocationsData> GetWeaponLocationsData()
        {
            List<WeaponLocationsData> weaponLocations = new List<WeaponLocationsData>()
            {
                new WeaponLocationsData("Equipment: Small Sword",64), // crypt
                new WeaponLocationsData("Equipment: Broadsword",65), // hall of heroes
                new WeaponLocationsData("Equipment: Magic Sword",66),// hall of heroes
                new WeaponLocationsData("Equipment: Club",67), // cemetery hill
                new WeaponLocationsData("Equipment: Hammer",68),// hall of heroes
                new WeaponLocationsData("Equipment: Daggers",69), // dan's crypt
                new WeaponLocationsData("Equipment: Axe",70),// hall of heroes
                new WeaponLocationsData("Equipment: Chicken Drumsticks",71), // enchanted earth
                new WeaponLocationsData("Equipment: Crossbow",72),// hall of heroes
                new WeaponLocationsData("Equipment: Longbow",73),// hall of heroes
                new WeaponLocationsData("Equipment: Fire Longbow",74),// hall of heroes
                new WeaponLocationsData("Equipment: Magic Longbow",75),// hall of heroes
                new WeaponLocationsData("Equipment: Spear",76),// hall of heroes
                new WeaponLocationsData("Equipment: Lightning",77),// hall of heroes
                new WeaponLocationsData("Equipment: Good Lightning",78), // zarok's lair
                new WeaponLocationsData("Equipment: Copper Shield",79), // dan's crypt  (Way more than just this)
                new WeaponLocationsData("Equipment: Silver Shield",80), // return to the graveyard  (Way more than just this)
                new WeaponLocationsData("Equipment: Gold Shield",81),// hall of heroes
                new WeaponLocationsData("Equipment: Dragon Armour",82), // crystal caves
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