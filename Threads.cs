using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using Archipelago.Core;
using Archipelago.Core.Util;
using Helpers = MedievilArchipelago.Helpers;
using Archipelago.Core.Models;
using Serilog;
using GameOverlay.Drawing;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Archipelago.MultiClient.Net.Models;
using SharpDX;
using System.Text.RegularExpressions;
using Newtonsoft.Json.Linq;


namespace MedievilArchipelago
{
    public class MemoryCheckThreads
    {

        static DateTime lastDeathTime = default(DateTime);



        //static internal void SetRuneAxis()
        //{
        //    Memory.WriteByte(Addresses.TG_EarthRuneYAxis, 0x0000);
        //}

        static internal void UpdateChestLocations(ArchipelagoClient client, int id)
        {
            var chestEntityList = Helpers.ChestContentsDictionary();
            foreach (ulong chestEntity in chestEntityList[id])
            {
                Memory.WriteByte(chestEntity, 0x0008);
            }

        }

        // feeling lazy so i'm just duplicating this.
        static internal string ExtractRuneName(string itemName)
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


        static internal void UpdateAsylumDynamicDrops()
        {
            List<uint> drops = [
                Addresses.IA_Pickup_GoldCoinsChestInBatRoom,
                Addresses.IA_Pickup_EnergyVialBatRoom,
                Addresses.IA_Pickup_GoldCoinsBagInBatRoomCentre,
                Addresses.IA_Pickup_GoldCoinsBagInBatRoomLeft,
                Addresses.IA_Pickup_GoldCoinsBagInBatRoomRight
            ];

            foreach (uint drop in drops)
            {
                Memory.WriteByte(drop, 0x07);
            }
        }

        static internal void OpenTheMap()
        {
            var mapList = Helpers.OpenMapMemoryLocations();
            foreach (uint address in mapList)
            {
                Memory.WriteByte(address, 0x01);
            }
        }

        static internal void ShowCurrentRuneStatus(ArchipelagoClient client, byte currentMapLevel)
        {
            var currentDanStatus = Memory.ReadUShort(Addresses.IsLoaded);

            if (currentDanStatus == 59580 || currentMapLevel == 0 || (currentMapLevel > 20 || currentMapLevel < 0))
            {
                return;
            }

            var items = client.CurrentSession.Items.AllItemsReceived;
            string levelName = Helpers.GetLevelNameFromMapId(currentMapLevel);

            List<string> currentRunesForLevel = [];

            foreach (ItemInfo itemInf in items)
            {
                if (itemInf.ItemName.ToLower().Contains("rune:") && itemInf.ItemName.ToLower().Contains("rune: " + levelName.ToLower())){
                    
                    currentRunesForLevel.Add(itemInf.ItemName);
                }
            }


            var dict = Helpers.ListCurrentRunesForLevel(currentRunesForLevel, currentMapLevel);

            string textMessage = "";


            foreach(string rune in dict.Keys)
            {
                string result = dict[rune] == false ? "[  ]" : "[X]";
                string line = rune + " " + result + "\n";
                textMessage = textMessage + line;

            }

            client.AddOverlayMessage(textMessage);

        }

        static internal void StartMenuToExit()
        {
            var exitList = Helpers.QuitTextMemoryLocations();
            Memory.WriteByte(exitList[0], 0x45);
            Memory.WriteByte(exitList[0], 0x78);
        }

        static internal void UpdateHallOfHeroesTable() 
        {
            // counting from base address to the item choice
            ulong offset = 0x36;

            Memory.WriteByte(Addresses.HOH_CannyTim1_drop, 0x08);
            Memory.WriteByte(Addresses.HOH_CannyTim2_drop, 0x08);
            Memory.WriteByte(Addresses.HOH_StanyerIronHewer1_drop, 0x08);
            Memory.WriteByte(Addresses.HOH_StanyerIronHewer2_drop, 0x08);
            Memory.WriteByte(Addresses.HOH_WodenTheMighty1_drop, 0x08);
            Memory.WriteByte(Addresses.HOH_WodenTheMighty2_drop, 0x08);
            Memory.WriteByte(Addresses.HOH_Imanzi1_drop, 0x08);
            Memory.WriteByte(Addresses.HOH_Imanzi2_drop, 0x08);
            Memory.WriteByte(Addresses.HOH_RavenHoovesTheArcher1_drop, 0x08);
            Memory.WriteByte(Addresses.HOH_RavenHoovesTheArcher2_drop, 0x08);
            Memory.WriteByte(Addresses.HOH_RavenHoovesTheArcher3_drop, 0x08);
            Memory.WriteByte(Addresses.HOH_RavenHoovesTheArcher4_drop, 0x08);
            Memory.WriteByte(Addresses.HOH_BloodmonathSkullCleaver1_drop, 0x08);
            Memory.WriteByte(Addresses.HOH_BloodmonathSkullCleaver2_drop, 0x08);
            Memory.WriteByte(Addresses.HOH_KarlStungard1_drop, 0x08);
            Memory.WriteByte(Addresses.HOH_KarlStungard2_drop, 0x08);
            Memory.WriteByte(Addresses.HOH_DirkSteadfast1_drop, 0x08);
            Memory.WriteByte(Addresses.HOH_DirkSteadfast2_drop, 0x08);
            Memory.WriteByte(Addresses.HOH_MegwynneStormbinder1_drop, 0x08);
            Memory.WriteByte(Addresses.HOH_MegwynneStormbinder2_drop, 0x08);
        }

