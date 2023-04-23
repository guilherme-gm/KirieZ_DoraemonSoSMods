---
title: CustomItemConfig
---

`CustomItemConfig` is the base class to register a new [Item](../../DoraemonSoS-Reference/Items.md) into the game logic.

A `CustomItemConfig` includes all the static definition for registering the item,
and once registered within kzModUtils, allows kzModUtils to do the work of inserting
it in the game code and maintaining the references for different game saves and etc.

!!! warning
	Currently kzModUtils only supports basic items that stays in the inventory.


## Properties

### `ModItemID` - string
kzModUtils identifier for this item.
See [Dynamic IDs](../../kzModUtils-Guide/Basics/Dynamic-IDs.md).

### `Name` - string
Item display name.

### `Description` - string
Item description.

### `IsImportant` - bool
Is a Key Item? Key Items are special items that appears in the menu
and are not interactable.

### `Category` - Define.Item.CategoryEnum
Item category/kind. See the enum for the list of options

### `Sprite` - CustomSpriteConfig
Item inventory sprite

### `ResourceId` - int
Unknown

### `AtlasId` - int
!!! warning
	This property is deprecated and should NOT be used. Use `Sprite` instead.
	It is being kept for a few more versions to prevent breaking existing mods.

ID of the atlas which contains the item sprite

### `SpriteId` - int
!!! warning
	This property is deprecated and should NOT be used. Use `Sprite` instead.
	It is being kept for a few more versions to prevent breaking existing mods.

ID of the sprite of this item


## Constructor

### `CustomItemConfig()`
Creates a new `CustomItemConfig`.


## Usage notes
This class is usually combined with [ItemHelper](./ItemHelper.md)
to register new events into the game system.


## Read more
- [Doraemon SoS Reference: Items](../../DoraemonSoS-Reference/Items.md.md)
- [Guide: Creating Events](../../kzModUtils-Guide/Customizing/Creating-Items.md)

