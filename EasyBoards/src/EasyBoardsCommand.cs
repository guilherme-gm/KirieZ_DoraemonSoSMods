using Define;

public class EasyBoardsCommand : AbstractCommand
{
	private ResponseModel createGuidesResponse()
	{
		var guideResponse = new ResponseModel(
			SingletonMonoBehaviour<MasterManager>.Instance.TextMaster.GetText(Text.Narration.ID_6000_0023), // What should I do...
			SingletonMonoBehaviour<UserManager>.Instance.User.Player.Id,
			"Guides"
		);
		guideResponse.AddNextResponse(new ResponseModel(
			UI.TypeEnum.PictureBookList,
			new PictureBookUIController.Argument(SingletonMonoBehaviour<UserManager>.Instance.User.BugPicturebookModel),
			SingletonMonoBehaviour<MasterManager>.Instance.TextMaster.GetText(Text.UI.ID_1723_0000) // Bug Guidebook
		));
		guideResponse.AddNextResponse(new ResponseModel(
			UI.TypeEnum.PictureBookList,
			new PictureBookUIController.Argument(SingletonMonoBehaviour<UserManager>.Instance.User.FishPicturebookModel),
			SingletonMonoBehaviour<MasterManager>.Instance.TextMaster.GetText(Text.UI.ID_1723_0001) // Fish Guidebook
		));
		guideResponse.AddNextResponse(new ResponseModel(
			UI.TypeEnum.PictureBookList,
			new PictureBookUIController.Argument(SingletonMonoBehaviour<UserManager>.Instance.User.FishPicturebook2Model),
			SingletonMonoBehaviour<MasterManager>.Instance.TextMaster.GetText(Text.UI.ID_1723_0011) // Fish Guidebook+
		));
		guideResponse.AddNextResponse(new ResponseModel(
			UI.TypeEnum.PictureBookList,
			new PictureBookUIController.Argument(SingletonMonoBehaviour<UserManager>.Instance.User.CropPicturebookModel),
			SingletonMonoBehaviour<MasterManager>.Instance.TextMaster.GetText(Text.UI.ID_1723_0002) // Produce Guidebook
		));
		guideResponse.AddNextResponse(new ResponseModel(
			UI.TypeEnum.PictureBookList,
			new PictureBookUIController.Argument(SingletonMonoBehaviour<UserManager>.Instance.User.CookingPicturebookModel),
			SingletonMonoBehaviour<MasterManager>.Instance.TextMaster.GetText(Text.UI.ID_1723_0003) // Recipe Book
		));
		guideResponse.AddNextResponse(
			new ResponseModel(string.Empty,
			SingletonMonoBehaviour<UserManager>.Instance.User.Player.Id,
			SingletonMonoBehaviour<MasterManager>.Instance.TextMaster.GetText(Text.Common.ID_0000_0027) // Nevermind
		));

		return guideResponse;
	}

	public override bool Execute(out ResponseModel response)
	{
		response = null;
		if (!this.IsValid())
			return false;

		response = new ResponseModel(
			SingletonMonoBehaviour<MasterManager>.Instance.TextMaster.GetText(Text.Narration.ID_6000_0023), // What should I do...
			SingletonMonoBehaviour<UserManager>.Instance.User.Player.Id
		);
		response.AddNextResponse(new ResponseModel(
			UI.TypeEnum.RegisterUI,
			null,
			"Ledger"
		));
		response.AddNextResponse(new ResponseModel(
			UI.TypeEnum.BulletinBoard,
			new BulletinBoardUIController.Argument(),
			"Bulletin Board"
		));
		response.AddNextResponse(this.createGuidesResponse());
		response.AddNextResponse(new ResponseModel(
			UI.TypeEnum.GalleryList,
			null,
			SingletonMonoBehaviour<MasterManager>.Instance.TextMaster.GetText(Text.UI.ID_1723_0004) // Gallery
		));
		response.AddNextResponse(
			new ResponseModel(string.Empty,
			SingletonMonoBehaviour<UserManager>.Instance.User.Player.Id,
			SingletonMonoBehaviour<MasterManager>.Instance.TextMaster.GetText(Text.Common.ID_0000_0027) // Nevermind
		));

		return true;
	}

	public override Scene.Farm.StateEnum GetState()
	{
		return Scene.Farm.StateEnum.CustomAction;
	}

	public override string GetCommandName()
	{
		return "View info";
	}

	public override int GetButton()
	{
		return Define.Input.ActionButton.Basic_Open;
	}

	public override bool CanExecute()
	{
		return true;
	}

	public override bool IsValid()
	{
		return true;
	}
}
