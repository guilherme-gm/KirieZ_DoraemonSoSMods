using BepInEx;
using kzModUtils.UI;

namespace kzModUtils
{
	[BepInPlugin("io.github.guilherme-gm.DoraemonSoSMods.kzModUtils", "kz Mod Utils", PluginInfo.PLUGIN_VERSION)]
	public class Plugin : BaseUnityPlugin
	{
		private IModule[] Modules = new IModule[]
		{
			TimeModule.Instance,
			UIModule.Instance,
		};

		private void Awake()
		{
			// Plugin startup logic
			Logger.LogInfo($"Plugin {PluginInfo.PLUGIN_GUID} is loaded!");

			foreach (var mod in Modules)
				mod.Initialize();
		}

		private void Destroy()
		{
			foreach (var mod in Modules)
				mod.Teardown();
		}
	}
}
