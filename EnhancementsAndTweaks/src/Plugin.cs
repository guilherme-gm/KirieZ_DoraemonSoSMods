using BepInEx;
using EnhancementsAndTweaks.Mods;
using HarmonyLib;
using System;
using System.Text;
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
			new AdjustKorobokkurFriendshipMod(),
			new AdjustToolStaminaMod(),
			new AlternativeFurnitureRotationMod(),
			new ConfirmBeehiveRemovalMod(),
			new EventAlertMod(),
			new MapShopTimesMod(),
			new ShowCanMakeRecipesMod(),
			new ShowItemPriceMod(),
			new SortListsMod(),
			new StaminaBarMod(),
		};

		private AssetBundle Assets;

		private string ClearModName(string name)
		{
			// from https://stackoverflow.com/a/1120277
			StringBuilder sb = new StringBuilder();

			foreach (char c in name) {
				if ((c >= '0' && c <= '9') || (c >= 'A' && c <= 'Z') || (c >= 'a' && c <= 'z') || c == '.' || c == '_')
					sb.Append(c);
			}

			return sb.ToString();
		}

		private void TryEnableMod(IMod mod)
		{
			try {
				bool enabled = Config.Bind(
					"Mod Enable",
					$"Enable{this.ClearModName(mod.GetName())}", true,
					$"Enable {mod.GetName()}.\n{mod.GetDescription()}"
				).Value;

				if (enabled) {
					if (!mod.PreInstall(Config, Assets))
						enabled = false;
				}

				if (enabled)
					Harmony.CreateAndPatchAll(mod.GetType(), mod.GetName());

				Logger.LogInfo(
					$"Mod \"{mod.GetName()}\" is {(enabled ? "ENABLED" : "DISABLED")}"
				);
			} catch (Exception error) {
				Logger.LogError($"Failed to initialize mod \"{mod.GetName()}\". Error:");
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
