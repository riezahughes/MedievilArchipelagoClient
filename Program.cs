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
using MedievilArchipelago.Models;
using System.Xml.Linq;
using System.Diagnostics;
using System.Drawing;
using System.Collections;

// set values
const byte US_OFFSET = 0x38; // this is ADDED to addresses to get their US location
const byte JP_OFFSET = 0; // could add more offsfets here
const int percentageMax = 100;
const int countMax = 32767;

// Connection details
string url;
string port;
string slot;
string password;

//
bool hasFoundMem = false;
ulong offsetChoice = 0x00;

List<ILocation> GameLocations = null;

////////////////////////////
//
// Main Program Flow
//
////////////////////////////

// Make sure the connect is initialised


DuckstationClient gameClient = null;
bool clientInitializedAndConnected = false; // Renamed for clarity
int retryAttempt = 0;

while (!clientInitializedAndConnected)
{
    Console.Clear();
    retryAttempt++;
    Console.WriteLine($"\nAttempt #{retryAttempt}:");

    try
    {
        gameClient = new DuckstationClient();
        clientInitializedAndConnected = true;
    }
    catch (Exception ex)
    {
        // Catch any exception thrown during the DuckstationClient constructor call
        // or any other unexpected error during the try block.
        Console.WriteLine($"Could not find Duckstation open.");

        // Wait for 5 seconds before the next retry
        Thread.Sleep(5000); // 5000 milliseconds = 5 seconds
    }
}


Console.Clear();

bool connected = gameClient.Connect();
var archipelagoClient = new ArchipelagoClient(gameClient);

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


// wait until you can read the euro/US

// doesn't work. got wrong memory address. I tried.
//while(!hasFoundMem)
//{
//    Console.Clear();
//    short region = Memory.ReadShort(0x001c3540);
    
//    if (region == 817)
//    {
//        Console.WriteLine("PAL Copy of the game found");
//        hasFoundMem = true;
//    }
//    if (region == 31370)
//    {
//        //Memory.GlobalOffset. = Memory.GlobalOffset - US_OFFSET; // this line isn't working. Once i can get the offset working it should go with the US version of the game. *should*..
//        Console.WriteLine("US Copy of the game found. Please note that i've not done any tests with the US version of the game. This progam has been built to work with the PAL one. Use so at your own discretion.");
//        hasFoundMem = true;
//    } else
//    {
//        Console.WriteLine("Looking for game version...");
//        await Task.Delay(5000);
//    }

//};

// wait till you're in-game
uint currentGameStatus = Memory.ReadUInt(Addresses.InGameCheck);


while (currentGameStatus != 0x800f8198) // 0x00 means not in a level
{
    currentGameStatus = Memory.ReadUInt(Addresses.InGameCheck);
    Console.WriteLine($"Waiting to be in-game...");
    await Task.Delay(5000);


}




//Console.Clear();

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

var cts = new CancellationTokenSource();
try
{
    await archipelagoClient.Connect(url + ":" + port, "Medievil");
    Console.WriteLine("Connected. Attempting to Log in...");
    await archipelagoClient.Login(slot, password);
    Console.WriteLine("Logged in!");

    var overlayOptions = new OverlayOptions();
    overlayOptions.XOffset = 50;
    overlayOptions.YOffset = 500;
    overlayOptions.FontSize = 12;
    overlayOptions.TextColor = Archipelago.Core.Util.Overlay.Color.Yellow;

    var gameOverlay = new WindowsOverlayService(overlayOptions);

    archipelagoClient.IntializeOverlayService(gameOverlay); // kinda works ? Commenting out for now.

    // Now CurrentSession is initialized, so it's safe to subscribe
    archipelagoClient.CurrentSession.Locations.CheckedLocationsUpdated += Locations_CheckedLocationsUpdated;

    GameLocations = Helpers.BuildLocationList(archipelagoClient.Options);

    //Console.Clear();
    Console.WriteLine("Client is connected and watching Medievil....");
    
    ;
    _ = MemoryCheckThreads.CheckForHallOfHeroes(archipelagoClient);
    _ = archipelagoClient.MonitorLocations(GameLocations); // Use _ = to suppress warning about unawaited task


    // The main thread now dedicates itself to reading console input.
    while (!cts.Token.IsCancellationRequested)
    {
        var input = Console.ReadLine();
        if (input?.Trim().ToLower() == "exit")
        {
            cts.Cancel(); // Signal background tasks to stop
            break; // Exit the input loop
        }
        else if (input?.Trim().ToLower().Contains("hint") == true)
        {

            string hintString = input?.Trim().ToLower() == "hint" ? "!hint" : $"!hint {input.Substring(5).Trim()}";
            archipelagoClient.SendMessage(hintString);
        }
        else if (input?.Trim().ToLower() == "update")
        {
            if (archipelagoClient.GameState.CompletedLocations != null)
            {
                UpdatePlayerState(archipelagoClient.GameState.ReceivedItems);
                Console.WriteLine($"Player state updated. Total Count: {archipelagoClient.GameState.ReceivedItems.Count}");
            }
            else
            {
                Console.WriteLine("Cannot update player state: GameState or CompletedLocations is null.");
            }
        }
        else if (!string.IsNullOrWhiteSpace(input))
        {
            Console.WriteLine($"Unknown command: '{input}'");
        }
    }
}
catch (Exception ex)
{
    Console.WriteLine($"An error occurred while connecting to Archipelago: {ex.Message}");
    Console.WriteLine(ex); // Print full exception for debugging
}
finally
{
    // Perform any necessary cleanup here
    Console.WriteLine("Shutting down...");

}


