using HarmonyLib;
using UnityEngine;
using kzModUtils.UI;
using kzModUtils.UI.Events;
using kzModUtils.UI.Elements;
using EnhancementsAndTweaks.StaminaBar;
using BepInEx.Configuration;

namespace EnhancementsAndTweaks.Mods
{
	public class StaminaBarMod : IMod
	{
		internal static readonly string TweakName = "Stamina Bar";

		private static StaminaBarUIPartController StaminaUI;

		string IMod.GetName()
		{
			return TweakName;
		}

		string IMod.GetDescription()
		{
			return
				"Adds a box beside with item price beside item description in inventory and storages UI."
				;
		}

		bool IMod.PreInstall(ConfigFile config, AssetBundle assets)
		{
			UIUtils.OnGameUIReady += (object sender, GameUIReadyEventArgs args) =>
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

			return true;
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
