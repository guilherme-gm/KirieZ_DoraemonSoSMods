---
title: Getting started (index)
weight: 3
---

kzModUtils is sort-of a framework for modding Doraemon Story of Seasons.
It is a mod that provides modders with several utility classes to interact with the game data
without requiring them to hook the original game code.

This helps reducing the chances of conflicts with other mods that does the same, and also
make the overall process of modding easier as you can use a documented API instead of
having to reverse engineer the original game and find points to hook at.

This area of the documentation will guide you through several aspects of kzModUtils with a few
short tutorials using features from the framework. For a detailed presentation of the classes,
parameters and methods, please view [kzModUtils Reference](../kzModUtils-Reference/index.md) section.


## Setting it up
To make use of kzModUtils in your mod project:

1. download the latest dll from [GitHub](https://github.com/guilherme-gm/KirieZ_DoraemonSoSMods/releases)
2. copy the dll to your mod project
3. add it as a reference to your project, similarly to how you would add the game's dll. For example, if you are using a Visual Studio project, you could add to your vsproj:
```XML
	<ItemGroup>
		<Reference Include="kzModUtils">
			<HintPath>.\kzModUtils.dll</HintPath>
		</Reference>
	</ItemGroup>
```

!!! note
	You should **NOT** ship kzModUtils dll when releasing your mod!! Ask users to install it separately.


You are ready to use it in your mod!

### BepInEx dependency
If your mod relies on BepInEx, make sure to declare a hard dependency on kzModUtils by adding the following [Attribute](https://learn.microsoft.com/en-us/dotnet/csharp/advanced-topics/reflection-and-attributes/):

```c#
// (1)
[BepInDependency("io.github.guilherme-gm.DoraemonSoSMods.kzModUtils", "0.6.0")]
public class Plugin : BaseUnityPlugin {}
```

1. Replace `0.6.0` with the minimal version of kzModUtils that you need (you may also simply set to the current version).
   This helps BepInEx alert in case the user is using an incompatible version.


With that, you are ready to start modding!
