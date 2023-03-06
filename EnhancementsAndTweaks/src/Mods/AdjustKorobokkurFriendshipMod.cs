using HarmonyLib;
using BepInEx.Configuration;
using UnityEngine;

namespace EnhancementsAndTweaks.Mods
{
	public class AdjustKorobokkurFriendshipMod: IMod
	{
		internal static readonly string TweakName = "Adjust Korobokkur Friendship";

		private static ConfigEntry<bool> DecreaseOnMax;

		private static ConfigEntry<int> AssistCost;

		private static bool shouldReduceFriendship(NpcModel npc)
		{
			if (AssistCost.Value == 0)
				return false;

			if (DecreaseOnMax.Value)
				return true;

			// We don't decrease on max (>=)
			return npc.LikabilityDegree < Define.Character.Npc.LikabilityDegree.MAX;
		}

		/**
		 * Replaces UserModel::TryAssistKorobokkur
		 */
		[HarmonyPatch(typeof(UserModel), "TryAssistKorobokkur")]
		[HarmonyPrefix]
		static void TryAssistKorobokkurPatch(
			int npc_id,
			ref bool __runOriginal,
			ref bool __result,
			UserModel __instance
		)
		{
			// We are simply replacing it all
			__runOriginal = false;

			NpcModel npc = __instance.GetNpc(npc_id);
			if (npc == null) {
				__result = false;
				return;
			}

			KorobokkurModel korobokkur = __instance.GetKorobokkur(npc_id);
			if (korobokkur == null){
				__result = false;
				return;
			}

			int assistDayCount = Define.Character.Korobokkur.GetAssistDayCount(npc.LikabilityDegreeRate);
			if (assistDayCount <= 0) {
				__result = false;
				return;
			}

			if (shouldReduceFriendship(npc)) {
				npc.AddLikabilityDegree(-AssistCost.Value);
			}

			korobokkur.Assist(assistDayCount);
			__result = true;
		}

		string IMod.GetName()
		{
			return TweakName;
		}

		string IMod.GetDescription()
		{
			return
				"Changes how asking help from Korobokkur affects the friendship with them.";
		}

		bool IMod.PreInstall(ConfigFile config, AssetBundle assets)
		{
			AdjustKorobokkurFriendshipMod.DecreaseOnMax = config.Bind(
				$"{TweakName} - Friendship Effect",
				"DecreaseOnMax", false,
				"Should requesting assist decrease friendship even when it is maxed? (Original: true)"
			);

			AdjustKorobokkurFriendshipMod.AssistCost = config.Bind(
				$"{TweakName} - Friendship Effect",
				"AssistCost", 10,
				"How much friendship requesting assist costs? (Original: 120)\n"
				+ "For reference:\n"
				+ "- Talk: 25 points\n"
				+ "- Liked gift: 25 points\n"
				+ "- Loved gift: 60 points\n"
				+ "- Cupid arrow: 100 points"
			);

			// Note: This mod can't change Define.Character.Korobokkur.REDUCE_ASSIST_LIKABILITY_VALUE
			// because it is readonly and C# optmizes it, so changes doesn't reflect in code.

			return true;
		}
	}
}
