using BepInEx;
using HarmonyLib;
using kzModUtils;
using kzModUtils.Events;
using kzModUtils.Resource;
using kzModUtils.UI;

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
			bool isDayChange = false;
			if (e.Time.Day != LastLoadedDay)
			{
				TodaysEvent = MasterManager.Instance.CarnivalMaster.GetCarnivalData(e.Time.Season, e.Time.Day);
				LastLoadedDay = e.Time.Day;
				isDayChange = true;
			}

			if (TodaysEvent == null)
				return;

			if (TodaysEvent.Type == Define.Carnival.TypeEnum.None)
				return;

			if (TodaysEvent.StartTime == null)
			{
				if (isDayChange)
					UIUtils.EventLog.AddLogRequest($"{ResourceUtils.GetText(TodaysEvent.NameId)} started.", -1);

				return;
			}

			if (TodaysEvent.StartTime.Hour != e.Time.Hour || TodaysEvent.StartTime.Minute != e.Time.Minute)
				return;

			UIUtils.EventLog.AddLogRequest($"{ResourceUtils.GetText(TodaysEvent.NameId)} started.", -1);
		}
	}
}
