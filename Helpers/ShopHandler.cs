using System.Text;
using Archipelago.Core;
using Archipelago.Core.Util;
using Archipelago.MultiClient.Net.Enums;
using Newtonsoft.Json.Linq;

namespace MedievilArchipelago.Helpers
{
    // ShopSanity: sells Archipelago items in the Merchant Gargoyles' "SECOND HAND GOODS" section, for gold.
    // All addresses are PAL, found by disassembling MEDIEVIL.EXE.
    //
    // How the game's shop works:
    //   Every merchant opens the same menu (0x800DA454), built from one table at 0x800DA2DC, 19 rows of
    //   { u8 inventory slot, u8 icon, u16 name message ID, u16 price, u16 amount }:
    //     rows 0-1  SERVICES           Enchant Sword 100, Repair Gold Shield 120
    //     rows 2-7  SUPPLIES           Daggers 40, Crossbow Bolts 80, Arrows 60, Flaming 80, Magical 100, Spears 100
    //     rows 8-18 SECOND HAND GOODS  stolen weapons: Small Sword 600, Broadsword 1200, Magic Sword 4000, Axe 1600,
    //                                  Crossbow 1400, Chicken Drumsticks 200, Longbow 2000, Lightning 3000,
    //                                  Hammer 1600, Flaming Longbow 2400, Magic Longbow 3000
    //   A SECOND HAND GOODS row is listed when bit 0 of its slot's high half is set (the game sets it on weapons an
    //   imp stole). Buying one (0x80063718) takes the price, clears that bit and fills the slot back up.
    //
    // What we do:
    //   AP rows point at slot 22, the unused "Lake Key", as a token. We set its bit, the player buys, the game
    //   clears the bit, and the selected row tells us which item was bought. Weapons that really are stolen keep
    //   their vanilla row, so they can still be bought back. Rows we don't use point at slot 37 (the Chalice,
    //   which is never stolen), so they're hidden. The table is only changed while no shop is open, so rows never
    //   move under the player.
    internal class ShopHandler
    {
        private const int FirstRow = 8;
        private const int RowCount = 11;
        private const uint RowSize = 8;

        private const byte TokenSlot = 22;   // Lake Key, left in the game but never used
        private const byte HiddenSlot = 37;  // Chalice: its buy back bit is never set, so the row isn't listed
        private const byte GoldIcon = 38;    // Gold Pieces
        private const uint TokenArmed = 0x0001FFFF;   // not owned (-1), buy back bit set
        private const uint TokenIdle = 0x0000FFFF;    // not owned

        // vanilla SECOND HAND GOODS rows 8-18
        private static readonly byte[][] VanillaRows =
        {
            new byte[] { 0x00, 0x07, 0xF3, 0x00, 0x58, 0x02, 0x00, 0x00 }, // Small Sword
            new byte[] { 0x01, 0x08, 0xF4, 0x00, 0xB0, 0x04, 0x00, 0x00 }, // Broadsword
            new byte[] { 0x02, 0x09, 0xF5, 0x00, 0xA0, 0x0F, 0x00, 0x00 }, // Magic Sword
            new byte[] { 0x06, 0x0D, 0xF9, 0x00, 0x40, 0x06, 0x00, 0x00 }, // Axe
            new byte[] { 0x08, 0x0E, 0xFA, 0x00, 0x78, 0x05, 0x00, 0x00 }, // Crossbow
            new byte[] { 0x07, 0x0F, 0xFB, 0x00, 0xC8, 0x00, 0x00, 0x00 }, // Chicken Drumsticks
            new byte[] { 0x09, 0x10, 0xFC, 0x00, 0xD0, 0x07, 0x00, 0x00 }, // Longbow
            new byte[] { 0x0D, 0x12, 0xFE, 0x00, 0xB8, 0x0B, 0x00, 0x00 }, // Lightning
            new byte[] { 0x04, 0x24, 0x1B, 0x01, 0x40, 0x06, 0x00, 0x00 }, // Hammer
            new byte[] { 0x0A, 0x22, 0x1C, 0x01, 0x60, 0x09, 0x00, 0x00 }, // Flaming Longbow
            new byte[] { 0x0B, 0x23, 0x1D, 0x01, 0xB8, 0x0B, 0x00, 0x00 }, // Magic Longbow
        };
        private const uint VanillaTitle = 0x800C5E94; // "SECOND HAND GOODS"

        // Message IDs for our row names, one per row. Checked against every overlay's {string, ID} message list and
        // the main game: empty in MEDIEVIL.EXE's table and never registered by any level (PopupHandler has 168
        // and 169). Note 328-333/358 are used by the Ant Caves and 345/346 by Pumpkin Serpent, so they're avoided.
        private static readonly uint[] RowMessageIds = { 102, 119, 122, 123, 129, 142, 143, 170, 237, 298, 299 };
        private const uint NameSize = 0x20;
        private const uint TitleBuffer = Addresses.ShopTextBuffer + RowCount * NameSize;
        private const string Title = "ARCHIPELAGO WARES";
        private const int MaxNameLength = 24;

