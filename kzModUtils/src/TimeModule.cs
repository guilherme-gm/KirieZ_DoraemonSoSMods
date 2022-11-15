using HarmonyLib;
using kzModUtils.Events;
using System;

namespace kzModUtils
{
	public class TimeModule: IModule
	{
		/**
		 * Called at every time change.
		 */
		public static event EventHandler<TimeChangeEventArgs> OnTimeChange;

		private static TimeModule mInstance;

		internal static TimeModule Instance
		{
			get {
				if (mInstance == null)
					mInstance = new TimeModule();

				return mInstance;
			}
			private set {}
		}

		public void Initialize()
		{
			Harmony.CreateAndPatchAll(typeof(TimeModule));
		}

		public void Teardown()
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
