using Archipelago.Core.Models;
using Archipelago.Core.Util;
using Archipelago.MultiClient.Net.Models;
using MedievilArchipelago;
using MedievilArchipelago.Models;
using Newtonsoft.Json;
using Serilog;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.InteropServices.JavaScript;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Location = Archipelago.Core.Models.Location;

namespace MedievilArchipelago
{
    public class Helpers
    {
        public static Dictionary<string, Tuple<int, uint, uint>> GetLevelCompleteStatuses()
        {
            return new Dictionary<string, Tuple<int, uint, uint>>
            {
               {"Cleared: Dan's Crypt", new Tuple<int, uint, uint>(Memory.ReadInt(Addresses.DansCryptLevelStatus), Addresses.DansCryptLevelStatus, Addresses.FakeAddress)},
               {"Cleared: The Graveyard", new Tuple<int, uint, uint>(Memory.ReadInt(Addresses.TheGraveyardLevelStatus), Addresses.TheGraveyardLevelStatus, Addresses.FakeAddress)},
               {"Cleared: Return to the Graveyard", new Tuple<int, uint, uint>(Memory.ReadInt(Addresses.ReturnToTheGraveyardLevelStatus), Addresses.ReturnToTheGraveyardLevelStatus, Addresses.FakeAddress)},
               {"Cleared: Cemetery Hill", new Tuple<int, uint, uint>(Memory.ReadInt(Addresses.CemeteryHillLevelStatus), Addresses.CemeteryHillLevelStatus, Addresses.FakeAddress)},
               {"Cleared: The Hilltop Mausoleum", new Tuple<int, uint, uint>(Memory.ReadInt(Addresses.TheHilltopMausoleumLevelStatus), Addresses.TheHilltopMausoleumLevelStatus, Addresses.FakeAddress)},
               {"Cleared: Scarecrow Fields", new Tuple<int, uint, uint>(Memory.ReadInt(Addresses.ScarecrowFieldsLevelStatus), Addresses.ScarecrowFieldsLevelStatus, Addresses.FakeAddress)},
               {"Cleared: Ant Hill", new Tuple<int, uint, uint>(Memory.ReadInt(Addresses.AntHillLevelStatus), Addresses.AntHillLevelStatus, Addresses.FakeAddress)},
               {"Cleared: The Crystal Caves", new Tuple<int, uint, uint>(Memory.ReadInt(Addresses.TheCrystalCavesLevelStatus), Addresses.TheCrystalCavesLevelStatus, Addresses.FakeAddress)},
               {"Cleared: The Lake", new Tuple<int, uint, uint>(Memory.ReadInt(Addresses.TheLakeLevelStatus), Addresses.TheLakeLevelStatus, Addresses.FakeAddress)},
               {"Cleared: Pumpkin Gorge", new Tuple<int, uint, uint>(Memory.ReadInt(Addresses.PumpkinGorgeLevelStatus), Addresses.PumpkinGorgeLevelStatus, Addresses.FakeAddress)},
               {"Cleared: Pumpkin Serpent", new Tuple<int, uint, uint>(Memory.ReadInt(Addresses.PumpkinSerpentLevelStatus), Addresses.PumpkinSerpentLevelStatus, Addresses.FakeAddress)},
               {"Cleared: Sleeping Village", new Tuple<int, uint, uint>(Memory.ReadInt(Addresses.SleepingVillageLevelStatus), Addresses.SleepingVillageLevelStatus, Addresses.FakeAddress)},
               {"Cleared: Pools of the Ancient Dead", new Tuple<int, uint, uint>(Memory.ReadInt(Addresses.PoolsOfTheAncientDeadLevelStatus), Addresses.PoolsOfTheAncientDeadLevelStatus, Addresses.FakeAddress)},
               {"Cleared: Asylum Grounds", new Tuple<int, uint, uint>(Memory.ReadInt(Addresses.AsylumGroundsLevelStatus), Addresses.AsylumGroundsLevelStatus, Addresses.FakeAddress)},
               {"Cleared: Inside the Asylum", new Tuple<int, uint, uint>(Memory.ReadInt(Addresses.InsideTheAsylumLevelStatus), Addresses.InsideTheAsylumLevelStatus, Addresses.FakeAddress)},
               {"Cleared: Enchanted Earth", new Tuple<int, uint, uint>(Memory.ReadInt(Addresses.EnchantedEarthLevelStatus), Addresses.EnchantedEarthLevelStatus, Addresses.FakeAddress)},
               {"Cleared: The Gallows Gauntlet", new Tuple<int, uint, uint>(Memory.ReadInt(Addresses.TheGallowsGauntletLevelStatus), Addresses.TheGallowsGauntletLevelStatus, Addresses.FakeAddress)},
               {"Cleared: The Haunted Ruins", new Tuple<int, uint, uint>(Memory.ReadInt(Addresses.TheHauntedRuinsLevelStatus), Addresses.TheHauntedRuinsLevelStatus, Addresses.FakeAddress)},
               {"Cleared: Ghost Ship", new Tuple<int, uint, uint>(Memory.ReadInt(Addresses.GhostShipLevelStatus), Addresses.GhostShipLevelStatus, Addresses.FakeAddress)},
               {"Cleared: The Entrance Hall", new Tuple<int, uint, uint>(Memory.ReadInt(Addresses.TheEntranceHallLevelStatus), Addresses.TheEntranceHallLevelStatus, Addresses.FakeAddress)},
               {"Cleared: The Time Device", new Tuple<int, uint, uint>(Memory.ReadInt(Addresses.TheTimeDeviceLevelStatus), Addresses.TheTimeDeviceLevelStatus, Addresses.FakeAddress)},
               {"Cleared: Zaroks Lair", new Tuple<int, uint, uint>(Memory.ReadInt(Addresses.ZaroksLairLevelStatus), Addresses.ZaroksLairLevelStatus, Addresses.FakeAddress)},
            };
        }
        public static Dictionary<string, Tuple<int, uint, uint>> GetChaliceLocationStatuses()
        {
            return new Dictionary<string, Tuple<int, uint, uint>>
            {
               {"Chalice: The Graveyard", new Tuple<int, uint, uint>(Memory.ReadInt(Addresses.TheGraveyardLevelStatus), Addresses.TheGraveyardLevelStatus, Addresses.FakeAddress)},
               {"Chalice: Return to the Graveyard", new Tuple<int, uint, uint>(Memory.ReadInt(Addresses.ReturnToTheGraveyardLevelStatus), Addresses.ReturnToTheGraveyardLevelStatus, Addresses.FakeAddress)},
               {"Chalice: Cemetery Hill", new Tuple<int, uint, uint>(Memory.ReadInt(Addresses.CemeteryHillLevelStatus), Addresses.CemeteryHillLevelStatus, Addresses.FakeAddress)},
               {"Chalice: The Hilltop Mausoleum", new Tuple<int, uint, uint>(Memory.ReadInt(Addresses.TheHilltopMausoleumLevelStatus), Addresses.TheHilltopMausoleumLevelStatus, Addresses.FakeAddress)},
               {"Chalice: Scarecrow Fields", new Tuple<int, uint, uint>(Memory.ReadInt(Addresses.ScarecrowFieldsLevelStatus), Addresses.ScarecrowFieldsLevelStatus, Addresses.FakeAddress)},
               {"Chalice: Ant Hill", new Tuple<int, uint, uint>(Memory.ReadInt(Addresses.AntHillLevelStatus), Addresses.AntHillLevelStatus, Addresses.FakeAddress)},
               {"Chalice: The Crystal Caves", new Tuple<int, uint, uint>(Memory.ReadInt(Addresses.TheCrystalCavesLevelStatus), Addresses.TheCrystalCavesLevelStatus, Addresses.FakeAddress)},
               {"Chalice: The Lake", new Tuple<int, uint, uint>(Memory.ReadInt(Addresses.TheLakeLevelStatus), Addresses.TheLakeLevelStatus, Addresses.FakeAddress)},
               {"Chalice: Pumpkin Gorge", new Tuple<int, uint, uint>(Memory.ReadInt(Addresses.PumpkinGorgeLevelStatus), Addresses.PumpkinGorgeLevelStatus, Addresses.FakeAddress)},
               {"Chalice: Pumpkin Serpent", new Tuple<int, uint, uint>(Memory.ReadInt(Addresses.PumpkinSerpentLevelStatus), Addresses.PumpkinSerpentLevelStatus, Addresses.FakeAddress)},
               {"Chalice: Sleeping Village", new Tuple<int, uint, uint>(Memory.ReadInt(Addresses.SleepingVillageLevelStatus), Addresses.SleepingVillageLevelStatus, Addresses.FakeAddress)},
               {"Chalice: Pools of the Ancient Dead", new Tuple<int, uint, uint>(Memory.ReadInt(Addresses.PoolsOfTheAncientDeadLevelStatus), Addresses.PoolsOfTheAncientDeadLevelStatus, Addresses.FakeAddress)},
               {"Chalice: Asylum Grounds", new Tuple<int, uint, uint>(Memory.ReadInt(Addresses.AsylumGroundsLevelStatus), Addresses.AsylumGroundsLevelStatus, Addresses.FakeAddress)},
               {"Chalice: Inside the Asylum", new Tuple<int, uint, uint>(Memory.ReadInt(Addresses.InsideTheAsylumLevelStatus), Addresses.InsideTheAsylumLevelStatus, Addresses.FakeAddress)},
               {"Chalice: Enchanted Earth", new Tuple<int, uint, uint>(Memory.ReadInt(Addresses.EnchantedEarthLevelStatus), Addresses.EnchantedEarthLevelStatus, Addresses.FakeAddress)},
               {"Chalice: The Gallows Gauntlet", new Tuple<int, uint, uint>(Memory.ReadInt(Addresses.TheGallowsGauntletLevelStatus), Addresses.TheGallowsGauntletLevelStatus, Addresses.FakeAddress)},
               {"Chalice: The Haunted Ruins", new Tuple<int, uint, uint>(Memory.ReadInt(Addresses.TheHauntedRuinsLevelStatus), Addresses.TheHauntedRuinsLevelStatus, Addresses.FakeAddress)},
               {"Chalice: Ghost Ship", new Tuple<int, uint, uint>(Memory.ReadInt(Addresses.GhostShipLevelStatus), Addresses.GhostShipLevelStatus, Addresses.FakeAddress)},
               {"Chalice: The Entrance Hall", new Tuple<int, uint, uint>(Memory.ReadInt(Addresses.TheEntranceHallLevelStatus), Addresses.TheEntranceHallLevelStatus, Addresses.FakeAddress)},
               {"Chalice: The Time Device", new Tuple<int, uint, uint>(Memory.ReadInt(Addresses.TheTimeDeviceLevelStatus), Addresses.TheTimeDeviceLevelStatus, Addresses.FakeAddress)},
            };
        }

