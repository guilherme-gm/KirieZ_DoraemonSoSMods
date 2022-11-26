using BepInEx;
using BepInEx.Configuration;
using HarmonyLib;
using System;

namespace SortLists
{
	[BepInPlugin("io.github.guilherme-gm.DoraemonSoSMods.sortLists", "Sort Lists", PluginInfo.PLUGIN_VERSION)]
	public class Plugin : BaseUnityPlugin
	{
		private static ConfigEntry<bool> SortRecipeList;

		private static ConfigEntry<bool> SortRecipeShopList;

		private static ConfigEntry<bool> SortProduceShopList;

		private static ConfigEntry<bool> SortMealsShopList;


		private ConfigEntry<bool> CreateBind(string key, bool defaultValue, string displayName)
		{
			return Config.Bind("Sort Target", key, defaultValue, $"Whether to sort or not {displayName}");
		}

		private void Awake()
		{
			// Plugin startup logic
			Logger.LogInfo($"Plugin {PluginInfo.PLUGIN_GUID} is loaded!");

			Plugin.SortRecipeList = this.CreateBind("SortRecipeList", true, "Kitchen's recipe list");
			Plugin.SortRecipeShopList = this.CreateBind("SortRecipeShopList", true, "Cafet's Recipe Shop list");
			Plugin.SortProduceShopList = this.CreateBind("SortProduceShopList", false, "Cafet's Produce list (vegetables/etc)");
			Plugin.SortMealsShopList = this.CreateBind("SortMealsShopList", false, "Cafet's Meals shop list (food shop)");

			Harmony.CreateAndPatchAll(typeof(Plugin));
		}

		private static void SortIfSub(bool shouldSort, ShopItemDataModel[] list)
		{
			if (shouldSort)
				Array.Sort(list, (a, b) => a.Name.CompareTo(b.Name));
		}

		[HarmonyPatch(typeof(RecipeListUIPartController), "Initialize")]
		[HarmonyPrefix]
		private static void OnInitRecipeList(ref CookingRecipeDataModel[] recipe_datas)
		{
			if (!Plugin.SortRecipeList.Value)
				return;

			Array.Sort(recipe_datas, (a, b) => a.Name.CompareTo(b.Name));
		}

		#region Restaurant

		[HarmonyPatch(typeof(FarmModel), "GetRestaurantShopCookingRecipeItems")]
		[HarmonyPostfix]
		private static void OnRecipeShopList(ref ShopItemDataModel[] __result)
		{
			SortIfSub(Plugin.SortRecipeShopList.Value, __result);
		}

		[HarmonyPatch(typeof(FarmModel), "GetRestaurantShopDishItems")]
		[HarmonyPostfix]
		private static void OnRestaurantMeals(ref ShopItemDataModel[] __result)
		{
			SortIfSub(Plugin.SortMealsShopList.Value, __result);
		}

		[HarmonyPatch(typeof(FarmModel), "GetRestaurantShopCropItems")]
		[HarmonyPostfix]
		private static void OnRestauranteProduce(ref ShopItemDataModel[] __result)
		{
			SortIfSub(Plugin.SortProduceShopList.Value, __result);
		}

		#endregion

	}
}
