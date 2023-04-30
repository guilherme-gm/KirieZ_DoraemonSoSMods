---
title: ResourceUtils
---

`ResourceUtils` is the main Resource Module entry point.

This static class allows modders to interact with game's resource files and content.

## Methods

### `string GetText(int textId)`
Retrieves text with id `textId` from `TextMaster`.


### `void GetCharacterSprite(Define.Character.Id character, Action<Sprite> callback)`

Fetches the sprite for `character` and calls `callback` with it.

!!! note
	This does not support custom character IDs

**Example:**

```C#
ResourceUtils.GetCharacterSprite(Define.Character.Id.KorobokkurRed, delegate (Sprite sprite) {
	title.SetSprite(sprite);
});
```


### `string GetAssetBundlePath(ResourceType resourceType, string resourceName, string extension = "jp")`
Returns the path for an asset bundle. This is just a helping method to join parts of strings and pre-defined paths.

`ResourceType` may be:
- `ResourceType.Data`: Path for `[data folder]/AssetBundle/[Platform name]`
- `ResourceType.StreamingAsset`: Path for `[streaming assets folder]/AssetBundle/[Platform name]`
- `ResourceType.Plugin`: Path for BepInEx "plugins" folder


**Example:**

```C#
var UIElementsAsset = AssetBundle.LoadFromFile(
	ResourceUtils.GetAssetBundlePath(ResourceType.Plugin, "kzModUtils/UIElements", "")
);
```

### `SpriteConfig RegisterSprite(Sprite sprite)`
Registers a new sprite into the game sprite library and returns a `SpriteConfig` to refer to it.

See [Dynamic IDs](../../kzModUtils-Guide/Basics/Dynamic-IDs.md) for information about Config classes.

