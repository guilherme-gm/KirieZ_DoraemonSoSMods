using BepInEx;
using HarmonyLib;
using kzModUtils;
using kzModUtils.Events;

namespace EventAlert
{
	[BepInPlugin("io.github.guilherme-gm.DoraemonSoSMods.eventAlert", "Event Alert", PluginInfo.PLUGIN_VERSION)]
	[BepInDependency("io.github.guilherme-gm.DoraemonSoSMods.kzModUtils")]
	public class Plugin : BaseUnityPlugin
	{
		private static int LastLoadedDay = 0;

		private static CarnivalMasterModel TodaysEvent = null;

		private void Awake()
		{
			// Plugin startup logic
			Logger.LogInfo($"Plugin {PluginInfo.PLUGIN_GUID} is loaded!");

			Harmony.CreateAndPatchAll(typeof(Plugin));

			TimeModule.OnTimeChange += ModUtilities_OnTimeChange;
		}

		private static void ModUtilities_OnTimeChange(object sender, TimeChangeEventArgs e)
		{
			if (e.Time.Day != LastLoadedDay)
			{
				TodaysEvent = MasterManager.Instance.CarnivalMaster.GetCarnivalData(e.Time.Season, e.Time.Day);
				LastLoadedDay = e.Time.Day;
			}

			if (TodaysEvent == null)
				return;

			if (TodaysEvent.StartTime.Hour != e.Time.Hour || TodaysEvent.StartTime.Minute != e.Time.Minute)
				return;

			UIModule.EventLog.AddLogRequest($"{ResourceModule.GetText(TodaysEvent.NameId)} started.", -1);
		}
	}
}
