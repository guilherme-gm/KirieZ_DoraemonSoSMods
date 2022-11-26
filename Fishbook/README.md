# Fishbook

Adds a new item to the general store: A fish book.

Obtaining this adds the "View fish book" option when you approach a fishing spot. The book will them display information related to fishes you found in the area.

Information is gathered while you fish and unlocked bit by bit.

Currently, the following information is tracked:
- Shadow size
- Time/Season/Weather that you may find the fish
- Whether you have catched a 5* or not (for fishguide completion)

The findings are split by season and time ranges, so if a fish appears during the entire day in every season, you will have to catch it 4 times to complete its entries: 1 time per season.

Additionaly, you can switch between an all seasons/current season view. This will give you an idea whether there are still fishes that you never caught in this season to help you hunt for them.

## Installation
You must have BepInEx v5 and kz Mod Utils installed on your game.
- [Download BepInEx v5](https://github.com/BepInEx/BepInEx/releases)
	- [How to install BepInEx](https://docs.bepinex.dev/articles/user_guide/installation/index.html)
- [kz Mod Utils page](../kzModUtils/)

After having both installed, download the latest version of the mod dll from the [Releases](https://github.com/guilherme-gm/KirieZ_DoraemonSoSMods/releases) page, extract it and add its folder to `plugins` folder.

**Important:** It must be kept insde the "Fishbook" folder.

## Building
You will need Visual Studio 2019 and .NET Framework 3.5 installed.

1. Clone this repository
2. Copy Doraemon's `Assembly-CSharp.dll` and `UnityEngine.UI.dll` into `libs` folder
3. Open the `Mods.sln` solution
4. Build it


## Contributting
See [CONTRIBUTTING.md](../CONTRIBUTTING.md).
