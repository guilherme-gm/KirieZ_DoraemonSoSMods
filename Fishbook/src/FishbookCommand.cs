using Define;
using Fishbook.UI;

namespace Fishbook
{
	internal class FishbookCommand : AbstractCommand
	{
		public int mId;

		public FishbookCommand(FishingPointController fishing_point, int mid)
		{
			this.mFishingPointController = fishing_point;
			this.mId = mid;
		}

		public FishingPointController FishingPoint
		{
			get
			{
				return this.mFishingPointController;
			}
		}

		public override bool Execute(out ResponseModel response)
		{
			response = null;
			if (!this.IsValid()) {
				return false;
			}

			response = new ResponseModel((Define.UI.TypeEnum) 500, new FishbookUIController.Argument(this.mFishingPointController.FishingPointId));
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
			return true;
		}

		private FishingPointController mFishingPointController;
	}
}