        public static Dictionary<string, Tuple<int, uint, uint>> GetKeyItemStatuses()
        {
            return new Dictionary<string, Tuple<int, uint, uint>>
            {
                {"Key Item: Dragon Gem: Pumpkin Gorge",new Tuple<int, uint, uint>(Memory.ReadInt(Addresses.DragonGem), Addresses.DragonGem, Addresses.FakeAddress)},
                {"Key Item: Dragon Gem: Inside the Asylum",new Tuple<int, uint, uint>(Memory.ReadInt(Addresses.DragonGem), Addresses.DragonGem, Addresses.FakeAddress)},
                {"Key Item: King Peregrine's Crown",new Tuple<int, uint, uint>(Memory.ReadInt(Addresses.KingPeregrinesCrown), Addresses.KingPeregrinesCrown, Addresses.FakeAddress)}, // not sure about this
                {"Key Item: Soul Helmet 1",new Tuple<int, uint, uint>(Memory.ReadInt(Addresses.SoulHelmet), Addresses.SoulHelmet, Addresses.FakeAddress)},
                {"Key Item: Soul Helmet 2",new Tuple<int, uint, uint>(Memory.ReadInt(Addresses.SoulHelmet), Addresses.SoulHelmet, Addresses.FakeAddress)},
                {"Key Item: Soul Helmet 3",new Tuple<int, uint, uint>(Memory.ReadInt(Addresses.SoulHelmet), Addresses.SoulHelmet, Addresses.FakeAddress)},
                {"Key Item: Soul Helmet 4",new Tuple<int, uint, uint>(Memory.ReadInt(Addresses.SoulHelmet), Addresses.SoulHelmet, Addresses.FakeAddress)},
                {"Key Item: Soul Helmet 5",new Tuple<int, uint, uint>(Memory.ReadInt(Addresses.SoulHelmet), Addresses.SoulHelmet, Addresses.FakeAddress)},
                {"Key Item: Soul Helmet 6",new Tuple<int, uint, uint>(Memory.ReadInt(Addresses.SoulHelmet), Addresses.SoulHelmet, Addresses.FakeAddress)},
                {"Key Item: Soul Helmet 7",new Tuple<int, uint, uint>(Memory.ReadInt(Addresses.SoulHelmet), Addresses.SoulHelmet, Addresses.FakeAddress)},
                {"Key Item: Soul Helmet 8",new Tuple<int, uint, uint>(Memory.ReadInt(Addresses.SoulHelmet), Addresses.SoulHelmet, Addresses.FakeAddress)},
                {"Key Item: Witches Talisman",new Tuple<int, uint, uint>(Memory.ReadInt(Addresses.WitchesTalisman), Addresses.WitchesTalisman, Addresses.FakeAddress)},
                {"Key Item: Safe Key",new Tuple<int, uint, uint>(Memory.ReadInt(Addresses.SafeKey), Addresses.SafeKey, Addresses.FakeAddress)},
                {"Key Item: Shadow Artefact",new Tuple<int, uint, uint>(Memory.ReadInt(Addresses.ShadowArtefact), Addresses.ShadowArtefact, Addresses.FakeAddress)},
                {"Key Item: Crucifix",new Tuple<int, uint, uint>(Memory.ReadInt(Addresses.Crucifix), Addresses.Crucifix, Addresses.FakeAddress)},
                {"Key Item: Landlords Bust",new Tuple<int, uint, uint>(Memory.ReadInt(Addresses.LandlordsBust), Addresses.LandlordsBust, Addresses.FakeAddress)},
                {"Key Item: Crucifix Cast",new Tuple<int, uint, uint>(Memory.ReadInt(Addresses.CrucifixCast), Addresses.CrucifixCast, Addresses.FakeAddress)},
                {"Key Item: Amber Piece 1",new Tuple<int, uint, uint>(Memory.ReadInt(Addresses.AmberPiece), Addresses.AmberPiece, Addresses.FakeAddress)},
                {"Key Item: Amber Piece 2",new Tuple<int, uint, uint>(Memory.ReadInt(Addresses.AmberPiece), Addresses.AmberPiece, Addresses.FakeAddress)},
                {"Key Item: Amber Piece 3",new Tuple<int, uint, uint>(Memory.ReadInt(Addresses.AmberPiece), Addresses.AmberPiece, Addresses.FakeAddress)},
                {"Key Item: Amber Piece 4",new Tuple<int, uint, uint>(Memory.ReadInt(Addresses.AmberPiece), Addresses.AmberPiece, Addresses.FakeAddress)},
                {"Key Item: Amber Piece 5",new Tuple<int, uint, uint>(Memory.ReadInt(Addresses.AmberPiece), Addresses.AmberPiece, Addresses.FakeAddress)},
                {"Key Item: Amber Piece 6",new Tuple<int, uint, uint>(Memory.ReadInt(Addresses.AmberPiece), Addresses.AmberPiece, Addresses.FakeAddress)},
                {"Key Item: Amber Piece 7",new Tuple<int, uint, uint>(Memory.ReadInt(Addresses.AmberPiece), Addresses.AmberPiece, Addresses.FakeAddress)},
                {"Key Item: Harvester Parts",new Tuple<int, uint, uint>(Memory.ReadInt(Addresses.HarvesterParts), Addresses.HarvesterParts, Addresses.FakeAddress)},
                {"Key Item: Skull Key",new Tuple<int, uint, uint>(Memory.ReadInt(Addresses.SkullKey), Addresses.SkullKey, Addresses.FakeAddress)},
                {"Key Item: Sheet Music",new Tuple<int, uint, uint>(Memory.ReadInt(Addresses.SheetMusic), Addresses.SheetMusic, Addresses.FakeAddress)},
            };
        }
        public static Dictionary<string, Tuple<int, uint, uint>> GetSkillStatuses()
        {
            return new Dictionary<string, Tuple<int, uint, uint>>
            {
                {"Skill: Daring Dash",new Tuple<int, uint, uint>(Memory.ReadInt(Addresses.DaringDashSkill), Addresses.DaringDashSkill, Addresses.FakeAddress)},
            };
        }

