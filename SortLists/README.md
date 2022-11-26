# Sort Lists

Sorts some in-game menu lists alphabetically.

Currently the following lists are supported (some are disabled by default, see Config on how to enable it):
- Kitchen's recipe list (default: sorted)
- Cafe's recipe shop (default: sorted)
- Cafe's produce shop (default: original / not sorted)
- Cafe's meals shop (default: original / not sorted)

If there is any other list that you feel the need to be sorted, let me know (or submit a PR!).

## Installation
You must have BepInEx v5 and kz Mod Utils installed on your game.
- [Download BepInEx v5](https://github.com/BepInEx/BepInEx/releases)
	- [How to install BepInEx](https://docs.bepinex.dev/articles/user_guide/installation/index.html)

After having BepInEx installed, download the latest version of the mod dll from the [Releases](https://github.com/guilherme-gm/KirieZ_DoraemonSoSMods/releases) page and add it to your BepInEx's plugins folder.

## Config
You can change which of the supported lists are to be sorted or not.

After installing the mod and running the game once, go to `<Game folder>/BepInEx/config` folder and open `io.github.guilherme-gm.DoraemonSoSMods.sortLists.cfg` in a text editor.

Once open, just change the `true`/`false` for each list as you wish. All of them contains a short description about what it does.

Having changed it as you wish, save the file and restart the game.

## Building
You will need Visual Studio 2019 and .NET Framework 3.5 installed.

1. Clone this repository
2. Copy Doraemon's `Assembly-CSharp.dll` and `UnityEngine.UI.dll` into `libs` folder
3. Open the `Mods.sln` solution
4. Build it


## Contributting
See [CONTRIBUTTING.md](../CONTRIBUTTING.md).