        private record StockItem(long Id, int Price);

        // what's in each row of the current layout: an AP item, or null (vanilla or hidden)
        private static readonly StockItem?[] _rows = new StockItem?[RowCount];

        private static Dictionary<int, List<StockItem>> _stock = new();

        public static bool IsShopLocation(long locationId) => _stock.Values.Any(items => items.Any(i => i.Id == locationId));
        private static readonly Dictionary<long, string> _names = new();
        private static readonly HashSet<long> _bought = new();        // bought, waiting for the server
        private static string _layoutKey = "";
        private static bool _shopWasOpen;
        private static Task? _worker;

        // how long to wait for the server when setting up; the shop works without names if scouting is slow
        private static readonly TimeSpan ServerTimeout = TimeSpan.FromSeconds(15);

        // Starts the shop on its own background task and returns straight away, so the server requests it makes
        // can never hold up the rest of the client (the location monitor in particular).
        public static void Start(ArchipelagoClient client, CancellationToken token)
        {
            if (_worker != null) return;
            _worker = Task.Run(async () =>
            {
                try
                {
                    if (!await LoadStock(client)) return;
                    await LoadNames(client);
                    Console.WriteLine($"ShopSanity: {_stock.Values.Sum(s => s.Count)} items in {_stock.Count} Merchant Gargoyle shops.");
                    await RunAsync(client, token);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"ShopSanity couldn't start: {ex.Message}");
                }
            }, token);
        }

        private static async Task<bool> LoadStock(ArchipelagoClient client)
        {
            var slotData = await WithTimeout(client.CurrentSession.DataStorage.GetSlotDataAsync());
            if (slotData == null)
            {
                Console.WriteLine("ShopSanity: the server didn't send the shop stock, shops stay vanilla.");
                return false;
            }
            if (!slotData.TryGetValue("shop_stock", out var raw) || raw == null) return false;

            foreach (var (level, items) in JObject.FromObject(raw))
            {
                _stock[int.Parse(level)] = items!.Select(i => new StockItem((long)i["id"]!, (int)i["price"]!)).ToList();
            }
            return _stock.Count > 0;
        }

        // a server request that gives up after ServerTimeout (null) instead of waiting forever
        private static async Task<T?> WithTimeout<T>(Task<T> task) where T : class
        {
            return await Task.WhenAny(task, Task.Delay(ServerTimeout)) == task ? await task : null;
        }

        // "Skull Key" for our own items, "Bob's Hookshot" for someone else's
        private static async Task LoadNames(ArchipelagoClient client)
        {
            var session = client.CurrentSession;
            long[] ids = _stock.Values.SelectMany(s => s).Select(s => s.Id).ToArray();
            try
            {
                var scouted = await WithTimeout(session.Locations.ScoutLocationsAsync(HintCreationPolicy.None, ids));
                if (scouted == null)
                {
                    Console.WriteLine("ShopSanity: the server didn't answer the item lookup in time, items will show as \"AP Item\".");
                    return;
                }
                foreach (var (id, info) in scouted)
                {
                    string item = StripPrefix(info.ItemName ?? "AP Item");
                    _names[id] = info.Player.Slot == session.ConnectionInfo.Slot ? item : $"{info.Player.Name}'s {item}";
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ShopSanity: couldn't look up the shop items ({ex.Message}), they'll show as \"AP Item\".");
            }
        }

        private static string StripPrefix(string name)
        {
            foreach (var prefix in new[] { "Equipment: ", "Key Item: ", "Skill: " })
                if (name.StartsWith(prefix)) return name.Substring(prefix.Length);
            return name;
        }

