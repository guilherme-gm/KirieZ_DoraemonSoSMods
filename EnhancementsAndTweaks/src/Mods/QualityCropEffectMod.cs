using HarmonyLib;
using BepInEx;
using System.Collections.Generic;
using UnityEngine;
using System;
using BepInEx.Configuration;

namespace EnhancementsAndTweaks.Mods
{
	public class QualityCropEffectMod : IMod
	{
		internal static readonly string TweakName = "Quality Crop Effect";

		private static Dictionary<string, GameObject> EffectMap = new Dictionary<string, GameObject>();

		private static GameObject EffectPrefab;

		[HarmonyPatch(typeof(EventManager), "SetMapEvent")]
		[HarmonyPostfix]
		static void OnMapChange(int map_id)
		{
			EffectMap.Clear();
		}

		[HarmonyPatch(typeof(CultivatedGround), "UpdateSprite", new Type[] { typeof(bool), typeof(bool) })]
		[HarmonyPrefix]
		private static void OnGroundUpdateSprite(CultivatedGround __instance, Transform ___mParent, GroundModel ___mGroundModel)
		{
			if (___mGroundModel == null)
				return;

			string id = $"{___mGroundModel.Id}_{___mGroundModel.AreaId}";
			var eff = EffectMap.GetValue(id);

			if (eff == null && ___mGroundModel.HasCrop) {
				eff = GameObject.Instantiate(EffectPrefab, ___mParent.position, ___mParent.rotation, ___mParent);
				EffectMap.Add(id, eff);
			}

			// No crop nor effect, we have nothing to do here
			if (eff == null)
				return;

			if (!___mGroundModel.HasCrop || ___mGroundModel.Crop.Quality < Define.Item.MAX_QUALITY) {
				eff.Deactivate();
				return;
			}

			eff.Activate();
		}

		#region Mod Setup

		string IMod.GetName()
		{
			return TweakName;
		}

		string IMod.GetDescription()
		{
			return
				"Add particles on crops that are already at max quality and don't require further fertilization."
				;
		}

		bool IMod.PreInstall(ConfigFile config, AssetBundle assets)
		{
			EffectPrefab = assets.LoadAsset<GameObject>("MaxQualityEffect");

			return true;
		}
		#endregion
	}
}