        public static Dictionary<string, Tuple<int, uint, uint>> GetWeaponLocationStatuses()
        {
            return new Dictionary<string, Tuple<int, uint, uint>>
            {
                // uses fake locations at the moment
                {"Equipment: Small Sword",new Tuple<int, uint, uint>(Memory.ReadInt(Addresses.FakeAddress), Addresses.FakeAddress, Addresses.SmallSword)},
                {"Equipment: Broadsword",new Tuple<int, uint, uint>(Memory.ReadInt(Addresses.FakeAddress), Addresses.FakeAddress,  Addresses.BroadswordCharge)},
                {"Equipment: Magic Sword",new Tuple<int, uint, uint>(Memory.ReadInt(Addresses.FakeAddress), Addresses.FakeAddress,  Addresses.MagicSword)},
                {"Equipment: Club",new Tuple<int, uint, uint>(Memory.ReadInt(Addresses.FakeAddress), Addresses.FakeAddress,  Addresses.ClubCharge)},
                {"Equipment: Hammer",new Tuple<int, uint, uint>(Memory.ReadInt(Addresses.FakeAddress), Addresses.FakeAddress,  Addresses.Hammer)},
                {"Equipment: Daggers",new Tuple<int, uint, uint>(Memory.ReadInt(Addresses.FakeAddress), Addresses.FakeAddress,  Addresses.DaggerAmmo)},
                {"Equipment: Axe",new Tuple<int, uint, uint>(Memory.ReadInt(Addresses.FakeAddress), Addresses.FakeAddress,  Addresses.Axe)},
                {"Equipment: Chicken Drumsticks",new Tuple<int, uint, uint>(Memory.ReadInt(Addresses.FakeAddress), Addresses.FakeAddress,  Addresses.ChickenDrumsticksAmmo)},
                {"Equipment: Crossbow",new Tuple<int, uint, uint>(Memory.ReadInt(Addresses.FakeAddress), Addresses.FakeAddress,  Addresses.CrossbowAmmo)},
                {"Equipment: Longbow",new Tuple<int, uint, uint>(Memory.ReadInt(Addresses.FakeAddress), Addresses.FakeAddress,  Addresses.LongbowAmmo)},
                {"Equipment: Fire Longbow",new Tuple<int, uint, uint>(Memory.ReadInt(Addresses.FakeAddress), Addresses.FakeAddress,  Addresses.FireLongbowAmmo)},
                {"Equipment: Magic Longbow",new Tuple<int, uint, uint>(Memory.ReadInt(Addresses.FakeAddress), Addresses.FakeAddress,  Addresses.MagicLongbowAmmo)},
                {"Equipment: Spear",new Tuple<int, uint, uint>(Memory.ReadInt(Addresses.FakeAddress), Addresses.FakeAddress,  Addresses.SpearAmmo)},
                {"Equipment: Lightning",new Tuple<int, uint, uint>(Memory.ReadInt(Addresses.FakeAddress), Addresses.FakeAddress,  Addresses.LightningCharge)},
                {"Equipment: Good Lightning",new Tuple<int, uint, uint>(Memory.ReadInt(Addresses.FakeAddress), Addresses.FakeAddress,  Addresses.GoodLightning)},
                {"Equipment: Copper Shield",new Tuple<int, uint, uint>(Memory.ReadInt(Addresses.FakeAddress), Addresses.FakeAddress,  Addresses.CopperShieldAmmo)},
                {"Equipment: Silver Shield",new Tuple<int, uint, uint>(Memory.ReadInt(Addresses.FakeAddress), Addresses.FakeAddress,  Addresses.SilverShieldAmmo)},
                {"Equipment: Gold Shield",new Tuple<int, uint, uint>(Memory.ReadInt(Addresses.FakeAddress), Addresses.FakeAddress,  Addresses.GoldShieldAmmo)},
                {"Equipment: Dragon Armour",new Tuple<int, uint, uint>(Memory.ReadInt(Addresses.FakeAddress), Addresses.FakeAddress,  Addresses.DragonArmour)},
            };
        }

