// See https://aka.ms/new-console-template for more information

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Archipelago.Core;
using Archipelago.Core.GameClients;
using Archipelago.Core.Models;
using Archipelago.Core.Traps;
using Archipelago.Core.Util;
using MedievilArchipelago;
using Serilog;
using Helpers = MedievilArchipelago.Helpers;
using System.Threading;

var gameClient = new DuckstationClient();
bool connected = gameClient.Connect();
var archipelagoClient = new ArchipelagoClient(gameClient);


// set values
byte US_OFFSET = 0x38;
int percentageMax = 100;
int countMax = 32767;

// Connection details
string url;
string port;
string slot;
string password;

List<Location> GameLocations = null;

// Event Handlers

void OnConnected(object sender, EventArgs args, ArchipelagoClient Client)
{
    Log.Logger.Information("Connected to Archipelago");
    Log.Logger.Information($"Playing {Client.CurrentSession.ConnectionInfo.Game} as {Client.CurrentSession.Players.GetPlayerName(Client.CurrentSession.ConnectionInfo.Slot)}");
    Console.WriteLine("Connected to Archipelago!");
}

async void OnDisconnected(object sender, EventArgs args, ArchipelagoClient Client)
{
    Console.WriteLine("Disconnected from Archipelago. Reconnecting...");
    try
    {
        await Client.Login(slot, password);
        Console.WriteLine("Reconnected to Archipelago!");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Failed to reconnect: {ex.Message}");
    }
}

// Item Getters

int GetItemMemoryValue(ulong itemMemoryAddress)
{
    var item = Memory.ReadByte(itemMemoryAddress);
    return (int)item;
}

int SetItemMemoryValue(string itemName, ulong itemMemoryAddress, int itemUpdateValue, int maxCount)
{
    int addition = itemUpdateValue > maxCount ? maxCount : itemUpdateValue;

    var count = archipelagoClient.GameState?.ReceivedItems.Where(x => x.Name.StartsWith(itemName)).Sum(x => x.Quantity) ?? 0;

    Memory.WriteByte(itemMemoryAddress, (byte)addition);

    // Add more types as needed
    return itemUpdateValue;
}

    //item setters

// Update functions with correct logic

void UpdateCurrentItemValue(string itemName, int numberUpdate, uint itemMemoryAddress, bool isCountType)
{
    var currentNumberAmount = GetItemMemoryValue(itemMemoryAddress);
    int maxValue = isCountType ? countMax : percentageMax; // Max count limit for gold, percentage for energy
    var newNumberAmount = Math.Min(currentNumberAmount + numberUpdate, maxValue); // Max count limit
    SetItemMemoryValue(itemName, itemMemoryAddress, newNumberAmount, countMax);
}

int ExtractBracketAmount(string itemName)
{
    var match = Regex.Match(itemName, @"\((\d+)\)");
    if (match.Success && int.TryParse(match.Groups[1].Value, out int bracketAmount))
    {
        return bracketAmount;
    }
    return 0; // Return 0 if no valid number is found
}


