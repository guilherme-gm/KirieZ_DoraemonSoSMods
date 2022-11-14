using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class FishbookFish
{
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
}