async void OnConnected(object sender, EventArgs args, ArchipelagoClient Client)
{
    Log.Logger.Information("Connected to Archipelago");
    Log.Logger.Information($"Playing {Client.CurrentSession.ConnectionInfo.Game} as {Client.CurrentSession.Players.GetPlayerName(Client.CurrentSession.ConnectionInfo.Slot)}");
    Console.WriteLine("Connected to Archipelago!");

    Console.WriteLine("Checking if the game is over");

    CheckGoalCondition();

    Console.WriteLine("Setting up player state..");
    UpdatePlayerState(Client.GameState.ReceivedItems);
}

void UpdatePlayerState(List<Item> itemsCollected)
{
    //Console.WriteLine("Updating player state...");
    // get a list of all locatoins
    Dictionary<string, uint> all_items = Helpers.FlattenedInventoryStrings();

    // get a list of used locations
    var usedItems = new List<string>();

    short currentWeapon = Memory.ReadShort(Addresses.ItemEquipped);

    SetItemMemoryValue(Addresses.CurrentLifePotions, 0, 0);
    SetItemMemoryValue(Addresses.SoulHelmet, 0, 0);

    // for each location that's coming in
    bool hasEquipableWeapon = false;

    foreach (Item val in itemsCollected)
    {
        var loc = new Item();
        loc.Name = val.Name;

        switch (loc)
        {
            // Update memory
            case var x when x.Name.ContainsAny("Ammo"): 
                // no plans yet
                break;
            case var x when x.Name.Contains("Skill"): ReceiveSkill(x); break;
            case var x when x.Name.Contains("Equipment"): 
                ReceiveEquipment(x);
                if (!x.Name.Contains("Shield"))
                {   
                    hasEquipableWeapon = true;
                }
                break;
            case var x when x.Name.Contains("Life Bottle"): ReceiveLifeBottle(); break;
            case var x when x.Name.Contains("Soul Helmet"): ReceiveSoulHelmet(); break;
            case var x when x.Name.Contains("Key Item"): ReceiveKeyItem(x); break;
            case var x when x.Name.Contains("Cleared"): ReceiveLevelCleared(x); break;
            case var x when x.Name.Contains("Chalice"): ReceiveChaliceComplete(x); break;

        }

        usedItems.Add(val.Name);

    }

    Dictionary<string, uint> remainingItemsDict = all_items
        .Where(kvp => !usedItems.Contains(kvp.Key))
        .ToDictionary(kvp => kvp.Key, kvp => kvp.Value);

    // UpdateCurrentItemValue(string itemName, int numberUpdate, uint itemMemoryAddress, bool isCountType, bool isEquipmentType)
    foreach (KeyValuePair<string, uint> item in remainingItemsDict)
    {

        string itemName = item.Key;
        uint itemAddress = item.Value;

        // reset any other values
        if (itemName.ContainsAny("Skill"))
        {
            SetItemMemoryValue(itemAddress, 0, 0);
        }
        else if (itemName.ContainsAny("Equipment"))
        {
            SetItemMemoryValue(itemAddress, 65535, 65535); // Assuming 65535 is "reset/max" for equipment
        }
        else if (itemName.ContainsAny("Complete"))
        {
            SetItemMemoryValue(itemAddress, 0, 0);

        }
        else if (itemName.ContainsAny("Key Item"))
        {
            SetItemMemoryValue(itemAddress, 65535, 65535);

        }

    }


    if (!hasEquipableWeapon)
    {
        DefaultToArm();
    } else
    {
        EquipWeapon(currentWeapon);
    }
}

