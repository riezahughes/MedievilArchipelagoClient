using System.Collections.Concurrent;
using System.Text;
using Archipelago.Core.Util;

namespace MedievilArchipelago.Helpers
{
    // Shows messages in the game's own popup window (the one used by gargoyles, books and the
    // "Don't forget to pick up the Chalice!" hint). All addresses are PAL, found by disassembling MEDIEVIL.EXE.
    //
    // How the game does it: level overlays copy {message ID, string pointer} pairs into a global message table,
    // then call 0x8005E32C(id, 1, 0x19, 0x19), which only fills one of 4 request slots. The game picks the
    // request up by itself and opens the window. We do the same with memory writes:
    //   1. write our text into an unused bit of RAM
    //   2. point an unused message ID at it
    //   3. fill an empty request slot
    //
    // Colour: @c sets the text colour for the whole popup (the game's text parser has no code to change colour
    // part-way through), so each kind of message gets its own colour.
    //
    // Only 4 popup windows can be open at once (window stack at 0x800EF3C8, count at gp+0x604). A 5th one
    // overwrites the request slots and crashes the game, so popups are queued and each waits for the last one
    // to close.
    internal class PopupHandler
    {
        // the style the game uses for its short timed hints ("Don't forget to pick up the Chalice!"):
        // @c5 = cyan text, @b0 = thin white border, @t3 = closes by itself.
        // No %jc (centring) in the text: without @h/@x setting the window size it leaves the popup empty.
        public const string DefaultStyle = "@c5@b0@t3@>";

        // Text colours (@c numbers). These come from the palette in the game's graphics, not the code, so they
        // need checking on screen with "popup colours". Confirmed: 2 = red, 5 = cyan, 13 = gold. White is a guess.
        public static int ForUsColour { get; set; } = 13;   // items for us, from another player or ourselves (gold)
        public static int OtherColour { get; set; } = 0;    // everything else (white)
        public static int TrapColour { get; set; } = 2;     // traps, whoever they're for (red)

        // Icons: inventory icons are %<@iN%> (19 = Skull Key, 38 = Gold Pieces). Button icons are %<@BN%>, the
        // button set for an action in the controls (B0 = main attack, X by default).
        public const string ProgressionIcon = "%<@i19%>";
        public const string OtherIcon = "%<@i38%>";
        public const string TrapIcon = "%<@B0%>";

        // Icons are drawn without moving the text along, so the text is pushed right with spaces to clear them.
        // LeadingPadding moves the icon in from the window's left edge, which otherwise crops it, and
        // TrailingPadding adds the same kind of gap after the text so the popup looks balanced.
        public static int LeadingPadding { get; set; } = 1;
        public static int IconPadding { get; set; } = 6;
        public static int TrailingPadding { get; set; } = 2;

        // Line height in pixels (@l header code; the game's default is 15, set at 0x8005F5A0). Popups are one
        // line, and the window is one line tall, so the line has to be taller than the icon. Icons are always
        // centred vertically in the window (0x80060AA8), so they're cropped top and bottom if it's too short.
        public static int LineHeight { get; set; } = 36;

        // on by default, can be switched with the "popup" CLI command
        public static bool Enabled { get; set; } = true;

        // how long to wait after a popup closes before showing the next one
        public static TimeSpan GapBetweenPopups { get; set; } = TimeSpan.FromMilliseconds(500);

        // the game's popup window stack holds at most 4 windows (see Addresses.PopupOpenWindowCount)
        private const int MaxWindows = 4;

        // request slots (Addresses.PopupRequestSlots), 12 bytes each: u32 id, u16 flags, s16 priority, s16, s16
        private const int RequestSlotCount = 4;
        private const uint RequestSlotSize = 0x0C;
        private const uint EmptySlotId = 0x191;

        // Two message IDs that no level or the main game uses (also free: 170, 199, 298, 299, 328, 331-333, 345,
        // 346, 358, 359), each with its own text buffer. They're used in turn so a new message never changes the
        // text of one that's still on screen.
        // The buffers sit in the unused tail of libcd's directory cache (see Addresses.PopupTextBuffer1).
        private static readonly (uint MessageId, uint Buffer)[] Channels =
        {
            (168, Addresses.PopupTextBuffer1),
            (169, Addresses.PopupTextBuffer2),
        };
        private const int TextBufferSize = 0xE0;
        private static int _nextChannel;

