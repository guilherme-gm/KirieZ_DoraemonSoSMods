using BepInEx;
using System.IO;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using HarmonyLib;
using System;
using System.Reflection;
using System.Text.RegularExpressions;

namespace DumpMaster
{
	[BepInPlugin("io.github.guilherme-gm.DoraemonSoSMods.dumpMaster", "Dump Master", PluginInfo.PLUGIN_VERSION)]
	public class Plugin : BaseUnityPlugin
	{
		private static readonly BindingFlags fieldReflectionFlag = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static;

		private static Dictionary<string, List<string>> Dump = new Dictionary<string, List<string>>();

		private void Awake()
		{
			// Plugin startup logic
			Logger.LogInfo($"Plugin {PluginInfo.PLUGIN_GUID} is loaded!");

			Harmony.CreateAndPatchAll(typeof(Plugin));
		}

		static void DumpDataDictionary(string outputKey, IDictionary data)
		{
			var keys = data.Keys;
			if (keys == null)
			{
				Console.WriteLine("Keys NOT found");
				return;
			}

			if (keys.Count == 0)
				return;

			if (!Dump.ContainsKey(outputKey))
			{
				Dump.Add(outputKey, new List<string>());
			}

			var output = Dump[outputKey];
			bool isFirst = true;
			FieldInfo[] fields = null;
			foreach (var key in keys)
			{
				var entry = data[key];

				if (isFirst)
				{
					fields = entry.GetType().GetFields(fieldReflectionFlag);

					output.Add($"Data: ${outputKey}");
					output.Add("");
					output.Add("--------- Data Structure/Types ---------");
					output.Add("Field name,Field type");

					string fileHeader = "#";
					foreach (var field in fields)
					{
						fileHeader += $",{field.Name}";
						output.Add($"{field.Name},${field.FieldType}");
					}

					output.Add("");
					output.Add("--------- Data ---------");
					output.Add(fileHeader);

					isFirst = false;
				}

				string line = $"{key}";
				foreach (var field in fields)
				{
					line += $",{field.GetValue(entry)}";
				}
				output.Add(line);
			}
		}

		static void DumpMasterCollection(string name, Type collection)
		{
			Console.WriteLine($"Master Collection: {name}");

			foreach (var collectionField in collection.GetFields(fieldReflectionFlag))
			{
				Console.Write($"> Field: {collectionField.Name}");
				IDictionary dict = collectionField.GetValue(null) as IDictionary;

				if (dict == null)
				{
					Console.WriteLine("Value is not dicrionary");
					continue;
				}

				DumpDataDictionary($"{name}__${collectionField.Name}", dict);
			}
			return;

		}

		/*

		[HarmonyPatch(typeof(MasterManager), "SetupMasters")]
		[HarmonyPostfix]
		static void OnMastersLoaded(MasterManager __instance)
        {
			foreach (var managerField in __instance.GetType().GetFields(fieldReflectionFlag))
            {
				IMasterCollection collection = managerField.GetValue(__instance) as IMasterCollection;
				if (collection == null)
                {
					Console.WriteLine("collection can't cast." + managerField.Name);
                    continue;
                }

				DumpMasterCollection(managerField.Name, collection);
			}

			Directory.CreateDirectory("Dump");
			Regex reg = new Regex("[^A-Za-z_0-9]");
			foreach (var k in Dump)
			{
				var fileName = reg.Replace(k.Key, "-");
				File.WriteAllLines(Path.Combine("Dump", $"{fileName}.csv"), k.Value.ToArray());
            }

			Console.WriteLine("Done.");
		}
		*/

