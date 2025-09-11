using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Archipelago.Core.Traps;
using Archipelago.Core.Util;

namespace MedievilArchipelago.Helpers
{
    internal class TrapHandlers
    {
        // traps need added here and logic added into what i have already

        public static void ResetTraps()
        {

            byte[] DefaultWeaponIconX = BitConverter.GetBytes(0x0018);
            byte[] DefaultShieldIconX = BitConverter.GetBytes(0x0050);
            byte[] DefaultHealthbarX = BitConverter.GetBytes(0x0100);
            byte[] DefaultChaliceIconX = BitConverter.GetBytes(0x017e);
            byte[] DefaultMoneyIconX = BitConverter.GetBytes(0x01b6);
            byte[] DefaultWeaponIconY = BitConverter.GetBytes(0x001e);
            byte[] DefaultShieldIconY = BitConverter.GetBytes(0x001e);
            byte[] DefaultHealthbarY = BitConverter.GetBytes(0x0022);
            byte[] DefaultChaliceIconY = BitConverter.GetBytes(0x001e);
            byte[] DefaultMoneyIconY = BitConverter.GetBytes(0x001e);

            byte[] defaultSpeedValue = BitConverter.GetBytes(0x0100);
            byte[] defaultJumpValue = BitConverter.GetBytes(0x002f);

            byte[] defaultRenderDistance = BitConverter.GetBytes(0x1000);

            // Reset Hud
            Memory.Write(Addresses.WeaponIconX, DefaultWeaponIconX);
            Memory.Write(Addresses.ShieldIconX, DefaultShieldIconX);
            Memory.Write(Addresses.HealthbarX, DefaultHealthbarX);
            Memory.Write(Addresses.ChaliceIconX, DefaultChaliceIconX);
            Memory.Write(Addresses.MoneyIconX, DefaultMoneyIconX);
            Memory.Write(Addresses.WeaponIconY, DefaultWeaponIconY);
            Memory.Write(Addresses.ShieldIconY, DefaultShieldIconY);
            Memory.Write(Addresses.HealthbarY, DefaultHealthbarY);
            Memory.Write(Addresses.ChaliceIconY, DefaultChaliceIconY);
            Memory.Write(Addresses.MoneyIconY, DefaultMoneyIconY);

            // Reset Jump Height
            Memory.Write(Addresses.DanJumpHeight, defaultJumpValue);

            // Reset Normal Speed
            Memory.Write(Addresses.DanForwardSpeed, defaultSpeedValue);

            // Reset Lighting
            Memory.Write(Addresses.RenderDistance, defaultRenderDistance);
        }

        public static async void RunLagTrap()
        {
            using (var lagTrap = new LagTrap(TimeSpan.FromSeconds(20)))
            {
                lagTrap.Start();
                await lagTrap.WaitForCompletionAsync();
            }
        }

        public static void HeavyDanTrap()

        {
            byte[] defaultValue = BitConverter.GetBytes(0x0100);
            byte[] changedValue = BitConverter.GetBytes(0x0040);
            TimeSpan duration = TimeSpan.FromSeconds(15);
            Memory.Write(Addresses.DanForwardSpeed, changedValue);

            Task.Delay(duration).ContinueWith(delegate
            {
                Memory.Write(Addresses.DanForwardSpeed, defaultValue);
            }, TaskScheduler.Default);

        }

        public static void LightDanTrap()
        {
            byte[] defaultValue = BitConverter.GetBytes(0x002f);
            byte[] changedValue = BitConverter.GetBytes(0x0064);
            TimeSpan duration = TimeSpan.FromSeconds(15);
            Memory.Write(Addresses.DanJumpHeight, changedValue);

            Task.Delay(duration).ContinueWith(delegate
            {
                Memory.Write(Addresses.DanJumpHeight, defaultValue);
            }, TaskScheduler.Default);
        }

        public static void DarknessTrap(int currentLevel)
        {

            byte[] byteArray = BitConverter.GetBytes(0x0600);
            byte[] defaultValue = BitConverter.GetBytes(0x1000);

            TimeSpan duration = TimeSpan.FromSeconds(15);

            if (currentLevel != 14)
            {
                Memory.WriteByteArray(Addresses.RenderDistance, byteArray);
                Task.Delay(duration).ContinueWith(delegate
                {
                    Memory.Write(Addresses.RenderDistance, defaultValue);
                }, TaskScheduler.Default);

            }
        }

        public static void HudlessTrap()
        {

            byte[] DefaultWeaponIconX = BitConverter.GetBytes(0x0018);
            byte[] DefaultShieldIconX = BitConverter.GetBytes(0x0050);
            byte[] DefaultHealthbarX = BitConverter.GetBytes(0x0100);
            byte[] DefaultChaliceIconX = BitConverter.GetBytes(0x017e);
            byte[] DefaultMoneyIconX = BitConverter.GetBytes(0x01b6);
            byte[] DefaultWeaponIconY = BitConverter.GetBytes(0x001e);
            byte[] DefaultShieldIconY = BitConverter.GetBytes(0x001e);
            byte[] DefaultHealthbarY = BitConverter.GetBytes(0x0022);
            byte[] DefaultChaliceIconY = BitConverter.GetBytes(0x001e);
            byte[] DefaultMoneyIconY = BitConverter.GetBytes(0x001e);

            byte[] ChangedWeaponIconX = BitConverter.GetBytes(0x0320);
            byte[] ChangedShieldIconX = BitConverter.GetBytes(0x0320);
            byte[] ChangedHealthbarX = BitConverter.GetBytes(0x0320);
            byte[] ChangedChaliceIconX = BitConverter.GetBytes(0x0320);
            byte[] ChangedMoneyIconX = BitConverter.GetBytes(0x0320);

            TimeSpan duration = TimeSpan.FromSeconds(15);
            Memory.Write(Addresses.WeaponIconX, ChangedWeaponIconX);
            Memory.Write(Addresses.ShieldIconX, ChangedShieldIconX);
            Memory.Write(Addresses.HealthbarX, ChangedHealthbarX);
            Memory.Write(Addresses.ChaliceIconX, ChangedChaliceIconX);
            Memory.Write(Addresses.MoneyIconX, ChangedMoneyIconX);

            Task.Delay(duration).ContinueWith(delegate
            {
                Memory.Write(Addresses.WeaponIconX, DefaultWeaponIconX);
                Memory.Write(Addresses.ShieldIconX, DefaultShieldIconX);
                Memory.Write(Addresses.HealthbarX, DefaultHealthbarX);
                Memory.Write(Addresses.ChaliceIconX, DefaultChaliceIconX);
                Memory.Write(Addresses.MoneyIconX, DefaultMoneyIconX);
                Memory.Write(Addresses.WeaponIconY, DefaultWeaponIconY);
                Memory.Write(Addresses.ShieldIconY, DefaultShieldIconY);
                Memory.Write(Addresses.HealthbarY, DefaultHealthbarY);
                Memory.Write(Addresses.ChaliceIconY, DefaultChaliceIconY);
                Memory.Write(Addresses.MoneyIconY, DefaultMoneyIconY);
            }, TaskScheduler.Default);

        }
    }
}
