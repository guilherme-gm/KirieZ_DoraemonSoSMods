using Define;
using Fishbook.UI;

namespace Fishbook
{
	internal class FishbookCommand : AbstractCommand
	{
		public int mId;

		private FishingPointController mFishingPointController;

		private int mPointId;

		public FishbookCommand(FishingPointController fishing_point, int mid)
		{
			this.mFishingPointController = fishing_point;
			this.mId = mid;
		}

		public FishbookCommand(int pointId, int mid)
		{
			this.mPointId = pointId;
			this.mId = mid;
		}

		public FishingPointController FishingPoint
		{
			get
			{
				return this.mFishingPointController;
			}
		}

		public int PointId
		{
			get
			{
				if (this.mFishingPointController != null) {
					return this.mFishingPointController.FishingPointId;
				}

				return this.mPointId;
			}
		}

		public override bool Execute(out ResponseModel response)
		{
			response = null;
			if (!this.IsValid()) {
				return false;
			}

			response = new ResponseModel((Define.UI.TypeEnum) 500, new FishbookUIController.Argument(this.PointId));
			return true;
		}

		public override string GetCommandName()
		{
			return "View fish book";
		}

		public override int GetButton()
		{
			return Define.Input.ActionButton.Basic_Open;
		}

		public override ActionModel[] GetActions()
		{
			return new ActionModel[]
			{
				new ActionModel(Define.Action.Id.None)
			};
		}

		public override Scene.Farm.StateEnum GetState()
		{
			return Scene.Farm.StateEnum.Talk;
		}

		public override bool CanExecute()
		{
			return true;
		}

		public override string GetWarningMessage()
		{
			return "";
		}

		public override bool IsValid()
		{
			var count = SingletonMonoBehaviour<UserManager>.Instance.User.GetItemCountFromInventory(Plugin.FishbookItem.Id);
			if (count <= 0)
				return false;

			if (this.PointId == 900)
				return SingletonMonoBehaviour<UserManager>.Instance.User.Inventory.GetItemIdInHand() == Item.ID_UNDERGROUND_FISHING_ROD;

			return true;
		}
	}
}
