using HarmonyLib;
using System;
using System.IO;

namespace kzModUtils.GameSave
{
	public class PatchLoadGame
	{
		private static string mDataPath;

		[HarmonyPatch(typeof(SaveDataManager), "Initialize")]
		[HarmonyPostfix]
		static void OnDataManagerInitialized(string ___mDataPath)
		{
			mDataPath = ___mDataPath;
		}

		private static bool Load(string name)
		{
			ISaveDataHandler currentHandler = null;
			try {
				foreach (var handler in GameSaveModule.Instance.SaveHandlers)
				{
					currentHandler = handler;
					if (handler == null) {
						Console.WriteLine("Load: A handler was not found. skipping it.");
						continue;
					}

					var filePath = $"{PatchLoadGame.mDataPath}/{name}_{handler.GetSuffix()}.dat";
					byte[] buffer = null;
					if (File.Exists(filePath))
						buffer = File.ReadAllBytes(filePath);

					handler.LoadGameData(buffer);
				}
			}
			catch (Exception error)
			{
				Console.WriteLine($"Failed to load handler. Suffix: \"{currentHandler?.GetSuffix()}\". Error: {error.Message}");
				return false;
			}

			return true;
		}

		[HarmonyPatch(typeof(UserManager), "IsGameClearForRecreateSaveTitle")]
		[HarmonyPostfix]
		static void OnIsGameClear_Load(
			Define.System.Save.SlotEnum slot,
			ref bool __result,
			ref bool __runOriginal
		)
		{
			string fileName = Define.System.Save.SlotToFileName(slot);
			if (string.IsNullOrEmpty(fileName))
				return;

			if (!Load(fileName)) {
				__result = false;
				__runOriginal = false;
				return;
			}

			/* our side is good, leave the rest for the game main load */
		}

		[HarmonyPatch(typeof(UserManager), "TryLoadUserModel")]
		[HarmonyPostfix]
		static void TryLoadUserModel(
			Define.System.Save.SlotEnum slot,
			UserManager __instance,
			ref bool __result,
			ref bool __runOriginal
		)
		{
			string fileName = Define.System.Save.SlotToFileName(slot);
			if (string.IsNullOrEmpty(fileName))
				return;

			if (!Load(fileName)) {
				__instance.GenerateNewUserModel();
				__result = false;
				__runOriginal = false;
				return;
			}

			/* our side is good, leave the rest for the game main load */
		}
	}
}
