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
using System.Net;
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
            {"Equipment: Small Sword", 0},
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


        // will contain a list of offset values based on the original entities on/off switch address given in addresses. As every entity follows the same table
        // it makes sense that you could just count forward a few bytes to get the chests contents.

        // pretty sure i could just loop these tbh, but i'll refactor when i'm not busy
        public static Dictionary<int, List<ulong>> ChestContentsDictionary()
        {

            ulong contents_offset = 0xc;
            return new Dictionary<int, List<ulong>>
            {
                [0] = [], // main menu
                [1] = [Addresses.TG_Pickup_CopperShield + contents_offset],
                [2] = [Addresses.RTG_Pickup_SilverShieldChestAtShop + contents_offset],
                [3] = [Addresses.CH_Pickup_Club + contents_offset, Addresses.CH_Pickup_CopperShield1stOnHill + contents_offset, Addresses.CH_Pickup_CopperShield2ndOnHill + contents_offset, Addresses.CH_Pickup_CopperShield3rdOnHill + contents_offset],
                [4] = [Addresses.HM_Pickup_ClubBrokenBenches + contents_offset, Addresses.HM_Pickup_DaggersBlockPuzzle + contents_offset],
                [5] = [Addresses.SF_Pickup_ClubInsideHut + contents_offset, Addresses.SF_Pickup_CopperShieldChestInTheBarn + contents_offset, Addresses.SF_Pickup_SilverShieldBehindWindmill + contents_offset],
                [6] = [Addresses.DC_Pickup_CopperShield + contents_offset],
                [7] = [Addresses.TA_Pickup_ClubChestAtBarrier + contents_offset],
                [8] = [], // crystal caves
                [9] = [Addresses.PG_Pickup_ClubInChestInTunnel + contents_offset, Addresses.PG_Pickup_SilverShieldInChestAtTopOfHill + contents_offset],
                [10] = [Addresses.PS_Pickup_SilverShieldInChestNearLeeches + contents_offset],
                [11] = [Addresses.TSV_Pickup_SilverShieldInBlacksmiths + contents_offset, Addresses.TSV_Pickup_ClubInChestUnderInnStairs + contents_offset],
                [12] = [Addresses.PAD_Pickup_SilverShieldInChestNearSoul5 + contents_offset],
                [13] = [Addresses.AG_Pickup_SilverShieldInChestBehindDoor + contents_offset],
                [14] = [Addresses.IA_Pickup_SilverShieldInBatRoom], // this is technically a chest here, but there's no need for an offset
                [15] = [], // enchanted earth
                [16] = [Addresses.GG_Pickup_SilverShieldInChestNearExit + contents_offset],
                [17] = [Addresses.HR_Pickup_SilverShieldInChestNearRuneDoor + contents_offset],
                [18] = [], // hall of heroes
                [19] = [Addresses.GS_Pickup_SilverShieldInChestInBarrelRoom + contents_offset, Addresses.GS_Pickup_ClubInChestAtCaptain + contents_offset],
                [20] = [], //entrance hall
                [21] = [Addresses.TD_Pickup_SilverShieldOnClock + contents_offset],
                [22] = [Addresses.TL_Pickup_SilverShieldInWhirlpool + contents_offset],
                [23] = [Addresses.ZL_Pickup_GoodLightning + contents_offset, Addresses.ZL_Pickup_SilverShield + contents_offset],
            };
        }

        public static List<uint> OpenMapMemoryLocations()
        {
            return new List<uint>
            {
                Addresses.MAP_Unlock1,
                Addresses.MAP_Unlock2,
                Addresses.MAP_Unlock3,
                Addresses.MAP_Unlock4,
                Addresses.MAP_Unlock5,
                Addresses.MAP_Unlock6,
                Addresses.MAP_Unlock7,
                Addresses.MAP_Unlock8,
                Addresses.MAP_Unlock9,
                Addresses.MAP_Unlock10,
                Addresses.MAP_Unlock11,
                Addresses.MAP_Unlock12,
                Addresses.MAP_Unlock13,
                Addresses.MAP_Unlock14,
                Addresses.MAP_Unlock15,
                Addresses.MAP_Unlock16,
                Addresses.MAP_Unlock17,
                Addresses.MAP_Unlock18,
                Addresses.MAP_Unlock19,
                Addresses.MAP_Unlock20,
                Addresses.MAP_Unlock21,
                Addresses.MAP_Unlock22,
                Addresses.MAP_Unlock23,
                Addresses.MAP_Unlock24
            };
        }

        


        public static List<ILocation> BuildLocationList(Dictionary<string, object> options)
        {
            int base_id = 99250000;
            int region_offset = 1000;

            int gargoyleSanity = Int32.Parse(options?.GetValueOrDefault("gargoylesanity", "0").ToString());
            int bookSanity = Int32.Parse(options?.GetValueOrDefault("booksanity", "0").ToString());

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
                "Zaroks Lair",
                "Locked Items DC",
                "Locked Items CH",
                "Locked Items HM",
                "Locked Items SF"
];

            List<ILocation> locations = new List<ILocation>();

            Dictionary<string, List<GenericItemsData>> allLevelLocations = new Dictionary<string, List<GenericItemsData>>();

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
            allLevelLocations.Add("Pools of the Ancient Dead", GetPoolsOfTheAncientDeadData());
            allLevelLocations.Add("The Lake", GetTheLakeData());
            allLevelLocations.Add("The Crystal Caves", GetTheCrystalCavesData());
            allLevelLocations.Add("The Gallows Gauntlet", GetGallowsGauntletData());
            allLevelLocations.Add("Asylum Grounds", GetTheAsylumGroundsData());
            allLevelLocations.Add("Inside the Asylum", GetInsideTheAsylumData());
            allLevelLocations.Add("Pumpkin Gorge", GetPumpkinGorgeData());
            allLevelLocations.Add("Pumpkin Serpent", GetPumpkinSerpentData());
            allLevelLocations.Add("The Haunted Ruins", GetTheHauntedRuinsData());
            allLevelLocations.Add("The Ghost Ship", GetTheGhostShipData());
            allLevelLocations.Add("The Entrance Hall", GetTheEntranceHallData());
            allLevelLocations.Add("The Time Device", GetTheTimeDeviceData());
            allLevelLocations.Add("Zaroks Lair", GetZaroksLairData());
            allLevelLocations.Add("Locked Items DC", GetLockedItemsDC());
            allLevelLocations.Add("Locked Items CH", GetLockedItemsCH());
            allLevelLocations.Add("Locked Items HM", GetLockedItemsHM());
            allLevelLocations.Add("Locked Items SF", GetLockedItemsSF());

            var regional_index = 0;

            var debug_levelCount = 0;
            foreach (var region_name in table_order.ToList())
            {

                long currentRegionBaseId = base_id + (regional_index * region_offset);

                if (allLevelLocations.ContainsKey(region_name))
                {
                    // Retrieve the list of locations for the current region
                    List<GenericItemsData> regionLocations = allLevelLocations[region_name];

                    var location_index = 0;

                    foreach (var loc in regionLocations)

                    {

                        int locationId = (int)currentRegionBaseId + location_index;

                        if (loc.Name.ToLower().Contains("in crystal"))
                        {
                            {
                                List<ILocation> conditionalChoice = new List<ILocation>();

                                conditionalChoice.Add(new Location()
                                {
                                    Id = -1,
                                    Name = "Level Check",
                                    Address = Addresses.CurrentLevel,
                                    CheckType = LocationCheckType.Byte,
                                    CompareType = LocationCheckCompareType.Match,
                                    CheckValue = loc.LevelId
                                });

                                conditionalChoice.Add(new Location()
                                {
                                    Id = -1,
                                    Name = "Crystal Check",
                                    Address = loc.Address,
                                    CheckType = LocationCheckType.Byte,
                                    CompareType = LocationCheckCompareType.GreaterThan,
                                    CheckValue = "0"
                                });

                                CompositeLocation location = new CompositeLocation()
                                {
                                    Name = loc.Name,
                                    Id = locationId,
                                    CheckType = LocationCheckType.AND,
                                    Conditions = conditionalChoice,
                                };

                                locations.Add(location);
                                location_index++;
                                continue;
                            };
                        }

                        if (loc.IsInChest) // if it's cleared and we don't have an option set 
                        {
                            {
                                List<ILocation> conditionalChoice = new List<ILocation>();

                                conditionalChoice.Add(new Location()
                                {
                                    Id = -1,
                                    Name = "Level Check",
                                    Address = Addresses.CurrentLevel,
                                    CheckType = LocationCheckType.Byte,
                                    CompareType = LocationCheckCompareType.Match,
                                    CheckValue = loc.LevelId
                                });

                                conditionalChoice.Add(new Location()
                                {
                                    Id = -1,
                                    Name = "Chest Check",
                                    Address = loc.Address,
                                    CheckType = LocationCheckType.Short,
                                    CompareType = LocationCheckCompareType.Match,
                                    CheckValue = "801"
                                });

                                CompositeLocation location = new CompositeLocation()
                                {
                                    Name = loc.Name,
                                    Id = locationId,
                                    CheckType = LocationCheckType.AND,
                                    Conditions = conditionalChoice,
                                };

                                locations.Add(location);
                                location_index++;
                                continue;
                            };
                        }

                        if (loc.Name.Contains("Gauntlet Cleared:")) // if it's cleared and we don't have an option set 
                        {
                            {
                                List<ILocation> conditionalChoice = new List<ILocation>();

                                conditionalChoice.Add(new Location()
                                {
                                    Id = -1,
                                    Name = "Level Check",
                                    Address = Addresses.CurrentLevel,
                                    CheckType = LocationCheckType.Byte,
                                    CompareType = LocationCheckCompareType.Match,
                                    CheckValue = loc.LevelId
                                });

                                conditionalChoice.Add(new Location()
                                {
                                    Id = -1,
                                    Name = "Gauntlet Check",
                                    Address = loc.Address,
                                    CheckType = LocationCheckType.UShort,
                                    CompareType = LocationCheckCompareType.Match,
                                    CheckValue = loc.Check
                                });

                                CompositeLocation location = new CompositeLocation()
                                {
                                    Name = loc.Name,
                                    Id = locationId,
                                    CheckType = LocationCheckType.AND,
                                    Conditions = conditionalChoice

                                };
                                locations.Add(location);
                                location_index++;
                                continue;
                            };
                        }

                        if (loc.Name.Contains("Cleared: Zaroks Lair")) // if it's cleared and we don't have an option set 
                        {
                            {
                                List<ILocation> conditionalChoice = new List<ILocation>();

                                conditionalChoice.Add(new Location()
                                {
                                    Id = -1,
                                    Name = "Cleared: Zaroks Lair",
                                    Address = loc.Address,
                                    CheckType = LocationCheckType.Byte,
                                    CompareType = LocationCheckCompareType.Match,
                                    CheckValue = "101"
                                });
                                CompositeLocation location = new CompositeLocation()
                                {
                                    Name = loc.Name,
                                    Id = locationId,
                                    CheckType = LocationCheckType.AND,
                                    Conditions = conditionalChoice
                                };

                                locations.Add(location);
                                location_index++;
                                continue;
                            }
                        }

                        if (loc.Name.Contains("Cleared: ")) // if it's cleared and we don't have an option set 
                        {

                            {
                                List<ILocation> conditionalChoice = new List<ILocation>();

                                conditionalChoice.Add(new Location()
                                {
                                    Id = -1,
                                    Name = "Cleared Level",
                                    Address = loc.Address,
                                    CheckType = LocationCheckType.Bit,
                                    AddressBit = 4
                                });
                                CompositeLocation location = new CompositeLocation()
                                {
                                    Name = loc.Name,
                                    Id = locationId,
                                    CheckType = LocationCheckType.AND,
                                    Conditions = conditionalChoice
                                };



                                locations.Add(location);
                                location_index++;
                                continue;
                            }
                        }

                        if (loc.Name.Contains("Skill:"))
                        {
                            {
                                List<ILocation> conditionalChoice = new List<ILocation>();

                                conditionalChoice.Add(new Location()
                                {
                                    Id = -1,
                                    Name = "Level Check",
                                    Address = Addresses.CurrentLevel,
                                    CheckType = LocationCheckType.Byte,
                                    CompareType = LocationCheckCompareType.Match,
                                    CheckValue = "2"
                                });

                                conditionalChoice.Add(new Location()
                                {
                                    Id = -1,
                                    Name = "Skill Check",
                                    Address = loc.Address,
                                    CheckType = LocationCheckType.UShort,
                                    CompareType = LocationCheckCompareType.Match,
                                    CheckValue = "257"
                                });

                                CompositeLocation location = new CompositeLocation()
                                {
                                    Name = loc.Name,
                                    Id = locationId,
                                    CheckType = LocationCheckType.AND,
                                    Conditions = conditionalChoice
                                };

                                locations.Add(location);
                                location_index++;
                                continue;
                            }

                        }

                        if (loc.Name.Contains("Gargoyle:") && gargoyleSanity == 1)
                        {
                            List<ILocation> conditionalChoice = new List<ILocation>();

                            conditionalChoice.Add(new Location()
                            {
                                Id = -1,
                                Name = "Level Check",
                                Address = Addresses.CurrentLevel,
                                CheckType = LocationCheckType.Byte,
                                CompareType = LocationCheckCompareType.Match,
                                CheckValue = loc.LevelId
                            });

                            conditionalChoice.Add(new Location()
                            {
                                Id = -1,
                                Name = "Gargoyle Check",
                                Address = loc.Address,
                                CheckType = LocationCheckType.Byte,
                                CompareType = LocationCheckCompareType.Match,
                                CheckValue = "0"
                            });

                            CompositeLocation location = new CompositeLocation()
                            {
                                Name = loc.Name,
                                Id = locationId,
                                CheckType = LocationCheckType.AND,
                                Conditions = conditionalChoice
                            };

                            locations.Add(location);
                            location_index++;
                            continue;
                        }

                        if (loc.Name.Contains("Book:") && bookSanity == 1)
                        {
                            List<ILocation> conditionalChoice = new List<ILocation>();

                            conditionalChoice.Add(new Location()
                            {
                                Id = -1,
                                Name = "Level Check",
                                Address = Addresses.CurrentLevel,
                                CheckType = LocationCheckType.Byte,
                                CompareType = LocationCheckCompareType.Match,
                                CheckValue = loc.LevelId
                            });

                            conditionalChoice.Add(new Location()
                            {
                                Id = -1,
                                Name = "Book Check",
                                Address = loc.Address,
                                CheckType = LocationCheckType.Byte,
                                CompareType = LocationCheckCompareType.Match,
                                CheckValue = "0"
                            });

                            CompositeLocation location = new CompositeLocation()
                            {
                                Name = loc.Name,
                                Id = locationId,
                                CheckType = LocationCheckType.AND,
                                Conditions = conditionalChoice
                            };

                            locations.Add(location);
                            location_index++;
                            continue;
                        }


                        if (loc.Name.Contains("Key Item:") || loc.Name.Contains("Chalice Reward") || loc.Name.Contains("Chalice:") || loc.Name.Contains("Rune:") || loc.Name.Contains("Equipment:") || loc.Name.Contains("Gold Coins:") || loc.Name.Contains("Skill:") || loc.Name.Contains("Life Bottle:") || loc.Name.Contains("Energy Vial:") || loc.Name.Contains("Fairy") || loc.Name.Contains("Egg Drop"))
                        {
                            {
                                List<ILocation> conditionalChoice = new List<ILocation>();

                                bool checkForSFRune = loc.LevelId == "5" && (loc.Name.ToLower().Contains("chaos") || loc.Name.ToLower().Contains("moon"));

                                bool checkForEEItems = loc.LevelId == "15" && (loc.Name.ToLower().Contains("star") || loc.Name.ToLower().Contains("egg"));

                                bool checkForSVBellows = loc.LevelId == "11" && loc.Name.ToLower() == "key item: crucifix - sv";

                                bool checkForIARooms = loc.LevelId == "14" && loc.Name.ToLower().Contains("gauntlet");

                                conditionalChoice.Add(new Location()
                                {
                                    Id = -1,
                                    Name = "Level Check",
                                    Address = Addresses.CurrentLevel,
                                    CheckType = LocationCheckType.Byte,
                                    CompareType = LocationCheckCompareType.Match,
                                    CheckValue = loc.LevelId
                                });

                                conditionalChoice.Add(new Location()
                                {
                                    Id = -1,
                                    Name = "Pickup Check",
                                    Address = loc.Address,
                                    CheckType = checkForIARooms || checkForSFRune || checkForEEItems || checkForSVBellows ? LocationCheckType.UShort : LocationCheckType.Int,
                                    CompareType = LocationCheckCompareType.Match,
                                    CheckValue = loc.Check
                                });

                                CompositeLocation location = new CompositeLocation()
                                {
                                    Name = loc.Name,
                                    Id = locationId,
                                    CheckType = LocationCheckType.AND,
                                    Conditions = conditionalChoice
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

            //Console.WriteLine($"Count is {locations.Count()}");
            //foreach (var location in locations)
            //{
            //    Console.WriteLine($"{location.Id}: {location.Name}");
            //}

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
                "Key Items",
                "Skills",
                "Level Status"
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
                    ["Shadow Talisman"] = Addresses.ShadowTalisman, 
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
                [0x07] = "Ant Caves",
                [0x08] = "The Crystal Caves",
                [0x09] = "Pumpkin Gorge",
                [0x0A] = "The Pumpkin Serpent",
                [0x0B] = "The Sleeping Village",
                [0x0C] = "Pools Of The Ancient Dead",
                [0x0D] = "The Asylum Grounds",
                [0x0E] = "Inside The Asylum",
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




        // Hall of Heroes needs an overhaul. Not worth dealing with right now

        private static List<GenericItemsData> GetHallOfHeroesData()
        {
            List<GenericItemsData> hallOfHeroesVisits = new List<GenericItemsData>()
            {
                new GenericItemsData("Chalice Reward 1", Addresses.HOH_CannyTim2, "18", "9999"),
                new GenericItemsData("Chalice Reward 2", Addresses.HOH_RavenHoovesTheArcher4, "18", "9999"),
                new GenericItemsData("Chalice Reward 3", Addresses.HOH_DirkSteadfast2, "18", "9999"),
                new GenericItemsData("Chalice Reward 4", Addresses.HOH_WodenTheMighty1, "18", "9999"),
                new GenericItemsData("Chalice Reward 5", Addresses.HOH_DirkSteadfast1, "18", "9999"),
                new GenericItemsData("Chalice Reward 6", Addresses.HOH_StanyerIronHewer1, "18", "9999"),
                new GenericItemsData("Chalice Reward 7", Addresses.HOH_BloodmonathSkullCleaver1, "18", "9999"),
                new GenericItemsData("Chalice Reward 8", Addresses.HOH_CannyTim1, "18", "9999"),
                new GenericItemsData("Chalice Reward 9", Addresses.HOH_RavenHoovesTheArcher1, "18", "9999"),
                new GenericItemsData("Chalice Reward 10", Addresses.HOH_RavenHoovesTheArcher2, "18", "9999"),
                new GenericItemsData("Chalice Reward 11", Addresses.HOH_RavenHoovesTheArcher3, "18", "9999"),
                new GenericItemsData("Chalice Reward 12", Addresses.HOH_Imanzi1, "18", "9999"),
                new GenericItemsData("Chalice Reward 13", Addresses.HOH_MegwynneStormbinder1, "18", "9999"),
                new GenericItemsData("Chalice Reward 14", Addresses.HOH_KarlStungard1, "18", "9999"),
                new GenericItemsData("Chalice Reward 15", Addresses.HOH_Imanzi2, "18", "9999"),
                new GenericItemsData("Chalice Reward 16", Addresses.HOH_MegwynneStormbinder2, "18", "9999"),
                new GenericItemsData("Chalice Reward 17", Addresses.HOH_StanyerIronHewer2, "18", "9999"),
                new GenericItemsData("Chalice Reward 18", Addresses.HOH_WodenTheMighty2, "18", "9999"),
                new GenericItemsData("Chalice Reward 19", Addresses.HOH_BloodmonathSkullCleaver2, "18", "9999"),
                new GenericItemsData("Chalice Reward 20", Addresses.HOH_KarlStungard2, "18", "9999"),
            };
            return hallOfHeroesVisits;
        }

        private static List<GenericItemsData> GetDansCryptData()
        {
            List<GenericItemsData> dcLocations = new List<GenericItemsData>() {
                new GenericItemsData("Star Rune: Dan's Crypt", Addresses.DC_Pickup_StarRune, "6", "32896"),
                new GenericItemsData("Life Bottle: Dan's Crypt", Addresses.DC_Pickup_LifeBottle,  "6", "32896"),
                new GenericItemsData("Equipment: Small Sword - DC", Addresses.DC_Pickup_Shortsword,  "6", "32896"),
                new GenericItemsData("Equipment: Copper Shield in Chest - DC", Addresses.DC_Pickup_CopperShield,  "6", "32896", true),
                new GenericItemsData("Equipment: Daggers - DC", Addresses.DC_Pickup_Daggers,  "6", "32896"),
                new GenericItemsData("Gold Coins: Over the water - DC",Addresses.DC_Pickup_GoldCoinsOverWater,  "6", "32896"),
                new GenericItemsData("Book: Unlocking Runes - DC", Addresses.DC_Book_UnlockingRunes, "6", "0"),
                new GenericItemsData("Book: Using Crypt - DC", Addresses.DC_Book_UsingCrypt, "6", "0"),
                new GenericItemsData("Book: Pressing Select - DC", Addresses.DC_Book_PressingSelect, "6", "0"),
                new GenericItemsData("Book: Power Attack - DC", Addresses.DC_Book_PowerAttack, "6", "0"),
                new GenericItemsData("Book: Swimming - DC", Addresses.DC_Book_Swimming, "6", "0"),
                new GenericItemsData("Book: Coins - DC", Addresses.DC_Book_Coins, "6", "0"),
                new GenericItemsData("Gargoyle: Left - DC", Addresses.DC_Gargoyle_Left, "6", "0"),
                new GenericItemsData("Gargoyle: Right - DC", Addresses.DC_Gargoyle_Right, "6", "0"),
                new GenericItemsData("Cleared: Dan's Crypt", Addresses.DC_LevelStatus,  "6", "16"),

            };
            return dcLocations;
        }

        private static List<GenericItemsData> GetTheGraveyardData()
        {
            List<GenericItemsData> tgLocations = new List<GenericItemsData>() {

                new GenericItemsData("Life Bottle: The Graveyard", Addresses.TG_Pickup_LifePotion, "1", "32896"),
                new GenericItemsData("Earth Rune: The Graveyard", Addresses.TG_Pickup_EarthRune, "1", "32896"),
                new GenericItemsData("Chaos Rune: The Graveyard", Addresses.TG_Pickup_ChaosRune, "1", "32896"),
                new GenericItemsData("Equipment: Copper Shield - TG", Addresses.TG_Pickup_CopperShield, "1", "32896", true),
                new GenericItemsData("Gold Coins: Bag at Start - TG", Addresses.TG_Pickup_GoldCoinsBagAtStart, "1", "32896"),
                new GenericItemsData("Gold Coins: Near Chaos Rune - TG", Addresses.TG_Pickup_GoldCoinsNearChaosRune, "1", "32896"),
                new GenericItemsData("Gold Coins: Gold Coins: Behind Fence at Statue - TG", Addresses.TG_Pickup_GoldCoinsBehindFenceAtStatue, "1", "32896"),
                new GenericItemsData("Gold Coins: Life Bottle Left Chest - TG", Addresses.TG_Pickup_GoldCoinsLifePotionLeftChest, "1", "32896"),
                new GenericItemsData("Gold Coins: Life Bottle Right Chest - TG", Addresses.TG_Pickup_GoldCoinsLifePotionRightChest, "1", "32896"),
                new GenericItemsData("Gold Coins: Shop Chest - TG", Addresses.TG_Pickup_GoldCoinsShopChest, "1", "32896"),
                new GenericItemsData("Gold Coins: Bag Near Hill Fountain - TG", Addresses.TG_Pickup_GoldCoinsBagNearHillFountain, "1", "32896"),
                new GenericItemsData("Book: Welcome Back - TG", Addresses.TG_Book_WelcomeBack, "1", "0"),
                new GenericItemsData("Book: Healing Fountain - TG", Addresses.TG_Book_HealingFountain, "1", "0"),
                new GenericItemsData("Book: Gaze of an Angel - TG", Addresses.TG_Book_GazeOfAnAngel, "1", "0"),
                new GenericItemsData("Book: Skull Key - TG", Addresses.TG_Book_SkullKey, "1", "0"),
                new GenericItemsData("Gargoyle: End of Level - TG", Addresses.TG_Gargoyle_EndOfLevel, "1", "0"),
                new GenericItemsData("Cleared: The Graveyard", Addresses.TG_LevelStatus, "1", "16"),
                new GenericItemsData("Chalice: The Graveyard", Addresses.TG_Pickup_Chalice, "1", "32896"),


            };
            return tgLocations;
        }

        private static List<GenericItemsData> GetCemeteryHillData()
        {
            List<GenericItemsData> chLocations = new List<GenericItemsData>() {

                new GenericItemsData("Key Item: Witches Talisman - CH", Addresses.CH_Pickup_WitchTalisman, "3", "32896"),
                new GenericItemsData("Equipment: Copper Shield 1 - CH", Addresses.CH_Pickup_CopperShield1stOnHill, "3", "32896", true),
                new GenericItemsData("Equipment: Copper Shield 2 - CH", Addresses.CH_Pickup_CopperShield2ndOnHill, "3", "32896", true),
                new GenericItemsData("Equipment: Copper Shield 3 - CH", Addresses.CH_Pickup_CopperShield3rdOnHill, "3", "32896", true),
                new GenericItemsData("Equipment: Club - CH", Addresses.CH_Pickup_Club, "3", "32896", true),
                new GenericItemsData("Equipment: Copper Shield in Arena - CH", Addresses.CH_Pickup_CopperShieldArena, "3", "32896"),
                new GenericItemsData("Energy Vial: Near Shop - CH", Addresses.CH_Pickup_EnergyVialNearShop, "3", "32896"),
                new GenericItemsData("Energy Vial: Arena - CH", Addresses.CH_Pickup_EnergyVialArena, "3", "32896"),
                new GenericItemsData("Gold Coins: Near Boulder Entrance - CH", Addresses.CH_Pickup_GoldCoinsNearBoulderEntrance, "3", "32896"),
                new GenericItemsData("Gold Coins: Up Hill 1 - CH", Addresses.CH_Pickup_GoldCoinsUpHill1, "3", "32896"),
                new GenericItemsData("Gold Coins: Up Hill 2 - CH", Addresses.CH_Pickup_GoldCoinsUpHill2, "3", "32896"),
                new GenericItemsData("Gold Coins: Chest at Exit - CH", Addresses.CH_Pickup_GoldCoinsChestAtExit, "3", "32896"),
                new GenericItemsData("Gold Coins: Chest in Arena - CH", Addresses.CH_Pickup_GoldCoinsChestInArena,"3",  "32896"),
                new GenericItemsData("Book: Breakables - CH", Addresses.CH_Book_Breakables, "3", "0"),
                new GenericItemsData("Book: Club - CH", Addresses.CH_Book_Club, "3", "0"),
                new GenericItemsData("Book: Destroy Boulder - CH", Addresses.CH_Book_DestroyBoulder, "3", "0"),
                new GenericItemsData("Book: A Guide to Covens - CH", Addresses.CH_Book_AGuideToCovens, "3", "0"),
                new GenericItemsData("Book: Hidden Locations - CH", Addresses.CH_Book_HiddenLocations, "3", "0"),
                new GenericItemsData("Gargoyle: Witch Cave - CH", Addresses.CH_Gargoyle_WitchCave, "3", "0"),
                new GenericItemsData("Cleared: Cemetery Hill", Addresses.CH_LevelStatus, "3", "16"),
            };
            return chLocations;
        }

        private static List<GenericItemsData> GetHilltopMausoleumData()
        {
            List<GenericItemsData> hmLocations = new List<GenericItemsData>() {
                new GenericItemsData("Key Item: Sheet Music - HM", Addresses.HM_Pickup_SheetMusic, "4", "32896"),
                new GenericItemsData("Key Item: Skull Key - HM", Addresses.HM_Pickup_GlassDemonSkullKey, "4", "32896"),
                new GenericItemsData("Chaos Rune: The Hilltop Mausoleum", Addresses.HM_Pickup_ChaosRune, "4", "32896"),
                new GenericItemsData("Earth Rune: The Hilltop Mausoleum", Addresses.HM_Pickup_EarthRune, "4", "32896"),
                new GenericItemsData("Moon Rune: The Hilltop Mausoleum", Addresses.HM_Pickup_MoonRune, "4", "32896"),
                new GenericItemsData("Equipment: Club near Broken Benches - HM", Addresses.HM_Pickup_ClubBrokenBenches, "4", "32896", true),
                new GenericItemsData("Equipment: Daggers near Block Puzzle - HM", Addresses.HM_Pickup_DaggersBlockPuzzle, "4", "32896", true),
                new GenericItemsData("Equipment: Copper Shield near Block Puzzle - HM", Addresses.HM_Pickup_CopperShieldBlockPuzzle, "4", "32896"),
                new GenericItemsData("Energy Vial: Right Coffin - HM", Addresses.HM_Pickup_EnergyVialRightCoffin, "4", "32896"),
                new GenericItemsData("Energy Vial: Near Rune on Left Ramp - HM", Addresses.HM_Pickup_EnergyVialNearRuneLeftRamp, "4", "32896"),
                new GenericItemsData("Energy Vial: Phantom of the Opera on Left - HM", Addresses.HM_Pickup_EnergyVialPhantomOfTheOperaLeft, "4", "32896"),
                new GenericItemsData("Energy Vial: Phantom of the Opera on Right - HM", Addresses.HM_Pickup_EnergyVialPhantomOfTheOperaRight, "4", "32896"),
                new GenericItemsData("Energy Vial: Moon Room - HM", Addresses.HM_Pickup_EnergyVialMoonRoom, "4", "32896"),
                new GenericItemsData("Gold Coins: Left Coffin - HM", Addresses.HM_Pickup_GoldCoinsLeftCoffin, "4", "32896"),
                new GenericItemsData("Gold Coins: After Earth Rune Door - HM", Addresses.HM_Pickup_GoldCoinsAfterEarthRuneDoor, "4", "32896"),
                new GenericItemsData("Gold Coins: Chest in Moon Room - HM", Addresses.HM_Pickup_GoldCoinsChestInMoonRoom, "4", "32896"),
                new GenericItemsData("Book: Glass Demon - HM", Addresses.HM_Book_GlassDemon, "4", "0"),
                new GenericItemsData("Book: Phantom of the Opera - HM", Addresses.HM_Book_PhantomOfTheOpera, "4", "0"),
                new GenericItemsData("Book: Demon Heart - HM", Addresses.HM_Book_DemonHeart, "4", "0"),
                new GenericItemsData("Book: Theving Imps - HM", Addresses.HM_Book_ThevingImps, "4", "0"),
                new GenericItemsData("Cleared: The Hilltop Mausoleum", Addresses.HM_LevelStatus, "4", "16"),
            };
            return hmLocations;
        }

        private static List<GenericItemsData> GetReturnToTheGraveyardData()
        {
            List<GenericItemsData> rtgLocations = new List<GenericItemsData>() {
                new GenericItemsData("Star Rune: Return to the Graveyard", Addresses.RTG_Pickup_StarRune, "2", "32896"),
                new GenericItemsData("Equipment: Silver Shield in Chest at Shop", Addresses.RTG_Pickup_SilverShieldChestAtShop, "2", "32896", true),
                new GenericItemsData("Skill: Daring Dash", Addresses.RTG_Pickup_DaringDash, "2", "257"),
                new GenericItemsData("Energy Vial: Coffin Area West - RTG", Addresses.RTG_Pickup_EnergyVialCoffinAreaWest, "2", "32896"),
                new GenericItemsData("Energy Vial: Coffin Area East - RTG", Addresses.RTG_Pickup_EnergyVialCoffinAreaEast, "2", "32896"),
                new GenericItemsData("Energy Vial: Below Shop - RTG", Addresses.RTG_Pickup_EnergyVialBelowShop, "2", "32896"),
                new GenericItemsData("Energy Vial: Undertakers Entrance - RTG", Addresses.RTG_Pickup_EnergyVialUndertakersEntrance, "2", "32896"),
                new GenericItemsData("Energy Vial: Cliffs Right - RTG", Addresses.RTG_Pickup_EnergyVialCliffsRight, "2", "32896"),
                new GenericItemsData("Energy Vial: Cliffs Left - RTG", Addresses.RTG_Pickup_EnergyVialCliffsLeft, "2", "32896"),
                new GenericItemsData("Gold Coins: Chest in Coffin Area 1 - RTG", Addresses.RTG_Pickup_GoldCoinsChestInCoffinArea1, "2", "32896"),
                new GenericItemsData("Gold Coins: Chest in Coffin Area 2 - RTG", Addresses.RTG_Pickup_GoldCoinsChestInCoffinArea2, "2", "32896"),
                new GenericItemsData("Gold Coins: Chest in Coffin Area 3 - RTG", Addresses.RTG_Pickup_GoldCoinsChestInCoffinArea3, "2", "32896"),
                new GenericItemsData("Gold Coins: Chest in Coffin Area 4 - RTG", Addresses.RTG_Pickup_GoldCoinsChestInCoffinArea4, "2", "32896"),
                new GenericItemsData("Gold Coins: Chest in Coffin Area 5 - RTG", Addresses.RTG_Pickup_GoldCoinsChestInCoffinArea5, "2", "32896"),
                new GenericItemsData("Gold Coins: Bag above Coffin Area - RTG", Addresses.RTG_Pickup_GoldCoinsBagAboveCoffinArea, "2", "32896"),
                new GenericItemsData("Gold Coins: Bag after Bridge - RTG", Addresses.RTG_Pickup_GoldCoinsBagAfterBridge, "2", "32896"),
                new GenericItemsData("Gold Coins: Bag at Shop - RTG", Addresses.RTG_Pickup_GoldCoinsBagAtShop, "2", "32896"),
                new GenericItemsData("Gold Coins: Bag at Closed Gate - RTG", Addresses.RTG_Pickup_GoldCoinsBagAtClosedGate, "2", "32896"),
                new GenericItemsData("Gold Coins: Chest on Island - RTG", Addresses.RTG_Pickup_GoldCoinsChestOnIsland, "2", "32896"),
                new GenericItemsData("Gold Coins: Undertakers Entrance - RTG", Addresses.RTG_Pickup_GoldCoinsUndertakersEntrance, "2", "32896"),
                new GenericItemsData("Gold Coins: Cliffs Left - RTG", Addresses.RTG_Pickup_GoldCoinsCliffsLeft, "2", "32896"),
                new GenericItemsData("Book: Secret Areas - RTG", Addresses.RTG_Book_SecretAreas, "2", "0"),
                new GenericItemsData("Book: Skull Key - RTG", Addresses.RTG_Book_SkullKey, "2", "0"),
                new GenericItemsData("Book: Daring Dash - RTG", Addresses.RTG_Book_DaringDash, "2", "0"),
                new GenericItemsData("Gargoyle: Exit - RTG", Addresses.RTG_Gargoyle_Exit, "2", "0"),
                new GenericItemsData("Cleared: Return to the Graveyard", Addresses.RTG_LevelStatus, "2", "16"),
                new GenericItemsData("Chalice: Return to the Graveyard", Addresses.RTG_Pickup_Chalice, "2", "32896"),
            };
            return rtgLocations;
        }

        private static List<GenericItemsData> GetScarecrowFieldsData()
        {
            List<GenericItemsData> sfLocations = new List<GenericItemsData>() {

                new GenericItemsData("Key Item: Harvester Parts - SF", Addresses.SF_Pickup_HarvesterPart, "5", "32896"),
                new GenericItemsData("Chaos Rune: Scarecrow Fields", Addresses.SF_Pickup_ChaosRune, "5", "32800"),
                new GenericItemsData("Earth Rune: Scarecrow Fields", Addresses.SF_Pickup_EarthRune, "5", "32896"),
                new GenericItemsData("Moon Rune: Scarecrow Fields", Addresses.SF_Pickup_MoonRune, "5", "32800"),
                new GenericItemsData("Equipment: Club Inside Hut - SF", Addresses.SF_Pickup_ClubInsideHut, "5", "32896", true),
                new GenericItemsData("Equipment: Silver Shield Behind Windmill - SF", Addresses.SF_Pickup_SilverShieldBehindWindmill, "5", "32896", true),
                new GenericItemsData("Equipment: Copper Shield in Chest In the Barn - SF", Addresses.SF_Pickup_CopperShieldChestInTheBarn, "5", "32896", true),
                new GenericItemsData("Energy Vial: Right of fire near Moon Door - SF", Addresses.SF_Pickup_EnergyVialRightOfFireNearMoonDoor, "5", "32896"),
                new GenericItemsData("Energy Vial: Cornfield Path - SF", Addresses.SF_Pickup_EnergyVialCornfieldPath, "5", "32896"),
                new GenericItemsData("Gold Coins: Haystack at Beginning - SF", Addresses.SF_Pickup_GoldCoinsHaystackAtBeginning, "5", "128"),
                new GenericItemsData("Gold Coins: Chest in Haystack near Moon Door - SF", Addresses.SF_Pickup_GoldCoinsChestInHaystackNearMoonDoor, "5", "128"),
                new GenericItemsData("Gold Coins: Left of fire near Moon Door - SF", Addresses.SF_Pickup_GoldCoinsLeftOfFireNearMoonDoor, "5", "32896"),
                new GenericItemsData("Gold Coins: Bag in the Barn - SF", Addresses.SF_Pickup_GoldCoinsBagInTheBarn, "5", "32896"),
                new GenericItemsData("Gold Coins: Cornfield Square near Barn - SF", Addresses.SF_Pickup_GoldCoinsCornfieldSquareNearBarn, "5", "32896"),
                new GenericItemsData("Gold Coins: Cornfield Path 1 - SF", Addresses.SF_Pickup_GoldCoinsCornfieldPath1, "5", "32896"),
                new GenericItemsData("Gold Coins: Chest Under Haybail - SF", Addresses.SF_Pickup_GoldCoinsChestUnderHaybail, "5", "128"),
                new GenericItemsData("Gold Coins: Bag under Barn Haybail - SF", Addresses.SF_Pickup_GoldCoinsBagUnderBarnHaybail, "5", "128"),
                new GenericItemsData("Gold Coins: Bag in the Press - SF", Addresses.SF_Pickup_GoldCoinsBagInThePress, "5", "32896"),
                new GenericItemsData("Gold Coins: Bag in the Spinner - SF", Addresses.SF_Pickup_GoldCoinsBagInTheSpinner, "5", "32896"),
                new GenericItemsData("Gold Coins: Chest next to Harvester Part - SF", Addresses.SF_Pickup_GoldCoinsChestNextToHarvesterPart, "5", "32896"),
                new GenericItemsData("Book: Scarecrows - SF", Addresses.SF_Book_Scarecrows, "5", "0"),
                new GenericItemsData("Book: Kul Katura - SF", Addresses.SF_Book_KulKatura, "5", "0"),
                new GenericItemsData("Book: Cornfields - SF", Addresses.SF_Book_Cornfields, "5", "0"),
                new GenericItemsData("Book: Mad Machines - SF", Addresses.SF_Book_MadMachines, "5", "0"),
                new GenericItemsData("Book: Corn Cutter - SF", Addresses.SF_Book_CornCutter, "5", "0"),
                new GenericItemsData("Gargoyle: Exit - SF", Addresses.SF_Gargoyle_Exit, "5", "0"),
                new GenericItemsData("Cleared: Scarecrow Fields", Addresses.SF_LevelStatus, "5", "16"),

            };
            return sfLocations;
        }

        private static List<GenericItemsData> GetAnthillData()
        {
            List<GenericItemsData> ahLocations = new List<GenericItemsData>() {
                new GenericItemsData("Key Item: Amber Piece 1 - TA", Addresses.AmberPiece, "7", "1"),
                new GenericItemsData("Key Item: Amber Piece 2 - TA", Addresses.AmberPiece, "7", "2"),
                new GenericItemsData("Key Item: Amber Piece 3 - TA", Addresses.AmberPiece, "7", "3"),
                new GenericItemsData("Key Item: Amber Piece 4 - TA", Addresses.AmberPiece, "7", "4"),
                new GenericItemsData("Key Item: Amber Piece 5 - TA", Addresses.AmberPiece, "7", "5"),
                new GenericItemsData("Key Item: Amber Piece 6 - TA", Addresses.AmberPiece, "7", "6"),
                new GenericItemsData("Key Item: Amber Piece 7 - TA", Addresses.AmberPiece, "7", "7"),
                new GenericItemsData("Key Item: Amber Piece 8 - TA", Addresses.AmberPiece, "7", "8"),
                new GenericItemsData("Key Item: Amber Piece 9 - TA", Addresses.AmberPiece, "7", "9"),
                new GenericItemsData("Key Item: Amber Piece 10 - TA", Addresses.AmberPiece, "7", "10"),
                new GenericItemsData("Fairy 1 - TA", Addresses.FairyCount, "7", "1"),
                new GenericItemsData("Fairy 2 - TA", Addresses.FairyCount, "7", "2"),
                new GenericItemsData("Fairy 3 - TA", Addresses.FairyCount, "7", "3"),
                new GenericItemsData("Fairy 4 - TA", Addresses.FairyCount, "7", "4"),
                new GenericItemsData("Fairy 5 - TA", Addresses.FairyCount, "7", "5"),
                new GenericItemsData("Fairy 6 - TA", Addresses.FairyCount, "7", "6"),
                new GenericItemsData("Equipment: Club in Chest at Barrier - TA", Addresses.TA_Pickup_ClubChestAtBarrier, "7", "32896", true),
                new GenericItemsData("Equipment: Chicken Drumsticks - TA", Addresses.TA_LevelStatus, "7", "16"),
                new GenericItemsData("Energy Vial: Before Fairy 1 - TA", Addresses.TA_Pickup_EnergyVialBeforeFairy1, "7", "32896"),
                new GenericItemsData("Energy Vial: After Amber 2 - TA", Addresses.TA_Pickup_EnergyVialAfterAmber2, "7", "32896"),
                new GenericItemsData("Energy Vial: Fairy 2 Room Center - TA", Addresses.TA_Pickup_EnergyVialFairy2RoomCenter, "7", "32896"),
                new GenericItemsData("Energy Vial: Fairy 3 - TA", Addresses.TA_Pickup_EnergyVialFairy3, "7", "32896"),
                new GenericItemsData("Energy Vial: Birthing room exit - TA", Addresses.TA_Pickup_EnergyVialBirthingRoomExit, "7", "32896"),
                new GenericItemsData("Gold Coins: Chest at Barrier Fairy - TA", Addresses.TA_Pickup_GoldCoinsChestAtBarrierFairy, "7", "32896"),
                new GenericItemsData("Book: Fairy Portal - AH", Addresses.AH_Book_FairyPortal, "7", "0"),
                new GenericItemsData("Book: Queen Ant - AH", Addresses.AH_Book_QueenAnt, "7", "0"),
                new GenericItemsData("Gargoyle: Entrance - AH", Addresses.AH_Gargoyle_Entrance, "7", "0"),
                new GenericItemsData("Cleared: Ant Hill", Addresses.TA_LevelStatus, "7", "16"),
                new GenericItemsData("Chalice: Ant Hill", Addresses.TA_LevelStatus, "7", "19"),
            };
            return ahLocations;
        }

        private static List<GenericItemsData> GetEnchantedEarthData()
        {
            List<GenericItemsData> eeLocations = new List<GenericItemsData>() {

                new GenericItemsData("Key Item: Shadow Talisman - EE", Addresses.EE_Pickup_ShadowTalisman, "15", "32896"),
                new GenericItemsData("Star Rune: Enchanted Earth", Addresses.EE_Pickup_StarRune, "15", "4865"),
                new GenericItemsData("Egg Drop 1 - EE", Addresses.EE_Pickup_GoldCoinsChestInEgg, "15", "32768"),
                new GenericItemsData("Egg Drop 2 - EE", Addresses.EE_Pickup_CopperShieldInEgg, "15", "32768"), 
                new GenericItemsData("Egg Drop 3 - EE", Addresses.EE_Pickup_EarthRune, "15", "32768"), 
                new GenericItemsData("Energy Vial: Shadow Talisman Cave - EE", Addresses.EE_Pickup_EnergyVialShadowTalismanCave, "15", "32896"),
                new GenericItemsData("Energy Vial: Left of Tree Drop - EE", Addresses.EE_Pickup_EnergyVialLeftOfTreeDrop, "15", "32896"),
                new GenericItemsData("Energy Vial: Right of Tree Drop - EE", Addresses.EE_Pickup_EnergyVialRightOfTreeDrop, "15", "32896"),
                new GenericItemsData("Gold Coins: Bag Near Tree Hollow - EE", Addresses.EE_Pickup_GoldCoinsBagNearTreeHollow, "15", "32896"),
                new GenericItemsData("Gold Coins: Bag Behind Big Tree 1 - EE", Addresses.EE_Pickup_GoldCoinsBagBehindBigTree1, "15", "32896"),
                new GenericItemsData("Gold Coins: Bag Behind Big Tree 2 - EE", Addresses.EE_Pickup_GoldCoinsBagBehindBigTree2, "15", "32896"),
                new GenericItemsData("Gold Coins: Bag at Cave Entrance - EE", Addresses.EE_Pickup_GoldCoinsBagAtCaveEntrance, "15", "32896"),
                new GenericItemsData("Gold Coins:Chest Near Barrier - EE", Addresses.EE_Pickup_GoldCoinsChestNearBarrier, "15", "32896"),
                new GenericItemsData("Gold Coins: Chest Left of Fountain - EE", Addresses.EE_Pickup_GoldCoinsChestLeftOfFountain, "15", "32896"),
                new GenericItemsData("Gold Coins: Chest Top of Fountain - EE", Addresses.EE_Pickup_GoldCoinsChestTopOfFountain, "15", "32896"),
                new GenericItemsData("Gold Coins: Chest Right of Fountain - EE", Addresses.EE_Pickup_GoldCoinsChestRightOfFountain, "15", "32896"),
                new GenericItemsData("Book: Coven of Witches - EE", Addresses.EE_Book_CovenOfWitches, "15", "0"),
                new GenericItemsData("Book: Dragon Bird - EE", Addresses.EE_Book_DragonBird, "15", "0"),
                new GenericItemsData("Book: Take the Talisman - EE", Addresses.EE_Book_TakeTheTalisman, "15", "0"),
                new GenericItemsData("Gargoyle: Outside Demon Entrance - EE", Addresses.EE_Gargoyle_OutsideDemonEntrance, "15", "0"),
                new GenericItemsData("Gargoyle: Outside Demon Exit - EE", Addresses.EE_Gargoyle_OutsideDemonExit, "15", "0"),
                new GenericItemsData("Cleared: Enchanted Earth", Addresses.EE_LevelStatus, "15", "16"),
                new GenericItemsData("Chalice: Enchanted Earth", Addresses.EE_Pickup_Chalice, "15", "32896"),
            };
            return eeLocations;
        }

        private static List<GenericItemsData> GetTheSleepingVillageData()
        {
            List<GenericItemsData> tsvLocations = new List<GenericItemsData>() {
                new GenericItemsData("Earth Rune: Sleeping Village", Addresses.TSV_Pickup_EarthRune,"11", "32896"),
                new GenericItemsData("Chaos Rune: Sleeping Village", Addresses.TSV_Pickup_ChaosRune, "11", "32896"),
                new GenericItemsData("Moon Rune: Sleeping Village", Addresses.TSV_Pickup_MoonRune, "11", "32896"),
                new GenericItemsData("Key Item: Safe Key - SV", Addresses.TSV_Pickup_SafeKey, "11", "32896"),
                new GenericItemsData("Key Item: Shadow Artefact - SV", Addresses.TSV_Pickup_ShadowArtefact, "11", "32896"),
                new GenericItemsData("Key Item: Crucifix - SV", Addresses.TSV_Pickup_Crucifix,"11",  "41208"),
                new GenericItemsData("Key Item: Landlords Bust - SV", Addresses.TSV_Pickup_LandlordsBust, "11", "32896"),
                new GenericItemsData("Key Item: Crucifix Cast - SV", Addresses.TSV_Pickup_CrucifixCast,"11",  "32896"),
                new GenericItemsData("Equipment: Silver Shield in Blacksmiths - SV", Addresses.TSV_Pickup_SilverShieldInBlacksmiths, "11", "32896", true),
                new GenericItemsData("Equipment: Club Chest under Inn Stairs - SV", Addresses.TSV_Pickup_ClubInChestUnderInnStairs, "11", "32896", true),
                new GenericItemsData("Energy Vial: At Pond - SV", Addresses.TSV_Pickup_EnergyVialAtPond, "11", "32896"),
                new GenericItemsData("Energy Vial: Bust Switch - SV", Addresses.TSV_Pickup_EnergyVialBustSwitch, "11", "32896"),
                new GenericItemsData("Energy Vial: Near Exit - SV", Addresses.TSV_Pickup_EnergyVialNearExit, "11", "32896"),
                new GenericItemsData("Energy Vial: Near Chalice - SV", Addresses.TSV_Pickup_EnergyVialNearChalice, "11", "32896"),
                new GenericItemsData("Gold Coins: Bag in Left Barrel at Blacksmith - SV", Addresses.TSV_Pickup_GoldCoinsBagInLeftBarrelAtBlacksmith, "11", "32896", true),
                new GenericItemsData("Gold Coins: Bag in Right Barrel at Blacksmith - SV", Addresses.TSV_Pickup_GoldCoinsBagInRightBarrelAtBlacksmith, "11", "32896", true),
                new GenericItemsData("Gold Coins: Bag at Pond - SV", Addresses.TSV_Pickup_GoldCoinsBagAtPond, "11", "32896"),
                new GenericItemsData("Gold Coins: Bag in Barrel at Inn - SV", Addresses.TSV_Pickup_GoldCoinsBagInBarrelAtInn, "11", "32896", true),
                new GenericItemsData("Gold Coins: Bag in Barrel at bottom of Inn Stairs - SV", Addresses.TSV_Pickup_GoldCoinsBagInBarrelAtBottomOfInnStairs, "11", "32896",true),
                new GenericItemsData("Gold Coins: Bag in Barrel Behind Inn Stairs - SV", Addresses.TSV_Pickup_GoldCoinsBagInBarrelBehindInnStairs, "11", "32896", true),
                new GenericItemsData("Gold Coins: Bag In Top Bust Barrel - SV", Addresses.TSV_Pickup_GoldCoinsBagInTopBustBarrel, "11", "32896", true),
                new GenericItemsData("Gold Coins: Bag In Switch Bust Barrel - SV", Addresses.TSV_Pickup_GoldCoinsBagInSwitchBustBarrel, "11", "32896", true),
                new GenericItemsData("Gold Coins: Bag in Library - SV", Addresses.TSV_Pickup_GoldCoinsBagInLibrary, "11", "32896"),
                new GenericItemsData("Gold Coins: Bag at Top of table - SV", Addresses.TSV_Pickup_GoldCoinsBagAtTopOfTable, "11", "32896"),
                new GenericItemsData("Gold Coins: Bag at Bottom of table - SV", Addresses.TSV_Pickup_GoldCoinsBagAtBottomOfTable, "11", "32896"),
                new GenericItemsData("Gold Coins: Chest next to Chalice - SV", Addresses.TSV_Pickup_GoldCoinsChestNextToChalice, "11", "32896"),
                new GenericItemsData("Book: Blacksmiths Montly - SV", Addresses.TSV_Book_BlacksmithsMontly, "11", "0"),
                new GenericItemsData("Book: Missing Crucifix - SV", Addresses.TSV_Book_MissingCrucifix, "11", "0"),
                new GenericItemsData("Book: Fountain Rune - SV", Addresses.TSV_Book_FountainRune, "11", "0"),
                new GenericItemsData("Book: Mayors Bust - SV", Addresses.TSV_Book_MayorsBust, "11", "0"),
                new GenericItemsData("Book: History of Gallowmere 1 - SV", Addresses.TSV_Book_HistoryOfGallowmere1, "11", "0"),
                new GenericItemsData("Book: History of Gallowmere 2 - SV", Addresses.TSV_Book_HistoryOfGallowmere2, "11", "0"),
                new GenericItemsData("Book: History of Gallowmere 3 - SV", Addresses.TSV_Book_HistoryOfGallowmere3, "11", "0"),
                new GenericItemsData("Book: History of Gallowmere 4 - SV", Addresses.TSV_Book_HistoryOfGallowmere4, "11", "0"),
                new GenericItemsData("Book: Heroes From History - SV", Addresses.TSV_Book_HeroesFromHistory, "11", "0"),
                new GenericItemsData("Book: Tourist Guide 1 - SV", Addresses.TSV_Book_TouristGuide1, "11", "0"),
                new GenericItemsData("Book: Tourist Guide 2 - SV", Addresses.TSV_Book_TouristGuide2, "11", "0"),
                new GenericItemsData("Book: Mayor Memoire - SV", Addresses.TSV_Book_MayorMemoire, "11", "0"),
                new GenericItemsData("Book: Mayors Regrets - SV", Addresses.TSV_Book_MayorsRegrets, "11", "0"),
                new GenericItemsData("Book: Zaroks Note - SV", Addresses.TSV_Book_ZaroksNote, "11", "0"),
                new GenericItemsData("Gargoyle: Entrance - SV", Addresses.TSV_Gargoyle_Entrance, "11", "0"),
                new GenericItemsData("Cleared: Sleeping Village", Addresses.TSV_LevelStatus, "11", "16"),
                new GenericItemsData("Chalice: Sleeping Village", Addresses.TSV_Pickup_Chalice, "11", "32896"),
            };
            return tsvLocations;
        }

        private static List<GenericItemsData> GetPoolsOfTheAncientDeadData()
        {
            List<GenericItemsData> padLocations = new List<GenericItemsData>() {
                new GenericItemsData("Life Bottle: Pools of the Ancient Dead", Addresses.PAD_Pickup_LifeBottle, "12", "32896"),
                new GenericItemsData("Chaos Rune: Pools of the Ancient Dead", Addresses.PAD_Pickup_ChaosRune, "12", "32896"),
                new GenericItemsData("Key Item: Soul Helmet 1 - PAD", Addresses.PAD_Pickup_LostSoul1, "12", "32896"),
                new GenericItemsData("Key Item: Soul Helmet 2 - PAD", Addresses.PAD_Pickup_LostSoul2, "12", "32896"),
                new GenericItemsData("Key Item: Soul Helmet 3 - PAD", Addresses.PAD_Pickup_LostSoul3, "12", "32896"),
                new GenericItemsData("Key Item: Soul Helmet 4 - PAD", Addresses.PAD_Pickup_LostSoul4, "12", "32896"),
                new GenericItemsData("Key Item: Soul Helmet 5 - PAD", Addresses.PAD_Pickup_LostSoul5, "12", "32896"),
                new GenericItemsData("Key Item: Soul Helmet 6 - PAD", Addresses.PAD_Pickup_LostSoul6, "12", "32896"),
                new GenericItemsData("Key Item: Soul Helmet 7 - PAD", Addresses.PAD_Pickup_LostSoul7, "12", "32896"),
                new GenericItemsData("Key Item: Soul Helmet 8 - PAD", Addresses.PAD_Pickup_LostSoul8, "12", "32896"),
                new GenericItemsData("Equipment: Silver Shield in Chest Near Soul 5 - PAD", Addresses.PAD_Pickup_SilverShieldInChestNearSoul5, "12", "32896", true),
                new GenericItemsData("Energy Vial: Broken Structure near Entrance - PAD", Addresses.PAD_Pickup_EnergyVialBrokenStructureNearEntrance, "12", "32896"),
                new GenericItemsData("Energy Vial: Next to Lost Soul 3 - PAD", Addresses.PAD_Pickup_EnergyVialNextToLostSoul3, "12", "32896"),
                new GenericItemsData("Energy Vial: Near Gate - PAD", Addresses.PAD_Pickup_EnergyVialNearGate, "12", "32896"),
                new GenericItemsData("Energy Vial: Chariot Right - PAD", Addresses.PAD_Pickup_EnergyVialChariotRight, "12", "32896"),
                new GenericItemsData("Energy Vial: Chariot Left - PAD", Addresses.PAD_Pickup_EnergyVialChariotLeft, "12", "32896"),
                new GenericItemsData("Energy Vial: Jump Spot 1 - PAD", Addresses.PAD_Pickup_EnergyVialJumpSpot1, "12", "32896"),
                new GenericItemsData("Energy Vial: Jump Spot 2 - PAD", Addresses.PAD_Pickup_EnergyVialJumpSpot2, "12", "32896"),
                new GenericItemsData("Gold Coins: Bag at Entrance - PAD", Addresses.PAD_Pickup_GoldCoinsBagAtEntrance, "12", "32896"),
                new GenericItemsData("Gold Coins: Bag on Island Near Soul 2 - PAD", Addresses.PAD_Pickup_GoldCoinsBagOnIslandNearSoul2, "12", "32896"),
                new GenericItemsData("Gold Coins: Jump Spot 1 - PAD", Addresses.PAD_Pickup_GoldCoinsJumpSpot1, "12", "32896"),
                new GenericItemsData("Gold Coins: Jump Spot 2 - PAD", Addresses.PAD_Pickup_GoldCoinsJumpSpot2, "12", "32896"),
                new GenericItemsData("Book: Enemy Warning - PAD", Addresses.PAD_Book_EnemyWarning, "12", "0"),
                new GenericItemsData("Gargoyle: Entrance - PAD", Addresses.PAD_Gargoyle_Entrance, "12", "0"),
                new GenericItemsData("Cleared: Pools of the Ancient Dead", Addresses.PAD_LevelStatus, "12", "16"),
                new GenericItemsData("Chalice: Pools of the Ancient Dead", Addresses.PAD_Pickup_Chalice, "12", "32896"),
            };
            return padLocations;
        }

        private static List<GenericItemsData> GetTheLakeData()
        {
            List<GenericItemsData> tlLocations = new List<GenericItemsData>() {
                new GenericItemsData("Chaos Rune: The Lake", Addresses.TL_Pickup_ChaosRune, "22", "32896"),
                new GenericItemsData("Earth Rune: The Lake", Addresses.TL_Pickup_EarthRune, "22", "32896"),
                new GenericItemsData("Star Rune: The Lake", Addresses.TL_Pickup_StarRune, "22", "32896"),
                new GenericItemsData("Time Rune: The Lake", Addresses.TL_Pickup_TimeRune, "22", "32896"),
                new GenericItemsData("Equipment: Silver Shield In Whirlpool - TL", Addresses.TL_Pickup_SilverShieldInWhirlpool, "22", "32896", true),
                new GenericItemsData("Energy Vial: Flooded House - TL", Addresses.TL_Pickup_EnergyVialFloodedHouse, "22", "32896"),
                new GenericItemsData("Energy Vial: Whirpool Wind 1 - TL", Addresses.TL_Pickup_EnergyVialWhirlpoolWind1, "22", "32896"),
                new GenericItemsData("Energy Vial: Whirpool Wind 2 - TL", Addresses.TL_Pickup_EnergyVialWhirlpoolWind2, "22", "32896"),
                new GenericItemsData("Gold Coins: Bag Outside Flooded House - TL", Addresses.TL_Pickup_GoldCoinsBagOutsideFloodedHouse, "22", "32896"),
                new GenericItemsData("Gold Coins: Bag Near Closed Gate - TL", Addresses.TL_Pickup_GoldCoinsBagNearClosedGate, "22", "32896"),
                new GenericItemsData("Gold Coins: Bag at the Whirlpool Entrance - TL", Addresses.TL_Pickup_GoldCoinsBagAtTheWhirlpoolEntrance, "22", "32896"),
                new GenericItemsData("Gold Coins: Whirlpool Wind 1 - TL", Addresses.TL_Pickup_GoldCoinsWhirlpoolWind1, "22", "32896"),
                new GenericItemsData("Gold Coins: Whirlpool Wind 2 - TL", Addresses.TL_Pickup_GoldCoinsWhirlpoolWind2, "22", "32896"),
                new GenericItemsData("Gold Coins: Outside Whirlpool Exit - TL", Addresses.TL_Pickup_GoldCoinsOutsideWhirlpoolExit, "22", "32896"),
                new GenericItemsData("Gold Coins: Chest in Whirlpool Switch Area - TL", Addresses.TL_Pickup_GoldChestWhirlpoolSwitchArea, "22", "32896"),
                new GenericItemsData("Book: Learn to Stealth - TL", Addresses.TL_Book_LearnToStealth, "22", "0"),
                new GenericItemsData("Book: Whirlpool Manual - TL", Addresses.TL_Book_WhirlpoolManual, "22", "0"),
                new GenericItemsData("Gargoyle: Exit - TL", Addresses.TL_Gargoyle_Exit, "22", "0"),
                new GenericItemsData("Cleared: The Lake", Addresses.TL_LevelStatus, "22", "16"),
                new GenericItemsData("Chalice: The Lake", Addresses.TL_Pickup_Chalice, "22", "32896"),
            };
            return tlLocations;
        }

        private static List<GenericItemsData> GetTheCrystalCavesData()
        {
            List<GenericItemsData> ccLocations = new List<GenericItemsData>() {

                new GenericItemsData("Earth Rune: The Crystal Caves", Addresses.CC_Pickup_EarthRune, "8", "32896"),
                new GenericItemsData("Star Rune: The Crystal Caves", Addresses.CC_Pickup_StarRune, "8", "32896"),
                new GenericItemsData("Equipment: Silver Shield in Crystal - CC", Addresses.CC_Pickup_SilverShieldInCrystal, "8", "128"),
                new GenericItemsData("Equipment: Dragon Armour - CC", Addresses.CC_Pickup_DragonArmour, "8", "32896"),
                new GenericItemsData("Energy Vial: Dragon Room 1st Platform - CC", Addresses.CC_Pickup_EnergyVialDragonRoom1stPlatform, "8", "32896"),
                new GenericItemsData("Energy Vial: Dragon Room 3rd Platform - CC", Addresses.CC_Pickup_EnergyVialDragonRoom3rdPlatform, "8", "32896"),
                new GenericItemsData("Gold Coins: Bag at bottom of winding staircase  - CC", Addresses.CC_Pickup_GoldCoinsBagAtWindingStaircase, "8", "32896"),
                new GenericItemsData("Gold Coins: Chest in Crystal after Pool - CC", Addresses.CC_Pickup_GoldCoinsChestInCrystalAfterPool, "8", "128"),
                new GenericItemsData("Gold Coins: Bag in Crystal at Start - CC", Addresses.CC_Pickup_GoldCoinsBagInCrystalAtStart, "8", "128"),
                new GenericItemsData("Gold Coins: Bag in Spinner - CC", Addresses.CC_Pickup_GoldCoinsBagInSpinner, "8", "32896"),
                new GenericItemsData("Gold Coins: Bag near Silver Shield - CC", Addresses.CC_Pickup_GoldCoinsBagNearSilverShield, "8", "32896"),
                new GenericItemsData("Gold Coins: Chest in Crystal After Earth Door - CC", Addresses.CC_Pickup_GoldCoinsChestInCrystalAfterEarthDoor, "8", "128"),
                new GenericItemsData("Gold Coins: Bag in Dragon Room 1 1st Platform - CC", Addresses.CC_Pickup_GoldCoinsBagInDragonRoom11stPlatform, "8", "32896"),
                new GenericItemsData("Gold Coins: Bag in Dragon Room 2 1st Platform - CC", Addresses.CC_Pickup_GoldCoinsBagInDragonRoom21stPlatform, "8", "32896"),
                new GenericItemsData("Gold Coins: Chest in Dragon Room 1st Platform - CC", Addresses.CC_Pickup_GoldCoinsChestInDragonRoom1stPlatform, "8", "32896"),
                new GenericItemsData("Gold Coins: Bag in Dragon Room 2nd Platform - CC", Addresses.CC_Pickup_GoldCoinsBagInDragonRoom2ndPlatform, "8", "32896"),
                new GenericItemsData("Gold Coins: Bag in Dragon Room 1 3rd Platform - CC", Addresses.CC_Pickup_GoldCoinsBagInDragonRoom13rdPlatform, "8", "32896"),
                new GenericItemsData("Gold Coins: Bag in Dragon Room 2 3rd Platform - CC", Addresses.CC_Pickup_GoldCoinsBagInDragonRoom23rdPlatform,"8",  "32896"),
                new GenericItemsData("Gold Coins: Chest in Dragon Room 3rd Platform - CC", Addresses.CC_Pickup_GoldCoinsChestInDragonRoom3rdPlatform, "8", "32896"),
                new GenericItemsData("Gold Coins: Bag in Dragon Room 4th Platform 1 - CC", Addresses.CC_Pickup_GoldCoinsBagInDragonRoom4thPlatform1, "8", "32896"),
                new GenericItemsData("Gold Coins: Chest in Dragon Room 4th Platform - CC", Addresses.CC_Pickup_GoldCoinsChestInDragonRoom4thPlatform, "8", "32896"),
                new GenericItemsData("Gold Coins: Bag in Dragon Room 4th Platform 2 - CC", Addresses.CC_Pickup_GoldCoinsBagInDragonRoom4thPlatform2, "8", "32896"),
                new GenericItemsData("Gold Coins: Bag on Left of Pool - CC", Addresses.CC_Pickup_GoldCoinsBagOnLeftOfPool, "8", "32896"),
                new GenericItemsData("Gold Coins: Bag on Right of Pool - CC", Addresses.CC_Pickup_GoldCoinsBagOnRightOfPool, "8", "32896"),
                new GenericItemsData("Book: Dragon Book - CC", Addresses.CC_Book_DragonBook, "8", "0"),
                new GenericItemsData("Gargoyle: Cave Entrance - CC", Addresses.CC_Gargoyle_CaveEntrance, "8", "0"),
                new GenericItemsData("Cleared: The Crystal Caves", Addresses.CC_LevelStatus, "8", "16"),
                new GenericItemsData("Chalice: The Crystal Caves", Addresses.CC_Pickup_Chalice, "8",  "32896"),
            };
            return ccLocations;
        }
        private static List<GenericItemsData> GetGallowsGauntletData()
        {
            List<GenericItemsData> ggLocations = new List<GenericItemsData>() {
                new GenericItemsData("Star Rune: The Gallows Gauntlet", Addresses.GG_Pickup_StarRune, "16", "32896"),
                new GenericItemsData("Equipment: Silver Shield in Chest Near Exit - GG", Addresses.GG_Pickup_SilverShieldInChestNearExit, "16", "32896", true),
                new GenericItemsData("Energy Vial: Near Chalice - GG", Addresses.GG_Pickup_EnergyVialNearChalice, "16", "32896"),
                new GenericItemsData("Gold Coins: Bag Behind Stone Dragon 1 - GG", Addresses.GG_Pickup_GoldCoinsBagBehindStoneDragon1, "16", "32896"),
                new GenericItemsData("Gold Coins: Bag Behind Stone Dragon 2 - GG", Addresses.GG_Pickup_GoldCoinsBagBehindStoneDragon2, "16", "32896"),
                new GenericItemsData("Gold Coins: Chest at Serpent - GG", Addresses.GG_Pickup_GoldCoinsChestAtSerpent, "16", "32896"),
                new GenericItemsData("Gold Coins: Chest Near Star Entrance - GG", Addresses.GG_Pickup_GoldCoinsChestNearStarEntrance, "16", "32896"),
                new GenericItemsData("Book: Serpent of Gallowmere - GG", Addresses.GG_Book_SerpentOfGallowmere, "16", "0"),
                new GenericItemsData("Book: Dragon Armour - GG", Addresses.GG_Book_DragonArmour, "16", "0"),
                new GenericItemsData("Book: Early Exit - GG", Addresses.GG_Book_EarlyExit, "16", "0"),
                new GenericItemsData("Book: Magical Barrier - GG", Addresses.GG_Book_MagicalBarrier, "16", "0"),
                new GenericItemsData("Cleared: The Gallows Gauntlet", Addresses.GG_LevelStatus, "16", "16"),
                new GenericItemsData("Chalice: The Gallows Gauntlet", Addresses.GG_Pickup_Chalice, "16", "32896"),
            };
            return ggLocations;
        }
        private static List<GenericItemsData> GetTheAsylumGroundsData()
        {
            List<GenericItemsData> agLocations = new List<GenericItemsData>() {
                new GenericItemsData("Chaos Rune: Asylum Grounds", Addresses.AG_Pickup_ChaosRune, "13", "32896"),
                new GenericItemsData("Equipment: Silver Shield in Chest Behind Door - AG", Addresses.AG_Pickup_SilverShieldInChestBehindDoor, "13", "32896", true),
                new GenericItemsData("Energy Vial: Near Bishop - AG", Addresses.AG_Pickup_EnergyVialNearBishop, "13", "32896"),
                new GenericItemsData("Energy Vial: Near King - AG", Addresses.AG_Pickup_EnergyVialNearKing, "13", "32896"),
                new GenericItemsData("Gold Coins: Bag in Bell Grave Near Bell - AG", Addresses.AG_Pickup_GoldCoinsBagInBellGraveNearBell, "13", "32896"),
                new GenericItemsData("Gold Coins: Bag in Bell Grave Near Entrance - AG", Addresses.AG_Pickup_GoldCoinsBagInBellGraveNearEntrance, "13", "32896"),
                new GenericItemsData("Gold Coins: Bag Near Shooting Statue - AG", Addresses.AG_Pickup_GoldCoinsBagNearShootingStatue, "13", "32896"),
                new GenericItemsData("Gold Coins: Bag in Rat Grave - AG", Addresses.AG_Pickup_GoldCoinsBagInRatGrave, "13", "32896"),
                new GenericItemsData("Gold Coins: Behind Chaos Gate - AG", Addresses.AG_Pickup_GoldCoinsBehindChaosGate, "13", "32896"),
                new GenericItemsData("Gold Coins: Behind Elephant in Grave - AG", Addresses.AG_Pickup_GoldCoinsBehindElephantInGrave, "13", "32896"),
                new GenericItemsData("Book: Seek Jack - AG", Addresses.AG_Book_SeekJack, "13", "0"),
                new GenericItemsData("Book: Secret Exit - AG", Addresses.AG_Book_SecretExit, "13", "0"),
                new GenericItemsData("Gargoyle: Jack of the Green - AG", Addresses.AG_Gargoyle_JackOfTheGreen, "13", "0"),
                new GenericItemsData("Cleared: Asylum Grounds", Addresses.AG_LevelStatus, "13", "16"),
                new GenericItemsData("Chalice: Asylum Grounds", Addresses.AG_Pickup_Chalice, "13", "32896"),
            };
            return agLocations;
        }

        private static List<GenericItemsData> GetInsideTheAsylumData()
        {
            List<GenericItemsData> iaLocations = new List<GenericItemsData>() {
                new GenericItemsData("Earth Rune: Inside the Asylum", Addresses.IA_Pickup_EarthRune, "14", "32896"),
                new GenericItemsData("Key Item: Dragon Gem - IA", Addresses.IA_Pickup_DragonGem, "14", "32896"),
                
                // these have been removed from the  game due to dynamic location issues. I can't figure out when a person has picked them up. Instead we'll be using room clears.
                // new GenericItemsData("Equipment: Silver Shield in Bat Room - IA", Addresses.IA_Pickup_SilverShieldInBatRoom, "14", "32896", false,  true),
                // new GenericItemsData("Energy Vial: Bat Room - IA", Addresses.IA_Pickup_EnergyVialBatRoom, "14", "32896", false,  true),
                // new GenericItemsData("Energy Vial: Asylumn Room 1 - IA", Addresses.IA_Pickup_EnergyVialAsylumRoom1, "14", "32896", false,  true),
                // new GenericItemsData("Energy Vial: Asylumn Room 2 - IA", Addresses.IA_Pickup_EnergyVialAsylumRoom2, "14", "32896", false,  true),
                // new GenericItemsData("Gold Coins: Bag in Bat Room Left - IA", Addresses.IA_Pickup_GoldCoinsBagInBatRoomLeft, "14", "32896", false,  true),
                // new GenericItemsData("Gold Coins: Chest in Bat Room - IA", Addresses.IA_Pickup_GoldCoinsChestInBatRoom, "14", "32896", false,  true),
                // new GenericItemsData("Gold Coins: Bag in Bat Room Centre - IA", Addresses.IA_Pickup_GoldCoinsBagInBatRoomCentre, "14", "32896", false,  true),
                // new GenericItemsData("Gold Coins: Bag in Bat Room Right - IA", Addresses.IA_Pickup_GoldCoinsBagInBatRoomRight, "14", "32896", false,  true),
                // new GenericItemsData("Gold Coins: Bag in Asylumn Room - IA", Addresses.IA_Pickup_GoldCoinsBagInAsylumRoom, "14", "32896", false,  true),


                new GenericItemsData("Gauntlet Cleared: Room 1 - IA", Addresses.IA_MonsterKills, "14", "28"),
                new GenericItemsData("Gauntlet Cleared: Room 2 - IA", Addresses.IA_MonsterKills, "14", "58"),
                new GenericItemsData("Gauntlet Cleared: Room 3 - IA", Addresses.IA_MonsterKills, "14", "93"),
                new GenericItemsData("Gauntlet Cleared: Room 4 - IA", Addresses.IA_MonsterKills, "14", "121"),
                new GenericItemsData("Gauntlet Cleared: Room 5 - IA", Addresses.IA_MonsterKills, "14", "158"),
                new GenericItemsData("Gold Coins: Bag in Sewer Prison Entrance - IA", Addresses.IA_Pickup_GoldCoinsBagInSewerPrisonEntrance, "14", "32896"),
                new GenericItemsData("Gold Coins: Bag on Sewer Prison Bench - IA", Addresses.IA_Pickup_GoldCoinsBagOnSewerPrisonBench, "14", "32896"),
                new GenericItemsData("Cleared: Inside the Asylum", Addresses.IA_LevelStatus, "14", "16"),
                new GenericItemsData("Chalice: Inside the Asylum", Addresses.IA_Pickup_Chalice, "14", "32896"),
            };
            return iaLocations;
        }


        private static List<GenericItemsData> GetPumpkinGorgeData()
        {
            List<GenericItemsData> pgLocations = new List<GenericItemsData>() {

                new GenericItemsData("Time Rune: Pumpkin Gorge", Addresses.PG_Pickup_TimeRune, "9", "256"), // this is still fucky.
                new GenericItemsData("Chaos Rune: Pumpkin Gorge", Addresses.PG_Pickup_ChaosRune, "9", "32896"),
                new GenericItemsData("Earth Rune: Pumpkin Gorge", Addresses.PG_Pickup_EarthRune, "9", "32896"),
                new GenericItemsData("Moon Rune: Pumpkin Gorge", Addresses.PG_Pickup_MoonRune, "9", "32896"),
                new GenericItemsData("Star Rune: Pumpkin Gorge", Addresses.PG_Pickup_StarRune, "9", "32896"),
                new GenericItemsData("Equipment: Club in Chest in Tunnel - PG", Addresses.PG_Pickup_ClubInChestInTunnel, "9", "32896", true),
                new GenericItemsData("Equipment: Silver Shield in Chest at Top of Hill - PG", Addresses.PG_Pickup_SilverShieldInChestAtTopOfHill, "9", "32896", true),
                new GenericItemsData("Energy Vial: Vine Patch Left - PG", Addresses.PG_Pickup_EnergyVialVinePatchLeft, "9", "32896"),
                new GenericItemsData("Energy Vial: Vine Patch Right - PG", Addresses.PG_Pickup_EnergyVialVinePatchRight, "9", "32896"),
                new GenericItemsData("Energy Vial: In Coop - PG", Addresses.PG_Pickup_EnergyVialInCoop, "9", "32896"),
                new GenericItemsData("Energy Vial: In Moon Hut - PG", Addresses.PG_Pickup_EnergyVialInMoonHut, "9", "32896"),
                new GenericItemsData("Energy Vial: Top of Hill - PG", Addresses.PG_Pickup_EnergyVialTopOfHill, "9", "32896"),
                new GenericItemsData("Energy Vial: Boulders After Star Rune - PG",  Addresses.PG_Pickup_EnergyVialBouldersAfterStarRune, "9", "32896"),
                new GenericItemsData("Energy Vial: Chalice Path - PG", Addresses.PG_Pickup_EnergyVialChalicePath, "9", "32896"),
                new GenericItemsData("Gold Coins: Bag Behind Rocks At Start - PG", Addresses.PG_Pickup_GoldCoinsBagBehindRocksAtStart, "9", "32896"),
                new GenericItemsData("Gold Coins: Chest in Coop 1 - PG", Addresses.PG_Pickup_GoldCoinsChestInCoop1, "9", "32896"),
                new GenericItemsData("Gold Coins: Chest in Coop 2 - PG", Addresses.PG_Pickup_GoldCoinsChestInCoop2, "9", "32896"),
                new GenericItemsData("Gold Coins: Chest in Coop 3 - PG", Addresses.PG_Pickup_GoldCoinsChestInCoop3, "9", "32896"),
                new GenericItemsData("Gold Coins: Bag in Mushroom Area - PG", Addresses.PG_Pickup_GoldCoinsBagInMushroomArea, "9", "32896"),
                new GenericItemsData("Gold Coins: Chest at Boulders after Star Rune - PG", Addresses.PG_Pickup_GoldCoinsChestAtBouldersAfterStarRune, "9", "32896"),
                new GenericItemsData("Gold Coins: Chest Near Chalice - PG", Addresses.PG_Pickup_GoldCoinsChestNearChalice,"9",  "32896"),
                new GenericItemsData("Gargoyle: Exit - PG", Addresses.PG_Gargoyle_Exit, "9", "0"),
                new GenericItemsData("Book: Mushrooms - PG", Addresses.PG_Book_Mushrooms, "9", "0"),
                new GenericItemsData("Cleared: Pumpkin Gorge", Addresses.PG_LevelStatus, "9", "16"),
                new GenericItemsData("Chalice: Pumpkin Gorge", Addresses.PG_Pickup_Chalice, "9", "32896"),
            };
            return pgLocations;
        }

        private static List<GenericItemsData> GetPumpkinSerpentData()
        {
            List<GenericItemsData> psLocations = new List<GenericItemsData>() {
                new GenericItemsData("Key Item: Dragon Gem - PS", Addresses.PS_Pickup_DragonsGem, "10", "32896"),
                new GenericItemsData("Equipment: Silver Shield in Chest near Leeches - PS", Addresses.PS_Pickup_SilverShieldInChestNearLeeches, "10", "32896", true),
                new GenericItemsData("Energy Vial: Left at Merchant Gargoyle - PS", Addresses.PS_Pickup_EnergyVialLeftAtMerchantGargoyle, "10", "32896"),
                new GenericItemsData("Energy Vial: Right at Merchant Gargoyle - PS", Addresses.PS_Pickup_EnergyVialRightAtMerchantGargoyle, "10", "32896"),
                new GenericItemsData("Gold Coins: Bag Behind House 1 - PS", Addresses.PS_Pickup_GoldCoinsBagBehindHouse1, "10", "32896"),
                new GenericItemsData("Gold Coins: Bag Behind House 2 - PS", Addresses.PS_Pickup_GoldCoinsBagBehindHouse2,"10",  "32896"),
                new GenericItemsData("Gold Coins: Bag Behind Vines and Pod - PS", Addresses.PS_Pickup_GoldCoinsBagBehindVinesAndPod, "10", "32896"),
                new GenericItemsData("Gold Coins: Chest at Merchant Gargoyle - PS", Addresses.PS_Pickup_GoldCoinsChestAtMerchantGargoyle, "10", "32896"),
                new GenericItemsData("Book: Pumpkin King - PS", Addresses.PS_Book_PumpkinKing, "10", "0"),
                new GenericItemsData("Book: Pumpkin Witch - PS", Addresses.PS_Book_PumpkinWitch, "10", "0"),
                new GenericItemsData("Cleared: Pumpkin Serpent", Addresses.PS_LevelStatus, "10", "16"),
                new GenericItemsData("Chalice: Pumpkin Serpent", Addresses.PS_Pickup_Chalice, "10", "32896"),
            };
            return psLocations;
        }

        private static List<GenericItemsData> GetTheHauntedRuinsData()
        {
            List<GenericItemsData> hrLocations = new List<GenericItemsData>() {
                new GenericItemsData("Key Item: King Peregrine's Crown - HR", Addresses.HR_Pickup_KingPeregrinsCrown, "17", "32896"),
                new GenericItemsData("Chaos Rune: The Haunted Ruins", Addresses.HR_Pickup_ChaosRune, "17", "32896"),
                new GenericItemsData("Earth Rune: The Haunted Ruins", Addresses.HR_Pickup_EarthRune, "17", "32896"),
                new GenericItemsData("Equipment: Silver Shield in Chest Near Rune Door - HR", Addresses.HR_Pickup_SilverShieldInChestNearRuneDoor, "17", "32896", true),
                new GenericItemsData("Energy Vial: Above Rune - HR", Addresses.HR_Pickup_EnergyVialAboveRune, "17", "32896"),
                new GenericItemsData("Energy Vial: Corner of Walls 1 - HR", Addresses.HR_Pickup_EnergyVialCornerOfWalls1, "17", "32896"),
                new GenericItemsData("Energy Vial: Corner of Walls 2 - HR", Addresses.HR_Pickup_EnergyVialCornerOfWalls2, "17", "32896"),
                new GenericItemsData("Energy Vial: Corner of Walls 3 - HR", Addresses.HR_Pickup_EnergyVialCornerOfWalls3, "17", "32896"),
                new GenericItemsData("Energy Vial: Up from Oil - HR", Addresses.HR_Pickup_EnergyVialUpFromOil, "17", "32896"),
                new GenericItemsData("Gold Coins: Near First Set of farmers - HR", Addresses.HR_Pickup_GoldCoinsNearFirstSetOfFarmers, "17", "32896"),
                new GenericItemsData("Gold Coins: Bag Near Chalice North - HR", Addresses.HR_Pickup_GoldCoinsBagNearChaliceNorth, "17", "32896"),
                new GenericItemsData("Gold Coins: Bag Near Chalice South - HR", Addresses.HR_Pickup_GoldCoinsBagNearChaliceSouth, "17", "32896"),
                new GenericItemsData("Gold Coins: Bag in Crown Room - HR", Addresses.HR_Pickup_GoldCoinsBagInCrownRoom, "17", "32896"),
                new GenericItemsData("Gold Coins: Chest at Catapult 1 - HR", Addresses.HR_Pickup_GoldCoinsChestAtCatapult1, "17", "32896"),
                new GenericItemsData("Gold Coins: Chest at Catapult 2 - HR", Addresses.HR_Pickup_GoldCoinsChestAtCatapult2, "17", "32896"),
                new GenericItemsData("Gold Coins: Chest at Catapult 3 - HR", Addresses.HR_Pickup_GoldCoinsChestAtCatapult3, "17", "32896"),
                new GenericItemsData("Book: Chickens - HR", Addresses.HR_Book_Chickens, "17", "0"),
                new GenericItemsData("Book: Farmers - HR", Addresses.HR_Book_Farmers, "17", "0"),
                new GenericItemsData("Book: Sad King - HR", Addresses.HR_Book_SadKing, "17", "0"),
                new GenericItemsData("Book: Ghost King - HR", Addresses.HR_Book_GhostKing, "17", "0"),
                new GenericItemsData("Book: The Volcano - HR", Addresses.HR_Book_TheVolcano, "17", "0"),
                new GenericItemsData("Book: Escape - HR", Addresses.HR_Book_Escape, "17", "0"),
                new GenericItemsData("Book: Oil - HR", Addresses.HR_Book_Oil, "17", "0"),
                new GenericItemsData("Gargoyle: Drawbridge - HR", Addresses.HR_Gargoyle_Drawbridge, "17", "0"),
                new GenericItemsData("Gargoyle: Steel Gates - HR", Addresses.HR_Gargoyle_SteelGates, "17", "0"),
                new GenericItemsData("Cleared: The Haunted Ruins", Addresses.HR_LevelStatus, "17", "16"),
                new GenericItemsData("Chalice: The Haunted Ruins", Addresses.HR_Pickup_Chalice, "17", "32896"),
            };
            return hrLocations;
        }

        private static List<GenericItemsData> GetTheGhostShipData()
        {
            List<GenericItemsData> gsLocations = new List<GenericItemsData>() {
                new GenericItemsData("Moon Rune: Ghost Ship", Addresses.GS_Pickup_MoonRune, "19", "32896"),
                new GenericItemsData("Chaos Rune: Ghost Ship", Addresses.GS_Pickup_ChaosRune, "19", "32896"),
                new GenericItemsData("Star Rune: Ghost Ship", Addresses.GS_Pickup_StarRune, "19", "32896"),
                new GenericItemsData("Equipment: Silver Shield in Chest in Barrel Room - GS", Addresses.GS_Pickup_SilverShieldInChestInBarrelRoom, "19", "32896", true),
                new GenericItemsData("Equipment: Club in Chest at Captain - GS", Addresses.GS_Pickup_ClubInChestAtCaptain, "19", "32896", true),
                new GenericItemsData("Energy Vial: In Cabin - GS", Addresses.GS_Pickup_EnergyVialInCabin, "19", "32896"),
                new GenericItemsData("Energy Vial: In Cannon Room - GS",Addresses.GS_Pickup_EnergyVialInCannonRoom, "19", "32896"),
                new GenericItemsData("Energy Vial: Rope Bridge 1 - GS", Addresses.GS_Pickup_EnergyVialRopeBridge1, "19", "32896"),
                new GenericItemsData("Energy Vial: Rope Bridge 2 - GS", Addresses.GS_Pickup_EnergyVialRopeBridge2, "19", "32896"),
                new GenericItemsData("Energy Vial: Cage Lift 1 - GS", Addresses.GS_Pickup_EnergyVialCageLift1, "19", "32896"),
                new GenericItemsData("Energy Vial: Cage Lift 2 - GS", Addresses.GS_Pickup_EnergyVialCageLift2, "19", "32896"),
                new GenericItemsData("Gold Coins: Bag in Rolling Barrels Room - GS", Addresses.GS_Pickup_GoldCoinsBagInRollingBarrelsRoom, "19", "32896"),
                new GenericItemsData("Gold Coins: Bag on Deck At Barrels - GS", Addresses.GS_Pickup_GoldCoinsBagOnDeckAtBarrels, "19", "32896"),
                new GenericItemsData("Gold Coins: Chest in Cannon Room - GS", Addresses.GS_Pickup_GoldCoinsChestInCannonRoom, "19", "32896"),
                new GenericItemsData("Gold Coins: Rope Bridge - GS", Addresses.GS_Pickup_GoldCoinsRopeBridge, "19", "32896"),
                new GenericItemsData("Book: Skeleton Warriors - GS", Addresses.GS_Book_SkeletonWarriors, "19", "0"),
                new GenericItemsData("Book: Boss Strategy - GS", Addresses.GS_Book_BossStrategy, "19", "0"),
                new GenericItemsData("Cleared: Ghost Ship", Addresses.GS_LevelStatus, "19", "16"),
                new GenericItemsData("Chalice: Ghost Ship", Addresses.GS_Pickup_Chalice, "19", "32896"),
            };
            return gsLocations;
        }

        private static List<GenericItemsData> GetTheEntranceHallData()
        {

            List<GenericItemsData> ehLocations = new List<GenericItemsData>() {
                new GenericItemsData("Book: Imp Magic - EH", Addresses.EH_Book_ImpMagic, "20", "0"),
                new GenericItemsData("Book: Spell Book - EH", Addresses.EH_Book_SpellBook, "20", "0"),
                new GenericItemsData("Book: Zaroks Diary - EH", Addresses.EH_Book_ZaroksDiary, "20", "0"),
                new GenericItemsData("Gargoyle: Entrance - EH", Addresses.EH_Gargoyle_Entrance, "20", "0"),
                new GenericItemsData("Cleared: The Entrance Hall", Addresses.EH_LevelStatus, "20", "16"),
                new GenericItemsData("Chalice: The Entrance Hall", Addresses.EH_Pickup_Chalice, "20", "32896"),
            };
            return ehLocations;
        }

        private static List<GenericItemsData> GetTheTimeDeviceData()
        {
            List<GenericItemsData> tdLocations = new List<GenericItemsData>() {
                new GenericItemsData("Life Bottle: The Time Device", Addresses.TD_Pickup_LifeBottle, "21", "32896"),
                new GenericItemsData("Chaos Rune: The Time Device", Addresses.TD_Pickup_ChaosRune, "21", "32896"),
                new GenericItemsData("Earth Rune: The Time Device", Addresses.TD_Pickup_EarthRune, "21", "32896"),
                new GenericItemsData("Moon Rune: The Time Device", Addresses.TD_Pickup_MoonRune, "21", "32896"),
                new GenericItemsData("Time Rune: The Time Device", Addresses.TD_Pickup_TimeRune, "21", "32896"),
                new GenericItemsData("Equipment: Silver Shield on Clock - TD", Addresses.TD_Pickup_SilverShieldOnClock, "21", "32896", true),
                new GenericItemsData("Gold Coins: Laser Platform Right - TD", Addresses.TD_Pickup_GoldCoinsLaserPlatformRight, "21", "32896"),
                new GenericItemsData("Gold Coins: Laser Platform Left - TD", Addresses.TD_Pickup_GoldCoinsLaserPlatformLeft, "21", "32896"),
                new GenericItemsData("Gold Coins: Lone Pillar 1 - TD", Addresses.TD_Pickup_GoldCoinsLonePillar1, "21", "32896"),
                new GenericItemsData("Gold Coins: Lone Pillar 2 - TD", Addresses.TD_Pickup_GoldCoinsLonePillar2, "21", "32896"),
                new GenericItemsData("Gold Coins: Lone Pillar 3 - TD", Addresses.TD_Pickup_GoldCoinsLonePillar3, "21", "32896"),
                new GenericItemsData("Gold Coins: Bag at Earth Station 1 - TD", Addresses.TD_Pickup_GoldCoinsBagAtEarthStation1, "21", "32896"),
                new GenericItemsData("Gold Coins: Bag at Earth Station 2 - TD", Addresses.TD_Pickup_GoldCoinsBagAtEarthStation2, "21", "32896"),
                new GenericItemsData("Gold Coins: Bag at Earth Station 3 - TD", Addresses.TD_Pickup_GoldCoinsBagAtEarthStation3, "21", "32896"),
                new GenericItemsData("Book: The Train - TD", Addresses.TD_Book_TheTrain, "21", "0"),
                new GenericItemsData("Gargoyle: Entrance - TD", Addresses.TD_Gargoyle_Entrance, "21", "0"),
                new GenericItemsData("Cleared: The Time Device", Addresses.TD_LevelStatus, "21", "16"),
                new GenericItemsData("Chalice: The Time Device", Addresses.TD_Pickup_Chalice, "21", "32896"),
            };
            return tdLocations;
        }

        private static List<GenericItemsData> GetZaroksLairData()
        {
            List<GenericItemsData> zlLocations = new List<GenericItemsData>() {
                new GenericItemsData("Equipment: Good Lightning - ZL", Addresses.ZL_Pickup_GoodLightning, "22", "32896", true),
                new GenericItemsData("Equipment: Silver Shield Arena - ZL", Addresses.ZL_Pickup_SilverShield,"22", "32896", true),
                new GenericItemsData("Cleared: Zaroks Lair", Addresses.WinConditionCredits, "22", "101"),
            };
            return zlLocations;
        }

        private static List<GenericItemsData> GetLockedItemsDC()
        {
            List<GenericItemsData> chLocations = new List<GenericItemsData>() {
                new GenericItemsData("Life Bottle: Dan's Crypt - Behind Wall",Addresses.DC_Pickup_LifeBottleWall,  "6", "32896"),
                new GenericItemsData("Gold Coins: Behind Wall in Crypt - Left - DC", Addresses.DC_Pickup_GoldCoinsBehindWallLeft,  "6", "32896"),
                new GenericItemsData("Gold Coins: Behind Wall in Crypt - Right - DC", Addresses.DC_Pickup_GoldCoinsBehindWallRight,  "6", "32896"),
            };
            return chLocations;
        }

        private static List<GenericItemsData> GetLockedItemsCH()
        {
            List<GenericItemsData> chLocations = new List<GenericItemsData>() {
                new GenericItemsData("Chalice: Cemetery Hill", Addresses.CH_Pickup_Chalice, "3", "32896"),
            };
            return chLocations;
        }

        private static List<GenericItemsData> GetLockedItemsHM()
        {
            List<GenericItemsData> hmLocations = new List<GenericItemsData>()
            {
                new GenericItemsData("Gold Coins: Gold Chest at Phantom of the Opera 1 - HM", Addresses.HM_Pickup_GoldChestPhantomOfTheOpera1, "4", "32896"),
                new GenericItemsData("Gold Coins: Gold Chest at Phantom of the Opera 2 - HM", Addresses.HM_Pickup_GoldChestPhantomOfTheOpera2, "4", "32896"),
                new GenericItemsData("Gold Coins: Gold Chest at Phantom of the Opera 3 - HM", Addresses.HM_Pickup_GoldChestPhantomOfTheOpera3, "4", "32896"),
                new GenericItemsData("Chalice: The Hilltop Mausoleum", Addresses.HM_Pickup_Chalice, "4", "32896"),
            };
            return hmLocations;
        }

        private static List<GenericItemsData> GetLockedItemsSF()
        {
            List<GenericItemsData> sfLocations = new List<GenericItemsData>()
            {
                new GenericItemsData("Life Bottle: Scarecrow Fields", Addresses.SF_Pickup_LifePotion, "5", "32896"),
                new GenericItemsData("Gold Coins: Chest Next to Chalice - SF", Addresses.SF_Pickup_GoldCoinsChestNextToChalice, "5", "32896"),
                new GenericItemsData("Chalice: Scarecrow Fields", Addresses.SF_Pickup_Chalice, "5", "32896"),
            };
            return sfLocations;
        }



    }
}