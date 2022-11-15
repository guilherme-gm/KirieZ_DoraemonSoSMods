using HarmonyLib;
using kzModUtils.UI.Events;
namespace kzModUtils.UI
{
	/**
	 * Module to take care of UI-related tasks.
	 * This class is not meant to be accessed by lib consumers, it works in doing the background work.
	 */
	internal class UIModule: IModule
	{
		private static UIModule mInstance;

		internal static UIModule Instance
		{
			get {
				if (mInstance == null)
					mInstance = new UIModule();

				return mInstance;
			}
			private set {}
		}

		public void Initialize()
		{
			Harmony.CreateAndPatchAll(typeof(UIModule));
			UIAssets.Initialize();
		}

		public void Teardown()
		{
			UIAssets.Teardown();
		}

		/**
		 * FarmTopUIController.Initialize is called when the GameUI is created.
		 *
		 * Hooking it allows us to find important elements of the interface, such as the event log and the main canvas.
		 */
		[HarmonyPatch(typeof(FarmTopUIController), "Initialize")]
		[HarmonyPostfix]
		internal static void FarmTopUIController_Initialize(
		   FarmTopUIController __instance,
		   UIArgument arg,
		   EventLogUIPartController ___mEventLog
		)
		{
			if (__instance.transform.parent != null)
			{
				UIUtils.CommonUICanvas = __instance.transform.parent.gameObject;
			}

			UIUtils.EventLog = ___mEventLog;
			UIUtils.FireGameUIReady(new GameUIReadyEventArgs(UIUtils.CommonUICanvas, __instance));
		}
	}
}
