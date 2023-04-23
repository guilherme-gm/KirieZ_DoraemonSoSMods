---
title: Shops
---

Modifying game [shops](../../DoraemonSoS-Reference/Shops.md) is something a bit messy
because they have different structures for each kind of shop combined with deep logic
to process purchases.

[ShopHelper](../../kzModUtils-Reference/ShopModule/ShopHelper.md) tries to alleviate
those differences by providing a single structure to deal with different shops.

!!! warning
	Not every shop is currently supported. New shops should be added as needed

Registering an item to be sold requires you to create a `ShopItemConfig` to define
every aspect of a purchaseable item and then pass it to `ShopHelper.RegisterItemToSell`
so it gets included into the game. The following example illustrates that:

```C#
var myItemConfig = new CustomItemConfig() { /* ... */ };
// register myItemConfig into the game (See custom items for details)

// Creates a config specifying how this item will be sold
var shopItemConfig = new ShopItemConfig(new IdHolder<CustomItemConfig>(myItemConfig)) {
	Price = 1500,
	TargetShop = ShopId.VarietyShop,
};

// Sets this item in the shop
ShopHelper.RegisterItemToSell(shopItemConfig);
```

Since `ShopItemConfig` expects an `IdHolder`, we could also give it an item from the original game.
See [Dynamic IDs](../Basics/Dynamic-IDs.md) for more details on `IdHolder`.


## Making it a one-time purchase
Sometimes you may want to make an item that may only be bought once. This is used, for example,
to make key items like bag expansions available in the shop until you buy it.

To do that, we need to make an [Event](../../DoraemonSoS-Reference/Events.md) that works like
a flag. Once the item is bought, the event is completed, and we don't show the item anymore.

This is an example of how to create an event for a one-time purchase:

```C#
var myItemConfig = new CustomItemConfig() { /* ... */ };
// register myItemConfig into the game (See custom items for details)

// Creates a BuyOnce event that is triggered once the player obtains myItemConfig
var itemBoughtEvent = new EventConfig("myMod.item1.bought") {
	RequiredItem = new IdHolder<CustomItemConfig>(myItemConfig),
};
ShopHelper.RegisterBuyOnceEvent(itemBoughtEvent);

// Creates a config specifying how this item will be sold
var shopItemConfig = new ShopItemConfig(new IdHolder<CustomItemConfig>(myItemConfig)) {
	Price = 1500,
	SellOnceEvent = new IdHolder<EventConfig>(itemBoughtEvent), // Set the flag event!
	TargetShop = ShopId.VarietyShop,
};

// Sets this item in the shop
ShopHelper.RegisterItemToSell(shopItemConfig);
```

By adding `SellOnceEvent` property to `ShopItemConfig` we are telling kzModUtils that this
item should be sold only once, and this restriction is controlled by the event passed in
`SellOnceEvent`.

