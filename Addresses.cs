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
        public const uint TheGraveyardLevelStatus = 0x000f81e1;
        public const uint ReturnToTheGraveyardLevelStatus = 0x000F81E2;
        public const uint CemeteryHillLevelStatus = 0x000F81E3;
        public const uint TheHilltopMausoleumLevelStatus = 0x000F81E4;
        public const uint ScarecrowFieldsLevelStatus = 0x000F81E5;
        public const uint DansCryptLevelStatus = 0x000F81E6;
        public const uint AntHillLevelStatus = 0x000F81E7;
        public const uint TheCrystalCavesLevelStatus = 0x000F81E8;
        public const uint PumpkinGorgeLevelStatus = 0x000F81E9;
        public const uint PumpkinSerpentLevelStatus = 0x000F81EA;
        public const uint SleepingVillageLevelStatus = 0x000F81EB;
        public const uint PoolsOfTheAncientDeadLevelStatus = 0x000F81EC;
        public const uint AsylumGroundsLevelStatus = 0x000F81ED;
        public const uint InsideTheAsylumLevelStatus = 0x000F81EE;
        public const uint EnchantedEarthLevelStatus = 0x000F81EF;
        public const uint TheGallowsGauntletLevelStatus = 0x000F81F0;
        public const uint TheHauntedRuinsLevelStatus = 0x000F81F1;
        public const uint GhostShipLevelStatus = 0x000F81F3;
        public const uint TheEntranceHallLevelStatus = 0x000F81F4;
        public const uint TheTimeDeviceLevelStatus = 0x000F81F5;
        public const uint TheLakeLevelStatus = 0x000F81F6;

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


        // Killing Zarok Win Condition
        public const uint ZaroksLairLevelStatus = 0x000FF0FF; // not correct. needs actually set to a "finish"

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

    }
}
