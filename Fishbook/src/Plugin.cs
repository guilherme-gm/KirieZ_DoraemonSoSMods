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
	[BepInDependency("io.github.guilherme-gm.DoraemonSoSMods.kzModUtils", "0.6.1")]
	public class Plugin : BaseUnityPlugin
	{
		private static AssetBundle Assets;

		private static GameObject FishbookUI;

		public static GameObject FishInfoUI;

		public static CustomItem FishbookItem;

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

		[HarmonyPatch(
			typeof(FarmFreeState),
			MethodType.Constructor,
			new Type[] {
				typeof(FarmScheduleRegisterer),
				typeof(FarmPlayerController),
				typeof(FarmCameraController),
				typeof(GroundController),
				typeof(FloorController),
				typeof(NpcGroupController),
				typeof(AnimalGroupController),
				typeof(BugGroupController),
				typeof(Transform),
				typeof(Action<WarpPointModel, bool, System.Action>),
				typeof(Action<Define.Scene.Farm.StateEnum, ICommandHolderObject, ICommand>),
				typeof(DailyScheduler)
			}
		)]
		[HarmonyPostfix]
		static void OnFarmFreeStateCreated(
			FarmFreeState __instance,
			ref ICommand[] ___mItemUseCommands
		)
		{
			var newCommands = new ICommand[___mItemUseCommands.Length + 1];
			for (var i = 0; i < ___mItemUseCommands.Length; i++)
				newCommands[i] = ___mItemUseCommands[i];

			newCommands[newCommands.Length - 1] = new FishbookCommand(900, -1);

			___mItemUseCommands = newCommands;
		}

		[HarmonyPatch(typeof(FarmTalkState), "BeginCallback")]
		[HarmonyPrefix]
		static void OnFarmTalkStateCallback(
			ICommandHolderObject collided_obj,
			ICommand stacked_command,
			Action<Define.Scene.Farm.StateEnum, ICommandHolderObject, ICommand> ___mChangeStateCallback,
			DailyScheduler ___mScheduler,
			ResponseReader ___mResponseReader,
			ref ICommandHolderObject ___mCollidedObj,
			ref ICommand ___mStackedCommand,
			ref bool __runOriginal
		)
		{
			if (!(stacked_command is FishbookCommand)) {
				return;
			}

			__runOriginal = false;

			if (___mScheduler != null)
				___mScheduler.Pause();

			ResponseModel response = null;
			if (!stacked_command.Execute(out response))
			{
				if (SingletonMonoBehaviour<EventManager>.Instance.IsEventQueuing)
					SingletonMonoBehaviour<SceneManager>.Instance.GoToEvent(Define.System.Save.SlotEnum.None);
				else
					___mChangeStateCallback(Define.Scene.Farm.StateEnum.Free, null, null);
				return;
			}

			___mResponseReader.Read(response);
			___mCollidedObj = collided_obj;
			___mStackedCommand = stacked_command;
		}
	}
}