void EquipWeapon(int value)
{
    SetItemMemoryValue(Addresses.ItemEquipped, value, value);
}

void DefaultToArm()
{
    SetItemMemoryValue(Addresses.ItemEquipped, 8, 8);
    SetItemMemoryValue(Addresses.SmallSword, 65535, 65535);
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

    if(currentNumberAmount == -1 && itemName.ContainsAny("Ammo", "Charge")){
        return;
    }

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
    } else if (isEquipmentType && !isCountType)
    {
        SetItemMemoryValue(itemMemoryAddress, 4096, countMax);
    }

}

int ExtractBracketAmount(string itemName)
{
    var match = Regex.Match(itemName, @"\((\d+)\)");
    if (match.Success && int.TryParse(match.Groups[1].Value, out int bracketAmount))
    {
        return bracketAmount;
    }
    return 0; 
}

string ExtractDictName(string itemName)
{

    var colonMatch = Regex.Match(itemName, @"^[^:]*:\s*(.*)$");
    if (colonMatch.Success)
    {
        return colonMatch.Groups[1].Value.Trim();
    }

    var parenthesesMatch = Regex.Match(itemName, @"^(.*?)(?:\s*\(.*?\))?$");
    if (parenthesesMatch.Success)
    {
        return parenthesesMatch.Groups[1].Value.Trim();
    }
    return "N/A";
}

string ExtractKeyItemName(string itemName)
{
    var dashRemovedMatch = Regex.Match(itemName, @"^(?:Key Item:\s*)?([^-]+?)(?: - .*)?$");

    string cleanedName = itemName; 

    if (dashRemovedMatch.Success)
    {
        cleanedName = dashRemovedMatch.Groups[1].Value.Trim();
    }
    else
    {
        var keyItemPrefixMatch = Regex.Match(itemName, @"^Key Item:\s*(.*)$");
        if (keyItemPrefixMatch.Success)
        {
            cleanedName = keyItemPrefixMatch.Groups[1].Value.Trim();
        }
        else
        {

            cleanedName = itemName.Trim();
        }
    }


    cleanedName = Regex.Replace(cleanedName, @"\s*\d+$", "").Trim();

    return cleanedName;
}

void ReceiveCountType(Item item)
{
    var addressDict = Helpers.StatusAndInventoryAddressDictionary();
    var amount = ExtractBracketAmount(item.Name);
    var name = ExtractDictName(item.Name);

    UpdateCurrentItemValue(item.Name, amount, addressDict["Ammo"][name], true, false);
}

void ReceiveChargeType(Item item)
{
    var addressDict = Helpers.StatusAndInventoryAddressDictionary();
    var amount = ExtractBracketAmount(item.Name);
    var name = ExtractDictName(item.Name);

    UpdateCurrentItemValue(item.Name, amount, addressDict["Ammo"][name], false, false);
}

void ReceiveEquipment(Item item)
{
    var addressDict = Helpers.StatusAndInventoryAddressDictionary();
    var name = ExtractDictName(item.Name);

    var isChargeType = item.Name.ContainsAny("Broadsword", "Club", "Lightning");

    UpdateCurrentItemValue(item.Name, 0, addressDict["Equipment"][name], !isChargeType, true);

}


void ReceiveLevelCleared(Item level)
{
    var addressDict = Helpers.StatusAndInventoryAddressDictionary();
    var name = ExtractDictName(level.Name);

    UpdateCurrentItemValue(level.Name, 16, addressDict["Level Status"][name], true, true);
}

void ReceiveChaliceComplete(Item level)
{
    var addressDict = Helpers.StatusAndInventoryAddressDictionary();
    var name = ExtractDictName(level.Name);

    UpdateCurrentItemValue(level.Name, 19, addressDict["Level Status"][name], true, true);
}

