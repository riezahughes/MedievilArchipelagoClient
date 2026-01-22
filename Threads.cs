using System.Net.NetworkInformation;
using Archipelago.Core;
using Archipelago.Core.Util;
using MedievilArchipelago.Helpers;
using Serilog;


namespace MedievilArchipelago
{
    public class MemoryCheckThreads
    {

        static bool withinAsylum = false;
        static bool updatedAmber = false;
        static bool hallOfHeroesChecked = false;
        static bool hallOfHeroesRewarded = false;
        static bool chestsFilled = false;
        async public static Task PassiveLogicChecks(ArchipelagoClient client, string url, CancellationTokenSource cts)
        {


            await Task.Run(() =>
            {
                Console.WriteLine("Background task running...");

                Ping ping = new Ping();

                byte currentLocation = Memory.ReadByte(Addresses.CurrentLevel);
                byte mapCoords = Memory.ReadByte(Addresses.CurrentMapPosition);

                // created an array of bytes for the update value to be 9999

                int runeSanityOption = Int32.Parse(client.Options?.GetValueOrDefault("runesanity", "0").ToString());
                int chaliceOption = Int32.Parse(client.Options?.GetValueOrDefault("include_chalices_in_checks", "0").ToString());
                int antHillOption = Int32.Parse(client.Options?.GetValueOrDefault("include_ant_hill_in_checks", "0").ToString());
                int openWorldOption = Int32.Parse(client.Options?.GetValueOrDefault("progression_option", "0").ToString());

                // creates a hashset to compare against
                HashSet<int> processedChaliceCounts = new HashSet<int>();

                ThreadHandlers.ChangeDropModels();

                if (currentLocation == 1 && runeSanityOption == 1)
                {

                    Memory.WriteByte(Addresses.CurrentLevel, 0);
                }

                void SetupAsylumMonitor()
                {
                    Memory.MonitorAddressForAction<byte>(
                        Addresses.CurrentLevel,
                        () =>
                        {
                            if (withinAsylum == false)
                            {
                                ThreadHandlers.UpdateAsylumDynamicDrops();
                                withinAsylum = true;
#if DEBUG
                                //Console.Write(".");
                                Console.WriteLine("---------Asylum Monitor Done");
#endif
                            }
                        },
                        value => value == 14);
                }

                void SetupAnthillMonitor()
                {
                    Memory.MonitorAddressForAction<ushort>(
                        Addresses.TA_BossHealth,
                        () =>
                        {
                            if (updatedAmber == false)
                            {
                                ThreadHandlers.UpdateInventoryWithAmber();
                                updatedAmber = true;
                            }
#if DEBUG
                            Console.WriteLine("---------AntHills Monitor Done");
#endif
                        },
                        value => value == 1000);
                }

                void SetupLevelChestMonitor()
                {
                    Memory.MonitorAddressForAction<ushort>(
                        Addresses.CurrentLevel,
                        () =>
                        {
                            Thread.Sleep(8000);
                            short checkMapCoords = Memory.ReadShort(Addresses.CurrentMapPosition);
                            var loc = Memory.ReadByte(Addresses.CurrentLevel);

                            if (chestsFilled == false)
                            {
                                ThreadHandlers.UpdateChestLocations(client, loc);
                                chestsFilled = true;
                            }
#if DEBUG
                            Console.WriteLine("---------Chest Monitor Done");

#endif
                        },
                        value => value < 23 && value > 0 && PlayerStateHandler.isInTheGame());
                }

                void SetupHallOfHeroesMonitor()
                {
                    Memory.MonitorAddressForAction<ushort>(
                        Addresses.CurrentLevel,
                        () =>
                        {
                            if (hallOfHeroesChecked == false)
                            {
                                ThreadHandlers.UpdateHallOfHeroesTable();
#if DEBUG
                                Console.WriteLine("---------HoH Array Update Monitor Done");
#endif
                                hallOfHeroesChecked = true;
                            }
                        },
                        value => value == 18);
                }

                void SetupHallOfHeroesRewardsMonitor()
                {
                    Memory.MonitorAddressForAction<byte>(
                        Addresses.HOH_ListenedToHero,
                        () =>
                        {
                            int chaliceCount = ThreadHandlers.CountChalicesFromLevelStatus();
                            byte currentLevel = Memory.ReadByte(Addresses.CurrentLevel);
                            short dialogueStatus = Memory.ReadShort(Addresses.HOH_ListenedToHero);

                            if (hallOfHeroesRewarded == false)
                            {
                                ThreadHandlers.UpdateHallOfHeroesChecks(chaliceCount);
#if DEBUG
                                Console.WriteLine("---------HoH Check Update Monitor Done");
#endif
                                hallOfHeroesRewarded = true;
                            }

                        }, value => value == 16 && PlayerStateHandler.isInTheGame());
                }


                void SetupCheatMenuMonitor()
                {
                    Memory.MonitorAddressForAction<ushort>(
                        Addresses.CurrentLevel,
                        () =>
                        {
                            var mainMenuCheck = Memory.ReadShort(Addresses.CurrentMapPosition);

                            if (mainMenuCheck != 0x0100)
                            {
                                ThreadHandlers.SetCheatMenu(client);
#if DEBUG
                                Console.WriteLine("---------Cheat Monitor Done");
#endif
                            }

                        },
                        value => value < 23 && value > 0 && PlayerStateHandler.isInTheGame());
                }

                void SetupOpenWorldMonitor()
                {
                    Memory.MonitorAddressForAction<ushort>(
                        Addresses.CurrentLevel,
                        () =>
                        {
                            ThreadHandlers.OpenTheMap();
#if DEBUG
                            Console.WriteLine("---------Open World Monitor Done");
#endif
                        },
                        value => value < 23 && value > 0 && PlayerStateHandler.isInTheGame());
                }

                /*
                 * Process Delayed Items (keep)
                 * Check Goal Conditions (keep)
                 * x Asylumn Dynamic Drops x 
                 * x Amber Inventory Changes x
                 * x Chest Location Changes per level
                 * x Player State Update if you're not in the hub and level changes
                 * x Open the World Map if there's a level change
                 * x Set the Cheat Menu on level change
                 * x Show Rune Status (does not work right now, can remove)
                 * x Hall of heroes checks
                 */

                SetupAsylumMonitor();
                SetupLevelChestMonitor();

                if (chaliceOption == 1)
                {
                    SetupHallOfHeroesMonitor();
                    SetupHallOfHeroesRewardsMonitor();
                }

                if (currentLocation == 7 && antHillOption == 1)
                {
                    SetupAnthillMonitor();
                }

                if (openWorldOption == 1)
                {
                    SetupOpenWorldMonitor();
                }

                SetupCheatMenuMonitor();

                while (!cts.Token.IsCancellationRequested)
                {
                    try
                    {
                        ThreadHandlers.ProcessDelayedItems(client);

                        // checks against current levels and updates chest entities
                        byte checkCurrentLevel = Memory.ReadByte(Addresses.CurrentLevel);
                        short checkQueenAntStatus = Memory.ReadShort(Addresses.TA_BossHealth);
                        short checkMapCoords = Memory.ReadShort(Addresses.CurrentMapPosition);


                        if (checkMapCoords == 0x0100)
                        {
                            if (openWorldOption == 1)
                            {
                                SetupOpenWorldMonitor();
                            }
                            SetupCheatMenuMonitor();
                            ThreadHandlers.ChangeDropModels();
                        }


                        // Sets up the drops for within the asylum correctly, so it doesn't ruin the randomizer
                        if (currentLocation != 14 && PlayerStateHandler.isInTheGame() && checkMapCoords != 0x0100)
                        {
                            withinAsylum = false;
                        }

                        // sets up the anthil amber choices at the boss fight in the anthill so it doesn't ruin the rewards
                        if (antHillOption == 1 && currentLocation == 7 && updatedAmber == false && PlayerStateHandler.isInTheGame() && checkMapCoords != 0x0100)
                        {
                            SetupAnthillMonitor();
                        }

                        if (currentLocation != 7 && PlayerStateHandler.isInTheGame() && checkMapCoords != 0x0100)
                        {
                            updatedAmber = false;
                        }


                        if (chaliceOption == 1 && currentLocation != 18 && PlayerStateHandler.isInTheGame() && checkMapCoords != 0x0100)
                        {
                            hallOfHeroesChecked = false;
                            SetupHallOfHeroesMonitor();
                        }

                        if (chaliceOption == 1 && currentLocation != 18 && currentLocation != 0 && hallOfHeroesRewarded == true && PlayerStateHandler.isInTheGame() && checkMapCoords != 0x0100)
                        {
                            hallOfHeroesRewarded = false;
                            SetupHallOfHeroesRewardsMonitor();
                        }

                        if (currentLocation != 0 && PlayerStateHandler.isInTheGame())
                        {
                            // this needs to run every time you enter a level. It needs to delay due to it maybe triggering in the middle of a level loading and causing crashes
                            Thread.Sleep(8000);
                            checkMapCoords = Memory.ReadShort(Addresses.CurrentMapPosition);
                            if (checkMapCoords != 0x0100)
                            {
                                PlayerStateHandler.UpdatePlayerState(client, false);
                            }

                        }

                        if (currentLocation != checkCurrentLevel && chestsFilled == true)
                        {
                            checkMapCoords = Memory.ReadShort(Addresses.CurrentMapPosition);
                            if (checkMapCoords != 0x0100)
                            {
                                chestsFilled = false;
                                SetupLevelChestMonitor();
                            }
                        }

                        if (currentLocation != checkCurrentLevel && checkMapCoords != 0x0100)
                        {
                            currentLocation = checkCurrentLevel;
                        }


                        GoalConditionHandlers.CheckGoalCondition(client);

                    }
                    catch (Exception ex)
                    {
                        cts.Cancel();
                        Console.WriteLine("Connection has timed out. Background Task Stopped. Please Restart the Client.");
                        Log.Error(ex, "Error in passive logic checks thread.");
                    }
#if DEBUG
                    //Console.Write(".");
#endif

                    Thread.Sleep(3000);
                }
            }, cts.Token);

        }
    }
}
