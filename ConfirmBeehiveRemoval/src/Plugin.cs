using BepInEx;
using Define;
using HarmonyLib;
using kzModUtils;
using System;

namespace ConfirmBeehiveRemoval
{
	[BepInPlugin("io.github.guilherme-gm.DoraemonSoSMods.confirmBeehiveRemoval", "Confirm Beehive Removal", PluginInfo.PLUGIN_VERSION)]
	[BepInDependency("io.github.guilherme-gm.DoraemonSoSMods.kzModUtils")]
	public class Plugin : BaseUnityPlugin
	{
		private void Awake()
		{
			// Plugin startup logic
			Logger.LogInfo($"Plugin {PluginInfo.PLUGIN_GUID} is loaded!");

			Harmony.CreateAndPatchAll(typeof(Plugin));
		}

		[HarmonyPatch(typeof(FurnitureRemoveCommand), "Execute")]
		[HarmonyPrefix]
		static bool ConfirmBeehiveRemoval(
			out ResponseModel response,
			Func<int, bool> ___mExecuteCallback,
			int ___mItemId,
			int ___mFloorId,
			FurnitureRemoveCommand __instance
		)
		{
			response = null;

			if (___mItemId != 7050000)
				return true; // Nothing to do here

			TextDialogUIController.Argument arg = new TextDialogUIController.Argument("Are you sure you want to remove a Beehive?", new string[] { "Yes", "No" }, delegate ()
			{
				if (___mExecuteCallback == null)
					return;

				if (!__instance.IsValid())
					return;

				if (!SingletonMonoBehaviour<UserManager>.Instance.User.Inventory.TryAddItem(___mItemId, 1, 1, -1))
					return;

				___mExecuteCallback(___mFloorId);

				UIModule.CloseDialog();
			}, new System.Action(UIModule.CloseDialog), null);
			SingletonMonoBehaviour<UIManager>.Instance.Open(UI.TypeEnum.TextDialog, arg);
			response = new ResponseModel(UI.TypeEnum.TextDialog, arg);

			return false;
		}
	}
}
