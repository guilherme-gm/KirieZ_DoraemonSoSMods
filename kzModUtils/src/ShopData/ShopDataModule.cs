using HarmonyLib;
using System;
using System.Collections.Generic;
using kzModUtils.ItemData;
using kzModUtils.EventData;

namespace kzModUtils.ShopData
{
	internal class ShopDataModule: IModule, ICollectionModule
	{
		private class SellOnceInfo
		{
			public IdHolder<CustomItemConfig> item;
			public IdHolder<EventConfig> BuyEvent;
		}

		#region Singleton Setup
		private static ShopDataModule mInstance;

		internal static ShopDataModule Instance
		{
			get {
				if (mInstance == null)
					mInstance = new ShopDataModule();

				return mInstance;
			}
			private set {}
		}
		#endregion

		internal List<ShopItemConfig> ShopConfigs = new List<ShopItemConfig>();

		internal Dictionary<int, ShopMasterModel> VarietyShopDatas;

		internal Dictionary<int, ShopMasterModel> VarietyShopFirstYearDatas;

		private Dictionary<int, SellOnceInfo> SellOnceConfig = new Dictionary<int, SellOnceInfo>();

		private HashSet<int> SellOnceEventIds = new HashSet<int>();


		public void Initialize()
		{
			Harmony.CreateAndPatchAll(typeof(ShopDataModule));
		}

		public void Teardown()
		{
		}

		[HarmonyPatch(typeof(ShopMasterCollection), "Setup")]
		[HarmonyPostfix]
		static void OnOriginalSetup(
			Dictionary<int, ShopMasterModel> ___mVarietyShopDatas,
			Dictionary<int, ShopMasterModel> ___mFirstYearVarietyShopDatas
		)
		{
			ShopDataModule.Instance.VarietyShopDatas = ___mVarietyShopDatas;
			ShopDataModule.Instance.VarietyShopFirstYearDatas = ___mFirstYearVarietyShopDatas;
		}

		[HarmonyPatch(typeof(Define.Event), "DLCItemIdToEventId")]
		[HarmonyPrefix]
		static void DlcItemToEvent(int item_id, ref bool __runOriginal, ref int __result)
		{
			if (ShopDataModule.Instance.SellOnceConfig.TryGetValue(item_id, out var sellOnceInfo))
			{
				__runOriginal = false;
				__result = sellOnceInfo.BuyEvent.Id;
			}
		}

		[HarmonyPatch(typeof(Define.Item), "IsDLCItem")]
		[HarmonyPrefix]
		static void IsDlcItem(int item_id, ref bool __runOriginal, ref bool __result)
		{
			if (ShopDataModule.Instance.SellOnceConfig.ContainsKey(item_id))
			{
				__runOriginal = false;
				__result = true;
			}
		}

		[HarmonyPatch(typeof(FinishedEventModel), "CheckEventFinished")]
		[HarmonyPrefix]
		static void IsEventFinished(int id, ref bool __runOriginal, ref bool __result)
		{
			if (!ShopDataModule.Instance.SellOnceEventIds.Contains(id))
				return;

			foreach (var config in ShopDataModule.Instance.SellOnceConfig)
			{
				if (config.Value.BuyEvent.Id == id) {
					__runOriginal = false;
					__result = UserManager.Instance.User.ImportantItemSlots.GetItemCount(config.Value.item.Id) > 0;
					return;
				}
			}
		}

		private Dictionary<int, ShopMasterModel> GetTargetShop(ShopId id)
		{
			switch (id)
			{
				case ShopId.VarietyShop:
					return this.VarietyShopDatas;

				case ShopId.VarietyShopFirstYear:
					return this.VarietyShopFirstYearDatas;

				default:
					return null;
			}
		}

		public void Setup()
		{
			foreach (var shopConfig in ShopConfigs)
			{
				var shop = this.GetTargetShop(shopConfig.TargetShop);
				if (shop == null) {
					Console.WriteLine($"Could not find shop \"{shopConfig.TargetShop}\". Skipping item \"{shopConfig.Item.Id}\".");
					return;
				}

				int newId = 0;
				var keys = shop.KeyToArray();
				for (var i = 0; i < keys.Length; i++) {
					if (keys[i] > newId)
						newId = keys[i];
				}

				if (newId >= int.MaxValue) {
					Console.WriteLine($"Can not register more items for shop \"{shopConfig.TargetShop}\". Limit reached.");
					return;
				}

				newId += 1;
				if (shopConfig.SellOnceEvent != null) {
					var item = SingletonMonoBehaviour<MasterManager>.Instance.ItemMaster.GetItem(shopConfig.Item.Id);
					if (!item.IsImportantItem) {
						Console.WriteLine($"Only important items may receive SellOnceEvent. Skipping item \"{item.Id}\"");
						continue;
					}

					if (this.SellOnceConfig.ContainsKey(shopConfig.Item.Id))
						continue;

					this.SellOnceConfig.Add(
						shopConfig.Item.Id,
						new SellOnceInfo() { item = shopConfig.Item, BuyEvent = shopConfig.SellOnceEvent }
					);
					this.SellOnceEventIds.Add(shopConfig.SellOnceEvent.Id);
				}

				shop.Add(newId, shopConfig.ToShopMasterModel(newId));
			}
		}
	}
}