        private static async Task RunAsync(ArchipelagoClient client, CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                try
                {
                    await Task.Delay(50, token);
                    SendPending(client);

                    if (IsShopOpen())
                    {
                        if (!_shopWasOpen) OnShopOpened();
                        _shopWasOpen = true;
                        CheckForPurchase(client);
                    }
                    else
                    {
                        _shopWasOpen = false;
                        if (PlayerStateHandler.isInTheGame()) Layout(client);
                    }
                }
                catch (OperationCanceledException)
                {
                    return;
                }
                catch (Exception ex)
                {
                    // the shop must never take the client down
                    Console.WriteLine($"ShopSanity failed: {ex.Message}");
                    await Task.Delay(1000, CancellationToken.None);
                }
            }
        }

        // the shop is open when a window on the game's window stack was opened from the shop menu
        private static bool IsShopOpen()
        {
            int count = Memory.ReadInt(Addresses.PopupOpenWindowCount);
            if (count <= 0 || count > 8) return false;
            for (uint i = 0; i < count; i++)
            {
                uint window = Memory.ReadUInt(Addresses.WindowStack + i * 4);
                if (!IsRamPointer(window)) continue;
                uint request = Memory.ReadUInt((window & 0x1FFFFF) + 4);
                if (!IsRamPointer(request)) continue;
                if (Memory.ReadUInt(request & 0x1FFFFF) == Addresses.ShopMenuId) return true;
            }
            return false;
        }

        private static bool IsRamPointer(uint p) => p >= 0x80000000 && p < 0x80200000;

        // the client prints this so levels with merchants can be confirmed (and added to the AP world's list)
        private static void OnShopOpened()
        {
            int level = Memory.ReadByte(Addresses.CurrentLevel);
            int left = _rows.Count(r => r != null && !_bought.Contains(r.Id));
            Console.WriteLine(_stock.ContainsKey(level)
                ? $"ShopSanity: Merchant Gargoyle opened in level {level} with {left} Archipelago items for sale."
                : $"ShopSanity: Merchant Gargoyle opened in level {level}, which has no Archipelago stock.");
        }

        // Works out what goes in rows 8-18 and writes it, but only when something changed.
        private static void Layout(ArchipelagoClient client)
        {
            int level = Memory.ReadByte(Addresses.CurrentLevel);
            var checkedIds = client.CurrentSession.Locations.AllLocationsChecked;
            var forSale = _stock.TryGetValue(level, out var items)
                ? items.Where(i => !checkedIds.Contains(i.Id) && !_bought.Contains(i.Id)).ToList()
                : new List<StockItem>();

            // weapons the imps stole come first, so buying them back always works
            var stolen = Enumerable.Range(0, RowCount).Where(r => IsListedForBuyBack(VanillaRows[r][0])).ToList();
            var apItems = forSale.Take(RowCount - stolen.Count).ToList();

            string key = $"{level}|{string.Join(",", stolen)}|{string.Join(",", apItems.Select(i => i.Id))}";
            if (key == _layoutKey) return;
            _layoutKey = key;

            if (apItems.Count == 0)
            {
                WriteVanilla();
                return;
            }

            int row = 0;
            foreach (int r in stolen)
            {
                _rows[row] = null;
                WriteRow(row++, VanillaRows[r]);
            }
            foreach (var item in apItems)
            {
                _rows[row] = item;
                WriteName(row, _names.GetValueOrDefault(item.Id, "AP Item"));
                byte[] data = new byte[RowSize];
                data[0] = TokenSlot;
                data[1] = GoldIcon;
                BitConverter.GetBytes((ushort)RowMessageIds[row]).CopyTo(data, 2);
                BitConverter.GetBytes((ushort)Math.Clamp(item.Price, 0, 9999)).CopyTo(data, 4);
                WriteRow(row++, data);
            }
            for (; row < RowCount; row++)
            {
                _rows[row] = null;
                WriteRow(row, Hidden(row));
            }

            WriteString(TitleBuffer, Title);
            Memory.Write(Addresses.ShopSecondHandTitle, 0x80000000u | TitleBuffer);
            Memory.Write(InventorySlot(TokenSlot), TokenArmed);
        }

        private static void WriteVanilla()
        {
            for (int r = 0; r < RowCount; r++)
            {
                _rows[r] = null;
                WriteRow(r, VanillaRows[r]);
            }
            Memory.Write(Addresses.ShopSecondHandTitle, VanillaTitle);
            Memory.Write(InventorySlot(TokenSlot), TokenIdle);
        }

        // While the shop is open: the game clears the token's buy back bit when an AP row is bought.
        private static void CheckForPurchase(ArchipelagoClient client)
        {
            uint token = Memory.ReadUInt(InventorySlot(TokenSlot));
            if (token == TokenArmed || token == TokenIdle) return;

            StockItem? bought = SelectedRow() is int row ? _rows[row] : null;
            if (bought == null)
            {
                Console.WriteLine("ShopSanity: an Archipelago item was bought, but the selected row isn't one. Ignoring it.");
            }
            else
            {
                _bought.Add(bought.Id);
                // the row stays on screen until the shop closes, so point it at the hidden slot: buying it again fails
                Memory.WriteByte(RowAddress(Array.IndexOf(_rows, bought)), HiddenSlot);
                Console.WriteLine($"ShopSanity: bought {_names.GetValueOrDefault(bought.Id, "AP Item")} for {bought.Price} gold.");
                SendPending(client, justBought: true);
            }

            bool moreForSale = _rows.Any(r => r != null && !_bought.Contains(r.Id));
            Memory.Write(InventorySlot(TokenSlot), moreForSale ? TokenArmed : TokenIdle);
        }

        // the shop row (0-10) the player has selected, from the menu the buy handler reads it from
        private static int? SelectedRow()
        {
            uint menu = Memory.ReadUInt(Addresses.CurrentMenu);
            if (!IsRamPointer(menu)) return null;
            uint row = Memory.ReadUInt((menu & 0x1FFFFF) + 0x0C);
            if (!IsRamPointer(row)) return null;
            int shopRow = Memory.ReadByte((row & 0x1FFFFF) + 0x17) - FirstRow;
            return shopRow >= 0 && shopRow < RowCount ? shopRow : null;
        }

        // Bought items go to the server in the background, so a slow server never stalls the shop loop. One send at
        // a time; anything the server hasn't confirmed is sent again after RetryInterval.
        private static readonly TimeSpan RetryInterval = TimeSpan.FromSeconds(5);
        private static int _sending;
        private static DateTime _lastSend = DateTime.MinValue;

        private static void SendPending(ArchipelagoClient client, bool justBought = false)
        {
            var checkedIds = client.CurrentSession.Locations.AllLocationsChecked;
            long[] pending = _bought.Where(id => !checkedIds.Contains(id)).ToArray();
            if (pending.Length == 0 || (!justBought && DateTime.UtcNow - _lastSend < RetryInterval)) return;
            if (Interlocked.Exchange(ref _sending, 1) == 1) return;
            _lastSend = DateTime.UtcNow;

            _ = Task.Run(async () =>
            {
                try
                {
                    await client.CurrentSession.Locations.CompleteLocationChecksAsync(pending);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"ShopSanity: couldn't send a shop check yet ({ex.Message}), will retry.");
                }
                finally
                {
                    Interlocked.Exchange(ref _sending, 0);
                }
            });
        }

        private static bool IsListedForBuyBack(byte slot) => (Memory.ReadUInt(InventorySlot(slot)) & 0x10000) != 0;

        private static byte[] Hidden(int row)
        {
            byte[] data = (byte[])VanillaRows[row].Clone();
            data[0] = HiddenSlot;
            return data;
        }

        private static uint InventorySlot(byte slot) => Addresses.Inventory + slot * 4u;
        private static uint RowAddress(int row) => Addresses.ShopTable + (uint)(FirstRow + row) * RowSize;
        private static void WriteRow(int row, byte[] data) => Memory.Write(RowAddress(row), data);

        private static void WriteName(int row, string name)
        {
            uint buffer = Addresses.ShopTextBuffer + (uint)row * NameSize;
            WriteString(buffer, Clean(name));
            Memory.Write(Addresses.PopupMessageTable + RowMessageIds[row] * 4, 0x80000000u | buffer);
        }

        private static void WriteString(uint address, string text)
        {
            byte[] bytes = Encoding.ASCII.GetBytes(text);
            byte[] terminated = new byte[Math.Min(bytes.Length, (int)NameSize - 1) + 1];
            Array.Copy(bytes, terminated, terminated.Length - 1);
            Memory.Write(address, terminated);
        }

        // plain ASCII without the game's control characters ('@' window command, '%' text command, '^' item name),
        // short enough to leave room for the price
        private static string Clean(string text)
        {
            var sb = new StringBuilder();
            foreach (char c in text)
            {
                if (c == '@' || c == '%' || c == '^') continue;
                if (c == '’' || c == '‘') sb.Append('\'');
                else if (c >= 0x20 && c < 0x7F) sb.Append(c);
                else sb.Append('?');
            }
            string s = sb.ToString();
            return s.Length > MaxNameLength ? s.Substring(0, MaxNameLength - 1).TrimEnd() + "." : s;
        }

        // prints the shop rows and token, to see what the game has
        public static void PrintDebug()
        {
            Console.WriteLine($"shop open: {IsShopOpen()}, level {Memory.ReadByte(Addresses.CurrentLevel)}, " +
                              $"token 0x{Memory.ReadUInt(InventorySlot(TokenSlot)):X8}, selected row {SelectedRow()?.ToString() ?? "-"}");
            for (int r = 0; r < RowCount; r++)
            {
                uint a = RowAddress(r);
                byte slot = Memory.ReadByte(a);
                Console.WriteLine($"row {FirstRow + r}: slot {slot} icon {Memory.ReadByte(a + 1)} msg {Memory.ReadUShort(a + 2)} " +
                                  $"price {Memory.ReadUShort(a + 4)} slot value 0x{Memory.ReadUInt(InventorySlot(slot)):X8} " +
                                  $"{(_rows[r] != null ? _names.GetValueOrDefault(_rows[r]!.Id, "AP Item") : "")}");
            }
        }
    }
}
