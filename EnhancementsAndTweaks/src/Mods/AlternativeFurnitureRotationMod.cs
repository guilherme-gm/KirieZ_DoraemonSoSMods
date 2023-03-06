using HarmonyLib;
using BepInEx.Configuration;
using System.Collections.Generic;
using UnityEngine;
using EnhancementsAndTweaks.AlternativeFurnitureRotation;
using System.Reflection;

namespace EnhancementsAndTweaks.Mods
{
	public class AlternativeFurnitureRotationMod: IMod
	{
		internal static readonly string TweakName = "Alternative Furniture Rotation";

		private static GameObject ArrowPrefab;

		private static GameObject ArrowObject;

		string IMod.GetName()
		{
			return TweakName;
		}

		string IMod.GetDescription()
		{
			return "Changes how furnitures are rotated during placement, giving more control over positioning and rotation.";
		}

		bool IMod.PreInstall(ConfigFile config, AssetBundle assets)
		{
			ArrowPrefab = assets.LoadAsset<GameObject>("DirectionArrow");
			return true;
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
			// @TODO: Is there a way to improve this?
			// System.Console.WriteLine($"> {playerRotationNum} / {furnitureRotationNum}. SizeX: {furniture.SizeX} . Size Z: {furniture.SizeZ}");
			switch (playerRotationNum)
			{
			case 0: // Looking top/left
				switch (furnitureRotationNum)
				{
				case 1:
					baseVector.x -= (furniture.SizeZ - 1);
					baseVector.z +=  (furniture.SizeX - 1);
					break;
				case 2:
					baseVector.z += (furniture.SizeZ - 1);
					baseVector.x += (furniture.SizeX - 1);
					break;
				}
				break;

			case 1: // looking top/right
				switch (furnitureRotationNum)
				{
				case 1:
					baseVector.z += (furniture.SizeX - 1);
					break;

				case 2:
					baseVector.z += (furniture.SizeZ - 1);
					baseVector.x += (furniture.SizeX - 1);
					break;

				case 3:
					baseVector.x += furniture.SizeZ - 1;
					break;
				}
				break;

			case 2: // looking bottom/right
				switch (furnitureRotationNum)
				{
				case 0:
					baseVector.z -= (furniture.SizeZ - 1);
					break;

				case 1:
					baseVector.x -= (furniture.SizeZ - 1);
					break;

				case 2:
					baseVector.x += (furniture.SizeX - 1);
					break;

				case 3:
					baseVector.z -= (furniture.SizeX - 1);
					break;
				}
				break;

			case 3: // looking bottom/left
				switch (furnitureRotationNum)
				{
				case 0:
					baseVector.x -= (furniture.SizeX - 1);
					break;

				case 1:
					baseVector.x -= (furniture.SizeZ - 1);
					baseVector.z += (furniture.SizeX - 1);
					break;

				case 2:
					{
						var dx = (int) ((furniture.SizeX / 2) - 1);
						if (dx < 0)
							dx = 0;
						baseVector.z += (furniture.SizeZ - 1);
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
			rotationVector = Quaternion.AngleAxis(FurnitureRotationData.Rotation, Vector3.up) * rotationVector;

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

		[HarmonyPatch(typeof(FloorController), "UpdateFurnitureGhost")]
		[HarmonyPostfix]
		private static void OnGhostCreated(
			GameObject ___mFurnitureGhost,
			int ___mRangeMarkRotNum
		)
		{
			if (___mFurnitureGhost == null) {
				if (ArrowObject != null) {
					GameObject.Destroy(ArrowObject);
					ArrowObject = null;
				}
				return;
			}

			if (ArrowObject == null)
				ArrowObject = GameObject.Instantiate(ArrowPrefab);

			ArrowObject.transform.position = ___mFurnitureGhost.transform.position;
			ArrowObject.transform.position += new Vector3(0, 3f, 0);

			ArrowObject.transform.rotation = Quaternion.Euler(0f, (___mRangeMarkRotNum - 1) * 90f, 0f);
		}

		[HarmonyPatch(typeof(FloorController), "DestroyFurnitureGhost")]
		[HarmonyPostfix]
		private static void OnGhostDestroyed(GameObject ___mFurnitureGhost)
		{
			if (ArrowObject != null) {
				GameObject.Destroy(ArrowObject);
				ArrowObject = null;
			}
		}


		[HarmonyPatch(typeof(FloorController), "PlaceFurniture")]
		[HarmonyPostfix]
		private static void OnFurniturePlaced(bool __result)
		{
			if (__result && ArrowObject != null) {
				GameObject.Destroy(ArrowObject);
				ArrowObject = null;
			}
		}
	}
}
