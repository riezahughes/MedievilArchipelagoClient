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
                Beeps2
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
                Console.Beep(2000, 125);
                Thread.Sleep(50);
                Console.Beep(2000, 125);
                Thread.Sleep(2700);
            }
        }
        
        private static void Beeps2()
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                Console.Beep(1750, 100);
                Console.Beep(1500, 100);
                Console.Beep(1250, 100);
                Console.Beep(1000, 100);
                Console.Beep(750, 100);
                Thread.Sleep(2500);

            }
        }

       

    }


}