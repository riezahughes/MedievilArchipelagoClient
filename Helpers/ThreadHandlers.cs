using Archipelago.Core.Util;
using Archipelago.Core;
using Archipelago.MultiClient.Net.Models;

namespace MedievilArchipelago.Helpers
{
    internal class ThreadHandlers
    {
        static internal void ProcessDelayedItems(ArchipelagoClient client)
        {

            if (!PlayerStateHandler.isInTheGame())
            {
                return;
            }

            Object sender = null;

            foreach (var item in Program.delayedItems)
            {
                APHandlers.ItemReceived(sender, item, client);
            }

            Program.delayedItems.Clear();
        }
        // will hide the rune under the map. Only set for one rune right now due to experimentation. Not used yet.
        static internal void SetRuneAxis()
        {
            Memory.WriteByte(Addresses.TG_EarthRuneYAxis, 0x0000);
        }

        static internal void UpdateChestLocations(ArchipelagoClient client, int id)
        {
            var chestEntityList = ChestContentsDictionary();
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
            var mapList = OpenMapMemoryLocations();
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
            string levelName = LevelHandlers.GetLevelNameFromMapId(currentMapLevel);

            List<string> currentRunesForLevel = [];

            foreach (ItemInfo itemInf in items)
            {
                if (itemInf.ItemName.ToLower().Contains("rune:") && itemInf.ItemName.ToLower().Contains("rune: " + levelName.ToLower()))
                {

                    currentRunesForLevel.Add(itemInf.ItemName);
                }
            }


            var dict = ListCurrentRunesForLevel(currentRunesForLevel, currentMapLevel);

            string textMessage = "";


            foreach (string rune in dict.Keys)
            {
                string result = dict[rune] == false ? "[  ]" : "[X]";
                string line = rune + " " + result + "\n";
                textMessage = textMessage + line;

            }

            client.AddOverlayMessage(textMessage);

        }

