using Fishbook.Entities;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Fishbook.UI
{
	internal class FishbookUIController : UIController
	{
		private FishbookWindowUIPartController windowController;

		private ButtonInfoUIPartController mButtonInfo;

		private List<FishbookItemUIPartController> ListItems;

		private int CurrentIndex = 0;

		public override void Initialize(UIArgument arg)
		{
			this.windowController = (SingletonMonoBehaviour<UIFactory>.Instance.Create((Define.UI.PartTypeEnum) 501, base.transform) as FishbookWindowUIPartController);
			this.windowController.Initialize();
			base.RegisterActionTargetUIPart(this.windowController, true);
			this.windowController.OnUp += this.FishbookWindow_OnUp;
			this.windowController.OnDown += this.FishbookWindow_OnDown;

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
			for (int i = 0; i < fishList.Length; i++)
			{
				var fish = fishList[i];
				var itemEntry = GameObject.Instantiate(Fishbook.Plugin.FishInfoUI, this.windowController.ListArea.transform);
				var itemController = itemEntry.AddComponent<FishbookItemUIPartController>();
				itemController.Initialize(i, fish);
				itemController.OnHover += FishbookItem_OnHover;
				base.AttachUIParts(itemController);

				ListItems.Add(itemController);
			}

			this.mButtonInfo = (SingletonMonoBehaviour<UIFactory>.Instance.Create(Define.UI.PartTypeEnum.ButtonInfo, this.windowController.InputArea.transform) as ButtonInfoUIPartController);
			string text = SingletonMonoBehaviour<MasterManager>.Instance.TextMaster.GetText(kzModUtils.Resource.TextID.Common.TEXT_CLOSE);
			int cancel_BUTTON = UIController.CANCEL_BUTTON;
			this.mButtonInfo.Initialize();
			this.mButtonInfo.SetInfo(text, cancel_BUTTON);
			base.AttachUIParts(this.mButtonInfo);

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

		private void OnFishbookWindowMove(int delta)
		{
			var newIndex = CurrentIndex + delta;
			if (newIndex < 0)
				newIndex = ListItems.Count - 1;
			else if (newIndex >= ListItems.Count)
				newIndex = 0;

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