// logic for item receiving goes here (gold, health, ammo, etc)
void ItemReceived(object sender, ItemReceivedEventArgs args)
{
    Console.WriteLine(args.Item.Id);
    Console.WriteLine($"Item Received: {args.Item.Name} x{args.Item.Quantity}");

    if (args.Item == null)
    {
        Console.WriteLine("Received an item with null data. Skipping.");
        return;
    }

    if (args.Item.Name.Contains("Gold"))
    {
        Console.WriteLine("Gold has been received!");
        var amount = ExtractBracketAmount(args.Item.Name);
        Console.WriteLine($"{amount} to be precise!");
        UpdateCurrentItemValue("Gold", amount, Addresses.CurrentGold, true);
    }
    else if (args.Item.Name.Contains("Broadsword"))
    {
        Console.WriteLine("Broadsword Charge has been received!");
        var amount = ExtractBracketAmount(args.Item.Name);
        Console.WriteLine($"{amount} to be precise!");
        UpdateCurrentItemValue("\"Broadsword", amount, Addresses.BroadswordCharge, false);
    }
    else if (args.Item.Name.Contains("Daggers"))
    {
        Console.WriteLine("Daggers has been received!");
        var amount = ExtractBracketAmount(args.Item.Name);
        Console.WriteLine($"{amount} to be precise!");
        UpdateCurrentItemValue("\"Daggers", amount, Addresses.DaggersAmmo, true);
    }
    else if (args.Item.Name.Contains("Club"))
    {
        Console.WriteLine("Club Charge has been received!");
        var amount = ExtractBracketAmount(args.Item.Name);
        Console.WriteLine($"{amount} to be precise!");
        UpdateCurrentItemValue("\"Club", amount, Addresses.ClubCharge, false);
    }
    else if (args.Item.Name.Contains("Chicken Drumsticks"))
    {
        Console.WriteLine("Chicken Drumsticks received!");
        var amount = ExtractBracketAmount(args.Item.Name);
        Console.WriteLine($"{amount} to be precise!");
        UpdateCurrentItemValue("\"Club", amount, Addresses.ChickenDrumsticksAmmo, true);
    }
    else if (args.Item.Name.Contains("Crossbow"))
    {
        Console.WriteLine("Crossbow Ammo received!");
        var amount = ExtractBracketAmount(args.Item.Name);
        Console.WriteLine($"{amount} to be precise!");
        UpdateCurrentItemValue("\"Club", amount, Addresses.CrossbowAmmo, true);
    }
    else if (args.Item.Name.StartsWith("Longbow"))
    {
        Console.WriteLine("Longbow Ammo received!");
        var amount = ExtractBracketAmount(args.Item.Name);
        Console.WriteLine($"{amount} to be precise!");
        UpdateCurrentItemValue("\"Longbow", amount, Addresses.LongbowAmmo, true);
    }
    else if (args.Item.Name.Contains("Fire Longbow"))
    {
        Console.WriteLine("Fire Longbow Ammo received!");
        var amount = ExtractBracketAmount(args.Item.Name);
        Console.WriteLine($"{amount} to be precise!");
        UpdateCurrentItemValue("\"Fire Longbow", amount, Addresses.FireLongbowAmmo, true);
    }
    else if (args.Item.Name.Contains("Magic Longbow"))
    {
        Console.WriteLine("Magic Longbow Ammo received!");
        var amount = ExtractBracketAmount(args.Item.Name);
        Console.WriteLine($"{amount} to be precise!");
        UpdateCurrentItemValue("\"Magic Longbow", amount, Addresses.MagicLongbowAmmo, true);
    }
    else if (args.Item.Name.Contains("Spear"))
    {
        Console.WriteLine("Spear Ammo received!");
        var amount = ExtractBracketAmount(args.Item.Name);
        Console.WriteLine($"{amount} to be precise!");
        UpdateCurrentItemValue("\"Spear", amount, Addresses.SpearAmmo, true);
    }
    else if (args.Item.Name.Contains("Lightning Charge"))
    {
        Console.WriteLine("Lightning Charge received!");
        var amount = ExtractBracketAmount(args.Item.Name);
        Console.WriteLine($"{amount} to be precise!");
        UpdateCurrentItemValue("\"Lightning", amount, Addresses.LightningCharge, false);
    }
    else if (args.Item.Name.Contains("Copper Shield"))
    {
        Console.WriteLine("Copper Shield Charge received!");
        var amount = ExtractBracketAmount(args.Item.Name);
        Console.WriteLine($"{amount} to be precise!");
        UpdateCurrentItemValue("\"Copper Shield", amount, Addresses.CopperShieldAmmo, true);
    }
    else if (args.Item.Name.Contains("Silver Shield"))
    {
        Console.WriteLine("Silver Shield Charge received!");
        var amount = ExtractBracketAmount(args.Item.Name);
        Console.WriteLine($"{amount} to be precise!");
        UpdateCurrentItemValue("\"Silver Shield", amount, Addresses.SilverShieldAmmo, true);
    }
    else if (args.Item.Name.Contains("Gold Shield"))
    {
        Console.WriteLine("Gold Shield Charge received!");
        var amount = ExtractBracketAmount(args.Item.Name);
        Console.WriteLine($"{amount} to be precise!");
        UpdateCurrentItemValue("\"Gold Shield", amount, Addresses.GoldShieldAmmo, true);
    }
}


void Client_MessageReceived(object sender, Archipelago.Core.Models.MessageReceivedEventArgs e)
{
    var message = string.Join("", e.Message.Parts.Select(p => p.Text));
    Console.WriteLine($"Message: {message}");
}

void Client_LocationCompleted(object sender, LocationCompletedEventArgs e)
{
    CheckGoalCondition();
}

void Locations_CheckedLocationsUpdated(System.Collections.ObjectModel.ReadOnlyCollection<long> newCheckedLocations)
{
    CheckGoalCondition();
}

