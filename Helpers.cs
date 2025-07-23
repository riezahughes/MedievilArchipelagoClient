using Archipelago.Core;
using Archipelago.Core.Models;
using Archipelago.Core.Util;
using Archipelago.MultiClient.Net.Models;
using MedievilArchipelago;
using MedievilArchipelago.Models;
using Newtonsoft.Json;
using Serilog;
using SharpDX.DXGI;
using System;
using System.Buffers.Text;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.NetworkInformation;
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
        public static Dictionary<string, int> WeaponEquipDictionary = new Dictionary<string, int>
        {
            {"Equipment: Short Sword", 0},
            {"Equipment: Broadsword", 1},
            {"Equipment: Magic Sword", 2},
            {"Equipment: Club", 5},
            {"Equipment: Hammer", 12},
            {"Equipment: Daggers", 3},
            {"Equipment: Axe", 4},
            {"Equipment: Chicken Drumsticks", 7},
            {"Equipment: Crossbow", 6},
            {"Equipment: Longbow", 10},
            {"Equipment: Fire Longbow",13 },
            {"Equipment: Magic Longbow",14},
            {"Equipment: Spear", 11},
            {"Equipment: Lightning", 9},
            {"Equipment: Good Lightning", 15},
            {"Equipment: Dragon Armour", 16 },
            {"Dans Arm", 8 }
        };
        public static Dictionary<string, int> ShieldEquipDictionary = new Dictionary<string, int>
        {
            {"Equipment: Copper Shield", 1},
            {"Equipment: Silver Shield", 2},
            {"Equipment: Gold Shield", 3}
        };


        public static List<Location> BuildLocationList(Dictionary<string, object> options)
        {
            int base_id = 99250000;
            int region_offset = 1000;

            List<string> table_order = [
                "Map",
                "Hall of Heroes",
                "Dan's Crypt",
                "The Graveyard",
                "Cemetery Hill",
                "The Hilltop Mausoleum",
                "Return to the Graveyard",
                "Scarecrow Fields",
                "Ant Hill",
                "Enchanted Earth",
                "The Sleeping Village",
                "Pools of the Ancient Dead",
                "The Lake",
                "The Crystal Caves",
                "The Gallows Gauntlet",
                "Asylum Grounds",
                "Inside the Asylum",
                "Pumpkin Gorge",
                "Pumpkin Serpent",
                "The Haunted Ruins",
                "The Ghost Ship",
                "The Entrance Hall",
                "The Time Device",
                "Zaroks Lair"
];

            List<Location> locations = new List<Location>();

            Dictionary<string, List<GenericItemsData>> allLevelLocations = new Dictionary<string, List<GenericItemsData>>();

            var option_excludesDynamicLocations = options["exclude_dynamic_items"];

            // Level Locations
            allLevelLocations.Add("Hall of Heroes", GetHallOfHeroesData());
            allLevelLocations.Add("Dan's Crypt", GetDansCryptData());
            allLevelLocations.Add("The Graveyard", GetTheGraveyardData());
            allLevelLocations.Add("Cemetery Hill", GetCemeteryHillData());
            allLevelLocations.Add("The Hilltop Mausoleum", GetHilltopMausoleumData());
            allLevelLocations.Add("Return to the Graveyard", GetReturnToTheGraveyardData());
            allLevelLocations.Add("Scarecrow Fields", GetScarecrowFieldsData());
            allLevelLocations.Add("Ant Hill", GetAnthillData());
            allLevelLocations.Add("Enchanted Earth", GetEnchantedEarthData());
            allLevelLocations.Add("The Sleeping Village", GetTheSleepingVillageData());
            allLevelLocations.Add("Pools Of The Ancient Dead", GetPoolsOfTheAncientDeadData());
            allLevelLocations.Add("The Lake", GetTheLakeData());
            allLevelLocations.Add("The Crystal Caves", GetTheCrystalCavesData());
            allLevelLocations.Add("Gallows Gauntlet", GetGallowsGauntletData());
            allLevelLocations.Add("Asylum Grounds", GetTheAsylumGroundsData());
            allLevelLocations.Add("Inside The Asylum", GetInsideTheAsylumData());
            allLevelLocations.Add("Pumpkin Gorge", GetPumpkinGorgeData());
            allLevelLocations.Add("Pumpkin Serpent", GetPumpkinSerpentData());
            allLevelLocations.Add("The Haunted Ruins", GetTheHauntedRuinsData());
            allLevelLocations.Add("The Ghost Ship", GetTheGhostShipData());
            allLevelLocations.Add("The Entrance Hall", GetTheEntranceHallData());
            allLevelLocations.Add("The Time Device", GetTheTimeDeviceData());
            allLevelLocations.Add("Zaroks Lair", GetZaroksLairData());

            var regional_index = 0;
            foreach (var region_name in table_order.ToList())
            {

                long currentRegionBaseId = base_id + (regional_index * region_offset);

                Console.WriteLine($"Base: {currentRegionBaseId}");

                if (allLevelLocations.ContainsKey(region_name))
                {
                    // Retrieve the list of locations for the current region
                    List<GenericItemsData> regionLocations = allLevelLocations[region_name];

                    var location_index = 0;

                    foreach (var loc in regionLocations)

                    {
                        Console.WriteLine(location_index);
                        Console.WriteLine((int)currentRegionBaseId + location_index);

                        int locationId = (int)currentRegionBaseId + location_index;

                        // option_excludesDynamicLocations


                        if (loc.DynamicItem == true)
                        {
                            location_index++;
                            continue;
                        }

                        if (loc.Name.Contains("Cleared:")) // if it's cleared and we don't have an option set 
                        {
                            {
                                Location location = new Location()
                                {
                                    Name = loc.Name,
                                    Address = loc.Address,
                                    Id = locationId,
                                    CheckType = LocationCheckType.Byte,
                                    CompareType = loc.Name == "Cleared: Zaroks Lair" ? LocationCheckCompareType.Match : LocationCheckCompareType.GreaterThan,
                                    CheckValue = loc.Name == "Cleared: Zaroks Lair" ? "101" : "16" // if zarok clear
                                };

                                if (loc.Name == "Cleared: Zaroks Lair")
                                {
                                    Console.WriteLine($"{location.Name}, {location.Address:X}, {location.Id}, {location.CheckType}, {location.CompareType}, {location.CheckValue}");
                                }

                                locations.Add(location);
                                location_index++;
                                continue;
                            };
                        }
                        if (loc.Name.Contains("Key Item:") || loc.Name.Contains("Chalice:") || loc.Name.Contains("Rune:") || loc.Name.Contains("Equipment:") || loc.Name.Contains("Gold Coins:") || loc.Name.Contains("Skill:") || loc.Name.Contains("Life Bottle:") || loc.Name.Contains("Energy Vial:"))
                        {
                            {
                                Location location = new Location()
                                {
                                    Name = loc.Name,
                                    Address = loc.Address,
                                    Id = locationId,
                                    CheckType = LocationCheckType.Int,
                                    CompareType = LocationCheckCompareType.Match,
                                    CheckValue = loc.Check
                                };

                                locations.Add(location);
                                location_index++;
                                continue;
                            };
                        }
                        else
                        {
                            Console.WriteLine($"Could not add {loc.Name}, id: {loc.Id}");
                        }
                    }
                }
                regional_index++;
            }

            // debugging line
            foreach (var location in locations)
            {
                Console.WriteLine($"ID: {location.Id} | name: \"{location.Name}\"");
            }

            return locations;
        }

        public static Dictionary<string, uint> FlattenedInventoryStrings()
        {
            Dictionary<string, Dictionary<string, uint>> currentDict = StatusAndInventoryAddressDictionary();
            Dictionary<string, uint> newDict = new Dictionary<string, uint>();

            List<string> validList = new List<string>
            {
                "Equipment",
                "Player Stats",
                "Key Items"
            };


            foreach (KeyValuePair<string, Dictionary<string, uint>> location in currentDict)
            {
                string categoryName = location.Key;

                if (!validList.Contains(categoryName))
                {
                    continue;
                }
                Dictionary<string, uint> categoryItems = location.Value;


                foreach (KeyValuePair<string, uint> item in categoryItems)
                {
                    string prefix = location.Key == "Equipment" ? "Equipment: " : location.Key == "Key Items" ? "Key Item: " : location.Key == "Skills" ? "Skill: " : location.Key == "Level Status" ? "Cleared: " : "";
                    string itemName = prefix + item.Key;
                    uint itemAddress = item.Value;
                    newDict.Add(itemName, itemAddress);
                }

            }

            return newDict;


        }
        public static Dictionary<string, Dictionary<string, uint>> StatusAndInventoryAddressDictionary()
        {
            return new Dictionary<string, Dictionary<string, uint>>
            {
                ["Equipment"] = new Dictionary<string, uint>
                {
                    ["Small Sword"] = Addresses.SmallSword,
                    ["Magic Sword"] = Addresses.MagicSword,
                    ["Hammer"] = Addresses.Hammer,
                    ["Axe"] = Addresses.Axe,
                    ["Good Lightning"] = Addresses.GoodLightning,
                    ["Dragon Armour"] = Addresses.DragonArmour,
                    ["Daggers"] = Addresses.DaggerAmmo,
                    ["Broadsword"] = Addresses.BroadswordCharge,
                    ["Club"] = Addresses.ClubCharge,
                    ["Chicken Drumsticks"] = Addresses.ChickenDrumsticksAmmo,
                    ["Crossbow"] = Addresses.CrossbowAmmo,
                    ["Longbow"] = Addresses.LongbowAmmo,
                    ["Fire Longbow"] = Addresses.FireLongbowAmmo,
                    ["Magic Longbow"] = Addresses.MagicLongbowAmmo,
                    ["Spear"] = Addresses.SpearAmmo,
                    ["Copper Shield"] = Addresses.CopperShieldAmmo,
                    ["Silver Shield"] = Addresses.SilverShieldAmmo,
                    ["Gold Shield"] = Addresses.GoldShieldAmmo,
                    ["Lightning"] = Addresses.LightningCharge,
                },

                ["Ammo"] = new Dictionary<string, uint>
                {
                    ["Dagger Ammo"] = Addresses.DaggerAmmo,
                    ["Broadsword Charge"] = Addresses.BroadswordCharge,
                    ["Club Charge"] = Addresses.ClubCharge,
                    ["Chicken Drumsticks Ammo"] = Addresses.ChickenDrumsticksAmmo,
                    ["Crossbow Ammo"] = Addresses.CrossbowAmmo,
                    ["Longbow Ammo"] = Addresses.LongbowAmmo,
                    ["Fire Longbow Ammo"] = Addresses.FireLongbowAmmo,
                    ["Magic Longbow Ammo"] = Addresses.MagicLongbowAmmo,
                    ["Spear Ammo"] = Addresses.SpearAmmo,
                    ["Copper Shield Ammo"] = Addresses.CopperShieldAmmo,
                    ["Silver Shield Ammo"] = Addresses.SilverShieldAmmo,
                    ["Gold Shield Ammo"] = Addresses.GoldShieldAmmo,
                    ["Lightning Charge"] = Addresses.LightningCharge,
                },

                ["Player Stats"] = new Dictionary<string, uint>
                {
                    ["Gold Coins"] = Addresses.CurrentGold,
                    ["Health"] = Addresses.CurrentEnergy,
                    ["Health Vial"] = Addresses.CurrentEnergy,
                    ["Life Bottle"] = Addresses.CurrentLifePotions,
                    ["Energy"] = Addresses.CurrentEnergy,
                },
                ["Skills"] = new Dictionary<string, uint>
                {
                    ["Daring Dash"] = Addresses.DaringDashSkill,
                },
                ["Key Items"] = new Dictionary<string, uint>
                {
                    ["Dragon Gem"] = Addresses.DragonGem,
                    ["King Peregrine's Crown"] = Addresses.KingPeregrinesCrown,
                    ["Soul Helmet"] = Addresses.SoulHelmet,
                    ["Witches Talisman"] = Addresses.WitchesTalisman,
                    ["Safe Key"] = Addresses.SafeKey,
                    ["Shadow Artefact"] = Addresses.ShadowArtefact,
                    ["Shadow Talisman"] = Addresses.FakeAddress, //// neeed the talisman address!
                    ["Crucifix"] = Addresses.Crucifix,
                    ["Landlords Bust"] = Addresses.LandlordsBust,
                    ["Crucifix Cast"] = Addresses.CrucifixCast,
                    ["Amber Piece"] = Addresses.AmberPiece,
                    ["Harvester Parts"] = Addresses.HarvesterParts,
                    ["Skull Key"] = Addresses.SkullKey,
                    ["Sheet Music"] = Addresses.SheetMusic
                },
                ["Level Status"] = new Dictionary<string, uint>
                {

                    ["Dan's Crypt"] = Addresses.DC_LevelStatus,
                    ["The Graveyard"] = Addresses.TG_LevelStatus,
                    ["Cemetery Hill"] = Addresses.CH_LevelStatus,
                    ["The Hilltop Mausoleum"] = Addresses.HM_LevelStatus,
                    ["Return to the Graveyard"] = Addresses.RTG_LevelStatus,
                    ["Scarecrow Fields"] = Addresses.SF_LevelStatus,
                    ["Ant Hill"] = Addresses.TA_LevelStatus,
                    ["Enchanted Earth"] = Addresses.EE_LevelStatus,
                    ["Sleeping Village"] = Addresses.TSV_LevelStatus,
                    ["Pools of the Ancient Dead"] = Addresses.PAD_LevelStatus,
                    ["The Lake"] = Addresses.TL_LevelStatus,
                    ["The Crystal Caves"] = Addresses.CC_LevelStatus,
                    ["The Gallows Gauntlet"] = Addresses.GG_LevelStatus,
                    ["Asylum Grounds"] = Addresses.AG_LevelStatus,
                    ["Inside the Asylum"] = Addresses.IA_LevelStatus,
                    ["Pumpkin Gorge"] = Addresses.PG_LevelStatus,
                    ["Pumpkin Serpent"] = Addresses.PS_LevelStatus,
                    ["The Haunted Ruins"] = Addresses.HR_LevelStatus,
                    ["Ghost Ship"] = Addresses.GS_LevelStatus,
                    ["The Entrance Hall"] = Addresses.EH_LevelStatus,
                    ["The Time Device"] = Addresses.TD_LevelStatus
                },

                ["Runes"] = new Dictionary<string, uint>
                {
                    ["Chaos Rune"] = Addresses.ChaosRune,
                    ["Earth Rune"] = Addresses.EarthRune,
                    ["Star Rune"] = Addresses.StarRune,
                    ["Moon Rune"] = Addresses.MoonRune,
                    ["Time Rune"] = Addresses.TimeRune
                }


            };
        }




        // Hall of Heroes needs an overhaul. Not worth dealing with right now

        private static List<GenericItemsData> GetHallOfHeroesData()
        {
            List<GenericItemsData> hallOfHeroeVisits = new List<GenericItemsData>()
            {
                new GenericItemsData("Life Bottle: Hall of Heroes (Canny Tim)", Addresses.FakeAddress, "9999", true),
                new GenericItemsData("Life Bottle: Hall of Heroes (Ravenhooves The Archer)", Addresses.FakeAddress, "9999", true),
                new GenericItemsData("Life Bottle: Hall of Heroes (Dirk Steadfast)", Addresses.FakeAddress, "9999", true),
                new GenericItemsData("Equipment: Broadsword from Woden the Mighty - HH", Addresses.FakeAddress, "9999", true),
                new GenericItemsData("Equipment: Magic Sword from Dirk Steadfast - HH", Addresses.FakeAddress, "9999", true),
                new GenericItemsData("Equipment: Hammer from Stanyer Iron Hewer - HH", Addresses.FakeAddress, "9999", true),
                new GenericItemsData("Equipment: Axe from Bloodmonath- HH", Addresses.FakeAddress, "9999", true),
                new GenericItemsData("Equipment: Crossbow from Canny Tim - HH", Addresses.FakeAddress, "9999", true),
                new GenericItemsData("Equipment: Longbow from Ravenhooves The Archer - HH", Addresses.FakeAddress, "9999", true),
                new GenericItemsData("Equipment: Fire Longbow from Ravenhooves the Archer - HH", Addresses.FakeAddress, "9999", true),
                new GenericItemsData("Equipment: Magic Longbow from Ravenhooves the Archer - HH", Addresses.FakeAddress, "9999", true),
                new GenericItemsData("Equipment: Spear from Imanzi Shongama - HH", Addresses.FakeAddress, "9999", true),
                new GenericItemsData("Equipment: Lightning from Megwynne Stormbinder - HH", Addresses.FakeAddress, "9999", true),
                new GenericItemsData("Equipment: Gold Shield from Karl Sturngard - HH", Addresses.FakeAddress, "9999", true),
                new GenericItemsData("Energy Vial: Imanzi Shongama 1 - HH", Addresses.FakeAddress, "9999", true),
                new GenericItemsData("Energy Vial: Imanzi Shongama 2 - HH", Addresses.FakeAddress, "9999", true),
                new GenericItemsData("Energy Vial: Megwynne Stormbinder 1 - HH", Addresses.FakeAddress, "9999", true),
                new GenericItemsData("Energy Vial: Megwynne Stormbinder 2 - HH", Addresses.FakeAddress, "9999", true),
                new GenericItemsData("Energy Vial: Megwynne Stormbinder 3 - HH", Addresses.FakeAddress, "9999", true),
                new GenericItemsData("Gold Coins: Stanyer Iron Hewer 1 - HH", Addresses.FakeAddress, "9999", true),
                new GenericItemsData("Gold Coins: Stanyer Iron Hewer 2 - HH", Addresses.FakeAddress, "9999", true),
                new GenericItemsData("Gold Coins: Woden the Mighty 1 - HH", Addresses.FakeAddress, "9999", true),
                new GenericItemsData("Gold Coins: Woden the Mighty 2 - HH", Addresses.FakeAddress, "9999", true),
                new GenericItemsData("Gold Coins: Bloodmonath 1 - HH", Addresses.FakeAddress, "9999", true),
                new GenericItemsData("Gold Coins: Bloodmonath 2 - HH", Addresses.FakeAddress, "9999", true),
                new GenericItemsData("Gold Coins: Bloodmonath 3 - HH", Addresses.FakeAddress, "9999", true),
                new GenericItemsData("Gold Coins: Karl Sturngard 1 - HH", Addresses.FakeAddress, "9999", true),
                new GenericItemsData("Gold Coins: Karl Sturngard 2 - HH", Addresses.FakeAddress, "9999", true),
                new GenericItemsData("Gold Coins: Karl Sturngard 3 - HH", Addresses.FakeAddress, "9999", true),
                new GenericItemsData("Gold Coins: Karl Sturngard 4 - HH", Addresses.FakeAddress, "9999", true)
            };
            return hallOfHeroeVisits;
        }

        private static List<GenericItemsData> GetDansCryptData()
        {
            List<GenericItemsData> dcLocations = new List<GenericItemsData>() {
                new GenericItemsData("Star Rune: Dan's Crypt", Addresses.DC_Pickup_StarRune, "32896"),
                new GenericItemsData("Life Bottle: Dan's Crypt", Addresses.DC_Pickup_LifeBottle, "32896"),
                new GenericItemsData("Life Bottle: Dan's Crypt - Behind Wall",Addresses.DC_Pickup_LifeBottleWall, "32896"),
                new GenericItemsData("Equipment: Small Sword - DC", Addresses.DC_Pickup_Shortsword, "32896"),
                new GenericItemsData("Equipment: Copper Shield in Chest - DC", Addresses.DC_Pickup_CopperShield, "32896", true), // <--- This one doesn't work (chest)
                new GenericItemsData("Equipment: Daggers - DC", Addresses.DC_Pickup_Daggers, "32896"),
                new GenericItemsData("Gold Coins: Over the water - DC",Addresses.DC_Pickup_GoldCoinsOverWater, "32896"),
                new GenericItemsData("Gold Coins: Behind Wall in Crypt - Left - DC", Addresses.DC_Pickup_GoldCoinsBehindWallLeft, "32896"),
                new GenericItemsData("Gold Coins: Behind Wall in Crypt - Right - DC", Addresses.DC_Pickup_GoldCoinsBehindWallRight, "32896"),
                new GenericItemsData("Cleared: Dan's Crypt", Addresses.DC_LevelStatus, "16"),

            };
            return dcLocations;
        }

        private static List<GenericItemsData> GetTheGraveyardData()
        {
            List<GenericItemsData> tgLocations = new List<GenericItemsData>() {

                new GenericItemsData("Life Bottle: The Graveyard", Addresses.TG_Pickup_LifePotion, "32896"),
                new GenericItemsData("Earth Rune: The Graveyard", Addresses.TG_Pickup_EarthRune, "32896"),
                new GenericItemsData("Chaos Rune: The Graveyard", Addresses.TG_Pickup_ChaosRune, "32896"),
                new GenericItemsData("Equipment: Copper Shield - TG", Addresses.TG_Pickup_CopperShield, "32896", true),
                new GenericItemsData("Gold Coins: Bag at Start - TG", Addresses.TG_Pickup_GoldCoinsBagAtStart, "32896"),
                new GenericItemsData("Gold Coins: Near Chaos Rune - TG", Addresses.TG_Pickup_GoldCoinsNearChaosRune, "32896"),
                new GenericItemsData("Gold Coins: Life Bottle Left Chest - TG", Addresses.TG_Pickup_GoldCoinsLifePotionLeftChest, "32896"),
                new GenericItemsData("Gold Coins: Life Bottle Right Chest - TG", Addresses.TG_Pickup_GoldCoinsLifePotionRightChest, "32896"),
                new GenericItemsData("Gold Coins: Shop Chest - TG", Addresses.TG_Pickup_GoldCoinsShopChest, "32896"),
                new GenericItemsData("Gold Coins: Bag Near Hill Fountain - TG", Addresses.TG_Pickup_GoldCoinsBagNearHillFountain, "32896"),
                new GenericItemsData("Cleared: The Graveyard", Addresses.TG_LevelStatus, "16"),
                new GenericItemsData("Chalice: The Graveyard", Addresses.TG_Pickup_Chalice, "32896"),

            };
            return tgLocations;
        }

        private static List<GenericItemsData> GetCemeteryHillData()
        {
            List<GenericItemsData> chLocations = new List<GenericItemsData>() {

                new GenericItemsData("Key Item: Witches Talisman - CH", Addresses.CH_Pickup_WitchTalisman, "32896"),
                new GenericItemsData("Equipment: Copper Shield 1 - CH", Addresses.CH_Pickup_CopperShield1stOnHill, "32896", true),
                new GenericItemsData("Equipment: Copper Shield 2 - CH", Addresses.CH_Pickup_CopperShield2ndOnHill, "32896", true),
                new GenericItemsData("Equipment: Copper Shield 3 - CH", Addresses.CH_Pickup_CopperShield3rdOnHill, "32896", true),
                new GenericItemsData("Equipment: Club - CH", Addresses.CH_Pickup_Club, "32896", true),
                new GenericItemsData("Equipment: Copper Shield in Arena - CH", Addresses.CH_Pickup_CopperShieldArena, "32896"),
                new GenericItemsData("Energy Vial: Near Shop - CH", Addresses.CH_Pickup_EnergyVialNearShop, "32896"),
                new GenericItemsData("Energy Vial: Arena - CH", Addresses.CH_Pickup_EnergyVialArena, "32896"),
                new GenericItemsData("Gold Coins: Near Boulder Entrance - CH", Addresses.CH_Pickup_GoldCoinsNearBoulderEntrance, "32896"),
                new GenericItemsData("Gold Coins: Up Hill 1 - CH", Addresses.CH_Pickup_GoldCoinsUpHill1, "32896"),
                new GenericItemsData("Gold Coins: Up Hill 2 - CH", Addresses.CH_Pickup_GoldCoinsUpHill2, "32896"),
                new GenericItemsData("Gold Coins: Chest at Exit - CH", Addresses.CH_Pickup_GoldCoinsChestAtExit, "32896"),
                new GenericItemsData("Gold Coins: Chest in Arena - CH", Addresses.CH_Pickup_GoldCoinsChestInArena, "32896"),
                new GenericItemsData("Cleared: Cemetery Hill", Addresses.CH_LevelStatus, "16"),
                new GenericItemsData("Chalice: Cemetery Hill", Addresses.CH_Pickup_Chalice, "32896"),
            };
            return chLocations;
        }

        private static List<GenericItemsData> GetHilltopMausoleumData()
        {
            List<GenericItemsData> hmLocations = new List<GenericItemsData>() {
                new GenericItemsData("Key Item: Sheet Music - HM", Addresses.HM_Pickup_SheetMusic, "32896"),
                new GenericItemsData("Key Item: Skull Key - HM", Addresses.HM_Pickup_GlassDemonSkullKey, "32896"),
                new GenericItemsData("Chaos Rune: The Hilltop Mausoleum", Addresses.HM_Pickup_ChaosRune, "32896"),
                new GenericItemsData("Earth Rune: The Hilltop Mausoleum", Addresses.HM_Pickup_EarthRune, "32896"),
                new GenericItemsData("Moon Rune: The Hilltop Mausoleum", Addresses.HM_Pickup_MoonRune, "32896"),
                new GenericItemsData("Equipment: Club near Broken Benches - HM", Addresses.HM_Pickup_ClubBrokenBenches, "32896", true),
                new GenericItemsData("Equipment: Daggers near Block Puzzle - HM", Addresses.HM_Pickup_DaggersBlockPuzzle, "32896", true),
                new GenericItemsData("Equipment: Copper Shield near Block Puzzle - HM", Addresses.HM_Pickup_CopperShieldBlockPuzzle, "32896"),
                new GenericItemsData("Energy Vial: Right Coffin - HM", Addresses.HM_Pickup_EnergyVialRightCoffin, "32896"),
                new GenericItemsData("Energy Vial: Near Rune on Left Ramp - HM", Addresses.HM_Pickup_EnergyVialNearRuneLeftRamp, "32896"),
                new GenericItemsData("Energy Vial: Phantom of the Opera on Left - HM", Addresses.HM_Pickup_EnergyVialPhantomOfTheOperaLeft, "32896"),
                new GenericItemsData("Energy Vial: Phantom of the Opera on Right - HM", Addresses.HM_Pickup_EnergyVialPhantomOfTheOperaRight, "32896"),
                new GenericItemsData("Energy Vial: Moon Room - HM", Addresses.HM_Pickup_EnergyVialMoonRoom, "32896"),
                new GenericItemsData("Gold Coins: Left Coffin - HM", Addresses.HM_Pickup_GoldCoinsLeftCoffin, "32896"),
                new GenericItemsData("Gold Coins: After Earth Rune Door - HM", Addresses.HM_Pickup_GoldCoinsAfterEarthRuneDoor, "32896"),
                new GenericItemsData("Gold Coins: Chest in Moon Room - HM", Addresses.HM_Pickup_GoldCoinsChestInMoonRoom, "32896"),
                new GenericItemsData("Gold Coins: Gold Chest at Phantom of the Opera 1 - HM", Addresses.HM_Pickup_GoldChestPhantomOfTheOpera1, "32896"),
                new GenericItemsData("Gold Coins: Gold Chest at Phantom of the Opera 2 - HM", Addresses.HM_Pickup_GoldChestPhantomOfTheOpera2, "32896"),
                new GenericItemsData("Gold Coins: Gold Chest at Phantom of the Opera 3 - HM", Addresses.HM_Pickup_GoldChestPhantomOfTheOpera3, "32896"),
                new GenericItemsData("Cleared: The Hilltop Mausoleum", Addresses.HM_LevelStatus, "16"),
                new GenericItemsData("Chalice: The Hilltop Mausoleum", Addresses.HM_Pickup_Chalice, "32896"),
            };
            return hmLocations;
        }

        private static List<GenericItemsData> GetReturnToTheGraveyardData()
        {
            List<GenericItemsData> rtgLocations = new List<GenericItemsData>() {
                new GenericItemsData("Star Rune: Return to the Graveyard", Addresses.RTG_Pickup_StarRune, "32896"),
                new GenericItemsData("Equipment: Silver Shield in Chest at Shop", Addresses.RTG_Pickup_SilverShieldChestAtShop, "32896", true),
                new GenericItemsData("Skill: Daring Dash", Addresses.DaringDashSkill, "32896"),
                new GenericItemsData("Energy Vial: Coffin Area West - RTG", Addresses.RTG_Pickup_EnergyVialCoffinAreaWest, "32896"),
                new GenericItemsData("Energy Vial: Coffin Area East - RTG", Addresses.RTG_Pickup_EnergyVialCoffinAreaEast, "32896"),
                new GenericItemsData("Energy Vial: Below Shop - RTG", Addresses.RTG_Pickup_EnergyVialBelowShop, "32896"),
                new GenericItemsData("Energy Vial: Undertakers Entrance - RTG", Addresses.RTG_Pickup_EnergyVialUndertakersEntrance, "32896"),
                new GenericItemsData("Energy Vial: Cliffs Right - RTG", Addresses.RTG_Pickup_EnergyVialCliffsRight, "32896"),
                new GenericItemsData("Energy Vial: Cliffs Left - RTG", Addresses.RTG_Pickup_EnergyVialCliffsLeft, "32896"),
                new GenericItemsData("Gold Coins: Chest in Coffin Area 1 - RTG", Addresses.RTG_Pickup_GoldCoinsChestInCoffinArea1, "32896"),
                new GenericItemsData("Gold Coins: Chest in Coffin Area 2 - RTG", Addresses.RTG_Pickup_GoldCoinsChestInCoffinArea2, "32896"),
                new GenericItemsData("Gold Coins: Chest in Coffin Area 3 - RTG", Addresses.RTG_Pickup_GoldCoinsChestInCoffinArea3, "32896"),
                new GenericItemsData("Gold Coins: Chest in Coffin Area 4 - RTG", Addresses.RTG_Pickup_GoldCoinsChestInCoffinArea4, "32896"),
                new GenericItemsData("Gold Coins: Chest in Coffin Area 5 - RTG", Addresses.RTG_Pickup_GoldCoinsChestInCoffinArea5, "32896"),
                new GenericItemsData("Gold Coins: Bag above Coffin Area - RTG", Addresses.RTG_Pickup_GoldCoinsBagAboveCoffinArea, "32896"),
                new GenericItemsData("Gold Coins: Bag after Bridge - RTG", Addresses.RTG_Pickup_GoldCoinsBagAfterBridge, "32896"),
                new GenericItemsData("Gold Coins: Bag at Shop - RTG", Addresses.RTG_Pickup_GoldCoinsBagAtShop, "32896"),
                new GenericItemsData("Gold Coins: Bag at Closed Gate - RTG", Addresses.RTG_Pickup_GoldCoinsBagAtClosedGate, "32896"),
                new GenericItemsData("Gold Coins: Chest on Island - RTG", Addresses.RTG_Pickup_GoldCoinsChestOnIsland, "32896"),
                new GenericItemsData("Gold Coins: Undertakers Entrance - RTG", Addresses.RTG_Pickup_GoldCoinsUndertakersEntrance, "32896"),
                new GenericItemsData("Gold Coins: Cliffs Left - RTG", Addresses.RTG_Pickup_GoldCoinsCliffsLeft, "32896"),
                new GenericItemsData("Cleared: Return to the Graveyard", Addresses.RTG_LevelStatus, "16"),
                new GenericItemsData("Chalice: Return to the Graveyard", Addresses.RTG_Pickup_Chalice, "32896"),
            };
            return rtgLocations;
        }

        private static List<GenericItemsData> GetScarecrowFieldsData()
        {
            List<GenericItemsData> sfLocations = new List<GenericItemsData>() {
                new GenericItemsData("Life Bottle: Scarecrow Fields", Addresses.SF_Pickup_LifePotion, "32896"),
                new GenericItemsData("Key Item: Harvester Parts - SF", Addresses.SF_Pickup_HarvesterPart, "32896"),
                new GenericItemsData("Chaos Rune: Scarecrow Fields", Addresses.SF_Pickup_ChaosRune, "32896", true),
                new GenericItemsData("Earth Rune: Scarecrow Fields", Addresses.SF_Pickup_EarthRune, "32896"),
                new GenericItemsData("Moon Rune: Scarecrow Fields", Addresses.SF_Pickup_MoonRune, "32896", true),
                new GenericItemsData("Equipment: Club Inside Hut - SF", Addresses.SF_Pickup_ClubInsideHut, "32896", true),
                new GenericItemsData("Equipment: Silver Shield Behind Windmill - SF", Addresses.SF_Pickup_SilverShieldBehindWindmill, "32896", true),
                new GenericItemsData("Equipment: Copper Shield in Chest In the Barn - SF", Addresses.SF_Pickup_CopperShieldChestInTheBarn, "32896", true),
                new GenericItemsData("Energy Vial: Right of fire near Moon Door - SF", Addresses.SF_Pickup_EnergyVialRightOfFireNearMoonDoor, "32896"),
                new GenericItemsData("Energy Vial: Cornfield Path - SF", Addresses.SF_Pickup_EnergyVialCornfieldPath, "32896"),
                new GenericItemsData("Gold Coins: Haystack at Beginning - SF", Addresses.SF_Pickup_GoldCoinsHaystackAtBeginning, "32896"),
                new GenericItemsData("Gold Coins: Chest in Haystack near Moon Door - SF", Addresses.SF_Pickup_GoldCoinsChestInHaystackNearMoonDoor, "32896"),
                new GenericItemsData("Gold Coins: Left of fire near Moon Door - SF", Addresses.SF_Pickup_GoldCoinsLeftOfFireNearMoonDoor, "32896"),
                new GenericItemsData("Gold Coins: Bag in the Barn - SF", Addresses.SF_Pickup_GoldCoinsBagInTheBarn, "32896"),
                new GenericItemsData("Gold Coins: Cornfield Square near Barn - SF", Addresses.SF_Pickup_GoldCoinsCornfieldSquareNearBarn, "32896"),
                new GenericItemsData("Gold Coins: Cornfield Path 1 - SF", Addresses.SF_Pickup_GoldCoinsCornfieldPath1, "32896"),
                new GenericItemsData("Gold Coins: Chest Under Haybail - SF", Addresses.SF_Pickup_GoldCoinsChestUnderHaybail, "32896", true),
                new GenericItemsData("Gold Coins: Bag under Barn Haybail - SF", Addresses.SF_Pickup_GoldCoinsBagUnderBarnHaybail, "32896", true),
                new GenericItemsData("Gold Coins: Bag in the Press - SF", Addresses.SF_Pickup_GoldCoinsBagInThePress, "32896"),
                new GenericItemsData("Gold Coins: Bag in the Spinner - SF", Addresses.SF_Pickup_GoldCoinsBagInTheSpinner, "32896"),
                new GenericItemsData("Gold Coins: Chest next to Harvester Part - SF", Addresses.SF_Pickup_GoldCoinsChestNextToHarvesterPart, "32896"),
                new GenericItemsData("Gold Coins: Chest Next to Chalice - SF", Addresses.SF_Pickup_GoldCoinsChestNextToChalice, "32896"),
                new GenericItemsData("Cleared: Scarecrow Fields", Addresses.SF_LevelStatus, "16"),
                new GenericItemsData("Chalice: Scarecrow Fields", Addresses.SF_Pickup_Chalice, "32896"),
            };
            return sfLocations;
        }

        private static List<GenericItemsData> GetAnthillData()
        {
            List<GenericItemsData> ahLocations = new List<GenericItemsData>() {
                new GenericItemsData("Key Item: Amber Piece 1 - TA", Addresses.TA_Pickup_Amber1, "32896"),
                new GenericItemsData("Key Item: Amber Piece 2 - TA", Addresses.TA_Pickup_Amber2, "32896"),
                new GenericItemsData("Key Item: Amber Piece 3 - TA", Addresses.TA_Pickup_Amber3, "32896"),
                new GenericItemsData("Key Item: Amber Piece 4 - TA", Addresses.TA_Pickup_Amber4, "32896"),
                new GenericItemsData("Key Item: Amber Piece 5 - TA", Addresses.TA_Pickup_Amber5, "32896"),
                new GenericItemsData("Key Item: Amber Piece 6 - TA", Addresses.TA_Pickup_Amber6, "32896"),
                new GenericItemsData("Key Item: Amber Piece 7 - TA", Addresses.TA_Pickup_Amber7, "32896"),
                new GenericItemsData("Key Item: Amber Piece 8 - TA", Addresses.TA_Pickup_Amber8, "32896"),
                new GenericItemsData("Key Item: Amber Piece 9 - TA", Addresses.TA_Pickup_Amber9, "32896"),
                new GenericItemsData("Key Item: Amber Piece 10 - TA", Addresses.TA_Pickup_Amber10, "32896"),
                new GenericItemsData("Equipment: Club in Chest at Barrier - TA", Addresses.TA_Pickup_ClubChestAtBarrier, "32896", true),
                new GenericItemsData("Equipment: Chicken Drumsticks - TA", Addresses.TA_LevelStatus, "16"),
                new GenericItemsData("Energy Vial: Before Fairy 1 - TA", Addresses.TA_Pickup_EnergyVialBeforeFairy1, "32896"),
                new GenericItemsData("Energy Vial: After Amber 2 - TA", Addresses.TA_Pickup_EnergyVialAfterAmber2, "32896"),
                new GenericItemsData("Energy Vial: Fairy 2 Room Center - TA", Addresses.TA_Pickup_EnergyVialFairy2RoomCenter, "32896"),
                new GenericItemsData("Energy Vial: Fairy 3 - TA", Addresses.TA_Pickup_EnergyVialFairy3, "32896"),
                new GenericItemsData("Energy Vial: Birthing room exit - TA", Addresses.TA_Pickup_EnergyVialBirthingRoomExit, "32896"),
                new GenericItemsData("Gold Coins: Chest at Barrier Fairy - TA", Addresses.TA_Pickup_GoldCoinsChestAtBarrierFairy, "32896"),
                new GenericItemsData("Gold Coins: Maggot at Amber 2 - TA", Addresses.TA_Pickup_GoldCoinsMaggotAtAmber2, "32896", true),
                new GenericItemsData("Gold Coins: Maggot after Amber 2 - TA", Addresses.TA_Pickup_GoldCoinsMaggotAfterAmber2, "32896", true),
                new GenericItemsData("Gold Coins: Fairy 2 Room Center - TA",Addresses.TA_Pickup_GoldCoinsFairy2RoomCenter, "32896"),
                new GenericItemsData("Gold Coins: Fairy 2 Room Maggot - TA", Addresses.TA_Pickup_GoldCoinsFairy2RoomMaggot, "32896"),
                new GenericItemsData("Gold Coins: Maggots before Amber 4 - TA", Addresses.TA_Pickup_GoldCoinsMaggotsBeforeAmber4, "32896", true),
                new GenericItemsData("Gold Coins: Maggots at Amber 5 - TA", Addresses.TA_Pickup_GoldCoinsMaggotsAtAmber5, "32896", true),
                new GenericItemsData("Gold Coins: Maggots at Amber 7 - TA", Addresses.TA_Pickup_GoldCoinsMaggotsAtAmber7_1, "32896", true),
                new GenericItemsData("Gold Coins: Maggot in nest at Amber 7 - TA", Addresses.TA_Pickup_GoldCoinsMaggotInNestAtAmber7, "32896", true),
                new GenericItemsData("Gold Coins: Maggot in Nest - TA", Addresses.TA_Pickup_GoldCoinsMaggotInNest, "32896", true),
                new GenericItemsData("Gold Coins: Maggot after Fairy 4 - TA", Addresses.TA_Pickup_GoldCoinsMaggotAfterFairy4, "32896", true),
                new GenericItemsData("Gold Coins: Maggot after Fairy 4 in Nest - TA", Addresses.TA_Pickup_GoldCoinsMaggotAfterFairy4InNest, "32896", true),
                new GenericItemsData("Gold Coins: Maggot at Fairy 5 - TA", Addresses.TA_Pickup_GoldCoinsMaggotAtFairy5, "32896", true),
                new GenericItemsData("Gold Coins: Maggot near Amber 9 - TA", Addresses.TA_Pickup_GoldCoinsMaggotNearAmber9, "32896", true),
                new GenericItemsData("Gold Coins: Maggot near Shop - TA", Addresses.TA_Pickup_GoldCoinsMaggotNearShop, "32896", true),
                new GenericItemsData("Cleared: Ant Hill", Addresses.TA_LevelStatus, "16"),
                new GenericItemsData("Chalice: Ant Hill", Addresses.TA_LevelStatus, "19"),
            };
            return ahLocations;
        }

        private static List<GenericItemsData> GetEnchantedEarthData()
        {
            List<GenericItemsData> eeLocations = new List<GenericItemsData>() {

                new GenericItemsData("Key Item: Shadow Talisman - EE", Addresses.EE_Pickup_ShadowTalisman, "32896"),
                new GenericItemsData("Star Rune: Enchanted Earth", Addresses.EE_Pickup_StarRune, "32896"),
                new GenericItemsData("Earth Rune: Enchanted Earth", Addresses.EE_Pickup_EarthRune, "32896"),
                new GenericItemsData("Equipment: Copper Shield in Egg - EE", Addresses.EE_Pickup_CopperShieldInEgg, "32896", true),
                new GenericItemsData("Energy Vial: Shadow Talisman Cave - EE", Addresses.EE_Pickup_EnergyVialShadowTalismanCave, "32896"),
                new GenericItemsData("Energy Vial: Left of Tree Drop - EE", Addresses.EE_Pickup_EnergyVialLeftOfTreeDrop, "32896"),
                new GenericItemsData("Energy Vial: Right of Tree Drop - EE", Addresses.EE_Pickup_EnergyVialRightOfTreeDrop, "32896"),
                new GenericItemsData("Gold Coins: Bag Near Tree Hollow - EE", Addresses.EE_Pickup_GoldCoinsBagNearTreeHollow, "32896"),
                new GenericItemsData("Gold Coins: Bag Behind Big Tree - EE", Addresses.EE_Pickup_GoldCoinsBagBehindBigTree, "32896"),
                new GenericItemsData("Gold Coins: Chest In Egg - EE", Addresses.EE_Pickup_GoldCoinsChestInEgg, "32896", true),
                new GenericItemsData("Gold Coins: Bag at Cave Entrance - EE", Addresses.EE_Pickup_GoldCoinsBagAtCaveEntrance, "32896"),
                new GenericItemsData("Gold Coins: Bag in Talisman Cave - EE", Addresses.EE_Pickup_GoldCoinsBagInShadowTalismanCave, "32896"),
                new GenericItemsData("Gold Coins:Chest Near Barrier - EE", Addresses.EE_Pickup_GoldCoinsChestNearBarrier, "32896"),
                new GenericItemsData("Gold Coins: Chest Left of Fountain - EE", Addresses.EE_Pickup_GoldCoinsChestLeftOfFountain, "32896"),
                new GenericItemsData("Gold Coins: Chest Top of Fountain - EE", Addresses.EE_Pickup_GoldCoinsChestTopOfFountain, "32896"),
                new GenericItemsData("Gold Coins: Chest Right of Fountain - EE", Addresses.EE_Pickup_GoldCoinsChestRightOfFountain, "32896"),
                new GenericItemsData("Cleared: Enchanted Earth", Addresses.EE_LevelStatus, "16"),
                new GenericItemsData("Chalice: Enchanted Earth", Addresses.EE_Pickup_Chalice, "32896"),
            };
            return eeLocations;
        }

        private static List<GenericItemsData> GetTheSleepingVillageData()
        {
            List<GenericItemsData> tsvLocations = new List<GenericItemsData>() {
                new GenericItemsData("Earth Rune: Sleeping Village", Addresses.TSV_Pickup_EarthRune, "32896"),
                new GenericItemsData("Chaos Rune: Sleeping Village", Addresses.TSV_Pickup_ChaosRune, "32896"),
                new GenericItemsData("Moon Rune: Sleeping Village", Addresses.TSV_Pickup_MoonRune, "32896"),
                new GenericItemsData("Key Item: Safe Key - SV", Addresses.TSV_Pickup_SafeKey, "32896"),
                new GenericItemsData("Key Item: Shadow Artefact - SV", Addresses.TSV_Pickup_ShadowArtefact, "32896", true),
                new GenericItemsData("Key Item: Crucifix - SV", Addresses.TSV_Pickup_Crucifix, "32896", true),
                new GenericItemsData("Key Item: Landlords Bust - SV", Addresses.TSV_Pickup_LandlordsBust, "32896"),
                new GenericItemsData("Key Item: Crucifix Cast - SV", Addresses.TSV_Pickup_CrucifixCast, "32896"),
                new GenericItemsData("Equipment: Silver Shield in Blacksmiths - SV", Addresses.TSV_Pickup_SilverShieldInBlacksmiths, "32896", true),
                new GenericItemsData("Equipment: Club Chest under Inn Stairs - SV", Addresses.TSV_Pickup_ClubInChestUnderInnStairs, "32896", true),
                new GenericItemsData("Energy Vial: At Pond - SV", Addresses.TSV_Pickup_EnergyVialAtPond, "32896"),
                new GenericItemsData("Energy Vial: Bust Switch - SV", Addresses.TSV_Pickup_EnergyVialBustSwitch, "32896"),
                new GenericItemsData("Energy Vial: Near Exit - SV", Addresses.TSV_Pickup_EnergyVialNearExit, "32896"),
                new GenericItemsData("Energy Vial: Near Chalice - SV", Addresses.TSV_Pickup_EnergyVialNearChalice, "32896"),
                new GenericItemsData("Gold Coins: Bag in Left Barrel at Blacksmith - SV", Addresses.TSV_Pickup_GoldCoinsBagInLeftBarrelAtBlacksmith, "32896", true),
                new GenericItemsData("Gold Coins: Bag in Right Barrel at Blacksmith - SV", Addresses.TSV_Pickup_GoldCoinsBagInRightBarrelAtBlacksmith, "32896", true),
                new GenericItemsData("Gold Coins: Bag at Pond - SV", Addresses.TSV_Pickup_GoldCoinsBagAtPond, "32896"),
                new GenericItemsData("Gold Coins: Bag in Barrel at Inn - SV", Addresses.TSV_Pickup_GoldCoinsBagInBarrelAtInn, "32896", true),
                new GenericItemsData("Gold Coins: Bag in Barrel at bottom of Inn Stairs - SV", Addresses.TSV_Pickup_GoldCoinsBagInBarrelAtBottomOfInnStairs, "32896", true),
                new GenericItemsData("Gold Coins: Bag in Barrel Behind Inn Stairs - SV", Addresses.TSV_Pickup_GoldCoinsBagInBarrelBehindInnStairs, "32896", true),
                new GenericItemsData("Gold Coins: Bag In Top Bust Barrel - SV", Addresses.TSV_Pickup_GoldCoinsBagInTopBustBarrel, "32896", true),
                new GenericItemsData("Gold Coins: Bag In Switch Bust Barrel - SV", Addresses.TSV_Pickup_GoldCoinsBagInSwitchBustBarrel, "32896", true),
                new GenericItemsData("Gold Coins: Bag in Library - SV", Addresses.TSV_Pickup_GoldCoinsBagInLibrary, "32896"),
                new GenericItemsData("Gold Coins: Bag at Top of table - SV", Addresses.TSV_Pickup_GoldCoinsBagAtTopOfTable, "32896"),
                new GenericItemsData("Gold Coins: Bag at Bottom of table - SV", Addresses.TSV_Pickup_GoldCoinsBagAtBottomOfTable, "32896"),
                new GenericItemsData("Gold Coins: Chest next to Chalice - SV", Addresses.TSV_Pickup_GoldCoinsChestNextToChalice, "32896"),
                new GenericItemsData("Cleared: Sleeping Village", Addresses.TSV_LevelStatus, "16"),
                new GenericItemsData("Chalice: Sleeping Village", Addresses.TSV_Pickup_Chalice, "32896"),
            };
            return tsvLocations;
        }

        private static List<GenericItemsData> GetPoolsOfTheAncientDeadData()
        {
            List<GenericItemsData> padLocations = new List<GenericItemsData>() {
                new GenericItemsData("Life Bottle: Pools of the Ancient Dead", Addresses.PAD_Pickup_LifeBottle, "32896"),
                new GenericItemsData("Chaos Rune: Pools of the Ancient Dead", Addresses.PAD_Pickup_ChaosRune, "32896"),
                new GenericItemsData("Key Item: Soul Helmet 1 - PAD", Addresses.PAD_Pickup_LostSoul1, "32896"),
                new GenericItemsData("Key Item: Soul Helmet 2 - PAD", Addresses.PAD_Pickup_LostSoul2, "32896"),
                new GenericItemsData("Key Item: Soul Helmet 3 - PAD", Addresses.PAD_Pickup_LostSoul3, "32896"),
                new GenericItemsData("Key Item: Soul Helmet 4 - PAD", Addresses.PAD_Pickup_LostSoul4, "32896"),
                new GenericItemsData("Key Item: Soul Helmet 5 - PAD", Addresses.PAD_Pickup_LostSoul5, "32896"),
                new GenericItemsData("Key Item: Soul Helmet 6 - PAD", Addresses.PAD_Pickup_LostSoul6, "32896"),
                new GenericItemsData("Key Item: Soul Helmet 7 - PAD", Addresses.PAD_Pickup_LostSoul7, "32896"),
                new GenericItemsData("Key Item: Soul Helmet 8 - PAD", Addresses.PAD_Pickup_LostSoul8, "32896"),
                new GenericItemsData("Equipment: Silver Shield in Chest Near Soul 5 - PAD", Addresses.PAD_Pickup_SilverShieldInChestNearSoul5, "32896", true),
                new GenericItemsData("Energy Vial: Broken Structure near Entrance - PAD", Addresses.PAD_Pickup_EnergyVialBrokenStructureNearEntrance, "32896"),
                new GenericItemsData("Energy Vial: Next to Lost Soul 3 - PAD", Addresses.PAD_Pickup_EnergyVialNextToLostSoul3, "32896"),
                new GenericItemsData("Energy Vial: Near Gate - PAD", Addresses.PAD_Pickup_EnergyVialNearGate, "32896"),
                new GenericItemsData("Energy Vial: Chariot Right - PAD", Addresses.PAD_Pickup_EnergyVialChariotRight, "32896"),
                new GenericItemsData("Energy Vial: Chariot Left - PAD", Addresses.PAD_Pickup_EnergyVialChariotLeft, "32896"),
                new GenericItemsData("Energy Vial: Jump Spot 1 - PAD", Addresses.PAD_Pickup_EnergyVialJumpSpot1, "32896"),
                new GenericItemsData("Energy Vial: Jump Spot 2 - PAD", Addresses.PAD_Pickup_EnergyVialJumpSpot2, "32896"),
                new GenericItemsData("Gold Coins: Bag at Entrance - PAD", Addresses.PAD_Pickup_GoldCoinsBagAtEntrance, "32896"),
                new GenericItemsData("Gold Coins: Bag on Island Near Soul 2 - PAD", Addresses.PAD_Pickup_GoldCoinsBagOnIslandNearSoul2, "32896"),
                new GenericItemsData("Gold Coins: Jump Spot 1 - PAD", Addresses.PAD_Pickup_GoldCoinsJumpSpot1, "32896"),
                new GenericItemsData("Gold Coins: Jump Spot 2 - PAD", Addresses.PAD_Pickup_GoldCoinsJumpSpot2, "32896"),
                new GenericItemsData("Cleared: Pools of the Ancient Dead", Addresses.PAD_LevelStatus, "16"),
                new GenericItemsData("Chalice: Pools of the Ancient Dead", Addresses.PAD_Pickup_Chalice, "32896"),
            };
            return padLocations;
        }

        private static List<GenericItemsData> GetTheLakeData()
        {
            List<GenericItemsData> tlLocations = new List<GenericItemsData>() {
                new GenericItemsData("Chaos Rune: The Lake", Addresses.TL_Pickup_ChaosRune, "32896"),
                new GenericItemsData("Earth Rune: The Lake", Addresses.TL_Pickup_EarthRune, "32896"),
                new GenericItemsData("Star Rune: The Lake", Addresses.TL_Pickup_StarRune, "32896"),
                new GenericItemsData("Time Rune: The Lake", Addresses.TL_Pickup_TimeRune, "32896"),
                new GenericItemsData("Equipment: Silver Shield In Whirlpool - TL", Addresses.TL_Pickup_SilverShieldInWhirlpool, "32896", true),
                new GenericItemsData("Energy Vial: Flooded House - TL", Addresses.TL_Pickup_EnergyVialFloodedHouse, "32896"),
                new GenericItemsData("Energy Vial: Whirpool Wind 1 - TL", Addresses.TL_Pickup_EnergyVialWhirlpoolWind1, "32896"),
                new GenericItemsData("Energy Vial: Whirpool Wind 2 - TL", Addresses.TL_Pickup_EnergyVialWhirlpoolWind2, "32896"),
                new GenericItemsData("Gold Coins: Bag Outside Flooded House - TL", Addresses.TL_Pickup_GoldCoinsBagOutsideFloodedHouse, "32896"),
                new GenericItemsData("Gold Coins: Bag Near Closed Gate - TL", Addresses.TL_Pickup_GoldCoinsBagNearClosedGate, "32896"),
                new GenericItemsData("Gold Coins: Bag at the Whirlpool Entrance - TL", Addresses.TL_Pickup_GoldCoinsBagAtTheWhirlpoolEntrance, "32896"),
                new GenericItemsData("Gold Coins: Whirlpool Wind 1 - TL", Addresses.TL_Pickup_GoldCoinsWhirlpoolWind1, "32896"),
                new GenericItemsData("Gold Coins: Whirlpool Wind 2 - TL", Addresses.TL_Pickup_GoldCoinsWhirlpoolWind2, "32896"),
                new GenericItemsData("Gold Coins: Outside Whirlpool Exit - TL", Addresses.TL_Pickup_GoldCoinsOutsideWhirlpoolExit, "32896"),
                new GenericItemsData("Cleared: The Lake", Addresses.TL_LevelStatus, "16"),
                new GenericItemsData("Chalice: The Lake", Addresses.TL_Pickup_Chalice, "32896"),
            };
            return tlLocations;
        }

        private static List<GenericItemsData> GetTheCrystalCavesData()
        {
            List<GenericItemsData> ccLocations = new List<GenericItemsData>() {

                new GenericItemsData("Earth Rune: The Crystal Caves", Addresses.CC_Pickup_EarthRune, "32896"),
                new GenericItemsData("Star Rune: The Crystal Caves", Addresses.CC_Pickup_StarRune, "32896"),
                new GenericItemsData("Equipment: Silver Shield in Crystal - CC", Addresses.CC_Pickup_SilverShieldInCrystal, "32896", true),
                new GenericItemsData("Equipment: Dragon Armour - CC", Addresses.CC_Pickup_DragonArmour, "32896"),
                new GenericItemsData("Energy Vial: Dragon Room 1st Platform - CC", Addresses.CC_Pickup_EnergyVialDragonRoom1stPlatform, "32896"),
                new GenericItemsData("Energy Vial: Dragon Room 3rd Platform - CC", Addresses.CC_Pickup_EnergyVialDragonRoom3rdPlatform, "32896"),
                new GenericItemsData("Gold Coins: Bag at bottom of winding staircase  - CC", Addresses.CC_Pickup_GoldCoinsBagAtWindingStaircase, "32896"),
                new GenericItemsData("Gold Coins: Chest in Crystal after Pool - CC", Addresses.CC_Pickup_GoldCoinsChestInCrystalAfterPool, "32896", true),
                new GenericItemsData("Gold Coins: Bag in Crystal at Start - CC", Addresses.CC_Pickup_GoldCoinsBagInCrystalAtStart, "32896", true),
                new GenericItemsData("Gold Coins: Bag in Spinner - CC", Addresses.CC_Pickup_GoldCoinsBagInSpinner, "32896"),
                new GenericItemsData("Gold Coins: Bag near Silver Shield - CC", Addresses.CC_Pickup_GoldCoinsBagNearSilverShield, "32896"),
                new GenericItemsData("Gold Coins: Chest in Crystal After Earth Door - CC", Addresses.CC_Pickup_GoldCoinsChestInCrystalAfterEarthDoor, "32896", true),
                new GenericItemsData("Gold Coins: Bag in Dragon Room 1 1st Platform - CC", Addresses.CC_Pickup_GoldCoinsBagInDragonRoom11stPlatform, "32896"),
                new GenericItemsData("Gold Coins: Bag in Dragon Room 2 1st Platform - CC", Addresses.CC_Pickup_GoldCoinsBagInDragonRoom21stPlatform, "32896"),
                new GenericItemsData("Gold Coins: Chest in Dragon Room 1st Platform - CC", Addresses.CC_Pickup_GoldCoinsChestInDragonRoom1stPlatform, "32896"),
                new GenericItemsData("Gold Coins: Bag in Dragon Room 2nd Platform - CC", Addresses.CC_Pickup_GoldCoinsBagInDragonRoom2ndPlatform, "32896"),
                new GenericItemsData("Gold Coins: Bag in Dragon Room 1 3rd Platform - CC", Addresses.CC_Pickup_GoldCoinsBagInDragonRoom13rdPlatform, "32896"),
                new GenericItemsData("Gold Coins: Bag in Dragon Room 2 3rd Platform - CC", Addresses.CC_Pickup_GoldCoinsBagInDragonRoom23rdPlatform, "32896"),
                new GenericItemsData("Gold Coins: Chest in Dragon Room 3rd Platform - CC", Addresses.CC_Pickup_GoldCoinsChestInDragonRoom3rdPlatform, "32896"),
                new GenericItemsData("Gold Coins: Bag in Dragon Room 4th Platform 1 - CC", Addresses.CC_Pickup_GoldCoinsBagInDragonRoom4thPlatform1, "32896"),
                new GenericItemsData("Gold Coins: Chest in Dragon Room 4th Platform - CC", Addresses.CC_Pickup_GoldCoinsChestInDragonRoom4thPlatform, "32896"),
                new GenericItemsData("Gold Coins: Bag in Dragon Room 4th Platform 2 - CC", Addresses.CC_Pickup_GoldCoinsBagInDragonRoom4thPlatform2, "32896"),
                new GenericItemsData("Gold Coins: Bag on Left of Pool - CC", Addresses.CC_Pickup_GoldCoinsBagOnLeftOfPool, "32896"),
                new GenericItemsData("Gold Coins: Bag on Right of Pool - CC", Addresses.CC_Pickup_GoldCoinsBagOnRightOfPool, "32896"),
                new GenericItemsData("Cleared: The Crystal Caves", Addresses.CC_LevelStatus, "16"),
                new GenericItemsData("Chalice: The Crystal Caves", Addresses.CC_Pickup_Chalice, "32896"),
            };
            return ccLocations;
        }
        private static List<GenericItemsData> GetGallowsGauntletData()
        {
            List<GenericItemsData> ggLocations = new List<GenericItemsData>() {
                new GenericItemsData("Star Rune: The Gallows Gauntlet", Addresses.GG_Pickup_StarRune, "32896"),
                new GenericItemsData("Equipment: Silver Shield in Chest Near Exit - GG", Addresses.GG_Pickup_SilverShieldInChestNearExit, "32896", true),
                new GenericItemsData("Energy Vial: Near Chalice - GG", Addresses.GG_Pickup_EnergyVialNearChalice, "32896"),
                new GenericItemsData("Gold Coins: Bag Behind Stone Dragon 1 - GG", Addresses.GG_Pickup_GoldCoinsBagBehindStoneDragon1, "32896"),
                new GenericItemsData("Gold Coins: Bag Behind Stone Dragon 2 - GG", Addresses.GG_Pickup_GoldCoinsBagBehindStoneDragon2, "32896"),
                new GenericItemsData("Gold Coins: Chest at Serpent - GG", Addresses.GG_Pickup_GoldCoinsChestAtSerpent, "32896"),
                new GenericItemsData("Gold Coins: Chest Near Star Entrance - GG", Addresses.GG_Pickup_GoldCoinsChestNearStarEntrance, "32896"),
                new GenericItemsData("Cleared: The Gallows Gauntlet", Addresses.GG_LevelStatus, "16"),
                new GenericItemsData("Chalice: The Gallows Gauntlet", Addresses.GG_Pickup_Chalice, "32896"),
            };
            return ggLocations;
        }
        private static List<GenericItemsData> GetTheAsylumGroundsData()
        {
            List<GenericItemsData> agLocations = new List<GenericItemsData>() {
                new GenericItemsData("Chaos Rune: Asylum Grounds", Addresses.AG_Pickup_ChaosRune, "32896"),
                new GenericItemsData("Equipment: Silver Shield in Chest Behind Door - AG", Addresses.AG_Pickup_SilverShieldInChestBehindDoor, "32896", true),
                new GenericItemsData("Energy Vial: Near Bishop - AG", Addresses.AG_Pickup_EnergyVialNearBishop, "32896"),
                new GenericItemsData("Energy Vial: Near King - AG", Addresses.AG_Pickup_EnergyVialNearKing, "32896"),
                new GenericItemsData("Gold Coins: Bag in Bell Grave Near Bell - AG", Addresses.AG_Pickup_GoldCoinsBagInBellGraveNearBell, "32896"),
                new GenericItemsData("Gold Coins: Bag in Bell Grave Near Entrance - AG", Addresses.AG_Pickup_GoldCoinsBagInBellGraveNearEntrance, "32896"),
                new GenericItemsData("Gold Coins: Bag Near Shooting Statue - AG", Addresses.AG_Pickup_GoldCoinsBagNearShootingStatue, "32896"),
                new GenericItemsData("Gold Coins: Bag in Rat Grave - AG", Addresses.AG_Pickup_GoldCoinsBagInRatGrave, "32896"),
                new GenericItemsData("Gold Coins: Behind Chaos Gate - AG", Addresses.AG_Pickup_GoldCoinsBehindChaosGate, "32896"),
                new GenericItemsData("Gold Coins: Behind Elephant in Grave - AG", Addresses.AG_Pickup_GoldCoinsBehindElephantInGrave, "32896"),
                new GenericItemsData("Cleared: Asylum Grounds", Addresses.AG_LevelStatus, "16"),
                new GenericItemsData("Chalice: Asylum Grounds", Addresses.AG_Pickup_Chalice, "32896"),
            };
            return agLocations;
        }

        private static List<GenericItemsData> GetInsideTheAsylumData()
        {
            List<GenericItemsData> iaLocations = new List<GenericItemsData>() {
                new GenericItemsData("Earth Rune: Inside the Asylum", Addresses.IA_Pickup_EarthRune, "32896"),
                new GenericItemsData("Key Item: Dragon Gem - IA", Addresses.IA_Pickup_DragonGem, "32896"),
                new GenericItemsData("Equipment: Silver Shield in Bat Room - IA", Addresses.IA_Pickup_SilverShieldInBatRoom, "32896", true),
                new GenericItemsData("Energy Vial: Bat Room - IA", Addresses.IA_Pickup_EnergyVialBatRoom, "32896", true),
                new GenericItemsData("Energy Vial: Asylumn Room 1 - IA", Addresses.IA_Pickup_EnergyVialAsylumRoom1, "32896", true),
                new GenericItemsData("Energy Vial: Asylumn Room 2 - IA", Addresses.IA_Pickup_EnergyVialAsylumRoom2, "32896", true),
                new GenericItemsData("Gold Coins: Bag in Bat Room Left - IA", Addresses.IA_Pickup_GoldCoinsBagInBatRoomLeft, "32896", true),
                new GenericItemsData("Gold Coins: Chest in Bat Room - IA", Addresses.IA_Pickup_GoldCoinsChestInBatRoom, "32896", true),
                new GenericItemsData("Gold Coins: Bag in Bat Room Centre - IA", Addresses.IA_Pickup_GoldCoinsBagInBatRoomCentre, "32896", true),
                new GenericItemsData("Gold Coins: Bag in Bat Room Right - IA", Addresses.IA_Pickup_GoldCoinsBagInBatRoomRight, "32896", true),
                new GenericItemsData("Gold Coins: Bag in Asylumn Room - IA", Addresses.IA_Pickup_GoldCoinsBagInAsylumRoom, "32896", true),
                new GenericItemsData("Gold Coins: Bag in Sewer Prison Entrance - IA", Addresses.IA_Pickup_GoldCoinsBagInSewerPrisonEntrance, "32896"),
                new GenericItemsData("Gold Coins: Bag on Sewer Prison Bench - IA", Addresses.IA_Pickup_GoldCoinsBagOnSewerPrisonBench, "32896"),
                new GenericItemsData("Cleared: Inside the Asylum", Addresses.IA_LevelStatus, "16"),
                new GenericItemsData("Chalice: Inside the Asylum", Addresses.IA_Pickup_Chalice, "32896"),
            };
            return iaLocations;
        }


        private static List<GenericItemsData> GetPumpkinGorgeData()
        {
            List<GenericItemsData> pgLocations = new List<GenericItemsData>() {

                new GenericItemsData("Time Rune: Pumpkin Gorge", Addresses.PG_Pickup_TimeRune, "32896"), // value of this one is very wierd.
                new GenericItemsData("Chaos Rune: Pumpkin Gorge", Addresses.PG_Pickup_ChaosRune, "32896"),
                new GenericItemsData("Earth Rune: Pumpkin Gorge", Addresses.PG_Pickup_EarthRune, "32896"),
                new GenericItemsData("Moon Rune: Pumpkin Gorge", Addresses.PG_Pickup_MoonRune, "32896"),
                new GenericItemsData("Equipment: Club in Chest in Tunnel - PG", Addresses.PG_Pickup_ClubInChestInTunnel, "32896", true),
                new GenericItemsData("Equipment: Silver Shield in Chest at Top of Hill - PG", Addresses.PG_Pickup_SilverShieldInChestAtTopOfHill, "32896", true),
                new GenericItemsData("Energy Vial: Vine Patch Left - PG", Addresses.PG_Pickup_EnergyVialVinePatchLeft, "32896"),
                new GenericItemsData("Energy Vial: Vine Patch Right - PG", Addresses.PG_Pickup_EnergyVialVinePatchRight, "32896"),
                new GenericItemsData("Energy Vial: In Coop - PG", Addresses.PG_Pickup_EnergyVialInCoop, "32896"),
                new GenericItemsData("Energy Vial: In Moon Hut - PG", Addresses.PG_Pickup_EnergyVialInMoonHut, "32896"),
                new GenericItemsData("Energy Vial: Top of Hill - PG", Addresses.PG_Pickup_EnergyVialTopOfHill, "32896"),
                new GenericItemsData("Energy Vial: Boulders After Star Rune - PG",  Addresses.PG_Pickup_EnergyVialBouldersAfterStarRune, "32896"),
                new GenericItemsData("Energy Vial: Chalice Path - PG", Addresses.PG_Pickup_EnergyVialChalicePath, "32896"),
                new GenericItemsData("Gold Coins: Bag Behind Rocks At Start - PG", Addresses.PG_Pickup_GoldCoinsBagBehindRocksAtStart, "32896"),
                new GenericItemsData("Gold Coins: Chest in Coop 1 - PG", Addresses.PG_Pickup_GoldCoinsChestInCoop1, "32896"),
                new GenericItemsData("Gold Coins: Chest in Coop 2 - PG", Addresses.PG_Pickup_GoldCoinsChestInCoop2, "32896"),
                new GenericItemsData("Gold Coins: Chest in Coop 3 - PG", Addresses.PG_Pickup_GoldCoinsChestInCoop3, "32896"),
                new GenericItemsData("Gold Coins: Bag in Mushroom Area - PG", Addresses.PG_Pickup_GoldCoinsBagInMushroomArea, "32896"),
                new GenericItemsData("Gold Coins: Chest at Boulders after Star Rune - PG", Addresses.PG_Pickup_GoldCoinsChestAtBouldersAfterStarRune, "32896"),
                new GenericItemsData("Gold Coins: Chest Near Chalice - PG", Addresses.PG_Pickup_GoldCoinsChestNearChalice, "32896"),
                new GenericItemsData("Cleared: Pumpkin Gorge", Addresses.PG_LevelStatus, "16"),
                new GenericItemsData("Chalice: Pumpkin Gorge", Addresses.PG_Pickup_Chalice, "32896"),
            };
            return pgLocations;
        }

        private static List<GenericItemsData> GetPumpkinSerpentData()
        {
            List<GenericItemsData> psLocations = new List<GenericItemsData>() {
                new GenericItemsData("Key Item: Dragon Gem - PS", Addresses.PS_Pickup_DragonsGem, "32896", true),
                new GenericItemsData("Equipment: Silver Shield in Chest near Leeches - PS", Addresses.PS_Pickup_SilverShieldInChestNearLeeches, "32896", true),
                new GenericItemsData("Energy Vial: Left at Merchant Gargoyle - PS", Addresses.PS_Pickup_EnergyVialLeftAtMerchantGargoyle, "32896"),
                new GenericItemsData("Energy Vial: Right at Merchant Gargoyle - PS", Addresses.PS_Pickup_EnergyVialRightAtMerchantGargoyle, "32896"),
                new GenericItemsData("Gold Coins: Bag Behind House - PS", Addresses.PS_Pickup_GoldCoinsBagBehindHouse, "32896"),
                new GenericItemsData("Gold Coins: Bag Behind Vines and Pod - PS", Addresses.PS_Pickup_GoldCoinsBagBehindVinesAndPod, "32896"),
                new GenericItemsData("Gold Coins: Chest at Merchant Gargoyle - PS", Addresses.PS_Pickup_GoldCoinsChestAtMerchantGargoyle, "32896"),
                new GenericItemsData("Cleared: Pumpkin Serpent", Addresses.PS_LevelStatus, "16"),
                new GenericItemsData("Chalice: Pumpkin Serpent", Addresses.PS_Pickup_Chalice, "32896"),
            };
            return psLocations;
        }

        private static List<GenericItemsData> GetTheHauntedRuinsData()
        {
            List<GenericItemsData> hrLocations = new List<GenericItemsData>() {
                new GenericItemsData("Key Item: King Peregrine's Crown - HR", Addresses.HR_Pickup_KingPeregrinsCrown, "32896"),
                new GenericItemsData("Chaos Rune: The Haunted Ruins", Addresses.HR_Pickup_ChaosRune, "32896"),
                new GenericItemsData("Earth Rune: The Haunted Ruins", Addresses.HR_Pickup_EarthRune, "32896"),
                new GenericItemsData("Equipment: Silver Shield in Chest Near Rune Door - HR", Addresses.HR_Pickup_SilverShieldInChestNearRuneDoor, "32896", true),
                new GenericItemsData("Energy Vial: Above Rune - HR", Addresses.HR_Pickup_EnergyVialAboveRune, "32896"),
                new GenericItemsData("Energy Vial: Corner of Walls 1 - HR", Addresses.HR_Pickup_EnergyVialCornerOfWalls1, "32896"),
                new GenericItemsData("Energy Vial: Corner of Walls 2 - HR", Addresses.HR_Pickup_EnergyVialCornerOfWalls2, "32896"),
                new GenericItemsData("Energy Vial: Corner of Walls 3 - HR", Addresses.HR_Pickup_EnergyVialCornerOfWalls3, "32896"),
                new GenericItemsData("Energy Vial: Up from Oil - HR", Addresses.HR_Pickup_EnergyVialUpFromOil, "32896"),
                new GenericItemsData("Gold Coins: Near First Set of farmers - HR", Addresses.HR_Pickup_GoldCoinsNearFirstSetOfFarmers, "32896"),
                new GenericItemsData("Gold Coins: Bag Near Chalice North - HR", Addresses.HR_Pickup_GoldCoinsBagNearChaliceNorth, "32896"),
                new GenericItemsData("Gold Coins: Bag Near Chalice South - HR", Addresses.HR_Pickup_GoldCoinsBagNearChaliceSouth, "32896"),
                new GenericItemsData("Gold Coins: Bag in Crown Room - HR", Addresses.HR_Pickup_GoldCoinsBagInCrownRoom, "32896"),
                new GenericItemsData("Gold Coins: Chest at Catapult 1 - HR", Addresses.HR_Pickup_GoldCoinsChestAtCatapult1, "32896"),
                new GenericItemsData("Gold Coins: Chest at Catapult 2 - HR", Addresses.HR_Pickup_GoldCoinsChestAtCatapult2, "32896"),
                new GenericItemsData("Gold Coins: Chest at Catapult 3 - HR", Addresses.HR_Pickup_GoldCoinsChestAtCatapult3, "32896"),
                new GenericItemsData("Cleared: The Haunted Ruins", Addresses.HR_LevelStatus, "16"),
                new GenericItemsData("Chalice: The Haunted Ruins", Addresses.HR_Pickup_Chalice, "32896"),
            };
            return hrLocations;
        }

        private static List<GenericItemsData> GetTheGhostShipData()
        {
            List<GenericItemsData> gsLocations = new List<GenericItemsData>() {
                new GenericItemsData("Moon Rune: Ghost Ship", Addresses.GS_Pickup_MoonRune, "32896"),
                new GenericItemsData("Chaos Rune: Ghost Ship", Addresses.GS_Pickup_ChaosRune, "32896"),
                new GenericItemsData("Star Rune: Ghost Ship", Addresses.GS_Pickup_StarRune, "32896"),
                new GenericItemsData("Equipment: Silver Shield in Chest in Barrel Room - GS", Addresses.GS_Pickup_SilverShieldInChestInBarrelRoom, "32896", true),
                new GenericItemsData("Equipment: Club in Chest at Captain - GS", Addresses.GS_Pickup_ClubInChestAtCaptain, "32896", true),
                new GenericItemsData("Energy Vial: In Cabin - GS", Addresses.GS_Pickup_EnergyVialInCabin, "32896"),
                new GenericItemsData("Energy Vial: In Cannon Room - GS",Addresses.GS_Pickup_EnergyVialInCannonRoom, "32896"),
                new GenericItemsData("Energy Vial: Rope Bridge 1 - GS", Addresses.GS_Pickup_EnergyVialRopeBridge1, "32896"),
                new GenericItemsData("Energy Vial: Rope Bridge 2 - GS", Addresses.GS_Pickup_EnergyVialRopeBridge2, "32896"),
                new GenericItemsData("Energy Vial: Cage Lift 1 - GS", Addresses.GS_Pickup_EnergyVialCageLift1, "32896"),
                new GenericItemsData("Energy Vial: Cage Lift 2 - GS", Addresses.GS_Pickup_EnergyVialCageLift2, "32896"),
                new GenericItemsData("Gold Coins: Bag in Rolling Barrels Room - GS", Addresses.GS_Pickup_GoldCoinsBagInRollingBarrelsRoom, "32896"),
                new GenericItemsData("Gold Coins: Bag on Deck At Barrels - GS", Addresses.GS_Pickup_GoldCoinsBagOnDeckAtBarrels, "32896"),
                new GenericItemsData("Gold Coins: Chest in Cannon Room - GS", Addresses.GS_Pickup_GoldCoinsChestInCannonRoom, "32896"),
                new GenericItemsData("Gold Coins: Rope Bridge - GS", Addresses.GS_Pickup_GoldCoinsRopeBridge, "32896"),
                new GenericItemsData("Cleared: Ghost Ship", Addresses.HR_LevelStatus, "16"),
                new GenericItemsData("Chalice: Ghost Ship", Addresses.HR_Pickup_Chalice, "32896"),
            };
            return gsLocations;
        }

        private static List<GenericItemsData> GetTheEntranceHallData()
        {
            List<GenericItemsData> ehLocations = new List<GenericItemsData>() {
                new GenericItemsData("Cleared: The Entrance Hall", Addresses.EH_LevelStatus, "16"),
                new GenericItemsData("Chalice: The Entrance Hall", Addresses.EH_Pickup_Chalice, "32896"),
            };
            return ehLocations;
        }

        private static List<GenericItemsData> GetTheTimeDeviceData()
        {
            List<GenericItemsData> tdLocations = new List<GenericItemsData>() {
                new GenericItemsData("Life Bottle: The Time Device", Addresses.TD_Pickup_LifeBottle, "32896"),
                new GenericItemsData("Chaos Rune: The Time Device", Addresses.TD_Pickup_ChaosRune, "32896"),
                new GenericItemsData("Earth Rune: The Time Device", Addresses.TD_Pickup_EarthRune, "32896"),
                new GenericItemsData("Moon Rune: The Time Device", Addresses.TD_Pickup_MoonRune, "32896"),
                new GenericItemsData("Time Rune: The Time Device", Addresses.TD_Pickup_TimeRune, "32896"),
                new GenericItemsData("Equipment: Silver Shield on Clock - TD", Addresses.TD_Pickup_SilverShieldOnClock, "32896", true),
                new GenericItemsData("Gold Coins: Laser Platform Right - TD", Addresses.TD_Pickup_GoldCoinsLaserPlatformRight, "32896"),
                new GenericItemsData("Gold Coins: Laser Platform Left - TD", Addresses.TD_Pickup_GoldCoinsLaserPlatformLeft, "32896"),
                new GenericItemsData("Gold Coins: Lone Pillar 1 - TD", Addresses.TD_Pickup_GoldCoinsLonePillar1, "32896"),
                new GenericItemsData("Gold Coins: Lone Pillar 2 - TD", Addresses.TD_Pickup_GoldCoinsLonePillar2, "32896"),
                new GenericItemsData("Gold Coins: Lone Pillar 3 - TD", Addresses.TD_Pickup_GoldCoinsLonePillar3, "32896"),
                new GenericItemsData("Gold Coins: Bag at Earth Station 1 - TD", Addresses.TD_Pickup_GoldCoinsBagAtEarthStation1, "32896"),
                new GenericItemsData("Gold Coins: Bag at Earth Station 2 - TD", Addresses.TD_Pickup_GoldCoinsBagAtEarthStation2, "32896"),
                new GenericItemsData("Gold Coins: Bag at Earth Station 3 - TD", Addresses.TD_Pickup_GoldCoinsBagAtEarthStation3, "32896"),
                new GenericItemsData("Cleared: The Time Device", Addresses.TD_LevelStatus, "16"),
                new GenericItemsData("Chalice: The Time Device", Addresses.TD_Pickup_Chalice, "32896"),
            };
            return tdLocations;
        }

        private static List<GenericItemsData> GetZaroksLairData()
        {
            List<GenericItemsData> zlLocations = new List<GenericItemsData>() {
                new GenericItemsData("Equipment: Good Lightning - ZL", Addresses.ZL_Pickup_GoodLightning, "32896", true),
                new GenericItemsData("Equipment: Silver Shield Arena - ZL", Addresses.ZL_Pickup_SilverShield, "32896", true),
                new GenericItemsData("Cleared: Zaroks Lair", Addresses.WinConditionCredits, "101"),
            };
            return zlLocations;
        }


    }
}