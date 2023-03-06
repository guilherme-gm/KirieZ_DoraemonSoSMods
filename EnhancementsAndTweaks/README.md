# Enhancements and Tweaks
A plugin/mod containing several additions and tweaks ("mods") to the game. It aims at improving the game experience (per mod-author own perspective).

All of its changes are configurable, so you may enable only the ones you want and even enabling them may allow you to further
customize it to your desires.

**Table of Contents:**
- [Installation](#installation)
- [List of mods](#list-of-mods)
	- [Adjust Korobokkur Friendship](#adjust-korobokkur-friendship)
		- [Reasoning](#reasoning)
		- [Configs](#configs)
	- [Adjust Tool Stamina](#adjust-tool-stamina)
		- [Reasoning](#reasoning-1)
	- [Alternative Furniture Rotation](#alternative-furniture-rotation)
	- [Confirm Beehive Removal](#confirm-beehive-removal)
	- [Event Alert](#event-alert)
	- [Map Shop Times](#map-shop-times)
	- [Quality Crop Effect](#quality-crop-effect)
	- [Show Can Make Recipes](#show-can-make-recipes)
	- [Show Item Price](#show-item-price)
	- [Sort Lists](#sort-lists)
		- [Configs](#configs-1)
	- [Stamina Bar](#stamina-bar)
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

---

## List of mods

### Adjust Korobokkur Friendship
Change how requesting help from the Korobokkur affects your friendship with them. By default:
- Friendship reduction goes from 120 points to 10 points (configurable)
- Once friendship is maxed out, it doesn't reduces anymore (configurable)

#### Reasoning
In the original game, requesting their help costs 120 friendship points, when you compare this to
the possibilities to improve friendship, you get the following:
- Talk: 25 points
- Liked gift: 25 points
- Loved gift: 60 points
- Cupid arrow: 100 points

Giving 1 loved gift + talking every day sums up to 85 points/day.
But asking for their help costs 120, and in the start, you only get 2 days of work.

Summarizing, this means that you have to give the best gift + talk to them every day just to be going up 50 points every 2 days.

And the only advantage you have is: You can get them working for you for more days in a row.

A cheaper alternative would be to leave them at the minimun friendship and just repeat asking their help
every 2 days -- but this is tedious!

Changing the cost to 10 points you can still grow the friendship with them while asking for help, as long
as you keep talking to them. And once you reach the maximum, you don't have to worry about losing points.

#### Configs
| Config Name   | Description                                            | Accepted Values  | Default |
| ------------- | ------------------------------------------------------ | ---------------- | ------- |
| DecreaseOnMax | When at 10 hears, asking help decreases friendship?    | `true`/`false`   | `false` |
| AssistCost    | How much friendship is decreased when asking for help? | positive numbers | `10`  |

---

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

---

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

---

### Confirm Beehive Removal
<p align="center">
	<img src="../docs/modImages/ConfirmBeehiveRemoval.gif" width="600px"/>
</p>

Adds a dialog to confirm before you remove a placed beehive.

This prevents you from accidentally removing a beehive that has bees and end up losing Honey and Bees ( I did this several times :) )

---

### Event Alert
Displays an alert using the event log (the place where stamina status / item obtained / etc are displayed) when a festival is about to start.

---

### Map Shop Times
<p align="center">
	<img src="../docs/modImages/ShopTimesMod.gif" width="600px"/>
</p>

Adds a new button to the minimap window to toggle a small extra window that shows shops working hours.

Shops in Doraemon SoS opens in weird times and different days, which is annoying to remember and you
usually will go for an external guide.

This mod simply adds a small window so you can check it in game.

---

### Quality Crop Effect
<p align="center">
	<img src="../docs/modImages/QualityCropEffect.gif" width="600px"/>
</p>

Add some particle effects on crops that are in their maximum quality.

This helps you tell when you can stop fertilizing, as otherwise you would have to or:
1. Fertilize every day, always. This leads you to waste a lot of time/money in fertilization, specially once you reach 5 star seeds
2. Do lots of math to calculate how many fertilizers to apply and remember this until you
   harvest the crop. This is hard, the game doesn't provide info for you.

---

### Show Can Make Recipes
<p align="center">
	<img src="../docs/modImages/ShowCanMakeRecipes.gif" width="600px"/>
</p>

Adds a new button in the Kitchen's menu that allows you to toggle between viewing all
available recipes or only those you have every requirement (ingredients/tools).

This makes easier to cookie when you already know what you want and have ingredients for.

---

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

---

### Sort Lists
Sorts some in-game menu lists alphabetically instead of the "random" order they are originally in.
This make it easier to find what you are looking for.

Currently the following lists are supported (some are disabled by default, see Config on how to enable them):
- Kitchen's recipe list (default: sorted)
- Cafe's recipe shop (default: sorted)
- Cafe's produce shop (default: original / not sorted)
- Cafe's meals shop (default: original / not sorted)

If there is any other list that you feel the need to be sorted, let me know (or submit a PR!).

#### Configs
| Config Name         | Description                                 | Accepted Values | Default |
| ------------------- | ------------------------------------------- | --------------- | ------- |
| SortRecipeList      | Sort Kitchen's recipe list?                 | `true`/`false`  | `true`  |
| SortRecipeShopList  | Sort Cafet's Recipe Shop list?              | `true`/`false`  | `true`  |
| SortProduceShopList | Sort Cafet's Produce list (vegetables/etc)? | `true`/`false`  | `false` |
| SortMealsShopList   | Sort Cafet's Meals shop list (food shop)?   | `true`/`false`  | `false` |

---

### Stamina Bar
<p align="center">
	<img src="../docs/modImages/StaminaBarMod.gif" width="600px"/>
</p>

Displays a small window below game's clock with your current stamina.

This removes the need to pause/open the game menu to check.

---

## Configuration

> **Note:** Using [BepInEx Configuration Manager](https://github.com/BepInEx/BepInEx.ConfigurationManager) to enable/disable mods won't work until you restart the game.
> For other settings it should work fine.

> **Note:** Before editting the config files, close the game.

After installing this mod and starting your game once, a config file will be generated at
`[install dir]/BepInEx/config/io.github.guilherme-gm.DoraemonSoSMods.enhancementsAndTweaks.cfg`.

Open this file and change the settings as you wish. The first section, `[Mod Enable]` will
cover configs to enable each of the mods supported by `Enhancements And Tweaks`.

To disable one mod (for example, `Adjust Tool Stamina`), simply search for the `Enable[ModName] = true` line. For example:
```
## Enable Adjust Tool Stamina.
## This will make every tool in the game that consumes stamina (e.g. Watering Can, Axe, ...)
## always consume 1, regardless of how much they were "charged".
# Setting type: Boolean
# Default value: true
EnableAdjustToolStamina = true <----- Here
```

After finding it, simply change `true` to `false` and save. The mod will be disabled.

Every other setting in this file works in a similar way, find the config, change it to the desired value (check each mod section above to know what each config does).

---

## Building
You will need Visual Studio 2019 and .NET Framework 3.5 installed.

1. Clone this repository
2. Copy Doraemon's `Assembly-CSharp.dll` into `libs` folder
3. Open the `Mods.sln` solution
4. Build it

---

## Contributting
See [CONTRIBUTTING.md](../CONTRIBUTTING.md).
