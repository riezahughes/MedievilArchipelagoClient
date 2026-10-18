# Medievil Archipelago Client

## Requirements
To play medievil on archipelago requires:

- A Windows or macOS machine
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
- Run the client (macOS users: see [Running on macOS](#running-on-macos) below)
- Fill in the blanks to connect to the server
- Start playing!

# Running on macOS

## Which download?

From the [releases page](https://github.com/riezahughes/MedievilArchipelagoClient/releases), grab the zip for your Mac:

- `MedievilArchipelago_<version>_osx-arm64.zip` for Apple Silicon Macs (M1, M2, M3, M4...)
- `MedievilArchipelago_<version>_osx-x64.zip` for older Intel Macs

Not sure which you have? Click the Apple menu > **About This Mac** and look at "Chip" (Apple M-something = arm64) or "Processor" (Intel = x64).

## First launch

1. Unzip the download. Keep `MedievilArchipelago`, `libcimgui.dylib` and the `Assets` folder together in the same folder.
2. macOS blocks apps from unidentified developers the first time you open them. To get past this, **right-click** (or Control-click) `MedievilArchipelago` and choose **Open**, then **Open** again on the warning.
   - If macOS only offers "Move to Bin" / "Done", go to **System Settings > Privacy & Security**, scroll down, and click **Open Anyway** next to the MedievilArchipelago message.
   - Alternatively, run this once in Terminal from the unzipped folder: `xattr -d com.apple.quarantine MedievilArchipelago`
3. After the first time, you can just double-click it.

## Running the client

1. Start DuckStation and boot Medievil **before** opening the client.
2. Double-click `MedievilArchipelago`. A Terminal window will open.
3. **Enter your Mac password** when asked. macOS only lets programs with administrator access read another app's memory, which the client needs to see what's happening in DuckStation. Nothing appears as you type, that's normal. Press Enter when done.
4. **Enter the DuckStation memory address.** On macOS the client can't find this by itself:
   1. In DuckStation, open **Tools > Memory Scanner**.
   2. In the bottom right, find the line `Address of RAM for HxD Usage: 0x000000011c400000` (your number will be different).
   3. Copy the `0x...` address (or the whole line) and paste it into the client, then press Enter.
5. Fill in the server, port, slot name and password as normal and start playing!

> The memory address can change when DuckStation is restarted, so grab it fresh each session.

## Troubleshooting

- **"zsh: killed" or the window closes instantly**: the download may have been damaged or blocked. Re-download the zip and follow the first launch steps again.
- **Sorry, try again**: the password was wrong. Use the password you log in to your Mac with.
- **Not an administrator**: your macOS user account must be an admin account to run the client.
- **No jingle sounds**: in-game jingles are currently Windows-only.

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