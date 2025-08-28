// See https://aka.ms/new-console-template for more information

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Archipelago.Core;
using Archipelago.Core.GameClients;
using Archipelago.Core.Models;
using Archipelago.Core.Traps;
using Archipelago.Core.Util;
using Archipelago.Core.Util.GPS;
using MedievilArchipelago;
using Serilog;
using Helpers = MedievilArchipelago.Helpers;
using System.Threading;
using Archipelago.Core.Util.Overlay;
using Newtonsoft.Json;
using MedievilArchipelago.Models;
using System.Xml.Linq;
using System.Diagnostics;
using System.Drawing;
using System.Collections;
using System.Reflection;
using System.Drawing.Text;
using Archipelago.MultiClient.Net.Models;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using Kokuban;
using System.Net;
using System.ComponentModel;

internal class Program
{
    private static async Task Main(string[] args)
    {
        // set values
        const byte US_OFFSET = 0x38; // this is ADDED to addresses to get their US location
        const byte JP_OFFSET = 0; // could add more offsfets here
        const int percentageMax = 4096;
        const int countMax = 32767;
        const int maxHealth = 300;

        bool firstRun = true;
        bool gameCleared = false;

        // Connection details
        string url;
        string port;
        string slot;
        string password;


        List<ILocation> GameLocations = null;

        ////////////////////////////
        //
        // Main Program Flow
        //
        ////////////////////////////

        // Make sure the connect is initialised


        DuckstationClient gameClient = null;
        bool clientInitializedAndConnected = false; // Renamed for clarity
        int retryAttempt = 0;

        while (!clientInitializedAndConnected)
        {
            Console.Clear();
            retryAttempt++;
            Console.WriteLine($"\nAttempt #{retryAttempt}:");

            try
            {
                gameClient = new DuckstationClient();
                clientInitializedAndConnected = true;
            }
            catch (Exception ex)
            {
                // Catch any exception thrown during the DuckstationClient constructor call
                // or any other unexpected error during the try block.
                Console.WriteLine($"Could not find Duckstation open.");

                // Wait for 5 seconds before the next retry
                Thread.Sleep(5000); // 5000 milliseconds = 5 seconds
            }
        }

#if DEBUG
#else
    Console.Clear();
#endif

        bool connected = gameClient.Connect();
        var archipelagoClient = new ArchipelagoClient(gameClient);

        archipelagoClient.CancelMonitors();
        archipelagoClient.Connected -= (sender, args) => OnConnected(sender, args, archipelagoClient);
        archipelagoClient.Disconnected -= (sender, args) => OnDisconnected(sender, args, archipelagoClient, firstRun);
        archipelagoClient.ItemReceived -= (sender, args) => ItemReceived(sender, args, archipelagoClient);
        archipelagoClient.LocationCompleted -= (sender, args) => Client_LocationCompleted(sender, args, archipelagoClient);

        Console.WriteLine("Successfully connected to Duckstation.");

        // get the duckstation offset
        try
        {
            Memory.GlobalOffset = Memory.GetDuckstationOffset();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An unexpected error occurred while getting Duckstation memory offset: {ex.Message}");
            Console.WriteLine(ex); // Print full exception for debugging
        }


        // wait until you can read the euro/US

        // doesn't work. got wrong memory address. I tried.
        //while(!hasFoundMem)
        //{
        //    Console.Clear();
        //    short region = Memory.ReadShort(0x001c3540);

        //    if (region == 817)
        //    {
        //        Console.WriteLine("PAL Copy of the game found");
        //        hasFoundMem = true;
        //    }
        //    if (region == 31370)
        //    {
        //        //Memory.GlobalOffset. = Memory.GlobalOffset - US_OFFSET; // this line isn't working. Once i can get the offset working it should go with the US version of the game. *should*..
        //        Console.WriteLine("US Copy of the game found. Please note that i've not done any tests with the US version of the game. This progam has been built to work with the PAL one. Use so at your own discretion.");
        //        hasFoundMem = true;
        //    } else
        //    {
        //        Console.WriteLine("Looking for game version...");
        //        await Task.Delay(5000);
        //    }

        //};

#if DEBUG

        var configuration = new ConfigurationBuilder()
            // Add the default appsettings.json file
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.Local.json", optional: true, reloadOnChange: true)
            .Build();

        Console.WriteLine("Logging in using settings in appsettings.Local.json");
        Console.WriteLine(configuration["port"]);
        Console.WriteLine(configuration["slot"]);
        Console.WriteLine(configuration["pass"]);
        url = "wss://archipelago.gg";
        port = configuration["port"];
        slot = configuration["slot"];
        password = configuration["pass"];
#else
// start AP Login

Console.WriteLine("Enter AP url: eg,archipelago.gg");
string lineUrl = Console.ReadLine();

url = string.IsNullOrWhiteSpace(lineUrl) ? "archipelago.gg" : lineUrl;

Console.WriteLine("Enter Port: eg, 80001");
port = Console.ReadLine();

Console.WriteLine("Enter Slot Name:");
slot = Console.ReadLine();

Console.WriteLine("Room Password:");
string linePassword = Console.ReadLine();
password = string.IsNullOrWhiteSpace(linePassword) ? null : linePassword;

Console.WriteLine("Details:");
Console.WriteLine($"URL:{url}:{port}");
Console.WriteLine($"Slot: {slot}");
Console.WriteLine($"Password: {password}");

if (string.IsNullOrWhiteSpace(slot))
{
    Console.WriteLine("Slot name cannot be empty. Please provide a valid slot name.");
    return;
}
#endif
        Console.WriteLine("Got the details! Attempting to connect to Archipelagos main server");

        // Register event handlers
        archipelagoClient.Connected += (sender, args) => OnConnected(sender, args, archipelagoClient);
        archipelagoClient.Disconnected += (sender, args) => OnDisconnected(sender, args, archipelagoClient, firstRun);
        archipelagoClient.ItemReceived += (sender, args) => ItemReceived(sender, args, archipelagoClient);
        archipelagoClient.MessageReceived += (sender, args) => Client_MessageReceived(sender, args, archipelagoClient);
        archipelagoClient.LocationCompleted += (sender, args) => Client_LocationCompleted(sender, args, archipelagoClient);
        archipelagoClient.EnableLocationsCondition = () => isInTheGame();
        archipelagoClient.GPSHandler = new GPSHandler(() =>
        {

            byte currentLevel = Memory.ReadByte(Addresses.CurrentLevel);

#if DEBUG
            Console.WriteLine("Sending GPS");
            Console.WriteLine(currentLevel);
#endif

            return new PositionData
            {
                MapId = currentLevel,
                MapName = Helpers.GetLevelNameFromId(currentLevel),
                X = currentLevel,
                Y = currentLevel,
                Z = 0
            };
        });

        archipelagoClient.GPSHandler.PositionChanged += (sender, args) =>
        {
            Console.WriteLine($"Current map changed");
        };

        var cts = new CancellationTokenSource();
        try
        {
            await archipelagoClient.Connect(url + ":" + port, "Medievil");
            Console.WriteLine("Connected. Attempting to Log in...");
            await archipelagoClient?.Login(slot, password);
            Console.WriteLine("Logged in!");

            var overlayOptions = new OverlayOptions();

            overlayOptions.XOffset = 50;
            overlayOptions.YOffset = 500;
            overlayOptions.FontSize = 12;
            overlayOptions.TextColor = Archipelago.Core.Util.Overlay.Color.Yellow;

            var gameOverlay = new WindowsOverlayService(overlayOptions);

            //var fontPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Assets", "MediEvilFont.ttf");
            //Console.WriteLine(fontPath)
            //gameOverlay.CreateFont(fontPath, 12);

            archipelagoClient.IntializeOverlayService(gameOverlay);

            while (archipelagoClient.CurrentSession == null)
            {
                Console.WriteLine("Waiting for current session");
                Thread.Sleep(1000);
            }

            archipelagoClient.ShouldSaveStateOnItemReceived = false;
            archipelagoClient.CurrentSession.Locations.CheckedLocationsUpdated += Locations_CheckedLocationsUpdated;



            GameLocations = Helpers.BuildLocationList(archipelagoClient.Options);


#if DEBUG
            //foreach (var opt in archipelagoClient.Options)
            //{
            //    Console.WriteLine($"Option: {opt.Key} - {opt.Value}");
            //}

#else
    Console.Clear();
#endif

            //foreach (var location in GameLocations)
            //{
            //    Console.WriteLine($"ID: {location.Id} - {location.Name}");
            //}

            _ = MemoryCheckThreads.PassiveLogicChecks(archipelagoClient, cts.Token);
            _ = archipelagoClient.MonitorLocations(GameLocations);



            firstRun = false;

            while (!cts.Token.IsCancellationRequested)
            {
                var input = Console.ReadLine();
                if (input?.Trim().ToLower() == "exit")
                {
                    cts.Cancel();
                    break;
                }
                else if (input?.Trim().ToLower().Contains("hint") == true)
                {

                    string hintString = input?.Trim().ToLower() == "hint" ? "!hint" : $"!hint {input.Substring(5).Trim()}";
                    archipelagoClient.SendMessage(hintString);
                }
                else if (input?.Trim().ToLower() == "update")
                {
                    if (archipelagoClient.GameState.CompletedLocations != null)
                    {
                        UpdatePlayerState(archipelagoClient.CurrentSession.Items.AllItemsReceived);
                        Console.WriteLine($"Player state updated. Total Count: {archipelagoClient.CurrentSession.Items.AllItemsReceived.Count}");

#if DEBUG
                        foreach (ItemInfo item in archipelagoClient.CurrentSession.Items.AllItemsReceived)
                        {
                            Console.WriteLine($"id: {item.ItemId} - {item.ItemName}");
                        }
#endif
                    }
                    else
                    {
                        Console.WriteLine("Cannot update player state: GameState or CompletedLocations is null.");
                    }
                }
                else if (input?.Trim().ToLower() == "items")
                {
                    var items = from item in archipelagoClient.CurrentSession.Items.AllItemsReceived where item.ItemName.Contains("Key Item") select item;

                    var bottles = from item in archipelagoClient.CurrentSession.Items.AllItemsReceived where item.ItemName.Contains("Bottle") select item;

                    var equipment = from item in archipelagoClient.CurrentSession.Items.AllItemsReceived where item.ItemName.Contains("Equipment") select item;

                    Console.WriteLine("Current Equipment: ");
                    foreach (var weapon in equipment.OrderBy(item => item.ItemName))
                    {
                        Console.WriteLine(weapon.ItemName);
                    }

                    Console.WriteLine("Current Key Items: ");
                    foreach (var item in items.OrderBy(item => item.ItemName))
                    {
                        Console.WriteLine(item.ItemName);
                    }

                    Console.WriteLine("Current Bottles: ");
                    foreach (var bottle in bottles.OrderBy(item => item.ItemName))
                    {
                        Console.WriteLine(bottle.ItemName);
                    }


                }
#if DEBUG
                else if (input?.Trim().ToLower() == "heavytrap")
                {
                    HeavyDanTrap();
                }
                else if (input?.Trim().ToLower() == "lighttrap")
                {
                    LightDanTrap();
                }
                else if (input?.Trim().ToLower() == "shieldtrap")
                {
                    GoodbyeShieldTrap();
                }
                else if (input?.Trim().ToLower() == "hudtrap")
                {
                    HudlessTrap();
                }
#endif
                else if (!string.IsNullOrWhiteSpace(input))
                {
                    Console.WriteLine($"Unknown command: '{input}'");
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred while connecting to Archipelago: {ex.Message}");
            Console.WriteLine(ex); // Print full exception for debugging
        }
        finally
        {
            // Perform any necessary cleanup here
            Console.WriteLine("Shutting down...");

        }

        bool isInTheGame()
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

        ////////////////////////////////////
        //
        // AP CORE HOOKS
        //
        ////////////////////////////////////


        async void OnConnected(object sender, EventArgs args, ArchipelagoClient client)
        {
            if (client.CurrentSession != null) {
                // if deathlink goes here
                int deathlink = int.Parse(client.Options?.GetValueOrDefault("deathlink", "0").ToString());
                if (deathlink == 1)
                {
#if DEBUG
                Console.WriteLine("Deathlink is activated.");
#endif
                    var deathLink = client.EnableDeathLink();
                    deathLink.OnDeathLinkReceived += (args) => KillPlayer();
                }

                Log.Logger.Information("Connected to Archipelago");
                Log.Logger.Information($"Playing {client.CurrentSession.ConnectionInfo.Game} as {client.CurrentSession.Players.GetPlayerName(client.CurrentSession.ConnectionInfo.Slot)}");

                Console.WriteLine("Setting up player state..");

#if DEBUG
            Console.WriteLine($"OnConnected Firing. Itemcount: {client.GameState.ReceivedItems.Count}");
#endif
                UpdatePlayerState(client.CurrentSession.Items.AllItemsReceived);


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

                if (isInTheGame())
                {
                    // Reset Hud
                    Memory.Write(Addresses.WeaponIconX, DefaultWeaponIconX);
                    Memory.Write(Addresses.ShieldIconX, DefaultShieldIconX);
                    Memory.Write(Addresses.HealthbarX, DefaultHealthbarX);
                    Memory.Write(Addresses.ChaliceIconX, DefaultChaliceIconX);
                    Memory.Write(Addresses.MoneyIconX, DefaultMoneyIconX);
                    Memory.Write(Addresses.WeaponIconY, DefaultWeaponIconY);
                    Memory.Write(Addresses.ShieldIconY, DefaultShieldIconY);
                    Memory.Write(Addresses.HealthbarY, DefaultHealthbarY);
                    Memory.Write(Addresses.ChaliceIconY, DefaultChaliceIconY);
                    Memory.Write(Addresses.MoneyIconY, DefaultMoneyIconY);

                    // Reset Jump Height
                    Memory.Write(Addresses.DanJumpHeight, defaultJumpValue);
                    Memory.Write(Addresses.DanForwardSpeed, defaultSpeedValue);
                }
            }
        }



        async void OnDisconnected(object sender, ConnectionChangedEventArgs args, ArchipelagoClient client, bool firstRun)
        {
            if (client.GameState == null && firstRun == false) ;
            {
                Console.WriteLine("Disconnected from Archipelago.");
            }
        }



        // logic for item receiving goes here (gold, health, ammo, etc)
        void ItemReceived(object sender, ItemReceivedEventArgs args, ArchipelagoClient client)
        {

            if (client.CurrentSession != null) {
#if DEBUG
            Console.WriteLine($"ItemReceived Firing. Itemcount: {client.CurrentSession.Items.AllItemsReceived.Count}");
#endif
                byte currentLevel = Memory.ReadByte(Addresses.CurrentLevel);
                int runeSanityOption = Int32.Parse(archipelagoClient.Options?.GetValueOrDefault("runesanity", "0").ToString());

                switch (args.Item)
                {
                    // incoming runes need added here
                    case var x when x.Name.ContainsAny("Rune") && runeSanityOption == 1: ReceiveRune(currentLevel, x); break;
                    case var x when x.Name.ContainsAny("Skill"): ReceiveSkill(x); break;
                    case var x when x.Name.ContainsAny("Equipment"): ReceiveEquipment(x); break;
                    case var x when x.Name.ContainsAny("Life Bottle"): ReceiveLifeBottle(); break;
                    case var x when x.Name.ContainsAny("Soul Helmet"): ReceiveSoulHelmet(); break;
                    case var x when x.Name.ContainsAny("Dragon Gem"): ReceiveDragonGem(); break;
                    case var x when x.Name.ContainsAny("Amber"): ReceiveAmber(); break;
                    case var x when x.Name.ContainsAny("Key Item"): ReceiveKeyItem(x); break;
                    case var x when x.Name.ContainsAny("Health", "Gold Coins", "Energy"): ReceiveStatItems(x); break;
                    case var x when x.Name.ContainsAny("Daggers", "Ammo", "Chicken Drumsticks", "Crossbow", "Longbow", "Fire Longbow", "Magic Longbow", "Spear", "Copper Shield", "Silver Shield", "Gold Shield"): ReceiveCountType(x); break;
                    case var x when x.Name.ContainsAny("Broadsword", "Club", "Lightning"): ReceiveChargeType(x); break;
                    case var x when x.Name.Contains("Trap: Heavy Dan"): HeavyDanTrap(); break;
                    case var x when x.Name.Contains("Trap: Light Dan"): LightDanTrap(); break;
                    case var x when x.Name.Contains("Trap: Goodbye Shield"): GoodbyeShieldTrap(); break;
                    case var x when x.Name.Contains("Trap: Hudless"): HudlessTrap(); break;
                    case var x when x.Name.Contains("Trap: Lag"): RunLagTrap(); break;
                    case null: Console.WriteLine("Received an item with null data. Skipping."); break;
                    default: Console.WriteLine($"Item not recognised. ({args.Item.Name}) Skipping"); break;
                };

                UpdatePlayerState(client.CurrentSession.Items.AllItemsReceived);
            }

        }

        void Client_MessageReceived(object sender, MessageReceivedEventArgs e, ArchipelagoClient client)
        {
#if DEBUG
            Console.WriteLine($"MessageReceived Firing. Itemcount: {client.CurrentSession.Items.AllItemsReceived.Count}");
#endif
            var message = string.Join("", e.Message.Parts.Select(p => p.Text));

            // this message can use emoji's through the overlay. Look into maybe making it a little more obvious 
            // what each item is with a symbol
            client.AddOverlayMessage(e.Message.ToString());

            Log.Logger.Information(JsonConvert.SerializeObject(e.Message));

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

        void Client_LocationCompleted(object sender, LocationCompletedEventArgs e, ArchipelagoClient client)
        {
            CheckGoalCondition();
#if DEBUG
            Console.WriteLine($"LocationCompleted Firing. Itemcount: {client.CurrentSession.Items.AllItemsReceived.Count}");
#endif
        }


        void Locations_CheckedLocationsUpdated(System.Collections.ObjectModel.ReadOnlyCollection<long> newCheckedLocations)
        {
#if DEBUG
            Console.WriteLine($"Location CheckedLocationsUpdated Firing.");
#endif
        }

        bool CheckZarokCondition(ArchipelagoClient client)
        {
            if (archipelagoClient?.GameState.CompletedLocations.Any(x => x.Name.Equals("Cleared: Zaroks Lair")) == true)
            {
                Console.WriteLine("You've Defeated Zarok");
                return true;
            }
            return false;
        }

        bool CheckChaliceCondition(ArchipelagoClient client)
        {
            int antOption = Int32.Parse(archipelagoClient.Options?.GetValueOrDefault("include_ant_hill_in_checks", "0").ToString());

            int maxChaliceCount = antOption == 1 ? 20 : 19;

            int currentCount = 0;

            foreach (CompositeLocation loc in archipelagoClient.GameState.CompletedLocations)
            {
                if (loc.Name.Contains("Chalice: "))
                {
                    currentCount++;
                }
            }

            if (currentCount == maxChaliceCount)
            {
                archipelagoClient.SendGoalCompletion();
                Console.WriteLine("You got all the chalices!");
                return true;
            }
            return false;
        }



        bool CheckGoalCondition() {

            if (archipelagoClient?.GameState?.CompletedLocations == null)
            {
                return false;
            }

            int goalCondition = Int32.Parse(archipelagoClient.Options?.GetValueOrDefault("goal", "0").ToString());

            if (goalCondition == PlayerGoals.DEFEAT_ZAROK)
            {
                bool goal = CheckZarokCondition(archipelagoClient);

                if (goal)
                {
                    archipelagoClient.SendGoalCompletion();
                    return true;
                }
            }

            if (goalCondition == PlayerGoals.CHALICE)
            {
                bool goal = CheckChaliceCondition(archipelagoClient);

                if (goal)
                {
                    archipelagoClient.SendGoalCompletion();
                    return true;
                }
            }

            if (goalCondition == PlayerGoals.BOTH)
            {
                bool zarokGoal = CheckZarokCondition(archipelagoClient);
                bool chaliceGoal = CheckChaliceCondition(archipelagoClient);

                if(zarokGoal && chaliceGoal)
                {
                    archipelagoClient.SendGoalCompletion();
                    return true;
                }
            }

            return false;
        }


        ////////////////////////////////////
        //
        // Player Status Update
        //
        ////////////////////////////////////



        void UpdatePlayerState(System.Collections.ObjectModel.ReadOnlyCollection<ItemInfo> itemsCollected)
        {
            // get a list of all locatoins
            Dictionary<string, uint> all_items = Helpers.FlattenedInventoryStrings();

            // get a list of used locations
            var usedItems = new List<string>();

            short currentWeapon = Memory.ReadShort(Addresses.ItemEquipped);
            byte currentLevel = Memory.ReadByte(Addresses.CurrentLevel);
            int runeSanityOption = Int32.Parse(archipelagoClient.Options?.GetValueOrDefault("runesanity", "0").ToString());

            SetItemMemoryValue(Addresses.CurrentLifePotions, 0, 0);
            SetItemMemoryValue(Addresses.SoulHelmet, 0, 0);
            SetItemMemoryValue(Addresses.DragonGem, 0, 0);
            SetItemMemoryValue(Addresses.APAmberPieces, 0, 0);
            SetItemMemoryValue(Addresses.MaxAmberPieces, 10, 10);
            SetItemMemoryValue(Addresses.ChaosRune, 65535, 65535);
            SetItemMemoryValue(Addresses.EarthRune, 65535, 65535);
            SetItemMemoryValue(Addresses.MoonRune, 65535, 65535);
            SetItemMemoryValue(Addresses.StarRune, 65535, 65535);
            SetItemMemoryValue(Addresses.TimeRune, 65535, 65535);

            // for each location that's coming in
            bool hasEquipableWeapon = false;
            int talismanCount = 0;
            bool hasTalisman = false;

            if(CheckGoalCondition() && gameCleared == false){
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
                        ReceiveRune(currentLevel, x);                        
                        break;
                    case var x when x.Name.ContainsAny("Charge"):
                        // no plans yet
                        break;
                    case var x when x.Name.Contains("Skill"): ReceiveSkill(x); break;
                    case var x when x.Name.Contains("Equipment"):
                        ReceiveEquipment(x);
                        if (!x.Name.Contains("Shield"))
                        {
                            hasEquipableWeapon = true;
                        }
                        break;
                    case var x when x.Name.Contains("Life Bottle"): ReceiveLifeBottle(); break;
                    case var x when x.Name.Contains("Soul Helmet"):
                        ReceiveSoulHelmet();
                        break;
                    case var x when x.Name.Contains("Dragon Gem"): ReceiveDragonGem(); break;
                    case var x when x.Name.Contains("Amber"): ReceiveAmber(); break;
                    case var x when x.Name.Contains("Key Item"): ReceiveKeyItem(x); break;
                    case var x when x.Name.Contains("Cleared"): ReceiveLevelCleared(x); break;
                }


                if (talismanCount == 2 && !hasTalisman)
                {
                    ReceiveTalismanAndArtefact();
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
                    SetItemMemoryValue(Addresses.ShadowArtefact, 65535, 65535);
                    SetItemMemoryValue(Addresses.ShadowTalisman, 65535, 65535);
                }

                // reset any other values
                if (itemName.ContainsAny("Skill"))
                {
                    SetItemMemoryValue(itemAddress, 0, 0);
                    continue;
                }
                else if (itemName.ContainsAny("Equipment"))
                {
                    SetItemMemoryValue(itemAddress, 65535, 65535); // Assuming 65535 is "reset/max" for equipment
                    continue;
                }
                else if (itemName.ContainsAny("Complete"))
                {
                    SetItemMemoryValue(itemAddress, 0, 0);
                    continue;

                }
                else if (itemName.ContainsAny("Key Item"))
                {
                    SetItemMemoryValue(itemAddress, 65535, 65535);
                    continue;

                }

            }


            if (!hasEquipableWeapon)
            {
                DefaultToArm();
            }
            else
            {
                EquipWeapon(currentWeapon);
            }
        }


        int SetItemMemoryValue(uint itemMemoryAddress, int itemUpdateValue, int maxCount)
        {
            int addition = Math.Min(itemUpdateValue, maxCount);

            byte[] byteArray = BitConverter.GetBytes(addition);

            Memory.WriteByteArray(itemMemoryAddress, byteArray);

            // Add more types as needed
            return itemUpdateValue;
        }

        // Update functions with correct logic

        void UpdateCurrentItemValue(string itemName, int numberUpdate, uint itemMemoryAddress, bool isCountType, bool isEquipmentType)
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
                //Console.WriteLine($"Name: {itemName}, Ammo count: {ammoToAdd}");
                SetItemMemoryValue(itemMemoryAddress, ammoToAdd, countMax);
            }
            else if (isEquipmentType && !isCountType)
            {
                var ammoToAdd = currentNumberAmount == -1 ? 4096 : newNumberAmount;
                //Console.WriteLine($"Name: {itemName}, Charge count: {ammoToAdd}");
                SetItemMemoryValue(itemMemoryAddress, ammoToAdd, countMax);
            }

        }

        int ExtractBracketAmount(string itemName)
        {
            var match = Regex.Match(itemName, @"\((\d+)\)");
            if (match.Success && int.TryParse(match.Groups[1].Value, out int bracketAmount))
            {
                return bracketAmount;
            }
            return 0;
        }

        string ExtractDictName(string itemName)
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

        string ExtractRuneName(string itemName)
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
        string ExtractRuneLevel(string itemName)
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

        string ExtractKeyItemName(string itemName)
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

        void ReceiveCountType(Item item)
        {
            var addressDict = Helpers.StatusAndInventoryAddressDictionary();
            var amount = ExtractBracketAmount(item.Name);
            var name = ExtractDictName(item.Name);

            UpdateCurrentItemValue(item.Name, amount, addressDict["Ammo"][name], true, true);
        }

        void ReceiveChargeType(Item item)
        {
            var addressDict = Helpers.StatusAndInventoryAddressDictionary();
            var amount = ExtractBracketAmount(item.Name);
            var name = ExtractDictName(item.Name);

            UpdateCurrentItemValue(item.Name, amount, addressDict["Ammo"][name], false, true);
        }

        void ReceiveEquipment(Item item)
        {
            var addressDict = Helpers.StatusAndInventoryAddressDictionary();
            var name = ExtractDictName(item.Name);

            var isChargeType = item.Name.ContainsAny("Broadsword", "Club", "Lightning");

            UpdateCurrentItemValue(item.Name, 0, addressDict["Equipment"][name], !isChargeType, true);

        }


        void ReceiveLevelCleared(Item level)
        {
            var addressDict = Helpers.StatusAndInventoryAddressDictionary();
            var name = ExtractDictName(level.Name);

            UpdateCurrentItemValue(level.Name, 16, addressDict["Level Status"][name], true, true);
        }

        void ReceiveKeyItem(Item item)
        {
            // commented out because i need to make a list of player data addresses to deal with this.
            var addressDict = Helpers.StatusAndInventoryAddressDictionary();
            var name = ExtractKeyItemName(item.Name);

            UpdateCurrentItemValue(item.Name, 0, addressDict["Key Items"][name], true, true);

        }

        void ReceiveLifeBottle()
        {
            var addressDict = Helpers.StatusAndInventoryAddressDictionary();

            UpdateCurrentItemValue("Life Bottle", 1, addressDict["Player Stats"]["Life Bottle"], true, false);
        }

        void ReceiveSoulHelmet()
        {
            var addressDict = Helpers.StatusAndInventoryAddressDictionary();
            UpdateCurrentItemValue("Soul Helmet", 1, addressDict["Key Items"]["Soul Helmet"], true, false);
        }

        void ReceiveDragonGem()
        {
            var addressDict = Helpers.StatusAndInventoryAddressDictionary();
            UpdateCurrentItemValue("Dragon Gem", 1, addressDict["Key Items"]["Dragon Gem"], true, false);
        }

        void ReceiveAmber()
        {
            UpdateCurrentItemValue("Amber Piece", 1, Addresses.APAmberPieces, true, false);
        }

        void ReceiveTalismanAndArtefact()
        {
            var addressDict = Helpers.StatusAndInventoryAddressDictionary();

            UpdateCurrentItemValue("Shadow Artefact", 0, addressDict["Key Items"]["Shadow Artefact"], true, true);
            UpdateCurrentItemValue("Shadow Talisman", 0, addressDict["Key Items"]["Shadow Talisman"], true, true);
        }

        void ReceiveStatItems(Item item)
        {
            var addressDict = Helpers.StatusAndInventoryAddressDictionary();
            var amount = ExtractBracketAmount(item.Name);
            var name = ExtractDictName(item.Name);

            UpdateCurrentItemValue(item.Name, amount, addressDict["Player Stats"][name], true, false);
        }

        void ReceiveRune(byte levelId, Item item)
        {

            var addressDict = Helpers.StatusAndInventoryAddressDictionary();

            var levelName = Helpers.GetLevelNameFromId(levelId);

            var name = ExtractRuneName(item.Name);
            var runeLevel = ExtractRuneLevel(item.Name);

            if (levelName == runeLevel)
            {
                SetItemMemoryValue(addressDict["Runes"][name], 1, 1);
                //UpdateCurrentItemValue(item.Name, 0, addressDict["Runes"][name], true, false);
            }
        }

        void ReceiveSkill(Item item)
        {
            // setting it here till i fix my ridiculous update function
            SetItemMemoryValue(Addresses.DaringDashSkill, 1, 1);
        }

        void EquipWeapon(int value)
        {
            SetItemMemoryValue(Addresses.ItemEquipped, value, value);
        }

        void DefaultToArm()
        {
            SetItemMemoryValue(Addresses.ItemEquipped, 8, 8);
            SetItemMemoryValue(Addresses.SmallSword, 65535, 65535);
        }

        // traps need added here and logic added into what i have already

        async void KillPlayer()
        {
            Memory.WriteByte(Addresses.CurrentHP, 0x00);
            // Memory.WriteByte(Addresses.CurrentEnergy, 0x00);
        }

        async void RunLagTrap()
        {
            using (var lagTrap = new LagTrap(TimeSpan.FromSeconds(20)))
            {
                lagTrap.Start();
                await lagTrap.WaitForCompletionAsync();
            }
        }

        void HeavyDanTrap()

        {
            byte[] defaultValue = BitConverter.GetBytes(0x0100);
            byte[] changedValue = BitConverter.GetBytes(0x0040);
            TimeSpan duration = TimeSpan.FromSeconds(15);
            Memory.Write(Addresses.DanForwardSpeed, changedValue);

            Task.Delay(duration).ContinueWith(delegate
            {
                Memory.Write(Addresses.DanForwardSpeed, defaultValue);
            }, TaskScheduler.Default);

        }

        void LightDanTrap()
        {
            byte[] defaultValue = BitConverter.GetBytes(0x002f);
            byte[] changedValue = BitConverter.GetBytes(0x0064);
            TimeSpan duration = TimeSpan.FromSeconds(15);
            Memory.Write(Addresses.DanJumpHeight, changedValue);

            Task.Delay(duration).ContinueWith(delegate
            {
                Memory.Write(Addresses.DanJumpHeight, defaultValue);
            }, TaskScheduler.Default);
        }

        void GoodbyeShieldTrap()
        {
            Console.WriteLine("Goodbye Shield Disabled due to bug! Sorry!");
            //Memory.WriteByte(Addresses.ShieldDropped, 0x01);
            //Memory.WriteByte(Addresses.ShieldEquipped, 0x00);
        }

        void HudlessTrap()
        {

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

            byte[] ChangedWeaponIconX = BitConverter.GetBytes(0x0320);
            byte[] ChangedShieldIconX = BitConverter.GetBytes(0x0320);
            byte[] ChangedHealthbarX = BitConverter.GetBytes(0x0320);
            byte[] ChangedChaliceIconX = BitConverter.GetBytes(0x0320);
            byte[] ChangedMoneyIconX = BitConverter.GetBytes(0x0320);

            TimeSpan duration = TimeSpan.FromSeconds(15);
            Memory.Write(Addresses.WeaponIconX, ChangedWeaponIconX);
            Memory.Write(Addresses.ShieldIconX, ChangedShieldIconX);
            Memory.Write(Addresses.HealthbarX, ChangedHealthbarX);
            Memory.Write(Addresses.ChaliceIconX, ChangedChaliceIconX);
            Memory.Write(Addresses.MoneyIconX, ChangedMoneyIconX);

            Task.Delay(duration).ContinueWith(delegate
            {
                Memory.Write(Addresses.WeaponIconX, DefaultWeaponIconX);
                Memory.Write(Addresses.ShieldIconX, DefaultShieldIconX);
                Memory.Write(Addresses.HealthbarX, DefaultHealthbarX);
                Memory.Write(Addresses.ChaliceIconX, DefaultChaliceIconX);
                Memory.Write(Addresses.MoneyIconX, DefaultMoneyIconX);
                Memory.Write(Addresses.WeaponIconY, DefaultWeaponIconY);
                Memory.Write(Addresses.ShieldIconY, DefaultShieldIconY);
                Memory.Write(Addresses.HealthbarY, DefaultHealthbarY);
                Memory.Write(Addresses.ChaliceIconY, DefaultChaliceIconY);
                Memory.Write(Addresses.MoneyIconY, DefaultMoneyIconY);
            }, TaskScheduler.Default);

        }
    }
}