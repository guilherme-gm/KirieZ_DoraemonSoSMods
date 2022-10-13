using HarmonyLib;
using kzModUtils.Events;
using System;

namespace kzModUtils
{
	public class TimeModule
	{
		/**
		 * Called at every time change.
		 */
		public static event EventHandler<TimeChangeEventArgs> OnTimeChange;

		public static void Initialize()
		{
			Harmony.CreateAndPatchAll(typeof(TimeModule));
		}

		public static void Teardown()
		{

		}

		[HarmonyPatch(typeof(FarmContentsController), "Start")]
		[HarmonyPostfix]
		static void TimeAdvanced(DailyScheduler ___mScheduler)
		{
			___mScheduler.ChangedTimeCallback += () =>
			{
				OnTimeChange?.Invoke(null, new TimeChangeEventArgs(SingletonMonoBehaviour<UserManager>.Instance.User.Time));
			};
		}
	}
}
