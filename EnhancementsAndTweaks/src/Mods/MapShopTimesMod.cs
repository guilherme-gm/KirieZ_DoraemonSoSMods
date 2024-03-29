using BepInEx;
using HarmonyLib;
using UnityEngine;
using System.Text;
using kzModUtils.UI;
using kzModUtils.UI.Events;
using kzModUtils.UI.Elements;
using kzModUtils.Resource;
using BepInEx.Configuration;

namespace EnhancementsAndTweaks.Mods
{
	public class MapShopTimesMod : IMod
	{
		private struct ShopTimes
		{
			public string shopName;

			public string[] times;
		}

		private static GameObject ShopTimeMenuPrefab;

		private static GameObject ShopTimeMenu;

		private static bool isAnywhereDoor = false;

		internal static readonly string TweakName = "Map Shop Times";

		private readonly ShopTimes[] Times =
		{
			new ShopTimes() {
				shopName = "Cafe",
				times = new string[]
				{
					"12 ~ 16",
					"18 ~ 22",
					"Closed: Mon/Fri"
				}
			},
			new ShopTimes() {
				shopName = "Carpenter",
				times = new string[]
				{
					"10 ~ 12",
					"16 ~ 20",
					"Closed: Tue/Sat"
				}
			},
			new ShopTimes() {
				shopName = "Chicken Shop",
				times = new string[]
				{
					"9 ~ 15",
					"Closed: Tue/Fri"
				}
			},
			new ShopTimes() {
				shopName = "Clinic",
				times = new string[]
				{
					"10 ~ 12",
					"14 ~ 17",
					"Sat: 10 ~ 12 only",
					"Closed: Thu/Sun"
				}
			},
			new ShopTimes() {
				shopName = "Cow Shop",
				times = new string[]
				{
					"9 ~ 12",
					"13 ~ 16",
					"Closed: Wed/Fri"
				}
			},
			new ShopTimes() {
				shopName = "Fish Shop",
				times = new string[]
				{
					"8 ~ 12",
					"14 ~ 16",
					"Closed: Mon/Wed/Fri"
				}
			},
			new ShopTimes() {
				shopName = "General Store",
				times = new string[]
				{
					"15 ~ 20",
					"Closed: Thu"
				}
			},
			new ShopTimes() {
				shopName = "Smithy",
				times = new string[]
				{
					"10 ~ 12",
					"14 ~ 17",
					"Closed: Mon/Sun"
				}
			},
		};

		string IMod.GetName()
		{
			return TweakName;
		}

		string IMod.GetDescription()
		{
			return
				"Adds a button in game's mini-map to displays a window with every shop working hours."
				;
		}

		bool IMod.PreInstall(ConfigFile config, AssetBundle assets)
		{
			UIUtils.OnGameUIReady += UIModule_OnGameUIReady;
			return true;
		}

		private void UIModule_OnGameUIReady(object sender, GameUIReadyEventArgs e)
		{
			StringBuilder sb = new StringBuilder();
			foreach (var shop in this.Times)
			{
				sb.Append($"** {shop.shopName} **\n");
				foreach (var time in shop.times)
				{
					sb.Append($"- {time}\n");
				}

				sb.Append("\n");
			}

			ShopTimeMenuPrefab = (new BackgroundImageBuilder())
				.SetCanvasAsParent()
				.SetPosition(new Vector3(15, 30))
				.SetSize(new Vector2(200, 660))
				.Build()
				.gameObject;


			(new TextBuilder())
				.SetParent(ShopTimeMenuPrefab.transform)
				.SetPosition(new Vector3(5, 10))
				.SetSize(new Vector2(190, 25))
				.SetText("- Shop Times -")
				.SetFontSize(22)
				.SetAlignment(TextAnchor.UpperCenter)
				.Build();

			(new TextBuilder())
				.SetParent(ShopTimeMenuPrefab.transform)
				.SetPosition(new Vector3(5, 40))
				.SetSize(new Vector2(190, 620))
				.SetText(sb.ToString())
				.Build();

			ShopTimeMenuPrefab.SetActive(false);
		}

		/**
		 * Override original button set
		 */
		[HarmonyPatch(typeof(MenuContentUIPartController), "ButtonInfoButtons", MethodType.Getter)]
		[HarmonyPrefix]
		public static void ButtonInfoButtons(ref bool __runOriginal, ref int[] __result, ref MenuContentUIPartController __instance)
		{
			if (!(__instance is MiniMapMenuUIPartController))
				return;

			__runOriginal = false;

			__result = new int[] {
				Define.Input.ActionButton.Menu_Submit,
				Define.Input.ActionButton.Menu_Close,
			};
		}

		private static void DestroyShopMenu()
		{
			if (ShopTimeMenu == null)
				return;

			ShopTimeMenu.SetActive(false);
			GameObject.Destroy(ShopTimeMenu);
			ShopTimeMenu = null;
		}

		/**
		 * Override original button texts
		 */
		[HarmonyPatch(typeof(MenuContentUIPartController), "ButtonInfoTexts", MethodType.Getter)]
		[HarmonyPrefix]
		public static void ButtonInfoTexts(ref bool __runOriginal, ref string[] __result, ref MenuContentUIPartController __instance)
		{
			if (!(__instance is MiniMapMenuUIPartController))
				return;

			__runOriginal = false;

			__result = new string[] {
				"Show/Hide Shop Times",
				ResourceUtils.GetText(TextID.Common.TEXT_CLOSE),
			};
		}

		[HarmonyPatch(typeof(UIPartController), "SubmitAction")]
		[HarmonyPrefix]
		public static void SubmitAction(ref bool __runOriginal, ref UIPartController __instance)
		{
			if (!(__instance is MiniMapMenuUIPartController) || isAnywhereDoor)
				return;

			__runOriginal = false;

			if (ShopTimeMenu != null)
			{
				DestroyShopMenu();
			}
			else
			{
				ShopTimeMenu = GameObject.Instantiate(ShopTimeMenuPrefab, UIUtils.CommonUICanvas.transform);
				ShopTimeMenu.SetActive(true);
			}
		}

		[HarmonyPatch(typeof(MenuUIController), "UpdateContents")]
		[HarmonyPostfix]
		public static void MenuChange(ref MenuContentUIPartController ___mCurrentMenuContent)
		{
			if (!(___mCurrentMenuContent is MiniMapMenuUIPartController))
				DestroyShopMenu();
		}

		[HarmonyPatch(typeof(MenuContentUIPartController), "ExitContents")]
		[HarmonyPrefix]
		public static void MenuClose()
		{
			DestroyShopMenu();
			isAnywhereDoor = false;
		}

		[HarmonyPatch(typeof(MiniMapMenuUIPartController), "ToDokodemoDoorCursor")]
		[HarmonyPrefix]
		public static void AnywhereDoorCursortSet()
		{
			isAnywhereDoor = true;
		}

		[HarmonyPatch(typeof(DokodemoDoorUIController), "Submit")]
		[HarmonyPostfix]
		public static void AnywhereDoorSubmit(int __result)
		{
			if (__result > 0)
				isAnywhereDoor = false;
		}
	}
}
