using BepInEx.Configuration;
using HarmonyLib;
using kzModUtils.Resource;
using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace EnhancementsAndTweaks.Mods
{

	internal static class RecipeExtensions {
		private static FieldInfo CursorIndex = typeof(RecipeListUIPartController)
			.GetField("mCursorIndex", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

		private static MethodInfo MoveCursor = typeof(RecipeListUIPartController)
			.GetMethod("MoveCursor", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

		public static void ResetCursor(this RecipeListUIPartController controller)
		{
			int? val = CursorIndex.GetValue(controller) as int?;
			if (val == null) {
				Console.WriteLine("ResetCursor: Could not find 'val' value.");
				return; // should never happen, but just in case
			}

			MoveCursor.Invoke(controller, new object[] { -val });
		}
	}

	public class ShowCanMakeRecipesMod : IMod
	{
		internal static readonly string TweakName = "Show Can Make Recipes";

		private static CookingRecipeDataModel[] RecipeList;

		private static bool CanMakeOnly = false;

		string IMod.GetName()
		{
			return TweakName;
		}

		string IMod.GetDescription()
		{
			return
				"Add button to filters kitchen's recipe list to only the recipes you have ingredients for."
				;
		}

		bool IMod.PreInstall(ConfigFile config, AssetBundle assets)
		{
			return true;
		}

		[HarmonyPatch(typeof(RecipeListUIController), "Initialize")]
		[HarmonyPostfix]
		private static void OnInitKitchenUI(
			RecipeListUIController __instance,
			ButtonGridUIPartController ___mButtonGrid,
			RecipeListUIPartController ___mRecipeListUIPartController,
			UIArgument arg
		)
		{
			CanMakeOnly = false;

			// Most likely this UI is for shop instead of cooking
			if (___mRecipeListUIPartController == null)
				return;

			RecipeListUIController.Argument argument = arg as RecipeListUIController.Argument;
			RecipeList = argument.RecipeDatas;
			string[] info_texts = new string[]
			{
				SingletonMonoBehaviour<MasterManager>.Instance.TextMaster.GetText(TextID.Common.TEXT_OK),
				"Can cook only/All recipes",
				SingletonMonoBehaviour<MasterManager>.Instance.TextMaster.GetText(Define.Text.Common.ID_0000_0049),
				SingletonMonoBehaviour<MasterManager>.Instance.TextMaster.GetText(TextID.Common.TEXT_CANCEL)
			};

			int[] info_buttons = new int[]
			{
				Define.Input.ActionButton.Menu_Submit, // X
				Define.Input.ActionButton.Menu_Revert, // []
				Define.Input.ActionButton.Menu_Confirm, // /\
				Define.Input.ActionButton.Menu_Close // O
			};

			___mButtonGrid.SetButtonInformations(info_texts, info_buttons);
		}

		private static bool CanMakeRecipe(CookingRecipeDataModel recipe)
		{
			if (
				recipe.IsNeedCookingTool
				&& SingletonMonoBehaviour<UserManager>.Instance.User.Farm.GetCookingTool(recipe.CookingToolId) == null
			)
				return false;

			int i = 0;
			while (i < recipe.MaterialItems.Length) {
				ItemModel itemModel = recipe.MaterialItems[i];
				int num = SingletonMonoBehaviour<UserManager>.Instance.User.GetItemCountFromInventory(itemModel.Id, -1);
				num += SingletonMonoBehaviour<UserManager>.Instance.User.Refrigerator.GetItemCount(itemModel.Id, -1);

				if (num < itemModel.Count)
					return false;

				i++;
			}

			return true;
		}

		[HarmonyPatch(typeof(RecipeListUIController), "InputButtonDownCallback")]
		[HarmonyPostfix]
		private static void OnKitchButtonDown(
			RecipeListUIController __instance,
			RecipeListUIPartController ___mRecipeListUIPartController,
			ref InputButtonModel buttons
		)
		{
			// Most likely this UI is for shop instead of cooking
			if (___mRecipeListUIPartController == null)
				return;

			if (buttons.Has(Define.Input.ActionButton.Menu_Revert)) {
				CanMakeOnly = !CanMakeOnly;

				___mRecipeListUIPartController.ResetCursor();
				if (!CanMakeOnly) {
					___mRecipeListUIPartController.Initialize(RecipeList);
				} else {
					List<CookingRecipeDataModel> filteredList = new List<CookingRecipeDataModel>();
					foreach (var recipe in RecipeList) {
						if (CanMakeRecipe(recipe))
							filteredList.Add(recipe);
					}

					___mRecipeListUIPartController.Initialize(filteredList.ToArray());
				}
			}
		}
	}
}
