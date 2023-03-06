using BepInEx.Configuration;
using Define;
using HarmonyLib;
using kzModUtils.UI;
using System;
using UnityEngine;

namespace EnhancementsAndTweaks.Mods
{
	public class ConfirmBeehiveRemovalMod : IMod
	{
		internal static readonly string TweakName = "Confirm Beehive Removal";

		private static readonly int ITEM_BEEHIVE = 7050000;

		string IMod.GetName()
		{
			return TweakName;
		}

		string IMod.GetDescription()
		{
			return
				"Adds a dialog to confirm before you remove a placed beehive.\n"
				+ "This prevents you from accidentally removing a beehive that has bees and end up losing Honey and Bees."
				;
		}

		bool IMod.PreInstall(ConfigFile config, AssetBundle assets)
		{
			return true;
		}

		[HarmonyPatch(typeof(FurnitureRemoveCommand), "Execute")]
		[HarmonyPrefix]
		static void ConfirmBeehiveRemoval(
			out ResponseModel response,
			Func<int, bool> ___mExecuteCallback,
			int ___mItemId,
			int ___mFloorId,
			FurnitureRemoveCommand __instance,
			ref bool __runOriginal
		)
		{
			__runOriginal = true;
			response = null;

			if (___mItemId != ITEM_BEEHIVE)
				return; // Nothing to do here

			TextDialogUIController.Argument arg = new TextDialogUIController.Argument("Are you sure you want to remove a Beehive?", new string[] { "Yes", "No" }, delegate ()
			{
				if (___mExecuteCallback == null)
					return;

				if (!__instance.IsValid())
					return;

				if (!SingletonMonoBehaviour<UserManager>.Instance.User.Inventory.TryAddItem(___mItemId, 1, 1, -1))
					return;

				___mExecuteCallback(___mFloorId);

				UIUtils.CloseDialog();
			}, new System.Action(UIUtils.CloseDialog), null);
			SingletonMonoBehaviour<UIManager>.Instance.Open(UI.TypeEnum.TextDialog, arg);
			response = new ResponseModel(UI.TypeEnum.TextDialog, arg);

			__runOriginal = false;
		}
	}
}
