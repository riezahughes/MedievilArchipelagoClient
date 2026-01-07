using Archipelago.Core.Models;
using Archipelago.Core.Util;
using Archipelago.Core;
using Archipelago.MultiClient.Net.BounceFeatures.DeathLink;
using Kokuban;
using Newtonsoft.Json;
using Serilog;
using Archipelago.Core.Util.GPS;

namespace MedievilArchipelago.Helpers
{
    internal class APHandlers
    {

        public static async void OnConnected(object sender, EventArgs args, ArchipelagoClient client)
        {
            if (client.CurrentSession == null)
            {
                Console.WriteLine("Client is null on connect hook");
                return;
            }

            Log.Logger.Information("Connected to Archipelago");
            Log.Logger.Information($"Playing {client.CurrentSession.ConnectionInfo.Game} as {client.CurrentSession.Players.GetPlayerName(client.CurrentSession.ConnectionInfo.Slot)}");

            // if deathlink goes here
            int deathlink = Int32.Parse(client.Options?.GetValueOrDefault("deathlink", "0").ToString());


            DeathLinkService deathLinkClient = null;

            if (deathlink == 1)
            {
                #if DEBUG
                    Console.WriteLine("Deathlink is activated.");
                #endif
                deathLinkClient = client.EnableDeathLink();
                deathLinkClient.OnDeathLinkReceived += (args) => PlayerStateHandler.KillPlayer();
                PlayerStateHandler.StartDeathlinkMonitor(deathLinkClient, client);
            }

            Console.WriteLine("Setting up player state..");

            #if DEBUG
                Console.WriteLine($"OnConnected Firing. Itemcount: {client.ItemState.ReceivedItems.Count}");
#endif

            if (PlayerStateHandler.isInTheGame())
            {
                PlayerStateHandler.UpdatePlayerState(client, false);
            }


            // reset traps in case of client crashes

            byte[] DefaultWeaponIconX = BitConverter.GetBytes(0x0018);
            byte[] DefaultShieldIconX = BitConverter.GetBytes(0x0050);
            byte[] DefaultHealthbarX = BitConverter.GetBytes(0x0100);
            byte[] DefaultChaliceIconX = BitConverter.GetBytes(0x017e);
            byte[] DefaultMoneyIconX = BitConverter.GetBytes(0x01b6);
            byte[] DefaultWeaponIconY = BitConverter.GetBytes(0x001e);
            byte[] DefaultShieldIconY = BitConverter.GetBytes(0x001e);
            byte[] DefaultHealthbarY = BitConverter.GetBytes(0x0022);
            byte[] DefaultChaliceIconY = BitConverter.GetBytes(0x001e);
            byte[] DefaultMoneyIconY = BitConverter.GetBytes(0x001e);

            byte[] defaultSpeedValue = BitConverter.GetBytes(0x0100);
            byte[] defaultJumpValue = BitConverter.GetBytes(0x002f);

            byte[] defaultRenderDistance = BitConverter.GetBytes(0x1000);

            if (PlayerStateHandler.isInTheGame())
            {
                TrapHandlers.ResetTraps();
            }

        }



        public static async void OnDisconnected(object sender, ConnectionChangedEventArgs args, ArchipelagoClient client, bool firstRun)
        {
            if(firstRun == true)
            {
                return;
            }
            if (!client.IsLoggedIn || !client.IsConnected)
            {
                Console.WriteLine("Disconnected from Archipelago. Please restart client.");
            }
        }



        // logic for item receiving goes here (gold, health, ammo, etc)
        public static void ItemReceived(object sender, ItemReceivedEventArgs args, ArchipelagoClient client)
        {
            var isValidForDelay = args.Item.Name.ContainsAny("Ammo", "Charge", "Energy", "Trap:");

            if (client.CurrentSession != null)
            {
                if (!PlayerStateHandler.isInTheGame() && isValidForDelay)
                {
                    Program.delayedItems.Add(args);
                    return;
                }

                #if DEBUG
                    Console.WriteLine($"ItemReceived Firing. Itemcount: {client.CurrentSession.Items.AllItemsReceived.Count}");
                #endif
                byte currentLevel = Memory.ReadByte(Addresses.CurrentLevel);
                int runeSanityOption = Int32.Parse(client.Options?.GetValueOrDefault("runesanity", "0").ToString());
                int breakAmmoLimitOption = Int32.Parse(client.Options?.GetValueOrDefault("break_ammo_limit", "0").ToString());
                int breakChargeLimitOption = Int32.Parse(client.Options?.GetValueOrDefault("break_percentage_limit", "0").ToString());

                switch (args.Item)
                {
                    // incoming runes need added here
                    case var x when x.Name.ContainsAny("Rune") && runeSanityOption == 1: ItemHandlers.ReceiveRune(currentLevel, x); break;
                    case var x when x.Name.ContainsAny("Skill"): ItemHandlers.ReceiveSkill(x); break;
                    case var x when x.Name.ContainsAny("Equipment"): ItemHandlers.ReceiveEquipment(x); break;
                    case var x when x.Name.ContainsAny("Life Bottle"): ItemHandlers.ReceiveLifeBottle(); break;
                    case var x when x.Name.ContainsAny("Soul Helmet"): ItemHandlers.ReceiveSoulHelmet(); break;
                    case var x when x.Name.ContainsAny("Dragon Gem"): ItemHandlers.ReceiveDragonGem(); break;
                    case var x when x.Name.ContainsAny("Amber"): ItemHandlers.ReceiveAmber(); break;
                    case var x when x.Name.ContainsAny("Key Item"): ItemHandlers.ReceiveKeyItem(x); break;
                    case var x when x.Name.ContainsAny("Health", "Gold Coins", "Energy"): ItemHandlers.ReceiveStatItems(x); break;
                    case var x when x.Name.ContainsAny("Daggers", "Ammo", "Chicken Drumsticks", "Crossbow", "Longbow", "Fire Longbow", "Magic Longbow", "Spear", "Copper Shield", "Silver Shield", "Gold Shield"): ItemHandlers.ReceiveCountType(x, breakAmmoLimitOption); break;
                    case var x when x.Name.ContainsAny("Broadsword", "Club", "Lightning"): ItemHandlers.ReceiveChargeType(x, breakChargeLimitOption); break;
                    case var x when x.Name.Contains("Trap: Heavy Dan"): TrapHandlers.HeavyDanTrap(); break;
                    case var x when x.Name.Contains("Trap: Light Dan"): TrapHandlers.LightDanTrap(); break;
                    case var x when x.Name.Contains("Trap: Darkness"): TrapHandlers.DarknessTrap(currentLevel); break;
                    case var x when x.Name.Contains("Trap: Hudless"): TrapHandlers.HudlessTrap(); break;
                    case var x when x.Name.Contains("Trap: Lag"): TrapHandlers.RunLagTrap(); break;
                    case null: Console.WriteLine("Received an item with null data. Skipping."); break;
                    default: Console.WriteLine($"Item not recognised. ({args.Item.Name}) Skipping"); break;
                };

                PlayerStateHandler.UpdatePlayerState(client, false);
            }

        }

