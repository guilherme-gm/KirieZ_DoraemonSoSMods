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

		private static Dictionary<UIElementType, GameObject> UIPrefabs = new Dictionary<UIElementType, GameObject>();


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

		private static T CreateUIElement<T>(string method, UIElementType elementType, Vector3 position, GameObject parent = null) where T: UnityEngine.UI.Graphic
		{
			if (parent == null)
			{
				parent = CommonUICanvas;
			}

			if (parent == null)
			{
				Console.WriteLine($"{method}: Can't create element without parent before Common Canvas is created.");
				return default;
			}

			GameObject prefab;
			if (!UIPrefabs.TryGetValue(elementType, out prefab))
			{
				Console.WriteLine($"{method}: prefab not found.");
				return default;
			}

			var uiObject = GameObject.Instantiate(prefab, parent.transform);
			var uiElement = uiObject.GetComponent<T>();

			uiElement.rectTransform.anchoredPosition = position;

			return uiElement;
		}

		public static UnityEngine.UI.Text CreateText(Vector3 position, Vector2 size, string text = "", GameObject parent = null)
		{
			var textComponent = CreateUIElement<UnityEngine.UI.Text>("UIModule.CreateText", UIElementType.Text, position, parent);
			if (textComponent != null)
			{
				textComponent.rectTransform.sizeDelta = size;
				textComponent.text = text;
			}

			return textComponent;
		}

		public static UnityEngine.UI.Image CreateBackgroundImage(Vector3 position, Vector2 size, GameObject parent = null)
		{
			var bgImageComponent = CreateUIElement<UnityEngine.UI.Image>("UIModule.CreateRectangle", UIElementType.BackgroundImage, position, parent);
			if (bgImageComponent != null)
			{
				bgImageComponent.rectTransform.sizeDelta = size;
			}

			return bgImageComponent;
		}
	}
}
