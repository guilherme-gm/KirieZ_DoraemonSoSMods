---
title: EventHelper
---

!!! warning
	This is an internal API not ready for full use yet.
	Although it works and is used internally, it is too incomplete for a safe public use.

`EventHelper` is the main Event Module entry point.

This static class allows modders to interact with game's Events.

## Methods

### `RegisterEvent(EventConfig config)`
Registers a new event defined by [EventConfig](./EventConfig.md) into the game.

After the event is registered, the internal handling will be performed by kzModUtils,
and consuming mods should use the passed `EventConfig` instance for any
future reference, since the `EventConfig` will work as the communication
between kzModUtils/game and the mod that registered this event.


## Read more
- [Doraemon SoS Reference: Events](../../DoraemonSoS-Reference/Events.md)
- [Guide: Creating Events](../kzModUtils-Guide/Creating-Events.md)
- [Reference: EventConfig](./EventConfig.md)
