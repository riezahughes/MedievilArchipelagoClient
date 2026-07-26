// See https://aka.ms/new-console-template for more information

using System.Text;
using Archipelago.Core;
using Archipelago.Core.Helpers;
using Archipelago.Core.Models;
using Archipelago.Core.Util;
using Archipelago.Core.Util.GPS;
using Archipelago.Core.Util.Overlay;
using MedievilArchipelago;
using MedievilArchipelago.Helpers;
using Newtonsoft.Json.Linq;
using Helpers = MedievilArchipelago.Helpers;

public class Program
{
    public static List<ItemReceivedEventArgs> delayedItems = new List<ItemReceivedEventArgs>();

    private static async Task Main(string[] args)
    {
        Console.OutputEncoding = Encoding.UTF8;
        Console.Title = "💀 Medievil Archipelago Client";

        // set values
        const byte US_OFFSET = 0x38; // this is ADDED to addresses to get their US location
        const byte JP_OFFSET = 0; // could add more offsfets here

        // Connection details
        string url;
        string port;
        string slot = "";
        string password;
        string gameName = "Medievil";
        bool firstRun = true;


        CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();

        List<ILocation> GameLocations = null;

        ////////////////////////////
        //
        // Main Program Flow
        //
        ////////////////////////////

        // Make sure the connect is initialised


        GameClient gameClient = null;
        bool clientInitializedAndConnected = false; // Renamed for clarity

        int retryAttempt = 0;

        while (!clientInitializedAndConnected)
        {
            Console.Clear();
            retryAttempt++;
            Console.WriteLine($"\nAttempt #{retryAttempt}:");

            try
            {
                gameClient = new GameClient("duckstation-qt-x64-ReleaseLTCG");
                clientInitializedAndConnected = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Could not find Duckstation open.");
                Thread.Sleep(5000);
            }
        }

#if DEBUG
#else
        Console.Clear();
#endif

        bool connected = gameClient.Connect();

        var archipelagoClient = new ArchipelagoClient(gameClient);

        Console.WriteLine("Successfully connected to Duckstation.");

        try
        {
            Memory.GlobalOffset = Memory.GetDuckstationOffset();
            Console.WriteLine($"Duckstation Memory Offset found at: 0x{Memory.GlobalOffset:X}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An unexpected error occurred while getting Duckstation memory offset: {ex.Message}");
            Console.WriteLine(ex); // Print full exception for debugging
        }


#if DEBUG
        // auto logs in with Local.json settings if it's set to dev (because laziness)
        var configuration = new ConfigurationBuilder()
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

        Console.WriteLine("Enter AP Domain: (archipelago.gg)");
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

        if (string.IsNullOrWhiteSpace(slot))
        {
            Console.WriteLine("Slot name cannot be empty. Please provide a valid slot name.");
            return;
        }

        Console.WriteLine("Got the details! Attempting to connect to Archipelagos main server");



        try
        {
            await archipelagoClient.Connect(url + ":" + port, gameName);
            Thread.Sleep(1000);
            await archipelagoClient?.Login(slot, password);
            int retryCount = 0;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"\nAn error occurred while connecting to Archipelago: {ex.Message}");
#if DEBUG
            Console.WriteLine(ex); // Print full exception for debugging
#endif
            Console.ReadKey();
            Environment.Exit(1);
        }

        while (archipelagoClient.CurrentSession == null)
        {
            Console.WriteLine("Waiting for current session");
            Thread.Sleep(1000);
        }

        archipelagoClient.Connected += (sender, args) => APHandlers.OnConnected(sender, args, archipelagoClient);
        archipelagoClient.Disconnected += (sender, args) => APHandlers.OnDisconnected(sender, args, archipelagoClient, firstRun);
        archipelagoClient.ItemManager.ItemReceived += (sender, args) => APHandlers.ItemReceived(sender, args, archipelagoClient);
        archipelagoClient.MessageReceived += (sender, args) => APHandlers.Client_MessageReceived(sender, args, archipelagoClient, slot);
        archipelagoClient.LocationManager.LocationCompleted += (sender, args) => APHandlers.Client_LocationCompleted(sender, args, archipelagoClient);
        archipelagoClient.LocationManager.EnableLocationsCondition = () => PlayerStateHandler.isInTheGame();

        archipelagoClient.CurrentSession.Locations.CheckedLocationsUpdated += APHandlers.Locations_CheckedLocationsUpdated;

        GameLocations = LocationHandlers.BuildLocationList(archipelagoClient.Options);


        Console.WriteLine("Successfully connected to Duckstation.");

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

