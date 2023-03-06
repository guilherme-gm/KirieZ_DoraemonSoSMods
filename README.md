# KirieZ_DoraemonSoSMods
Mods for Doraemon Story of Seasons originally made by KirieZ (@guilherme-gm)


## kz Mod Utils

Library with utilities to make mods. The aim of this lib is to provide other mods a direct and consistent interface to do their work, so they don't have to rely on the game internals in case it ever changes.

It also helps solve conflicts between mods, for example, by providing a single and incremental item ID system that mods may extend.

## List of mods

### Alternative Furniture Rotation
> **NOTE:** This mod was moved to [Enhancements And Tweaks](#enhancements-and-tweaks) and will soon be removed.
> Please, migrate to it.

Changes how furniture placement works to provide a different rotation system.
1. It doesn't force the rotation to the character direction;
2. The objects have a different pivot now;
3. Adds a "Rotate" button so you can try different positioning from the same spot;
4. Adds a directional arrow to help determining direction of objects where direct matters (e.g.: Bed).

I won't call it as "better" because it is also not that fluid, but it is an alternative way that gives a bit more control / less side effects.

**Note:** This mod will probably conflict with other mods touch furniture placement, because it replaces the main rotation method.

![Alterative Furniture Rotation](./docs/modImages/AlternativeFurnitureRotation.gif)

### Confirm Beehive Removal
> **NOTE:** This mod was moved to [Enhancements And Tweaks](#enhancements-and-tweaks) and will soon be removed.
> Please, migrate to it.

Adds a dialog to confirm before you remove a placed beehive. This prevents you from accidentally removing a beehive that has bees and end up losing Honey and Bees.

![Confirm beehive removal](./docs/modImages/ConfirmBeehiveRemoval.gif)

### Dump Master
Utility to dump the binary resources as text. This is a prototype and quite unstable.

### Enhancements and Tweaks
This mod incorporates several Quality of Life and minor adjustments to the game to improve progression and general playing experience (in the mod-author's vision).
You are able to toggle each change separately to pick only the ones you want.

Summarized list of changes: (View [mod page](./EnhancementsAndTweaks/) for details)
- **(Adjustment)** Make tools consume 1 of stamina regardless of the charging level
- **(QoL)** Put a new system to rotate/place objects
<p align="center">
	<img src="./docs/modImages/AlternativeFurnitureRotation.gif" width="400px" />
</p>

- **(QoL)** Add a confirmation dialog before you remove a Beehive
<p align="center">
	<img src="./docs/modImages/ConfirmBeehiveRemoval.gif" width="400px" />
</p>

### Event Alert
Displays an alert using the event log (the place where stamina status / item obtained / etc are displayed) when a festival is about to start.

### Fishbook
Adds a fishbook item to be bought. This item will track your progress as you catch fishes on the game and display info about each fishing spot when you get near them.

This mod is pretty much done, but still could use some visual improvements.

![Fishbook](./docs/modImages/Fishbook.gif)

### Map Shop Times
Adds a new button to the minimap window to toggle a small extra window that shows shops opening times so you don't have to check external guides.

![Map Shop times](./docs/modImages/ShopTimesMod.gif)

### Show Item Price
Adds a new line to item descriptions in inventory stating the value for which you would sell it. It takes into account the quality bonus.

![Show item price](./docs/modImages/ShowItemPrice.gif)

### Sort Lists
Displays some of the in-game list menus alphabetically sorted instead of in a "random" order. This make it easier to find what you are looking for in long lists.

### Stamina Bar
Displays a little window with your current stamina, so you don't have to pause the game to check.

![Stamina Bar mod](./docs/modImages/StaminaBarMod.gif)

## Doramon Story of Seasons mod template settings
- TFM: net35
- Unity version: 2017.4.10

## Building
To build those mods yourself you will need .NET Framework installed (per BepInEx dependency) and Visual Studio.

After having those pre-requisites:
1. Clone this repository
2. Copy `Assembly-CSharp.dll` and `UnityEngine.UI.dll` from your Doraemon Story of Seasons folder (path: `[SteamApps folder]/DORAEMON STORY OF SEASONS/DORaEMON STORY OF SEASONS_Data/Managed/`) into `libs` folder in this cloned repository.
3. Open `Mods.sln` and wait until it gets additional dependencies via NuGet
4. You are ready to click on "Build"

Similar steps may work for VSCode, but I am not sure how to download NuGet packages there.

## Issues
If you have found any issue, consider opening an Issue and stating as much details as possible.

## Contributting
See [Contributting](./CONTRIBUTTING.md).
