using System;
using HarmonyLib;
using UnityEngine;

namespace kzModUtils.UI
{
	internal class CustomGameMenuPatch
	{
		private static bool IsMenuInit = false;

		[HarmonyPatch(typeof(Define.UI.Menu), "GetTabNames")]
		[HarmonyPostfix]
		private static void UI_Menu_GetTabNames(ref string[] __result)
		{
			if (UIUtils.CustomMenuTabs.Count == 0)
				return;

			var newResult = new string[__result.Length + UIUtils.CustomMenuTabs.Count];
			Array.Copy(__result, newResult, __result.Length);

			for (int i = __result.Length, j = 0; i < newResult.Length; i++, j++)
				newResult[i] = UIUtils.CustomMenuTabs[j].Name;

			__result = newResult;
		}

		[HarmonyPatch(typeof(Define.UI.Menu), "GetTabIconDatas")]
		[HarmonyPostfix]
		private static void UI_Menu_GetTabIconDatas(ref ScrollableTabsController.IconData[] __result)
		{
			if (UIUtils.CustomMenuTabs.Count == 0)
				return;

			var newResult = new ScrollableTabsController.IconData[__result.Length + UIUtils.CustomMenuTabs.Count];
			Array.Copy(__result, newResult, __result.Length);

			for (int i = __result.Length, j = 0; i < newResult.Length; i++, j++) {
				var spriteConfig = UIUtils.CustomMenuTabs[j].Sprite;
				newResult[i] = new ScrollableTabsController.IconData(spriteConfig.AtlasId, spriteConfig.SpriteId);
			}

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

			PluginLogger.LogInfo("> Part 2");

			if (contents == null) {
				PluginLogger.LogError("Could not find menu contents.");
				return;
			}

			var newMenu = new MenuContentUIPartController[___mMenuContents.Length + UIUtils.CustomMenuTabs.Count];
			Array.Copy(___mMenuContents, newMenu, ___mMenuContents.Length);

			for (int i = ___mMenuContents.Length, j = 0; i < newMenu.Length; i++, j++) {
				var controller = UIUtils.CustomMenuTabs[j].MenuPrefabController;
				var menuObject = GameObject.Instantiate(controller.gameObject, contents);

				menuObject.SetActive(true);
				newMenu[i] = (MenuContentUIPartController) menuObject.GetComponent(controller.GetType());

				__instance.AttachUIParts(newMenu[i]);
				if (newMenu[i] != null)
					newMenu[i].Deactivate();
			}

			___mMenuContents = newMenu;
		}
	}
}
