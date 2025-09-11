using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Archipelago.Core.Models;
using Archipelago.Core.Util;
using Archipelago.Core;
using Archipelago.MultiClient.Net.Models;
using Archipelago.MultiClient.Net.BounceFeatures.DeathLink;
using Kokuban;
using System.Threading;

namespace MedievilArchipelago.Helpers
{
    internal class PlayerStateHandler
    {
        internal static DateTime lastDeathTime = default(DateTime);
        internal static Task _deathlinkMonitorTask = null;
        internal static bool gameCleared = false;
        internal static bool playerStateUpdating = false;

        public static bool isInTheGame()
        {
            ulong currentGameStatus = Memory.ReadUInt(Addresses.InGameCheck);
            ulong currentGold = Memory.ReadUInt(Addresses.CurrentGold);
            ulong currentMapPosition = Memory.ReadUInt(Addresses.CurrentMapPosition);


            if (currentGameStatus != 0x800f8198 || currentGold == 0x82a4 || currentMapPosition > 0x32)
            {
                return false;
            }
            return true;

        }

        public static void KillPlayer()
        {
            Memory.WriteByte(Addresses.GameGlobalScene, 0x06);
        }

        public static void StartDeathlinkMonitor(DeathLinkService deathLink, ArchipelagoClient client)
        {

            CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();
            CancellationToken cancellationToken = _cancellationTokenSource.Token;

            _deathlinkMonitorTask = Task.Run(async () =>
            {
                // This is a continuous loop that runs until the task is canceled.
                while (!cancellationToken.IsCancellationRequested)
                {
                    // Read the memory address.
                    int currentValue = Memory.ReadUShort(Addresses.CurrentEnergy);

                    // Check your condition.
                    if (currentValue == 0)
                    {
                        // Execute the action.
                        IsTrulyDead(deathLink, client);
                    }

                    // Await a short delay to prevent high CPU usage.
                    await Task.Delay(100, cancellationToken);
                }
            }, cancellationToken);
        }

        public static void IsTrulyDead(DeathLinkService deathlink, ArchipelagoClient client)
        {
            var rnd = new Random();

            List<string> deathResponse = new List<string>
            {
                "Everyone disliked that.",
                "We're in danger!",
                "You hate to see it.",
                "Press F to pay respects.",
                "This is fine.",
                "Dan's dissapointment: Immeasurable.",
                "We're going down swimming.",
                "Lock, Stock and... we're all dead."
            };

            int listChoice = rnd.Next(deathResponse.Count);


            if (DateTime.Now - lastDeathTime >= TimeSpan.FromSeconds(30))
            {
                ushort bottleEnergy = Memory.ReadUShort(Addresses.CurrentStoredEnergy);
                ushort currentLevel = Memory.ReadByte(Addresses.CurrentLevel);

                Kokuban.AnsiEscape.AnsiStyle bg = Chalk.BgCyan;
                Kokuban.AnsiEscape.AnsiStyle fg = Chalk.Black;

                if (bottleEnergy == 0 && currentLevel != 0 && isInTheGame())
                {
                    Console.WriteLine(bg + (fg + "[   ☠️💀 Deathlink Sent. " + deathResponse[listChoice] + " 💀☠️   ]"));
                    deathlink.SendDeathLink(new DeathLink(client.CurrentSession.Players.ActivePlayer.Name));
                    lastDeathTime = DateTime.Now;
                }
            }
        }

