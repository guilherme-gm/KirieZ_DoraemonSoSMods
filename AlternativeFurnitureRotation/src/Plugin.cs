using HarmonyLib;
using BepInEx;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace AlternativeFurnitureRotation
{
	[BepInPlugin("io.github.guilherme-gm.DoraemonSoSMods.alternativeFurnitureRotation", "Alternative Furniture Rotation", PluginInfo.PLUGIN_VERSION)]
	public class Plugin : BaseUnityPlugin
	{
		public static float Rotation = -90f;

		private void Awake()
		{
			// Plugin startup logic
			Logger.LogInfo($"Plugin {PluginInfo.PLUGIN_GUID} is loaded!");

			Harmony.CreateAndPatchAll(typeof(Plugin));
		}

		[HarmonyPatch(typeof(FloorChipController), "Initialize")]
		[HarmonyPrefix]
		static void FloorChipInit(
			FloorChipModel model,
			ref ICommand[] command
		)
		{
			command = command.AddToArray(new RotateFurnitureCommand(model));
		}

		private static Vector3 AdjustRotation(Vector3 baseVector, FurnitureMasterModel furniture, int playerRotationNum, int furnitureRotationNum)
		{
			switch (playerRotationNum)
			{
			case 0:
				switch (furnitureRotationNum)
				{
				case 1:
					baseVector.x -= (furniture.SizeX < 3) ? (furniture.SizeX - 1) : furniture.SizeX;
					baseVector.z +=  (int)(furniture.SizeZ / 2);
					break;
				case 2:
					baseVector.z += furniture.SizeZ - 1;
					baseVector.x += furniture.SizeX - 1;
					break;
				}
				break;

			case 1:
				switch (furnitureRotationNum)
				{
				case 1:
					baseVector.z += (furniture.SizeX - 1);
					break;

				case 2:
					baseVector.z += furniture.SizeZ - 1;
					baseVector.x += furniture.SizeX - 1;
					break;

				case 3:
					baseVector.x += (furniture.SizeX < 3) ? (furniture.SizeX - 1) : furniture.SizeX;
					break;
				}
				break;

			case 2:
				switch (furnitureRotationNum)
				{
				case 0:
					baseVector.z -= (furniture.SizeZ - 1);
					break;

				case 1:
					baseVector.x -= (furniture.SizeX < 3) ? (furniture.SizeX - 1) : furniture.SizeX;
					break;

				case 2:
					baseVector.x += furniture.SizeX - 1;
					break;

				case 3:
					baseVector.z -= (furniture.SizeX - 1);
					break;
				}
				break;

			case 3:
				switch (furnitureRotationNum)
				{
				case 0:
					baseVector.x -= (furniture.SizeX - 1);
					break;

				case 1:
					baseVector.x -= (furniture.SizeX < 3) ? (furniture.SizeX - 1) : furniture.SizeX;
					baseVector.z += (int)(furniture.SizeZ / 2);
					break;

				case 2:
					{
						var dx = (int) ((furniture.SizeX / 2) - 1);
						if (dx < 0)
							dx = 0;
						baseVector.z += furniture.SizeZ - 1;
						baseVector.x -= dx;
					}
					break;
				}
				break;
			}

			return baseVector;
		}

		[HarmonyPatch(typeof(FloorController), "SearchOccupyFloor")]
		[HarmonyPrefix]
		private static void SearchOccupyFloor(
			FloorController __instance,
			ref bool __runOriginal,
			ref int[] __result,
			FloorChipModel floor_chip_model,
			int furniture_id,
			ref int ___mRangeMarkRotNum,
			Dictionary<int, FloorChipController> ___mDicChip
		)
		{
			__runOriginal = false;

			FurnitureMasterModel furniture = SingletonMonoBehaviour<MasterManager>.Instance.FurnitureMaster.GetFurniture(furniture_id);
			List<Vector3> requiredArea = new List<Vector3>();

			for (int i = 0; i < furniture.SizeX; i++)
			{
				for (int j = 0; j < furniture.SizeZ; j++)
					requiredArea.Add(new Vector3((float)i, 0f, (float)j));
			}

			Vector3 rotationVector = SingletonMonoBehaviour<UserManager>.Instance.User.Player.Rotation * Vector3.forward;
			Vector3 playerRotation = Quaternion.AngleAxis(0, Vector3.up) * rotationVector;
			rotationVector = Quaternion.AngleAxis(Rotation, Vector3.up) * rotationVector;

			Vector3[] rotatedPoints = GroundUtility.RotateRange(requiredArea.ToArray(), rotationVector);

			var calcRotateNum = __instance.GetType().GetMethod("FurnitureRotateNum", BindingFlags.NonPublic | BindingFlags.Instance);

			___mRangeMarkRotNum = (int) calcRotateNum.Invoke(__instance, new object[] { rotationVector });
			var playerRotNum = (int) calcRotateNum.Invoke(__instance, new object[] { playerRotation });

			Vector3 baseVector = FurnitureUtility.ToFloorVec(floor_chip_model.Id);
			baseVector = AdjustRotation(baseVector, furniture, playerRotNum, ___mRangeMarkRotNum);

			List<int> cellsToFill = new List<int>();
			for (int k = 0; k < rotatedPoints.Length; k++)
			{
				int finalFloorId = FurnitureUtility.ToFloorId(rotatedPoints[k] + baseVector);
				if (___mDicChip.ContainsKey(finalFloorId))
					cellsToFill.Add(finalFloorId);
			}

			__result = cellsToFill.ToArray();
		}
	}
}
