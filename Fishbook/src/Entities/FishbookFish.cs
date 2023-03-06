using System.Collections.Generic;
using UnityEngine;

namespace Fishbook.Entities
{
	[System.Serializable]
	internal class FishbookFish
	{
		public static readonly Dictionary<int, string> PointNames = new Dictionary<int, string>()
		{
			// { 100, "???" },
			{ 200, "River" },
			{ 210, "Farm Pond" },
			{ 220, "River Falls" },
			{ 300, "Lake" },
			{ 310, "Swamp" },
			{ 400, "Beach" },
			{ 410, "River to Ocean" },
			{ 430, "Ocean" },
			{ 500, "Underground Cave" },
			{ 600, "Beluga Cave" },
			{ 900, "Land Fishing" },
		};

		public int PointId { get; set; }

		public int FishId { get; set; }

		public FishMasterModel Fish { get; private set; }

		public ItemMasterModel Item { get; private set; }

		public ShadowSize Shadow { get; set; }

		public Dictionary<int, FishbookCondition> Conditions { get; set; }

		public bool FoundOnce {
			get { return this.mFoundOnce; }
			set { this.mFoundOnce = value; }
		}

		[SerializeField]
		private bool mFoundOnce = false;

		public FishbookFish(int pointId, int fishId)
		{
			this.PointId = pointId;
			this.FishId = fishId;
			this.Fish = SingletonMonoBehaviour<MasterManager>.Instance.FishingMaster.GetFishData(fishId);
			this.Item = SingletonMonoBehaviour<MasterManager>.Instance.ItemMaster.GetItem(this.Fish.ItemId);
			this.Conditions = new Dictionary<int, FishbookCondition>();
			this.FoundOnce = false;
		}

		public void UpdateFish(FishingPointMasterModel pointData)
		{
			this.FoundOnce = true;
			var targetCondition = Conditions[pointData.Id];
			targetCondition.UpdateProgress();
		}

		public bool AppearsInSeason(FishPointCondtion season)
		{
			foreach (var item in this.Conditions.Values)
			{
				if ((item.CompleteFlag & season) == season)
					return true;
			}

			return false;
		}

		public string GetPointName()
		{
			return PointNames.GetValue(this.PointId, "???");
		}

		public int GetConditionsCount()
		{
			int count = 0;
			foreach (var condition in this.Conditions.Values)
				count += condition.GetConditionsCount();

			return count;
		}

		public int GetCompletedConditionsCount()
		{
			int count = 0;
			foreach (var condition in this.Conditions.Values)
				count += condition.GetCompletedConditionsCount();

			return count;
		}
	}
}
