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

        async public static Task CheckForHallOfHeroes(ArchipelagoClient client)
        {
            await Task.Run(() =>
            {

                // created an array of bytes for the update value to be 9999
                byte[] updateValue = BitConverter.GetBytes(0x270F);

                // creates a hashset to compare against
                HashSet<int> processedChaliceCounts = new HashSet<int>();


                while (true)
                {

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

                    // get some basic settings
                    byte currentLevel = Memory.ReadByte(Addresses.CurrentLevel);
                    short dropStatus = Memory.ReadShort(Addresses.HOH_ItemCount);
                    short dialogueStatus = Memory.ReadShort(Addresses.HOH_ListenedToHero);

                    if (currentLevel == 18 && dialogueStatus == 16 && dropStatus == 0 && !processedChaliceCounts.Contains(currentChaliceCount) && client.IsConnected)
                    {
                        Console.WriteLine($"Drop Status: {dropStatus}");

                            // check chalice count against all HOH entries.
                            switch (currentChaliceCount)
                            {
                                case 1:
                                Memory.WriteByteArray(Addresses.HOH_CannyTim1, updateValue);
                                processedChaliceCounts.Add(currentChaliceCount);
                                break;
                            }
                            Thread.Sleep(3000);
       
                    }

                    Thread.Sleep(3000);
                }
            });

        }
    }
}
