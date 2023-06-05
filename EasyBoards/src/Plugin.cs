using BepInEx;
using HarmonyLib;
using System.Collections.Generic;
using System.Reflection;

namespace EasyBoards
{
	[BepInPlugin("io.github.guilherme-gm.DoraemonSoSMods.EasyBoards", "Easy Boards", PluginInfo.PLUGIN_VERSION)]
	public class Plugin : BaseUnityPlugin
	{
		private static MethodInfo FloorController_RemoveFurniture = typeof(FloorController)
			.GetMethod("RemoveFurniture", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

		private static MethodInfo FloorController_PlayGimmickFurniture = typeof(FloorController)
			.GetMethod("PlayGimmickFurniture", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

		private void Awake()
		{
			PluginLogger.Logger = this.Logger;
			PluginLogger.LogInfo($"Plugin {PluginInfo.PLUGIN_GUID} is loaded!");

			Harmony.CreateAndPatchAll(typeof(Plugin));
		}

		[HarmonyPatch(typeof(FloorController), "SetFurnitureCommands")]
		[HarmonyPrefix]
		public static void After_FloorController_SetFurnitureCommands(
			FloorController __instance,
			FurnitureController furniture_controller, FurnitureModel furniture_model, BeehiveController beehive_controller,
			Dictionary<int, FloorChipController> ___mDicChip,
			ref bool __runOriginal
		)
		{
			if (furniture_model?.Master == null)
				return;

			FloorChipController floorChipController = ___mDicChip[furniture_controller.FloorChipId];
			FurnitureMasterModel furnitureMasterModel = furniture_model.Master;

			if (furnitureMasterModel.Type != (int) CFurnitureTypeData.FURNITURE_TYPE.CHEST)
				return;

			var command = new ICommand[]
			{
				new FurnitureRemoveCommand(
					furnitureMasterModel.ItemId,
					furniture_controller.FloorChipId,
					bool (int a) => { return (bool) FloorController_RemoveFurniture.Invoke(__instance, new object[] { a }); },
					floorChipController.ChipModel
				),
				new FurnitureGimmickCommand(
					furniture_model,
					furniture_controller.FloorChipId,
					bool (int a) => { return (bool) FloorController_PlayGimmickFurniture.Invoke(__instance, new object[] { a }); }
				),
				new ChestCommand(),
				new EasyBoardsCommand(),
			};
			furniture_controller.SetCommand(command);

			__runOriginal = false;
		}
	}
}