        public static Dictionary<string, Tuple<int, uint, uint>> GetHallOfHeroesStatuses()
        {
            return new Dictionary<string, Tuple<int, uint, uint>>
            {
                // these locations need updated.

                {"Hall of Heroes: Canny Tim 1",new Tuple<int, uint, uint>(Memory.ReadInt(Addresses.CannyTim1), Addresses.CannyTim1, Addresses.FakeAddress)},
                {"Hall of Heroes: Canny Tim 2",new Tuple<int, uint, uint>(Memory.ReadInt(Addresses.CannyTim2), Addresses.CannyTim2, Addresses.FakeAddress)},
                {"Hall of Heroes: Stanyer Iron Hewer 1",new Tuple<int, uint, uint>(Memory.ReadInt(Addresses.StanyerIronHewer1), Addresses.StanyerIronHewer1, Addresses.FakeAddress)},
                {"Hall of Heroes: Stanyer Iron Hewer 2",new Tuple<int, uint, uint>(Memory.ReadInt(Addresses.StanyerIronHewer2), Addresses.StanyerIronHewer2, Addresses.FakeAddress)},
                {"Hall of Heroes: Woden The Mighty 1",new Tuple<int, uint, uint>(Memory.ReadInt(Addresses.WodenTheMighty1), Addresses.WodenTheMighty1, Addresses.FakeAddress)},
                {"Hall of Heroes: Woden The Mighty 2",new Tuple<int, uint, uint>(Memory.ReadInt(Addresses.WodenTheMighty2), Addresses.WodenTheMighty2, Addresses.FakeAddress)},
                {"Hall of Heroes: RavenHooves The Archer 1",new Tuple<int, uint, uint>(Memory.ReadInt(Addresses.RavenHoovesTheArcher1), Addresses.RavenHoovesTheArcher1, Addresses.FakeAddress)},
                {"Hall of Heroes: RavenHooves The Archer 2",new Tuple<int, uint, uint>(Memory.ReadInt(Addresses.RavenHoovesTheArcher2), Addresses.RavenHoovesTheArcher2, Addresses.FakeAddress)},
                {"Hall of Heroes: RavenHooves The Archer 3",new Tuple<int, uint, uint>(Memory.ReadInt(Addresses.RavenHoovesTheArcher3), Addresses.RavenHoovesTheArcher3, Addresses.FakeAddress)},
                {"Hall of Heroes: RavenHooves The Archer 4",new Tuple<int, uint, uint>(Memory.ReadInt(Addresses.RavenHoovesTheArcher4), Addresses.RavenHoovesTheArcher4, Addresses.FakeAddress)},
                {"Hall of Heroes: Imanzi 1",new Tuple<int, uint, uint>(Memory.ReadInt(Addresses.Imanzi1), Addresses.Imanzi1, Addresses.FakeAddress)},
                {"Hall of Heroes: Imanzi 2",new Tuple<int, uint, uint>(Memory.ReadInt(Addresses.Imanzi2), Addresses.Imanzi2, Addresses.FakeAddress)},
                {"Hall of Heroes: Dark Steadfast 1",new Tuple<int, uint, uint>(Memory.ReadInt(Addresses.DarkSteadfast1), Addresses.DarkSteadfast1, Addresses.FakeAddress)},
                {"Hall of Heroes: Dark Steadfast 2",new Tuple<int, uint, uint>(Memory.ReadInt(Addresses.DarkSteadfast2), Addresses.DarkSteadfast2, Addresses.FakeAddress)},
                {"Hall of Heroes: Karl Stungard 1",new Tuple<int, uint, uint>(Memory.ReadInt(Addresses.KarlStungard1), Addresses.KarlStungard1, Addresses.FakeAddress)},
                {"Hall of Heroes: Karl Stungard 2",new Tuple<int, uint, uint>(Memory.ReadInt(Addresses.KarlStungard2), Addresses.KarlStungard2, Addresses.FakeAddress)},
                {"Hall of Heroes: Bloodmonath Skill Cleaver 1",new Tuple<int, uint, uint>(Memory.ReadInt(Addresses.BloodmonathSkillCleaver1), Addresses.BloodmonathSkillCleaver1, Addresses.FakeAddress)},
                {"Hall of Heroes: Bloodmonath Skill Cleaver 2",new Tuple<int, uint, uint>(Memory.ReadInt(Addresses.BloodmonathSkillCleaver2), Addresses.BloodmonathSkillCleaver2, Addresses.FakeAddress)},
                {"Hall of Heroes: Megwynne Stormbinder 1",new Tuple<int, uint, uint>(Memory.ReadInt(Addresses.MegwynneStormbinder1), Addresses.MegwynneStormbinder1, Addresses.FakeAddress)},
                {"Hall of Heroes: Megwynne Stormbinder 2",new Tuple<int, uint, uint>(Memory.ReadInt(Addresses.MegwynneStormbinder2), Addresses.MegwynneStormbinder2, Addresses.FakeAddress)},
            };
        }

        public static Dictionary<string, Tuple<int, uint, uint>> GetLifeBottleLocationStatuses()
        {
            return new Dictionary<string, Tuple<int, uint, uint>>
            {
                // uses fake locations at the moment
                {"Life Bottle: Dan's Crypt", new Tuple<int, uint, uint>(Memory.ReadInt(Addresses.FakeAddress), Addresses.FakeAddress, Addresses.FakeAddress)},
                {"Life Bottle: The Graveyard", new Tuple<int, uint, uint>(Memory.ReadInt(Addresses.FakeAddress), Addresses.FakeAddress, Addresses.FakeAddress)},
                {"Life Bottle: Hall of Heroes (Canny Tim)", new Tuple<int, uint, uint>(Memory.ReadInt(Addresses.FakeAddress), Addresses.FakeAddress, Addresses.FakeAddress)},
                {"Life Bottle: Dan's Crypt - Behind Wall", new Tuple<int, uint, uint>(Memory.ReadInt(Addresses.FakeAddress), Addresses.FakeAddress, Addresses.FakeAddress)},
                {"Life Bottle: Scarecrow Fields", new Tuple<int, uint, uint>(Memory.ReadInt(Addresses.FakeAddress), Addresses.FakeAddress, Addresses.FakeAddress)},
                {"Life Bottle: Pools of the Ancient Dead", new Tuple<int, uint, uint>(Memory.ReadInt(Addresses.FakeAddress), Addresses.FakeAddress, Addresses.FakeAddress)},
                {"Life Bottle: Hall of Heroes (Ravenhooves The Archer)", new Tuple<int, uint, uint>(Memory.ReadInt(Addresses.FakeAddress), Addresses.FakeAddress, Addresses.FakeAddress)},
                {"Life Bottle: Hall of Heroes (Dirk Steadfast)", new Tuple<int, uint, uint>(Memory.ReadInt(Addresses.FakeAddress), Addresses.FakeAddress, Addresses.FakeAddress)},
                {"Life Bottle: The Time Device", new Tuple<int, uint, uint>(Memory.ReadInt(Addresses.FakeAddress), Addresses.FakeAddress, Addresses.FakeAddress)}
            };
        }

