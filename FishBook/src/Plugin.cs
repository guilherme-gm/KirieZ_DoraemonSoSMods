using BepInEx;
using HarmonyLib;
using System;
using UnityEngine;
using System.Collections.Generic;
using kzModUtils.UI.Elements;
using kzModUtils.Resource;
using System.Text;
using System.IO;
using System.Reflection;

namespace FishBook
{
	[BepInPlugin("io.github.guilherme-gm.DoraemonSoSMods.fishBook", "Fish Book", PluginInfo.PLUGIN_VERSION)]
	public class Plugin : BaseUnityPlugin
	{
		public static readonly int ATLAS_ID = 99999;

		private static AssetBundle Assets;

		private static GameObject FishbookUI;

		public static GameObject FishInfoUI;

		private void Awake()
		{
			// Plugin startup logic
			Logger.LogInfo($"Plugin {PluginInfo.PLUGIN_GUID} is loaded!");

			Harmony.CreateAndPatchAll(typeof(Plugin));
			Harmony.CreateAndPatchAll(typeof(Fishbook));
			Harmony.CreateAndPatchAll(typeof(PatchGameSave));
			Harmony.CreateAndPatchAll(typeof(PatchLoadGame));

			Assets = AssetBundle.LoadFromFile(ResourceUtils.GetAssetBundlePath(ResourceType.Plugin, "Fishbook/FishBook", ""));
			FishbookUI = Assets.LoadAsset<GameObject>("Fishbook");
			FishInfoUI = Assets.LoadAsset<GameObject>("FishInfo");
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

		[HarmonyPatch(typeof(FarmContentsController), "Start")]
		[HarmonyPostfix]
		static void OnFarmStart()
		{
			// Console.WriteLine("Starting Fishbook");
			// Fishbook.Instance.Initialize();
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
			Console.WriteLine("Creating ... " + type);
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
			Fishbook.Instance.UpdateBook(catchFishingData.FishingPointData);
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
			Console.WriteLine("Creating Part ... " + part_type);
			if ((int) part_type != 501)
				return;

			__runOriginal = false;
			var go = GameObject.Instantiate(FishbookUI, parent, false);
			__result = go.AddComponent<FishbookWindowUIPartController>();
		}


		[HarmonyPatch(typeof(TextMasterCollection), "GetText")]
		[HarmonyPrefix]
		static void OnGetText(
			int text_id,
			ref string __result,
			ref bool __runOriginal
		)
		{
			string text = null;
			switch (text_id) {
				case 2_110_000_000:
					text = "Fish Book";
					break;

				case 2_110_000_001:
					text = "The fish book";
					break;
			}

			if (text == null)
				return;

			__result = text;
			__runOriginal = false;
		}

		[HarmonyPatch(typeof(ItemMasterCollection), "Setup")]
		[HarmonyPostfix]
		static void OnItemSetup(
			ItemMasterCollection __instance,
			Dictionary<int, ItemMasterModel> ___mItems
		)
		{
			// 1002130,1002130,    -1,     -1,       False,            True,         False,              -1,1010021300,    1010021301,        -1,     -1,   2130,         -1,          -1,       2130,   20101,  1002130,       0,       0
			var item = new ItemMasterModel(new CItemData.SItemData() {
				mCropId = -1,
				mItemId = 11_000_000,
				mNameId = 2_110_000_000,
				mDescriptionId = 2_110_000_001,
				mFoodGroup = -1,
				mFurnitureId = -1,
				mHasQuality = false,
				mIngredientsPoint = -1,
				mIsImportantItem = true,
				mIsIngredients = false,
				mMaxSize = 0,
				mMinSize = 0,
				mOrnamentId = -1,
				mPrice = -1,
				mRecover = -1,
				mToolId = -1,
				//
				mAtlasId = 20101, // ATLAS_ID,
				mResourceId = 2130, // FIXME: What is exactly this used for?
				mSpriteId = 1002130, // 1,
			});
			___mItems.Add(11_000_000, item);
			// var v = __instance.ItemsByCategory[(int) Define.Item.CategoryEnum.Tool];
			// Setup item
			// Setup Catgory
			// Setup tool data (if needed)
		}

		[HarmonyPatch(typeof(AtlasFactory), "LoadSprite")]
		[HarmonyPrefix]
		static void OnLoadSprite(
			int atlas_id, int sprite_id, Action<UnityEngine.Sprite> loaded_callback,
			ref bool __runOriginal
		)
		{
			if (atlas_id != ATLAS_ID) {
				return;
			}

			__runOriginal = false;
			loaded_callback(null); // FIXME: include sprite
		}

		[HarmonyPatch(typeof(CVarietyShopData), "Setup")]
		[HarmonyPostfix]
		static void OnShop()
		{
			CVarietyShopData.mVarietyShopDataStructDic.Add(400100, new CVarietyShopData.SVarietyShopData() {
				mDLCIndex = 30,
				mIsFirstYear = true,
				mItemId = 11_000_000,
				mPrice = 100,
				mSeason = -1,
				mVarietyId = 400100,
			});

			// Nothing uses it and we can't patch it without reflection, let's hope this owrks
			// CVarietyShopData.Length = CVarietyShopData.mVarietyShopDataStructDic.Count;
		}

		[HarmonyPatch(typeof(Define.Event), "DLCItemIdToEventId")]
		[HarmonyPrefix]
		static void DlcItemToEvent(
			int item_id,
			ref bool __runOriginal,
			ref int __result
		)
		{
			if (item_id != 11_000_000)
				return;

			__runOriginal = false;
			__result = 81120050;
		}

		[HarmonyPatch(typeof(Define.Item), "IsDLCItem")]
		[HarmonyPrefix]
		static void IsDlcItem(
			int item_id,
			ref bool __runOriginal,
			ref bool __result
		)
		{
			if (item_id != 11_000_000)
				return;

			__runOriginal = false;
			__result = true;
		}

		[HarmonyPatch(typeof(FishingPointController), "Awake")]
		[HarmonyPostfix]
		static void OnFishPointCreate(FishingPointController __instance, ref ICommand[] ___mCommands, int ___mId)
		{
			var count = SingletonMonoBehaviour<UserManager>.Instance.User.GetItemCountFromInventory(11_000_000);

			if (SingletonMonoBehaviour<UserManager>.Instance.User.GetItemCountFromInventory(11_000_000) > 0) {
				var newCommands = new ICommand[___mCommands.Length + 1];
				for (var i = 0; i < ___mCommands.Length; i++)
					newCommands[i] = ___mCommands[i];

				newCommands[newCommands.Length - 1] = new FishbookCommand(__instance, ___mId);

				___mCommands = newCommands;
			}
		}
/*
		[HarmonyPatch(typeof(SaveDataManager), "Load")]
		[HarmonyPostfix]
		static void OnLoadGame(
			SaveDataManager __instance,
			UserModel load_data,
			string file_name,
			ref SaveDataManager.Result __result,
			string ___mDataPath
		)
		{
			Console.WriteLine(" Loading");
			if (__result != SaveDataManager.Result.Success)
				return;

			Console.WriteLine(" Loading 2" + file_name);
			Console.WriteLine(" Loading 2_a" + load_data);
			if (!(load_data is UserModel))
				return;

			Console.WriteLine(" Loading 3");

			Fishbook.Instance.Initialize();
			string path = string.Format("{0}/{1}_fishbook.json", ___mDataPath, file_name);
			if (!File.Exists(path))
			{
				Console.WriteLine("New fishbook");
				// Fishbook doesn't exists yet, maybe it was lost / first time using the mod. That's fine.
				return;
			}

			SaveDataManager.Result result = SaveDataManager.Result.Failure;
			using (FileStream fileStream = new FileStream(path, FileMode.Open, FileAccess.Read))
			{
				using (BinaryReader binaryReader = new BinaryReader(fileStream))
				{
					byte[] bytes = binaryReader.ReadBytes((int)fileStream.Length);
					string json = Encoding.UTF8.GetString(bytes);
					binaryReader.Close();
					fileStream.Close();
					try {
						JsonUtility.FromJsonOverwrite(json, Fishbook.Instance);
					}
					catch
					{
						Console.WriteLine("Fishbook Load failed.");
						__result = SaveDataManager.Result.Failure;
						return;
					}
					result = SaveDataManager.Result.Success;
				}
			}

			Console.WriteLine("Fishbook Loaded.");

			__result = result;
		}
*/
	}
}
