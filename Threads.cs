﻿using System;
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

       static internal void UpdateChestLocations(ArchipelagoClient client, int id)
        {
            var chestEntityList = Helpers.ChestContentsDictionary();
            foreach (ulong chestEntity in chestEntityList[id])
            {
                Memory.WriteByte(chestEntity, 0x0008);
            }

        }


        async public static Task CheckForHallOfHeroes(ArchipelagoClient client)
        {
            await Task.Run(() =>
            {

                byte currentLocation = Memory.ReadByte(Addresses.CurrentLevel);

                // created an array of bytes for the update value to be 9999
                byte[] updateValue = BitConverter.GetBytes(0x270F);

                // creates a hashset to compare against
                HashSet<int> processedChaliceCounts = new HashSet<int>();

                bool firstLoop = true;

                UpdateChestLocations(client, currentLocation);

                while (true)
                {
                    // checks against current levels and updates chest entities
                    byte checkCurrentLevel = Memory.ReadByte(Addresses.CurrentLevel);

                    if (!firstLoop && checkCurrentLevel < 17 && checkCurrentLevel > 0)
                    {
                        UpdateChestLocations(client, checkCurrentLevel);
                    }

                    if (currentLocation != checkCurrentLevel)
                    {
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
                    short dropStatus = Memory.ReadShort(Addresses.HOH_ItemCount);
                    short dialogueStatus = Memory.ReadShort(Addresses.HOH_ListenedToHero);
                    ulong offset = 0x92;

                    if (currentLevel == 18 && dialogueStatus == 16 && dropStatus == 0 && !processedChaliceCounts.Contains(currentChaliceCount) && client.IsConnected)
                    {
                        Console.WriteLine($"Drop Status: {dropStatus}");
                            
                            // check chalice count against all HOH entries.
                            switch (currentChaliceCount)
                            {
                                case 1:
                                    Memory.WriteByteArray(Addresses.HOH_CannyTim1, updateValue);
                                    Memory.WriteByte(Addresses.HOH_CannyTim1 + offset , 0x08);
                                    processedChaliceCounts.Add(currentChaliceCount);
                                    break;
                                case 2:
                                    Memory.WriteByteArray(Addresses.HOH_CannyTim2, updateValue);
                                    Memory.WriteByte(Addresses.HOH_CannyTim2 + offset, 0x08);
                                    processedChaliceCounts.Add(currentChaliceCount);
                                    break;
                                case 3:
                                    Memory.WriteByteArray(Addresses.HOH_StanyerIronHewer1, updateValue);
                                    Memory.WriteByte(Addresses.HOH_StanyerIronHewer1 + offset, 0x08);
                                    processedChaliceCounts.Add(currentChaliceCount);
                                    break;
                                case 4:
                                    Memory.WriteByteArray(Addresses.HOH_StanyerIronHewer2, updateValue);
                                    processedChaliceCounts.Add(currentChaliceCount);
                                    break;
                                case 5:
                                    Memory.WriteByteArray(Addresses.HOH_WodenTheMighty1, updateValue);
                                    Memory.WriteByte(Addresses.HOH_WodenTheMighty1 + offset, 0x08);
                                    processedChaliceCounts.Add(currentChaliceCount);
                                    break;
                                case 6:
                                    Memory.WriteByteArray(Addresses.HOH_WodenTheMighty2, updateValue);
                                    processedChaliceCounts.Add(currentChaliceCount);
                                    break;
                                case 7:
                                    Memory.WriteByteArray(Addresses.HOH_Imanzi1, updateValue);
                                    Memory.WriteByte(Addresses.HOH_Imanzi1 + offset, 0x08);
                                    processedChaliceCounts.Add(currentChaliceCount);
                                    break;
                                case 8:
                                    Memory.WriteByteArray(Addresses.HOH_RavenHoovesTheArcher1, updateValue);
                                    Memory.WriteByte(Addresses.HOH_RavenHoovesTheArcher1 + offset, 0x08);
                                    processedChaliceCounts.Add(currentChaliceCount);
                                    break;
                                case 9:
                                    Memory.WriteByteArray(Addresses.HOH_BloodmonathSkullCleaver1, updateValue);
                                    Memory.WriteByte(Addresses.HOH_BloodmonathSkullCleaver1 + offset, 0x08);
                                    processedChaliceCounts.Add(currentChaliceCount);
                                    break;
                                case 10:
                                    Memory.WriteByteArray(Addresses.HOH_RavenHoovesTheArcher2, updateValue);
                                    Memory.WriteByte(Addresses.HOH_RavenHoovesTheArcher2 + offset, 0x08);
                                    processedChaliceCounts.Add(currentChaliceCount);
                                    break;
                                case 11:
                                    Memory.WriteByteArray(Addresses.HOH_KarlStungard1, updateValue);
                                    Memory.WriteByte(Addresses.HOH_KarlStungard1 + offset, 0x08);
                                    processedChaliceCounts.Add(currentChaliceCount);
                                    break;
                                case 12:
                                    Memory.WriteByteArray(Addresses.HOH_BloodmonathSkullCleaver2, updateValue);
                                    processedChaliceCounts.Add(currentChaliceCount);
                                    break;
                                case 13:
                                    Memory.WriteByteArray(Addresses.HOH_DirkSteadfast1, updateValue);
                                    Memory.WriteByte(Addresses.HOH_DirkSteadfast1 + offset, 0x08);
                                    processedChaliceCounts.Add(currentChaliceCount);
                                    break;
                                case 14:
                                    Memory.WriteByteArray(Addresses.HOH_RavenHoovesTheArcher3, updateValue);
                                    Memory.WriteByte(Addresses.HOH_RavenHoovesTheArcher3 + offset, 0x08);
                                    processedChaliceCounts.Add(currentChaliceCount);
                                    break;
                                case 15:
                                    Memory.WriteByteArray(Addresses.HOH_MegwynneStormbinder1, updateValue);
                                    Memory.WriteByte(Addresses.HOH_MegwynneStormbinder1 + offset, 0x08);
                                    processedChaliceCounts.Add(currentChaliceCount);
                                    break;
                                case 16:
                                    Memory.WriteByteArray(Addresses.HOH_RavenHoovesTheArcher4, updateValue);
                                    Memory.WriteByte(Addresses.HOH_RavenHoovesTheArcher4 + offset, 0x08);
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
                                    Memory.WriteByte(Addresses.HOH_DirkSteadfast2 + offset, 0x08);
                                    processedChaliceCounts.Add(currentChaliceCount);
                                    break;
                                case 20:
                                    Memory.WriteByteArray(Addresses.HOH_MegwynneStormbinder2, updateValue);
                                    processedChaliceCounts.Add(currentChaliceCount);
                                    break;
                        }
       
                    }
                    Thread.Sleep(8000);
                }
            });

        }
    }
}
