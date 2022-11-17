using BepInEx;
using HarmonyLib;
using kzModUtils.UI;

namespace kzModUtils
{
	[BepInPlugin("io.github.guilherme-gm.DoraemonSoSMods.kzModUtils", "kz Mod Utils", PluginInfo.PLUGIN_VERSION)]
	public class Plugin : BaseUnityPlugin
	{
		private static Plugin Instance;

		private IModule[] Modules = new IModule[]
		{
			TimeModule.Instance,
			UIModule.Instance,
			TextData.TextDataModule.Instance,
			ItemData.ItemModule.Instance,
			EventData.EventDataModule.Instance,
		};

		private ICollectionModule[] CollectionModules = new ICollectionModule[]
		{
			TextData.TextDataModule.Instance,
			ItemData.ItemModule.Instance, // Requires text
			EventData.EventDataModule.Instance, // Requires text
		};

		private void Awake()
		{
			Plugin.Instance = this;

			// Plugin startup logic
			Logger.LogInfo($"Plugin {PluginInfo.PLUGIN_GUID} is loaded!");

			Harmony.CreateAndPatchAll(typeof(Plugin));

			foreach (var mod in Modules)
				mod.Initialize();
		}

		private void Destroy()
		{
			foreach (var mod in Modules)
				mod.Teardown();
		}

		[HarmonyPatch(typeof(MasterManager), "SetupMasters")]
		[HarmonyPostfix]
		private static void OnMasterCollectionSetup()
		{
			foreach (var mod in Instance.CollectionModules)
				mod.Setup();
		}
	}
}
