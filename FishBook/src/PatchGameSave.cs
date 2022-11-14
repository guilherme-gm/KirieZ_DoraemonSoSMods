using System.Reflection;
using HarmonyLib;
using System;
using UnityEngine;
using System.Text;
using System.IO;

[HarmonyPatch]
public class PatchGameSave
{
	public static MethodBase TargetMethod()
	{
		var mtd = AccessTools.FirstMethod(typeof(SaveDataManager), m => m.Name.Contains("Save"));
		return mtd.MakeGenericMethod(typeof(object));
	}

	private static void SaveTemp(string path, FishbookSaveState state)
	{
		using (var f = File.Open(path, FileMode.Create, FileAccess.Write)) {
			using (var bw = new BinaryWriter(f)) {
				bw.Write(state.State.Count);
				foreach (var item in state.State)
				{
					var stateId = item.Key;
					var stateValue = item.Value;

					bw.Write(stateId);
					bw.Write(stateValue.FoundOnce);
					bw.Write(stateValue.Conditions.Count);
					foreach (var cond in stateValue.Conditions)
					{
						bw.Write(cond.Key);
						bw.Write((int) cond.Value);
					}
				}
			}
		}
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

		try {
			string path = string.Format("{0}/{1}_fishbook.dat", ___mDataPath, file_name);
			SaveTemp($"{path}.tmp", Fishbook.Instance.GetSaveState());
			if (File.Exists(path))
				File.Replace($"{path}.tmp", path, null);
			else
				File.Move($"{path}.tmp", path);

			__result = SaveDataManager.Result.Success;
		} catch (Exception error) {
			Console.WriteLine($"Failed to write fishbook. Error: {error.Message}");
			__result = SaveDataManager.Result.Failure;
		}
	}
}