        public static Dictionary<string, Tuple<int, uint, uint>> GetRuneLocationStatuses()
        {
            return new Dictionary<string, Tuple<int, uint, uint>>
            {
                // uses fake locations at the moment

                // Chaos Runes
                {"Chaos Rune: The Graveyard", new Tuple<int, uint, uint>(Memory.ReadInt(Addresses.FakeAddress), Addresses.FakeAddress, Addresses.FakeAddress)},
                {"Chaos Rune: The Hilltop Mausoleum", new Tuple<int, uint, uint>(Memory.ReadInt(Addresses.FakeAddress), Addresses.FakeAddress, Addresses.FakeAddress)},
                {"Chaos Rune: Scarecrow Fields", new Tuple<int, uint, uint>(Memory.ReadInt(Addresses.FakeAddress), Addresses.FakeAddress, Addresses.FakeAddress)},
                {"Chaos Rune: The Lake", new Tuple<int, uint, uint>(Memory.ReadInt(Addresses.FakeAddress), Addresses.FakeAddress, Addresses.FakeAddress)},
                {"Chaos Rune: Pumpkin Gorge", new Tuple<int, uint, uint>(Memory.ReadInt(Addresses.FakeAddress), Addresses.FakeAddress, Addresses.FakeAddress)},
                {"Chaos Rune: Sleeping Village", new Tuple<int, uint, uint>(Memory.ReadInt(Addresses.FakeAddress), Addresses.FakeAddress, Addresses.FakeAddress)},
                {"Chaos Rune: Pools of the Ancient Dead", new Tuple<int, uint, uint>(Memory.ReadInt(Addresses.FakeAddress), Addresses.FakeAddress, Addresses.FakeAddress)},
                {"Chaos Rune: Asylum Grounds", new Tuple<int, uint, uint>(Memory.ReadInt(Addresses.FakeAddress), Addresses.FakeAddress, Addresses.FakeAddress)},
                {"Chaos Rune: The Haunted Ruins", new Tuple<int, uint, uint>(Memory.ReadInt(Addresses.FakeAddress), Addresses.FakeAddress, Addresses.FakeAddress)},
                {"Chaos Rune: Ghost Ship", new Tuple<int, uint, uint>(Memory.ReadInt(Addresses.FakeAddress), Addresses.FakeAddress, Addresses.FakeAddress)},
                {"Chaos Rune: The Time Device", new Tuple<int, uint, uint>(Memory.ReadInt(Addresses.FakeAddress), Addresses.FakeAddress, Addresses.FakeAddress)},

                // Earth Runes
                {"Earth Rune: The Graveyard", new Tuple<int, uint, uint>(Memory.ReadInt(Addresses.FakeAddress), Addresses.FakeAddress, Addresses.FakeAddress)},
                {"Earth Rune: The Hilltop Mausoleum", new Tuple<int, uint, uint>(Memory.ReadInt(Addresses.FakeAddress), Addresses.FakeAddress, Addresses.FakeAddress)},
                {"Earth Rune: Scarecrow Fields", new Tuple<int, uint, uint>(Memory.ReadInt(Addresses.FakeAddress), Addresses.FakeAddress, Addresses.FakeAddress)},
                {"Earth Rune: The Crystal Caves", new Tuple<int, uint, uint>(Memory.ReadInt(Addresses.FakeAddress), Addresses.FakeAddress, Addresses.FakeAddress)},
                {"Earth Rune: The Lake", new Tuple<int, uint, uint>(Memory.ReadInt(Addresses.FakeAddress), Addresses.FakeAddress, Addresses.FakeAddress)},
                {"Earth Rune: Pumpkin Gorge", new Tuple<int, uint, uint>(Memory.ReadInt(Addresses.FakeAddress), Addresses.FakeAddress, Addresses.FakeAddress)},
                {"Earth Rune: Sleeping Village", new Tuple<int, uint, uint>(Memory.ReadInt(Addresses.FakeAddress), Addresses.FakeAddress, Addresses.FakeAddress)},
                {"Earth Rune: Inside the Asylum", new Tuple<int, uint, uint>(Memory.ReadInt(Addresses.FakeAddress), Addresses.FakeAddress, Addresses.FakeAddress)},
                {"Earth Rune: Enchanted Earth", new Tuple<int, uint, uint>(Memory.ReadInt(Addresses.FakeAddress), Addresses.FakeAddress, Addresses.FakeAddress)},
                {"Earth Rune: The Haunted Ruins", new Tuple<int, uint, uint>(Memory.ReadInt(Addresses.FakeAddress), Addresses.FakeAddress, Addresses.FakeAddress)},
                {"Earth Rune: The Entrance Hall", new Tuple<int, uint, uint>(Memory.ReadInt(Addresses.FakeAddress), Addresses.FakeAddress, Addresses.FakeAddress)},
                {"Earth Rune: The Time Device", new Tuple<int, uint, uint>(Memory.ReadInt(Addresses.FakeAddress), Addresses.FakeAddress, Addresses.FakeAddress)},

                // Moon Runes
                {"Moon Rune: The Hilltop Mausoleum", new Tuple<int, uint, uint>(Memory.ReadInt(Addresses.FakeAddress), Addresses.FakeAddress, Addresses.FakeAddress)},
                {"Moon Rune: Scarecrow Fields", new Tuple<int, uint, uint>(Memory.ReadInt(Addresses.FakeAddress), Addresses.FakeAddress, Addresses.FakeAddress)},
                {"Moon Rune: Pumpkin Gorge", new Tuple<int, uint, uint>(Memory.ReadInt(Addresses.FakeAddress), Addresses.FakeAddress, Addresses.FakeAddress)},
                {"Moon Rune: Ghost Ship", new Tuple<int, uint, uint>(Memory.ReadInt(Addresses.FakeAddress), Addresses.FakeAddress, Addresses.FakeAddress)},
                {"Moon Rune: The Time Device", new Tuple<int, uint, uint>(Memory.ReadInt(Addresses.FakeAddress), Addresses.FakeAddress, Addresses.FakeAddress)},

                // Star Runes
                {"Star Rune: Return to the Graveyard", new Tuple<int, uint, uint>(Memory.ReadInt(Addresses.FakeAddress), Addresses.FakeAddress, Addresses.FakeAddress)},
                {"Star Rune: Dan's Crypt", new Tuple<int, uint, uint>(Memory.ReadInt(Addresses.FakeAddress), Addresses.FakeAddress, Addresses.FakeAddress)},
                {"Star Rune: The Crystal Caves", new Tuple<int, uint, uint>(Memory.ReadInt(Addresses.FakeAddress), Addresses.FakeAddress, Addresses.FakeAddress)},
                {"Star Rune: The Lake", new Tuple<int, uint, uint>(Memory.ReadInt(Addresses.FakeAddress), Addresses.FakeAddress, Addresses.FakeAddress)},
                {"Star Rune: Enchanted Earth", new Tuple<int, uint, uint>(Memory.ReadInt(Addresses.FakeAddress), Addresses.FakeAddress, Addresses.FakeAddress)},
                {"Star Rune: The Gallows Gauntlet", new Tuple<int, uint, uint>(Memory.ReadInt(Addresses.FakeAddress), Addresses.FakeAddress, Addresses.FakeAddress)},
                {"Star Rune: Ghost Ship", new Tuple<int, uint, uint>(Memory.ReadInt(Addresses.FakeAddress), Addresses.FakeAddress, Addresses.FakeAddress)},

                // Time Runes
                {"Time Rune: The Lake", new Tuple<int, uint, uint>(Memory.ReadInt(Addresses.FakeAddress), Addresses.FakeAddress, Addresses.FakeAddress)},
                {"Time Rune: Pumpkin Gorge", new Tuple<int, uint, uint>(Memory.ReadInt(Addresses.FakeAddress), Addresses.FakeAddress, Addresses.FakeAddress)},
                {"Time Rune: The Time Device", new Tuple<int, uint, uint>(Memory.ReadInt(Addresses.FakeAddress), Addresses.FakeAddress, Addresses.FakeAddress)}
            };
        }


