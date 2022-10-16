using BepInEx;
using HarmonyLib;
using UnityEngine;
using kzModUtils;
using kzModUtils.Events;

namespace StaminaBar
{
	[BepInPlugin("io.github.guilherme-gm.DoraemonSoSMods.staminaBar", "Stamina Bar", PluginInfo.PLUGIN_VERSION)]
	[BepInDependency("io.github.guilherme-gm.DoraemonSoSMods.kzModUtils")]
	public class Plugin : BaseUnityPlugin
	{
		private static StaminaBarUIPartController StaminaUI;

		private void Awake()
		{
			// Plugin startup logic
			Logger.LogInfo($"Plugin {PluginInfo.PLUGIN_GUID} is loaded!");

			Harmony.CreateAndPatchAll(typeof(Plugin));

			UIModule.OnGameUIReady += (object sender, GameUIReadyEventArgs args) =>
			{
				var uiObj = UIModule.CreateBackgroundImage(new Vector3(-5, -87), new Vector2(135, 26)).gameObject;
				UIModule.CreateText(new Vector3(17, 0), new Vector2(117, 26), "", uiObj);

				StaminaUI = uiObj.AddComponent<StaminaBarUIPartController>();
				StaminaUI.Initialize();

				args.FarmTop.DestroyedCallback += FarmTop_DestroyedCallback;
			};
		}
		private static void FarmTop_DestroyedCallback()
		{
			GameObject.Destroy(StaminaUI.gameObject);
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
