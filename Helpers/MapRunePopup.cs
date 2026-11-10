using System.Text;
using Archipelago.Core;
using Archipelago.Core.Util;

namespace MedievilArchipelago.Helpers
{
    // On the world map, shows a small popup with the rune icons you have for the level under the cursor,
    // followed by the ones still missing (e.g. "[Chaos]  Need: Earth", or "All runes"). Levels without runes
    // get no popup.
    //
    // World map addresses (PAL, LANDMAP.BIN, only valid while it's loaded - see PopupHandler.IsOnWorldMap):
    //   0x80014FB0          pointer to the map's state; +0x18 is the index of the level under the cursor
    //   0x8001493C + i*0x24 one record per level; +0 is a pointer to the level's name
    internal class MapRunePopup
    {
        private const uint HoveredLevelOffset = 0x18;
        private const uint LevelRecordSize = 0x24;
        private const int LevelCount = 21;

        // how long the cursor has to stay on a level before its popup is shown
        private static readonly TimeSpan SettleTime = TimeSpan.FromMilliseconds(300);

        // rune icons (%<@iN%>) by the first word of the rune's item name
        private static readonly Dictionary<string, int> RuneIcons = new()
        {
            { "Chaos", 0 }, { "Moon", 1 }, { "Time", 2 }, { "Star", 3 }, { "Earth", 4 },
        };

        private static Task? _worker;

        public static void Start(ArchipelagoClient client, CancellationToken token)
        {
            if (_worker != null) return;
            _worker = Task.Run(() => RunAsync(client, token), token);
        }

        private static async Task RunAsync(ArchipelagoClient client, CancellationToken token)
        {
            int lastShown = -1, candidate = -1;
            DateTime candidateSince = DateTime.MinValue;

            while (!token.IsCancellationRequested)
            {
                try
                {
                    await Task.Delay(100, token);

                    int hovered = PopupHandler.IsOnWorldMap() ? ReadHoveredLevel() : -1;
                    if (hovered < 0)
                    {
                        lastShown = candidate = -1; // show it again next time the map opens
                        continue;
                    }

                    if (hovered != candidate)
                    {
                        candidate = hovered;
                        candidateSince = DateTime.UtcNow;
                        continue;
                    }

                    if (hovered == lastShown || DateTime.UtcNow - candidateSince < SettleTime) continue;
                    lastShown = hovered;

                    string? popup = BuildPopup(client, ReadLevelName(hovered));
                    if (popup != null) PopupHandler.ShowOnMap(popup);
                }
                catch (OperationCanceledException)
                {
                    return;
                }
                catch (Exception ex)
                {
                    // a failed popup must never take the client down
                    Console.WriteLine($"Map rune popup failed: {ex.Message}");
                    await Task.Delay(1000, CancellationToken.None);
                }
            }
        }

        private static int ReadHoveredLevel()
        {
            uint state = Memory.ReadUInt(Addresses.WorldMapStatePointer);
            if (state < 0x80000000 || state >= 0x80200000) return -1;
            int index = Memory.ReadInt((state & 0x001FFFFF) + HoveredLevelOffset);
            return index >= 0 && index < LevelCount ? index : -1;
        }

        private static string ReadLevelName(int index)
        {
            uint namePointer = Memory.ReadUInt(Addresses.WorldMapLevelRecords + (uint)index * LevelRecordSize);
            if (namePointer < 0x80000000 || namePointer >= 0x80200000) return "";
            byte[] bytes = Memory.ReadByteArray(namePointer & 0x001FFFFF, 32);
            int end = Array.IndexOf(bytes, (byte)0);
            return Encoding.ASCII.GetString(bytes, 0, end < 0 ? bytes.Length : end);
        }

        // "Chaos Rune: The Graveyard" -> "Chaos"
        private static string RuneKind(string runeItemName) => runeItemName.Split(' ')[0];

        private static string Normalise(string name)
        {
            string n = name.Trim().ToLowerInvariant();
            return n.StartsWith("the ") ? n[4..] : n;
        }

        // rune icons you have, then the missing ones; null if the level has no runes
        private static string? BuildPopup(ArchipelagoClient client, string levelName)
        {
            // the map's names don't always match the client's ("The Ghost Ship" vs "Ghost Ship", "Inside The
            // Asylum" vs "Inside the Asylum"), so compare ignoring case and a leading "The"
            var levelRunes = ThreadHandlers.LevelToRuneItemDictionary
                .FirstOrDefault(kv => Normalise(kv.Key) == Normalise(levelName)).Value;
            if (levelRunes == null || levelRunes.Count == 0) return null;

            var received = client.CurrentSession?.Items.AllItemsReceived.Select(i => i.ItemName).ToHashSet() ?? new HashSet<string>();
            var owned = levelRunes.Where(received.Contains).ToList();

            string lead = new string(' ', PopupHandler.LeadingPadding);
            string pad = new string(' ', PopupHandler.IconPadding);
            string trail = new string(' ', PopupHandler.TrailingPadding);

            // icons for the runes you have (they take no width, so each one is followed by padding to make room)
            var icons = new StringBuilder();
            foreach (var rune in owned)
            {
                if (RuneIcons.TryGetValue(RuneKind(rune), out int icon)) icons.Append($"%<@i{icon}%>{pad}");
            }

            // then the ones still missing, by name, e.g. "Need: Earth Moon"
            var missing = levelRunes.Where(r => !owned.Contains(r)).Select(RuneKind).ToList();
            string status = missing.Count == 0 ? "All runes" : $"Need: {string.Join(" ", missing)}";

            return $"@c{PopupHandler.ForUsColour}@b0@l{PopupHandler.LineHeight}@t3@>{lead}{icons}{status}{trail}";
        }
    }
}
