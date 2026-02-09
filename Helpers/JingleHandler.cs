using System.Runtime.InteropServices;

namespace MedievilArchipelago.Helpers
{
    internal class JingleHandler
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
                Beeps,
                Beeps2,
                Beeps3
            };
        }

        private static void Beeps()
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

        private static void Beeps2()
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

        private static void Beeps3()
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                Console.Beep(2000, 250);
                Thread.Sleep(50);
                Console.Beep(2000, 250);
                Thread.Sleep(2450);
            }
        }


    }


}