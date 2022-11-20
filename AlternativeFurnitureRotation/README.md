# Alternative Furniture Rotation

Changes how furniture is rotated during placement. This does 2 changes:
1. It no longer follows player direction by default
2. A new "Rotate" key was added which rotates the object 90 degrees each time it is pressed. This rotation is kept until the key is pressed again.

In my opinion, this gives you more freedom to place objects wherever you want. But I won't call it a "better" mode, as it still not so natural to use.

This was tested with basic furnitures: Initial bed, chair and storage box.

## Installation
You must have BepInEx v5 installed on your game.
- [Download BepInEx v5](https://github.com/BepInEx/BepInEx/releases)
- [How to install BepInEx](https://docs.bepinex.dev/articles/user_guide/installation/index.html)

After having it ready, download the latest version of the mod dll from the [Releases](https://github.com/guilherme-gm/KirieZ_DoraemonSoSMods/releases) page and add extract it to your BepInEx's plugins folder.
**Important:** Both files must be kept inside the "AlternativeFurnitureRotation" folder.

## Building
You will need Visual Studio 2019 and .NET Framework 3.5 installed.

1. Clone this repository
2. Copy Doraemon's `Assembly-CSharp.dll` into `libs` folder
3. Open the `Mods.sln` solution
4. Build it


## Contributting
See [CONTRIBUTTING.md](../CONTRIBUTTING.md).
