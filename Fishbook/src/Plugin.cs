using BepInEx;
using HarmonyLib;
using System;
using UnityEngine;
using kzModUtils.Resource;
using kzModUtils.ItemData;
using kzModUtils.ShopData;
using kzModUtils.EventData;
using kzModUtils.GameSave;
using Fishbook.Entities;
using Fishbook.UI;

namespace Fishbook
{
	[BepInPlugin("io.github.guilherme-gm.DoraemonSoSMods.fishBook", "Fish Book", PluginInfo.PLUGIN_VERSION)]
	[BepInDependency("io.github.guilherme-gm.DoraemonSoSMods.kzModUtils", "0.5.0")]
	public class Plugin : BaseUnityPlugin
	{
		private static AssetBundle Assets;

		private static GameObject FishbookUI;

		public static GameObject FishInfoUI;

		private static CustomItem FishbookItem;

		private void Awake()
		{
			// Plugin startup logic
			Logger.LogInfo($"Plugin {PluginInfo.PLUGIN_GUID} is loaded!");

			Harmony.CreateAndPatchAll(typeof(Plugin));
			Harmony.CreateAndPatchAll(typeof(FishbookBook));

			Assets = AssetBundle.LoadFromFile(ResourceUtils.GetAssetBundlePath(ResourceType.Plugin, "Fishbook/FishBook", ""));
			FishbookUI = Assets.LoadAsset<GameObject>("Fishbook");
			FishInfoUI = Assets.LoadAsset<GameObject>("FishInfo");
			var bookSprite = Assets.LoadAsset<Sprite>("Fishbook_Icon");

			var fishbook = new CustomItemConfig() {
				ModItemID = "fishbook.book",
				Name = "Fish book",
				Description = "A book containing details about fishes you have catched.",
				IsImportant = true,
				Category = Define.Item.CategoryEnum.ConsumptionTool,
				Sprite = ResourceUtils.RegisterSprite(bookSprite),
				ResourceId = 0,
			};

			ItemHelper.Instance.RegisterItem(fishbook, (item) => {
				FishbookItem = item;
			});

			var bookBoughtEvent = new EventConfig("fishbook.book.bought") {
				RequiredItem = new IdHolder<CustomItemConfig>(fishbook),
			};
			ShopHelper.RegisterBuyOnceEvent(bookBoughtEvent);

			ShopHelper.RegisterItemToSell(new ShopItemConfig(new IdHolder<CustomItemConfig>(fishbook)) {
				Price = 1500,
				SellOnceEvent = new IdHolder<EventConfig>(bookBoughtEvent),
				TargetShop = ShopId.VarietyShop,
			});
			ShopHelper.RegisterItemToSell(new ShopItemConfig(new IdHolder<CustomItemConfig>(fishbook)) {
				Price = 1500,
				SellOnceEvent = new IdHolder<EventConfig>(bookBoughtEvent),
				TargetShop = ShopId.VarietyShopFirstYear,
			});

			GameSaveHelper.RegisterSaveHandler(new FishbookSaveDataHandler());
		}

		private static UIController CreateUIController<T>(string name, Transform parent) where T : UIController
		{
			GameObject gameObject = new GameObject(name);
			if (parent != null)
			{
				gameObject.transform.SetParent(parent.transform, false);
				RectTransform rectTransform = gameObject.AddComponent<RectTransform>();
				rectTransform.anchorMin = Vector2.zero;
				rectTransform.anchorMax = Vector2.one;
				rectTransform.sizeDelta = Vector2.zero;
			}
			return gameObject.AddComponent<T>();
		}

		[HarmonyPatch(typeof(UIFactory), "Create", new Type[] { typeof(Define.UI.TypeEnum), typeof(Transform) })]
		[HarmonyPrefix]
		static void CreateUi(
			Define.UI.TypeEnum type,
			Transform parent,
			ref UIController __result,
			ref bool __runOriginal
		)
		{
			if ((int) type != 500)
				return;

			__runOriginal = false;
			__result = CreateUIController<FishbookUIController>("FishbookUI", parent);
		}

		public static FishingDataModel catchFishingData;

		[HarmonyPatch(typeof(FarmFishingState), "Update")]
		[HarmonyPrefix]
		static void OnFarmFishing(FishingDataModel ___mCatchFishingData, int ___mPhase)
		{
			if (___mPhase == 9)
				catchFishingData = ___mCatchFishingData;
			else if (___mPhase == 13)
				catchFishingData = null;
		}

		[HarmonyPatch(typeof(AchievementChecker), "CheckFishing")]
		[HarmonyPrefix]
		static void AchievFish(ItemModel model)
		{
			FishbookBook.Instance.UpdateBook(catchFishingData.FishingPointData);
		}

		[HarmonyPatch(typeof(UIFactory), "Create", new Type[] { typeof(Define.UI.PartTypeEnum), typeof(Transform) })]
		[HarmonyPrefix]
		static void CreateUiPart(
			Define.UI.PartTypeEnum part_type,
			Transform parent,
			ref UIPartController __result,
			ref bool __runOriginal
		)
		{
			if ((int) part_type != 501)
				return;

			__runOriginal = false;
			var go = GameObject.Instantiate(FishbookUI, parent, false);
			__result = go.AddComponent<FishbookWindowUIPartController>();
		}

		[HarmonyPatch(typeof(FishingPointController), "Awake")]
		[HarmonyPostfix]
		static void OnFishPointCreate(FishingPointController __instance, ref ICommand[] ___mCommands, int ___mId)
		{
			var count = SingletonMonoBehaviour<UserManager>.Instance.User.GetItemCountFromInventory(FishbookItem.Id);

			if (SingletonMonoBehaviour<UserManager>.Instance.User.GetItemCountFromInventory(FishbookItem.Id) > 0) {
				var newCommands = new ICommand[___mCommands.Length + 1];
				for (var i = 0; i < ___mCommands.Length; i++)
					newCommands[i] = ___mCommands[i];

				newCommands[newCommands.Length - 1] = new FishbookCommand(__instance, ___mId);

				___mCommands = newCommands;
			}
		}
	}
}