        try
        {

            var gameOverlay = new WindowsOverlayService(new OverlayOptions
            {
                XOffset = 50,
                YOffset = 500,
                FontSize = 12,
                DefaultTextColor = Archipelago.Core.Util.Overlay.Color.Yellow,
                FadeDuration = 10.0f
            });

            archipelagoClient.IntializeOverlayService(gameOverlay);

            while (archipelagoClient.CurrentSession == null)
            {
                Console.WriteLine("Waiting for current session");
                Thread.Sleep(1000);
            }

            // Set up GPS
            archipelagoClient.GPSHandler = APHandlers.Client_GPSHandler();
            archipelagoClient.GPSHandler.SetInterval(500);
            archipelagoClient.GPSHandler.PositionChanged += (sender, args) => APHandlers.Client_GPSPositionChanged(archipelagoClient, GameLocations);
            archipelagoClient.GPSHandler.MapChanged += (sender, args) =>
            {
                PositionData Data = archipelagoClient.GPSHandler.GetCurrentPosition();
                Data.Region = ItemHandlers.GetChaliceCount(archipelagoClient).ToString();
                JObject Package = JObject.FromObject(Data);
                archipelagoClient.CurrentSession.DataStorage[$"Medievil_GPS_Team{archipelagoClient.CurrentSession.Players.ActivePlayer.Team.ToString()}_{archipelagoClient.CurrentSession.Players.ActivePlayer}"] = Package;
            };

            gameOverlay.AddRichTextPopup(new List<ColoredTextSpan>
              {
                  new ColoredTextSpan
                  {
                      Text = "Hello World",
                      Color = Archipelago.Core.Util.Overlay.Color.Yellow
                  }
              });

            gameOverlay.AddTextPopup("💀 Medievil is ready to go.");

            int runeSanityOption = Int32.Parse(archipelagoClient.Options?.GetValueOrDefault("runesanity", "0").ToString());
            int chaliceOption = Int32.Parse(archipelagoClient.Options?.GetValueOrDefault("include_chalices_in_checks", "0").ToString());
            int antHillOption = Int32.Parse(archipelagoClient.Options?.GetValueOrDefault("include_ant_hill_in_checks", "0").ToString());
            int openWorldOption = Int32.Parse(archipelagoClient.Options?.GetValueOrDefault("progression_option", "0").ToString());

            byte initialLocation = Memory.ReadByte(Addresses.CurrentLevel);
            if (initialLocation == 1 && runeSanityOption == 1)
                Memory.WriteByte(Addresses.CurrentLevel, 0);

            ThreadHandlers.ChangeDropModels();
            ThreadHandlers.SetupDropModelsOnMenuReturn();
            ThreadHandlers.SetupAsylumMonitor();
            ThreadHandlers.SetupSleepingVillageSpawnMonitor();
            ThreadHandlers.SetupLevelChestMonitor(archipelagoClient);
            ThreadHandlers.SetupPlayerStateMonitor(archipelagoClient);

            if (chaliceOption == 1)
            {
                ThreadHandlers.SetupHallOfHeroesMonitor();
                ThreadHandlers.SetupHallOfHeroesRewardsMonitor();
            }

            if (initialLocation == 7 && antHillOption == 1)
                ThreadHandlers.SetupAnthillMonitor();

            if (openWorldOption == 1)
                ThreadHandlers.SetupOpenWorldMonitor();

            ThreadHandlers.SetupCheatMenuMonitor(archipelagoClient);

            firstRun = false;
            archipelagoClient.GPSHandler.Start();
            await archipelagoClient.ReceiveReady();

            _ = ThreadHandlers.StartBackgroundLoop(archipelagoClient, _cancellationTokenSource);
            _ = archipelagoClient.LocationManager.MonitorLocationsAsync(archipelagoClient.CurrentSession, GameLocations, _cancellationTokenSource.Token);

            while (!_cancellationTokenSource.Token.IsCancellationRequested)
            {
                try
                {
                    var input = Console.ReadLine();
                    if (input?.Trim().ToLower() == "exit")
                    {
                        _cancellationTokenSource.Cancel();
                        break;
                    }
                    else if (input?.Trim().ToLower().Contains("hint") == true)
                    {

                        string hintString = input?.Trim().ToLower() == "hint" ? "!hint" : $"!hint {input.Substring(5).Trim()}";
                        archipelagoClient.SendMessage(hintString);
                    }
                    else if (input?.Trim().ToLower().Contains("release") == true)
                    {
                        Console.WriteLine("Manually sending goal completion ping to Archipelago server...");
                        archipelagoClient.SendGoalCompletion();
                    }
                    else if (input?.Trim().ToLower() == "goal")
                    {
                        if (archipelagoClient.CurrentSession.Locations.AllLocationsChecked != null)
                        {
                            int getCurrentGoal = Int32.Parse(archipelagoClient.Options?.GetValueOrDefault("goal", "0").ToString());
                            var currentChaliceCount = Helpers.ItemHandlers.GetChaliceCount(archipelagoClient);
                            var currentChaliceLocations = Helpers.ItemHandlers.GetChaliceCountNames(archipelagoClient);
                            int maxChaliceCount = Int32.Parse(archipelagoClient.Options?.GetValueOrDefault("chalice_win_count", "0").ToString());

                            if (currentChaliceCount == 248)
                            {
                                currentChaliceCount = 0;
                            }
                            else if (currentChaliceCount > 20)
                            {
                                currentChaliceCount = 20;
                            }


                            bool ZarokDead = LocationHandlers.IsZarokDead(archipelagoClient);

                            switch (getCurrentGoal)
                            {
                                case 0:

                                    Console.WriteLine("Current Goal: Defeat Zarok");
                                    Console.WriteLine($"Zarok Defeated: {(ZarokDead ? "Yes" : "No")}");
                                    break;
                                case 1:
                                    Console.WriteLine($"Current Goal: Collect {maxChaliceCount} Chalices");
                                    Console.WriteLine($"Current Chalice Count: {currentChaliceCount} / {maxChaliceCount}");
                                    if (currentChaliceCount > 0)
                                    {
                                        Console.Write("Currently have: ");
                                        foreach (var chalice in currentChaliceLocations)
                                        {
                                            Console.WriteLine($"{chalice}, ");
                                        }
                                    }

                                    break;
                                default:
                                    Console.WriteLine($"Current Goal: Beat Zarok + Collect {maxChaliceCount} Chalices");
                                    Console.WriteLine($"Zarok Defeated: {(ZarokDead ? "Yes" : "No")}");
                                    Console.WriteLine($"Current Chalice Count: {currentChaliceCount} / {maxChaliceCount}");
                                    Console.Write("Currently have: ");
                                    foreach (var chalice in currentChaliceLocations)
                                    {
                                        Console.WriteLine($"{chalice}, ");
                                    }
                                    break;
                            }

                        }
                    }
#if DEBUG
                    else if (input?.Trim().Contains("sound") == true)
                    {
                        try
                        {
                            int picked = Int32.Parse(input.Substring(5).Trim());
                            Console.WriteLine($"sound input number: {picked}");
                            if (picked < Helpers.JingleHandler.Sounds().Count && picked > -1)
                            {
                                Helpers.JingleHandler.Sounds()[picked]();
                            }
                            else Console.WriteLine($"out of list {picked}");
                        }
                        catch (FormatException)
                        {
                            Console.WriteLine($"Unable to parse '{input}'");
                        }

                    }
#endif
                    else if (input?.Trim().ToLower() == "update")
                    {
                        if (archipelagoClient.CurrentSession.Locations.AllLocationsChecked != null)
                        {
                            Helpers.PlayerStateHandler.UpdatePlayerState(archipelagoClient, false);
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
                    // allow manually handling traps if you're in dev mode (for testing)
#if DEBUG
                    else if (input?.Trim().ToLower() == "heavytrap")
                    {
                        Helpers.TrapHandlers.HeavyDanTrap();
                    }
                    else if (input?.Trim().ToLower() == "lighttrap")
                    {
                        Helpers.TrapHandlers.LightDanTrap();
                    }
                    else if (input?.Trim().ToLower() == "darknesstrap")
                    {
                        Helpers.TrapHandlers.DarknessTrap(0x01, archipelagoClient);
                    }
                    else if (input?.Trim().ToLower() == "hudtrap")
                    {
                        Helpers.TrapHandlers.HudlessTrap(archipelagoClient);
                    }
#endif
                    else if (!string.IsNullOrWhiteSpace(input))
                    {
                        Console.WriteLine($"Unknown command: '{input}'");
                    }
                }
                catch (Exception ex)
                {
                    _cancellationTokenSource.Cancel();
                    Console.WriteLine("The system has crashed. (Probably broken connection");
                }
            }

            Console.WriteLine("Shutting down due to connection issues.");
        }
        catch (Exception ex)
        {
            _cancellationTokenSource.Cancel();
            Console.WriteLine($"An unexpected error occurred while connecting to Archipelago: {ex.Message}");
            Console.WriteLine(ex); // Print full exception for debugging
        }
        finally
        {
            // Perform any necessary cleanup here
            Console.WriteLine("Shutting down...");
        }
    }

}