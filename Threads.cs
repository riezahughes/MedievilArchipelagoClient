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


namespace MedievilArchipelago
{
    public class MemoryCheckThreads
    {

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

                // created an array of bytes for the update value to be 9999
                byte[] updateValue = BitConverter.GetBytes(0x270F);

                int runeSanityOption = Int32.Parse(client.Options?.GetValueOrDefault("runesanity", "0").ToString());

                // creates a hashset to compare against
                HashSet<int> processedChaliceCounts = new HashSet<int>();

                bool firstLoop = true;

                UpdateChestLocations(client, currentLocation);

                if(currentLocation != 0)
                {
                    if (runeSanityOption == 1)
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

                        if (currentLocation != checkCurrentLevel)
                        {

                            if(checkCurrentLevel != 0)
                            {
                                if (runeSanityOption == 1)
                                {
                                    OpenTheMap();
                                }
                                //SetRuneAxis();
                            }
                            currentLocation = checkCurrentLevel;
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
                        Console.WriteLine("Passive Checks...");
                    #endif
                    Thread.Sleep(10000);
                }
            }, cts);

        }
    }
}
