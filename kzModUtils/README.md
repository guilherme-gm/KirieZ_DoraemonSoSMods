# kz Mod Utils

**Note:** This is not a mod that directly affects the game, but you may be asked to install it in order to us other mods.

Library with utilities to make mods. The aim of this lib is to provide other mods a direct and consistent interface to do their work, so they don't have to rely on the game internals in case it ever changes.

It may also help solve conflicts between mods, for example, by providing a single and incremental item ID system that mods may extend (not implemented right now).

## Installation
You must have BepInEx v5 and kz Mod Utils installed on your game.
- [Download BepInEx v5](https://github.com/BepInEx/BepInEx/releases)
	- [How to install BepInEx](https://docs.bepinex.dev/articles/user_guide/installation/index.html)

After having it installed, download the latest version of the kz Mod Utils from the [Releases](https://github.com/guilherme-gm/KirieZ_DoraemonSoSMods/releases?q=kz+Mod+Utils&expanded=true) page and add its dll to your BepInEx's plugins folder.


## Building
You will need Visual Studio 2019 and .NET Framework 3.5 installed.

1. Clone this repository
2. Copy Doraemon's `Assembly-CSharp.dll` and `UnityEngine.UI.dll` into `libs` folder
3. Open the `Mods.sln` solution
4. Build it


## Contributting
See [CONTRIBUTTING.md](../CONTRIBUTTING.md).
