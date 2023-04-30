---
title: Events in Game Logic
---

kzModUtils has ready to use events hooked to a few game internals that may be
useful for some mods.


## Time change
You can listen to the in-game time updates by adding a listener to
`TimeModule.OnTimeChange`. This event will be called at every in-game minute
change.

Example:

```C#
void AddListener() // Some random method in your plugin
{
	TimeModule.OnTimeChange += ModUtilities_OnTimeChange;
	return true;
}

private static void ModUtilities_OnTimeChange(object sender, TimeChangeEventArgs e)
{
	Console.Writeline($"It is now {e.Time.Hour}:{e.Time.Minute}");
}
```

This example you show in BepInEx console "It is now 10:01" (if it is 10:01 in-game),
and will write a new line once it reaches "10:02" and so on.


## UI Events
The game has 2 main user interfaces that we are constantly interacting with:
- the Title UI which shows up whenever we start the game or go back to the main menu
- the Game UI that is loaded once we start playing

When creating custom UI you usually need to rely on those events in order to
load and draw your content. kzModUtils offers 2 events for those cases:
`UIUtils.OnTitleUIReady` and `UIUtils.OnGameUIReady`


### Title UI Ready
`TitleUIReady` brings you an `TitleUIReadyEventArgs` object that contains the root
of the title UI (where you can attach new menus).

### Game UI Ready
`GameUIReady` brings you an `GameUIReadyEventArgs` object that contains the root
of the game UI canvas (where you can attach new menus)


Check the examples in "Creating UIs" section for more details on how to use those.
