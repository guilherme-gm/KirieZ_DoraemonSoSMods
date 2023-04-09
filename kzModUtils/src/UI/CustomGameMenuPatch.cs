using System;
using System.Collections.Generic;
using HarmonyLib;
using UnityEngine;

namespace kzModUtils.UI
{
	internal class CustomGameMenuPatch
	{
		private static bool IsMenuInit = false;

		private static void GenerateNewMenuArray<T>(
			T[] originalArray,
			T[] newArray,
			Func<CustomGameMenuTabConfig, T> getMenuDataFunction
		) {
			int currentUpperRange = 0;
			int originalIdx = 0;
			int newIdx = 0;

			foreach (KeyValuePair<int, CustomGameMenuTabConfig> customMenu in UIUtils.CustomMenuTabs)
			{
				while (originalIdx < originalArray.Length && customMenu.Key >= currentUpperRange) {
					newArray[newIdx] = originalArray[originalIdx];
					newIdx++;
					originalIdx++;

					currentUpperRange += UIUtils.MenuRangePadding;
				}

				newArray[newIdx] = getMenuDataFunction(customMenu.Value);
				newIdx++;
			}

			Array.Copy(originalArray, originalIdx, newArray, newIdx, originalArray.Length - originalIdx);
		}

		[HarmonyPatch(typeof(Define.UI.Menu), "GetTabNames")]
		[HarmonyPostfix]
		private static void UI_Menu_GetTabNames(ref string[] __result)
		{
			if (UIUtils.CustomMenuTabs.Count == 0)
				return;

			var newResult = new string[__result.Length + UIUtils.CustomMenuTabs.Count];
			GenerateNewMenuArray<string>(__result, newResult, delegate (CustomGameMenuTabConfig config) {
				return config.Name;
			});

			__result = newResult;
		}

		[HarmonyPatch(typeof(Define.UI.Menu), "GetTabIconDatas")]
		[HarmonyPostfix]
		private static void UI_Menu_GetTabIconDatas(ref ScrollableTabsController.IconData[] __result)
		{
			if (UIUtils.CustomMenuTabs.Count == 0)
				return;

			var newResult = new ScrollableTabsController.IconData[__result.Length + UIUtils.CustomMenuTabs.Count];
			GenerateNewMenuArray<ScrollableTabsController.IconData>(__result, newResult, delegate (CustomGameMenuTabConfig config) {
				return new ScrollableTabsController.IconData(config.Sprite.AtlasId, config.Sprite.SpriteId);
			});

			__result = newResult;
		}

		[HarmonyPatch(typeof(MenuUIController), "Initialize")]
		[HarmonyPrefix]
		private static void MenuUI_PreInitialize(
			MenuUIController __instance,
			ref MenuContentUIPartController[] ___mMenuContents
		)
		{
			IsMenuInit = true;
		}

		[HarmonyPatch(typeof(MenuUIController), "Initialize")]
		[HarmonyPostfix]
		private static void MenuUI_PostInitialize(
			MenuUIController __instance,
			ref MenuContentUIPartController[] ___mMenuContents
		)
		{
			IsMenuInit = false;
		}

		[HarmonyPatch(typeof(MenuUIController), "GetNpcContentsList")]
		[HarmonyPrefix]
		private static void MenuUI_Initialize(
			MenuUIController __instance,
			ref MenuContentUIPartController[] ___mMenuContents
		)
		{
			if (!IsMenuInit || UIUtils.CustomMenuTabs.Count == 0)
				return;

			var contents = __instance.transform
				?.Find("UI_Menu")
				?.Find("Controller")
				?.Find("Body")
				?.Find("Contents");

			if (contents == null) {
				PluginLogger.LogError("Could not find menu contents.");
				return;
			}

			var newMenu = new MenuContentUIPartController[___mMenuContents.Length + UIUtils.CustomMenuTabs.Count];
			GenerateNewMenuArray<MenuContentUIPartController>(___mMenuContents, newMenu, delegate (CustomGameMenuTabConfig config) {
				var prefabController = config.MenuPrefabController;
				var menuObject = GameObject.Instantiate(prefabController.gameObject, contents);

				menuObject.SetActive(true);
				var controller = (MenuContentUIPartController) menuObject.GetComponent(prefabController.GetType());

				__instance.AttachUIParts(controller);
				if (controller != null)
					controller.Deactivate();

				return controller;
			});

			___mMenuContents = newMenu;
		}
	}
}
