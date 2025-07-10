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

        public const uint DaggersAmmo = 0x000f8240;
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
