using BepInEx;
using HarmonyLib;
using UnityEngine;
using kzModUtils;
using kzModUtils.Events;
using kzModUtils.UIElementBuilder;

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
				var uiObj = (new BackgroundImageBuilder())
					.SetCanvasAsParent()
					.SetPosition(new Vector3(-5, 87))
					.SetSize(new Vector2(135, 26))
					.Build()
					.gameObject;
				(new TextBuilder())
					.SetParent(uiObj.transform)
					.SetPosition(new Vector3(17, 0))
					.SetSize(new Vector2(117, 26))
					.SetAlignment(TextAnchor.MiddleLeft)
					.SetText("")
					.Build();

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
