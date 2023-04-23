---
title: EventConfig
---

`EventConfig` is the base class to register a new [Event](../../DoraemonSoS-Reference/Events.md) into the game logic.

An `EventConfig` includes all the static definition for registering the event and works as the "communication" object
for kzModUtils to do its magic when injecting custom events into the main game and giving back information to
the consuming mod.

!!! warning
	Currently kzModUtils only supports "flag" events, since cutscenes are handled by a special script language/VM.


## Properties

### `EventModId` - String
An kzModUtils identifier for this event. kzModUtils uses this identifier
to map it to an actual free id in the game save.
(See [Dynamic IDs](../kzModUtils-Guide/Dynamic-IDs.md))

This is a required property, and it is **recommended** that it contains a prefix
refering to your mod to avoid conflicts in kzModUtils.

**Example:** `myMod.item1Bought`

### `Title` - String or null
Readable title for this event.

### `EventId` (**read-only**) - int
The actual Event ID per game logic. This value is assigned automatically
based on the active game save, **don't** store it elsewhere, since the value may
change between game saves.

See [Dynamic IDs](../kzModUtils-Guide/Dynamic-IDs.md) for more info about this.

### `TitleId` (**read-only**) - int
The ID for Title in game's text database. This value is assigned automatically
based on game load logic.

See [Dynamic IDs](../kzModUtils-Guide/Dynamic-IDs.md) for more info about this.


## Constructor

### `EventConfig(string id)`
Creates a new `EventConfig` setting its `EventModId`.


## Usage notes
This class is usually combined with `EventHelper` to register new events into
the game system.


## Read more
- [Doraemon SoS Reference: Events](../../DoraemonSoS-Reference/Events.md)
- [Guide: Creating Events](../kzModUtils-Guide/Creating-Events.md)
- [Reference: EventHelper](./EventHelper.md)
