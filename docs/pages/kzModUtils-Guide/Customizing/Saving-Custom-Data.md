---
title: Saving Custom Data
---

!!! warning "Steam cloud"
	Steam Cloud storage for Doraemon SoS is quite limited and sometimes
	custom save states are not saved.


Sometimes just saving things the game already have is not enough.

Let's say you have some custom configs, or an entirely new functionality
that keeps several new states that are not part of the game already.

At those times, you need a way to hook into the game saving and loading process
to do your stuff too. For those times, kzModUtils offers the `GameSaveHelper`.

`GameSaveHelper.RegisterSaveHandler(ISaveDataHandler handler)`  allows you to
include your own game saving process into the game logic by providing a class
that implements `ISaveDataHandler`.

Usually, you will want your handler to communicate to some other part of your mod
in order to set the saved/loaded state. In the following example, we do that by
calling `MyMod.Instance.Initialize()` :

```C#
[Serializable]
internal class MyModSaveState
{
	bool DoSomething;

	int DoSomethingElse;
}

internal class MySaveDataHandler : ISaveDataHandler
{
	public string GetSuffix()
	{
		return "myMod";
	}

	public void LoadGameData(byte[]? buffer)
	{
		// No saved data for this handler
		if (buffer == null) {
			MyMod.Instance.Initialize();
			return;
		}

		MyModSaveState saveState = new MyModSaveState();

		using (var stream = new MemoryStream(buffer)) {
			using (BinaryReader reader = new BinaryReader(stream)) {
				saveState.DoSomething = reader.ReadBoolean();
				saveState.DoSomethingElse = reader.ReadInt32();
			}
		}

		// Sets the mod with this game save loaded state
		MyMod.Instance.Initialize(saveState);
	}

	public byte[] SaveGameData()
	{
		// Gets the mod current state
		var state = MyMod.Instance.GetSaveState();

		using (var stream = new MemoryStream())
		{
			using (var writer = new BinaryWriter(stream))
			{
				writer.Write(state.DoSomething);
				writer.Write(state.DoSomethingElse);
			}

			// Retruns the written state
			return stream.ToArray();
		}
	}
}
```

Note that this is an implementation using `MemoryStream`, `BinaryReader`
and `BinaryWriter`. You are free to implement your handling as you wish,
as long as you are implementing the interface which works with `byte[]`.


## How it works
Under the hood, kzModUtils has hooked the loading and saving process from
the game and when those operations happens, it creates (or reads) files
in the game save folder named in the following pattern:

```
{GameSaveName}_{Suffix}.dat
```

In other words, if you are currently playing in `GameSave1` with a mod
called whose suffix is `MyMod`, the game folder will contain a new file
called `GameSave1_MyMod.dat` with the content you serialized via `SaveGameData`.

