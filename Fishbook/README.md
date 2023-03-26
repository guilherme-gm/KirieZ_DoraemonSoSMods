# Fishbook
<!-- nexus-disable -->
<p align="center">
	<img src="../docs/modImages/Fishbook.gif" width="600px"/>
</p>
<!-- nexus-enable -->
<!-- nexus-only
[center][img]@@@RawRepositoryURL@@@/docs/modImages/Fishbook.gif[/img][/center]
-->

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

<!-- nexus-disable -->
After having them installed:
1. download the latest version of the mod files from [Releases](https://github.com/guilherme-gm/KirieZ_DoraemonSoSMods/releases) page and extract it somewhere
2. Copy the `Fishbook` folder into your game's `BepInEx/plugins/` folder

<!-- nexus-enable -->
<!-- nexus-only
After having them installed:
[list=1]
[*] download the latest version of the mod from Files tab and extract it somewhere
[*] Copy the "Fishbook" folder into your game&#39;s  [font=Courier New][color=#ffff00]BepInEx/plugins/[/color][/font]  folder
[/list]
-->

<!-- nexus-disable -->
## Building
You will need Visual Studio 2019 and .NET Framework 3.5 installed.

1. Clone this repository
2. Copy Doraemon's `Assembly-CSharp.dll` and `UnityEngine.UI.dll` into `libs` folder
3. Open the `Mods.sln` solution
4. Build it

<!-- nexus-enable -->

## Found an issue?
If you have found any issue or have questions about it, feel free to open an issue in the [GitHub repository](https://github.com/guilherme-gm/KirieZ_DoraemonSoSMods/issues).

<!-- nexus-only
You may also message on the mod board here in Nexus and I will check sometime.
-->


## Contributting
See [CONTRIBUTTING.md](../CONTRIBUTTING.md).
