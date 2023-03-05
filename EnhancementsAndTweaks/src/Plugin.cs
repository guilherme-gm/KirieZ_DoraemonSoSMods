using HarmonyLib;
using BepInEx;

namespace EnhancementsAndTweaks
{
	[BepInPlugin("io.github.guilherme-gm.DoraemonSoSMods.enhancementsAndTweaks", "Enhancements and Tweaks", PluginInfo.PLUGIN_VERSION)]
	public class Plugin : BaseUnityPlugin
	{
		private void Awake()
		{
			Logger.LogInfo($"Plugin {PluginInfo.PLUGIN_GUID} is loaded!");
		}
	}
}