        public static Dictionary<string, Tuple<int, uint, uint>> GetAllLocationsAndAddresses()
        {
            Dictionary<string, Tuple<int, uint, uint>> allStatuses = new Dictionary<string, Tuple<int, uint, uint>>();

            // Helper function to safely add dictionary entries
            Action<Dictionary<string, Tuple<int, uint, uint>>> addDictionary = (dict) =>
            {
                foreach (var entry in dict)
                {
                    if (!allStatuses.ContainsKey(entry.Key)) // Avoid adding duplicate keys if any exist
                    {
                        allStatuses.Add(entry.Key, entry.Value);
                    }
                    else
                    {
                        // Handle duplicate keys, e.g., log a warning or overwrite
                        // For now, we'll just ignore if a key already exists.
                        // If you want to overwrite, you can do: allStatuses[entry.Key] = entry.Value;
                        Console.WriteLine($"Warning: Duplicate key '{entry.Key}' found. Skipping addition.");
                    }
                }
            };

            addDictionary(GetLevelCompleteStatuses());
            addDictionary(GetChaliceLocationStatuses());
            addDictionary(GetSkillStatuses());
            addDictionary(GetWeaponLocationStatuses());
            addDictionary(GetHallOfHeroesStatuses());
            addDictionary(GetLifeBottleLocationStatuses());
            addDictionary(GetKeyItemStatuses());
            addDictionary(GetRuneLocationStatuses());

            return allStatuses;
        }



        public static List<Location> BuildLocationList()
        {
            int baseId = 99251000;
            List<Location> locations = new List<Location>();

            List<LevelData> levels = GetLevelData();
            List<ChaliceData> chalices = GetChaliceData();
            List<HallOfHeroesData> hallOfHeroesVisits = GetHallOfHeroesData();
            List<SkillData> skills = GetSkillData();
            List<WeaponLocationsData> weapons = GetWeaponLocationsData();
            List<LifeBottlesData> bottles = GetLifeBottleLocationsData();
            List<KeyItemsData> keys = GetKeyItemsData();
            List<RuneData> runes = GetRuneLocationsData();

            var levelDict = GetLevelCompleteStatuses();
            var chaliceDict = GetChaliceLocationStatuses();
            var skillDict = GetSkillStatuses();
            var hallOfHeroesDict = GetHallOfHeroesStatuses();
            var weaponLocationDict = GetWeaponLocationStatuses();
            var bottleLocationDict = GetLifeBottleLocationStatuses();
            var keyItemDict = GetKeyItemStatuses();
            var runeLocationDict = GetRuneLocationStatuses();


            // level and Chalice locations
            foreach (var level in levels) {
                if (level.Name.Contains("Cleared")) {
                    {
                        Location location = new Location()
                        {
                            Name = level.Name,
                            Address = levelDict[level.Name].Item2,
                            Id = baseId + level.LevelId,
                            CheckType = LocationCheckType.Byte,
                            CompareType = level.Name.Contains("Zarok") ? LocationCheckCompareType.Match : LocationCheckCompareType.GreaterThan,
                            CheckValue = level.Name = "16"
                        };

                        locations.Add(location);

                    };
                }
            }
            foreach (var chalice in chalices)
            {
                if (chalice.Name.Contains("Chalice"))
                {
                    {
                        Location location = new Location()
                        {
                            Name = chalice.Name,
                            Address = chaliceDict[chalice.Name].Item2,
                            Id = baseId + chalice.LevelId,
                            CheckType = LocationCheckType.Byte,
                            CheckValue = "19"
                        };

                        locations.Add(location);

                    };
                }
            }
            foreach (var skill in skills)
            {
                {
                    Location location = new Location()
                    {
                        Name = skill.Name,
                        Address = skillDict[skill.Name].Item2,
                        Id = baseId + skill.LevelId,
                        CheckType = LocationCheckType.Byte,
                        CompareType = LocationCheckCompareType.Match,
                        CheckValue = skill.Name = "1"
                    };

                    locations.Add(location);

                };
            }
            foreach (var hallOfHeroesVisit in hallOfHeroesVisits)
            {
                {
                    Location location = new Location()
                    {
                        Name = hallOfHeroesVisit.Name,
                        Address = hallOfHeroesDict[hallOfHeroesVisit.Name].Item2,
                        Id = baseId + hallOfHeroesVisit.LevelId,
                        CheckType = LocationCheckType.Byte,
                        CompareType = LocationCheckCompareType.GreaterThan,
                        CheckValue = hallOfHeroesVisit.Name = "0"
                    };

                    locations.Add(location);

                };
            }
            foreach (var weaponLocation in weapons)
            {
                {
                    Location location = new Location()
                    {
                        Name = weaponLocation.Name,
                        Address = weaponLocationDict[weaponLocation.Name].Item2,
                        Id = baseId + weaponLocation.LevelId,
                        CheckType = LocationCheckType.Byte,
                        CompareType = LocationCheckCompareType.GreaterThan,
                        CheckValue = weaponLocation.Name = "999" // fake number as this is just a dummy location for now
                    };

                    locations.Add(location);

                };
            }
            foreach (var bottleLocation in bottles)
            {
                {
                    Location location = new Location()
                    {
                        Name = bottleLocation.Name,
                        Address = bottleLocationDict[bottleLocation.Name].Item2,
                        Id = baseId + bottleLocation.LevelId,
                        CheckType = LocationCheckType.Byte,
                        CompareType = LocationCheckCompareType.GreaterThan,
                        CheckValue = bottleLocation.Name = "999" // fake number as this is just a dummy location for now
                    };

                    locations.Add(location);

                };
            }
            foreach (var keyItem in keys)
            {
                {
                    Location location = new Location()
                    {
                        Name = keyItem.Name,
                        Address = keyItemDict[keyItem.Name].Item2,
                        Id = baseId + keyItem.LevelId,
                        CheckType = LocationCheckType.Byte,
                        CompareType = LocationCheckCompareType.GreaterThan,
                        CheckValue = keyItem.Name = "999" // fake number as this is just a dummy location for now
                    };

                    locations.Add(location);

                };
            }
            foreach (var runeLocation in runes)
            {
                {
                    Location location = new Location()
                    {
                        Name = runeLocation.Name,
                        Address = runeLocationDict[runeLocation.Name].Item2,
                        Id = baseId + runeLocation.LevelId,
                        CheckType = LocationCheckType.Byte,
                        CompareType = LocationCheckCompareType.GreaterThan,
                        CheckValue = runeLocation.Name = "999" // fake number as this is just a dummy location for now
                    };

                    locations.Add(location);

                };
            }
            return locations;
        }
        internal static readonly Dictionary<string, uint> AmmoAddressDictionary = new()
        {
            ["Gold Coins"] = Addresses.CurrentGold,
            ["Health"] = Addresses.CurrentEnergy,
            ["Health Vial"] = Addresses.CurrentEnergy,
            ["Small Sword"] = Addresses.SmallSword,
            ["Magic Sword"] = Addresses.MagicSword,
            ["Hammer"] = Addresses.Hammer,
            ["Axe"] = Addresses.Axe,
            ["Good Lightning"] = Addresses.GoodLightning,
            ["Dragon Armour"] = Addresses.DragonArmour,
            ["Life Bottle"] = Addresses.CurrentLifePotions,
            ["Energy"] = Addresses.CurrentEnergy,
            ["Dagger"] = Addresses.DaggerAmmo,
            ["Dagger Ammo"] = Addresses.DaggerAmmo,
            ["Broadsword"] = Addresses.BroadswordCharge,
            ["Broadsword Charge"] = Addresses.BroadswordCharge,
            ["Club"] = Addresses.ClubCharge,
            ["Club Charge"] = Addresses.ClubCharge,
            ["Chicken Drumsticks"] = Addresses.ChickenDrumsticksAmmo,
            ["Chicken Drumsticks Ammo"] = Addresses.ChickenDrumsticksAmmo,
            ["Crossbow"] = Addresses.CrossbowAmmo,
            ["Crossbow Ammo"] = Addresses.CrossbowAmmo,
            ["Longbow"] = Addresses.LongbowAmmo,
            ["Longbow Ammo"] = Addresses.LongbowAmmo,
            ["Fire Longbow"] = Addresses.FireLongbowAmmo,
            ["Fire Longbow Ammo"] = Addresses.FireLongbowAmmo,
            ["Magic Longbow"] = Addresses.MagicLongbowAmmo,
            ["Magic Longbow Ammo"] = Addresses.MagicLongbowAmmo,
            ["Spear"] = Addresses.SpearAmmo,
            ["Spear Ammo"] = Addresses.SpearAmmo,
            ["Copper Shield"] = Addresses.CopperShieldAmmo,
            ["Copper Shield Ammo"] = Addresses.CopperShieldAmmo,
            ["Silver Shield"] = Addresses.SilverShieldAmmo,
            ["Silver Shield Ammo"] = Addresses.SilverShieldAmmo,
            ["Gold Shield"] = Addresses.GoldShieldAmmo,
            ["Gold Shield Ammo"] = Addresses.GoldShieldAmmo,
            ["Lightning"] = Addresses.LightningCharge,
            ["Lightning Charge"] = Addresses.LightningCharge
        };

