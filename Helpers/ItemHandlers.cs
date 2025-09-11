using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Archipelago.Core.Models;
using Archipelago.Core.Util;

namespace MedievilArchipelago.Helpers
{
    internal class ItemHandlers
    {
        
        internal const int percentageMax = 20480;
        internal const int countMax = 32767;
        internal const int maxHealth = 300;

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

        public static Dictionary<string, int> AmmoAndChargeLimits()
        {
            return new Dictionary<string, int>
            {
                ["Dagger"] = 250,
                ["Broadsword"] = 4096,
                ["Club"] = 4096,
                ["Chicken Drumsticks"] = 30,
                ["Crossbow"] = 200,
                ["Longbow"] = 200,
                ["Fire Longbow"] = 100,
                ["Magic Longbow"] = 50,
                ["Spear"] = 30,
                ["Copper Shield"] = 150,
                ["Silver Shield"] = 250,
                ["Gold Shield"] = 400,
                ["Lightning"] = 4096
            };
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
                ["AmmoMaximum"] = new Dictionary<string, uint>
                {

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



        public static int SetItemMemoryValue(uint itemMemoryAddress, int itemUpdateValue, int maxCount)
        {
            int addition = Math.Min(itemUpdateValue, maxCount);

            byte[] byteArray = BitConverter.GetBytes(addition);

            Memory.WriteByteArray(itemMemoryAddress, byteArray);

            // Add more types as needed
            return itemUpdateValue;
        }

        // Update functions with correct logic

        public static void UpdateCurrentItemValue(string itemName, int numberUpdate, uint itemMemoryAddress, bool isCountType, bool isEquipmentType, int breakLimit = 0)
        {
            var currentNumberAmount = Memory.ReadShort(itemMemoryAddress);

            var chargeConversion = numberUpdate == 50 ? 2048 : numberUpdate == 20 ? 819 : 0;

            if (currentNumberAmount == -1 && itemName.ContainsAny("Ammo", "Charge"))
            {
                return;
            }


            // life bottles have 1 in the chamber before showing
            if (itemName == "Life Bottle" && currentNumberAmount == 0)
            {
                currentNumberAmount = 1;
            }


            // leaving for the sake of debugging
            //Console.WriteLine($"{ itemName} current amount: {currentNumberAmount}, update amount: {numberUpdate}");

            // there needs to be some logic here. It has to go something like:
            // if ammo and you don't have the equipment, then don't trigger, but save the ammo for when you have it.
            // There's also an interesting issue with lifebottles where when you get your first one, it's automatically equipping a sword.
            // I think i need to look into some sort of "global state" for the player using archipelago's slot data. Though i'm not sure
            // how much of this is "too much" as it says in the docs to use it sparingly. Loading Dan's state should be fine though. Hopefully.
            // just need to make sure to not trigger the state until you are actually loaded into a level.

            int maxValue = 0;

            if (itemName.ContainsAny("Health", "Energy"))
            {
                maxValue = maxHealth;
            }
            else if (breakLimit == 0 && itemName.ContainsAny("Ammo", "Charge"))
            {
                var dict = AmmoAndChargeLimits();
                string mainWeapon = itemName.Replace(" Ammo", "");
                mainWeapon = mainWeapon.Replace(" Charge", "");
                maxValue = dict[mainWeapon];
            }
            else
            {
                maxValue = isCountType ? countMax : percentageMax; // Max count limit for gold, percentage for energy

            }


            var newNumberAmount = isCountType ? Math.Min(currentNumberAmount + numberUpdate, maxValue) : Math.Min(currentNumberAmount + chargeConversion, maxValue); // Max count limit

            var baseValue = isEquipmentType ? 0 : newNumberAmount;

            //Console.WriteLine($"Name: {itemName}, current: {currentNumberAmount}, adding: {numberUpdate}");

            SetItemMemoryValue(itemMemoryAddress, baseValue, countMax);



            if (itemName == "Life Bottle")
            {
                SetItemMemoryValue(Addresses.LifeBottleSwitch, (300 * newNumberAmount - 1), 10000);
            }

            // if you're getting a piece of equipment like the longbow/crossbow/spear/etc give it some ammo.
            if (isEquipmentType && isCountType)
            {
                var ammoToAdd = currentNumberAmount == -1 ? 100 : newNumberAmount;
                //Console.WriteLine($"Name: {itemName}, Ammo count: {ammoToAdd}, Max: {maxValue}");
                SetItemMemoryValue(itemMemoryAddress, ammoToAdd, maxValue);
            }
            else if (isEquipmentType && !isCountType)
            {
                var ammoToAdd = currentNumberAmount == -1 ? 4096 : newNumberAmount;
                //Console.WriteLine($"Name: {itemName}, Charge count: {ammoToAdd}, Max: {maxValue}");
                SetItemMemoryValue(itemMemoryAddress, ammoToAdd, maxValue);
            }

        }

        public static int ExtractBracketAmount(string itemName)
        {
            var match = Regex.Match(itemName, @"\((\d+)\)");
            if (match.Success && int.TryParse(match.Groups[1].Value, out int bracketAmount))
            {
                return bracketAmount;
            }
            return 0;
        }

        public static string ExtractDictName(string itemName)
        {

            var colonMatch = Regex.Match(itemName, @"^[^:]*:\s*(.*)$");
            if (colonMatch.Success)
            {
                return colonMatch.Groups[1].Value.Trim();
            }

            var parenthesesMatch = Regex.Match(itemName, @"^(.*?)(?:\s*\(.*?\))?$");
            if (parenthesesMatch.Success)
            {
                return parenthesesMatch.Groups[1].Value.Trim();
            }
            return "N/A";
        }

        public static string ExtractRuneName(string itemName)
        {
            // The regex pattern to match and capture the text before the colon
            string pattern = @"^(.+?):";

            // Find the first match in the input string
            Match match = Regex.Match(itemName, pattern);

            // Check if a match was found
            if (match.Success)
            {
                // The captured group is at index 1. Trim to remove any trailing spaces.
                return match.Groups[1].Value.Trim();
            }

            return null;
        }
        public static string ExtractRuneLevel(string itemName)
        {
            // The regex pattern to match and capture the text after the colon and space
            string pattern = @":\s(.+)";

            // Find the first match in the input string
            Match match = Regex.Match(itemName, pattern);

            // Check if a match was found
            if (match.Success)
            {
                // The captured group is at index 1
                return match.Groups[1].Value;
            }

            // Return null or an empty string if no match is found
            return null;
        }

        public static string ExtractKeyItemName(string itemName)
        {
            var dashRemovedMatch = Regex.Match(itemName, @"^(?:Key Item:\s*)?([^-]+?)(?: - .*)?$");

            string cleanedName = itemName;

            if (dashRemovedMatch.Success)
            {
                cleanedName = dashRemovedMatch.Groups[1].Value.Trim();
            }
            else
            {
                var keyItemPrefixMatch = Regex.Match(itemName, @"^Key Item:\s*(.*)$");
                if (keyItemPrefixMatch.Success)
                {
                    cleanedName = keyItemPrefixMatch.Groups[1].Value.Trim();
                }
                else
                {

                    cleanedName = itemName.Trim();
                }
            }


            cleanedName = Regex.Replace(cleanedName, @"\s*\d+$", "").Trim();

            return cleanedName;
        }

        public static void ReceiveCountType(Item item, int breakAmmoLimit)
        {
            var addressDict = StatusAndInventoryAddressDictionary();
            var amount = ExtractBracketAmount(item.Name);
            var name = ExtractDictName(item.Name);

            UpdateCurrentItemValue(name, amount, addressDict["Ammo"][name], true, true, breakAmmoLimit);
        }

        public static void ReceiveChargeType(Item item, int breakChargeLimit)
        {
            var addressDict = StatusAndInventoryAddressDictionary();
            var amount = ExtractBracketAmount(item.Name);
            var name = ExtractDictName(item.Name);

            UpdateCurrentItemValue(name, amount, addressDict["Ammo"][name], false, true, breakChargeLimit);
        }

        public static void ReceiveEquipment(Item item)
        {
            var addressDict = StatusAndInventoryAddressDictionary();
            var name = ExtractDictName(item.Name);

            var isChargeType = item.Name.ContainsAny("Broadsword", "Club", "Lightning");

            UpdateCurrentItemValue(item.Name, 0, addressDict["Equipment"][name], !isChargeType, true);

        }


        public static void ReceiveLevelCleared(Item level)
        {
            var addressDict = StatusAndInventoryAddressDictionary();
            var name = ExtractDictName(level.Name);

            UpdateCurrentItemValue(level.Name, 16, addressDict["Level Status"][name], true, true);
        }

        public static void ReceiveKeyItem(Item item)
        {
            // commented out because i need to make a list of player data addresses to deal with this.
            var addressDict = StatusAndInventoryAddressDictionary();
            var name = ExtractKeyItemName(item.Name);

            UpdateCurrentItemValue(item.Name, 0, addressDict["Key Items"][name], true, true);

        }

        public static void ReceiveLifeBottle()
        {
            var addressDict = StatusAndInventoryAddressDictionary();

            UpdateCurrentItemValue("Life Bottle", 1, addressDict["Player Stats"]["Life Bottle"], true, false);
        }

        public static void ReceiveSoulHelmet()
        {
            var addressDict = StatusAndInventoryAddressDictionary();
            UpdateCurrentItemValue("Soul Helmet", 1, addressDict["Key Items"]["Soul Helmet"], true, false);
        }

        public static void ReceiveDragonGem()
        {
            var addressDict = StatusAndInventoryAddressDictionary();
            UpdateCurrentItemValue("Dragon Gem", 1, addressDict["Key Items"]["Dragon Gem"], true, false);
        }

        public static void ReceiveAmber()
        {
            UpdateCurrentItemValue("Amber Piece", 1, Addresses.APAmberPieces, true, false);
        }

        public static void ReceiveTalismanAndArtefact()
        {
            var addressDict = StatusAndInventoryAddressDictionary();

            UpdateCurrentItemValue("Shadow Artefact", 0, addressDict["Key Items"]["Shadow Artefact"], true, true);
            UpdateCurrentItemValue("Shadow Talisman", 0, addressDict["Key Items"]["Shadow Talisman"], true, true);
        }

        public static void ReceiveStatItems(Item item)
        {
            var addressDict = StatusAndInventoryAddressDictionary();
            var amount = ExtractBracketAmount(item.Name);
            var name = ExtractDictName(item.Name);

            UpdateCurrentItemValue(item.Name, amount, addressDict["Player Stats"][name], true, false);
        }

        public static void ReceiveRune(byte levelId, Item item)
        {

            var addressDict = StatusAndInventoryAddressDictionary();

            var levelName = LevelHandlers.GetLevelNameFromId(levelId);

            var name = ExtractRuneName(item.Name);
            var runeLevel = ExtractRuneLevel(item.Name);

            if (levelName == runeLevel)
            {
                SetItemMemoryValue(addressDict["Runes"][name], 1, 1);
            }
        }

        public static void ReceiveSkill(Item item)
        {
            // setting it here till i fix my ridiculous update function
            SetItemMemoryValue(Addresses.DaringDashSkill, 1, 1);
        }

        public static void EquipWeapon(int value)
        {
            SetItemMemoryValue(Addresses.ItemEquipped, value, value);
        }

        public static void DefaultToArm()
        {
            SetItemMemoryValue(Addresses.ItemEquipped, 8, 8);
            SetItemMemoryValue(Addresses.SmallSword, 65535, 65535);
        }
    }
}
