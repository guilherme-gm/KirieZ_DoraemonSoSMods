---
title: Creating Events
---

!!! warning
	kzModUtils currently supports "buy once flag" events only.
	Advanced events that includes cutscenes are not supported yet due to missing
	script engine.

In order to create a custom event, we must first create a `EventConfig` instance,
this EventConfig will contain the definitions of our event so we can register it
in kzModUtils and get the benefits of the framework, and also retrieve info
handled by it, such as the [Dynamic ID](../Basics/Dynamic-IDs.md).

In this example, we will create a event that is triggered when the player buys
a specific item in a shop. This is the only kind of event registering possible
right now.

```C#
CustomItemConfig myItem = new CustomItemConfig() { /* ... */ };
ItemHelper.Instance.RegisterItem(myItem, (item) => { /* ... */ });

var myItemBoughtEvent = new EventConfig("myMod.book.bought") {
	RequiredItem = new IdHolder<CustomItemConfig>(myItem),
};
ShopHelper.RegisterBuyOnceEvent(myItemBoughtEvent);
```

In these lines, we first created a basic item, and them registered an event that:

- is uniquely identified by `myMod.book.bought`
- is triggered by `RequiredItem` condition, which would mean
    "player has acquired this item in their inventory"

!!! note
	Since we are using a custom item, we also have to use an `IdHolder` due to
	the Dynamic IDs system. An `IdHolder` is simply an abstraction to deal with
	the changing IDs between game saves.


After having our event config created, we can ask kzModUtils to actually register
this event so it may be triggered in game. This is accomplished by using:

```C#
ShopHelper.RegisterBuyOnceEvent(myItemBoughtEvent);
```
