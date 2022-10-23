using HarmonyLib;
using kzModUtils.Events;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace kzModUtils
{
	/**
	 * Module to take care of UI-related tasks
	 */
	public class UIModule
	{
		private static AssetBundle Assets;

		/**
		 * Event triggered when the Game UI is ready to receive UI elements.
		 * 
		 * To be used by mods that want to extend the game's UI
		 */
		public static event EventHandler<GameUIReadyEventArgs> OnGameUIReady;

		/**
		 * Game's main UI Canvas.
		 * Usually you want to add your UI elements as a child of this canvas. Specially if it is an element that is permanently on screen.
		 */
		public static GameObject CommonUICanvas = null;

		/**
		 * Event Log controller.
		 * Event Log is responsible for displaying quick status updates in the bottom left part of the screen,
		 * such as item obtained or stamina info.
		 */
		public static EventLogUIPartController EventLog = null;

		internal static Dictionary<UIElementType, GameObject> UIPrefabs = new Dictionary<UIElementType, GameObject>();


		public static void Initialize()
		{
			Harmony.CreateAndPatchAll(typeof(UIModule));

			Assets = AssetBundle.LoadFromFile(Path.Combine(Application.dataPath, Path.Combine("AssetBundles", "kzModUtils_UIElements")));
			UIPrefabs.Add(UIElementType.BackgroundImage, Assets.LoadAsset<GameObject>("BackgroundImage"));
			UIPrefabs.Add(UIElementType.Text, Assets.LoadAsset<GameObject>("TextElement"));
		}

		public static void Teardown()
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
		static void FarmTopUIController_Initialize(
		   FarmTopUIController __instance,
		   UIArgument arg,
		   EventLogUIPartController ___mEventLog
		)
		{
			if (__instance.transform.parent != null)
			{
				CommonUICanvas = __instance.transform.parent.gameObject;
			}

			OnGameUIReady?.Invoke(null, new GameUIReadyEventArgs(CommonUICanvas, __instance));

			EventLog = ___mEventLog;
		}

		/**
		 * Closes any dialog that is currently open.
		 */
		public static void CloseDialog()
		{
			// SingletonMonoBehaviour<UIManager>.Instance.Pop(null); 
			SingletonMonoBehaviour<UIManager>.Instance.PopExceptForBottom(null);
		}
	}
}