		[HarmonyPatch(typeof(CAchievementData), "Setup")]
		[HarmonyPatch(typeof(CAnimalData), "Setup")]
		[HarmonyPatch(typeof(CAreaData), "Setup")]
		[HarmonyPatch(typeof(CBgmData), "Setup")]
		[HarmonyPatch(typeof(CBugData), "Setup")]
		[HarmonyPatch(typeof(CBugPointData), "Setup")]
		[HarmonyPatch(typeof(CBugRaceData), "Setup")]
		[HarmonyPatch(typeof(CBuildingData), "Setup")]
		[HarmonyPatch(typeof(CCarnivalData), "Setup")]
		[HarmonyPatch(typeof(CCharacterData), "Setup")]
		[HarmonyPatch(typeof(CCookingRecipeData), "Setup")]
		[HarmonyPatch(typeof(CCookingToolData), "Setup")]
		[HarmonyPatch(typeof(CDigPointData), "Setup")]
		[HarmonyPatch(typeof(CEndRoll01Data), "Setup")]
		[HarmonyPatch(typeof(CEndRoll02Data), "Setup")]
		[HarmonyPatch(typeof(CEventData), "Setup")]
		[HarmonyPatch(typeof(CRewardEventData), "Setup")]
		[HarmonyPatch(typeof(CEventGroupData), "Setup")]
		[HarmonyPatch(typeof(CCropData), "Setup")]
		[HarmonyPatch(typeof(CWeatherData), "Setup")]
		[HarmonyPatch(typeof(CObstacleData), "Setup")]
		[HarmonyPatch(typeof(CGatheringData), "Setup")]
		[HarmonyPatch(typeof(CGatheringGroupData), "Setup")]
		[HarmonyPatch(typeof(CFishData), "Setup")]
		[HarmonyPatch(typeof(CFurnitureData), "Setup")]
		[HarmonyPatch(typeof(CFurnitureTypeData), "Setup")]
		[HarmonyPatch(typeof(CFurnitureCategoryData), "Setup")]
		[HarmonyPatch(typeof(CItemData), "Setup")]
		[HarmonyPatch(typeof(CToolData), "Setup")]
		[HarmonyPatch(typeof(CExchangeItemData), "Setup")]
		[HarmonyPatch(typeof(CDolphinItemData), "Setup")]
		[HarmonyPatch(typeof(CMapData), "Setup")]
		[HarmonyPatch(typeof(CRestrictionMapData), "Setup")]
		[HarmonyPatch(typeof(CMineData), "Setup")]
		[HarmonyPatch(typeof(CMineFloorData), "Setup")]
		[HarmonyPatch(typeof(CMineItemData), "Setup")]
		[HarmonyPatch(typeof(CMiniGameTutorialData), "Setup")]
		[HarmonyPatch(typeof(CQuestData), "Setup")]
		[HarmonyPatch(typeof(CQuestItemData), "Setup")]
		[HarmonyPatch(typeof(CChickenShopData), "Setup")]
		[HarmonyPatch(typeof(CCattleAndSheepShopData), "Setup")]
		[HarmonyPatch(typeof(CUpgradeToolShopData), "Setup")]
		[HarmonyPatch(typeof(CMineralShopData), "Setup")]
		[HarmonyPatch(typeof(CMakerShopData), "Setup")]
		[HarmonyPatch(typeof(CBuildingShopData), "Setup")]
		[HarmonyPatch(typeof(CFurnitureShopData), "Setup")]
		[HarmonyPatch(typeof(CMaterialShopData), "Setup")]
		[HarmonyPatch(typeof(CRestaurantShopData), "Setup")]
		[HarmonyPatch(typeof(CKorobokkurShopData), "Setup")]
		[HarmonyPatch(typeof(CVarietyShopData), "Setup")]
		[HarmonyPatch(typeof(CFishingTackleShopData), "Setup")]
		[HarmonyPatch(typeof(CHospitalShopData), "Setup")]
		[HarmonyPatch(typeof(CCharacterData), "Setup")]
		[HarmonyPatch(typeof(CTodaysWordData), "Setup")]
		[HarmonyPostfix]
		static void OnDataSetup(MethodBase __originalMethod)
		{
			/*
			foreach (var managerField in __instance.GetType().GetFields(fieldReflectionFlag))
			{
				IMasterCollection collection = managerField.GetValue(__instance) as IMasterCollection;
				if (collection == null)
				{
					Console.WriteLine("collection can't cast." + managerField.Name);
					continue;
				}

				DumpMasterCollection(managerField.Name, collection);
			}
			*/
			DumpMasterCollection("", __originalMethod.DeclaringType);
			Directory.CreateDirectory("Dump");
			Regex reg = new Regex("[^A-Za-z_0-9]");
			foreach (var k in Dump)
			{
				var fileName = reg.Replace(k.Key, "-");
				File.WriteAllLines(Path.Combine("Dump", $"{fileName}.csv"), k.Value.ToArray());
			}

			Console.WriteLine("Done.");
		}
	}
}