        internal static readonly Dictionary<string, uint> SkillDictionary = new()
        {
            ["Daring Dash"] = Addresses.DaringDashSkill,
        };

        internal static readonly Dictionary<string, uint> KeyItemDictionary = new()
        {
            ["Daring Dash"] = Addresses.DaringDashSkill,
        };


        private static List<LevelData> GetLevelData()
        {
            List<LevelData> levels = new List<LevelData>()
            {
                new LevelData("Cleared: Dan's Crypt", 1, [], []),
                new LevelData("Cleared: The Graveyard", 2, [], []),
                new LevelData("Cleared: Return to the Graveyard",3, [], []),
                new LevelData("Cleared: Cemetery Hill",4, [], []),
                new LevelData("Cleared: The Hilltop Mausoleum",5, [], []),
                new LevelData("Cleared: Scarecrow Fields",6, [], []),
                new LevelData("Cleared: Ant Hill",7, [], []),
                new LevelData("Cleared: The Crystal Caves",6, [], []),
                new LevelData("Cleared: The Lake",9, [], []),
                new LevelData("Cleared: Pumpkin Gorge",10, [], []),
                new LevelData("Cleared: Pumpkin Serpent",11, [], []),
                new LevelData("Cleared: Sleeping Village",12, [], []),
                new LevelData("Cleared: Pools of the Ancient Dead",13, [], []),
                new LevelData("Cleared: Asylum Grounds",14, [], []),
                new LevelData("Cleared: Inside the Asylum",15, [], []),
                new LevelData("Cleared: Enchanted Earth",16, [], []),
                new LevelData("Cleared: The Gallows Gauntlet" ,17, [], []),
                new LevelData("Cleared: The Haunted Ruins",18, [], []),
                new LevelData("Cleared: Ghost Ship",19, [], []),
                new LevelData("Cleared: The Entrance Hall",20, [], []),
                new LevelData("Cleared: The Time Device",21, [], []),
                new LevelData("Cleared: Zaroks Lair",22, [], []), // not sure if this is considered a level as it's the clear condition. Putting it here for now

            };
            return levels;
        }
        private static List<ChaliceData> GetChaliceData()
        {
            List<ChaliceData> chalices = new List<ChaliceData>()
            {
                new ChaliceData("Chalice: The Graveyard", 23),
                new ChaliceData("Chalice: Return to the Graveyard", 24),
                new ChaliceData("Chalice: Cemetery Hill", 25),
                new ChaliceData("Chalice: The Hilltop Mausoleum", 26),
                new ChaliceData("Chalice: Scarecrow Fields", 27),
                new ChaliceData("Chalice: Ant Hill", 28),
                new ChaliceData("Chalice: The Crystal Caves", 29),
                new ChaliceData("Chalice: The Lake", 30),
                new ChaliceData("Chalice: Pumpkin Gorge", 31),
                new ChaliceData("Chalice: Pumpkin Serpent", 32),
                new ChaliceData("Chalice: Sleeping Village", 33),
                new ChaliceData("Chalice: Pools of the Ancient Dead", 34),
                new ChaliceData("Chalice: Asylum Grounds", 35),
                new ChaliceData("Chalice: Inside the Asylum", 36),
                new ChaliceData("Chalice: Enchanted Earth", 37),
                new ChaliceData("Chalice: The Gallows Gauntlet", 38),
                new ChaliceData("Chalice: The Haunted Ruins", 39),
                new ChaliceData("Chalice: Ghost Ship", 40),
                new ChaliceData("Chalice: The Entrance Hall", 41),
                new ChaliceData("Chalice: The Time Device", 42)
            };
            return chalices;
        }

        private static List<SkillData> GetSkillData()
        {
            List<SkillData> skills = new List<SkillData>()
            {
                new SkillData("Skill: Daring Dash", 43)
            };
            return skills;
        }

        private static List<HallOfHeroesData> GetHallOfHeroesData()
        {
            List<HallOfHeroesData> hallOfHeroeVisits = new List<HallOfHeroesData>()
            {
                new HallOfHeroesData("Hall of Heroes: Canny Tim 1",44),
                new HallOfHeroesData("Hall of Heroes: Canny Tim 2",45),
                new HallOfHeroesData("Hall of Heroes: Stanyer Iron Hewer 1",46),
                new HallOfHeroesData("Hall of Heroes: Stanyer Iron Hewer 2",47),
                new HallOfHeroesData("Hall of Heroes: Woden The Mighty 1",48),
                new HallOfHeroesData("Hall of Heroes: Woden The Mighty 2",49),
                new HallOfHeroesData("Hall of Heroes: RavenHooves The Archer 1",50),
                new HallOfHeroesData("Hall of Heroes: RavenHooves The Archer 2",51),
                new HallOfHeroesData("Hall of Heroes: RavenHooves The Archer 3",52),
                new HallOfHeroesData("Hall of Heroes: RavenHooves The Archer 4",53),
                new HallOfHeroesData("Hall of Heroes: Imanzi 1",54),
                new HallOfHeroesData("Hall of Heroes: Imanzi 2",55),
                new HallOfHeroesData("Hall of Heroes: Dark Steadfast 1",56),
                new HallOfHeroesData("Hall of Heroes: Dark Steadfast 2",57),
                new HallOfHeroesData("Hall of Heroes: Karl Stungard 1",58),
                new HallOfHeroesData("Hall of Heroes: Karl Stungard 2",59),
                new HallOfHeroesData("Hall of Heroes: Bloodmonath Skill Cleaver 1",60),
                new HallOfHeroesData("Hall of Heroes: Bloodmonath Skill Cleaver 2",61),
                new HallOfHeroesData("Hall of Heroes: Megwynne Stormbinder 1",62),
                new HallOfHeroesData("Hall of Heroes: Megwynne Stormbinder 2",63),
            };
            return hallOfHeroeVisits;
        }
        private static List<WeaponLocationsData> GetWeaponLocationsData()
        {
            List<WeaponLocationsData> weaponLocations = new List<WeaponLocationsData>()
            {
                new WeaponLocationsData("Equipment: Small Sword",64), // crypt
                new WeaponLocationsData("Equipment: Broadsword",65), // hall of heroes
                new WeaponLocationsData("Equipment: Magic Sword",66),// hall of heroes
                new WeaponLocationsData("Equipment: Club",67), // cemetery hill
                new WeaponLocationsData("Equipment: Hammer",68),// hall of heroes
                new WeaponLocationsData("Equipment: Daggers",69), // dan's crypt
                new WeaponLocationsData("Equipment: Axe",70),// hall of heroes
                new WeaponLocationsData("Equipment: Chicken Drumsticks",71), // enchanted earth
                new WeaponLocationsData("Equipment: Crossbow",72),// hall of heroes
                new WeaponLocationsData("Equipment: Longbow",73),// hall of heroes
                new WeaponLocationsData("Equipment: Fire Longbow",74),// hall of heroes
                new WeaponLocationsData("Equipment: Magic Longbow",75),// hall of heroes
                new WeaponLocationsData("Equipment: Spear",76),// hall of heroes
                new WeaponLocationsData("Equipment: Lightning",77),// hall of heroes
                new WeaponLocationsData("Equipment: Good Lightning",78), // zarok's lair
                new WeaponLocationsData("Equipment: Copper Shield",79), // dan's crypt  (Way more than just this)
                new WeaponLocationsData("Equipment: Silver Shield",80), // return to the graveyard  (Way more than just this)
                new WeaponLocationsData("Equipment: Gold Shield",81),// hall of heroes
                new WeaponLocationsData("Equipment: Dragon Armour",82), // crystal caves
            };
            return weaponLocations;
        }