        static internal void StartMenuToExit()
        {
            var exitList = ThreadHandlers.QuitTextMemoryLocations();
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
            if (client.Options == null)
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

        public static Dictionary<string, bool> ListCurrentRunesForLevel(List<string> currentRunes, byte currentLevel)
        {
            var reference = LevelHandlers.GetLevelNameFromMapId(currentLevel);
            var dictOfRunes = LevelToRuneItemDictionary[reference];

            var levelStatus = new Dictionary<string, bool>();

            foreach (var rune in dictOfRunes)
            {
                levelStatus[rune] = currentRunes.Contains(rune);
            }

            return levelStatus;

        }


        public static Dictionary<string, List<string>> LevelToRuneItemDictionary = new Dictionary<string, List<string>>
        {
            { "The Graveyard", new List<string> { "Chaos Rune: The Graveyard", "Earth Rune: The Graveyard" } },
            { "Return to the Graveyard", new List<string> { "Star Rune: Return to the Graveyard" } },
            { "Cemetery Hill", new List<string> {} },
            { "The Hilltop Mausoleum", new List<string> { "Chaos Rune: The Hilltop Mausoleum", "Earth Rune: The Hilltop Mausoleum", "Moon Rune: The Hilltop Mausoleum" } },
            { "Scarecrow Fields", new List<string> { "Chaos Rune: Scarecrow Fields", "Earth Rune: Scarecrow Fields", "Moon Rune: Scarecrow Fields" } },
            { "Dan's Crypt", new List<string> { "Star Rune: Dan's Crypt" } },
            { "Ant Hill", new List<string> {} },
            { "The Crystal Caves", new List<string> { "Earth Rune: The Crystal Caves", "Star Rune: The Crystal Caves" } },
            { "Pumpkin Gorge", new List<string> { "Chaos Rune: Pumpkin Gorge", "Earth Rune: Pumpkin Gorge", "Moon Rune: Pumpkin Gorge", "Star Rune: Pumpkin Gorge", "Time Rune: Pumpkin Gorge" } },
            { "The Pumpkin Serpent", new List<string> {} },
            { "The Sleeping Village", new List<string> { "Chaos Rune: The Sleeping Village", "Earth Rune: The Sleeping Village", "Moon Rune: The Sleeping Village" } },
            { "Pools of the Ancient Dead", new List<string> { "Chaos Rune: Pools of the Ancient Dead" } },
            { "The Asylum Grounds", new List<string> { "Chaos Rune: The Asylum Grounds" } },
            { "Inside the Asylum", new List<string> { "Earth Rune: Inside the Asylum" } },
            { "Enchanted Earth", new List<string> { "Earth Rune: Enchanted Earth", "Star Rune: Enchanted Earth" } },
            { "The Gallows Gauntlet", new List<string> { "Star Rune: The Gallows Gauntlet" } },
            { "The Haunted Ruins", new List<string> { "Chaos Rune: The Haunted Ruins", "Earth Rune: The Haunted Ruins" } },
            { "Hall of Heroes", new List<string> {} },
            { "Ghost Ship", new List<string> { "Chaos Rune: Ghost Ship", "Moon Rune: Ghost Ship", "Star Rune: Ghost Ship" } },
            { "The Entrance Hall", new List<string> {} },
            { "The Time Device", new List<string> { "Chaos Rune: The Time Device", "Earth Rune: The Time Device", "Moon Rune: The Time Device", "Time Rune: The Time Device" } },
            { "The Lake", new List<string> { "Chaos Rune: The Lake", "Earth Rune: The Lake", "Star Rune: The Lake", "Time Rune: The Lake" } },
            { "Zarok's Lair", new List<string> {} }

        };

        // will contain a list of offset values based on the original entities on/off switch address given in addresses. As every entity follows the same table
        // it makes sense that you could just count forward a few bytes to get the chests contents.

        // pretty sure i could just loop these tbh, but i'll refactor when i'm not busy
        public static Dictionary<int, List<ulong>> ChestContentsDictionary()
        {

            ulong contents_offset = 0xc;
            return new Dictionary<int, List<ulong>>
            {
                [0] = [], // main menu
                [1] = [Addresses.TG_Pickup_CopperShield + contents_offset],
                [2] = [Addresses.RTG_Pickup_SilverShieldChestAtShop + contents_offset],
                [3] = [Addresses.CH_Pickup_Club + contents_offset, Addresses.CH_Pickup_CopperShield1stOnHill + contents_offset, Addresses.CH_Pickup_CopperShield2ndOnHill + contents_offset, Addresses.CH_Pickup_CopperShield3rdOnHill + contents_offset],
                [4] = [Addresses.HM_Pickup_ClubBrokenBenches + contents_offset, Addresses.HM_Pickup_DaggersBlockPuzzle + contents_offset],
                [5] = [Addresses.SF_Pickup_ClubInsideHut + contents_offset, Addresses.SF_Pickup_CopperShieldChestInTheBarn + contents_offset, Addresses.SF_Pickup_SilverShieldBehindWindmill + contents_offset],
                [6] = [Addresses.DC_Pickup_CopperShield + contents_offset],
                [7] = [Addresses.TA_Pickup_ClubChestAtBarrier + contents_offset],
                [8] = [], // crystal caves
                [9] = [Addresses.PG_Pickup_ClubInChestInTunnel + contents_offset, Addresses.PG_Pickup_SilverShieldInChestAtTopOfHill + contents_offset],
                [10] = [Addresses.PS_Pickup_SilverShieldInChestNearLeeches + contents_offset],
                [11] = [Addresses.TSV_Pickup_SilverShieldInBlacksmiths + contents_offset, Addresses.TSV_Pickup_ClubInChestUnderInnStairs + contents_offset],
                [12] = [Addresses.PAD_Pickup_SilverShieldInChestNearSoul5 + contents_offset],
                [13] = [Addresses.AG_Pickup_SilverShieldInChestBehindDoor + contents_offset],
                [14] = [Addresses.IA_Pickup_SilverShieldInBatRoom], // this is technically a chest here, but there's no need for an offset
                [15] = [], // enchanted earth
                [16] = [Addresses.GG_Pickup_SilverShieldInChestNearExit + contents_offset],
                [17] = [Addresses.HR_Pickup_SilverShieldInChestNearRuneDoor + contents_offset],
                [18] = [], // hall of heroes
                [19] = [Addresses.GS_Pickup_SilverShieldInChestInBarrelRoom + contents_offset, Addresses.GS_Pickup_ClubInChestAtCaptain + contents_offset],
                [20] = [], //entrance hall
                [21] = [Addresses.TD_Pickup_SilverShieldOnClock + contents_offset],
                [22] = [Addresses.TL_Pickup_SilverShieldInWhirlpool + contents_offset],
                [23] = [Addresses.ZL_Pickup_GoodLightning + contents_offset, Addresses.ZL_Pickup_SilverShield + contents_offset],
            };
        }

        public static List<uint> OpenMapMemoryLocations()
        {
            return new List<uint>
            {
                Addresses.MAP_Unlock1,
                Addresses.MAP_Unlock2,
                Addresses.MAP_Unlock3,
                Addresses.MAP_Unlock4,
                Addresses.MAP_Unlock5,
                Addresses.MAP_Unlock6,
                Addresses.MAP_Unlock7,
                Addresses.MAP_Unlock8,
                Addresses.MAP_Unlock9,
                Addresses.MAP_Unlock10,
                Addresses.MAP_Unlock11,
                Addresses.MAP_Unlock12,
                Addresses.MAP_Unlock13,
                Addresses.MAP_Unlock14,
                Addresses.MAP_Unlock15,
                Addresses.MAP_Unlock16,
                Addresses.MAP_Unlock17,
                Addresses.MAP_Unlock18,
                Addresses.MAP_Unlock19,
                Addresses.MAP_Unlock20,
                Addresses.MAP_Unlock21,
                Addresses.MAP_Unlock22,
                Addresses.MAP_Unlock23,
                Addresses.MAP_Unlock24
            };
        }

        public static List<uint> QuitTextMemoryLocations()
        {
            return new List<uint>
            {
                Addresses.MENU_Quit1,
                Addresses.MENU_Quit2,
            };
        }
    }
}
