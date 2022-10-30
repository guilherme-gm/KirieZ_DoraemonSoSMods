using HarmonyLib;
using kzModUtils.Resource;
using kzModUtils.UI.Events;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace kzModUtils.UI
{
	/**
	 * Module to take care of UI-related tasks.
	 * This class is not meant to be accessed by lib consumers, it works in doing the background work.
	 */
	internal static class UIModule
	{
		private static AssetBundle Assets;

		internal static Dictionary<UIElementType, GameObject> UIPrefabs = new Dictionary<UIElementType, GameObject>();

		internal static void Initialize()
		{
			Harmony.CreateAndPatchAll(typeof(UIModule));

			Assets = AssetBundle.LoadFromFile(ResourceUtils.GetAssetBundlePath(ResourceType.Plugin, "kzModUtils/UIElements", ""));
			UIPrefabs.Add(UIElementType.BackgroundImage, Assets.LoadAsset<GameObject>("BackgroundImage"));
			UIPrefabs.Add(UIElementType.Text, Assets.LoadAsset<GameObject>("TextElement"));
		}

		internal static void Teardown()
		{
			foreach (var prefab in UIPrefabs.Values)
			{
				GameObject.Destroy(prefab);
			}
			UIPrefabs = new Dictionary<UIElementType, GameObject>();

			Assets.Unload(true);
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
