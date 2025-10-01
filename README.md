# Twitch Toolkit

Twitch Toolkit is a mod for the game RimWorld that allows Twitch viewers to affect the game through digital currency.

---

## Project History

- **TwitchStories** was the original mod, created by Finally.
- **Twitch Toolkit** was forked and significantly developed from TwitchStories by hodlhodl, evolving into an independent mod.
- The project is now actively maintained and developed by the Toolkit Community.

---

## License

Twitch Toolkit is licensed under the [GNU Affero General Public License v3.0 (AGPL-3.0)](https://www.gnu.org/licenses/agpl-3.0.html).
See the `LICENSE` file for the full license text and the `NOTICE` file for the detailed project lineage and attribution.

---

## Installation

### Steam Workshop

The mod is available on the [Steam Workshop](https://steamcommunity.com/sharedfiles/filedetails/?id=3013874066). You
will also need to subscribe to [ToolkitCore](https://steamcommunity.com/sharedfiles/filedetails/?id=3013877477), as well
as [Harmony](https://steamcommunity.com/workshop/filedetails/?id=2009463077).

### Manual Installation

1. Download the latest release from the [Releases](https://github.com/harleyknd1/twitchtoolkit/releases) page.
2. Download the [Harmony](https://github.com/pardeike/HarmonyRimWorld/releases) library.
3. Download the [ToolkitCore](https://github.com/harleyknd1/toolkitcore/releases) library.
4. Extract the Harmony and ToolkitCore archives to your RimWorld Mods folder.
5. Extract the TwitchToolkit archive to your RimWorld Mods folder.
6. Enable the mods in the RimWorld Mods menu.
7. Enjoy!

---

## Contributing

Contributions, bug reports, and feature requests are welcome!

--

## Building

To build the mod, you will need the following:

- [Mono](https://www.mono-project.com/download/stable/) if you’re not on Windows.
- [.NET 4.7.2 Developer Pack](https://dotnet.microsoft.com/en-us/download/dotnet-framework/net472) if you're on Windows.
- [Git](https://git-scm.com/downloads)
- [RimWorld](https://rimworldgame.com/)
- Your favorite IDE.

We recognize people’s games are different, so we’ve provided properties you may change if the project doesn’t build for
you:

- `ToolkitCoreDir` is a property that points to the directory where your copy of ToolkitCore is located.
- `SteamRootDir` is a property that points to the directory where your copy of Steam is located.
- `RootDestinationDir` is a property that points to the directory where you’ll house your compiled copy of Twitch Toolkit. The build process will insert a valid "vX.X\Assemblies" subdirectory at build time.
- `RimWorldModsDir` is a property that points to the directory where your mods folder of RimWorld is located.

You may set either of these properties at build time in your IDE, or through the command line:

- Command line: `msbuild /p:ToolkitCoreDir=C:\path\to\toolkitcore ...`
- IDE: Refer to your IDE’s documentation on how to set global properties.

---

## Community and Support

- **GitHub**: https://github.com/harleyknd1/twitchtoolkit
- **Discord**: https://discord.gg/qrtg224
- **Issue Tracker**: https://github.com/harleyknd1/twitchtoolkit/issues

---

## Acknowledgements

Special thanks to:
- Finally for creating TwitchStories.
- [hodlhodl](https://github.com/hodlhodl1132) for creating TwitchToolkit.
- Toolkit Community—Ongoing maintainers and contributors.

---

*Happy Modding!*
