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
using Archipelago.Core.Util.Overlay;
using Newtonsoft.Json;

var gameClient = new DuckstationClient();
bool connected = gameClient.Connect();
var archipelagoClient = new ArchipelagoClient(gameClient);


// set values
const byte US_OFFSET = 0x38; // this is ADDED to addresses to get their US location
const int percentageMax = 100;
const int countMax = 32767;

// Connection details
string url;
string port;
string slot;
string password;

List<Location> GameLocations = null;

// Event Handlers


async void OnConnected(object sender, EventArgs args, ArchipelagoClient Client)
{
    Log.Logger.Information("Connected to Archipelago");
    Log.Logger.Information($"Playing {Client.CurrentSession.ConnectionInfo.Game} as {Client.CurrentSession.Players.GetPlayerName(Client.CurrentSession.ConnectionInfo.Slot)}");
    Console.WriteLine("Connected to Archipelago!");

    Console.WriteLine("Setting up player state..");

    // put here for debugging
    foreach (Location val in Client.GameState.CompletedLocations)
    {
        if (val.Name.Contains("Equipment")) {
            Console.WriteLine(val.Id.ToString(), val.Name);
        }

        if (val.Name.Contains("Skill"))
        {

        }

        if (val.Name.Contains("Life Bottle"))
        {

        }

        if (val.Name.Contains("Skill"))
        {

        }

    }
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

int SetItemMemoryValue(uint itemMemoryAddress, int itemUpdateValue, int maxCount)
{
    int addition = Math.Min(itemUpdateValue, maxCount);

    byte[] byteArray = BitConverter.GetBytes(addition);

    Memory.WriteByteArray(itemMemoryAddress, byteArray);

    // Add more types as needed
    return itemUpdateValue;
}

// Update functions with correct logic

void UpdateCurrentItemValue(string itemName, int numberUpdate, uint itemMemoryAddress, bool isCountType, bool isEquipmentType)
{
    var currentNumberAmount = Memory.ReadShort(itemMemoryAddress);

    // life bottles have 1 in the chamber before showing
    if(itemName == "Life Bottle" && currentNumberAmount == 0)
    {
        currentNumberAmount = 1;
    }

    // leaving for the sake of debugging
    //Console.WriteLine($"{ itemName} current amount: {currentNumberAmount}, update amount: {numberUpdate}");

    // there needs to be some logic here. It has to go something like:
    // if ammo and you don't have the equipment, then don't trigger, but save the ammo for when you have it.
    // There's also an interesting issue with lifebottles where when you get your first one, it's automatically equipping a sword.
    // I think i need to look into some sort of "global state" for the player using archipelago's slot data. Though i'm not sure
    // how much of this is "too much" as it says in the docs to use it sparingly. Loading Dan's state should be fine though. Hopefully.
    // just need to make sure to not trigger the state until you are actually loaded into a level.

    int maxValue = isCountType ? countMax : percentageMax; // Max count limit for gold, percentage for energy

    var newNumberAmount = isEquipmentType ? 0 : Math.Min(currentNumberAmount + numberUpdate, maxValue); // Max count limit


    SetItemMemoryValue(itemMemoryAddress, newNumberAmount, countMax);

    // if you're getting a piece of equipment like the longbow/crossbow/spear/etc give it some ammo.
    if (isEquipmentType && isCountType)
    {
        SetItemMemoryValue(itemMemoryAddress, 100, countMax);
    }

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

string ExtractDictName(string itemName)
{
    var match = Regex.Match(itemName, @"^(.*?)(?:\s*\(.*?\))?$");
    if (match.Success)
    {
        return match.Groups[1].Value.Trim();
    }
    return "N/A"; // Return the original string if no match is found  
}

void ReceiveCountType(Item item)
{
    var addressDict = Helpers.AmmoAddressDictionary;
    var amount = ExtractBracketAmount(item.Name);
    var name = ExtractDictName(item.Name);

    UpdateCurrentItemValue(item.Name, amount, addressDict[name], true, false);
}
void ReceiveChargeType(Item item)
{
    var addressDict = Helpers.AmmoAddressDictionary;
    var amount = ExtractBracketAmount(item.Name);
    var name = ExtractDictName(item.Name);
    UpdateCurrentItemValue(item.Name, amount, addressDict[name], false, false);
}

void ReceiveEquipment(Item item)
{
    var addressDict = Helpers.AmmoAddressDictionary;
    var name = ExtractDictName(item.Name);

    UpdateCurrentItemValue(item.Name, 0, addressDict[name], true, true);

}

void ReceiveLifeBottle(Item item)
{
    var addressDict = Helpers.AmmoAddressDictionary;
    
    UpdateCurrentItemValue("Life Bottle", 1, addressDict["Life Bottle"], false, false);
}

void ReceiveSkill(Item item)
{   
    // there's literally only one skill in this game but i made a skill dict anyway for future projects
    // and to keep things in line

    var skillDict = Helpers.SkillDictionary;

    UpdateCurrentItemValue("Daring Dash", 1, skillDict["Daring Dash"], false, false);
}

// logic for item receiving goes here (gold, health, ammo, etc)
void ItemReceived(object sender, ItemReceivedEventArgs args)
{
    Console.WriteLine(args.Item.Id);
    Console.WriteLine($"Item Received: {args.Item.Name}");

    switch (args.Item)
    {
        case var x when x.Name.ContainsAny("Skill"): ReceiveSkill(x); break;
        case var x when x.Name.ContainsAny("Equipment"): ReceiveEquipment(x); break;
        case var x when x.Name.ContainsAny("Life Bottle"): ReceiveLifeBottle (x); break;
        case var x when x.Name.ContainsAny("Health", "Gold Coins", "Dagger", "Chicken Drumsticks", "Crossbow", "Longbow", "Fire Longbow", "Magic Longbow", "Spear", "Copper Shield", "Silver Shield", "Gold Shield"): ReceiveCountType(x); break;
        case var x when x.Name.ContainsAny("Broadsword", "Club", "Lightning"): ReceiveChargeType(x); break;
        case null: Console.WriteLine("Received an item with null data. Skipping."); break;
        default: Console.WriteLine("Item not recognised. Skipping"); break;
    };


}



void Client_MessageReceived(object sender, Archipelago.Core.Models.MessageReceivedEventArgs e)
{
    var message = string.Join("", e.Message.Parts.Select(p => p.Text));

    //archipelagoClient.AddOverlayMessage(e.Message.ToString(), TimeSpan.FromSeconds(10)); // kinda works? Removing for now till we can test more.

    Log.Logger.Information(JsonConvert.SerializeObject(e.Message));
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

////////////////////////////
//
// Main Program Flow
//
////////////////////////////

// Make sure the connect is initialised
if (!connected)
{
    Console.WriteLine("Failed to connect to Duckstation. Is it running and loaded with a game?");
    Console.WriteLine("Press any key to exit...");
    Console.ReadKey();
    return; // Exit if not connected
}

Console.WriteLine("Successfully connected to Duckstation.");

// get the duckstation offset
try
{
    Memory.GlobalOffset = Memory.GetDuckstationOffset();
}
catch (Exception ex)
{
    Console.WriteLine($"An unexpected error occurred while getting Duckstation memory offset: {ex.Message}");
    Console.WriteLine(ex); // Print full exception for debugging
}

// wait till you're in-game
uint currentGameStatus = Memory.ReadUInt(Addresses.InGameCheck);

while (currentGameStatus != 0x800f8198) // 0x00 means not in a level
{
    currentGameStatus = Memory.ReadUInt(Addresses.InGameCheck);
    Console.WriteLine($"Waiting to be in-game...");
    await Task.Delay(5000);
}

// start AP Login

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

    //archipelagoClient.IntializeOverlayService(new WindowsOverlayService()); // kinda works ? Commenting out for now.

    // Now CurrentSession is initialized, so it's safe to subscribe
    archipelagoClient.CurrentSession.Locations.CheckedLocationsUpdated += Locations_CheckedLocationsUpdated;

    GameLocations = Helpers.BuildLocationList();


    Console.WriteLine("Built Locations list. Launching Monitor");
    await archipelagoClient.MonitorLocations(GameLocations);
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
    