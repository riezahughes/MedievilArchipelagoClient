using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Runtime.InteropServices.JavaScript;
using System.Text;
using System.Threading.Tasks;
using SharpDX.Direct2D1;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace MedievilArchipelago
{
    public static class Addresses
    {
        public const uint FakeAddress = 0x00000000; // This is a placeholder for an address that doesn't exist in the game. Used for testing.

        public const uint InGameCheck = 0x000f88a0;
        // Current Level
        public const uint CurrentLevel = 0x000eee68;

        // Game Complete
        public const uint WinConditionCredits = 0x00010038;
        /*
            0x00=None/Out of Game
            0x01=The Graveyard
            0x02=Return to the Graveyard
            0x03=Cemetery Hill
            0x04=The Hilltop Mausoleum
            0x05=Scarecrow Fields
            0x06=Dan's Crypt
            0x07=The Ant Caves
            0x08=The Crystal Caves
            0x09=Pumpkin Gorge
            0x0A=The Pumpkin Serpent
            0x0B=The Sleeping Village
            0x0C=Pools Of The Ancient Dead
            0x0D=The Asylum Grounds
            0x0E=Inside The Asylum
            0x0F=The Enchanted Earth
            0x10=The Gallows Gauntlet
            0x11=The Haunted Ruins
            0x12=Hall of Heroes
            0x13=The Ghost Ship
            0x14=The Entrance Hall
            0x15=The Time Device
            0x16=The Lake
            0x17=Zarok's Lair
            0x18=Programmers's Playground (Freezes)
         */

        public const uint CurrentChalicePercentage = 0x000f82c0;
        // 0x1000 (4096) = 100%

        public const uint DaringDashSkill = 0x000F81C4;

        // Completed levels
        public const uint TG_LevelStatus = 0x000f81e1;
        public const uint RTG_LevelStatus = 0x000F81E2;
        public const uint CH_LevelStatus = 0x000F81E3;
        public const uint HM_LevelStatus = 0x000F81E4;
        public const uint SF_LevelStatus = 0x000F81E5;
        public const uint DC_LevelStatus = 0x000F81E6;
        public const uint TA_LevelStatus = 0x000F81E7;
        public const uint TL_LevelStatus = 0x000F81F6;
        public const uint CC_LevelStatus = 0x000F81E8;
        public const uint PG_LevelStatus = 0x000F81E9;
        public const uint PS_LevelStatus = 0x000F81EA;
        public const uint TSV_LevelStatus = 0x000F81EB;
        public const uint PAD_LevelStatus = 0x000F81EC;
        public const uint AG_LevelStatus = 0x000F81ED;
        public const uint IA_LevelStatus = 0x000F81EE;
        public const uint EE_LevelStatus = 0x000F81EF;
        public const uint GG_LevelStatus = 0x000F81F0;
        public const uint HR_LevelStatus = 0x000F81F1;
        public const uint GS_LevelStatus = 0x000F81F3;
        public const uint EH_LevelStatus = 0x000F81F4;
        public const uint TD_LevelStatus = 0x000F81F5;

        // List of Hall of Heroes encounters

        public const uint CannyTim1 = 0x000197BA;
        public const uint CannyTim2 = 0x000197C2;
        public const uint StanyerIronHewer1 = 0x000197CA;
        public const uint StanyerIronHewer2 = 0x000197D2;
        public const uint WodenTheMighty1 = 0x000197EA;
        public const uint WodenTheMighty2 = 0x000197F2;
        public const uint RavenHoovesTheArcher1 = 0x0001978A;
        public const uint RavenHoovesTheArcher2 = 0x00019792;
        public const uint RavenHoovesTheArcher3 = 0x0001979A;
        public const uint RavenHoovesTheArcher4 = 0x000197A2;
        public const uint Imanzi1 = 0x0001977A;
        public const uint Imanzi2 = 0x00019782;
        public const uint DarkSteadfast1 = 0x000197FA;
        public const uint DarkSteadfast2 = 0x00019802;
        public const uint KarlStungard1 = 0x0001980A;
        public const uint KarlStungard2 = 0x00019812;
        public const uint BloodmonathSkillCleaver1 = 0x000197AA;
        public const uint BloodmonathSkillCleaver2 = 0x000197B2;
        public const uint MegwynneStormbinder1 = 0x000197DA;
        public const uint MegwynneStormbinder2 = 0x000197E2;

        // key items

        public const uint DragonGem = 0x000f82c8;
        public const uint KingPeregrinesCrown = 0x00000000; // not sure about this one yet. Will probably need to check.
        public const uint SoulHelmet = 0x000F8290;
        public const uint WitchesTalisman = 0x000f829c;
        public const uint SafeKey = 0x000F82A8;
        public const uint ShadowArtefact = 0x000f8294;
        public const uint Crucifix = 0x00000000;
        public const uint LandlordsBust = 0x000F82AC;
        public const uint CrucifixCast = 0x000F82A4;
        public const uint AmberPiece = 0x000F828C;
        public const uint HarvesterParts = 0x000F8288;
        public const uint SkullKey = 0x000f8280;
        public const uint SheetMusic = 0x000F827C;

        // runes

        public const uint ChaosRune = 0x000f8268;
        public const uint EarthRune = 0x000f826c;
        public const uint MoonRune = 0x000f8270;
        public const uint StarRune = 0x000f8274;
        public const uint TimeRune = 0x000f8278;

        // Dan's Stats  
        public const uint CurrentGold = 0x000f82c4;
        public const uint CurrentEnergy = 0x000f81ac;
        public const uint CurrentLifePotions = 0x000f81b5;

        // Dans Hands
        public const uint ItemEquipped = 0x000f81b8;
        public const uint ShieldEquipped = 0x000f81bc;

        // Items Without Ammo
        public const uint SmallSword = 0x000f822c;
        public const uint MagicSword = 0x000f8234;
        public const uint Hammer = 0x000f823c;
        public const uint Axe = 0x000f8244;
        public const uint GoodLightning = 0x000f8264;
        public const uint DragonArmour = 0x000f82d0;

        // Item Charges

        public const uint BroadswordCharge = 0x000f8230;
        public const uint ClubCharge = 0x000f8238;
        public const uint LightningCharge = 0x000f8260;

        // Item Ammo

        public const uint DaggerAmmo = 0x000f8240;
        public const uint ChickenDrumsticksAmmo = 0x000f8248;
        public const uint CrossbowAmmo = 0x000f824c;
        public const uint LongbowAmmo = 0x000f8250;
        public const uint FireLongbowAmmo = 0x000f8254;
        public const uint MagicLongbowAmmo = 0x000f8258;
        public const uint SpearAmmo = 0x000f825c;
        public const uint CopperShieldAmmo = 0x000f82b4;
        public const uint SilverShieldAmmo = 0x000f82b8;
        public const uint GoldShieldAmmo = 0x000f82cc;

        // Level Pickups

        // Dans Crypt
        public const uint DC_Pickup_Shortsword = 0x0012dfb4;
        public const uint DC_Pickup_StarRune = 0x0012e0f4;
        public const uint DC_Pickup_GoldCoinsOverWater = 0x0012e134;
        public const uint DC_Pickup_CopperShield = 0x00186d10;
        public const uint DC_Pickup_LifeBottle = 0x0012e234;
        public const uint DC_Pickup_Daggers = 0x0012e1f4;
        public const uint DC_Pickup_LifeBottleWall = 0x0012d93c;
        public const uint DC_Pickup_GoldCoinsBehindWallLeft = 0x0012d8fc;
        public const uint DC_Pickup_GoldCoinsBehindWallRight = 0x0012d8fc;

        // The Graveyard
        public const uint TG_Pickup_EarthRune = 0x0012ede4;
        public const uint TG_Pickup_ChaosRune = 0x0012eda4;
        public const uint TG_Pickup_GoldCoinsNearChaosRune = 0x001300a4;
        public const uint TG_Pickup_LifePotion = 0x001301e4;
        public const uint TG_Pickup_GoldCoinsLifePotionLeftChest = 0x001301a4;
        public const uint TG_Pickup_GoldCoinsLifePotionRightChest = 0x0012f3a4;
        public const uint TG_Pickup_GoldCoinsShopChest = 0x0012f2a4;
        public const uint TG_Pickup_GoldCoinsBagNearHillFountain = 0x0012f664;
        public const uint TG_Pickup_CopperShield = 0x00000000; // Need the actual value for this. Missed it.
        public const uint TG_Pickup_Chalice = 0x0012fda4;

        // Cemetery Hill
        public const uint CH_Pickup_GoldCoinsNearBoulderEntrance = 0x0012e85c;
        public const uint CH_Pickup_EnergyVialNearShop = 0x0012fb1c;
        public const uint CH_Pickup_CopperShield1stOnHill = 0x001c7dd4;
        public const uint CH_Pickup_GoldCoinsUpHill1 = 0x0012f9dc;
        public const uint CH_Pickup_CopperShield2ndOnHill = 0x001c9714;
        public const uint CH_Pickup_GoldCoinsUpHill2 = 0x0012f91c;
        public const uint CH_Pickup_CopperShield3rdOnHill = 0x001c2a1c;
        public const uint CH_Pickup_GoldCoinsChestAtExit = 0x0012f95c;
        public const uint CH_Pickup_Club = 0x001c94e4;
        public const uint CH_Pickup_WitchTalisman = 0x0012f65c;
        public const uint CH_Pickup_CopperShieldArena = 0x0012f35c;
        public const uint CH_Pickup_EnergyVialArena = 0x0012fbdc;
        public const uint CH_Pickup_GoldCoinsChestInArena = 0x0012f69c;
        public const uint CH_Pickup_Chalice = 0x0012f79c;

        // Hilltop Mosoleum
        public const uint HM_Pickup_EnergyVialRightCoffin = 0x001c64a0;
        public const uint HM_Pickup_GoldCoinsLeftCoffin = 0x0012e718;
        public const uint HM_Pickup_ClubBrokenBenches = 0x001c3484;
        public const uint HM_Pickup_EnergyVialNearRuneLeftRamp = 0x0012f458;
        public const uint HM_Pickup_EarthRune = 0x0012ed98;
        public const uint HM_Pickup_GoldCoinsAfterEarthRuneDoor = 0x0012ec98;
        public const uint HM_Pickup_MoonRune = 0x0012fbd8;
        public const uint HM_Pickup_ChaosRune = 0x0012f198;
        public const uint HM_Pickup_EnergyVialPhantomOfTheOperaLeft = 0x00130318;
        public const uint HM_Pickup_EnergyVialPhantomOfTheOperaRight = 0x00130358;
        public const uint HM_Pickup_GoldCoinsChestInMoonRoom = 0x0012ec18;
        public const uint HM_Pickup_EnergyVialMoonRoom = 0x001302d8;
        public const uint HM_Pickup_SheetMusic = 0x0012f418;
        public const uint HM_Pickup_DaggersBlockPuzzle = 0x001c0c74;
        public const uint HM_Pickup_CopperShieldBlockPuzzle = 0x001c3e70;
        public const uint HM_Pickup_GlassDemonSkullKey = 0x001cb768; // Changed from "Boss" to "Pickup" for consistency
        public const uint HM_Pickup_GoldChestPhantomOfTheOpera1 = 0x001cc128;
        public const uint HM_Pickup_GoldChestPhantomOfTheOpera2 = 0x001c9e54;
        public const uint HM_Pickup_GoldChestPhantomOfTheOpera3 = 0x001c9ae0;
        public const uint HM_Pickup_Chalice = 0x001cc49c;

        // Return to the Graveyard
        public const uint RTG_Pickup_GoldCoinsChestInCoffinArea1 = 0x0012ee38;
        public const uint RTG_Pickup_EnergyVialCoffinAreaWest = 0x00130ab8;
        public const uint RTG_Pickup_GoldCoinsChestInCoffinArea2 = 0x0012ed38;
        public const uint RTG_Pickup_GoldCoinsChestInCoffinArea3 = 0x0012ed78;
        public const uint RTG_Pickup_GoldCoinsChestInCoffinArea4 = 0x0012ee38;
        public const uint RTG_Pickup_EnergyVialCoffinAreaEast = 0x00130af8;
        public const uint RTG_Pickup_GoldCoinsChestInCoffinArea5 = 0x0012edb8;
        public const uint RTG_Pickup_GoldCoinsBagAboveCoffinArea = 0x00130a78;
        public const uint RTG_Pickup_GoldCoinsBagAfterBridge = 0x001c5470;
        public const uint RTG_Pickup_EnergyVialBelowShop = 0x001303f8;
        public const uint RTG_Pickup_GoldCoinsBagAtShop = 0x00130b38;
        public const uint RTG_Pickup_SilverShieldChestAtShop = 0x001cc318;
        public const uint RTG_Pickup_GoldCoinsBagAtClosedGate = 0x00130978;
        public const uint RTG_Pickup_StarRune = 0x0012ebb8;
        public const uint RTG_Pickup_GoldCoinsChestOnIsland = 0x00130cb8;
        public const uint RTG_Pickup_GoldCoinsUndertakersEntrance = 0x00130938;
        public const uint RTG_Pickup_EnergyVialUndertakersEntrance = 0x00130c78;
        public const uint RTG_Pickup_EnergyVialCliffsRight = 0x001303b8;
        public const uint RTG_Pickup_GoldCoinsCliffsLeft = 0x00130338;
        public const uint RTG_Pickup_EnergyVialCliffsLeft = 0x00130378;
        public const uint RTG_Pickup_DaringDash = 0x00000000;
        public const uint RTG_Pickup_Chalice = 0x001302f8;

        // Scarecrow Fields

        public const uint SF_Pickup_GoldCoinsHaystackAtBeginning = 0x001de5d0;
        public const uint SF_Pickup_GoldCoinsChestInHaystackNearMoonDoor = 0x001dafc0;
        public const uint SF_Pickup_GoldCoinsLeftOfFireNearMoonDoor = 0x001e1740;
        public const uint SF_Pickup_EnergyVialRightOfFireNearMoonDoor = 0x001de644;
        public const uint SF_Pickup_MoonRune = 0x001e4e44;
        public const uint SF_Pickup_EarthRune = 0x001e48f0;
        public const uint SF_Pickup_ClubInsideHut = 0x001de9b4;
        public const uint SF_Pickup_SilverShieldBehindWindmill = 0x001e2198;
        public const uint SF_Pickup_ChaosRune = 0x001e3b7c;
        public const uint SF_Pickup_GoldCoinsBagInTheBarn = 0x001e0a28;
        public const uint SF_Pickup_CopperShieldChestInTheBarn = 0x001e29e0;
        public const uint SF_Pickup_GoldCoinsCornfieldSquareNearBarn = 0x001df09c;
        public const uint SF_Pickup_GoldCoinsCornfieldPath1 = 0x001dcc70;
        public const uint SF_Pickup_EnergyVialCornfieldPath = 0x001d6420;
        public const uint SF_Pickup_GoldCoinsChestUnderHaybail = 0x001dd17c;
        public const uint SF_Pickup_GoldCoinsBagUnderBarnHaybail = 0x001ddf64;
        public const uint SF_Pickup_GoldCoinsBagInThePress = 0x001e1788;
        public const uint SF_Pickup_GoldCoinsBagInTheSpinner = 0x001e1788;
        public const uint SF_Pickup_GoldCoinsChestNextToHarvesterPart = 0x001de5a0;
        public const uint SF_Pickup_HarvesterPart = 0x001de090;
        public const uint SF_Pickup_LifePotion = 0x001dc5d0;
        public const uint SF_Pickup_GoldCoinsChestNextToChalice = 0x001dbee8;
        public const uint SF_Pickup_Chalice = 0x001db380;

        // The Anthill
        public const uint TA_Pickup_ClubChestAtBarrier = 0x001dd562;
        public const uint TA_Pickup_GoldCoinsChestAtBarrierFairy = 0x0012f5ac;
        public const uint TA_Pickup_Amber1 = 0x001304ac;
        public const uint TA_Pickup_Amber2 = 0x001dc03c;
        public const uint TA_Pickup_Amber3 = 0x002e525e;
        public const uint TA_Pickup_Amber4 = 0x002e0656;
        public const uint TA_Pickup_Amber5 = 0x002c09c0;
        public const uint TA_Pickup_Amber6 = 0x002c2798;
        public const uint TA_Pickup_Amber7 = 0x002e0bac;
        public const uint TA_Pickup_Amber8 = 0x001dc560;
        public const uint TA_Pickup_Amber9 = 0x001304ec;
        public const uint TA_Pickup_Amber10 = 0x001dc064;
        public const uint TA_Pickup_GoldCoinsMaggotAtAmber2 = 0x002b4528;
        public const uint TA_Pickup_GoldCoinsMaggotAfterAmber2 = 0x002ce062;
        public const uint TA_Pickup_EnergyVialBeforeFairy1 = 0x002b679e;
        public const uint TA_Pickup_EnergyVialAfterAmber2 = 0x00000000;
        public const uint TA_Pickup_EnergyVialFairy2RoomCenter = 0x002e066e;
        public const uint TA_Pickup_GoldCoinsFairy2RoomCenter = 0x002be6dc;
        public const uint TA_Pickup_GoldCoinsFairy2RoomMaggot = 0x002c08a4;
        public const uint TA_Pickup_GoldCoinsMaggotsBeforeAmber4 = 0x002b4420;
        public const uint TA_Pickup_GoldCoinsMaggotsAtAmber5 = 0x002c2506;
        public const uint TA_Pickup_EnergyVialFairy3 = 0x002e066e;
        public const uint TA_Pickup_GoldCoinsMaggotsAtAmber7_1 = 0x002c0f2a;
        public const uint TA_Pickup_GoldCoinsMaggotInNestAtAmber7 = 0x002c1b62;
        public const uint TA_Pickup_GoldCoinsMaggotInNest = 0x002c09c0;
        public const uint TA_Pickup_EnergyVialBirthingRoomExit = 0x00000000;
        public const uint TA_Pickup_GoldCoinsMaggotAfterFairy4 = 0x001db5fc;
        public const uint TA_Pickup_GoldCoinsMaggotAfterFairy4InNest = 0x001e1b58;
        public const uint TA_Pickup_GoldCoinsMaggotAtFairy5 = 0x001dfe38;
        public const uint TA_Pickup_GoldCoinsMaggotNearAmber9 = 0x001e0a20;
        public const uint TA_Pickup_GoldCoinsMaggotNearShop = 0x001e3afc;

        // Enchanted Earth
        public const uint EE_Pickup_GoldCoinsBagNearTreeHollow = 0x001cf9f0;
        public const uint EE_Pickup_GoldCoinsBagBehindBigTree = 0x001ce358;
        public const uint EE_Pickup_EarthRune = 0x001d807c;
        public const uint EE_Pickup_GoldCoinsChestInEgg = 0x001cf00c;
        public const uint EE_Pickup_CopperShieldInEgg = 0x001d4630;
        public const uint EE_Pickup_GoldCoinsBagAtCaveEntrance = 0x001d298c;
        public const uint EE_Pickup_ShadowTalisman = 0x001cebf8;
        public const uint EE_Pickup_EnergyVialShadowTalismanCave = 0x00130538;
        public const uint EE_Pickup_GoldCoinsChestNearBarrier = 0x00130038;
        public const uint EE_Pickup_StarRune = 0x001cf360;
        public const uint EE_Pickup_GoldCoinsChestLeftOfFountain = 0x0012fef8;
        public const uint EE_Pickup_GoldCoinsChestTopOfFountain = 0x0012feb8;
        public const uint EE_Pickup_GoldCoinsChestRightOfFountain = 0x0012ff38;
        public const uint EE_Pickup_EnergyVialLeftOfTreeDrop = 0x001305b8;
        public const uint EE_Pickup_EnergyVialRightOfTreeDrop = 0x001305f8;
        public const uint EE_Pickup_Chalice = 0x0012fd78;

        // The Sleeping Village
        public const uint TSV_Pickup_GoldCoinsBagInLeftBarrelAtBlacksmith = 0x001ceafc;
        public const uint TSV_Pickup_GoldCoinsBagInRightBarrelAtBlacksmith = 0x001cba90;
        public const uint TSV_Pickup_SilverShieldInBlacksmiths = 0x001cfd68;
        public const uint TSV_Pickup_GoldCoinsBagAtPond = 0x001c88d8;
        public const uint TSV_Pickup_EnergyVialAtPond = 0x001c92dc;
        public const uint TSV_Pickup_MoonRune = 0x001d6494;
        public const uint TSV_Pickup_GoldCoinsBagInBarrelAtInn = 0x001cf57c;
        public const uint TSV_Pickup_GoldCoinsBagInBarrelAtBottomOfInnStairs = 0x001d6dfc;
        public const uint TSV_Pickup_GoldCoinsBagInBarrelBehindInnStairs = 0x001d6dfc;
        public const uint TSV_Pickup_ClubInChestUnderInnStairs = 0x001cfa28;
        public const uint TSV_Pickup_EarthRune = 0x001d1864;
        public const uint TSV_Pickup_EnergyVialBustSwitch = 0x001d6dfc;
        public const uint TSV_Pickup_GoldCoinsBagInTopBustBarrel = 0x001c7e58;
        public const uint TSV_Pickup_GoldCoinsBagInSwitchBustBarrel = 0x001cbe44;
        public const uint TSV_Pickup_LandlordsBust = 0x0012f670;
        public const uint TSV_Pickup_ChaosRune = 0x001d0c04;
        public const uint TSV_Pickup_GoldCoinsBagInLibrary = 0x001c8f6c;
        public const uint TSV_Pickup_CrucifixCast = 0x001d0d88;
        public const uint TSV_Pickup_Crucifix = 0x001d0664;
        public const uint TSV_Pickup_SafeKey = 0x0012f6f0;
        public const uint TSV_Pickup_ShadowArtefact = 0x001cdc5c;
        public const uint TSV_Pickup_GoldCoinsBagAtTopOfTable = 0x001cdfd0;
        public const uint TSV_Pickup_GoldCoinsBagAtBottomOfTable = 0x001d2d20;
        public const uint TSV_Pickup_GoldCoinsChestNextToChalice = 0x001319f0;
        public const uint TSV_Pickup_EnergyVialNearExit = 0x00131630;
        public const uint TSV_Pickup_EnergyVialNearChalice = 0x001318f0;
        public const uint TSV_Pickup_Chalice = 0x00130970;

        // Pools of the Ancient Dead
        public const uint PAD_Pickup_GoldCoinsBagAtEntrance = 0x0012fa44;
        public const uint PAD_Pickup_EnergyVialBrokenStructureNearEntrance = 0x0012fd44;
        public const uint PAD_Pickup_LostSoul1 = 0x0012f204;
        public const uint PAD_Pickup_LostSoul2 = 0x0012f184;
        public const uint PAD_Pickup_LostSoul3 = 0x0012f144;
        public const uint PAD_Pickup_LostSoul4 = 0x0012f104;
        public const uint PAD_Pickup_LostSoul5 = 0x0012f1c4;
        public const uint PAD_Pickup_LostSoul6 = 0x0012f304;
        public const uint PAD_Pickup_LostSoul7 = 0x0012f2c4;
        public const uint PAD_Pickup_LostSoul8 = 0x0012f344;
        public const uint PAD_Pickup_GoldCoinsBagOnIslandNearSoul2 = 0x0012fb04;
        public const uint PAD_Pickup_ChaosRune = 0x0012ea84;
        public const uint PAD_Pickup_EnergyVialNextToLostSoul3 = 0x0012fa04;
        public const uint PAD_Pickup_EnergyVialNearGate = 0x0012fb44;
        public const uint PAD_Pickup_SilverShieldInChestNearSoul5 = 0x001d2ce8;
        public const uint PAD_Pickup_EnergyVialChariotRight = 0x0012fa84;
        public const uint PAD_Pickup_EnergyVialChariotLeft = 0x0012fac4;
        public const uint PAD_Pickup_EnergyVialJumpSpot1 = 0x0012fd84;
        public const uint PAD_Pickup_EnergyVialJumpSpot2 = 0x0012fdc4;
        public const uint PAD_Pickup_GoldCoinsJumpSpot1 = 0x0012f944;
        public const uint PAD_Pickup_GoldCoinsJumpSpot2 = 0x0012f904;
        public const uint PAD_Pickup_LifeBottle = 0x0012f0c4;
        public const uint PAD_Pickup_Chalice = 0x0012f7c4;

        // The Lake
        public const uint TL_Pickup_EnergyVialFloodedHouse = 0x001319d8;
        public const uint TL_Pickup_ChaosRune = 0x0012ef18;
        public const uint TL_Pickup_GoldCoinsBagOutsideFloodedHouse = 0x0012f658;
        public const uint TL_Pickup_EarthRune = 0x0012f618;
        public const uint TL_Pickup_GoldCoinsBagNearClosedGate = 0x00131a58;
        public const uint TL_Pickup_TimeRune = 0x0012f7d8;
        public const uint TL_Pickup_SilverShieldInWhirlpool = 0x001a33e0;
        public const uint TL_Pickup_GoldCoinsBagAtTheWhirlpoolEntrance = 0x00131ad8;
        public const uint TL_Pickup_EnergyVialWhirlpoolWind1 = 0x00131b98;
        public const uint TL_Pickup_EnergyVialWhirlpoolWind2 = 0x00131bd8;
        public const uint TL_Pickup_GoldCoinsWhirlpoolWind1 = 0x00131b18;
        public const uint TL_Pickup_GoldCoinsWhirlpoolWind2 = 0x00131b58;
        public const uint TL_Pickup_GoldCoinsOutsideWhirlpoolExit = 0x00131a98;
        public const uint TL_Pickup_StarRune = 0x001311d8;
        public const uint TL_Pickup_Chalice = 0x00131218;

        // The Crystal Caves
        public const uint CC_Pickup_GoldCoinsBagAtWindingStaircase = 0x00000000;
        public const uint CC_Pickup_GoldCoinsBagInCrystalAtStart = 0x001e0478;
        public const uint CC_Pickup_GoldCoinsBagInSpinner = 0x0012e870;
        public const uint CC_Pickup_EarthRune = 0x0012e8b0;
        public const uint CC_Pickup_SilverShieldInCrystal = 0x001e0848;
        public const uint CC_Pickup_GoldCoinsBagNearSilverShield = 0x0012fc30;
        public const uint CC_Pickup_GoldCoinsChestInCrystalAfterEarthDoor = 0x001e3480;
        public const uint CC_Pickup_GoldCoinsBagInDragonRoom11stPlatform = 0x0012fa70;
        public const uint CC_Pickup_EnergyVialDragonRoom1stPlatform = 0x0012fab0;
        public const uint CC_Pickup_GoldCoinsBagInDragonRoom21stPlatform = 0x0012fa30;
        public const uint CC_Pickup_GoldCoinsChestInDragonRoom1stPlatform = 0x0012f1b0;
        public const uint CC_Pickup_GoldCoinsBagInDragonRoom2ndPlatform = 0x0012faf0;
        public const uint CC_Pickup_GoldCoinsBagInDragonRoom13rdPlatform = 0x0012fb30;
        public const uint CC_Pickup_EnergyVialDragonRoom3rdPlatform = 0x0012fb70;
        public const uint CC_Pickup_GoldCoinsBagInDragonRoom23rdPlatform = 0x0012f0b0;
        public const uint CC_Pickup_GoldCoinsChestInDragonRoom3rdPlatform = 0x0012f070;
        public const uint CC_Pickup_StarRune = 0x0012e8f0;
        public const uint CC_Pickup_GoldCoinsBagInDragonRoom4thPlatform1 = 0x0012fbb0;
        public const uint CC_Pickup_GoldCoinsChestInDragonRoom4thPlatform = 0x0012fbf0;
        public const uint CC_Pickup_GoldCoinsBagInDragonRoom4thPlatform2 = 0x0012f0f0;
        public const uint CC_Pickup_GoldCoinsBagOnLeftOfPool = 0x0012f7b0;
        public const uint CC_Pickup_GoldCoinsBagOnRightOfPool = 0x0012e930;
        public const uint CC_Pickup_GoldCoinsChestInCrystalAfterPool = 0x001d9668;
        public const uint CC_Pickup_DragonArmour = 0x0012f5f0;
        public const uint CC_Pickup_Chalice = 0x0012eff0;

        // The Gallows Gauntlet
        public const uint GG_Pickup_GoldCoinsBagBehindStoneDragon1 = 0x0012f2ac;
        public const uint GG_Pickup_GoldCoinsBagBehindStoneDragon2 = 0x0012f2ec;
        public const uint GG_Pickup_SilverShieldInChestNearExit = 0x001b7428;
        public const uint GG_Pickup_GoldCoinsChestAtSerpent = 0x0012f0ac;
        public const uint GG_Pickup_StarRune = 0x0012ecec;
        public const uint GG_Pickup_GoldCoinsChestNearStarEntrance = 0x0012f0ec;
        public const uint GG_Pickup_EnergyVialNearChalice = 0x0012f12c;
        public const uint GG_Pickup_Chalice = 0x0012edac;

        // Asylumn Grounds
        public const uint AG_Pickup_GoldCoinsBagInBellGraveNearBell = 0x0012fb40;
        public const uint AG_Pickup_GoldCoinsBagInBellGraveNearEntrance = 0x0012fb80;
        public const uint AG_Pickup_GoldCoinsBagNearShootingStatue = 0x0012fa80;
        public const uint AG_Pickup_SilverShieldInChestBehindDoor = 0x001c7144;
        public const uint AG_Pickup_GoldCoinsBagInRatGrave = 0x0012fbc0;
        public const uint AG_Pickup_ChaosRune = 0x0012f400;
        public const uint AG_Pickup_GoldCoinsBehindChaosGate = 0x0012fb00;
        public const uint AG_Pickup_GoldCoinsBehindElephantInGrave = 0x0012fc00;
        public const uint AG_Pickup_EnergyVialNearBishop = 0x0012fd00;
        public const uint AG_Pickup_EnergyVialNearKing = 0x0012fcc0;
        public const uint AG_Pickup_Chalice = 0x0012fa00;

        // Inside the Aslyum
        public const uint IA_Pickup_EnergyVialBatRoom = 0x001b5eb8;
        public const uint IA_Pickup_GoldCoinsBagInBatRoomLeft = 0x001b62f0;
        public const uint IA_Pickup_GoldCoinsChestInBatRoom = 0x001b4f60;
        public const uint IA_Pickup_GoldCoinsBagInBatRoomCentre = 0x001b1a34;
        public const uint IA_Pickup_SilverShieldInBatRoom = 0x001b8b34;
        public const uint IA_Pickup_GoldCoinsBagInBatRoomRight = 0x001b6e8c;
        public const uint IA_Pickup_EnergyVialAsylumRoom1 = 0x001b8a7c;
        public const uint IA_Pickup_EnergyVialAsylumRoom2 = 0x001b4418;
        public const uint IA_Pickup_GoldCoinsBagInAsylumRoom = 0x001b86c8;
        public const uint IA_Pickup_GoldCoinsBagInSewerPrisonEntrance = 0x001ba108;
        public const uint IA_Pickup_GoldCoinsBagOnSewerPrisonBench = 0x001b4d68;
        public const uint IA_Pickup_EarthRune = 0x001b74d8;
        public const uint IA_Pickup_DragonGem = 0x001b3478;
        public const uint IA_Pickup_Chalice = 0x001ba76c;

        // Pumpkin Gorge
        public const uint PG_Pickup_MoonRune = 0x0012de48;
        public const uint PG_Pickup_ClubInChestInTunnel = 0x001ed0e8;
        public const uint PG_Pickup_GoldCoinsBagBehindRocksAtStart = 0x00132b88;
        public const uint PG_Pickup_GoldCoinsChestInCoop1 = 0x00131088;
        public const uint PG_Pickup_EnergyVialInCoop = 0x00132b48;
        public const uint PG_Pickup_GoldCoinsChestInCoop2 = 0x001326c8;
        public const uint PG_Pickup_GoldCoinsChestInCoop3 = 0x00132708;
        public const uint PG_Pickup_EnergyVialInMoonHut = 0x00132888;
        public const uint PG_Pickup_ChaosRune = 0x0012de88;
        public const uint PG_Pickup_GoldCoinsBagInMushroomArea = 0x00132bc8;
        public const uint PG_Pickup_EarthRune = 0x00130dc8;
        public const uint PG_Pickup_EnergyVialTopOfHill = 0x00132808;
        public const uint PG_Pickup_StarRune = 0x00131948;
        public const uint PG_Pickup_SilverShieldInChestAtTopOfHill = 0x001d3fec;
        public const uint PG_Pickup_TimeRune = 0x000f8278;
        public const uint PG_Pickup_EnergyVialBouldersAfterStarRune = 0x00132c88;
        public const uint PG_Pickup_GoldCoinsChestAtBouldersAfterStarRune = 0x00132cc8;
        public const uint PG_Pickup_EnergyVialVinePatchLeft = 0x00132c08;
        public const uint PG_Pickup_EnergyVialVinePatchRight = 0x00132748;
        public const uint PG_Pickup_GoldCoinsChestNearChalice = 0x0012f4c8;
        public const uint PG_Pickup_EnergyVialChalicePath = 0x00132d08;
        public const uint PG_Pickup_Chalice = 0x0012f108;


        // Pumpkin Servent
        public const uint PS_Pickup_GoldCoinsBagBehindHouse = 0x001303b4;
        public const uint PS_Pickup_SilverShieldInChestNearLeeches = 0x001c298c;
        public const uint PS_Pickup_GoldCoinsBagBehindVinesAndPod = 0x001303f4;
        public const uint PS_Pickup_DragonsGem = 0x0012fd34;
        public const uint PS_Pickup_EnergyVialLeftAtMerchantGargoyle = 0x00130474;
        public const uint PS_Pickup_GoldCoinsChestAtMerchantGargoyle = 0x00130374;
        public const uint PS_Pickup_EnergyVialRightAtMerchantGargoyle = 0x00130434;
        public const uint PS_Pickup_Chalice = 0x0012e974;

        // The Haunted Ruins
        public const uint HR_Pickup_ChaosRune = 0x0012f818;
        public const uint HR_Pickup_GoldCoinsNearFirstSetOfFarmers = 0x00130958;
        public const uint HR_Pickup_EnergyVialAboveRune = 0x00130a18;
        public const uint HR_Pickup_EnergyVialCornerOfWalls1 = 0x00130a58;
        public const uint HR_Pickup_SilverShieldInChestNearRuneDoor = 0x001d99b4;
        public const uint HR_Pickup_EnergyVialCornerOfWalls2 = 0x00130a98;
        public const uint HR_Pickup_EnergyVialCornerOfWalls3 = 0x00130ad8;
        public const uint HR_Pickup_EnergyVialUpFromOil = 0x00130918;
        public const uint HR_Pickup_GoldCoinsBagNearChaliceNorth = 0x0012f698;
        public const uint HR_Pickup_GoldCoinsBagNearChaliceSouth = 0x0012f6d8;
        public const uint HR_Pickup_KingPeregrinsCrown = 0x0012ff98;
        public const uint HR_Pickup_GoldCoinsBagInCrownRoom = 0x001309d8;
        public const uint HR_Pickup_EarthRune = 0x001a1fc4;
        public const uint HR_Pickup_GoldCoinsChestAtCatapult1 = 0x0012f758;
        public const uint HR_Pickup_GoldCoinsChestAtCatapult2 = 0x0012f7d8;
        public const uint HR_Pickup_GoldCoinsChestAtCatapult3 = 0x0012f798;
        public const uint HR_Pickup_Chalice = 0x0012f918;

        // The Ghost Ship
        public const uint GS_Pickup_MoonRune = 0x0012ed64;
        public const uint GS_Pickup_GoldCoinsBagInRollingBarrelsRoom = 0x0012fde4;
        public const uint GS_Pickup_SilverShieldInChestInBarrelRoom = 0x001cde00;
        public const uint GS_Pickup_GoldCoinsBagOnDeckAtBarrels = 0x0012f264;
        public const uint GS_Pickup_EnergyVialInCabin = 0x00130124;
        public const uint GS_Pickup_StarRune = 0x0012ede4;
        public const uint GS_Pickup_ChaosRune = 0x0012ee64;
        public const uint GS_Pickup_EnergyVialInCannonRoom = 0x00130224;
        public const uint GS_Pickup_GoldCoinsChestInCannonRoom = 0x0012fe24;
        public const uint GS_Pickup_EnergyVialRopeBridge1 = 0x001301e4;
        public const uint GS_Pickup_EnergyVialRopeBridge2 = 0x001301a4;
        public const uint GS_Pickup_GoldCoinsRopeBridge = 0x0012fe64;
        public const uint GS_Pickup_ClubInChestAtCaptain = 0x001c9a34;
        public const uint GS_Pickup_EnergyVialCageLift1 = 0x0012f764;
        public const uint GS_Pickup_EnergyVialCageLift2 = 0x00130264;
        public const uint GS_Pickup_Chalice = 0x0012ece4;

        // The Entrance Hall
        public const uint EH_Pickup_Chalice = 0x0012e09c;

        // The Time Device

        public const uint TD_Pickup_SilverShieldOnClock = 0x001d8b7c;
        public const uint TD_Pickup_TimeRune = 0x0012e258;
        public const uint TD_Pickup_ChaosRune = 0x0012ef18;
        public const uint TD_Pickup_GoldCoinsLaserPlatformRight = 0x0012f098;
        public const uint TD_Pickup_GoldCoinsLaserPlatformLeft = 0x0012f058;
        public const uint TD_Pickup_EarthRune = 0x0012f2d8;
        public const uint TD_Pickup_GoldCoinsLonePillar1 = 0x00130498;
        public const uint TD_Pickup_GoldCoinsLonePillar2 = 0x001304d8;
        public const uint TD_Pickup_GoldCoinsLonePillar3 = 0x00130518;
        public const uint TD_Pickup_LifeBottle = 0x0012eb18;
        public const uint TD_Pickup_GoldCoinsBagAtEarthStation1 = 0x00130698;
        public const uint TD_Pickup_GoldCoinsBagAtEarthStation2 = 0x00130658;
        public const uint TD_Pickup_GoldCoinsBagAtEarthStation3 = 0x00130618;
        public const uint TD_Pickup_MoonRune = 0x0012f918;
        public const uint TD_Pickup_Chalice = 0x0012ee58;

        // Zaroks Lair

        public const uint ZL_Pickup_GoodLightning = 0x001af3d0;
        public const uint ZL_Pickup_SilverShield = 0x001aecf4;
    }
}
