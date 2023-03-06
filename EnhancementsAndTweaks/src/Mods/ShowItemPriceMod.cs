using HarmonyLib;
using System;
using UnityEngine;
using System.Collections.Generic;
using kzModUtils.UI.Elements;
using BepInEx.Configuration;
using EnhancementsAndTweaks.ShowItemPrice;

namespace EnhancementsAndTweaks.Mods
{
	public class ShowItemPriceMod : IMod
	{
		internal static readonly string TweakName = "Show Item Price";

		private static Dictionary<ItemExplainController, ItemPriceController> PriceControllers = new Dictionary<ItemExplainController, ItemPriceController>();

		string IMod.GetName()
		{
			return TweakName;
		}

		string IMod.GetDescription()
		{
			return
				"Adds a box beside with item price beside item description in inventory and storages UI."
				;
		}

		bool IMod.PreInstall(ConfigFile config, AssetBundle assets)
		{
			return true;
		}

		private static MessageBoxStyles? GetBoxStyle(ItemExplainController __instance)
		{
			if (__instance?.transform?.parent?.transform?.parent?.transform?.parent?.GetComponent<InventoryUIPartController>() != null)
				return MessageBoxStyles.Brown;

			var chestController = __instance?.transform?.parent?.transform?.parent?.transform?.parent?.GetComponent<ChestUIPartController>();
			if (chestController != null)
			{
				switch (chestController.StorageType)
				{
					case Define.Storage.TypeEnum.Chest:
						return MessageBoxStyles.Green;

					case Define.Storage.TypeEnum.MaterialStoringSite:
						return MessageBoxStyles.Red;

					case Define.Storage.TypeEnum.Refrigerator:
						return MessageBoxStyles.Blue;

					case Define.Storage.TypeEnum.ShippingBox:
						return MessageBoxStyles.Purple;
				}
			}

			return null;
		}

		[HarmonyPatch(typeof(ItemExplainController), "Initialize")]
		[HarmonyPrefix]
		static void InitItemExplain(ItemExplainController __instance)
		{
			if (PriceControllers.ContainsKey(__instance))
				return;

			var rect = __instance.gameObject.GetComponent<RectTransform>();
			if (rect == null)
			{
				Console.WriteLine("ItemExplainController doesn't have a RectTransform.");
				return;
			}

			MessageBoxStyles? style = GetBoxStyle(__instance);
			if (style == null)
				return;

			var msgBox = (new MessageBoxBuilder())
				.SetParent(__instance.gameObject.transform)
				.SetPosition(new Vector3(350, -83))
				.SetSize(new Vector2(150, 140))
				.SetTitle("Price Info")
				.SetStyle((MessageBoxStyles) style)
				.Build();
			var controller = msgBox.gameObject.AddComponent<ItemPriceController>();
			controller.Initialize(msgBox);

			PriceControllers.Add(__instance, controller);
		}

		[HarmonyPatch(typeof(ItemExplainController), "SetItem")]
		[HarmonyPrefix]
		static void SetItemWithPrice(ItemExplainController __instance, ItemModel item)
		{
			if (!PriceControllers.TryGetValue(__instance, out var controller)) {
				Console.WriteLine("Could not find PriceController for Window");
				return;
			}

			controller.SetItem(item);
		}
	}
}
