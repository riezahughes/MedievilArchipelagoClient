# Medievil Archipelago Client

## Requirements
To play medievil on archipelago requires:

- A windows machine (for now)
- [Archipelago](https://archipelago.gg) installed
- The latest copy of [the medievil client](https://github.com/riezahughes/MedievilArchipelagoClient/releases)
- The latest copy of [the medievil AP World](https://github.com/riezahughes/MedievilAPWorld/releases)
- A legal copy of the EU Version of Medievil
- [Duckstation] (https://www.duckstation.org)

# Setup

- Install the APWorld in Archipelago
- Generate the templates
- Copy and paste a copy of the medievil template into the `players` folder of your archipelago install. If it doesn't exist, make it.
- Edit the yaml in whatever you feel like. Almost every options doesn't work, but you might want to change the slot name
- Generate the game
- Host the game using the zip that's generated in your `output` folder of your archipelago install (either on your own server, locally or on [the archipelagos website](https://archipelago.gg/uploads) )
- Boot up the game in duckstation.
- Run the client
- Fill in the blanks to connect to the server
- Start playing!

# Development
- Clone the repository
- Create an appsettings.Local.Json in the root of the project
- Create an AP room on the archipelago website and fill the file with the following content:

```json
{
    "port": "portnumber",
    "slot": "slotname",
    "pass": "passifused"
}

```

- Running debug will now automatically connect you to the room without the need of you filling in the fields

Note: Sometimes the ap client can be funny in debug mode. When you run it, check visual steudio's play button and see if you need to hit continue. There's a race condition under the hood. In a release you don't see it.