void CheckGoalCondition()
{
    //var currentEggs = CalculateCurrentEggs();
    if (GameLocations == null || archipelagoClient.CurrentSession?.Locations?.AllLocationsChecked == null)
        return;

    if (archipelagoClient.CurrentSession.Locations.AllLocationsChecked.Any(x =>
        GameLocations.FirstOrDefault(y => y.Id == x)?.Name == "Zarok Defeated"))
    {
        archipelagoClient.SendGoalCompletion();
        Console.WriteLine("Goal completed! Sent goal completion to Archipelago.");
    }
}

async void RunLagTrap()
{
    using (var lagTrap = new LagTrap(TimeSpan.FromSeconds(20)))
    {
        lagTrap.Start();
        Console.WriteLine("Lag Trap activated for 20 seconds!");
        await lagTrap.WaitForCompletionAsync();
        Console.WriteLine("Lag Trap ended.");
    }
}

// Main Program Flow

if (!connected)
{
    Console.WriteLine("Failed to connect to Duckstation. Is it running and loaded with a game?");
    Console.WriteLine("Press any key to exit...");
    Console.ReadKey();
    return; // Exit if not connected
}

Console.WriteLine("Successfully connected to Duckstation.");
try
{
    ulong cheatChoice = 0x000EEE68;
    Memory.GlobalOffset = Memory.GetDuckstationOffset();
    Console.WriteLine($"Memory Location: {cheatChoice} ");
    Memory.WriteByte(cheatChoice, 0x09); // Adjust offset as needed
}
catch (Exception ex)
{
    Console.WriteLine($"An unexpected error occurred while getting Duckstation memory offset: {ex.Message}");
    Console.WriteLine(ex); // Print full exception for debugging
}

Console.WriteLine("Enter AP url: eg,archipelago.gg");
string lineUrl = Console.ReadLine();

url = string.IsNullOrWhiteSpace(lineUrl) ? "wss://archipelago.gg" : "wss://" + lineUrl;

Console.WriteLine("Enter Port: eg, 80001");
port = Console.ReadLine();

Console.WriteLine("Enter Slot Name:");
slot = Console.ReadLine();

Console.WriteLine("Room Password:");
string linePassword = Console.ReadLine();
password = string.IsNullOrWhiteSpace(linePassword) ? "None" : linePassword;

Console.WriteLine("Details:");
Console.WriteLine($"URL:{url}:{port}");
Console.WriteLine($"Slot: {slot}");
Console.WriteLine($"Password: {password}");

if (string.IsNullOrWhiteSpace(slot))
{
    Console.WriteLine("Slot name cannot be empty. Please provide a valid slot name.");
    return;
}

Console.WriteLine("Got the details! Attempting to connect to Archipelagos main server");

// Register event handlers
archipelagoClient.Connected += (sender, args) => OnConnected(sender, args, archipelagoClient);
archipelagoClient.Disconnected += (sender, args) => OnDisconnected(sender, args, archipelagoClient); ;
archipelagoClient.ItemReceived += ItemReceived;
archipelagoClient.MessageReceived += Client_MessageReceived;
archipelagoClient.LocationCompleted += Client_LocationCompleted;

try
{
    await archipelagoClient.Connect(url + ":" + port, "Medievil");
    Console.WriteLine("Connected. Attempting to Log in...");
    await archipelagoClient.Login(slot, password);
    Console.WriteLine("Logged in!");

    // Now CurrentSession is initialized, so it's safe to subscribe
    archipelagoClient.CurrentSession.Locations.CheckedLocationsUpdated += Locations_CheckedLocationsUpdated;

    GameLocations = Helpers.BuildLocationList();
    Console.WriteLine("Loaded Locations");
    await archipelagoClient.MonitorLocations(GameLocations);
    Console.WriteLine("Monitoring Locations...:");
}
catch (Exception ex)
{
    Console.WriteLine($"An error occurred while connecting to Archipelago: {ex.Message}");
    Console.WriteLine(ex); // Print full exception for debugging
}

var cts = new CancellationTokenSource();

Task inputTask = Task.Run(() =>
{
    // listening to background commands.
    Console.WriteLine("Type 'exit' to quit the application.");
    while (!cts.Token.IsCancellationRequested)
    {

        var input = Console.ReadLine();
        //if (input?.Trim().ToLower() == "exit")
        //{
        //    cts.Cancel();
        //    break;
        //}
        if (input?.Trim().ToLower() == "/hint")
        {
            Console.WriteLine("Hints aren't supported yet. Sorry!");
            break;
        }
    }
});

try
{
    // Keep the application running until cancellation is requested
    await inputTask;
}
finally
{
    // Perform any necessary cleanup here
    Console.WriteLine("Shutting down...");
}