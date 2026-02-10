using System.Runtime.InteropServices;

namespace MedievilArchipelago.Helpers
{
    public class JingleHandler
    {
        public static void DeathJingle()
        {
            var rnd = new Random();
            var pick = rnd.Next(Sounds().Count);
            Sounds()[pick]();
        }

        public static List<Action> Sounds()
        {
            return new List<Action>
            {
                Beeps0,
                Beeps1,
                Beeps2,
                Beeps3,
                Beeps4,
                Beeps5
            };
        }

        private static void Beeps0()
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                Console.Beep(1320, 500);
                Thread.Sleep(125);
                Console.Beep(1320, 500);
                Thread.Sleep(125);
                Console.Beep(1320, 500);
                Thread.Sleep(125);
                Console.Beep(2000, 1125);
            }
        }

        private static void Beeps1()
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                Console.Beep(1350, 300);
                Thread.Sleep(50);
                Console.Beep(1050, 300);
                Thread.Sleep(50);
                Console.Beep(750, 300);
                Thread.Sleep(2000);
            }
        }

        private static void Beeps2()
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                Console.Beep(2000, 250);
                Thread.Sleep(50);
                Console.Beep(2000, 250);
                Thread.Sleep(2450);
            }
        }
        private static void Beeps3()
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                Console.Beep(2000, 125);
                Thread.Sleep(50);
                Console.Beep(2000, 125);
                Thread.Sleep(2700);
            }
        }

        private static void Beeps4()
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                Console.Beep(1320, 625);
                Thread.Sleep(50);
                Console.Beep(1320, 625);
                Thread.Sleep(50);
                Console.Beep(1320, 625);
                Thread.Sleep(50);
                Console.Beep(2000, 975);
            }
        }
        private static void Beeps5()
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                Console.Beep(1320, 650);
                Thread.Sleep(25);
                Console.Beep(1320, 650);
                Thread.Sleep(25);
                Console.Beep(1320, 650);
                Thread.Sleep(25);
                Console.Beep(2000, 975);
            }
        }

    }


}