        private static readonly ConcurrentQueue<string> _queue = new();
        private static Task? _worker;

        // the latest world-map popup (rune status of the hovered level); a newer one replaces it
        private static string? _pendingMapPopup;

        // the world map overlay (LANDMAP.BIN) is loaded when Addresses.LoadedOverlayId holds its ID
        private const uint WorldMapOverlayId = 0x1A;

        public static bool IsOnWorldMap() => Memory.ReadUInt(Addresses.LoadedOverlayId) == WorldMapOverlayId;

        // Queues a popup. If the text doesn't start with '@', the default timed hint style is put in front of it.
        // Safe to call from any thread; popups show one after another.
        public static void Show(string text)
        {
            if (!Enabled || string.IsNullOrWhiteSpace(text)) return;
            _queue.Enqueue(text.StartsWith("@") ? text : DefaultStyle + text);
        }

        // Shows a popup on the world map. Only the latest one is kept, so moving the cursor quickly between
        // levels doesn't pile them up. It's dropped if you leave the map before it shows.
        public static void ShowOnMap(string text)
        {
            if (!Enabled || string.IsNullOrWhiteSpace(text)) return;
            _pendingMapPopup = text;
        }

        // fromPlayer is the player whose world the item was found in
        public static void ItemReceived(string itemName, string fromPlayer, bool progression, bool trap = false) =>
            ShowItem($"{Clean(itemName)} found by {Clean(fromPlayer)}", progression, trap, forUs: true);

        public static void ItemSent(string itemName, string toPlayer, bool progression, bool trap = false) =>
            ShowItem($"Sent {Clean(itemName)} to {Clean(toPlayer)}", progression, trap, forUs: false);

        public static void ItemFound(string itemName, bool progression, bool trap = false) =>
            ShowItem($"Found {Clean(itemName)}", progression, trap, forUs: true);

        // one line of text with the icon on the left:
        //   trap        -> main attack button icon, red text (whoever it's for)
        //   progression -> key icon, otherwise gold icon
        //   gold text for items coming to us, white for the rest
        private static void ShowItem(string text, bool progression, bool trap, bool forUs)
        {
            int colour = trap ? TrapColour : forUs ? ForUsColour : OtherColour;
            string icon = trap ? TrapIcon : progression ? ProgressionIcon : OtherIcon;
            Show(WithIcon(colour, icon, text));
        }

        // queues one sample popup per text colour, to see which number is which colour
        public static void ShowColourSamples(int from = 0, int to = 15)
        {
            for (int c = from; c <= to; c++)
                Show(WithIcon(c, OtherIcon, $"Colour {c} sample text"));
        }

        // one line: padding, icon, padding to clear the icon (it takes no width), the text, then end padding
        private static string WithIcon(int colour, string icon, string text)
        {
            string lead = new string(' ', LeadingPadding);
            string pad = new string(' ', IconPadding);
            string trail = new string(' ', TrailingPadding);
            return $"@c{colour}@b0@l{LineHeight}@t3@>{lead}{icon}{pad}{text}{trail}";
        }

        // Starts the background loop that puts queued popups on screen. Call once after connecting.
        public static void Start(CancellationToken token)
        {
            if (_worker != null) return;
            _worker = Task.Run(() => RunAsync(token), token);
        }

