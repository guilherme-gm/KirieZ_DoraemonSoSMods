using System;
using System.Collections.Generic;
using HarmonyLib;

namespace kzModUtils.ItemData
{
	internal class ItemModule: IModule, ICollectionModule
	{
		#region Singleton setup
		private static ItemModule mInstance;

		internal static ItemModule Instance
		{
			get {
				if (mInstance == null)
					mInstance = new ItemModule();

				return mInstance;
			}
			private set {}
		}
		#endregion

		private Dictionary<int, ItemMasterModel> Items;

		[HarmonyPatch(typeof(ItemMasterCollection), "Setup")]
		[HarmonyPostfix]
		private static void OnItemSetup(Dictionary<int, ItemMasterModel> ___mItems)
		{
			ItemModule.Instance.Items = ___mItems;
		}

		private static Dictionary<Define.Item.CategoryEnum, int[]> GetFreeIdPool()
		{
			// List manually generated based on existing items and some range for safety
			return new Dictionary<Define.Item.CategoryEnum, int[]>()
			{
				{ Define.Item.CategoryEnum.Tool,               new int[] { 10_03200, 10_99999 } },
				{ Define.Item.CategoryEnum.ConsumptionTool,    new int[] { 11_07000, 11_99999 } },
				{ Define.Item.CategoryEnum.Seed,               new int[] { 20_00600, 20_99999 } },
				{ Define.Item.CategoryEnum.Crop,               new int[] { 30_00600, 30_99999 } },
				{ Define.Item.CategoryEnum.Gather,             new int[] { 31_00500, 31_99999 } },
				{ Define.Item.CategoryEnum.AnimalProductFoods, new int[] { 32_00200, 32_99999 } },
				{ Define.Item.CategoryEnum.Dish,               new int[] { 33_01000, 33_99999 } },
				{ Define.Item.CategoryEnum.ProcessedFoods,     new int[] { 34_00200, 34_99999 } },
				{ Define.Item.CategoryEnum.Mineral,            new int[] { 40_01200, 40_99999 } },
				{ Define.Item.CategoryEnum.AnimalProducts,     new int[] { 41_00104, 40_99999 } },
				{ Define.Item.CategoryEnum.Fish,               new int[] { 50_00200, 50_99999 } },
				{ Define.Item.CategoryEnum.NewFish,            new int[] { 51_00300, 51_99999 } },
				{ Define.Item.CategoryEnum.Bug,                new int[] { 60_00200, 60_99999 } },
				{ Define.Item.CategoryEnum.Furniture,          new int[] { 70_68000, 70_99999 } },
				{ Define.Item.CategoryEnum.Material,           new int[] { 71_12000, 71_99999 } },
				{ Define.Item.CategoryEnum.CookingTool,        new int[] { 90_11000, 90_99999 } },
			};
		}

		internal Dictionary<string, CustomItem> ItemIdMap = new Dictionary<string, CustomItem>();

		internal List<CustomItemConfig> CustomItemConfigs = new List<CustomItemConfig>();

		private Dictionary<Define.Item.CategoryEnum, int[]> IdPool = GetFreeIdPool();

		public void Initialize()
		{
			Harmony.CreateAndPatchAll(typeof(ItemModule));
		}

		public void Setup()
		{
			foreach (var config in this.CustomItemConfigs)
			{
				var idRange = this.IdPool.GetValue(config.Category, null);
				if (idRange == null) {
					Console.WriteLine($"Could not find ID Pool for item \"{config.ModItemID}\". Skipping item.");
					continue;
				}

				while (idRange[0] <= idRange[1] && Items.ContainsKey(idRange[0]))
					idRange[0]++;

				if (idRange[0] > idRange[1]) {
					Console.WriteLine($"ID Pool for \"{config.Category}\" is full, no more custom items of that type can be added. Skipping item \"{config.ModItemID}\".");
					continue;
				}

				var itemId = idRange[0];
				idRange[0]++;

				var item = config.ToMasterModel(itemId);
				this.Items.Add(itemId, item);
				this.ItemIdMap.Add(config.ModItemID, item);

				config.ItemId = itemId;
				config.Callback?.Invoke(item);
			}
		}

		public void Teardown()
		{

		}
	}
}
