using HarmonyLib;
using BepInEx;
using System.Collections.Generic;

namespace AdjustToolStamina
{
	[BepInPlugin("io.github.guilherme-gm.DoraemonSoSMods.adjustToolStamina", "Adjust tool stamina", PluginInfo.PLUGIN_VERSION)]
	public class Plugin : BaseUnityPlugin
	{
		private void Awake()
		{
			// Plugin startup logic
			Logger.LogInfo($"Plugin {PluginInfo.PLUGIN_GUID} is loaded!");

			Harmony.CreateAndPatchAll(typeof(Plugin));
		}

		private static List<int> toolList = new List<int>()
		{
			// Hoes
			1000,
			1001,
			1002,
			1003,
			1004,
			1005,

			// Hammers
			1010,
			1011,
			1012,
			1013,
			1014,
			1015,

			// Watering Can
			1020,
			1021,
			1022,
			1023,
			1024,
			1025,

			// Scythe
			1030,
			1031,
			1032,
			1033,
			1034,
			1035,

			// Axe
			1040,
			1041,
			1042,
			1043,
			1044,
			1045,

			// Pick
			1050,
			1051,
			1052,
			1053,
			1054,
			1055,

			// Rods (no need as they use 0)
			/*
			1060,
			1061,
			1062,
			1063,
			1064,
			1065,
			1066, // Land fishing rod
			*/
		};

		[HarmonyPatch(typeof(UserModel), "ConsumeStaminaInUsingTool")]
		[HarmonyPrefix]
		static bool ConsumeStaminaInUsingTool(
			UserModel __instance,
			int charge_num = 0
		)
		{
			ItemModel itemInHand = __instance.Inventory.GetItemInHand();
			if (itemInHand == null || itemInHand.Tool == null)
			{
				return false;
			}

			if (itemInHand.Tool.StaminaConsumption > 0 && toolList.Contains(itemInHand.Tool.Id))
			{
				__instance.Player.Stamina.Consume(1);
				return false;
			}

			__instance.Player.Stamina.Consume(itemInHand.Tool.StaminaConsumption + charge_num);
			return false;
		}
	}
}