        private static async Task RunAsync(CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                try
                {
                    await Task.Delay(250, token);

                    bool onMap = IsOnWorldMap();
                    if (!onMap) _pendingMapPopup = null;
                    if (!Enabled || (_queue.IsEmpty && _pendingMapPopup == null)) continue;

                    // popups show in a level or on the world map, when there's room for another window and a
                    // free request slot
                    if (!onMap && !PlayerStateHandler.isInTheGame()) continue;
                    int windowsBefore = Memory.ReadInt(Addresses.PopupOpenWindowCount);
                    if (windowsBefore < 0 || windowsBefore >= MaxWindows - 1) continue;
                    uint? slot = FindEmptySlot();
                    if (slot == null) continue;

                    // on the map the hovered level's rune popup goes first, otherwise the next queued popup
                    string? text = onMap ? Interlocked.Exchange(ref _pendingMapPopup, null) : null;
                    if (text == null && !_queue.TryDequeue(out text)) continue;

                    var (messageId, buffer) = Channels[_nextChannel];
                    _nextChannel = (_nextChannel + 1) % Channels.Length;

                    Request(slot.Value, messageId, buffer, text);

                    // wait for the game to take the request (up to 4s)...
                    for (int i = 0; i < 40 && Memory.ReadUInt(slot.Value) == messageId; i++)
                        await Task.Delay(100, token);

                    // ...for our window to open (up to 2s)...
                    for (int i = 0; i < 20 && Memory.ReadInt(Addresses.PopupOpenWindowCount) <= windowsBefore; i++)
                        await Task.Delay(100, token);

                    // ...and for it to close again (up to 20s, in case something else keeps a window open)
                    for (int i = 0; i < 200 && Memory.ReadInt(Addresses.PopupOpenWindowCount) > windowsBefore; i++)
                        await Task.Delay(100, token);

                    await Task.Delay(GapBetweenPopups, token);
                }
                catch (OperationCanceledException)
                {
                    return;
                }
                catch (Exception ex)
                {
                    // a failed popup must never take the client down
                    Console.WriteLine($"In-game popup failed: {ex.Message}");
                    await Task.Delay(1000, CancellationToken.None);
                }
            }
        }

        private static void Request(uint slot, uint messageId, uint buffer, string text)
        {
            byte[] bytes = Encoding.ASCII.GetBytes(text);
            if (bytes.Length > TextBufferSize - 1) bytes = bytes[..(TextBufferSize - 1)];
            byte[] terminated = new byte[bytes.Length + 1]; // zero-terminated
            bytes.CopyTo(terminated, 0);

            Memory.Write(buffer, terminated);
            Memory.Write(Addresses.PopupMessageTable + messageId * 4, 0x80000000u | buffer);

            // same as 0x8005E32C(id, 1, 0x19, 0x19): fill in the slot, writing the id last so the game
            // never sees a half-written request
            Memory.Write(slot + 0x04, (ushort)0);     // flags
            Memory.Write(slot + 0x06, (short)1);      // priority
            Memory.Write(slot + 0x08, (short)0x19);
            Memory.Write(slot + 0x0A, (short)0x19);
            Memory.Write(slot + 0x00, messageId);
        }

        private static uint? FindEmptySlot()
        {
            for (uint i = 0; i < RequestSlotCount; i++)
            {
                uint slot = Addresses.PopupRequestSlots + i * RequestSlotSize;
                if (Memory.ReadUInt(slot) == EmptySlotId) return slot;
            }
            return null;
        }

        // Names come from other games, so keep them to plain ASCII and take out the game's control characters:
        // '@' starts a window command, '%' a text command, '^' is replaced with an item name.
        private static string Clean(string text)
        {
            var sb = new StringBuilder(text.Length);
            foreach (char c in text)
            {
                if (c == '@' || c == '%' || c == '^') continue;
                if (c == '’' || c == '‘') sb.Append('\'');
                else if (c >= 0x20 && c < 0x7F) sb.Append(c);
                else sb.Append('?');
            }
            return sb.ToString();
        }

        // prints the request slots and our message table entries, to help work out what the game does with them
        public static void PrintDebug()
        {
            Console.WriteLine($"open windows: {Memory.ReadInt(Addresses.PopupOpenWindowCount)} (queue waits while this is {MaxWindows - 1} or more)");
            for (uint i = 0; i < RequestSlotCount; i++)
            {
                uint slot = Addresses.PopupRequestSlots + i * RequestSlotSize;
                Console.WriteLine($"slot {i}: id 0x{Memory.ReadUInt(slot):X} flags 0x{Memory.ReadUShort(slot + 4):X4} " +
                                  $"priority {Memory.ReadShort(slot + 6)} {Memory.ReadShort(slot + 8)} {Memory.ReadShort(slot + 10)}");
            }
            foreach (var (messageId, _) in Channels)
                Console.WriteLine($"message {messageId} -> 0x{Memory.ReadUInt(Addresses.PopupMessageTable + messageId * 4):X8}");
        }
    }
}
