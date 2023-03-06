using BepInEx;
using EnhancementsAndTweaks.Mods;
using HarmonyLib;
using System;

namespace EnhancementsAndTweaks
{
	[BepInPlugin("io.github.guilherme-gm.DoraemonSoSMods.enhancementsAndTweaks", "Enhancements and Tweaks", PluginInfo.PLUGIN_VERSION)]
	public class Plugin : BaseUnityPlugin
	{
		private static readonly IMod[] ModList = new IMod[]{
			new AdjustToolStaminaMod(),
		};

		private void TryEnableMod(IMod mod)
		{
			try {
				bool enabled = Config.Bind(
					mod.GetName(),
					"Enabled", true,
					$"Enable {mod.GetName()}.\n{mod.GetDescription()}"
				).Value;

				if (enabled) {
					if (!mod.PreInstall(Config))
						enabled = false;
				}

				if (enabled)
					Harmony.CreateAndPatchAll(typeof(AdjustToolStaminaMod), mod.GetName());

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

			foreach (var mod in ModList) {
				this.TryEnableMod(mod);
			}
		}
	}
}
