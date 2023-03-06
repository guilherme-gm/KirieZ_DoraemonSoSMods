# Enhancements and Tweaks
A mod containing several additions and tweaks to the game. It aims at improving the game experience (per mod-author own perspective).

All of its changes are configurable, so you may enable only the ones you want and even enabling them may allow you to further
customize it to your desires.

**Table of Contents:**
- [Installation](#installation)
- [Change list](#change-list)
	- [Adjust Tool Stamina](#adjust-tool-stamina)
		- [Reasoning](#reasoning)
		- [Configs](#configs)
	- [Alternative Furniture Rotation](#alternative-furniture-rotation)
		- [Configs](#configs-1)
	- [Confirm Beehive Removal](#confirm-beehive-removal)
		- [Configs](#configs-2)
	- [Event Alert](#event-alert)
		- [Configs](#configs-3)
	- [Map Shop Times](#map-shop-times)
		- [Configs](#configs-4)
	- [Show Item Price](#show-item-price)
		- [Configs](#configs-5)
- [Configuration](#configuration)
- [Building](#building)
- [Contributting](#contributting)


## Installation
**Requirements:**
You must have BepInEx v5 and kz Mod Utils installed on your game.
- [Download BepInEx v5](https://github.com/BepInEx/BepInEx/releases)
	- [How to install BepInEx](https://docs.bepinex.dev/articles/user_guide/installation/index.html)
- [kz Mod Utils page](../kzModUtils/)

After having them installed:
1. download the latest version of the mod dll from the [Releases](https://github.com/guilherme-gm/KirieZ_DoraemonSoSMods/releases) page
2. Extract its content into your game's `BepInEx/plugins/` folder
3. Start the game
4. **(Optional)** If you don't want to use the default settings, close the game and see [Configuration](#configuration)


## Change list

### Adjust Tool Stamina
Makes every tool that consumes stamina always use 1, whatever the charge level is.

#### Reasoning
In the original game, improving the tool doesn't help much because although they cover a larger area,
the stamina consumption is the same as if you have used it several times. Thus, you just run out of stamina faster.

One could say this gives you more time for doing power naps, but in this case you just have to use technique endlessly
instead of enjoying the game. OR, if possible, you have to rush some gadget that does the work for you.

By making every charge level consume 1 stamina, you may aim at upgrading the tools you are using the most so
you spend less stamina doing your everyday stuff, having stamina for the rest of the day to do more.

Note that this change made Silver upgrade a bit weird, because it just charges faster. I did not touch it,
and just consider it as a "I can finish my work faster" upgrade.

#### Configs
| Config Name | Description        | Accepted Values | Default |
| ----------- | ------------------ | --------------- | ------- |
| Enabled     | Enable/Disable mod | `true`/`false`  | `true`  |


### Alternative Furniture Rotation
<p align="center">
	<img src="../docs/modImages/AlternativeFurnitureRotation.gif" width="600px"/>
</p>

Changes how furniture placement works to provide a different rotation system.
1. It doesn't force the rotation to the character direction;
2. The objects have a different pivot now;
3. Adds a "Rotate" button so you can try different positioning from the same spot;
4. Adds a directional arrow to help determining direction of objects where direct matters (e.g.: Bed).

I won't call it as "better" because it is also not that fluid, but it is an alternative way that gives a bit more control / less side effects.

**Note:** This mod will probably conflict with other mods touch furniture placement, because it replaces the main rotation code.

#### Configs
| Config Name | Description        | Accepted Values | Default |
| ----------- | ------------------ | --------------- | ------- |
| Enabled     | Enable/Disable mod | `true`/`false`  | `true`  |


### Confirm Beehive Removal
<p align="center">
	<img src="../docs/modImages/ConfirmBeehiveRemoval.gif" width="600px"/>
</p>

Adds a dialog to confirm before you remove a placed beehive.

This prevents you from accidentally removing a beehive that has bees and end up losing Honey and Bees ( I did this several times :) )

#### Configs
| Config Name | Description        | Accepted Values | Default |
| ----------- | ------------------ | --------------- | ------- |
| Enabled     | Enable/Disable mod | `true`/`false`  | `true`  |


### Event Alert
Displays an alert using the event log (the place where stamina status / item obtained / etc are displayed) when a festival is about to start.

#### Configs
| Config Name | Description        | Accepted Values | Default |
| ----------- | ------------------ | --------------- | ------- |
| Enabled     | Enable/Disable mod | `true`/`false`  | `true`  |


### Map Shop Times
<p align="center">
	<img src="../docs/modImages/MapShopTimes.gif" width="600px"/>
</p>

Adds a new button to the minimap window to toggle a small extra window that shows shops working hours.

Shops in Doraemon SoS opens in weird times and different days, which is annoying to remember and you
usually will go for an external guide.

This mod simply adds a small window so you can check it in game.

#### Configs
| Config Name | Description        | Accepted Values | Default |
| ----------- | ------------------ | --------------- | ------- |
| Enabled     | Enable/Disable mod | `true`/`false`  | `true`  |


### Show Item Price
<p align="center">
	<img src="../docs/modImages/ShowItemPrice.gif" width="600px"/>
</p>

Adds a new box beside item description in inventory and storages UIs.
This box presents information about the price of the item when sold.

It includes the base price (0.5 star) and the current price, helping you keep track of how much you are
earning for those items/deciding what to sell.

Although the game provides a UI to keep track of gainings/losses, it is really hard to track individual
prices, specially when quality stars are added in the sum.

#### Configs
| Config Name | Description        | Accepted Values | Default |
| ----------- | ------------------ | --------------- | ------- |
| Enabled     | Enable/Disable mod | `true`/`false`  | `true`  |


## Configuration
TODO:

## Building
You will need Visual Studio 2019 and .NET Framework 3.5 installed.

1. Clone this repository
2. Copy Doraemon's `Assembly-CSharp.dll` into `libs` folder
3. Open the `Mods.sln` solution
4. Build it


## Contributting
See [CONTRIBUTTING.md](../CONTRIBUTTING.md).