        public static void UpdatePlayerState(ArchipelagoClient client, bool gameCleared)
        {
            if (playerStateUpdating == true) { return; }

            playerStateUpdating = true;

            // get a list of all locatoins
            Dictionary<string, uint> all_items = ItemHandlers.FlattenedInventoryStrings();


            System.Collections.ObjectModel.ReadOnlyCollection<ItemInfo> itemsCollected = client.CurrentSession.Items.AllItemsReceived;
            // get a list of used locations
            var usedItems = new List<string>();

            short currentWeapon = Memory.ReadShort(Addresses.ItemEquipped);
            byte currentLevel = Memory.ReadByte(Addresses.CurrentLevel);
            int runeSanityOption = Int32.Parse(client.Options?.GetValueOrDefault("runesanity", "0").ToString());
            //int breakAmmoLimitOption = Int32.Parse(archipelagoClient.Options?.GetValueOrDefault("break_ammo_limit", "0").ToString());
            ////int breakChargeLimitOption = Int32.Parse(archipelagoClient.Options?.GetValueOrDefault("break_percentage_limit", "0").ToString());

            //Console.WriteLine(breakChargeLimitOption);
            //Console.WriteLine(breakAmmoLimitOption);

            ItemHandlers.SetItemMemoryValue(Addresses.CurrentLifePotions, 0, 0);
            ItemHandlers.SetItemMemoryValue(Addresses.SoulHelmet, 0, 0);
            ItemHandlers.SetItemMemoryValue(Addresses.DragonGem, 0, 0);
            ItemHandlers.SetItemMemoryValue(Addresses.APAmberPieces, 0, 0);
            ItemHandlers.SetItemMemoryValue(Addresses.MaxAmberPieces, 10, 10);

            if (runeSanityOption == 1)
            {
                ItemHandlers.SetItemMemoryValue(Addresses.ChaosRune, 65535, 65535);
                ItemHandlers.SetItemMemoryValue(Addresses.EarthRune, 65535, 65535);
                ItemHandlers.SetItemMemoryValue(Addresses.MoonRune, 65535, 65535);
                ItemHandlers.SetItemMemoryValue(Addresses.StarRune, 65535, 65535);
                ItemHandlers.SetItemMemoryValue(Addresses.TimeRune, 65535, 65535);
            }

            // for each location that's coming in
            bool hasEquipableWeapon = false;
            int talismanCount = 0;
            bool hasTalisman = false;

            if (GoalConditionHandlers.CheckGoalCondition(client) && gameCleared == false)
            {
                gameCleared = true;
                Console.WriteLine("No need for player state update. You've cleared!");
                return;
            };

            foreach (ItemInfo itemInf in itemsCollected)
            {
                Item itm = new Item();
                itm.Name = itemInf.ItemName;

                if (itm.Name.ContainsAny("Shadow"))
                {
                    talismanCount++;
                }

                switch (itm)
                {
                    // Update memory
                    case var x when x.Name.ContainsAny("Ammo"):
                        // no plans yet
                        break;
                    case var x when x.Name.Contains("Rune") && runeSanityOption == 1:
                        ItemHandlers.ReceiveRune(currentLevel, x);
                        break;
                    case var x when x.Name.ContainsAny("Charge"):
                        // no plans yet
                        break;
                    case var x when x.Name.Contains("Skill"): ItemHandlers.ReceiveSkill(x); break;
                    case var x when x.Name.Contains("Equipment"):
                        ItemHandlers.ReceiveEquipment(x);
                        if (!x.Name.Contains("Shield"))
                        {
                            hasEquipableWeapon = true;
                        }
                        break;
                    case var x when x.Name.Contains("Life Bottle"): ItemHandlers.ReceiveLifeBottle(); break;
                    case var x when x.Name.Contains("Soul Helmet"):
                        ItemHandlers.ReceiveSoulHelmet();
                        break;
                    case var x when x.Name.Contains("Dragon Gem"): ItemHandlers.ReceiveDragonGem(); break;
                    case var x when x.Name.Contains("Amber"): ItemHandlers.ReceiveAmber(); break;
                    case var x when x.Name.Contains("Key Item"): ItemHandlers.ReceiveKeyItem(x); break;
                    case var x when x.Name.Contains("Cleared"): ItemHandlers.ReceiveLevelCleared(x); break;
                }


                if (talismanCount == 2 && !hasTalisman)
                {
                    ItemHandlers.ReceiveTalismanAndArtefact();
                    hasTalisman = true;
                }

                usedItems.Add(itm.Name);


            }

            Dictionary<string, uint> remainingItemsDict = all_items
                .Where(kvp => !usedItems.Contains(kvp.Key))
                .ToDictionary(kvp => kvp.Key, kvp => kvp.Value);

            foreach (KeyValuePair<string, uint> item in remainingItemsDict)
            {

                string itemName = item.Key;
                uint itemAddress = item.Value;

                if (itemName.ContainsAny("Soul Helmet", "Dragon Gem", "Amber"))
                {
                    continue;
                }

                if (itemName.ContainsAny("Shadow Artefact", "Shadow Talisman"))
                {
                    ItemHandlers.SetItemMemoryValue(Addresses.ShadowArtefact, 65535, 65535);
                    ItemHandlers.SetItemMemoryValue(Addresses.ShadowTalisman, 65535, 65535);
                }

                // reset any other values
                if (itemName.ContainsAny("Skill"))
                {
                    ItemHandlers.SetItemMemoryValue(itemAddress, 0, 0);
                    continue;
                }
                else if (itemName.ContainsAny("Equipment"))
                {
                    ItemHandlers.SetItemMemoryValue(itemAddress, 65535, 65535); // Assuming 65535 is "reset/max" for equipment
                    continue;
                }
                else if (itemName.ContainsAny("Complete"))
                {
                    ItemHandlers.SetItemMemoryValue(itemAddress, 0, 0);
                    continue;

                }
                else if (itemName.ContainsAny("Key Item"))
                {
                    ItemHandlers.SetItemMemoryValue(itemAddress, 65535, 65535);
                    continue;

                }

            }


            if (!hasEquipableWeapon)
            {
                ItemHandlers.DefaultToArm();
            }
            else
            {
                ItemHandlers.EquipWeapon(currentWeapon);
            }
            playerStateUpdating = false;
        }
    }
}
