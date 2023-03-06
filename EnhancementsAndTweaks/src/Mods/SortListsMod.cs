using BepInEx.Configuration;
using HarmonyLib;
using System;
using UnityEngine;

namespace EnhancementsAndTweaks.Mods
{
	public class SortListsMod : IMod
	{
		internal static readonly string TweakName = "Sort Lists";

		private static ConfigEntry<bool> SortRecipeList;

		private static ConfigEntry<bool> SortRecipeShopList;

		private static ConfigEntry<bool> SortProduceShopList;

		private static ConfigEntry<bool> SortMealsShopList;


		private ConfigEntry<bool> CreateBind(ConfigFile config, string key, bool defaultValue, string displayName)
		{
			return config.Bind("Sort Target", key, defaultValue, $"Whether to sort or not {displayName}");
		}

		string IMod.GetName()
		{
			return TweakName;
		}

		string IMod.GetDescription()
		{
			return
				"Sorts some list menus alphabetically."
				;
		}

		bool IMod.PreInstall(ConfigFile config, AssetBundle assets)
		{
			SortListsMod.SortRecipeList = this.CreateBind(config, "SortRecipeList", true, "Kitchen's recipe list");
			SortListsMod.SortRecipeShopList = this.CreateBind(config, "SortRecipeShopList", true, "Cafet's Recipe Shop list");
			SortListsMod.SortProduceShopList = this.CreateBind(config, "SortProduceShopList", false, "Cafet's Produce list (vegetables/etc)");
			SortListsMod.SortMealsShopList = this.CreateBind(config, "SortMealsShopList", false, "Cafet's Meals shop list (food shop)");

			return true;
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
			if (!SortListsMod.SortRecipeList.Value)
				return;

			Array.Sort(recipe_datas, (a, b) => a.Name.CompareTo(b.Name));
		}

		#region Restaurant

		[HarmonyPatch(typeof(FarmModel), "GetRestaurantShopCookingRecipeItems")]
		[HarmonyPostfix]
		private static void OnRecipeShopList(ref ShopItemDataModel[] __result)
		{
			SortIfSub(SortListsMod.SortRecipeShopList.Value, __result);
		}

		[HarmonyPatch(typeof(FarmModel), "GetRestaurantShopDishItems")]
		[HarmonyPostfix]
		private static void OnRestaurantMeals(ref ShopItemDataModel[] __result)
		{
			SortIfSub(SortListsMod.SortMealsShopList.Value, __result);
		}

		[HarmonyPatch(typeof(FarmModel), "GetRestaurantShopCropItems")]
		[HarmonyPostfix]
		private static void OnRestauranteProduce(ref ShopItemDataModel[] __result)
		{
			SortIfSub(SortListsMod.SortProduceShopList.Value, __result);
		}

		#endregion

	}
}
