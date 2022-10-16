using BepInEx;

namespace kzModUtils
{
	[BepInPlugin("io.github.guilherme-gm.DoraemonSoSMods.kzModUtils", "kz Mod Utils", PluginInfo.PLUGIN_VERSION)]
	public class Plugin : BaseUnityPlugin
	{	
		private void Awake()
		{
			// Plugin startup logic
			Logger.LogInfo($"Plugin {PluginInfo.PLUGIN_GUID} is loaded!");

			TimeModule.Initialize();
			UIModule.Initialize();
		}

		private void Destroy()
		{
			TimeModule.Teardown();
			UIModule.Teardown();
		}
	}
}
