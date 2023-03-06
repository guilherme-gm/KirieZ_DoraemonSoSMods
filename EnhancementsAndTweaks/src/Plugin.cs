using BepInEx;
using EnhancementsAndTweaks.Mods;
using HarmonyLib;
using System;
using UnityEngine;

namespace EnhancementsAndTweaks
{
	[BepInPlugin("io.github.guilherme-gm.DoraemonSoSMods.enhancementsAndTweaks", "Enhancements and Tweaks", PluginInfo.PLUGIN_VERSION)]
	[BepInDependency("io.github.guilherme-gm.DoraemonSoSMods.kzModUtils")]
	public class Plugin : BaseUnityPlugin
	{
		private static string AssetPath = (Application.platform == RuntimePlatform.OSXPlayer
			? $"{Application.dataPath}/../../BepInEx/plugins"
			: (
				Application.platform == RuntimePlatform.WindowsPlayer
					? $"{Application.dataPath}/../BepInEx/plugins"
					: Application.dataPath
			)
		);

		private static readonly IMod[] ModList = new IMod[]{
			new AdjustToolStaminaMod(),
			new AlternativeFurnitureRotationMod(),
			new ConfirmBeehiveRemovalMod(),
			new EventAlertMod(),
		};

		private AssetBundle Assets;

		private void TryEnableMod(IMod mod)
		{
			try {
				bool enabled = Config.Bind(
					mod.GetName(),
					"Enabled", true,
					$"Enable {mod.GetName()}.\n{mod.GetDescription()}"
				).Value;

				if (enabled) {
					if (!mod.PreInstall(Config, Assets))
						enabled = false;
				}

				if (enabled)
					Harmony.CreateAndPatchAll(mod.GetType(), mod.GetName());

				Logger.LogInfo(
					$"Mod \"{TtyUtils.BoldText(mod.GetName())}\" is {TtyUtils.BoldText((enabled ? "ENABLED" : "DISABLED"))}"
				);
			} catch (Exception error) {
				Logger.LogError($"Failed to initialize mod \"{TtyUtils.BoldText(mod.GetName())}\". Error:");
				Logger.LogError(error);
			}
		}

		private void Awake()
		{
			Logger.LogInfo($"Plugin {PluginInfo.PLUGIN_GUID} is loaded!");

			Assets = AssetBundle.LoadFromFile($"{AssetPath}/EnhancementsAndTweaks/enhancements_and_tweaks");

			foreach (var mod in ModList) {
				this.TryEnableMod(mod);
			}
		}
	}
}