        private static List<LifeBottlesData> GetLifeBottleLocationsData()
        {
            List<LifeBottlesData> bottleLocations = new List<LifeBottlesData>()
            {
                new LifeBottlesData("Life Bottle: Dan's Crypt",83),
                new LifeBottlesData("Life Bottle: The Graveyard",84),
                new LifeBottlesData("Life Bottle: Hall of Heroes (Canny Tim)",85),
                new LifeBottlesData("Life Bottle: Dan's Crypt - Behind Wall",86),
                new LifeBottlesData("Life Bottle: Scarecrow Fields",87),
                new LifeBottlesData("Life Bottle: Pools of the Ancient Dead",88),
                new LifeBottlesData("Life Bottle: Hall of Heroes (Ravenhooves The Archer)",89),
                new LifeBottlesData("Life Bottle: Hall of Heroes (Dirk Steadfast)",90),
                new LifeBottlesData("Life Bottle: The Time Device",91),
            };
            return bottleLocations;
        }


        private static List<KeyItemsData> GetKeyItemsData()
        {
            List<KeyItemsData> bottleLocations = new List<KeyItemsData>()
            {
                new KeyItemsData("Key Item: Dragon Gem: Pumpkin Gorge",92),
                new KeyItemsData("Key Item: Dragon Gem: Inside the Asylum",93),
                new KeyItemsData("Key Item: King Peregrine's Crown",94),
                new KeyItemsData("Key Item: Soul Helmet 1",95),
                new KeyItemsData("Key Item: Soul Helmet 2",96),
                new KeyItemsData("Key Item: Soul Helmet 3",97),
                new KeyItemsData("Key Item: Soul Helmet 4",98),
                new KeyItemsData("Key Item: Soul Helmet 5",99),
                new KeyItemsData("Key Item: Soul Helmet 6",100),
                new KeyItemsData("Key Item: Soul Helmet 7",101),
                new KeyItemsData("Key Item: Soul Helmet 8",102),
                new KeyItemsData("Key Item: Witches Talisman",103),
                new KeyItemsData("Key Item: Safe Key",104),
                new KeyItemsData("Key Item: Shadow Artefact",105),
                new KeyItemsData("Key Item: Crucifix",106),
                new KeyItemsData("Key Item: Landlords Bust",107),
                new KeyItemsData("Key Item: Crucifix Cast",108),
                new KeyItemsData("Key Item: Amber Piece 1",109),
                new KeyItemsData("Key Item: Amber Piece 2",110),
                new KeyItemsData("Key Item: Amber Piece 3",111),
                new KeyItemsData("Key Item: Amber Piece 4",112),
                new KeyItemsData("Key Item: Amber Piece 5",113),
                new KeyItemsData("Key Item: Amber Piece 6",114),
                new KeyItemsData("Key Item: Amber Piece 7",115),
                new KeyItemsData("Key Item: Harvester Parts",116),
                new KeyItemsData("Key Item: Skull Key",117),
                new KeyItemsData("Key Item: Sheet Music",118),
            };
            return bottleLocations;
        }

        private static List<RuneData> GetRuneLocationsData()
        {
            List<RuneData> runeLocations = new List<RuneData>()
            {
                new RuneData("Chaos Rune: The Graveyard",119),
                new RuneData("Chaos Rune: The Hilltop Mausoleum",120),
                new RuneData("Chaos Rune: Scarecrow Fields",121),
                new RuneData("Chaos Rune: The Lake",122),
                new RuneData("Chaos Rune: Pumpkin Gorge",123),
                new RuneData("Chaos Rune: Sleeping Village",124),
                new RuneData("Chaos Rune: Pools of the Ancient Dead",125),
                new RuneData("Chaos Rune: Asylum Grounds",126),
                new RuneData("Chaos Rune: The Haunted Ruins",127),
                new RuneData("Chaos Rune: Ghost Ship",128),
                new RuneData("Chaos Rune: The Time Device",129),
                new RuneData("Earth Rune: The Graveyard",130),
                new RuneData("Earth Rune: The Hilltop Mausoleum",131),
                new RuneData("Earth Rune: Scarecrow Fields",132),
                new RuneData("Earth Rune: The Crystal Caves",133),
                new RuneData("Earth Rune: The Lake",134),
                new RuneData("Earth Rune: Pumpkin Gorge",135),
                new RuneData("Earth Rune: Sleeping Village",136),
                new RuneData("Earth Rune: Inside the Asylum",137),
                new RuneData("Earth Rune: Enchanted Earth",138),
                new RuneData("Earth Rune: The Haunted Ruins",139),
                new RuneData("Earth Rune: The Entrance Hall",140),
                new RuneData("Earth Rune: The Time Device",141),
                new RuneData("Moon Rune: The Hilltop Mausoleum",142),
                new RuneData("Moon Rune: Scarecrow Fields",143),
                new RuneData("Moon Rune: Pumpkin Gorge",144),
                new RuneData("Moon Rune: Ghost Ship",145),
                new RuneData("Moon Rune: The Time Device",146),
                new RuneData("Star Rune: Return to the Graveyard",147),
                new RuneData("Star Rune: Dan's Crypt",148),
                new RuneData("Star Rune: The Crystal Caves",149),
                new RuneData("Star Rune: The Lake",150),
                new RuneData("Star Rune: Enchanted Earth",151),
                new RuneData("Star Rune: The Gallows Gauntlet",152),
                new RuneData("Star Rune: Ghost Ship",153),
                new RuneData("Time Rune: The Lake",154),
                new RuneData("Time Rune: Pumpkin Gorge",155),
                new RuneData("Time Rune: The Time Device",156),
            };
            return runeLocations;
        }

    }
}