using Archipelago.Core;
using Archipelago.Core.Util;
using Serilog;
using MedievilArchipelago.Helpers;
using System.Net.NetworkInformation;


namespace MedievilArchipelago
{
    public class MemoryCheckThreads
    {

        async public static Task PassiveLogicChecks(ArchipelagoClient client, string url, CancellationTokenSource cts)
        {
            await Task.Run(() =>
            {
                Console.WriteLine("Background task running...");

                Ping ping = new Ping();

                byte currentLocation = Memory.ReadByte(Addresses.CurrentLevel);
                byte mapCoords = Memory.ReadByte(Addresses.CurrentMapPosition);

                // created an array of bytes for the update value to be 9999
                byte[] updateValue = BitConverter.GetBytes(0x270F);

                int runeSanityOption = Int32.Parse(client.Options?.GetValueOrDefault("runesanity", "0").ToString());
                int openWorldOption = Int32.Parse(client.Options?.GetValueOrDefault("progression_option", "0").ToString());

                //StartMenuToExit();
                ThreadHandlers.SetCheatMenu(client);

                // creates a hashset to compare against
                HashSet<int> processedChaliceCounts = new HashSet<int>();

                bool firstLoop = true;

                ThreadHandlers.UpdateChestLocations(client, currentLocation);

                if (currentLocation == 1 && runeSanityOption == 1)
                {

                    Memory.WriteByte(Addresses.CurrentLevel, 0);
                }

                if (runeSanityOption == 1)
                {
                    ThreadHandlers.ShowCurrentRuneStatus(client, mapCoords);
                }

                if(currentLocation == 0)
                {
                    ItemHandlers.SendChaliceCountToDataStorage(client);
                }


                if(currentLocation != 0)
                {
                    if (openWorldOption == 1)
                    {
                        ThreadHandlers.OpenTheMap();
                    }

                    //SetRuneAxis();
                }

                if (currentLocation == 14)
                {
                    ThreadHandlers.UpdateAsylumDynamicDrops();
                }

                while (!cts.Token.IsCancellationRequested)
                {
                    try
                    {
                        // a really dumb way to check but it works. Removing for now. There's no need and people complained when hosting without any public internet access.
                        //if (!url.Contains("localhost")) {
                        //    PingReply reply = ping.Send("www.google.com", 1000);

                        //    if (reply.Status == IPStatus.TimedOut)
                        //    {
                        //        cts.Cancel();
                        //        Console.WriteLine("Connection has timed out. Background Task Stopped. Please Restart the Client.");
                        //        throw new Exception("Connection has timed out");
                        //    }
                        //}

                        ThreadHandlers.ProcessDelayedItems(client);

                        // checks against current levels and updates chest entities
                        byte checkCurrentLevel = Memory.ReadByte(Addresses.CurrentLevel);
                        short checkQueenAntStatus = Memory.ReadShort(Addresses.TA_BossHealth);
                        byte checkMapCoords = Memory.ReadByte(Addresses.CurrentMapPosition);

                        if (currentLocation == 14)
                        {
                            ThreadHandlers.UpdateAsylumDynamicDrops();
                        }

                        if (currentLocation == 7 && checkQueenAntStatus == 0x03e8) // if we're in the ant hill and the queens hp has spawned
                        {
                            ThreadHandlers.UpdateInventoryWithAmber();
                        }

                        if (!firstLoop && checkCurrentLevel < 23 && checkCurrentLevel > 0)
                        {
                            ThreadHandlers.UpdateChestLocations(client, checkCurrentLevel);
                        }
                        // i think i'm losing my mind
                        //if (currentLocation == 0 && openWorldOption == 1)
                        //{
                        //    Memory.WriteByte(Addresses.CurrentLevel, 0);
                        //}

                        if (currentLocation == 0)
                        {
                            ItemHandlers.SendChaliceCountToDataStorage(client);
                        }

                        if (currentLocation != 0 && PlayerStateHandler.isInTheGame())
                        {
                            // this needs to run every time you enter a level. It needs to delay due to it maybe triggering in the middle of a level loading and causing crashes
                            PlayerStateHandler.UpdatePlayerState(client, false);
                            Thread.Sleep(3);
                        }

                        if (currentLocation != checkCurrentLevel)
                        {

                            //StartMenuToExit();
                            ThreadHandlers.SetCheatMenu(client);

                            if (checkCurrentLevel != 0)
                            {
                                if (openWorldOption == 1)
                                {
                                    ThreadHandlers.OpenTheMap();
                                }
                                //SetRuneAxis();
                            }

                            currentLocation = checkCurrentLevel;


                        }

                        if(runeSanityOption == 1)
                        {
                            ThreadHandlers.ShowCurrentRuneStatus(client, checkMapCoords);
                        }

                        firstLoop = false;

                        GoalConditionHandlers.CheckGoalCondition(client);

                        int currentChaliceCount = 0;
                        var playerStatus = ItemHandlers.StatusAndInventoryAddressDictionary();

                        // for every level, read the status in memory. For ever level that matches either 19 (cleared) or 3 (picked up chalace, but havn't finished hall) increase the chalice count
                        foreach (KeyValuePair<string, uint> ch in playerStatus["Level Status"])
                        {
                            int levelStatus = Memory.ReadByte(ch.Value);

                            if (levelStatus == 19 || levelStatus == 3)
                            {
                                currentChaliceCount++;
                            }

                        }
                        // updates hall of heroes item dropped

                        byte currentLevel = Memory.ReadByte(Addresses.CurrentLevel);

                        if (currentLevel == 18)
                        {
                            ThreadHandlers.UpdateHallOfHeroesTable();
                        }



                        short dialogueStatus = Memory.ReadShort(Addresses.HOH_ListenedToHero);

                        // for debugging
                        //Console.WriteLine($"{currentLevel} - {dialogueStatus} - {!processedChaliceCounts.Contains(currentChaliceCount)} - {client.IsConnected}");

                        if (currentLevel == 18 && dialogueStatus == 16 && !processedChaliceCounts.Contains(currentChaliceCount) && client.IsConnected)
                        {

                            // check chalice count against all HOH entries.
                            switch (currentChaliceCount)
                            {
                                case 1:
                                    Memory.WriteByteArray(Addresses.HOH_CannyTim1, updateValue);
                                    processedChaliceCounts.Add(currentChaliceCount);
                                    break;
                                case 2:
                                    Memory.WriteByteArray(Addresses.HOH_CannyTim2, updateValue);
                                    processedChaliceCounts.Add(currentChaliceCount);
                                    break;
                                case 3:
                                    Memory.WriteByteArray(Addresses.HOH_StanyerIronHewer1, updateValue);
                                    processedChaliceCounts.Add(currentChaliceCount);
                                    break;
                                case 4:
                                    Memory.WriteByteArray(Addresses.HOH_StanyerIronHewer2, updateValue);
                                    processedChaliceCounts.Add(currentChaliceCount);
                                    break;
                                case 5:
                                    Memory.WriteByteArray(Addresses.HOH_WodenTheMighty1, updateValue);
                                    processedChaliceCounts.Add(currentChaliceCount);
                                    break;
                                case 6:
                                    Memory.WriteByteArray(Addresses.HOH_WodenTheMighty2, updateValue);
                                    processedChaliceCounts.Add(currentChaliceCount);
                                    break;
                                case 7:
                                    Memory.WriteByteArray(Addresses.HOH_Imanzi1, updateValue);
                                    processedChaliceCounts.Add(currentChaliceCount);
                                    break;
                                case 8:
                                    Memory.WriteByteArray(Addresses.HOH_RavenHoovesTheArcher1, updateValue);
                                    processedChaliceCounts.Add(currentChaliceCount);
                                    break;
                                case 9:
                                    Memory.WriteByteArray(Addresses.HOH_BloodmonathSkullCleaver1, updateValue);
                                    processedChaliceCounts.Add(currentChaliceCount);
                                    break;
                                case 10:
                                    Memory.WriteByteArray(Addresses.HOH_RavenHoovesTheArcher2, updateValue);
                                    processedChaliceCounts.Add(currentChaliceCount);
                                    break;
                                case 11:
                                    Memory.WriteByteArray(Addresses.HOH_KarlStungard1, updateValue);
                                    processedChaliceCounts.Add(currentChaliceCount);
                                    break;
                                case 12:
                                    Memory.WriteByteArray(Addresses.HOH_BloodmonathSkullCleaver2, updateValue);
                                    processedChaliceCounts.Add(currentChaliceCount);
                                    break;
                                case 13:
                                    Memory.WriteByteArray(Addresses.HOH_DirkSteadfast1, updateValue);
                                    processedChaliceCounts.Add(currentChaliceCount);
                                    break;
                                case 14:
                                    Memory.WriteByteArray(Addresses.HOH_RavenHoovesTheArcher3, updateValue);
                                    processedChaliceCounts.Add(currentChaliceCount);
                                    break;
                                case 15:
                                    Memory.WriteByteArray(Addresses.HOH_MegwynneStormbinder1, updateValue);
                                    processedChaliceCounts.Add(currentChaliceCount);
                                    break;
                                case 16:
                                    Memory.WriteByteArray(Addresses.HOH_RavenHoovesTheArcher4, updateValue);
                                    processedChaliceCounts.Add(currentChaliceCount);
                                    break;
                                case 17:
                                    Memory.WriteByteArray(Addresses.HOH_Imanzi2, updateValue);
                                    processedChaliceCounts.Add(currentChaliceCount);
                                    break;
                                case 18:
                                    Memory.WriteByteArray(Addresses.HOH_KarlStungard2, updateValue);
                                    processedChaliceCounts.Add(currentChaliceCount);
                                    break;
                                case 19:
                                    Memory.WriteByteArray(Addresses.HOH_DirkSteadfast2, updateValue);
                                    processedChaliceCounts.Add(currentChaliceCount);
                                    break;
                                case 20:
                                    Memory.WriteByteArray(Addresses.HOH_MegwynneStormbinder2, updateValue);
                                    processedChaliceCounts.Add(currentChaliceCount);
                                    break;
                            }

                        }
                    }
                    catch (Exception ex)
                    {
                        cts.Cancel();
                        Console.WriteLine("Connection has timed out. Background Task Stopped. Please Restart the Client.");
                        Log.Error(ex, "Error in passive logic checks thread.");
                    }
                    #if DEBUG
                        Console.Write(".");
                    #endif

                    Thread.Sleep(10000);
                }
            }, cts.Token);

        }
    }
}
