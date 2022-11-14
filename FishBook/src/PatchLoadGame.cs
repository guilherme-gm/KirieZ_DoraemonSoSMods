using System.Reflection;
using HarmonyLib;
using System;
using UnityEngine;
using System.Text;
using System.IO;


public class PatchLoadGame
{
	private static string mDataPath;

	[HarmonyPatch(typeof(SaveDataManager), "Initialize")]
	[HarmonyPostfix]
	static void OnDataManagerInitialized(string ___mDataPath)
	{
		mDataPath = ___mDataPath;
	}

	private static FishbookSaveState Load(string name)
	{
		var filePath = $"{PatchLoadGame.mDataPath}/{name}";
		Console.WriteLine(filePath);
		if (!File.Exists(filePath)) {
			Console.WriteLine("File not found");
			return null;
		}

		FishbookSaveState saveState = new FishbookSaveState();

		using (FileStream f = File.OpenRead(filePath)) {
			using (BinaryReader br = new BinaryReader(f)) {
				var stateCount = br.ReadInt32();
				for (var i = 0; i < stateCount; i++) {
					var state = new FishbookSaveState.FishbookSaveStateItem();
					saveState.State.Add(br.ReadString(), state);

					state.FoundOnce = br.ReadBoolean();

					var conditionCount = br.ReadInt32();
					for (var j = 0; j < conditionCount; j++) {
						var conditionId = br.ReadInt32();
						var conditionValue = (FishPointCondtion) br.ReadInt32();

						state.Conditions.Add(conditionId, conditionValue);
					}
				}
			}
		}

		return saveState;
	}

	[HarmonyPatch(typeof(UserManager), "IsGameClearForRecreateSaveTitle")]
	[HarmonyPostfix]
	static void OnIsGameClear_Load(
		Define.System.Save.SlotEnum slot,
		ref bool __result,
		ref bool __runOriginal
	)
	{
		Console.WriteLine("OnIsGameClear_Load");
		string fileName = Define.System.Save.SlotToFileName(slot);
		if (string.IsNullOrEmpty(fileName))
		{
			__result = false;
			__runOriginal = false;
			return;
		}

		try {
			var savedFishbook = Load($"{fileName}_fishbook.dat");
			if (savedFishbook == null) {
				Fishbook.Instance.Initialize();
			} else {
				Fishbook.Instance.Initialize(savedFishbook);
			}
		} catch (Exception error) {
			Console.WriteLine($"Failed to load Fishbook file name \"{fileName}\". Result: {error.Message}");
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
		Console.WriteLine("TryLoadUserModel");
		string fileName = Define.System.Save.SlotToFileName(slot);
		if (string.IsNullOrEmpty(fileName))
		{
			Console.WriteLine("File not found");
			__result = false;
			__runOriginal = false;
			return;
		}

		try {
			var savedFishbook = Load($"{fileName}_fishbook.dat");
			if (savedFishbook == null) {
				Fishbook.Instance.Initialize();
			} else {
				Fishbook.Instance.Initialize(savedFishbook);
			}
		} catch (Exception error) {
			__instance.GenerateNewUserModel();
			Console.WriteLine($"TryLoadUserMode: Failed to load Fishbook file name \"{fileName}\". Result: {error}");
			__result = false;
			__runOriginal = false;
			return;
		}

		/* our side is good, leave the rest for the game main load */
	}

/*
	public static void Prefix(
		SaveDataManager __instance,
		out object load_data,
		string file_name,
		ref SaveDataManager.Result __result,
		bool ___mIsInitialized,
		string ___mDataPath,
		bool __runOriginal
	)
	{
		Console.WriteLine(" Loading");
		if (__result != SaveDataManager.Result.Success)
			return;

		Console.WriteLine(" Loading 2" + file_name);
		Console.WriteLine(" Loading 2_a" + load_data);
		if (!(load_data is UserModel))
			return;

		Console.WriteLine(" Loading 3");

		Fishbook.Instance.Initialize();
		string path = string.Format("{0}/{1}_fishbook.json", ___mDataPath, file_name);
		if (!File.Exists(path))
		{
			Console.WriteLine("New fishbook");
			// Fishbook doesn't exists yet, maybe it was lost / first time using the mod. That's fine.
			return;
		}

		SaveDataManager.Result result = SaveDataManager.Result.Failure;
		using (FileStream fileStream = new FileStream(path, FileMode.Open, FileAccess.Read))
		{
			using (BinaryReader binaryReader = new BinaryReader(fileStream))
			{
				byte[] bytes = binaryReader.ReadBytes((int)fileStream.Length);
				string json = Encoding.UTF8.GetString(bytes);
				binaryReader.Close();
				fileStream.Close();
				try {
					JsonUtility.FromJsonOverwrite(json, Fishbook.Instance);
				}
				catch
				{
					Console.WriteLine("Fishbook Load failed.");
					__result = SaveDataManager.Result.Failure;
					return;
				}
				result = SaveDataManager.Result.Success;
			}
		}

		Console.WriteLine("Fishbook Loaded.");

		__result = result;
	}
	*/
}