        static internal void SetCheatMenu(ArchipelagoClient client)
        {
            if(client.Options == null)
            {
                return;
            }

            int cheatMenu = Int32.Parse(client.Options?.GetValueOrDefault("cheat_menu", "0").ToString());

            switch (cheatMenu)
            {
                case 0:
                    break;
                case 1:
                    Memory.WriteByte(Addresses.CheatMenu, 0x01);
                    break;
                case 2:
                    Memory.WriteByte(Addresses.CheatMenu, 0x03);
                    break;
            }
            return;
        }

        static internal void UpdateInventoryWithAmber()
        {
            var currentCount = Memory.ReadByte(Addresses.APAmberPieces);
            Memory.WriteByte(Addresses.AmberPiece, currentCount);
        }


        async public static Task PassiveLogicChecks(ArchipelagoClient client, CancellationToken cts)
        {
            await Task.Run(() =>
            {
                Console.WriteLine("Background task running...");

                byte currentLocation = Memory.ReadByte(Addresses.CurrentLevel);
                byte mapCoords = Memory.ReadByte(Addresses.CurrentMapPosition);

                // created an array of bytes for the update value to be 9999
                byte[] updateValue = BitConverter.GetBytes(0x270F);

                int runeSanityOption = Int32.Parse(client.Options?.GetValueOrDefault("runesanity", "0").ToString());
                int openWorldOption = Int32.Parse(client.Options?.GetValueOrDefault("progression_option", "0").ToString());

                //StartMenuToExit();
                SetCheatMenu(client);

                // creates a hashset to compare against
                HashSet<int> processedChaliceCounts = new HashSet<int>();

                bool firstLoop = true;

                UpdateChestLocations(client, currentLocation);
                if (currentLocation == 1 && runeSanityOption == 1)
                {

                    Memory.WriteByte(Addresses.CurrentLevel, 0);
                }

                if (runeSanityOption == 1)
                {
                    ShowCurrentRuneStatus(client, mapCoords);
                }


                if(currentLocation != 0)
                {
                    if (openWorldOption == 1)
                    {
                        OpenTheMap();
                    }

                    //SetRuneAxis();
                }

                if (currentLocation == 14)
                {
                    UpdateAsylumDynamicDrops();
                }

                while (!cts.IsCancellationRequested)
                {
                    try
                    {
                        // checks against current levels and updates chest entities
                        byte checkCurrentLevel = Memory.ReadByte(Addresses.CurrentLevel);
                        short checkQueenAntStatus = Memory.ReadShort(Addresses.TA_BossHealth);
                        byte checkMapCoords = Memory.ReadByte(Addresses.CurrentMapPosition);

                        if (currentLocation == 14)
                        {
                            UpdateAsylumDynamicDrops();
                        }

                        if (currentLocation == 7 && checkQueenAntStatus == 0x03e8) // if we're in the ant hill and the queens hp has spawned
                        {
                            UpdateInventoryWithAmber();
                        }

                        if (!firstLoop && checkCurrentLevel < 17 && checkCurrentLevel > 0)
                        {
                            UpdateChestLocations(client, checkCurrentLevel);
                        }
                        // i think i'm losing my mind
                        //if (currentLocation == 0 && openWorldOption == 1)
                        //{
                        //    Memory.WriteByte(Addresses.CurrentLevel, 0);
                        //}

                        if (currentLocation != checkCurrentLevel)
                        {
                            //StartMenuToExit();
                            SetCheatMenu(client);

                            if (checkCurrentLevel != 0)
                            {
                                if (openWorldOption == 1)
                                {
                                    OpenTheMap();
                                }
                                //SetRuneAxis();
                            }
                            currentLocation = checkCurrentLevel;
                        }

                        if(runeSanityOption == 1)
                        {
                            ShowCurrentRuneStatus(client, checkMapCoords);
                        }

                        firstLoop = false;



                        int currentChaliceCount = 0;
                        var playerStatus = Helpers.StatusAndInventoryAddressDictionary();

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
                            UpdateHallOfHeroesTable();
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
                        Log.Error(ex, "Error in passive logic checks thread.");
                    }
                    #if DEBUG
                        Console.Write(".");
                    #endif

                    Thread.Sleep(10000);
                }
            }, cts);

        }
    }
}
