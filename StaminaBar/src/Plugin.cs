using BepInEx;
using HarmonyLib;
using UnityEngine;
using kzModUtils;
using kzModUtils.Events;
using System.IO;

namespace StaminaBar
{
	[BepInPlugin("io.github.guilherme-gm.DoraemonSoSMods.staminaBar", "Stamina Bar", PluginInfo.PLUGIN_VERSION)]
	[BepInDependency("io.github.guilherme-gm.DoraemonSoSMods.kzModUtils")]
	public class Plugin : BaseUnityPlugin
	{
		private static AssetBundle Assets;

		private static StaminaBarUIPartController StaminaUI;

		private void Awake()
		{
			// Plugin startup logic
			Logger.LogInfo($"Plugin {PluginInfo.PLUGIN_GUID} is loaded!");

			Harmony.CreateAndPatchAll(typeof(Plugin));

			Assets = AssetBundle.LoadFromFile(Path.Combine(Application.dataPath, Path.Combine("AssetBundles", "StaminaUIMod")));
			var staminaUIobject = Assets.LoadAsset<GameObject>("StaminaPanel");

			UIModule.OnGameUIReady += (object sender, GameUIReadyEventArgs args) =>
			{
				var uiObj = Instantiate(staminaUIobject, args.Parent.transform);
				StaminaUI = uiObj.AddComponent<StaminaBarUIPartController>();
				StaminaUI.Initialize();

				args.FarmTop.DestroyedCallback += FarmTop_DestroyedCallback;
			};
		}
		private static void FarmTop_DestroyedCallback()
		{
			GameObject.Destroy(StaminaUI.gameObject);
		}

		public static void Teardown()
		{
			Assets.Unload(true);
		}

		[HarmonyPatch(typeof(StaminaModel), "RaiseMax")]
		[HarmonyPatch(typeof(StaminaModel), "Consume")]
		[HarmonyPatch(typeof(StaminaModel), "Recover")]
		[HarmonyPatch(typeof(StaminaModel), "RecoverFully")]
		[HarmonyPatch(typeof(StaminaModel), "EnableConsumption")]
		[HarmonyPatch(typeof(StaminaModel), "DisbleConsumption")]
		[HarmonyPatch(typeof(StaminaModel), "Empty")]
		[HarmonyPostfix]
		static void RefreshStaminaBar()
		{
			StaminaUI.UpdateText();
		}
	}
}
