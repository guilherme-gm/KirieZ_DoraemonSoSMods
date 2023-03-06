using Define;

namespace EnhancementsAndTweaks.AlternativeFurnitureRotation
{
	public class RotateFurnitureCommand : AbstractCommand
	{
		public RotateFurnitureCommand(FloorChipModel floor_chip)
		{
			this.mFloorChipModel = floor_chip;
		}

		public override bool Execute(out ResponseModel response)
		{
			response = null;
			if (!this.IsValid())
			{
				return false;
			}
			ItemModel itemInHand = SingletonMonoBehaviour<UserManager>.Instance.User.Inventory.GetItemInHand();
			if (itemInHand == null)
				return false;

			FurnitureRotationData.Rotation -= 90f;
			if (FurnitureRotationData.Rotation <= -360f)
				FurnitureRotationData.Rotation = 0f;

			return true;
		}

		public override string GetCommandName()
		{
			return "Rotate";
		}

		public override int GetButton()
		{
			return Define.Input.ActionButton.Basic_Open;
		}

		public override Scene.Farm.StateEnum GetState()
		{
			return Scene.Farm.StateEnum.Action;
		}

		public override string GetWarningMessage()
		{
			return SingletonMonoBehaviour<MasterManager>.Instance.TextMaster.GetText(Text.Narration.ID_6000_0056);
		}

		public override bool IsValid()
		{
			// Not a modifiable cell (I guess this can't happen because this is linked, but :shrug:)
			if (this.mFloorChipModel == null)
				return false;

			// Check if user is holding a furniture (it only makes sense to place a furniture)
			ItemModel itemInHand = SingletonMonoBehaviour<UserManager>.Instance.User.Inventory.GetItemInHand();
			if (itemInHand == null)
				return false;

			if (itemInHand.Master.FurnitureId < 0)
				return false;

			MapMasterModel map = SingletonMonoBehaviour<MasterManager>.Instance.MapMaster.GetMap(
				SingletonMonoBehaviour<UserManager>.Instance.User.CurrentMapId
			);

			if (map.IsOutdoorFurniture != FurnitureUtility.IsOutdoorFurniture(itemInHand.Master.FurnitureId))
				return false; // The item doesn't match the environment (e.g. farm item in house / house item in farm)

			return true;
		}

		public override bool CanExecute()
		{
			return SingletonMonoBehaviour<UserManager>.Instance.User.CurrentMap.CanPlaceFurniture();
		}

		private FloorChipModel mFloorChipModel;
	}
}
