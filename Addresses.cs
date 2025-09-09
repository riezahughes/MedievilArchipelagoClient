using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Runtime.InteropServices.JavaScript;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using SharpDX.Direct2D1;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace MedievilArchipelago
{
    public static class Addresses
    {
        public const uint FakeAddress = 0x00000000; // This is a placeholder for an address that doesn't exist in the game. Used for testing.

        // checks if you can control Dan
        public const uint InGameCheck = 0x000f88a0;

        public const uint IsLoaded = 0x001fff5c;

        // Current Level
        public const uint CurrentLevel = 0x000eee68;

        // Current Map POS (could randomize it for fun i guess)
        public const uint CurrentMapPosition = 0x000f81d8;

        // Cheat menu. Comes in two flavours. 0x01 or 0x02
        public const uint CheatMenu = 0x000EEE76;

        // Game Complete
        public const uint WinConditionCredits = 0x00010038;

        public const uint FairyCount = 0x001D5680;

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

        public const uint DanRespawnPositionX = 0x000f8886;
        public const uint DanRespawnPositionY = 0x000f8888;
        public const uint DanRespawnPositionZ = 0x000f888a;

        public const uint DanForwardSpeed = 0x000d5110;

        public const uint DanJumpHeight = 0x000d5126;

        public const uint DanDropShield = 0x000F81BE;

        public const uint RenderDistance = 0x000eee1c;

        public const uint DanFrozen = 0x000eeea8;

        public const uint CurrentChalicePercentage = 0x000f82c0;

        public const uint GameGlobalScene = 0x000eeea4;

        // 0x1000 (4096) = 100%
        public const uint DaringDashSkill = 0x000F81C4;

        public const uint WeaponIconX =	0x000DD78C;
        public const uint WeaponIconY =	0x000DD78E;
        public const uint ShieldIconX =	0x000DD798;
        public const uint ShieldIconY =	0x000DD79A;
        public const uint HealthbarX =	0x000DD7A4;
        public const uint HealthbarY =	0x000DD7A6;
        public const uint ChaliceIconX = 0x000DD7BC;
        public const uint ChaliceIconY = 0x000DD7BE;
        public const uint MoneyIconX =	0x000DD7C8;
        public const uint MoneyIconY =	0x000DD7CA;

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
        public const uint EE_LevelStatus = 0x000ee96e; // the level switch is 0x000F81EF, however it interferes with the anthill;
        public const uint GG_LevelStatus = 0x000F81F0;
        public const uint HR_LevelStatus = 0x000F81F1;
        public const uint GS_LevelStatus = 0x000F81F3;
        public const uint EH_LevelStatus = 0x000F81F4;
        public const uint TD_LevelStatus = 0x000F81F5;

        // Unlocking the Map
        public const uint MAP_Unlock1 = 0x000F8214;
        public const uint MAP_Unlock2 = 0x000F8215;
        public const uint MAP_Unlock3 = 0x000F8216;
        public const uint MAP_Unlock4 = 0x000F8217;
        public const uint MAP_Unlock5 = 0x000F8218;
        public const uint MAP_Unlock6 = 0x000F8219;
        public const uint MAP_Unlock7 = 0x000F821A;
        public const uint MAP_Unlock8 = 0x000F821B;
        public const uint MAP_Unlock9 = 0x000F821C;
        public const uint MAP_Unlock10 = 0x000F821D;
        public const uint MAP_Unlock11 = 0x000F821E;
        public const uint MAP_Unlock12 = 0x000F821F;
        public const uint MAP_Unlock13 = 0x000F8220;
        public const uint MAP_Unlock14 = 0x000F8221;
        public const uint MAP_Unlock15 = 0x000F8222;
        public const uint MAP_Unlock16 = 0x000F8223;
        public const uint MAP_Unlock17 = 0x000F8224;
        public const uint MAP_Unlock18 = 0x000F8225;
        public const uint MAP_Unlock19 = 0x000F8226;
        public const uint MAP_Unlock20 = 0x000F8227;
        public const uint MAP_Unlock21 = 0x000F8228;
        public const uint MAP_Unlock22 = 0x000F8229;
        public const uint MAP_Unlock23 = 0x000F822A;
        public const uint MAP_Unlock24 = 0x000F822B;

        // Quit characters on the start menu
        public const uint MENU_Quit1 = 0x000ed764;
        public const uint MENU_Quit2 = 0x000ed765; 


        // Rune Entity Locations

        public const uint TG_ChaosRuneYAxis = 0x00000000;
        public const uint HM_ChaosRuneYAxis = 0x00000000;
        public const uint SF_ChaosRuneYAxis = 0x00000000;
        public const uint TL_ChaosRuneYAxis = 0x00000000;
        public const uint PG_ChaosRuneYAxis = 0x00000000;
        public const uint SV_ChaosRuneYAxis = 0x00000000;
        public const uint POD_ChaosRuneYAxis = 0x00000000;
        public const uint AG_ChaosRuneYAxis = 0x00000000;
        public const uint HR_ChaosRuneYAxis = 0x00000000;
        public const uint GS_ChaosRuneYAxis = 0x00000000;
        public const uint TD_ChaosRuneYAxis = 0x00000000;

        public const uint TG_EarthRuneYAxis = 0x0012edc4;
        public const uint HM_EarthRuneYAxis = 0x00000000;
        public const uint SF_EarthRuneYAxis = 0x00000000;
        public const uint CC_EarthRuneYAxis = 0x00000000;
        public const uint TL_EarthRuneYAxis = 0x00000000;
        public const uint PG_EarthRuneYAxis = 0x00000000;
        public const uint SV_EarthRuneYAxis = 0x00000000;
        public const uint IA_EarthRuneYAxis = 0x00000000;
        public const uint EE_EarthRuneYAxis = 0x00000000;
        public const uint HR_EarthRuneYAxis = 0x00000000;
        public const uint EH_EarthRuneYAxis = 0x00000000;
        public const uint TD_EarthRuneYAxis = 0x00000000;

        public const uint HM_MoonRuneYAxis = 0x00000000;
        public const uint SF_MoonRuneYAxis = 0x00000000;
        public const uint PG_MoonRuneYAxis = 0x00000000;
        public const uint GS_MoonRuneYAxis = 0x00000000;
        public const uint TD_MoonRuneYAxis = 0x00000000;

        public const uint RTG_StarRuneYAxis = 0x00000000;
        public const uint DC_StarRuneYAxis = 0x00000000;
        public const uint CC_StarRuneYAxis = 0x00000000;
        public const uint TL_StarRuneYAxis = 0x00000000;
        public const uint EE_StarRuneYAxis = 0x00000000;
        public const uint GG_StarRuneYAxis = 0x00000000;
        public const uint GS_StarRuneYAxis = 0x00000000;

        public const uint TL_TimeRuneYAxis = 0x00000000;
        public const uint PG_TimeRuneYAxis = 0x00000000;
        public const uint TD_TimeRuneYAxis = 0x00000000;

        // Boss Max Health
        public const uint TA_BossHealth = 0x000F1754; // The Ant Caves

        // List of Hall of Heroes encounters

        public const uint HOH_CannyTim1 = 0x001408b0;
        public const uint HOH_CannyTim2 = 0x001408b4;
        public const uint HOH_StanyerIronHewer1 = 0x001408b8;
        public const uint HOH_StanyerIronHewer2 = 0x001408bc;
        public const uint HOH_WodenTheMighty1 = 0x001408c0;
        public const uint HOH_WodenTheMighty2 = 0x001408c4;
        public const uint HOH_RavenHoovesTheArcher1 = 0x001408c8;
        public const uint HOH_RavenHoovesTheArcher2 = 0x001408cc;
        public const uint HOH_RavenHoovesTheArcher3 = 0x001408d0;
        public const uint HOH_RavenHoovesTheArcher4 = 0x001408d4;
        public const uint HOH_Imanzi1 = 0x001408d8;
        public const uint HOH_Imanzi2 = 0x001408dc;
        public const uint HOH_DirkSteadfast1 = 0x001408e0;
        public const uint HOH_DirkSteadfast2 = 0x001408e4;
        public const uint HOH_KarlStungard1 = 0x001408e8;
        public const uint HOH_KarlStungard2 = 0x001408ec;
        public const uint HOH_BloodmonathSkullCleaver1 = 0x001408f0;
        public const uint HOH_BloodmonathSkullCleaver2 = 0x001408f4;
        public const uint HOH_MegwynneStormbinder1 = 0x001408f8;
        public const uint HOH_MegwynneStormbinder2 = 0x001408fc;

        public const uint HOH_CannyTim1_drop = 0x00019782;
        public const uint HOH_CannyTim2_drop = 0x0001978a;
        public const uint HOH_StanyerIronHewer1_drop = 0x00019792;
        public const uint HOH_StanyerIronHewer2_drop = 0x0001979a;
        public const uint HOH_WodenTheMighty1_drop = 0x000197b2;
        public const uint HOH_WodenTheMighty2_drop = 0x000197ba;
        public const uint HOH_RavenHoovesTheArcher1_drop = 0x00019752;
        public const uint HOH_RavenHoovesTheArcher2_drop = 0x0001975a;
        public const uint HOH_RavenHoovesTheArcher3_drop = 0x00019762;
        public const uint HOH_RavenHoovesTheArcher4_drop = 0x0001976A;
        public const uint HOH_Imanzi1_drop = 0x00019742;
        public const uint HOH_Imanzi2_drop = 0x0001974A;
        public const uint HOH_DirkSteadfast1_drop = 0x000197c2;
        public const uint HOH_DirkSteadfast2_drop = 0x000197cA;
        public const uint HOH_KarlStungard1_drop = 0x000197d2;
        public const uint HOH_KarlStungard2_drop = 0x000197dA;
        public const uint HOH_BloodmonathSkullCleaver1_drop = 0x00019772;
        public const uint HOH_BloodmonathSkullCleaver2_drop = 0x0001977a;
        public const uint HOH_MegwynneStormbinder1_drop = 0x000197a2;
        public const uint HOH_MegwynneStormbinder2_drop = 0x000197aa;

        public const uint HOH_Book_HeroesOfEld = 0x0001bcf8;
        public const uint HOH_Gargoyle_Entrance = 0x0001b6ce;

        public const uint HOH_ItemCount = 0x000f90fa;
        public const uint HOH_ListenedToHero = 0x000196f0;

        public const uint HoH_Gargoyle_Entrance = 0x0001b6ce;



        // key items

        public const uint DragonGem = 0x000f82c8;
        public const uint KingPeregrinesCrown = 0x00000000; // not sure about this one yet. Will probably need to check.
        public const uint SoulHelmet = 0x000F8290;
        public const uint WitchesTalisman = 0x000f829c;
        public const uint SafeKey = 0x000F82A8;
        public const uint ShadowArtefact = 0x000f8294;
        public const uint ShadowTalisman = 0x000f8298;
        public const uint Crucifix = 0x000f82a0;
        public const uint LandlordsBust = 0x000F82AC;
        public const uint CrucifixCast = 0x000F82A4;

        public const uint AmberPiece = 0x000F828C;
        public const uint MaxAmberPieces = 0x000da16a;
        public const uint APAmberPieces = 0x00000730; // used to track amber pieces seperately, so a count can be made in the anthill correctly for checks.
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
        public const uint CurrentStoredEnergy = 0x000f81b0;
        public const uint CurrentLifePotions = 0x000f81b5;
        public const uint LifeBottleSwitch = 0x000f81b4;

        // Dans Hands
        public const uint ItemEquipped = 0x000f81b8;
        public const uint ShieldEquipped = 0x000f81bc;
        public const uint ShieldDropped = 0x000F81BE;

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
        public const uint DC_Pickup_CopperShield = 0x0012e5b0;
        public const uint DC_Pickup_LifeBottle = 0x0012e234;
        public const uint DC_Pickup_Daggers = 0x0012e1f4;
        public const uint DC_Pickup_LifeBottleWall = 0x0012d93c;
        public const uint DC_Pickup_GoldCoinsBehindWallLeft = 0x0012d8fc;
        public const uint DC_Pickup_GoldCoinsBehindWallRight = 0x0012d8bc;
        public const uint DC_Book_UsingCrypt = 0x000113f5;
        public const uint DC_Book_UnlockingRunes = 0x0001171f;
        public const uint DC_Book_PressingSelect = 0x000114ac;
        public const uint DC_Book_PowerAttack = 0x00011533;
        public const uint DC_Book_Swimming = 0x000115e0;
        public const uint DC_Book_Coins = 0x00011802;
        public const uint DC_Book_Zarok = 0x00011959;
        public const uint DC_Gargoyle_Left = 0x00011302;
        public const uint DC_Gargoyle_Right = 0x00011c05;

        // The Graveyard
        public const uint TG_Pickup_EarthRune = 0x0012ede4;
        public const uint TG_Pickup_ChaosRune = 0x0012eda4;
        public const uint TG_Pickup_GoldCoinsNearChaosRune = 0x001300a4;
        public const uint TG_Pickup_LifePotion = 0x001301e4;
        public const uint TG_Pickup_GoldCoinsBagAtStart = 0x00130264;
        public const uint TG_Pickup_GoldCoinsBehindFenceAtStatue = 0x0012f1a4;
        public const uint TG_Pickup_GoldCoinsLifePotionLeftChest = 0x001301a4;
        public const uint TG_Pickup_GoldCoinsLifePotionRightChest = 0x0012f3a4;
        public const uint TG_Pickup_GoldCoinsShopChest = 0x0012f2a4;
        public const uint TG_Pickup_GoldCoinsBagNearHillFountain = 0x0012f664;
        public const uint TG_Pickup_CopperShield = 0x00130220;
        public const uint TG_Book_WelcomeBack = 0x00013961;
        public const uint TG_Book_HealingFountain = 0x00013751;
        public const uint TG_Book_GazeOfAnAngel = 0x0001386f;
        public const uint TG_Book_SkullKey = 0x000138c4;
        public const uint TG_Gargoyle_EndOfLevel = 0x00013a12;
        public const uint TG_Pickup_Chalice = 0x0012fda4;

        // Cemetery Hill
        public const uint CH_Pickup_GoldCoinsNearBoulderEntrance = 0x0012e85c;
        public const uint CH_Pickup_EnergyVialNearShop = 0x0012fb1c;
        public const uint CH_Pickup_CopperShield1stOnHill = 0x0012f998;
        public const uint CH_Pickup_GoldCoinsUpHill1 = 0x0012f9dc;
        public const uint CH_Pickup_CopperShield2ndOnHill = 0x0012f898;
        public const uint CH_Pickup_GoldCoinsUpHill2 = 0x0012f91c;
        public const uint CH_Pickup_CopperShield3rdOnHill = 0x0012f8d8;
        public const uint CH_Pickup_GoldCoinsChestAtExit = 0x0012f95c;
        public const uint CH_Pickup_Club = 0x0012f098;
        public const uint CH_Pickup_WitchTalisman = 0x0012f65c;
        public const uint CH_Pickup_CopperShieldArena = 0x0012f35c;
        public const uint CH_Pickup_EnergyVialArena = 0x0012fbdc;
        public const uint CH_Pickup_GoldCoinsChestInArena = 0x0012f69c;
        public const uint CH_Book_Breakables = 0x00015607;
        public const uint CH_Book_Club = 0x0001567c;
        public const uint CH_Book_DestroyBoulder = 0x00015c45;
        public const uint CH_Book_AGuideToCovens = 0x00015858;
        public const uint CH_Book_HiddenLocations = 0x00015c94;
        public const uint CH_Gargoyle_WitchCave = 0x00015ace;
        public const uint CH_Pickup_Chalice = 0x0012f79c;

        // Hilltop Mosoleum
        public const uint HM_Pickup_EnergyVialRightCoffin = 0x0012e758;
        public const uint HM_Pickup_GoldCoinsLeftCoffin = 0x0012e718;
        public const uint HM_Pickup_ClubBrokenBenches = 0x0012f754;
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
        public const uint HM_Pickup_DaggersBlockPuzzle = 0x00130294;
        public const uint HM_Pickup_CopperShieldBlockPuzzle = 0x0012ed58;
        public const uint HM_Pickup_GlassDemonSkullKey = 0x0012f658; // Changed from "Boss" to "Pickup" for consistency
        public const uint HM_Pickup_GoldChestPhantomOfTheOpera1 = 0x0012f258;
        public const uint HM_Pickup_GoldChestPhantomOfTheOpera2 = 0x0012f218;
        public const uint HM_Pickup_GoldChestPhantomOfTheOpera3 = 0x0012f298;
        public const uint HM_Pickup_Chalice = 0x0012f1d8;
        public const uint HM_Book_GlassDemon = 0x0001d4c6;
        public const uint HM_Book_PhantomOfTheOpera = 0x0001d42f;
        public const uint HM_Book_DemonHeart = 0x0001d549;
        public const uint HM_Book_ThevingImps = 0x0001d3be;

        // Return to the Graveyard
        public const uint RTG_Pickup_GoldCoinsChestInCoffinArea1 = 0x0012ee38;
        public const uint RTG_Pickup_EnergyVialCoffinAreaWest = 0x00130ab8;
        public const uint RTG_Pickup_GoldCoinsChestInCoffinArea2 = 0x0012ed38;
        public const uint RTG_Pickup_GoldCoinsChestInCoffinArea3 = 0x0012ed78;
        public const uint RTG_Pickup_GoldCoinsChestInCoffinArea4 = 0x0012edf8;
        public const uint RTG_Pickup_EnergyVialCoffinAreaEast = 0x00130af8;
        public const uint RTG_Pickup_GoldCoinsChestInCoffinArea5 = 0x0012edb8;
        public const uint RTG_Pickup_GoldCoinsBagAboveCoffinArea = 0x00130a78;
        public const uint RTG_Pickup_GoldCoinsBagAfterBridge = 0x0012eff8;
        public const uint RTG_Pickup_EnergyVialBelowShop = 0x001303f8;
        public const uint RTG_Pickup_GoldCoinsBagAtShop = 0x00130b38;
        public const uint RTG_Pickup_SilverShieldChestAtShop = 0x0012eb74;
        public const uint RTG_Pickup_GoldCoinsBagAtClosedGate = 0x00130978;
        public const uint RTG_Pickup_StarRune = 0x0012ebb8;
        public const uint RTG_Pickup_GoldCoinsChestOnIsland = 0x00130cb8;
        public const uint RTG_Pickup_GoldCoinsUndertakersEntrance = 0x00130938;
        public const uint RTG_Pickup_EnergyVialUndertakersEntrance = 0x00130c78;
        public const uint RTG_Pickup_EnergyVialCliffsRight = 0x001303b8;
        public const uint RTG_Pickup_GoldCoinsCliffsLeft = 0x00130338;
        public const uint RTG_Pickup_EnergyVialCliffsLeft = 0x00130378;
        public const uint RTG_Pickup_DaringDash = 0x00019160;
        public const uint RTG_Book_SecretAreas = 0x00018db4;
        public const uint RTG_Book_SkullKey = 0x00018f80;
        public const uint RTG_Book_DaringDash = 0x0001901b;
        public const uint RTG_Gargoyle_Exit = 0x000f09e3;
        public const uint RTG_Pickup_Chalice = 0x001302f8;

        // Scarecrow Fields

        public const uint SF_Pickup_GoldCoinsHayStackAtBeginning = 0x0012f9f0;
        public const uint SF_Pickup_GoldCoinsChestInHayStackNearMoonDoor = 0x0012f9b0;
        public const uint SF_Pickup_GoldCoinsLeftOfFireNearMoonDoor = 0x00130124;
        public const uint SF_Pickup_EnergyVialRightOfFireNearMoonDoor = 0x001301a4;
        public const uint SF_Pickup_MoonRune = 0x0012fc24;
        public const uint SF_Pickup_ChaosRune = 0x0012fba4;
        public const uint SF_Pickup_EarthRune = 0x0012ffa4;
        public const uint SF_Pickup_ClubInsideHut = 0x00130d20;
        public const uint SF_Pickup_SilverShieldBehindWindmill = 0x00130de0;
        public const uint SF_Pickup_GoldCoinsBagInTheBarn = 0x0012f7a4;
        public const uint SF_Pickup_CopperShieldChestInTheBarn = 0x001308a0;
        public const uint SF_Pickup_GoldCoinsCornfieldSquareNearBarn = 0x00130da4;
        public const uint SF_Pickup_GoldCoinsCornfieldPath1 = 0x00130024;
        public const uint SF_Pickup_EnergyVialCornfieldPath = 0x0012ffe4;
        public const uint SF_Pickup_GoldCoinsChestUnderHayStack = 0x0012f870;
        public const uint SF_Pickup_GoldCoinsBagUnderBarnHayStack = 0x0012f830;
        public const uint SF_Pickup_GoldCoinsBagInThePress = 0x0012f5a4;
        public const uint SF_Pickup_GoldCoinsBagInTheSpinner = 0x0012f5e4;
        public const uint SF_Pickup_GoldCoinsChestNextToHarvesterPart = 0x0012f564;
        public const uint SF_Pickup_HarvesterPart = 0x0012f8a4;
        public const uint SF_Pickup_LifePotion = 0x00130aa4;
        public const uint SF_Pickup_GoldCoinsChestNextToChalice = 0x00130ae4;
        public const uint SF_Book_Scarecrows = 0x0001ca3f;
        public const uint SF_Book_MischiefMakers = 0x0001cab9;
        public const uint SF_Book_KulKatura = 0x0001cd70;
        public const uint SF_Book_Cornfields = 0x0001cb6a;
        public const uint SF_Book_MadMachines = 0x0001cbd0;
        public const uint SF_Book_CornCutter = 0x0001cc48;
        public const uint SF_Gargoyle_Exit = 0x0001cd01;
        public const uint SF_Pickup_Chalice = 0x0012f3e4;

        // The Anthill
        public const uint TA_Pickup_ClubChestAtBarrier = 0x001309e8;
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
        public const uint TA_Pickup_EnergyVialBeforeFairy1 = 0x00130aac;
        public const uint TA_Pickup_EnergyVialAfterAmber2 = 0x00130aec;
        public const uint TA_Pickup_EnergyVialFairy2RoomCenter = 0x00130b2c;
        public const uint TA_Pickup_EnergyVialFairy3 = 0x00130b6c;
        public const uint TA_Pickup_EnergyVialBirthingRoomExit = 0x00130a6c;
        public const uint AH_Book_FairyPortal = 0x0001c82e;
        public const uint AH_Gargoyle_Entrance = 0x0001c799;
        public const uint AH_Book_QueenAnt = 0x0001c8c2;

        // Enchanted Earth
        public const uint EE_Pickup_GoldCoinsBagNearTreeHollow = 0x00130638;
        public const uint EE_Pickup_GoldCoinsBagBehindBigTree1 = 0x001304b8;
        public const uint EE_Pickup_GoldCoinsBagBehindBigTree2 = 0x001304f8;
        public const uint EE_Pickup_EarthRune = 0x00130478;
        public const uint EE_Pickup_GoldCoinsChestInEgg = 0x00130438;
        public const uint EE_Pickup_CopperShieldInEgg = 0x001303f8;
        public const uint EE_Pickup_GoldCoinsBagAtCaveEntrance = 0x00130678;
        public const uint EE_Pickup_ShadowTalisman = 0x0012fd38;
        public const uint EE_Pickup_EnergyVialShadowTalismanCave = 0x00130538;
        public const uint EE_Pickup_GoldCoinsBagInShadowTalismanCave = 0x00130538;
        public const uint EE_Pickup_GoldCoinsChestNearBarrier = 0x00130038;
        public const uint EE_Pickup_StarRune = 0x0012f855;
        public const uint EE_Pickup_GoldCoinsChestLeftOfFountain = 0x0012fef8;
        public const uint EE_Pickup_GoldCoinsChestTopOfFountain = 0x0012feb8;
        public const uint EE_Pickup_GoldCoinsChestRightOfFountain = 0x0012ff38;
        public const uint EE_Pickup_EnergyVialLeftOfTreeDrop = 0x001305b8;
        public const uint EE_Pickup_EnergyVialRightOfTreeDrop = 0x001305f8;
        public const uint EE_Book_CovenOfWitches = 0x0001de33;
        public const uint EE_Book_DragonBird = 0x0001e427;
        // public const uint EE_Book_KeepOut = ??????????; -  Note: "Keep Out" has no hexadecimal value provided.
        public const uint EE_Book_TakeTheTalisman = 0x0001dc53;
        public const uint EE_Gargoyle_OutsideDemonEntrance = 0x0001df3d;
        public const uint EE_Gargoyle_OutsideDemonExit = 0x0001df9c;
        public const uint EE_Pickup_Chalice = 0x0012fd78;

        // The Sleeping Village
        public const uint TSV_Pickup_GoldCoinsBagInLeftBarrelAtBlacksmith = 0x0012fc6c;
        public const uint TSV_Pickup_GoldCoinsBagInRightBarrelAtBlacksmith = 0x0012fcec;
        public const uint TSV_Pickup_SilverShieldInBlacksmiths = 0x00131e2c;
        public const uint TSV_Pickup_GoldCoinsBagAtPond = 0x00131cb0;
        public const uint TSV_Pickup_EnergyVialAtPond = 0x00131530;
        public const uint TSV_Pickup_MoonRune = 0x0012f970;
        public const uint TSV_Pickup_GoldCoinsBagInBarrelAtInn = 0x0012ff6c;
        public const uint TSV_Pickup_GoldCoinsBagInBarrelAtBottomOfInnStairs = 0x0013126c;
        public const uint TSV_Pickup_GoldCoinsBagInBarrelBehindInnStairs = 0x0013122c;
        public const uint TSV_Pickup_ClubInChestUnderInnStairs = 0x001314ac;
        public const uint TSV_Pickup_EarthRune = 0x00131c30;
        public const uint TSV_Pickup_EnergyVialBustSwitch = 0x001315f0;
        public const uint TSV_Pickup_GoldCoinsBagInTopBustBarrel = 0x001310ac;
        public const uint TSV_Pickup_GoldCoinsBagInSwitchBustBarrel = 0x0013102c;
        public const uint TSV_Pickup_LandlordsBust = 0x0012f670;
        public const uint TSV_Pickup_ChaosRune = 0x0012e8b0;
        public const uint TSV_Pickup_GoldCoinsBagInLibrary = 0x001319b0;
        public const uint TSV_Pickup_CrucifixCast = 0x0012f7f0;
        public const uint TSV_Pickup_Crucifix = 0x0015d682;
        public const uint TSV_Pickup_SafeKey = 0x0012f6f0;
        public const uint TSV_Pickup_ShadowArtefact = 0x001308f0;
        public const uint TSV_Pickup_GoldCoinsBagAtTopOfTable = 0x00130df0;
        public const uint TSV_Pickup_GoldCoinsBagAtBottomOfTable = 0x00131a30;
        public const uint TSV_Pickup_GoldCoinsChestNextToChalice = 0x001319f0;
        public const uint TSV_Pickup_EnergyVialNearExit = 0x00131630;
        public const uint TSV_Pickup_EnergyVialNearChalice = 0x001318f0;
        public const uint TSV_Book_BlacksmithsMontly = 0x01ca8c;
        public const uint TSV_Book_MissingCrucifix = 0x01a5b6;
        public const uint TSV_Book_FountainRune = 0x01a638;
        public const uint TSV_Book_MayorsBust = 0x01a8d3;
        public const uint TSV_Book_HistoryOfGallowmere1 = 0x01aae1;
        public const uint TSV_Book_HistoryOfGallowmere2 = 0x01ace8;
        public const uint TSV_Book_HistoryOfGallowmere3 = 0x01b065;
        public const uint TSV_Book_HistoryOfGallowmere4 = 0x01b377;
        public const uint TSV_Book_HeroesFromHistory = 0x01bde6;
        public const uint TSV_Book_TouristGuide1 = 0x01b914;
        public const uint TSV_Book_TouristGuide2 = 0x01ba79;
        public const uint TSV_Book_MayorMemoire = 0x01a795;
        public const uint TSV_Book_MayorsRegrets = 0x01b655;
        public const uint TSV_Book_ZaroksNote = 0x01a6de;
        public const uint TSV_Gargoyle_Entrance = 0x01a529;
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
        public const uint PAD_Pickup_SilverShieldInChestNearSoul5 = 0x0012f8c0;
        public const uint PAD_Pickup_EnergyVialChariotRight = 0x0012fa84;
        public const uint PAD_Pickup_EnergyVialChariotLeft = 0x0012fac4;
        public const uint PAD_Pickup_EnergyVialJumpSpot1 = 0x0012fd84;
        public const uint PAD_Pickup_EnergyVialJumpSpot2 = 0x0012fdc4;
        public const uint PAD_Pickup_GoldCoinsJumpSpot1 = 0x0012f944;
        public const uint PAD_Pickup_GoldCoinsJumpSpot2 = 0x0012f904;
        public const uint PAD_Pickup_LifeBottle = 0x0012f0c4;
        public const uint PAD_Book_EnemyWarning = 0x00015b3d;
        public const uint PAD_Gargoyle_Entrance = 0x00015a34;
        public const uint PAD_Pickup_Chalice = 0x0012f7c4;

        // The Lake
        public const uint TL_Pickup_EnergyVialFloodedHouse = 0x001319d8;
        public const uint TL_Pickup_ChaosRune = 0x0012ef18;
        public const uint TL_Pickup_GoldCoinsBagOutsideFloodedHouse = 0x0012f658;
        public const uint TL_Pickup_EarthRune = 0x0012f618;
        public const uint TL_Pickup_GoldCoinsBagNearClosedGate = 0x00131a58;
        public const uint TL_Pickup_TimeRune = 0x0012f7d8;
        public const uint TL_Pickup_SilverShieldInWhirlpool = 0x00131c14;
        public const uint TL_Pickup_GoldCoinsBagAtTheWhirlpoolEntrance = 0x00131ad8;
        public const uint TL_Pickup_EnergyVialWhirlpoolWind1 = 0x00131b98;
        public const uint TL_Pickup_EnergyVialWhirlpoolWind2 = 0x00131bd8;
        public const uint TL_Pickup_GoldCoinsWhirlpoolWind1 = 0x00131b18;
        public const uint TL_Pickup_GoldCoinsWhirlpoolWind2 = 0x00131b58;
        public const uint TL_Pickup_GoldCoinsOutsideWhirlpoolExit = 0x00131a98;
        public const uint TL_Pickup_GoldChestWhirlpoolSwitchArea = 0x0012f798;
        public const uint TL_Pickup_StarRune = 0x001311d8;
        public const uint TL_Book_LearnToStealth = 0x000175ee;
        public const uint TL_Book_WhirlpoolManual = 0x0001781b;
        public const uint TL_Gargoyle_Exit = 0x00017704;
        public const uint TL_Pickup_Chalice = 0x00131218;

        // The Crystal Caves
        public const uint CC_Pickup_GoldCoinsBagAtWindingStaircase = 0x0012fc70;
        public const uint CC_Pickup_GoldCoinsBagInCrystalAtStart = 0x0012e5d2;
        public const uint CC_Pickup_GoldCoinsBagInSpinner = 0x0012e870;
        public const uint CC_Pickup_EarthRune = 0x0012e8b0;
        public const uint CC_Pickup_SilverShieldInCrystal = 0x0012e7cc;
        public const uint CC_Pickup_GoldCoinsBagNearSilverShield = 0x0012fc30;
        public const uint CC_Pickup_GoldCoinsChestInCrystalAfterEarthDoor = 0x0012ec0c;
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
        public const uint CC_Pickup_GoldCoinsChestInCrystalAfterPool = 0x0012e64e;
        public const uint CC_Pickup_DragonArmour = 0x0012f5f0;
        public const uint CC_Book_DragonBook = 0x000193cc;
        public const uint CC_Book_SummonDragon = 0x0001941f;
        public const uint CC_Gargoyle_CaveEntrance = 0x000191dd;
        public const uint CC_Pickup_Chalice = 0x0012eff0;

        // The Gallows Gauntlet
        public const uint GG_Pickup_GoldCoinsBagBehindStoneDragon1 = 0x0012f2ac;
        public const uint GG_Pickup_GoldCoinsBagBehindStoneDragon2 = 0x0012f2ec;
        public const uint GG_Pickup_SilverShieldInChestNearExit = 0x0012e3e8;
        public const uint GG_Pickup_GoldCoinsChestAtSerpent = 0x0012f0ac;
        public const uint GG_Pickup_StarRune = 0x0012ecec;
        public const uint GG_Pickup_GoldCoinsChestNearStarEntrance = 0x0012f0ec;
        public const uint GG_Pickup_EnergyVialNearChalice = 0x0012f12c;
        public const uint GG_Book_SerpentOfGallowmere = 0x00012f9d;
        public const uint GG_Book_DragonArmour = 0x00013223;
        public const uint GG_Book_EarlyExit = 0x0001318a;
        public const uint GG_Book_MagicalBarrier = 0x000130e0;
        public const uint GG_Pickup_Chalice = 0x0012edac;

        // Asylumn Grounds
        public const uint AG_Pickup_GoldCoinsBagInBellGraveNearBell = 0x0012fb40;
        public const uint AG_Pickup_GoldCoinsBagInBellGraveNearEntrance = 0x0012fb80;
        public const uint AG_Pickup_GoldCoinsBagNearShootingStatue = 0x0012fa80;
        public const uint AG_Pickup_SilverShieldInChestBehindDoor = 0x0012fc7c;
        public const uint AG_Pickup_GoldCoinsBagInRatGrave = 0x0012fbc0;
        public const uint AG_Pickup_ChaosRune = 0x0012f400;
        public const uint AG_Pickup_GoldCoinsBehindChaosGate = 0x0012fb00;
        public const uint AG_Pickup_GoldCoinsBehindElephantInGrave = 0x0012fc00;
        public const uint AG_Pickup_EnergyVialNearBishop = 0x0012fd00;
        public const uint AG_Pickup_EnergyVialNearKing = 0x0012fcc0;
        public const uint AG_Book_SeekJack = 0x019304;
        public const uint AG_Book_SecretExit = 0x0193cb;
        public const uint AG_Gargoyle_JackOfTheGreen = 0x018ca5;
        public const uint AG_Pickup_Chalice = 0x0012fa00;

        // Inside the Aslyum
        public const uint IA_MonsterKills = 0x000ee9b4;
        public const uint IA_Pickup_EnergyVialBatRoom = 0x0012F1A8;
        public const uint IA_Pickup_GoldCoinsBagInBatRoomLeft = 0x0012F068;
        public const uint IA_Pickup_GoldCoinsChestInBatRoom = 0x0012F0A8;
        public const uint IA_Pickup_GoldCoinsBagInBatRoomCentre = 0x0012F0E8;
        public const uint IA_Pickup_SilverShieldInBatRoom = 0x0012F168;
        public const uint IA_Pickup_GoldCoinsBagInBatRoomRight = 0x0012F168;
        public const uint IA_Pickup_EnergyVialAsylumRoom1 = 0x0012F1E8;
        public const uint IA_Pickup_EnergyVialAsylumRoom2 = 0x0012F228;
        public const uint IA_Pickup_GoldCoinsBagInAsylumRoom = 0x0012F268;
        public const uint IA_Pickup_GoldCoinsBagInSewerPrisonEntrance = 0x0012f2e4;
        public const uint IA_Pickup_GoldCoinsBagOnSewerPrisonBench = 0x0012f324;
        public const uint IA_Pickup_EarthRune = 0x0012efa4;
        public const uint IA_Pickup_DragonGem = 0x0012f8a4;
        public const uint IA_Pickup_Chalice = 0x0012f724;

        // Pumpkin Gorge
        public const uint PG_Pickup_MoonRune = 0x0012de48;
        public const uint PG_Pickup_ClubInChestInTunnel = 0x00131a84;
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
        public const uint PG_Pickup_SilverShieldInChestAtTopOfHill = 0x00132c44;
        public const uint PG_Pickup_TimeRune = 0x000ee974;
        public const uint PG_Pickup_EnergyVialBouldersAfterStarRune = 0x00132c88;
        public const uint PG_Pickup_GoldCoinsChestAtBouldersAfterStarRune = 0x00132cc8;
        public const uint PG_Pickup_EnergyVialVinePatchLeft = 0x00132c08;
        public const uint PG_Pickup_EnergyVialVinePatchRight = 0x00132748;
        public const uint PG_Pickup_GoldCoinsChestNearChalice = 0x0012f4c8;
        public const uint PG_Pickup_EnergyVialChalicePath = 0x00132d08;
        public const uint PG_Book_Mushrooms = 0x0001946c;
        public const uint PG_Gargoyle_Exit = 0x000193dd;
        public const uint PG_Pickup_Chalice = 0x0012f108;


        // Pumpkin Servent
        public const uint PS_Pickup_GoldCoinsBagBehindHouse1 = 0x001303b4;
        public const uint PS_Pickup_GoldCoinsBagBehindHouse2 = 0x001304f4;
        public const uint PS_Pickup_SilverShieldInChestNearLeeches = 0x00130570;
        public const uint PS_Pickup_GoldCoinsBagBehindVinesAndPod = 0x001303f4;
        public const uint PS_Pickup_DragonsGem = 0x0012fd34;
        public const uint PS_Pickup_EnergyVialLeftAtMerchantGargoyle = 0x00130474;
        public const uint PS_Pickup_GoldCoinsChestAtMerchantGargoyle = 0x00130374;
        public const uint PS_Pickup_EnergyVialRightAtMerchantGargoyle = 0x00130434;
        public const uint PS_Book_PumpkinKing = 0x0001c709;
        public const uint PS_Book_PumpkinWitch = 0x0001cafc;
        public const uint PS_Pickup_Chalice = 0x0012e974;

        // The Haunted Ruins
        public const uint HR_Pickup_ChaosRune = 0x0012f818;
        public const uint HR_Pickup_GoldCoinsNearFirstSetOfFarmers = 0x00130958;
        public const uint HR_Pickup_EnergyVialAboveRune = 0x00130a18;
        public const uint HR_Pickup_EnergyVialCornerOfWalls1 = 0x00130a58;
        public const uint HR_Pickup_SilverShieldInChestNearRuneDoor = 0x00130994;
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
        public const uint HR_Book_Chickens = 0x0001aec0;
        public const uint HR_Book_Farmers = 0x0001afc1;
        public const uint HR_Book_SadKing = 0x0001b157;
        public const uint HR_Book_GhostKing = 0x0001b157;
        public const uint HR_Book_TheVolcano = 0x0001b1fb;
        public const uint HR_Book_Escape = 0x0001b299;
        public const uint HR_Book_Oil = 0x0001ba57;
        public const uint HR_Gargoyle_Drawbridge = 0x0001b6a7;
        public const uint HR_Gargoyle_SteelGates = 0x0001b08f;
        public const uint HR_Pickup_Chalice = 0x0012f918;

        // The Ghost Ship
        public const uint GS_Pickup_MoonRune = 0x0012ed64;
        public const uint GS_Pickup_GoldCoinsBagInRollingBarrelsRoom = 0x0012fde4;
        public const uint GS_Pickup_SilverShieldInChestInBarrelRoom = 0x00130160;
        public const uint GS_Pickup_GoldCoinsBagOnDeckAtBarrels = 0x0012f264;
        public const uint GS_Pickup_EnergyVialInCabin = 0x00130124;
        public const uint GS_Pickup_StarRune = 0x0012ede4;
        public const uint GS_Pickup_ChaosRune = 0x0012ee64;
        public const uint GS_Pickup_EnergyVialInCannonRoom = 0x00130224;
        public const uint GS_Pickup_GoldCoinsChestInCannonRoom = 0x0012fe24;
        public const uint GS_Pickup_EnergyVialRopeBridge1 = 0x001301e4;
        public const uint GS_Pickup_EnergyVialRopeBridge2 = 0x001301a4;
        public const uint GS_Pickup_GoldCoinsRopeBridge = 0x0012fe64;
        public const uint GS_Pickup_ClubInChestAtCaptain = 0x0012fea0;
        public const uint GS_Pickup_EnergyVialCageLift1 = 0x0012f764;
        public const uint GS_Pickup_EnergyVialCageLift2 = 0x00130264;
        public const uint GS_Book_SkeletonWarriors = 0x0001756e;
        public const uint GS_Book_BossStrategy = 0x000175f2;

        public const uint GS_Pickup_Chalice = 0x0012ece4;

        // The Entrance Hall
        public const uint EH_Pickup_Chalice = 0x0012e09c;
        public const uint EH_Book_ImpMagic = 0x000127a4;
        public const uint EH_Book_SpellBook = 0x0001280a;
        public const uint EH_Book_ZaroksDiary = 0x00012946;
        public const uint EH_Gargoyle_Entrance = 0x000126e3;

        // The Time Device

        public const uint TD_Pickup_SilverShieldOnClock = 0x001306d4;
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
        public const uint TD_Book_TheTrain = 0x0001cc03;
        public const uint TD_Gargoyle_Entrance = 0x0001cb62;
        public const uint TD_Pickup_Chalice = 0x0012ee58;


        // Zaroks Lair

        public const uint ZL_Pickup_GoodLightning = 0x0012ED38;
        public const uint ZL_Pickup_SilverShield = 0x0012EDF8;
        public const uint ZL_Gargoyle_Entrance = 0x0019f26e;
    }
}
