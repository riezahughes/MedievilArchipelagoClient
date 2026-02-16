using NAudio.Wave;
using NAudio.Wave.SampleProviders;

namespace MedievilArchipelago.Helpers
{
    public class JingleHandler
    {
        /// <summary>
        /// Volume level from 0.0 (silent) to 1.0 (full volume). Default is 0.3.
        /// </summary>
        public static float Volume { get; set; } = 0.03f;

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

        private static void PlayTone(int frequency, int durationMs)
        {
            var sine = new SignalGenerator()
            {
                Gain = Volume,
                Frequency = frequency,
                Type = SignalGeneratorType.Sin
            };

            var taken = sine.Take(TimeSpan.FromMilliseconds(durationMs));

            using var waveOut = new WaveOutEvent();
            waveOut.Init(taken);
            waveOut.Play();

            // Block until playback finishes
            while (waveOut.PlaybackState == PlaybackState.Playing)
            {
                Thread.Sleep(10);
            }
        }

        private static void Beeps0()
        {
            PlayTone(1320, 500);
            Thread.Sleep(125);
            PlayTone(1320, 500);
            Thread.Sleep(125);
            PlayTone(1320, 500);
            Thread.Sleep(125);
            PlayTone(2000, 1125);
        }

        private static void Beeps1()
        {
            PlayTone(2000, 125);
            Thread.Sleep(50);
            PlayTone(2000, 125);
            Thread.Sleep(2700);
        }

        private static void Beeps2()
        {
            PlayTone(1750, 100);
            PlayTone(1500, 100);
            PlayTone(1250, 100);
            PlayTone(1000, 100);
            PlayTone(750, 100);
            Thread.Sleep(2500);
        }
    }
}