        public static void Client_MessageReceived(object sender, MessageReceivedEventArgs e, ArchipelagoClient client, string slot)
        {
            #if DEBUG
                Console.WriteLine($"MessageReceived Firing. Itemcount: {client.CurrentSession.Items.AllItemsReceived.Count}");
            #endif
            var message = string.Join("", e.Message.Parts.Select(p => p.Text));

            // this message can use emoji's through the overlay. Look into maybe making it a little more obvious 
            // what each item is with a symbol
            //client.AddOverlayMessage(e.Message.ToString());

            string prefix;
            Kokuban.AnsiEscape.AnsiStyle bg;
            Kokuban.AnsiEscape.AnsiStyle fg;

            if (message.Contains($"{slot} found") || message.Contains($"{slot} sent"))
            {
                bg = message.Contains("Trap:") ? Chalk.BgRed : message.Contains("Congratulations") ? Chalk.Yellow : Chalk.BgBlue;
                fg = Chalk.White;
                prefix = " >> ";
            }
            else
            {
                bg = message.Contains("Trap:") ? Chalk.BgRed : message.Contains("Congratulations") ? Chalk.Yellow : Chalk.BgGreen;
                fg = Chalk.White;
                prefix = " << ";
            }

            Console.WriteLine(bg + (fg + $"{prefix} {message} "));
        }

        // should be renamed "location triggered". As in "i will trigger every time a location is matched".
        // This could end up with a very long wait time if not careful.
        // added a guard so it doesn't fire prematurely
        public static void Client_LocationCompleted(object sender, LocationCompletedEventArgs e, ArchipelagoClient client)
        {
            if (client?.CurrentSession?.Items?.AllItemsReceived.Count == client?.ItemState.ReceivedItems.Count())
            {
                PlayerStateHandler.UpdatePlayerState(client, false);
                #if DEBUG
                    Console.WriteLine($"LocationCompleted Firing. {e.CompletedLocation.Name} - {e.CompletedLocation.Id} Itemcount: {client.CurrentSession.Items.AllItemsReceived.Count}");
                #endif
            }
        }


        public static void Locations_CheckedLocationsUpdated(System.Collections.ObjectModel.ReadOnlyCollection<long> newCheckedLocations)
        {
            #if DEBUG
                Console.WriteLine($"Location CheckedLocationsUpdated Firing.");
            #endif
        }

        public static GPSHandler Client_GPSHandler()
        {
            return new GPSHandler(() =>
            {
                byte currentLevel = Memory.ReadByte(Addresses.CurrentLevel);
                if (PlayerStateHandler.isInTheGame())
                {
                    return new PositionData
                    {
                        MapId = currentLevel,
                        MapName = LevelHandlers.GetLevelNameFromId(currentLevel),
                        X = Memory.ReadUShort(Addresses.DanRespawnPositionX),
                        Y = Memory.ReadUShort(Addresses.DanRespawnPositionY),
                        Z = Memory.ReadUShort(Addresses.DanRespawnPositionZ)
                    };
                }
                else
                {
                    return new PositionData
                    {
                        MapId = 0,
                        MapName = "No Map Detected",
                        X = 0,
                        Y = 0,
                        Z = 0,
                    };
                }
            });
        }

        public static void Client_GPSPositionChanged(ArchipelagoClient client, List<ILocation> gameLocations)
        {
            if (!PlayerStateHandler.isInTheGame()) return;

            LevelHandlers.CheckPositionalLocations(client, gameLocations);
        }
    }
}
