using HarmonyLib;
using BepInEx.Configuration;
using System.Collections.Generic;

namespace EnhancementsAndTweaks.Mods
{
	public class AdjustToolStaminaMod: IMod
	{
		internal static readonly string TweakName = "Adjust Tool Stamina";

		private static readonly HashSet<int> ToolList = new HashSet<int>()
		{
			// Hoes
			Define.Item.ID_HOE,
			Define.Item.ID_HOE_1,
			Define.Item.ID_HOE_2,
			Define.Item.ID_HOE_3,
			Define.Item.ID_HOE_4,
			Define.Item.ID_HOE_5,

			// Hammers
			Define.Item.ID_HAMMER,
			Define.Item.ID_HAMMER_1,
			Define.Item.ID_HAMMER_2,
			Define.Item.ID_HAMMER_3,
			Define.Item.ID_HAMMER_4,
			Define.Item.ID_HAMMER_5,

			// Watering Can
			Define.Item.ID_WATERING_CAN,
			Define.Item.ID_WATERING_CAN_1,
			Define.Item.ID_WATERING_CAN_2,
			Define.Item.ID_WATERING_CAN_3,
			Define.Item.ID_WATERING_CAN_4,
			Define.Item.ID_WATERING_CAN_5,

			// Scythe
			Define.Item.ID_SICKLE,
			Define.Item.ID_SICKLE_1,
			Define.Item.ID_SICKLE_2,
			Define.Item.ID_SICKLE_3,
			Define.Item.ID_SICKLE_4,
			Define.Item.ID_SICKLE_5,

			// Axe
			Define.Item.ID_AXE,
			Define.Item.ID_AXE_1,
			Define.Item.ID_AXE_2,
			Define.Item.ID_AXE_3,
			Define.Item.ID_AXE_4,
			Define.Item.ID_AXE_5,

			// Pick
			Define.Item.ID_PICKEL_0,
			Define.Item.ID_PICKEL_1,
			Define.Item.ID_PICKEL_2,
			Define.Item.ID_PICKEL_3,
			Define.Item.ID_PICKEL_4,
			Define.Item.ID_PICKEL_5,

			// Rods (no need as they use 0)
			/*
			Define.Item.ID_FISHING_ROD,
			Define.Item.ID_FISHING_ROD_1,
			Define.Item.ID_FISHING_ROD_2,
			Define.Item.ID_FISHING_ROD_3,
			Define.Item.ID_FISHING_ROD_4,
			Define.Item.ID_FISHING_ROD_5,
			Define.Item.ID_UNDERGROUND_FISHING_ROD, // Landing fishing rod
			*/
		};

		string IMod.GetName()
		{
			return TweakName;
		}

		string IMod.GetDescription()
		{
			return
				"This will make every tool in the game that consumes stamina (e.g. Watering Can, Axe, ...)\n"
				+ "always consume 1, regardless of how much they were \"charged\".";
		}

		bool IMod.PreInstall(ConfigFile config)
		{

			return true;
		}

		[HarmonyPatch(typeof(UserModel), "ConsumeStaminaInUsingTool")]
		[HarmonyPrefix]
		static void ConsumeStaminaInUsingTool(
			UserModel __instance,
			ref bool __runOriginal,
			int charge_num = 0
		)
		{
			__runOriginal = true;

			ItemModel itemInHand = __instance.Inventory.GetItemInHand();

			if (
				itemInHand?.Tool?.StaminaConsumption > 0
				&& itemInHand != null
				&& ToolList.Contains(itemInHand.Master.Id)
			) {
				__runOriginal = false;
				__instance.Player.Stamina.Consume(1);
				return;
			}
		}
	}
}
