// See https://aka.ms/new-console-template for more information

using System;
using Archipelago.Core;
using Archipelago.Core.GameClients;
using Archipelago.Core.Util;
using MedievilArchipelago;

var gameClient = new DuckstationClient();

bool connected = gameClient.Connect();

var archipelagoClient = new ArchipelagoClient(gameClient);

byte US_OFFSET = 0x38;

if (!connected)
{
    Console.WriteLine("Failed to connect to Duckstation. Is it running and loaded with a game?");
    Console.WriteLine("Press any key to exit...");
    Console.ReadKey();
    return; // Exit if not connected
}

Console.WriteLine("Successfully connected to Duckstation.");
Console.WriteLine($"{}");
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
Console.
Console.ReadLine();
Console.WriteLine("Enter Port: eg, 80001");
