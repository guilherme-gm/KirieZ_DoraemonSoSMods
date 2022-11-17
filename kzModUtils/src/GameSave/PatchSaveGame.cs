using System.Reflection;
using HarmonyLib;
using System;
using System.IO;

namespace kzModUtils.GameSave
{
	[HarmonyPatch]
	public class PatchSaveGame
	{
		public static MethodBase TargetMethod()
		{
			var mtd = AccessTools.FirstMethod(typeof(SaveDataManager), m => m.Name.Contains("Save"));
			return mtd.MakeGenericMethod(typeof(object));
		}

		public static void Postfix(
			object save_data,
			string file_name,
			ref SaveDataManager.Result __result,
			string ___mDataPath
		)
		{
			if (!(save_data is UserModel))
				return;

			if (__result != SaveDataManager.Result.Success)
				return;

			bool allPass = true;
			foreach (var handler in GameSaveModule.Instance.SaveHandlers)
			{
				try {
					string path = $"{___mDataPath}/{file_name}_{handler.GetSuffix()}.dat";
					byte[] buffer = handler.SaveGameData();

					File.WriteAllBytes(path, buffer);
				} catch (Exception error) {
					Console.WriteLine($"Failed to write \"{file_name}_{handler?.GetSuffix()}\". Error: {error?.Message}");
					allPass = false;
				}
			}

			__result = allPass ? SaveDataManager.Result.Success : SaveDataManager.Result.Failure;
		}
	}
}
