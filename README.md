# KirieZ_DoraemonSoSMods
Mods for Doraemon Story of Seasons originally made by KirieZ (@guilherme-gm)


## kz Mod Utils

Library with utilities to make mods. The aim of this lib is to provide other mods a direct and consistent interface to do their work, so they don't have to rely on the game internals in case it ever changes.

It also helps solve conflicts between mods, for example, by providing a single and incremental item ID system that mods may extend.

## List of mods

### Adjust Tool Stamina
Makes every tool that consumes stamina always use 1, whatever the charge level is.

This aims at providing a better progression around tools, as you usually get better tools but still consumes the same amount of stamina as if you were using level 1.

### Confirm Beehive Removal
Adds a dialog to confirm before you remove a placed beehive. This prevents you from accidentally removing a beehive that has bees and end up losing Honey and Bees.

![Confirm beehive removal](./docs/modImages/ConfirmBeehiveRemoval.gif)

### Dump Master
Utility to dump the binary resources as text. This is a prototype and quite unstable.

### Event Alert
Displays an alert using the event log (the place where stamina status / item obtained / etc are displayed) when a festival is about to start.

### Fishbook (WIP)
Adds a fishbook item to be bought. This item will track your progress as you catch fishes on the game and display info about each fishing spot when you get near them.

This mod is functional but still misses several visual improvements.

![Fishbook](./docs/modImages/Fishbook.gif)

### Map Shop Times
Adds a new button to the minimap window to toggle a small extra window that shows shops opening times so you don't have to check external guides.

![Map Shop times](./docs/modImages/ShopTimesMod.gif)

### Show Item Price
Adds a new line to item descriptions in inventory stating the value for which you would sell it. It takes into account the quality bonus.

![Show item price](./docs/modImages/ShowItemPrice.gif)

### Stamina Bar
Displays a little window with your current stamina, so you don't have to pause the game to check.

![Stamina Bar mod](./docs/modImages/StaminaBarMod.gif)

## Doramon Story of Seasons mod template settings
- TFM: net35
- Unity version: 2017.4.10


## Issues
If you have found any issue, consider opening an Issue and stating as much details as possible.

## Contributting
See [Contributting](./CONTRIBUTTING.md).
