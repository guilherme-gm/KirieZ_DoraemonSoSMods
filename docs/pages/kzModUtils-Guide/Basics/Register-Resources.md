---
title: Register Resources
---

Some times you need to add some basic resources into the game,
like text and sprites for some element, like an item.
For those cases, we have to register the data into the game library.


## Registering Sprites
To include a sprite into the game library, we have to use [ResourceUtils.RegisterSprite](../../kzModUtils-Reference/Resource-and-Text-Modules/ResourceUtils.md#customspriteconfig-registerspritesprite-sprite).
This utility method will take care of including your sprite in a special Atlas
and make sure this Atlas is available for the game to load.

`RegisterSprite` will give you a `CustomSpriteConfig`, which allows you to
get the current Atlas ID and Sprite ID assigned to your sprite.

**Example:**

```C#
var bookSprite = Assets.LoadAsset<Sprite>("Fishbook_Icon");
var bookSpriteConfig = ResourceUtils.RegisterSprite(bookSprite)
var fishbook = new CustomItemConfig() {
	ModItemID = "fishbook.book",
	Name = "Fish book",
	Description = "A book containing details about fishes you have catched.",
	IsImportant = true,
	Category = Define.Item.CategoryEnum.ConsumptionTool,
	Sprite = bookSpriteConfig,
	ResourceId = 0,
};
```

In this example, a new sprite is loaded and registered, afterwards, it is given
to a new custom item.


## Registering Text
Including new text into the game for items/etc requires you to register it
in `TextMaster`. kzModUtils helps you there by providing [TextHelper](../../kzModUtils-Reference/Resource-and-Text-Modules/TextHelper.md).

