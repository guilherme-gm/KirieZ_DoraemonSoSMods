using System;
using System.Collections.Generic;
using HarmonyLib;
using UnityEngine;

namespace Fishbook.Entities
{
	[System.Serializable]
	internal class FishbookBook
	{
		private static Dictionary<int, FishingPointMasterModel> FishingPointDatas;

		private static FishbookBook mInstance = null;

		public static FishbookBook Instance {
			get {
				if (mInstance == null) {
					mInstance = new FishbookBook();
				}

				return mInstance;
			}
			private set {}
		}

		[HarmonyPatch(typeof(FishingMasterCollection), "Setup")]
		[HarmonyPostfix]
		static void OnFishMasterCollectionSetup(Dictionary<int, FishingPointMasterModel> ___mFishingPointDatas)
		{
			FishingPointDatas = ___mFishingPointDatas;
		}

		[SerializeField]
		private Dictionary<int, Dictionary<int, FishbookFish>> Points
			= new Dictionary<int, Dictionary<int, FishbookFish>>();

		private FishbookBook() {}

		private ShadowSize SilhouetteToShadow(int silhouetteId)
		{
			switch (silhouetteId)
			{
				case 0:
					return ShadowSize.Small;

				case 10:
					return ShadowSize.Medium;

				case 20:
					return ShadowSize.Large;
			}

			Console.WriteLine($"Could not find shadow size for SilhoutteId '{silhouetteId}'");
			return ShadowSize.Small;
		}

		private FishPointCondtion CalculateCompleteFlag(FishingPointMasterModel model)
		{
			switch (model.Season)
			{
				case -1:
					return FishPointCondtion.Spring | FishPointCondtion.Summer | FishPointCondtion.Autumn | FishPointCondtion.Winter;

				case 0:
					return FishPointCondtion.Spring;

				case 1:
					return FishPointCondtion.Summer;

				case 2:
					return FishPointCondtion.Autumn;

				case 3:
					return FishPointCondtion.Winter;
			}

			Console.WriteLine($"Could not resolve Compelte Flag for '{model.Season}' season");
			return FishPointCondtion.None;
		}

		public void Initialize(FishbookSaveState existingState = null)
		{
			this.Points.Clear();
			foreach (var pointData in FishbookBook.FishingPointDatas)
			{
				var pointId = pointData.Value.PointId;
				var pointInfo = pointData.Value;

				if (pointInfo.FishId == -1)
					continue;

				var fish = SingletonMonoBehaviour<MasterManager>.Instance.FishingMaster.GetFishData(pointInfo.FishId);
				if (fish.SilhouetteId == -1)
					continue;

				if (!this.Points.ContainsKey(pointId))
					this.Points.Add(pointId, new Dictionary<int, FishbookFish>());

				var fishList = this.Points[pointId];
				if (!fishList.ContainsKey(pointInfo.FishId))
					fishList.Add(pointInfo.FishId, new FishbookFish(pointId, pointInfo.FishId));

				var fishInfo = fishList[pointInfo.FishId];
				fishInfo.Shadow = this.SilhouetteToShadow(fish.SilhouetteId);

				var condition = new FishbookCondition();
				condition.ConditionModel = pointInfo;
				condition.CompleteFlag = this.CalculateCompleteFlag(pointInfo);
				fishInfo.Conditions.Add(pointInfo.Id, condition);
			}

			if (existingState != null) {
				foreach (var state in existingState.State)
				{
					var parts = state.Key.Split('_');
					var pointId = Int32.Parse(parts[0]);
					var fishId = Int32.Parse(parts[1]);

					var point = this.Points[pointId][fishId];
					if (point == null) {
						Console.WriteLine($"Point not found: {pointId} {fishId}");
						continue;
					}

					point.FoundOnce = state.Value.FoundOnce;

					foreach (var cond in state.Value.Conditions) {
						var condId = cond.Key;
						if (point.Conditions.ContainsKey(condId))
							point.Conditions[condId].ProgressFlag = cond.Value;
					}
				}
			}
		}

		public FishbookFish[] GetFishesForPoint(int pointId)
		{
			if (!this.Points.TryGetValue(pointId, out var fishDictionary))
				return null;

			return fishDictionary.ValueToArray();
		}

		public void UpdateBook(FishingPointMasterModel pointData)
		{
			if (!this.Points.TryGetValue(pointData.PointId, out var pointFishes)) {
				Console.WriteLine($"Point {pointData.PointId} not found");
				return;
			}

			if (!pointFishes.TryGetValue(pointData.FishId, out var fishData)) {
				Console.WriteLine($"Fish {pointData.FishId} not found on point {pointData.PointId}");
				return;
			}

			fishData.UpdateFish(pointData);
		}

		public FishbookSaveState GetSaveState()
		{
			var state = new FishbookSaveState();

			foreach (var point in this.Points)
			{
				var pointId = point.Key;

				foreach (var fish in point.Value)
				{
					var fishId = fish.Key;
					var stateItem = new FishbookSaveState.FishbookSaveStateItem();

					state.State.Add($"{pointId}_{fishId}", stateItem);
					stateItem.FoundOnce = fish.Value.FoundOnce;

					foreach (var condition in fish.Value.Conditions)
					{
						var conditionId = condition.Key;

						stateItem.Conditions.Add(conditionId, condition.Value.ProgressFlag);
					}
				}
			}

			return state;
		}
	}
}
