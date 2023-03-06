using BepInEx.Configuration;
using kzModUtils;
using kzModUtils.Events;
using kzModUtils.Resource;
using kzModUtils.UI;
using UnityEngine;

namespace EnhancementsAndTweaks.Mods
{
	public class EventAlertMod : IMod
	{
		internal static readonly string TweakName = "Event Alert";

		private static int LastLoadedDay = 0;

		private static CarnivalMasterModel TodaysEvent = null;

		string IMod.GetName()
		{
			return TweakName;
		}

		string IMod.GetDescription()
		{
			return
				"Displays an alert using the event log when a festival is about to start."
				;
		}

		bool IMod.PreInstall(ConfigFile config, AssetBundle assets)
		{
			TimeModule.OnTimeChange += ModUtilities_OnTimeChange;
			return true;
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