void ReceiveKeyItem(Item item)
{
    // commented out because i need to make a list of player data addresses to deal with this.
    var addressDict = Helpers.StatusAndInventoryAddressDictionary();
    var name = ExtractKeyItemName(item.Name);

    UpdateCurrentItemValue(item.Name, 0, addressDict["Key Items"][name], true, true);

}

void ReceiveLifeBottle()
{
    var addressDict = Helpers.StatusAndInventoryAddressDictionary();
    
    UpdateCurrentItemValue("Life Bottle", 1, addressDict["Player Stats"]["Life Bottle"], true, false);
}

void ReceiveSoulHelmet()
{
    var addressDict = Helpers.StatusAndInventoryAddressDictionary();
    UpdateCurrentItemValue("Soul Helmet", 1, addressDict["Key Items"]["Soul Helmet"], true, false);
}

void ReceiveStatItems(Item item)
{
    var addressDict = Helpers.StatusAndInventoryAddressDictionary();
    var amount = ExtractBracketAmount(item.Name);
    var name = ExtractDictName(item.Name);

    UpdateCurrentItemValue(item.Name, amount, addressDict["Player Stats"][name], true, false);
}

void ReceiveRune(Item item)
{
    // need to do the logic for this
    return;
    // get the type of rune

    //var runeName = GetRuneName(item.Name);
    //var addressDict = Helpers.InventoryAddressDictionary;

    //UpdateCurrentItemValue(item.Name, 1, addressDict[runeName], false, false);
}

void ReceiveSkill(Item item) { 
    // setting it here till i fix my ridiculous update function
    SetItemMemoryValue(Addresses.DaringDashSkill, 1, 1);
}

// logic for item receiving goes here (gold, health, ammo, etc)
void ItemReceived(object sender, ItemReceivedEventArgs args)
{

    switch (args.Item)
    {
        // incoming runes need added here
        case var x when x.Name.ContainsAny("Rune"): ReceiveRune(x); break;
        case var x when x.Name.ContainsAny("Skill"): ReceiveSkill(x); break;
        case var x when x.Name.ContainsAny("Equipment"): ReceiveEquipment(x); break;
        case var x when x.Name.ContainsAny("Life Bottle"): ReceiveLifeBottle(); break;
        case var x when x.Name.ContainsAny("Soul Helmet"): ReceiveSoulHelmet(); break;
        case var x when x.Name.ContainsAny("Key Item"): ReceiveKeyItem(x); break;
        case var x when x.Name.ContainsAny("Health", "Gold Coins", "Energy"): ReceiveStatItems(x); break;
        case var x when x.Name.ContainsAny("Daggers", "Ammo", "Chicken Drumsticks", "Crossbow", "Longbow", "Fire Longbow", "Magic Longbow", "Spear", "Copper Shield", "Silver Shield", "Gold Shield"): ReceiveCountType(x); break;
        case var x when x.Name.ContainsAny("Broadsword", "Club", "Lightning"): ReceiveChargeType(x); break;
        case null: Console.WriteLine("Received an item with null data. Skipping."); break;
        default: Console.WriteLine($"Item not recognised. ({args.Item.Name}) Skipping"); break;
    };

    UpdatePlayerState(archipelagoClient.GameState.ReceivedItems);
}

void Client_MessageReceived(object sender, Archipelago.Core.Models.MessageReceivedEventArgs e)
{
    var message = string.Join("", e.Message.Parts.Select(p => p.Text));

    archipelagoClient.AddOverlayMessage(e.Message.ToString());

    Log.Logger.Information(JsonConvert.SerializeObject(e.Message));
    Console.WriteLine($"Message: {message}");
}

void Client_LocationCompleted(object sender, LocationCompletedEventArgs e)
{
    UpdatePlayerState(archipelagoClient.GameState.ReceivedItems);
    CheckGoalCondition();
}


void Locations_CheckedLocationsUpdated(System.Collections.ObjectModel.ReadOnlyCollection<long> newCheckedLocations)
{
    CheckGoalCondition();
}



void CheckGoalCondition()
{

    // If you 

    if (GameLocations == null || archipelagoClient.CurrentSession?.Locations?.AllLocationsChecked == null)
        return;


    if (archipelagoClient?.GameState.CompletedLocations.Any(x => x.Name == "Cleared: Zaroks Lair") == true)
    {
        archipelagoClient.SendGoalCompletion();
        Console.WriteLine("Defeated Zarok");
        return;
    }
}

// traps need added here and logic added into what i have already

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


    