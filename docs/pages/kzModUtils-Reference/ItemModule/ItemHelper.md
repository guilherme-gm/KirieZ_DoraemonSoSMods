---
title: ItemHelper
---

`ItemHelper` is the main Item Module entry point.

This singleton class allows modders to interact with game's Items.


## Methods

### `RegisterItem(CustomItemConfig itemConfig, OnItemRegistered callback)`
Registers a new item defined by [CustomItemConfig](./CustomItemConfig.md) into the game.

Once registered, `callback` is called with the created `CustomItem`, which
extends original `ItemMasterModel` class.

After the item is registered, the internal handling will be performed by kzModUtils,
and consuming mods should use the passed `CustomItemConfig` and `CustomItem` instances
for any future reference.


## Read more
- [Doraemon SoS Reference: Items](../../DoraemonSoS-Reference/Items.md)
- [Guide: Creating Items](../../kzModUtils-Guide/Customizing/Creating-Items.md)
- [Reference: CustomItemConfig](./CustomItemConfig.md)
