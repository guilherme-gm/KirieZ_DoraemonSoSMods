using Fishbook.Entities;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Fishbook.UI
{
	internal class FishbookUIController : UIController
	{
		private FishbookWindowUIPartController windowController;

		private ButtonInfoUIPartController closeButtonInfo;

		private ButtonInfoUIPartController seasonButtonInfo;

		private bool toggleSeason;

		private List<FishbookItemUIPartController> ListItems;

		private int CurrentIndex = 0;

		public override void Initialize(UIArgument arg)
		{
			this.windowController = (SingletonMonoBehaviour<UIFactory>.Instance.Create((Define.UI.PartTypeEnum) 501, base.transform) as FishbookWindowUIPartController);
			this.windowController.Initialize();
			base.RegisterActionTargetUIPart(this.windowController, true);
			this.windowController.OnUp += this.FishbookWindow_OnUp;
			this.windowController.OnDown += this.FishbookWindow_OnDown;
			this.windowController.OnToggleSeason += this.FishbookWindow_OnToggleSeason;

			var pointInfo = arg as Argument;
			if (pointInfo == null) {
				Console.WriteLine("Missing point ID");
				return;
			}

			Console.WriteLine("PointID: " + pointInfo.PointId);

			var fishingPointIds = SingletonMonoBehaviour<MasterManager>.Instance.FishingMaster.GetFishingPointIds(pointInfo.PointId);
			if (fishingPointIds == null) {
				Console.WriteLine("Point IDs is null");
				return;
			}

			var fishList = FishbookBook.Instance?.GetFishesForPoint(pointInfo.PointId);
			if (fishList == null) {
				Console.WriteLine("FishList not found");
				return;
			}

			ListItems = new List<FishbookItemUIPartController>();
			int totalConditions = 0;
			int discoveredConditions = 0;
			for (int i = 0; i < fishList.Length; i++)
			{
				var fish = fishList[i];
				var itemEntry = GameObject.Instantiate(Fishbook.Plugin.FishInfoUI, this.windowController.ListArea.transform);
				var itemController = itemEntry.AddComponent<FishbookItemUIPartController>();
				itemController.Initialize(i, fish);
				itemController.OnHover += FishbookItem_OnHover;
				base.AttachUIParts(itemController);

				ListItems.Add(itemController);

				totalConditions += fish.GetConditionsCount();
				discoveredConditions += fish.GetCompletedConditionsCount();
			}

			if (fishList.Length > 0) {
				this.windowController.SetPointName($"{fishList[0].GetPointName()} ({pointInfo.PointId})");
			}

			this.windowController.SetPointCompletion(totalConditions > 0 ? ((float) discoveredConditions/totalConditions) * 100f : 100f);

			this.closeButtonInfo = (SingletonMonoBehaviour<UIFactory>.Instance.Create(Define.UI.PartTypeEnum.ButtonInfo, this.windowController.InputArea.transform) as ButtonInfoUIPartController);
			this.closeButtonInfo.Initialize();
			this.closeButtonInfo.SetInfo(
				SingletonMonoBehaviour<MasterManager>.Instance.TextMaster.GetText(kzModUtils.Resource.TextID.Common.TEXT_CLOSE),
				UIController.CANCEL_BUTTON
			);
			base.AttachUIParts(this.closeButtonInfo);

			this.seasonButtonInfo = (SingletonMonoBehaviour<UIFactory>.Instance.Create(Define.UI.PartTypeEnum.ButtonInfo, this.windowController.InputArea.transform) as ButtonInfoUIPartController);
			this.seasonButtonInfo.Initialize();
			this.seasonButtonInfo.SetInfo(
				"Current season",
				UIController.SUBMIT_BUTTON
			);
			base.AttachUIParts(this.seasonButtonInfo);

			this.SelectItem(0);
		}

		private void FishbookItem_OnHover(object sender, EventArgs e)
		{
			var hoverItem = sender as FishbookItemUIPartController;
			if (hoverItem == null)
				return;

			this.SelectItem(hoverItem.Index);
		}

		private void FishbookWindow_OnUp(object sender, EventArgs e)
		{
			this.OnFishbookWindowMove(-1);
		}

		private void FishbookWindow_OnDown(object sender, EventArgs e)
		{
			this.OnFishbookWindowMove(1);
		}

		private void FishbookWindow_OnToggleSeason(object sender, EventArgs e)
		{
			this.toggleSeason = !this.toggleSeason;
			if (toggleSeason) {
				FishPointCondtion expectedSeason = 0;
				switch (SingletonMonoBehaviour<UserManager>.Instance.User.Time.Season)
				{
				case Define.Time.SeasonEnum.Spring:
					expectedSeason = FishPointCondtion.Spring;
					break;

				case Define.Time.SeasonEnum.Summer:
					expectedSeason = FishPointCondtion.Summer;
					break;

				case Define.Time.SeasonEnum.Autumn:
					expectedSeason = FishPointCondtion.Autumn;
					break;

				case Define.Time.SeasonEnum.Winter:
					expectedSeason = FishPointCondtion.Winter;
					break;
				}

				foreach (var item in this.ListItems)
				{
					if (!item.Fish.AppearsInSeason(expectedSeason))
						item.gameObject.Deactivate();
				}

				if (this.ListItems[CurrentIndex]?.gameObject?.activeInHierarchy != true)
					this.OnFishbookWindowMove(1);

				this.seasonButtonInfo.SetInfo(
					"All seasons",
					UIController.SUBMIT_BUTTON
				);
			} else {
				this.seasonButtonInfo.SetInfo(
					"Current season",
					UIController.SUBMIT_BUTTON
				);

				foreach (var item in this.ListItems)
					item.gameObject.Activate();
			}
		}

		private void OnFishbookWindowMove(int delta)
		{
			var newIndex = CurrentIndex + delta;
			if (newIndex < 0)
				newIndex = ListItems.Count - 1;
			else if (newIndex >= ListItems.Count)
				newIndex = 0;

			while (this.ListItems[newIndex]?.gameObject?.activeInHierarchy != true && newIndex != CurrentIndex)
			{
				newIndex += delta;
				if (newIndex < 0)
					newIndex = ListItems.Count - 1;
				else if (newIndex >= ListItems.Count)
					newIndex = 0;
			}

			if (this.ListItems[newIndex]?.gameObject?.activeInHierarchy == true)
				this.SelectItem(newIndex);
		}

		private void SelectItem(int index)
		{
			var itemToSelect = ListItems[index];
			if (itemToSelect == null)
				return;

			var currentItem = ListItems[this.CurrentIndex];
			currentItem.Deselect();

			itemToSelect.Select();
			CurrentIndex = index;

			windowController.SnapTo(itemToSelect.gameObject.GetComponent<RectTransform>());
			windowController.SetFish(itemToSelect.Fish);
		}

		public class Argument : UIArgument
		{
			public int PointId;

			public Argument(int pointId)
			{
				this.PointId = pointId;
			}
		}
	}
}
