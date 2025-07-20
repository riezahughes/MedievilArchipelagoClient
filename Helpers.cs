using Archipelago.Core.Models;
using Archipelago.Core.Util;
using Archipelago.MultiClient.Net.Models;
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
        public static Dictionary<string, Tuple<int, uint, uint>> GetKeyItemInventoryStatuses()
        {
            return new Dictionary<string, Tuple<int, uint, uint>>
            {
                
                {"Key Item: Dragon Gem: Inside the Asylum",new Tuple<int, uint, uint>(Memory.ReadInt(Addresses.DragonGem), Addresses.DragonGem, Addresses.FakeAddress)},
                {"Key Item: King Peregrine's Crown",new Tuple<int, uint, uint>(Memory.ReadInt(Addresses.KingPeregrinesCrown), Addresses.KingPeregrinesCrown, Addresses.FakeAddress)}, // not sure about this
                {"Key Item: Soul Helmet 1",new Tuple<int, uint, uint>(Memory.ReadInt(Addresses.SoulHelmet), Addresses.SoulHelmet, Addresses.FakeAddress)},
                {"Key Item: Soul Helmet 2",new Tuple<int, uint, uint>(Memory.ReadInt(Addresses.SoulHelmet), Addresses.SoulHelmet, Addresses.FakeAddress)},
                {"Key Item: Soul Helmet 3",new Tuple<int, uint, uint>(Memory.ReadInt(Addresses.SoulHelmet), Addresses.SoulHelmet, Addresses.FakeAddress)},
                {"Key Item: Soul Helmet 4",new Tuple<int, uint, uint>(Memory.ReadInt(Addresses.SoulHelmet), Addresses.SoulHelmet, Addresses.FakeAddress)},
                {"Key Item: Soul Helmet 5",new Tuple<int, uint, uint>(Memory.ReadInt(Addresses.SoulHelmet), Addresses.SoulHelmet, Addresses.FakeAddress)},
                {"Key Item: Soul Helmet 6",new Tuple<int, uint, uint>(Memory.ReadInt(Addresses.SoulHelmet), Addresses.SoulHelmet, Addresses.FakeAddress)},
                {"Key Item: Soul Helmet 7",new Tuple<int, uint, uint>(Memory.ReadInt(Addresses.SoulHelmet), Addresses.SoulHelmet, Addresses.FakeAddress)},
                {"Key Item: Soul Helmet 8",new Tuple<int, uint, uint>(Memory.ReadInt(Addresses.SoulHelmet), Addresses.SoulHelmet, Addresses.FakeAddress)},
                {"Key Item: Witches Talisman",new Tuple<int, uint, uint>(Memory.ReadInt(Addresses.WitchesTalisman), Addresses.WitchesTalisman, Addresses.FakeAddress)},
                {"Key Item: Safe Key",new Tuple<int, uint, uint>(Memory.ReadInt(Addresses.SafeKey), Addresses.SafeKey, Addresses.FakeAddress)},
                {"Key Item: Shadow Artefact",new Tuple<int, uint, uint>(Memory.ReadInt(Addresses.ShadowArtefact), Addresses.ShadowArtefact, Addresses.FakeAddress)},
                {"Key Item: Crucifix",new Tuple<int, uint, uint>(Memory.ReadInt(Addresses.Crucifix), Addresses.Crucifix, Addresses.FakeAddress)},
                {"Key Item: Landlords Bust",new Tuple<int, uint, uint>(Memory.ReadInt(Addresses.LandlordsBust), Addresses.LandlordsBust, Addresses.FakeAddress)},
                {"Key Item: Crucifix Cast",new Tuple<int, uint, uint>(Memory.ReadInt(Addresses.CrucifixCast), Addresses.CrucifixCast, Addresses.FakeAddress)},
                {"Key Item: Amber Piece 1",new Tuple<int, uint, uint>(Memory.ReadInt(Addresses.AmberPiece), Addresses.AmberPiece, Addresses.FakeAddress)},
                {"Key Item: Amber Piece 2",new Tuple<int, uint, uint>(Memory.ReadInt(Addresses.AmberPiece), Addresses.AmberPiece, Addresses.FakeAddress)},
                {"Key Item: Amber Piece 3",new Tuple<int, uint, uint>(Memory.ReadInt(Addresses.AmberPiece), Addresses.AmberPiece, Addresses.FakeAddress)},
                {"Key Item: Amber Piece 4",new Tuple<int, uint, uint>(Memory.ReadInt(Addresses.AmberPiece), Addresses.AmberPiece, Addresses.FakeAddress)},
                {"Key Item: Amber Piece 5",new Tuple<int, uint, uint>(Memory.ReadInt(Addresses.AmberPiece), Addresses.AmberPiece, Addresses.FakeAddress)},
                {"Key Item: Amber Piece 6",new Tuple<int, uint, uint>(Memory.ReadInt(Addresses.AmberPiece), Addresses.AmberPiece, Addresses.FakeAddress)},
                {"Key Item: Amber Piece 7",new Tuple<int, uint, uint>(Memory.ReadInt(Addresses.AmberPiece), Addresses.AmberPiece, Addresses.FakeAddress)},
                {"Key Item: Harvester Parts",new Tuple<int, uint, uint>(Memory.ReadInt(Addresses.HarvesterParts), Addresses.HarvesterParts, Addresses.FakeAddress)},
                {"Key Item: Skull Key",new Tuple<int, uint, uint>(Memory.ReadInt(Addresses.SkullKey), Addresses.SkullKey, Addresses.FakeAddress)},
                {"Key Item: Sheet Music",new Tuple<int, uint, uint>(Memory.ReadInt(Addresses.SheetMusic), Addresses.SheetMusic, Addresses.FakeAddress)},
            };
        }
        public static Dictionary<string, Tuple<int, uint, uint>> GetSkillStatuses()
        {
            return new Dictionary<string, Tuple<int, uint, uint>>
            {
                {"Skill: Daring Dash",new Tuple<int, uint, uint>(Memory.ReadInt(Addresses.DaringDashSkill), Addresses.DaringDashSkill, Addresses.FakeAddress)},
            };
        }

        public static Dictionary<string, Tuple<int, uint, uint>> GetWeaponInventoryLocationStatuses()
        {
            return new Dictionary<string, Tuple<int, uint, uint>>
            {
                {"Equipment: Small Sword",new Tuple<int, uint, uint>(Memory.ReadInt(Addresses.DC_Pickup_Shortsword), Addresses.DC_Pickup_Shortsword, Addresses.SmallSword)},
                {"Equipment: Broadsword",new Tuple<int, uint, uint>(Memory.ReadInt(Addresses.FakeAddress), Addresses.FakeAddress,  Addresses.BroadswordCharge)},
                {"Equipment: Magic Sword",new Tuple<int, uint, uint>(Memory.ReadInt(Addresses.FakeAddress), Addresses.FakeAddress,  Addresses.MagicSword)},
                {"Equipment: Club",new Tuple<int, uint, uint>(Memory.ReadInt(Addresses.FakeAddress), Addresses.FakeAddress,  Addresses.ClubCharge)},
                {"Equipment: Hammer",new Tuple<int, uint, uint>(Memory.ReadInt(Addresses.FakeAddress), Addresses.FakeAddress,  Addresses.Hammer)},
                {"Equipment: Daggers",new Tuple<int, uint, uint>(Memory.ReadInt(Addresses.DC_Pickup_Daggers), Addresses.DC_Pickup_Daggers,  Addresses.DaggerAmmo)},
                {"Equipment: Axe",new Tuple<int, uint, uint>(Memory.ReadInt(Addresses.FakeAddress), Addresses.FakeAddress,  Addresses.Axe)},
                {"Equipment: Chicken Drumsticks",new Tuple<int, uint, uint>(Memory.ReadInt(Addresses.FakeAddress), Addresses.FakeAddress,  Addresses.ChickenDrumsticksAmmo)},
                {"Equipment: Crossbow",new Tuple<int, uint, uint>(Memory.ReadInt(Addresses.FakeAddress), Addresses.FakeAddress,  Addresses.CrossbowAmmo)},
                {"Equipment: Longbow",new Tuple<int, uint, uint>(Memory.ReadInt(Addresses.FakeAddress), Addresses.FakeAddress,  Addresses.LongbowAmmo)},
                {"Equipment: Fire Longbow",new Tuple<int, uint, uint>(Memory.ReadInt(Addresses.FakeAddress), Addresses.FakeAddress,  Addresses.FireLongbowAmmo)},
                {"Equipment: Magic Longbow",new Tuple<int, uint, uint>(Memory.ReadInt(Addresses.FakeAddress), Addresses.FakeAddress,  Addresses.MagicLongbowAmmo)},
                {"Equipment: Spear",new Tuple<int, uint, uint>(Memory.ReadInt(Addresses.FakeAddress), Addresses.FakeAddress,  Addresses.SpearAmmo)},
                {"Equipment: Lightning",new Tuple<int, uint, uint>(Memory.ReadInt(Addresses.FakeAddress), Addresses.FakeAddress,  Addresses.LightningCharge)},
                {"Equipment: Good Lightning",new Tuple<int, uint, uint>(Memory.ReadInt(Addresses.FakeAddress), Addresses.FakeAddress,  Addresses.GoodLightning)},
                {"Equipment: Copper Shield",new Tuple<int, uint, uint>(Memory.ReadInt(Addresses.DC_Pickup_CopperShield), Addresses.DC_Pickup_CopperShield,  Addresses.CopperShieldAmmo)},
                {"Equipment: Silver Shield",new Tuple<int, uint, uint>(Memory.ReadInt(Addresses.FakeAddress), Addresses.FakeAddress,  Addresses.SilverShieldAmmo)},
                {"Equipment: Gold Shield",new Tuple<int, uint, uint>(Memory.ReadInt(Addresses.FakeAddress), Addresses.FakeAddress,  Addresses.GoldShieldAmmo)},
                {"Equipment: Dragon Armour",new Tuple<int, uint, uint>(Memory.ReadInt(Addresses.FakeAddress), Addresses.FakeAddress,  Addresses.DragonArmour)},
            };
        }

        public static Dictionary<string, Tuple<int, uint, uint>> GetHallOfHeroesStatuses()
        {
            return new Dictionary<string, Tuple<int, uint, uint>>
            {
                // these locations need updated.

                {"Hall of Heroes: Canny Tim 1",new Tuple<int, uint, uint>(Memory.ReadInt(Addresses.CannyTim1), Addresses.CannyTim1, Addresses.FakeAddress)},
                {"Hall of Heroes: Canny Tim 2",new Tuple<int, uint, uint>(Memory.ReadInt(Addresses.CannyTim2), Addresses.CannyTim2, Addresses.FakeAddress)},
                {"Hall of Heroes: Stanyer Iron Hewer 1",new Tuple<int, uint, uint>(Memory.ReadInt(Addresses.StanyerIronHewer1), Addresses.StanyerIronHewer1, Addresses.FakeAddress)},
                {"Hall of Heroes: Stanyer Iron Hewer 2",new Tuple<int, uint, uint>(Memory.ReadInt(Addresses.StanyerIronHewer2), Addresses.StanyerIronHewer2, Addresses.FakeAddress)},
                {"Hall of Heroes: Woden The Mighty 1",new Tuple<int, uint, uint>(Memory.ReadInt(Addresses.WodenTheMighty1), Addresses.WodenTheMighty1, Addresses.FakeAddress)},
                {"Hall of Heroes: Woden The Mighty 2",new Tuple<int, uint, uint>(Memory.ReadInt(Addresses.WodenTheMighty2), Addresses.WodenTheMighty2, Addresses.FakeAddress)},
                {"Hall of Heroes: RavenHooves The Archer 1",new Tuple<int, uint, uint>(Memory.ReadInt(Addresses.RavenHoovesTheArcher1), Addresses.RavenHoovesTheArcher1, Addresses.FakeAddress)},
                {"Hall of Heroes: RavenHooves The Archer 2",new Tuple<int, uint, uint>(Memory.ReadInt(Addresses.RavenHoovesTheArcher2), Addresses.RavenHoovesTheArcher2, Addresses.FakeAddress)},
                {"Hall of Heroes: RavenHooves The Archer 3",new Tuple<int, uint, uint>(Memory.ReadInt(Addresses.RavenHoovesTheArcher3), Addresses.RavenHoovesTheArcher3, Addresses.FakeAddress)},
                {"Hall of Heroes: RavenHooves The Archer 4",new Tuple<int, uint, uint>(Memory.ReadInt(Addresses.RavenHoovesTheArcher4), Addresses.RavenHoovesTheArcher4, Addresses.FakeAddress)},
                {"Hall of Heroes: Imanzi 1",new Tuple<int, uint, uint>(Memory.ReadInt(Addresses.Imanzi1), Addresses.Imanzi1, Addresses.FakeAddress)},
                {"Hall of Heroes: Imanzi 2",new Tuple<int, uint, uint>(Memory.ReadInt(Addresses.Imanzi2), Addresses.Imanzi2, Addresses.FakeAddress)},
                {"Hall of Heroes: Dark Steadfast 1",new Tuple<int, uint, uint>(Memory.ReadInt(Addresses.DarkSteadfast1), Addresses.DarkSteadfast1, Addresses.FakeAddress)},
                {"Hall of Heroes: Dark Steadfast 2",new Tuple<int, uint, uint>(Memory.ReadInt(Addresses.DarkSteadfast2), Addresses.DarkSteadfast2, Addresses.FakeAddress)},
                {"Hall of Heroes: Karl Stungard 1",new Tuple<int, uint, uint>(Memory.ReadInt(Addresses.KarlStungard1), Addresses.KarlStungard1, Addresses.FakeAddress)},
                {"Hall of Heroes: Karl Stungard 2",new Tuple<int, uint, uint>(Memory.ReadInt(Addresses.KarlStungard2), Addresses.KarlStungard2, Addresses.FakeAddress)},
                {"Hall of Heroes: Bloodmonath Skill Cleaver 1",new Tuple<int, uint, uint>(Memory.ReadInt(Addresses.BloodmonathSkillCleaver1), Addresses.BloodmonathSkillCleaver1, Addresses.FakeAddress)},
                {"Hall of Heroes: Bloodmonath Skill Cleaver 2",new Tuple<int, uint, uint>(Memory.ReadInt(Addresses.BloodmonathSkillCleaver2), Addresses.BloodmonathSkillCleaver2, Addresses.FakeAddress)},
                {"Hall of Heroes: Megwynne Stormbinder 1",new Tuple<int, uint, uint>(Memory.ReadInt(Addresses.MegwynneStormbinder1), Addresses.MegwynneStormbinder1, Addresses.FakeAddress)},
                {"Hall of Heroes: Megwynne Stormbinder 2",new Tuple<int, uint, uint>(Memory.ReadInt(Addresses.MegwynneStormbinder2), Addresses.MegwynneStormbinder2, Addresses.FakeAddress)},
            };
        }

        public static Dictionary<string, Tuple<int, uint, uint>> GetRuneLocationStatuses()
        {
            return new Dictionary<string, Tuple<int, uint, uint>>
            {
                // uses fake locations at the moment

                // Chaos Runes
                {"Chaos Rune: The Graveyard", new Tuple<int, uint, uint>(Memory.ReadInt(Addresses.FakeAddress), Addresses.FakeAddress, Addresses.FakeAddress)},
                {"Chaos Rune: The Hilltop Mausoleum", new Tuple<int, uint, uint>(Memory.ReadInt(Addresses.FakeAddress), Addresses.FakeAddress, Addresses.FakeAddress)},
                {"Chaos Rune: Scarecrow Fields", new Tuple<int, uint, uint>(Memory.ReadInt(Addresses.FakeAddress), Addresses.FakeAddress, Addresses.FakeAddress)},
                {"Chaos Rune: The Lake", new Tuple<int, uint, uint>(Memory.ReadInt(Addresses.FakeAddress), Addresses.FakeAddress, Addresses.FakeAddress)},
                {"Chaos Rune: Pumpkin Gorge", new Tuple<int, uint, uint>(Memory.ReadInt(Addresses.FakeAddress), Addresses.FakeAddress, Addresses.FakeAddress)},
                {"Chaos Rune: Sleeping Village", new Tuple<int, uint, uint>(Memory.ReadInt(Addresses.FakeAddress), Addresses.FakeAddress, Addresses.FakeAddress)},
                {"Chaos Rune: Pools of the Ancient Dead", new Tuple<int, uint, uint>(Memory.ReadInt(Addresses.FakeAddress), Addresses.FakeAddress, Addresses.FakeAddress)},
                {"Chaos Rune: Asylum Grounds", new Tuple<int, uint, uint>(Memory.ReadInt(Addresses.FakeAddress), Addresses.FakeAddress, Addresses.FakeAddress)},
                {"Chaos Rune: The Haunted Ruins", new Tuple<int, uint, uint>(Memory.ReadInt(Addresses.FakeAddress), Addresses.FakeAddress, Addresses.FakeAddress)},
                {"Chaos Rune: Ghost Ship", new Tuple<int, uint, uint>(Memory.ReadInt(Addresses.FakeAddress), Addresses.FakeAddress, Addresses.FakeAddress)},
                {"Chaos Rune: The Time Device", new Tuple<int, uint, uint>(Memory.ReadInt(Addresses.FakeAddress), Addresses.FakeAddress, Addresses.FakeAddress)},

                // Earth Runes
                {"Earth Rune: The Graveyard", new Tuple<int, uint, uint>(Memory.ReadInt(Addresses.FakeAddress), Addresses.FakeAddress, Addresses.FakeAddress)},
                {"Earth Rune: The Hilltop Mausoleum", new Tuple<int, uint, uint>(Memory.ReadInt(Addresses.FakeAddress), Addresses.FakeAddress, Addresses.FakeAddress)},
                {"Earth Rune: Scarecrow Fields", new Tuple<int, uint, uint>(Memory.ReadInt(Addresses.FakeAddress), Addresses.FakeAddress, Addresses.FakeAddress)},
                {"Earth Rune: The Crystal Caves", new Tuple<int, uint, uint>(Memory.ReadInt(Addresses.FakeAddress), Addresses.FakeAddress, Addresses.FakeAddress)},
                {"Earth Rune: The Lake", new Tuple<int, uint, uint>(Memory.ReadInt(Addresses.FakeAddress), Addresses.FakeAddress, Addresses.FakeAddress)},
                {"Earth Rune: Pumpkin Gorge", new Tuple<int, uint, uint>(Memory.ReadInt(Addresses.FakeAddress), Addresses.FakeAddress, Addresses.FakeAddress)},
                {"Earth Rune: Sleeping Village", new Tuple<int, uint, uint>(Memory.ReadInt(Addresses.FakeAddress), Addresses.FakeAddress, Addresses.FakeAddress)},
                {"Earth Rune: Inside the Asylum", new Tuple<int, uint, uint>(Memory.ReadInt(Addresses.FakeAddress), Addresses.FakeAddress, Addresses.FakeAddress)},
                {"Earth Rune: Enchanted Earth", new Tuple<int, uint, uint>(Memory.ReadInt(Addresses.FakeAddress), Addresses.FakeAddress, Addresses.FakeAddress)},
                {"Earth Rune: The Haunted Ruins", new Tuple<int, uint, uint>(Memory.ReadInt(Addresses.FakeAddress), Addresses.FakeAddress, Addresses.FakeAddress)},
                {"Earth Rune: The Entrance Hall", new Tuple<int, uint, uint>(Memory.ReadInt(Addresses.FakeAddress), Addresses.FakeAddress, Addresses.FakeAddress)},
                {"Earth Rune: The Time Device", new Tuple<int, uint, uint>(Memory.ReadInt(Addresses.FakeAddress), Addresses.FakeAddress, Addresses.FakeAddress)},

                // Moon Runes
                {"Moon Rune: The Hilltop Mausoleum", new Tuple<int, uint, uint>(Memory.ReadInt(Addresses.FakeAddress), Addresses.FakeAddress, Addresses.FakeAddress)},
                {"Moon Rune: Scarecrow Fields", new Tuple<int, uint, uint>(Memory.ReadInt(Addresses.FakeAddress), Addresses.FakeAddress, Addresses.FakeAddress)},
                {"Moon Rune: Pumpkin Gorge", new Tuple<int, uint, uint>(Memory.ReadInt(Addresses.FakeAddress), Addresses.FakeAddress, Addresses.FakeAddress)},
                {"Moon Rune: Ghost Ship", new Tuple<int, uint, uint>(Memory.ReadInt(Addresses.FakeAddress), Addresses.FakeAddress, Addresses.FakeAddress)},
                {"Moon Rune: The Time Device", new Tuple<int, uint, uint>(Memory.ReadInt(Addresses.FakeAddress), Addresses.FakeAddress, Addresses.FakeAddress)},

                // Star Runes
                {"Star Rune: Return to the Graveyard", new Tuple<int, uint, uint>(Memory.ReadInt(Addresses.DC_Pickup_StarRune), Addresses.DC_Pickup_StarRune, Addresses.FakeAddress)},
                {"Star Rune: Dan's Crypt", new Tuple<int, uint, uint>(Memory.ReadInt(Addresses.FakeAddress), Addresses.FakeAddress, Addresses.FakeAddress)},
                {"Star Rune: The Crystal Caves", new Tuple<int, uint, uint>(Memory.ReadInt(Addresses.FakeAddress), Addresses.FakeAddress, Addresses.FakeAddress)},
                {"Star Rune: The Lake", new Tuple<int, uint, uint>(Memory.ReadInt(Addresses.FakeAddress), Addresses.FakeAddress, Addresses.FakeAddress)},
                {"Star Rune: Enchanted Earth", new Tuple<int, uint, uint>(Memory.ReadInt(Addresses.FakeAddress), Addresses.FakeAddress, Addresses.FakeAddress)},
                {"Star Rune: The Gallows Gauntlet", new Tuple<int, uint, uint>(Memory.ReadInt(Addresses.FakeAddress), Addresses.FakeAddress, Addresses.FakeAddress)},
                {"Star Rune: Ghost Ship", new Tuple<int, uint, uint>(Memory.ReadInt(Addresses.FakeAddress), Addresses.FakeAddress, Addresses.FakeAddress)},

                // Time Runes
                {"Time Rune: The Lake", new Tuple<int, uint, uint>(Memory.ReadInt(Addresses.FakeAddress), Addresses.FakeAddress, Addresses.FakeAddress)},
                {"Time Rune: Pumpkin Gorge", new Tuple<int, uint, uint>(Memory.ReadInt(Addresses.FakeAddress), Addresses.FakeAddress, Addresses.FakeAddress)},
                {"Time Rune: The Time Device", new Tuple<int, uint, uint>(Memory.ReadInt(Addresses.FakeAddress), Addresses.FakeAddress, Addresses.FakeAddress)}
            };
        }


        // needs renamed. This will basically pull out everything related to player state and give a way to read it for player status updates
        public static Dictionary<string, Tuple<int, uint, uint>> GetAllLocationsAndAddresses()
        {

            Dictionary<string, Tuple<int, uint, uint>> allStatuses = new Dictionary<string, Tuple<int, uint, uint>>();

            // Helper function to safely add dictionary entries
            Action<Dictionary<string, Tuple<int, uint, uint>>> addDictionary = (dict) =>
            {
                foreach (var entry in dict)
                {
                    if (!allStatuses.ContainsKey(entry.Key)) // Avoid adding duplicate keys if any exist
                    {
                        allStatuses.Add(entry.Key, entry.Value);
                    }
                    else
                    {
                        // Handle duplicate keys, e.g., log a warning or overwrite
                        // For now, we'll just ignore if a key already exists.
                        // If you want to overwrite, you can do: allStatuses[entry.Key] = entry.Value;
                        Console.WriteLine($"Warning: Duplicate key '{entry.Key}' found. Skipping addition.");
                    }
                }
            };

            addDictionary(GetSkillStatuses());
            addDictionary(GetWeaponInventoryLocationStatuses());
            addDictionary(GetHallOfHeroesStatuses());
            //addDictionary(GetLifeBottleLocationStatuses());
            addDictionary(GetKeyItemInventoryStatuses());
            addDictionary(GetRuneLocationStatuses());

            return allStatuses;
        }



        public static List<Location> BuildLocationList()
        {
            int baseId = 99251000;
            List<Location> locations = new List<Location>();

            List<GenericItemsData> levels = GetLevelData();
            List<GenericItemsData> chalices = GetChaliceData();
            //List<GenericItemsData> hallOfHeroesVisits = GetHallOfHeroesData();
            List<GenericItemsData> skills = GetSkillData();
            List<GenericItemsData> weaponInventorySlots = GetWeaponInventoryData();
            List<GenericItemsData> weaponDrops = GetWeaponDropLocationsData();
            List<GenericItemsData> bottles = GetLifeBottleLocationsData();
            List<GenericItemsData> keys = GetKeyItemsData();
            List<GenericItemsData> runes = GetRuneLocationsData();

            List<GenericItemsData> allLevelLocations = new List<GenericItemsData>();

            // Level Locations
            List<GenericItemsData> dcLocations = GetDansCryptData();
            List<GenericItemsData> tgLocations = GetTheGraveyardData();
            List<GenericItemsData> chLocations = GetCemeteryHillData();
            List<GenericItemsData> hmLocations = GetHilltopMausoleumData();
            List<GenericItemsData> rtgLocations = GetReturnToTheGraveyardData();
            List<GenericItemsData> sfLocations = GetScarecrowFieldsData();
            List<GenericItemsData> ahLocations = GetAnthillData();
            List<GenericItemsData> eeLocations = GetEnchantedEarthData();
            List<GenericItemsData> tsvLocations = GetTheSleepingVillageData();
            List<GenericItemsData> padLocations = GetPoolsOfTheAncientDeadData();
            List<GenericItemsData> tlLocations = GetTheLakeData();
            List<GenericItemsData> ccLocations = GetTheCrystalCavesData();
            List<GenericItemsData> ggLocations = GetGallowsGauntletData();
            List<GenericItemsData> agLocations = GetTheAsylumGroundsData();
            List<GenericItemsData> iaLocations = GetInsideTheAsylumData();
            List<GenericItemsData> pgLocations = GetPumpkinGorgeData();
            List<GenericItemsData> psLocations = GetPumpkinServantData();
            List<GenericItemsData> hrLocations = GetTheHauntedRuinsData();
            List<GenericItemsData> gsLocations = GetTheGhostShipData();
            List<GenericItemsData> tdLocations = GetTheTimeDeviceData();
            // the entrance hall is just the chalice
            List<GenericItemsData> zlLocations = GetZaroksLairData();



            allLevelLocations.AddRange(dcLocations);
            allLevelLocations.AddRange(hmLocations);
            allLevelLocations.AddRange(rtgLocations);
            allLevelLocations.AddRange(sfLocations);
            allLevelLocations.AddRange(ahLocations);
            allLevelLocations.AddRange(eeLocations);
            allLevelLocations.AddRange(tsvLocations);
            allLevelLocations.AddRange(padLocations);
            allLevelLocations.AddRange(tlLocations);
            allLevelLocations.AddRange(ccLocations);
            allLevelLocations.AddRange(ggLocations);
            allLevelLocations.AddRange(agLocations);
            allLevelLocations.AddRange(iaLocations);
            allLevelLocations.AddRange(pgLocations);
            allLevelLocations.AddRange(psLocations);
            allLevelLocations.AddRange(hrLocations);
            allLevelLocations.AddRange(gsLocations);
            allLevelLocations.AddRange(tdLocations);
            allLevelLocations.AddRange(zlLocations);

            var skillDict = GetSkillStatuses();
            var hallOfHeroesDict = GetHallOfHeroesStatuses();
            var weaponLocationDict = GetWeaponInventoryLocationStatuses();
            //var bottleLocationDict = GetLifeBottleLocationStatuses();
            var keyItemDict = GetKeyItemInventoryStatuses();
            var runeLocationDict = GetRuneLocationStatuses();



            // level and Chalice locations

            foreach (var level in levels)
            {
                if (level.Name.Contains("Cleared"))
                {
                    {
                        Location location = new Location()
                        {
                            Name = level.Name,
                            Address = level.Address,
                            Id = baseId + level.Id,
                            CheckType = LocationCheckType.Byte,
                            CompareType = LocationCheckCompareType.GreaterThan,
                            CheckValue = "16"
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
                            Address = chalice.Address,
                            Id = baseId + chalice.Id,
                            CheckType = LocationCheckType.Byte,
                            CompareType = LocationCheckCompareType.Match,
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
                        Id = baseId + skill.Id,
                        CheckType = LocationCheckType.Byte,
                        CompareType = LocationCheckCompareType.Match,
                        CheckValue = skill.Name = "1"
                    };

                    locations.Add(location);

                };
            }
            //foreach (var hallOfHeroesVisit in hallOfHeroesVisits)
            //{
            //    {
            //        Location location = new Location()
            //        {
            //            Name = hallOfHeroesVisit.Name,
            //            Address = hallOfHeroesDict[hallOfHeroesVisit.Name].Item2,
            //            Id = baseId + hallOfHeroesVisit.Id,
            //            CheckType = LocationCheckType.Byte,
            //            CompareType = LocationCheckCompareType.GreaterThan,
            //            CheckValue = hallOfHeroesVisit.Name = "0"
            //        };

            //        locations.Add(location);

            //    };
            //}
            //foreach (var weaponDropLocation in weaponDrops)
            //{
            //    {
            //        Location location = new Location()
            //        {
            //            Name = weaponDropLocation.Name,
            //            Address = weaponDropLocation.Address,
            //            Id = baseId + weaponDropLocation.Id,
            //            CheckType = LocationCheckType.Int,
            //            CompareType = LocationCheckCompareType.Match,
            //            CheckValue = weaponDropLocation.Check // Pickup Number
            //        };

            //        locations.Add(location);

            //    };
            //}
            //foreach (var bottleLocation in bottles)
            //{
            //    {
            //        Location location = new Location()
            //        {
            //            Name = bottleLocation.Name,
            //            Address = bottleLocationDict[bottleLocation.Name].Item2,
            //            Id = baseId + bottleLocation.Id,
            //            CheckType = LocationCheckType.Byte,
            //            CompareType = LocationCheckCompareType.GreaterThan,
            //            CheckValue = bottleLocation.Name = "999" // fake number as this is just a dummy location for now
            //        };

            //        locations.Add(location);

            //    };
            //}
            //foreach (var keyItem in keys)
            //{
            //    {
            //        Location location = new Location()
            //        {
            //            Name = keyItem.Name,
            //            Address = keyItemDict[keyItem.Name].Item2,
            //            Id = baseId + keyItem.Id,
            //            CheckType = LocationCheckType.Byte,
            //            CompareType = LocationCheckCompareType.GreaterThan,
            //            CheckValue = keyItem.Name = "999" // fake number as this is just a dummy location for now
            //        };

            //        locations.Add(location);

            //    };
            //}
            //foreach (var runeLocation in runes)
            //{
            //    {
            //        Location location = new Location()
            //        {
            //            Name = runeLocation.Name,
            //            Address = runeLocationDict[runeLocation.Name].Item2,
            //            Id = baseId + runeLocation.Id,
            //            CheckType = LocationCheckType.Byte,
            //            CompareType = LocationCheckCompareType.GreaterThan,
            //            CheckValue = runeLocation.Name = "999" // fake number as this is just a dummy location for now
            //        };

            //        locations.Add(location);

            //    };
            //}

            // starting fresh here.

            // return dans crypt locations
            foreach (var levelLocation in allLevelLocations)
            {
                {
                    Location location = new Location()
                    {
                        Name = levelLocation.Name,
                        Address = levelLocation.Address,
                        Id = baseId + levelLocation.Id,
                        CheckType = LocationCheckType.Int,
                        CompareType = LocationCheckCompareType.Match,
                        CheckValue = levelLocation.Check
                    };

                    locations.Add(location);

                };
            }

            foreach (var location in locations)
            {
                Console.WriteLine($"{location.Name} | {location.Id} | {location.Address:X} {location.CheckValue:X}");
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

        private static List<GenericItemsData> GetDansCryptData()
        {
            List<GenericItemsData> dcLocations = new List<GenericItemsData>() {
                new GenericItemsData("Life Bottle: Dan's Crypt", 62, Addresses.DC_Pickup_LifeBottle, "32896"),
                new GenericItemsData("Life Bottle: Dan's Crypt - Behind Wall",65, Addresses.DC_Pickup_LifeBottleWall, "32896"),
                new GenericItemsData("Equipment: Small Sword in DC", 138, Addresses.DC_Pickup_Shortsword, "32896"),
                new GenericItemsData("Star Rune: Dan's Crypt", 129, Addresses.DC_Pickup_StarRune, "32896"),
                new GenericItemsData("Gold Coins: Over the water", 139, Addresses.DC_Pickup_GoldCoinsOverWater, "32896"),
                new GenericItemsData("Equipment: Copper Shield in DC", 140, Addresses.DC_Pickup_CopperShield, "32896"), // <--- This one doesn't work (chest)
                new GenericItemsData("Equipment: Daggers in DC", 141, Addresses.DC_Pickup_Daggers, "32896"),
                new GenericItemsData("Gold Coins: Behind Wall in Crypt - Left", 142, Addresses.DC_Pickup_GoldCoinsBehindWallLeft, "32896"),
                new GenericItemsData("Gold Coins: Behind Wall in Crypt - Right", 143, Addresses.DC_Pickup_GoldCoinsBehindWallRight, "32896"),
            };
            return dcLocations;
        }

        private static List<GenericItemsData> GetTheGraveyardData()
        {
            List<GenericItemsData> tgLocations = new List<GenericItemsData>() {
                new GenericItemsData("Gold Coins: Near Chaos Rune", 144, Addresses.TG_Pickup_GoldCoinsNearChaosRune, "32896"),
                new GenericItemsData("Gold Coins: Life Potion Left Chest", 145, Addresses.TG_Pickup_GoldCoinsLifePotionLeftChest, "32896"),
                new GenericItemsData("Gold Coins: Life Potion Right Chest", 146, Addresses.TG_Pickup_GoldCoinsLifePotionRightChest, "32896"),
                new GenericItemsData("Gold Coins: Shop Chest", 147, Addresses.TG_Pickup_GoldCoinsShopChest, "32896"),
                new GenericItemsData("Gold Coins: Bag Near Hill Fountain", 148, Addresses.TG_Pickup_GoldCoinsBagNearHillFountain, "32896"),
                new GenericItemsData("Equipment: Copper Shield in GY", 149, Addresses.TG_Pickup_CopperShield, "32896"),
                new GenericItemsData("Life Bottle: The Graveyard", 63, Addresses.TG_Pickup_LifePotion, "32896"),

            };
            return tgLocations;
        }

        private static List<GenericItemsData> GetCemeteryHillData()
        {
            List<GenericItemsData> chLocations = new List<GenericItemsData>() {
                new GenericItemsData("Gold Coins: Near Boulder Entrance", 150, Addresses.CH_Pickup_GoldCoinsNearBoulderEntrance, "32896"),
                new GenericItemsData("Energy Vial: Near Shop", 151, Addresses.CH_Pickup_EnergyVialNearShop, "32896"),
                new GenericItemsData("Equipment: Copper Shield in CH 1", 152, Addresses.CH_Pickup_CopperShield1stOnHill, "32896"),
                new GenericItemsData("Gold Coins: Up Hill 1", 153, Addresses.CH_Pickup_GoldCoinsUpHill1, "32896"),
                new GenericItemsData("Equipment: Copper Shield in CH 2", 154, Addresses.CH_Pickup_CopperShield2ndOnHill, "32896"),
                new GenericItemsData("Gold Coins: Up Hill 2", 155, Addresses.CH_Pickup_GoldCoinsUpHill2, "32896"),
                new GenericItemsData("Equipment: Copper Shield in CH 3", 156, Addresses.CH_Pickup_CopperShield3rdOnHill, "32896"),
                new GenericItemsData("Gold Coins: Chest at Exit", 157, Addresses.CH_Pickup_GoldCoinsChestAtExit, "32896"),
                new GenericItemsData("Equipment: Club in CH", 158, Addresses.CH_Pickup_Club, "32896"),
                new GenericItemsData("Equipment: Copper Shield in Arena", 159, Addresses.CH_Pickup_CopperShieldArena, "32896"),
                new GenericItemsData("Energy Vial: Arena", 160, Addresses.CH_Pickup_EnergyVialArena, "32896"),
                new GenericItemsData("Gold Coins: Chest in Arena", 161, Addresses.CH_Pickup_GoldCoinsChestInArena, "32896"),
            };
            return chLocations;
        }

        private static List<GenericItemsData> GetHilltopMausoleumData()
        {
            List<GenericItemsData> hmLocations = new List<GenericItemsData>() {
                new GenericItemsData("Energy Vial: Right Coffin", 162, Addresses.HM_Pickup_EnergyVialRightCoffin, "32896"),
                new GenericItemsData("Gold Coins: Left Coffin", 163, Addresses.HM_Pickup_GoldCoinsLeftCoffin, "32896"),
                new GenericItemsData("Equipment: Club in HM Broken Benches", 164, Addresses.HM_Pickup_ClubBrokenBenches, "32896"),
                new GenericItemsData("Energy Vial: Near Rune - Left Ramp", 165, Addresses.HM_Pickup_EnergyVialNearRuneLeftRamp, "32896"),
                new GenericItemsData("Gold Coins: After Earth Rune Door", 166, Addresses.HM_Pickup_GoldCoinsAfterEarthRuneDoor, "32896"),
                new GenericItemsData("Energy Vial: Phantom of the Opera - Left", 167, Addresses.HM_Pickup_EnergyVialPhantomOfTheOperaLeft, "32896"),
                new GenericItemsData("Energy Vial: Phantom of the Opera - Right", 168, Addresses.HM_Pickup_EnergyVialPhantomOfTheOperaRight, "32896"),
                new GenericItemsData("Gold Coins: Chest in Moon Room", 169, Addresses.HM_Pickup_GoldCoinsChestInMoonRoom, "32896"),
                new GenericItemsData("Energy Vial: Moon Room", 170, Addresses.HM_Pickup_EnergyVialMoonRoom, "32896"),
                new GenericItemsData("Equipment: Daggers in HM Block Puzzle", 171, Addresses.HM_Pickup_DaggersBlockPuzzle, "32896"),
                new GenericItemsData("Equipment: Copper Shield in HM Block Puzzle", 172, Addresses.HM_Pickup_CopperShieldBlockPuzzle, "32896"),
                new GenericItemsData("Gold Chest: Phantom of the Opera 1", 173, Addresses.HM_Pickup_GoldChestPhantomOfTheOpera1, "32896"),
                new GenericItemsData("Gold Chest: Phantom of the Opera 2", 174, Addresses.HM_Pickup_GoldChestPhantomOfTheOpera2, "32896"),
                new GenericItemsData("Gold Chest: Phantom of the Opera 3", 175, Addresses.HM_Pickup_GoldChestPhantomOfTheOpera3, "32896"),
            };
            return hmLocations;
        }

        private static List<GenericItemsData> GetReturnToTheGraveyardData()
        {
            List<GenericItemsData> rtgLocations = new List<GenericItemsData>() {
                new GenericItemsData("Gold Coins: Chest in Coffin Area 1", 176, Addresses.RTG_Pickup_GoldCoinsChestInCoffinArea1, "32896"),
                new GenericItemsData("Energy Vial: Coffin Area West", 177, Addresses.RTG_Pickup_EnergyVialCoffinAreaWest, "32896"),
                new GenericItemsData("Gold Coins: Chest in Coffin Area 2", 178, Addresses.RTG_Pickup_GoldCoinsChestInCoffinArea2, "32896"),
                new GenericItemsData("Gold Coins: Chest in Coffin Area 3", 179, Addresses.RTG_Pickup_GoldCoinsChestInCoffinArea3, "32896"),
                new GenericItemsData("Gold Coins: Chest in Coffin Area 4", 180, Addresses.RTG_Pickup_GoldCoinsChestInCoffinArea4, "32896"),
                new GenericItemsData("Energy Vial: Coffin Area East", 181, Addresses.RTG_Pickup_EnergyVialCoffinAreaEast, "32896"),
                new GenericItemsData("Gold Coins: Chest in Coffin Area 5", 182, Addresses.RTG_Pickup_GoldCoinsChestInCoffinArea5, "32896"),
                new GenericItemsData("Gold Coins: Bag above Coffin Area", 183, Addresses.RTG_Pickup_GoldCoinsBagAboveCoffinArea, "32896"),
                new GenericItemsData("Gold Coins: Bag after Bridge", 184, Addresses.RTG_Pickup_GoldCoinsBagAfterBridge, "32896"),
                new GenericItemsData("Energy Vial: Below Shop", 185, Addresses.RTG_Pickup_EnergyVialBelowShop, "32896"),
                new GenericItemsData("Gold Coins: Bag at Shop", 186, Addresses.RTG_Pickup_GoldCoinsBagAtShop, "32896"),
                new GenericItemsData("Equipment: Silver Shield in RG Chest At Shop", 187, Addresses.RTG_Pickup_SilverShieldChestAtShop, "32896"),
                new GenericItemsData("Gold Coins: Bag at Closed Gate", 188, Addresses.RTG_Pickup_GoldCoinsBagAtClosedGate, "32896"),
                new GenericItemsData("Gold Coins: Chest on Island", 189, Addresses.RTG_Pickup_GoldCoinsChestOnIsland, "32896"),
                new GenericItemsData("Gold Coins: Undertakers Entrance", 190, Addresses.RTG_Pickup_GoldCoinsUndertakersEntrance, "32896"),
                new GenericItemsData("Energy Vial: Undertakers Entrance", 191, Addresses.RTG_Pickup_EnergyVialUndertakersEntrance, "32896"),
                new GenericItemsData("Energy Vial: Cliffs Right", 192, Addresses.RTG_Pickup_EnergyVialCliffsRight, "32896"),
                new GenericItemsData("Gold Coins: Cliffs Left", 193, Addresses.RTG_Pickup_GoldCoinsCliffsLeft, "32896"),
                new GenericItemsData("Energy Vial: Cliffs Left", 194, Addresses.RTG_Pickup_EnergyVialCliffsLeft, "32896"),
            };
            return rtgLocations;
        }

        private static List<GenericItemsData> GetScarecrowFieldsData()
        {
            List<GenericItemsData> sfLocations = new List<GenericItemsData>() {
                new GenericItemsData("Gold Coins: Haystack at Beginning", 195, Addresses.SF_Pickup_GoldCoinsHaystackAtBeginning, "32896"),
                new GenericItemsData("Gold Coins: Chest in Haystack near Moon Door", 196, Addresses.SF_Pickup_GoldCoinsChestInHaystackNearMoonDoor, "32896"),
                new GenericItemsData("Gold Coins: Left of fire near Moon Door", 197, Addresses.SF_Pickup_GoldCoinsLeftOfFireNearMoonDoor, "32896"),
                new GenericItemsData("Energy Vial: Right of fire near Moon Door", 198, Addresses.SF_Pickup_EnergyVialRightOfFireNearMoonDoor, "32896"),
                new GenericItemsData("Equipment: Club in SF Inside Hut", 199, Addresses.SF_Pickup_ClubInsideHut, "32896"),
                new GenericItemsData("Equipment: Silver Shield in SF Behind Windmill", 200, Addresses.SF_Pickup_SilverShieldBehindWindmill, "32896"),
                new GenericItemsData("Gold Coins: Bag in the Barn", 201, Addresses.SF_Pickup_GoldCoinsBagInTheBarn, "32896"),
                new GenericItemsData("Equipment: Copper Shield in SF - Chest In the Barn", 202, Addresses.SF_Pickup_CopperShieldChestInTheBarn, "32896"),
                new GenericItemsData("Gold Coins: Cornfield Square near Barn", 203, Addresses.SF_Pickup_GoldCoinsCornfieldSquareNearBarn, "32896"),
                new GenericItemsData("Gold Coins: Cornfield Path 1", 204, Addresses.SF_Pickup_GoldCoinsCornfieldPath1, "32896"),
                new GenericItemsData("Energy Vial: Cornfield Path", 205, Addresses.SF_Pickup_EnergyVialCornfieldPath, "32896"),
                new GenericItemsData("Gold Coins: Chest Under Haybail", 206, Addresses.SF_Pickup_GoldCoinsChestUnderHaybail, "32896"),
                new GenericItemsData("Gold Coins: Bag under Barn Haybail", 207, Addresses.SF_Pickup_GoldCoinsBagUnderBarnHaybail, "32896"),
                new GenericItemsData("Gold Coins: Bag in the Press", 208, Addresses.SF_Pickup_GoldCoinsBagInThePress, "32896"),
                new GenericItemsData("Gold Coins: Bag in the Spinner", 209, Addresses.SF_Pickup_GoldCoinsBagInTheSpinner, "32896"),
                new GenericItemsData("Gold Coins: Chest next to Harvester Part", 210, Addresses.SF_Pickup_GoldCoinsChestNextToHarvesterPart, "32896"),
                new GenericItemsData("Gold Coins: Chest Next to Chalice", 211, Addresses.SF_Pickup_GoldCoinsChestNextToChalice, "32896"),
                new GenericItemsData("Life Bottle: Scarecrow Fields", 66, Addresses.SF_Pickup_LifePotion, "32896"),
            };
            return sfLocations;
        }

        private static List<GenericItemsData> GetAnthillData()
        {
            List<GenericItemsData> ahLocations = new List<GenericItemsData>() {
                new GenericItemsData("Equipment: Club in AH Chest at Barrier", 212, Addresses.TA_Pickup_ClubChestAtBarrier, "32896"),
                new GenericItemsData("Gold Coins: Chest at Barrier Fairy", 213, Addresses.TA_Pickup_GoldCoinsChestAtBarrierFairy, "32896"),
                new GenericItemsData("Gold Coins: Maggot at Amber 2", 214, Addresses.TA_Pickup_GoldCoinsMaggotAtAmber2, "32896"),
                new GenericItemsData("Gold Coins: Maggot after Amber 2", 215, Addresses.TA_Pickup_GoldCoinsMaggotAfterAmber2, "32896"),
                new GenericItemsData("Energy Vial: Before Fairy 1", 216, Addresses.TA_Pickup_EnergyVialBeforeFairy1, "32896"),
                new GenericItemsData("Energy Vial: After Amber 2", 217, Addresses.TA_Pickup_EnergyVialAfterAmber2, "32896"),
                new GenericItemsData("Energy Vial: Fairy 2 Room Center", 218, Addresses.TA_Pickup_EnergyVialFairy2RoomCenter, "32896"),
                new GenericItemsData("Gold Coins: Fairy 2 Room Center", 219, Addresses.TA_Pickup_GoldCoinsFairy2RoomCenter, "32896"),
                new GenericItemsData("Gold Coins: Fairy 2 Room Maggot", 220, Addresses.TA_Pickup_GoldCoinsFairy2RoomMaggot, "32896"),
                new GenericItemsData("Gold Coins: Maggots before Amber 4", 221, Addresses.TA_Pickup_GoldCoinsMaggotsBeforeAmber4, "32896"),
                new GenericItemsData("Gold Coins: Maggots at Amber 5", 222, Addresses.TA_Pickup_GoldCoinsMaggotsAtAmber5, "32896"),
                new GenericItemsData("Energy Vial: Fairy 3", 223, Addresses.TA_Pickup_EnergyVialFairy3, "32896"),
                new GenericItemsData("Gold Coins: Maggots at Amber 7 - 1", 224, Addresses.TA_Pickup_GoldCoinsMaggotsAtAmber7_1, "32896"),
                new GenericItemsData("Gold Coins: Maggot in nest at Amber 7", 225, Addresses.TA_Pickup_GoldCoinsMaggotInNestAtAmber7, "32896"),
                new GenericItemsData("Gold Coins: Maggot in Nest", 226, Addresses.TA_Pickup_GoldCoinsMaggotInNest, "32896"),
                new GenericItemsData("Energy Vial: Birthing room exit", 227, Addresses.TA_Pickup_EnergyVialBirthingRoomExit, "32896"),
                new GenericItemsData("Gold Coins: Maggot after Fairy 4", 228, Addresses.TA_Pickup_GoldCoinsMaggotAfterFairy4, "32896"),
                new GenericItemsData("Gold Coins: Maggot after Fairy 4 in Nest", 229, Addresses.TA_Pickup_GoldCoinsMaggotAfterFairy4InNest, "32896"),
                new GenericItemsData("Gold Coins: Maggot at Fairy 5", 230, Addresses.TA_Pickup_GoldCoinsMaggotAtFairy5, "32896"),
                new GenericItemsData("Gold Coins: Maggot near Amber 9", 231, Addresses.TA_Pickup_GoldCoinsMaggotNearAmber9, "32896"),
                new GenericItemsData("Gold Coins: Maggot near Shop", 232, Addresses.TA_Pickup_GoldCoinsMaggotNearShop, "32896"),
                // Note: "Equipment: Chicken Drumsticks in TA" (ID 233) has no direct mapping in the provided address list.
                // Assuming it might be a generic "Pickup_ChickenDrumsticks" or similar if one exists.
                // For now, it's excluded as per the provided address list, similar to "CH_Pickup_WitchTalisman" and "CH_Pickup_Chalice" in the example output.
            };
            return ahLocations;
        }

        private static List<GenericItemsData> GetEnchantedEarthData()
        {
            List<GenericItemsData> eeLocations = new List<GenericItemsData>() {
                new GenericItemsData("Gold Coins: Bag Near Tree Hollow", 234, Addresses.EE_Pickup_GoldCoinsBagNearTreeHollow, "32896"),
                new GenericItemsData("Gold Coins: Bag Behind Big Tree", 235, Addresses.EE_Pickup_GoldCoinsBagBehindBigTree, "32896"),
                new GenericItemsData("Gold Coins: Chest In Egg", 236, Addresses.EE_Pickup_GoldCoinsChestInEgg, "32896"),
                new GenericItemsData("Equipment: Copper Shield in EE in Egg", 237, Addresses.EE_Pickup_CopperShieldInEgg, "32896"),
                new GenericItemsData("Gold Coins: Bag at Cave Entrance", 238, Addresses.EE_Pickup_GoldCoinsBagAtCaveEntrance, "32896"),
                new GenericItemsData("Energy Vial: Shadow Talisman Cave", 239, Addresses.EE_Pickup_EnergyVialShadowTalismanCave, "32896"),
                new GenericItemsData("Gold Coins:Chest Near Barrier", 240, Addresses.EE_Pickup_GoldCoinsChestNearBarrier, "32896"),
                new GenericItemsData("Gold Coins: Chest Left of Fountain", 241, Addresses.EE_Pickup_GoldCoinsChestLeftOfFountain, "32896"),
                new GenericItemsData("Gold Coins: Chest Top of Fountain", 242, Addresses.EE_Pickup_GoldCoinsChestTopOfFountain, "32896"),
                new GenericItemsData("Gold Coins: Chest Right of Fountain", 243, Addresses.EE_Pickup_GoldCoinsChestRightOfFountain, "32896"),
                new GenericItemsData("Energy Vial: Left of Tree Drop", 244, Addresses.EE_Pickup_EnergyVialLeftOfTreeDrop, "32896"),
                new GenericItemsData("Energy Vial: Right of Tree Drop", 245, Addresses.EE_Pickup_EnergyVialRightOfTreeDrop, "32896"),
            };
            return eeLocations;
        }

        private static List<GenericItemsData> GetTheSleepingVillageData()
        {
            List<GenericItemsData> tsvLocations = new List<GenericItemsData>() {
                new GenericItemsData("Gold Coins: Bag in Left Barrel at Blacksmith", 246, Addresses.TSV_Pickup_GoldCoinsBagInLeftBarrelAtBlacksmith, "32896"),
                new GenericItemsData("Gold Coins: Bag in Right Barrel at Blacksmith", 247, Addresses.TSV_Pickup_GoldCoinsBagInRightBarrelAtBlacksmith, "32896"),
                new GenericItemsData("Equipment: Silver Shield in SV in Blacksmiths", 248, Addresses.TSV_Pickup_SilverShieldInBlacksmiths, "32896"),
                new GenericItemsData("Gold Coins: Bag at Pond", 249, Addresses.TSV_Pickup_GoldCoinsBagAtPond, "32896"),
                new GenericItemsData("Energy Vial: At Pond", 250, Addresses.TSV_Pickup_EnergyVialAtPond, "32896"),
                new GenericItemsData("Gold Coins: Bag in Barrel at Inn", 251, Addresses.TSV_Pickup_GoldCoinsBagInBarrelAtInn, "32896"),
                new GenericItemsData("Gold Coins: Bag in Barrel at bottom of Inn Stairs", 252, Addresses.TSV_Pickup_GoldCoinsBagInBarrelAtBottomOfInnStairs, "32896"),
                new GenericItemsData("Gold Coins: Bag in Barrel Behind Inn Stairs", 253, Addresses.TSV_Pickup_GoldCoinsBagInBarrelBehindInnStairs, "32896"),
                new GenericItemsData("Equipment: Club in SV Chest under Inn Stairs", 254, Addresses.TSV_Pickup_ClubInChestUnderInnStairs, "32896"),
                new GenericItemsData("Energy Vial: Bust Switch", 255, Addresses.TSV_Pickup_EnergyVialBustSwitch, "32896"),
                new GenericItemsData("Gold Coins: Bag In Top Bust Barrel", 256, Addresses.TSV_Pickup_GoldCoinsBagInTopBustBarrel, "32896"),
                new GenericItemsData("Gold Coins: Bag In Switch Bust Barrel", 257, Addresses.TSV_Pickup_GoldCoinsBagInSwitchBustBarrel, "32896"),
                new GenericItemsData("Gold Coins: Bag in Library", 258, Addresses.TSV_Pickup_GoldCoinsBagInLibrary, "32896"),
                new GenericItemsData("Gold Coins: Bag at Top of table", 259, Addresses.TSV_Pickup_GoldCoinsBagAtTopOfTable, "32896"),
                new GenericItemsData("Gold Coins: Bag at Bottom of table", 260, Addresses.TSV_Pickup_GoldCoinsBagAtBottomOfTable, "32896"),
                new GenericItemsData("Gold Coins: Chest next to Chalice", 261, Addresses.TSV_Pickup_GoldCoinsChestNextToChalice, "32896"),
                new GenericItemsData("Energy Vial: Near Exit", 262, Addresses.TSV_Pickup_EnergyVialNearExit, "32896"),
                new GenericItemsData("Energy Vial: Near Chalice", 263, Addresses.TSV_Pickup_EnergyVialNearChalice, "32896"),
            };
            return tsvLocations;
        }

        private static List<GenericItemsData> GetPoolsOfTheAncientDeadData()
        {
            List<GenericItemsData> padLocations = new List<GenericItemsData>() {
                new GenericItemsData("Gold Coins: Bag at Entrance", 264, Addresses.PAD_Pickup_GoldCoinsBagAtEntrance, "32896"),
                new GenericItemsData("Energy Vial: Broken Structure near Entrance", 265, Addresses.PAD_Pickup_EnergyVialBrokenStructureNearEntrance, "32896"),
                new GenericItemsData("Gold Coins: Bag on Island Near Soul 2", 266, Addresses.PAD_Pickup_GoldCoinsBagOnIslandNearSoul2, "32896"),
                new GenericItemsData("Energy Vial: Next to Lost Soul 3", 267, Addresses.PAD_Pickup_EnergyVialNextToLostSoul3, "32896"),
                new GenericItemsData("Energy Vial: Near Gate", 268, Addresses.PAD_Pickup_EnergyVialNearGate, "32896"),
                new GenericItemsData("Equipment: Silver Shield in PAD in Chest Near Soul 5", 269, Addresses.PAD_Pickup_SilverShieldInChestNearSoul5, "32896"),
                new GenericItemsData("Energy Vial: Chariot Right", 270, Addresses.PAD_Pickup_EnergyVialChariotRight, "32896"),
                new GenericItemsData("Energy Vial: Chariot Left", 271, Addresses.PAD_Pickup_EnergyVialChariotLeft, "32896"),
                new GenericItemsData("Energy Vial: Jump Spot 1", 272, Addresses.PAD_Pickup_EnergyVialJumpSpot1, "32896"),
                new GenericItemsData("Energy Vial: Jump Spot 2", 273, Addresses.PAD_Pickup_EnergyVialJumpSpot2, "32896"),
                new GenericItemsData("Gold Coins: Jump Spot 1", 274, Addresses.PAD_Pickup_GoldCoinsJumpSpot1, "32896"),
                new GenericItemsData("Gold Coins: Jump Spot 2", 275, Addresses.PAD_Pickup_GoldCoinsJumpSpot2, "32896"),
                new GenericItemsData("Life Bottle: Pools of the Ancient Dead", 67, Addresses.PAD_Pickup_LifeBottle, "32896"),
            };
            return padLocations;
        }

        private static List<GenericItemsData> GetTheLakeData()
        {
            List<GenericItemsData> tlLocations = new List<GenericItemsData>() {
                new GenericItemsData("Energy Vial: Flooded House", 276, Addresses.TL_Pickup_EnergyVialFloodedHouse, "32896"),
                new GenericItemsData("Gold Coins: Bag Outside Flooded House", 277, Addresses.TL_Pickup_GoldCoinsBagOutsideFloodedHouse, "32896"),
                new GenericItemsData("Gold Coins: Bag Near Closed Gate", 278, Addresses.TL_Pickup_GoldCoinsBagNearClosedGate, "32896"),
                new GenericItemsData("Equipment: Silver Shield in TL In Whirlpool", 279, Addresses.TL_Pickup_SilverShieldInWhirlpool, "32896"),
                new GenericItemsData("Gold Coins: Bag at the Whirlpool Entrance", 280, Addresses.TL_Pickup_GoldCoinsBagAtTheWhirlpoolEntrance, "32896"),
                new GenericItemsData("Energy Vial: Whirpool Wind 1", 281, Addresses.TL_Pickup_EnergyVialWhirlpoolWind1, "32896"),
                new GenericItemsData("Energy Vial: Whirpool Wind 2", 282, Addresses.TL_Pickup_EnergyVialWhirlpoolWind2, "32896"),
                new GenericItemsData("Gold Coins: Whirlpool Wind 1", 283, Addresses.TL_Pickup_GoldCoinsWhirlpoolWind1, "32896"),
                new GenericItemsData("Gold Coins: Whirlpool Wind 2", 284, Addresses.TL_Pickup_GoldCoinsWhirlpoolWind2, "32896"),
                new GenericItemsData("Gold Coins: Outside Whirlpool Exit", 285, Addresses.TL_Pickup_GoldCoinsOutsideWhirlpoolExit, "32896"),
            };
            return tlLocations;
        }

        private static List<GenericItemsData> GetTheCrystalCavesData()
        {
            List<GenericItemsData> ccLocations = new List<GenericItemsData>() {
                new GenericItemsData("Gold Coins: Bag in Crystal at Start", 286, Addresses.CC_Pickup_GoldCoinsBagInCrystalAtStart, "32896"),
                new GenericItemsData("Gold Coins: Bag in Spinner", 287, Addresses.CC_Pickup_GoldCoinsBagInSpinner, "32896"),
                new GenericItemsData("Equipment: Silver Shield in CC in Crystal", 288, Addresses.CC_Pickup_SilverShieldInCrystal, "32896"),
                new GenericItemsData("Gold Coins: Bag near Silver Shield", 289, Addresses.CC_Pickup_GoldCoinsBagNearSilverShield, "32896"),
                new GenericItemsData("Gold Coins: Chest in Crystal After Earth Door", 290, Addresses.CC_Pickup_GoldCoinsChestInCrystalAfterEarthDoor, "32896"),
                new GenericItemsData("Gold Coins: Bag in Dragon Room 1 1st Platform", 291, Addresses.CC_Pickup_GoldCoinsBagInDragonRoom11stPlatform, "32896"),
                new GenericItemsData("Energy Vial: Dragon Room 1st Platform", 292, Addresses.CC_Pickup_EnergyVialDragonRoom1stPlatform, "32896"),
                new GenericItemsData("Gold Coins: Bag in Dragon Room 2 1st Platform", 293, Addresses.CC_Pickup_GoldCoinsBagInDragonRoom21stPlatform, "32896"),
                new GenericItemsData("Gold Coins: Chest in Dragon Room 1st Platform", 294, Addresses.CC_Pickup_GoldCoinsChestInDragonRoom1stPlatform, "32896"),
                new GenericItemsData("Gold Coins: Bag in Dragon Room 2nd Platform", 295, Addresses.CC_Pickup_GoldCoinsBagInDragonRoom2ndPlatform, "32896"),
                new GenericItemsData("Gold Coins: Bag in Dragon Room 1 3rd Platform", 296, Addresses.CC_Pickup_GoldCoinsBagInDragonRoom13rdPlatform, "32896"),
                new GenericItemsData("Energy Vial: Dragon Room 3rd Platform", 297, Addresses.CC_Pickup_EnergyVialDragonRoom3rdPlatform, "32896"),
                new GenericItemsData("Gold Coins: Bag in Dragon Room 2 3rd Platform", 298, Addresses.CC_Pickup_GoldCoinsBagInDragonRoom23rdPlatform, "32896"),
                new GenericItemsData("Gold Coins: Chest in Dragon Room 3rd Platform", 299, Addresses.CC_Pickup_GoldCoinsChestInDragonRoom3rdPlatform, "32896"),
                new GenericItemsData("Gold Coins: Bag in Dragon Room 4th Platform 1", 300, Addresses.CC_Pickup_GoldCoinsBagInDragonRoom4thPlatform1, "32896"),
                new GenericItemsData("Gold Coins: Chest in Dragon Room 4th Platform", 301, Addresses.CC_Pickup_GoldCoinsChestInDragonRoom4thPlatform, "32896"),
                new GenericItemsData("Gold Coins: Bag in Dragon Room 4th Platform 2", 302, Addresses.CC_Pickup_GoldCoinsBagInDragonRoom4thPlatform2, "32896"),
                new GenericItemsData("Gold Coins: Bag on Left of Pool", 303, Addresses.CC_Pickup_GoldCoinsBagOnLeftOfPool, "32896"),
                new GenericItemsData("Gold Coins: Bag on Right of Pool", 304, Addresses.CC_Pickup_GoldCoinsBagOnRightOfPool, "32896"),
                new GenericItemsData("Gold Coins: Chest in Crystal after Pool", 305, Addresses.CC_Pickup_GoldCoinsChestInCrystalAfterPool, "32896"),
            };
            return ccLocations;
        }

        private static List<GenericItemsData> GetTheAsylumGroundsData()
        {
            List<GenericItemsData> agLocations = new List<GenericItemsData>() {
                new GenericItemsData("Gold Coins: Bag in Bell Grave Near Bell", 312, Addresses.AG_Pickup_GoldCoinsBagInBellGraveNearBell, "32896"),
                new GenericItemsData("Gold Coins: Bag in Bell Grave Near Entrance", 313, Addresses.AG_Pickup_GoldCoinsBagInBellGraveNearEntrance, "32896"),
                new GenericItemsData("Gold Coins: Bag Near Shooting Statue", 314, Addresses.AG_Pickup_GoldCoinsBagNearShootingStatue, "32896"),
                new GenericItemsData("Equipment: Silver Shield in AG in Chest Behind Door", 315, Addresses.AG_Pickup_SilverShieldInChestBehindDoor, "32896"),
                new GenericItemsData("Gold Coins: Bag in Rat Grave", 316, Addresses.AG_Pickup_GoldCoinsBagInRatGrave, "32896"),
                new GenericItemsData("Gold Coins: Behind Chaos Gate", 317, Addresses.AG_Pickup_GoldCoinsBehindChaosGate, "32896"),
                new GenericItemsData("Gold Coins: Behind Elephant in Grave", 318, Addresses.AG_Pickup_GoldCoinsBehindElephantInGrave, "32896"),
                new GenericItemsData("Energy Vial: Near Bishop", 319, Addresses.AG_Pickup_EnergyVialNearBishop, "32896"),
                new GenericItemsData("Energy Vial: Near King", 320, Addresses.AG_Pickup_EnergyVialNearKing, "32896"),
            };
            return agLocations;
        }

        private static List<GenericItemsData> GetInsideTheAsylumData()
        {
            List<GenericItemsData> iaLocations = new List<GenericItemsData>() {
                new GenericItemsData("Energy Vial: Bat Room", 324, Addresses.IA_Pickup_EnergyVialBatRoom, "32896"),
                new GenericItemsData("Gold Coins: Bag in Bat Room Left", 325, Addresses.IA_Pickup_GoldCoinsBagInBatRoomLeft, "32896"),
                new GenericItemsData("Gold Coins: Chest in Bat Room", 323, Addresses.IA_Pickup_GoldCoinsChestInBatRoom, "32896"),
                new GenericItemsData("Gold Coins: Bag in Bat Room Centre", 324, Addresses.IA_Pickup_GoldCoinsBagInBatRoomCentre, "32896"),
                new GenericItemsData("Equipment: Silver Shield in IA in Bat Room", 325, Addresses.IA_Pickup_SilverShieldInBatRoom, "32896"),
                new GenericItemsData("Gold Coins: Bag in Bat Room Right", 326, Addresses.IA_Pickup_GoldCoinsBagInBatRoomRight, "32896"),
                new GenericItemsData("Energy Vial: Asylumn Room 1", 327, Addresses.IA_Pickup_EnergyVialAsylumRoom1, "32896"),
                new GenericItemsData("Energy Vial: Asylumn Room 2", 328, Addresses.IA_Pickup_EnergyVialAsylumRoom2, "32896"),
                new GenericItemsData("Gold Coins: Bag in Asylumn Room", 329, Addresses.IA_Pickup_GoldCoinsBagInAsylumRoom, "32896"),
                new GenericItemsData("Gold Coins: Bag in Sewer Prison Entrance", 3230, Addresses.IA_Pickup_GoldCoinsBagInSewerPrisonEntrance, "32896"),
                new GenericItemsData("Gold Coins: Bag on Sewer Prison Bench", 331, Addresses.IA_Pickup_GoldCoinsBagOnSewerPrisonBench, "32896"),
                new GenericItemsData("Key Item: Dragon Gem: Inside the Asylum", 331, Addresses.IA_Pickup_DragonGem, "32896"),
            };
            return iaLocations;
        }




        private static List<GenericItemsData> GetGallowsGauntletData()
        {
            List<GenericItemsData> ggLocations = new List<GenericItemsData>() {
                new GenericItemsData("Gold Coins: Bag Behind Stone Dragon 1", 306, Addresses.GG_Pickup_GoldCoinsBagBehindStoneDragon1, "32896"),
                new GenericItemsData("Gold Coins: Bag Behind Stone Dragon 2", 307, Addresses.GG_Pickup_GoldCoinsBagBehindStoneDragon2, "32896"),
                new GenericItemsData("Equipment: Silver Shield in GG in Chest Near Exit", 308, Addresses.GG_Pickup_SilverShieldInChestNearExit, "32896"),
                new GenericItemsData("Gold Coins: Chest at Serpent", 309, Addresses.GG_Pickup_GoldCoinsChestAtSerpent, "32896"),
                new GenericItemsData("Gold Coins: Chest Near Star Entrance", 310, Addresses.GG_Pickup_GoldCoinsChestNearStarEntrance, "32896"),
                new GenericItemsData("Energy Vial: Near Chalice", 311, Addresses.GG_Pickup_EnergyVialNearChalice, "32896"),
            };
            return ggLocations;
        }

        private static List<GenericItemsData> GetPumpkinGorgeData()
        {
            List<GenericItemsData> pgLocations = new List<GenericItemsData>() {
                new GenericItemsData("Equipment: Club in PG in Chest in Tunnel", 332, Addresses.PG_Pickup_ClubInChestInTunnel, "32896"),
                new GenericItemsData("Gold Coins: Bag Between Hidden Pumpkins", 333, Addresses.PG_Pickup_GoldCoinsBagBetweenHiddenPumpkins, "32896"),
                new GenericItemsData("Gold Coins: Chest in Coop 1", 334, Addresses.PG_Pickup_GoldCoinsChestInCoop1, "32896"),
                new GenericItemsData("Energy Vial: In Coop", 335, Addresses.PG_Pickup_EnergyVialInCoop, "32896"),
                new GenericItemsData("Gold Coins: Chest in Coop 2", 336, Addresses.PG_Pickup_GoldCoinsChestInCoop2, "32896"),
                new GenericItemsData("Gold Coins: Chest in Coop 3", 337, Addresses.PG_Pickup_GoldCoinsChestInCoop3, "32896"),
                new GenericItemsData("Energy Vial: In Moon Hut", 338, Addresses.PG_Pickup_EnergyVialInMoonHut, "32896"),
                new GenericItemsData("Gold Coins: Bag in Mushroom Area", 339, Addresses.PG_Pickup_GoldCoinsBagInMushroomArea, "32896"),
                new GenericItemsData("Energy Vial: Top of Hill", 340, Addresses.PG_Pickup_EnergyVialTopOfHill, "32896"),
                new GenericItemsData("Equipment: Silver Shield in PG in Chest at Top of Hill", 341, Addresses.PG_Pickup_SilverShieldInChestAtTopOfHill, "32896"),
                new GenericItemsData("Energy Vial: Boulders After Star Rune", 342, Addresses.PG_Pickup_EnergyVialBouldersAfterStarRune, "32896"),
                new GenericItemsData("Gold Coins: Chest at Boulders after Star Rune", 343, Addresses.PG_Pickup_GoldCoinsChestAtBouldersAfterStarRune, "32896"),
                new GenericItemsData("Energy Vial: Vine Patch Left", 344, Addresses.PG_Pickup_EnergyVialVinePatchLeft, "32896"),
                new GenericItemsData("Energy Vial: Vine Patch Right", 345, Addresses.PG_Pickup_EnergyVialVinePatchRight, "32896"),
                new GenericItemsData("Gold Coins: Chest Near Chalice", 346, Addresses.PG_Pickup_GoldCoinsChestNearChalice, "32896"),
                new GenericItemsData("Energy Vial: Chalice Path", 347, Addresses.PG_Pickup_EnergyVialChalicePath, "32896"),
                //new GenericItemsData("Key Item: Dragon Gem: Pumpkin Gorge", 347, Addresses.pg_, "32896"),
            };
            return pgLocations;
        }

        private static List<GenericItemsData> GetPumpkinServantData()
        {
            List<GenericItemsData> psLocations = new List<GenericItemsData>() {
                new GenericItemsData("Gold Coins: Bag Behind House", 348, Addresses.PS_Pickup_GoldCoinsBagBehindHouse, "32896"),
                new GenericItemsData("Equipment: Silver Shield in PS in Chest near Leeches", 349, Addresses.PS_Pickup_SilverShieldInChestNearLeeches, "32896"),
                new GenericItemsData("Gold Coins: Bag Behind Vines and Pod", 350, Addresses.PS_Pickup_GoldCoinsBagBehindVinesAndPod, "32896"),
                new GenericItemsData("Energy Vial: Left at Merchant Gargoyle", 351, Addresses.PS_Pickup_EnergyVialLeftAtMerchantGargoyle, "32896"),
                new GenericItemsData("Gold Coins: Chest at Merchant Gargoyle", 352, Addresses.PS_Pickup_GoldCoinsChestAtMerchantGargoyle, "32896"),
                new GenericItemsData("Energy Vial: Right at Merchant Gargoyle", 353, Addresses.PS_Pickup_EnergyVialRightAtMerchantGargoyle, "32896"),
            };
            return psLocations;
        }

        private static List<GenericItemsData> GetTheHauntedRuinsData()
        {
            List<GenericItemsData> hrLocations = new List<GenericItemsData>() {
                new GenericItemsData("Gold Coins: Near First Set of farmers", 354, Addresses.HR_Pickup_GoldCoinsNearFirstSetOfFarmers, "32896"),
                new GenericItemsData("Energy Vial: Above Rune", 355, Addresses.HR_Pickup_EnergyVialAboveRune, "32896"),
                new GenericItemsData("Energy Vial: Corner of Walls 1", 356, Addresses.HR_Pickup_EnergyVialCornerOfWalls1, "32896"),
                new GenericItemsData("Equipment: Silver Shield in HR in Chest Near Rune Door", 357, Addresses.HR_Pickup_SilverShieldInChestNearRuneDoor, "32896"),
                new GenericItemsData("Energy Vial: Corner of Walls 2", 358, Addresses.HR_Pickup_EnergyVialCornerOfWalls2, "32896"),
                new GenericItemsData("Energy Vial: Corner of Walls 3", 359, Addresses.HR_Pickup_EnergyVialCornerOfWalls3, "32896"),
                new GenericItemsData("Energy Vial: Up from Oil", 360, Addresses.HR_Pickup_EnergyVialUpFromOil, "32896"),
                new GenericItemsData("Gold Coins: Bag Near Chalice North", 361, Addresses.HR_Pickup_GoldCoinsBagNearChaliceNorth, "32896"),
                new GenericItemsData("Gold Coins: Bag Near Chalice South", 362, Addresses.HR_Pickup_GoldCoinsBagNearChaliceSouth, "32896"),
                new GenericItemsData("Gold Coins: Bag in Crown Room", 363, Addresses.HR_Pickup_GoldCoinsBagInCrownRoom, "32896"),
                new GenericItemsData("Gold Coins: Chest at Catapult 1", 364, Addresses.HR_Pickup_GoldCoinsChestAtCatapult1, "32896"),
                new GenericItemsData("Gold Coins: Chest at Catapult 2", 365, Addresses.HR_Pickup_GoldCoinsChestAtCatapult2, "32896"),
                new GenericItemsData("Gold Coins: Chest at Catapult 3", 366, Addresses.HR_Pickup_GoldCoinsChestAtCatapult3, "32896"),
            };
            return hrLocations;
        }

        private static List<GenericItemsData> GetTheGhostShipData()
        {
            List<GenericItemsData> gsLocations = new List<GenericItemsData>() {
                new GenericItemsData("Gold Coins: Bag in Rolling Barrels Room", 367, Addresses.GS_Pickup_GoldCoinsBagInRollingBarrelsRoom, "32896"),
                new GenericItemsData("Equipment: Silver Shield in GS in Chest in Barrel Room", 368, Addresses.GS_Pickup_SilverShieldInChestInBarrelRoom, "32896"),
                new GenericItemsData("Gold Coins: Bag on Deck At Barrels", 369, Addresses.GS_Pickup_GoldCoinsBagOnDeckAtBarrels, "32896"),
                new GenericItemsData("Energy Vial: In Cabin", 370, Addresses.GS_Pickup_EnergyVialInCabin, "32896"),
                new GenericItemsData("Energy Vial: In Cannon Room", 371, Addresses.GS_Pickup_EnergyVialInCannonRoom, "32896"),
                new GenericItemsData("Gold Coins: Chest in Cannon Room", 372, Addresses.GS_Pickup_GoldCoinsChestInCannonRoom, "32896"),
                new GenericItemsData("Energy Vial: Rope Bridge 1", 373, Addresses.GS_Pickup_EnergyVialRopeBridge1, "32896"),
                new GenericItemsData("Energy Vial: Rope Bridge 2", 374, Addresses.GS_Pickup_EnergyVialRopeBridge2, "32896"),
                new GenericItemsData("Gold Coins: Rope Bridge", 375, Addresses.GS_Pickup_GoldCoinsRopeBridge, "32896"),
                new GenericItemsData("Equipment: Club in GS in Chest at Captain", 376, Addresses.GS_Pickup_ClubInChestAtCaptain, "32896"),
                new GenericItemsData("Energy Vial: Cage Lift 1", 377, Addresses.GS_Pickup_EnergyVialCageLift1, "32896"),
                new GenericItemsData("Energy Vial: Cage Lift 2", 378, Addresses.GS_Pickup_EnergyVialCageLift2, "32896"),
            };
            return gsLocations;
        }

        private static List<GenericItemsData> GetTheTimeDeviceData()
        {
            List<GenericItemsData> tdLocations = new List<GenericItemsData>() {
                new GenericItemsData("Equipment: Silver Shield on Clock", 379, Addresses.TD_Pickup_SilverShieldOnClock, "32896"),
                new GenericItemsData("Gold Coins: Laser Platform Right", 380, Addresses.TD_Pickup_GoldCoinsLaserPlatformRight, "32896"),
                new GenericItemsData("Gold Coins: Laser Platform Left", 381, Addresses.TD_Pickup_GoldCoinsLaserPlatformLeft, "32896"),
                new GenericItemsData("Gold Coins: Lone Pillar 1", 382, Addresses.TD_Pickup_GoldCoinsLonePillar1, "32896"),
                new GenericItemsData("Gold Coins: Lone Pillar 2", 383, Addresses.TD_Pickup_GoldCoinsLonePillar2, "32896"),
                new GenericItemsData("Gold Coins: Lone Pillar 3", 384, Addresses.TD_Pickup_GoldCoinsLonePillar3, "32896"),
                new GenericItemsData("Gold Coins: Bag at Earth Station 1", 385, Addresses.TD_Pickup_GoldCoinsBagAtEarthStation1, "32896"),
                new GenericItemsData("Gold Coins: Bag at Earth Station 2", 386, Addresses.TD_Pickup_GoldCoinsBagAtEarthStation2, "32896"),
                new GenericItemsData("Gold Coins: Bag at Earth Station 3", 387, Addresses.TD_Pickup_GoldCoinsBagAtEarthStation3, "32896"),
                new GenericItemsData("Life Bottle: The Time Device", 70, Addresses.TD_Pickup_LifeBottle, "32896"),
            };
            return tdLocations;
        }

        private static List<GenericItemsData> GetZaroksLairData()
        {
            List<GenericItemsData> zlLocations = new List<GenericItemsData>() {
                new GenericItemsData("Equipment: Good Lightning in ZL", 388, Addresses.ZL_Pickup_GoodLightning, "32896"),
                new GenericItemsData("Equipment: Silver Shield in ZL Arena", 389, Addresses.ZL_Pickup_SilverShield, "32896"),
            };
            return zlLocations;
        }



        private static List<GenericItemsData> GetLevelData()
        {
            List<GenericItemsData> levels = new List<GenericItemsData>()
            {
                new GenericItemsData("Cleared: Dan's Crypt", 1, Addresses.DansCryptLevelStatus, "16"),
                new GenericItemsData("Cleared: The Graveyard", 2, Addresses.TheGraveyardLevelStatus, "16"),
                new GenericItemsData("Cleared: Return to the Graveyard",3, Addresses.ReturnToTheGraveyardLevelStatus, "16"),
                new GenericItemsData("Cleared: Cemetery Hill",4, Addresses.CemeteryHillLevelStatus, "16"),
                new GenericItemsData("Cleared: The Hilltop Mausoleum",5, Addresses.TheHilltopMausoleumLevelStatus, "16"),
                new GenericItemsData("Cleared: Scarecrow Fields",6, Addresses.ScarecrowFieldsLevelStatus, "16"),
                new GenericItemsData("Cleared: Ant Hill",7, Addresses.AntHillLevelStatus, "16"),
                new GenericItemsData("Cleared: The Crystal Caves",6, Addresses.TheCrystalCavesLevelStatus, "16"),
                new GenericItemsData("Cleared: The Lake",9, Addresses.TheLakeLevelStatus, "16"),
                new GenericItemsData("Cleared: Pumpkin Gorge",10, Addresses.PumpkinGorgeLevelStatus, "16"),
                new GenericItemsData("Cleared: Pumpkin Serpent",11, Addresses.PumpkinSerpentLevelStatus, "16"),
                new GenericItemsData("Cleared: Sleeping Village",12, Addresses.SleepingVillageLevelStatus, "16"),
                new GenericItemsData("Cleared: Pools of the Ancient Dead",13, Addresses.PoolsOfTheAncientDeadLevelStatus, "16"),
                new GenericItemsData("Cleared: Asylum Grounds",14, Addresses.AsylumGroundsLevelStatus, "16"),
                new GenericItemsData("Cleared: Inside the Asylum",15, Addresses.InsideTheAsylumLevelStatus, "16"),
                new GenericItemsData("Cleared: Enchanted Earth",16, Addresses.EnchantedEarthLevelStatus, "16"),
                new GenericItemsData("Cleared: The Gallows Gauntlet" ,17, Addresses.TheGallowsGauntletLevelStatus, "16"),
                new GenericItemsData("Cleared: The Haunted Ruins",18, Addresses.TheHauntedRuinsLevelStatus, "16"),
                new GenericItemsData("Cleared: Ghost Ship",19, Addresses.GhostShipLevelStatus, "16"),
                new GenericItemsData("Cleared: The Entrance Hall",20, Addresses.TheEntranceHallLevelStatus, "16"),
                new GenericItemsData("Cleared: The Time Device",21, Addresses.TheTimeDeviceLevelStatus, "16"),
                new GenericItemsData("Cleared: Zaroks Lair",999, Addresses.WinConditionCredits, "16")

            };
            return levels;
        }
        private static List<GenericItemsData> GetChaliceData()
        {
            List<GenericItemsData> chalices = new List<GenericItemsData>()
            {
                new GenericItemsData("Chalice: The Graveyard", 23, Addresses.TG_Pickup_Chalice, "32896"),
                new GenericItemsData("Chalice: Return to the Graveyard", 24, Addresses.RTG_Pickup_Chalice, "32896"),
                new GenericItemsData("Chalice: Cemetery Hill", 25, Addresses.CH_Pickup_Chalice, "32896"),
                new GenericItemsData("Chalice: The Hilltop Mausoleum", 26, Addresses.HM_Pickup_Chalice, "32896"),
                new GenericItemsData("Chalice: Scarecrow Fields", 27, Addresses.SF_Pickup_Chalice, "32896"),
                //new GenericItemsData("Chalice: Ant Hill", 28, Addresses.), // This is the odd one i need to consider
                new GenericItemsData("Chalice: The Crystal Caves", 29, Addresses.CC_Pickup_Chalice,"32896"),
                new GenericItemsData("Chalice: The Lake", 30, Addresses.TL_Pickup_Chalice,"32896"),
                new GenericItemsData("Chalice: Pumpkin Gorge", 31, Addresses.PG_Pickup_Chalice,"32896"),
                new GenericItemsData("Chalice: Pumpkin Serpent", 32, Addresses.PS_Pickup_Chalice,"32896"),
                new GenericItemsData("Chalice: Sleeping Village", 33, Addresses.TSV_Pickup_Chalice,"32896"),
                new GenericItemsData("Chalice: Pools of the Ancient Dead", 34, Addresses.PAD_Pickup_Chalice,"32896"),
                new GenericItemsData("Chalice: Asylum Grounds", 35, Addresses.AG_Pickup_Chalice,"32896"),
                new GenericItemsData("Chalice: Inside the Asylum", 36, Addresses.IA_Pickup_Chalice,"32896"),
                new GenericItemsData("Chalice: Enchanted Earth", 37, Addresses.EE_Pickup_Chalice,"32896"),
                new GenericItemsData("Chalice: The Gallows Gauntlet", 38, Addresses.GG_Pickup_Chalice,"32896"),
                new GenericItemsData("Chalice: The Haunted Ruins", 39, Addresses.HR_Pickup_Chalice,"32896"),
                new GenericItemsData("Chalice: Ghost Ship", 40, Addresses.GS_Pickup_Chalice,"32896"),
                new GenericItemsData("Chalice: The Entrance Hall", 41, Addresses.EH_Pickup_Chalice,"32896"),
                new GenericItemsData("Chalice: The Time Device", 42, Addresses.TD_Pickup_Chalice,"32896")
            };
            return chalices;
        }

        private static List<GenericItemsData> GetSkillData()
        {
            List<GenericItemsData> skills = new List<GenericItemsData>()
            {
                new GenericItemsData("Skill: Daring Dash", 43, Addresses.ReturnToTheGraveyardLevelStatus, "1") // no easy way to check if AP is doing state, so level end works
            };
            return skills;
        }

        // Hall of Heroes needs an overhaul. Not worth dealing with right now

        //private static List<GenericItemsData> GetHallOfHeroesData()
        //{
        //    List<GenericItemsData> hallOfHeroeVisits = new List<GenericItemsData>()
        //    {
        //        new GenericItemsData("Hall of Heroes: Canny Tim 1",44),
        //        new GenericItemsData("Hall of Heroes: Canny Tim 2",45),
        //        new GenericItemsData("Hall of Heroes: Stanyer Iron Hewer 1",46),
        //        new GenericItemsData("Hall of Heroes: Stanyer Iron Hewer 2",47),
        //        new GenericItemsData("Hall of Heroes: Woden The Mighty 1",48),
        //        new GenericItemsData("Hall of Heroes: Woden The Mighty 2",49),
        //        new GenericItemsData("Hall of Heroes: RavenHooves The Archer 1",50),
        //        new GenericItemsData("Hall of Heroes: RavenHooves The Archer 2",51),
        //        new GenericItemsData("Hall of Heroes: RavenHooves The Archer 3",52),
        //        new GenericItemsData("Hall of Heroes: RavenHooves The Archer 4",53),
        //        new GenericItemsData("Hall of Heroes: Imanzi 1",54),
        //        new GenericItemsData("Hall of Heroes: Imanzi 2",55),
        //        new GenericItemsData("Hall of Heroes: Dark Steadfast 1",56),
        //        new GenericItemsData("Hall of Heroes: Dark Steadfast 2",57),
        //        new GenericItemsData("Hall of Heroes: Karl Stungard 1",58),
        //        new GenericItemsData("Hall of Heroes: Karl Stungard 2",59),
        //        new GenericItemsData("Hall of Heroes: Bloodmonath Skill Cleaver 1",60),
        //        new GenericItemsData("Hall of Heroes: Bloodmonath Skill Cleaver 2",61),
        //        new GenericItemsData("Hall of Heroes: Megwynne Stormbinder 1",62),
        //        new GenericItemsData("Hall of Heroes: Megwynne Stormbinder 2",63),
        //    };
        //    return hallOfHeroeVisits;
        //}

        // basically, the above needs ripped out and the actual values for each item pickup put into a table
        // however, getting the values is very awkward and annoying. I tried a few times but couldn't get them right.s
        private static List<GenericItemsData> GetWeaponInventoryData()
        {
            List<GenericItemsData> weaponInventoryLocations = new List<GenericItemsData>() {
                new GenericItemsData("Equipment: Small Sword",64, Addresses.SmallSword, "0"), // crypt
                new GenericItemsData("Equipment: Broadsword",65, Addresses.BroadswordCharge, "0"), // hall of heroes
                new GenericItemsData("Equipment: Magic Sword",66, Addresses.MagicSword, "0"),// hall of heroes
                new GenericItemsData("Equipment: Club",67, Addresses.ClubCharge, "0"), // cemetery hill
                new GenericItemsData("Equipment: Hammer",68, Addresses.Hammer, "0"),// hall of heroes
                new GenericItemsData("Equipment: Daggers",69, Addresses.DaggerAmmo, "0"), // dan's crypt
                new GenericItemsData("Equipment: Axe",70, Addresses.Axe, "0"),// hall of heroes
                new GenericItemsData("Equipment: Chicken Drumsticks",71, Addresses.ChickenDrumsticksAmmo, "0"), // enchanted earth
                new GenericItemsData("Equipment: Crossbow",72, Addresses.CrossbowAmmo, "0"),// hall of heroes
                new GenericItemsData("Equipment: Longbow",73, Addresses.LongbowAmmo, "0"),// hall of heroes
                new GenericItemsData("Equipment: Fire Longbow",74, Addresses.FireLongbowAmmo, "0"),// hall of heroes
                new GenericItemsData("Equipment: Magic Longbow",75, Addresses.MagicLongbowAmmo, "0"),// hall of heroes
                new GenericItemsData("Equipment: Spear",76, Addresses.SpearAmmo, "0"),// hall of heroes
                new GenericItemsData("Equipment: Lightning",77, Addresses.LightningCharge, "0"),// hall of heroes
                new GenericItemsData("Equipment: Good Lightning",78, Addresses.GoodLightning, "0"), // zarok's lair
                new GenericItemsData("Equipment: Copper Shield",79, Addresses.CopperShieldAmmo, "0"), // dan's crypt  (Way more than just this)
                new GenericItemsData("Equipment: Silver Shield",80, Addresses.SilverShieldAmmo, "0"), // return to the graveyard  (Way more than just this)
                new GenericItemsData("Equipment: Gold Shield",81, Addresses.GoldShieldAmmo, "0"),// hall of heroes
                new GenericItemsData("Equipment: Dragon Armour",82, Addresses.DragonArmour, "0"), // crystal caves
            };
            return weaponInventoryLocations;
        }
        private static List<GenericItemsData> GetWeaponDropLocationsData()
        {
            List<GenericItemsData> weaponDropLocations = new List<GenericItemsData>()
            {
                new GenericItemsData("Equipment: Broadsword",44, Addresses.FakeAddress, "0"), // hall of heroes not being dealt with atm
                new GenericItemsData("Equipment: Magic Sword",45, Addresses.FakeAddress, "0"),// hall of heroes not being dealt with atm

                new GenericItemsData("Equipment: Club in CH",67, Addresses.CH_Pickup_Club, "32896"),
                new GenericItemsData("Equipment: Club in HM Broken Benches",185, Addresses.HM_Pickup_ClubBrokenBenches, "32896"),
                new GenericItemsData("Equipment: Club in SF Inside Hut",220, Addresses.SF_Pickup_ClubInsideHut, "32896"),
                new GenericItemsData("Equipment: Club in AH Chest at Barrier",234, Addresses.TA_Pickup_ClubChestAtBarrier, "32896"),
                new GenericItemsData("Equipment: Club in SV Chest under Inn Stairs",276, Addresses.TSV_Pickup_ClubInChestUnderInnStairs, "32896"),
                new GenericItemsData("Equipment: Club in PG in Chest in Tunnel",354, Addresses.PG_Pickup_ClubInChestInTunnel, "32896"),
                new GenericItemsData("Equipment: Club in GS in Chest at Captain",398, Addresses.GS_Pickup_ClubInChestAtCaptain, "32896"),

                new GenericItemsData("Equipment: Hammer",68, Addresses.FakeAddress, "0"),// hall of heroes not being dealt with atm
                new GenericItemsData("Equipment: Daggers in HM Block Puzzle",192, Addresses.HM_Pickup_DaggersBlockPuzzle, "32896"),
                new GenericItemsData("Equipment: Axe",70, Addresses.FakeAddress, "0"),// hall of heroes not being dealt with atm
                new GenericItemsData("Equipment: Chicken Drumsticks in TA",71, Addresses.AntHillLevelStatus, "1"),
                new GenericItemsData("Equipment: Crossbow",72, Addresses.FakeAddress, "0"),// hall of heroes not being dealt with atm
                new GenericItemsData("Equipment: Longbow",73, Addresses.FakeAddress, "0"),// hall of heroes not being dealt with atm
                new GenericItemsData("Equipment: Fire Longbow",74, Addresses.FakeAddress, "0"),// hall of heroes not being dealt with atm
                new GenericItemsData("Equipment: Magic Longbow",75, Addresses.FakeAddress, "0"),// hall of heroes not being dealt with atm
                new GenericItemsData("Equipment: Spear",76, Addresses.FakeAddress, "0"),// hall of heroes not being dealt with atm
                new GenericItemsData("Equipment: Lightning",77, Addresses.FakeAddress, "0"),// hall of heroes not being dealt with atm
                new GenericItemsData("Equipment: Good Lightning in ZL",78, Addresses.ZL_Pickup_GoodLightning, "32896"), // zarok's lair

                new GenericItemsData("Equipment: Copper Shield in DC",139, Addresses.DC_Pickup_CopperShield, "32896"),
                new GenericItemsData("Equipment: Copper Shield in GY",170, Addresses.TG_Pickup_CopperShield, "32896"), // copper shield here needs value
                new GenericItemsData("Equipment: Copper Shield in CH 1",173, Addresses.CH_Pickup_CopperShield1stOnHill, "32896"),
                new GenericItemsData("Equipment: Copper Shield in CH 2",175, Addresses.CH_Pickup_CopperShield2ndOnHill, "32896"),
                new GenericItemsData("Equipment: Copper Shield in CH 3",177, Addresses.CH_Pickup_CopperShield3rdOnHill, "32896"),
                new GenericItemsData("Equipment: Copper Shield in Arena",180, Addresses.CH_Pickup_CopperShieldArena, "32896"),
                new GenericItemsData("Equipment: Copper Shield in HM Block Puzzle",193, Addresses.HM_Pickup_CopperShieldBlockPuzzle, "32896"),
                new GenericItemsData("Equipment: Copper Shield in SF - Chest In the Barn",223, Addresses.SF_Pickup_CopperShieldChestInTheBarn, "32896"),
                new GenericItemsData("Equipment: Copper Shield in EE in Egg",259, Addresses.EE_Pickup_CopperShieldInEgg, "32896"),

                new GenericItemsData("Equipment: Silver Shield in RG Chest At Shop",208, Addresses.RTG_Pickup_SilverShieldChestAtShop, "32896"),
                new GenericItemsData("Equipment: Silver Shield in SF Behind Windmill",221, Addresses.SF_Pickup_SilverShieldBehindWindmill, "32896"),
                new GenericItemsData("Equipment: Silver Shield in SV in Blacksmiths",270, Addresses.TSV_Pickup_SilverShieldInBlacksmiths, "32896"),
                new GenericItemsData("Equipment: Silver Shield in PAD in Chest Near Soul 5",291, Addresses.PAD_Pickup_SilverShieldInChestNearSoul5, "32896"),
                new GenericItemsData("Equipment: Silver Shield in TL In Whirlpool",301, Addresses.TL_Pickup_SilverShieldInWhirlpool, "32896"),
                new GenericItemsData("Equipment: Silver Shield in CC in Crystal",310, Addresses.CC_Pickup_SilverShieldInCrystal, "32896"),
                new GenericItemsData("Equipment: Silver Shield in GG in Chest Near Exit",330, Addresses.GG_Pickup_SilverShieldInChestNearExit, "32896"),
                new GenericItemsData("Equipment: Silver Shield in AG in Chest Behind Door",337, Addresses.AG_Pickup_SilverShieldInChestBehindDoor, "32896"),
                new GenericItemsData("Equipment: Silver Shield in IA in Bat Room",347, Addresses.IA_Pickup_SilverShieldInBatRoom, "32896"),
                new GenericItemsData("Equipment: Silver Shield in PG in Chest at Top of Hill",363, Addresses.PG_Pickup_SilverShieldInChestAtTopOfHill, "32896"),
                new GenericItemsData("Equipment: Silver Shield in PS in Chest near Leeches",371, Addresses.PS_Pickup_SilverShieldInChestNearLeeches,  "32896"),
                new GenericItemsData("Equipment: Silver Shield in HR in Chest Near Rune Door",379, Addresses.HR_Pickup_SilverShieldInChestNearRuneDoor, "32896"),
                new GenericItemsData("Equipment: Silver Shield in GS in Chest in Barrel Room", 390, Addresses.GS_Pickup_SilverShieldInChestInBarrelRoom, "32896"),
                new GenericItemsData("Equipment: Silver Shield in ZL Arena", 402, Addresses.ZL_Pickup_SilverShield, "32896"),

                new GenericItemsData("Equipment: Gold Shield",81, Addresses.FakeAddress, "0"),// hall of heroes
                new GenericItemsData("Equipment: Dragon Armour",82, Addresses.CC_Pickup_DragonArmour, "32896"), // crystal caves
            };
            return weaponDropLocations;
        }

        private static List<GenericItemsData> GetLifeBottleLocationsData()
        {
            List<GenericItemsData> bottleLocations = new List<GenericItemsData>()
            {
                new GenericItemsData("Life Bottle: Dan's Crypt",62, Addresses.DC_Pickup_LifeBottle, "32896"),
                //new GenericItemsData("Life Bottle: The Graveyard",84),
                //new GenericItemsData("Life Bottle: Hall of Heroes (Canny Tim)",85),

                //new GenericItemsData("Life Bottle: Scarecrow Fields",87),
                //new GenericItemsData("Life Bottle: Pools of the Ancient Dead",88),
                //new GenericItemsData("Life Bottle: Hall of Heroes (Ravenhooves The Archer)",89),
                //new GenericItemsData("Life Bottle: Hall of Heroes (Dirk Steadfast)",90),
                //new GenericItemsData("Life Bottle: The Time Device",91),
            };
            return bottleLocations;
        }


        private static List<GenericItemsData> GetKeyItemsData()
        {
            List<GenericItemsData> bottleLocations = new List<GenericItemsData>()
            {
                //new GenericItemsData("Key Item: Dragon Gem: Pumpkin Gorge",92),
                //new GenericItemsData("Key Item: Dragon Gem: Inside the Asylum",93),
                //new GenericItemsData("Key Item: King Peregrine's Crown",94),
                //new GenericItemsData("Key Item: Soul Helmet 1",95),
                //new GenericItemsData("Key Item: Soul Helmet 2",96),
                //new GenericItemsData("Key Item: Soul Helmet 3",97),
                //new GenericItemsData("Key Item: Soul Helmet 4",98),
                //new GenericItemsData("Key Item: Soul Helmet 5",99),
                //new GenericItemsData("Key Item: Soul Helmet 6",100),
                //new GenericItemsData("Key Item: Soul Helmet 7",101),
                //new GenericItemsData("Key Item: Soul Helmet 8",102),
                //new GenericItemsData("Key Item: Witches Talisman",103),
                //new GenericItemsData("Key Item: Safe Key",104),
                //new GenericItemsData("Key Item: Shadow Artefact",105),
                //new GenericItemsData("Key Item: Crucifix",106),
                //new GenericItemsData("Key Item: Landlords Bust",107),
                //new GenericItemsData("Key Item: Crucifix Cast",108),
                //new GenericItemsData("Key Item: Amber Piece 1",109),
                //new GenericItemsData("Key Item: Amber Piece 2",110),
                //new GenericItemsData("Key Item: Amber Piece 3",111),
                //new GenericItemsData("Key Item: Amber Piece 4",112),
                //new GenericItemsData("Key Item: Amber Piece 5",113),
                //new GenericItemsData("Key Item: Amber Piece 6",114),
                //new GenericItemsData("Key Item: Amber Piece 7",115),
                //new GenericItemsData("Key Item: Harvester Parts",116),
                //new GenericItemsData("Key Item: Skull Key",117),
                //new GenericItemsData("Key Item: Sheet Music",118),
            };
            return bottleLocations;
        }

        private static List<GenericItemsData> GetRuneLocationsData()
        {
            List<GenericItemsData> runeLocations = new List<GenericItemsData>()
            {
                //new GenericItemsData("Chaos Rune: The Graveyard",119),
                //new GenericItemsData("Chaos Rune: The Hilltop Mausoleum",120),
                //new GenericItemsData("Chaos Rune: Scarecrow Fields",121),
                //new GenericItemsData("Chaos Rune: The Lake",122),
                //new GenericItemsData("Chaos Rune: Pumpkin Gorge",123),
                //new GenericItemsData("Chaos Rune: Sleeping Village",124),
                //new GenericItemsData("Chaos Rune: Pools of the Ancient Dead",125),
                //new GenericItemsData("Chaos Rune: Asylum Grounds",126),
                //new GenericItemsData("Chaos Rune: The Haunted Ruins",127),
                //new GenericItemsData("Chaos Rune: Ghost Ship",128),
                //new GenericItemsData("Chaos Rune: The Time Device",129),
                //new GenericItemsData("Earth Rune: The Graveyard",130),
                //new GenericItemsData("Earth Rune: The Hilltop Mausoleum",131),
                //new GenericItemsData("Earth Rune: Scarecrow Fields",132),
                //new GenericItemsData("Earth Rune: The Crystal Caves",133),
                //new GenericItemsData("Earth Rune: The Lake",134),
                //new GenericItemsData("Earth Rune: Pumpkin Gorge",135),
                //new GenericItemsData("Earth Rune: Sleeping Village",136),
                //new GenericItemsData("Earth Rune: Inside the Asylum",137),
                //new GenericItemsData("Earth Rune: Enchanted Earth",138),
                //new GenericItemsData("Earth Rune: The Haunted Ruins",139),
                //new GenericItemsData("Earth Rune: The Entrance Hall",140),
                //new GenericItemsData("Earth Rune: The Time Device",141),
                //new GenericItemsData("Moon Rune: The Hilltop Mausoleum",142),
                //new GenericItemsData("Moon Rune: Scarecrow Fields",143),
                //new GenericItemsData("Moon Rune: Pumpkin Gorge",144),
                //new GenericItemsData("Moon Rune: Ghost Ship",145),
                //new GenericItemsData("Moon Rune: The Time Device",146),
                //new GenericItemsData("Star Rune: Return to the Graveyard",147),
                //new GenericItemsData("Star Rune: The Crystal Caves",149),
                //new GenericItemsData("Star Rune: The Lake",150),
                //new GenericItemsData("Star Rune: Enchanted Earth",151),
                //new GenericItemsData("Star Rune: The Gallows Gauntlet",152),
                //new GenericItemsData("Star Rune: Ghost Ship",153),
                //new GenericItemsData("Time Rune: The Lake",154),
                //new GenericItemsData("Time Rune: Pumpkin Gorge",155),
                //new GenericItemsData("Time Rune: The Time Device",156),
            };
            return runeLocations;
        }



    }
}