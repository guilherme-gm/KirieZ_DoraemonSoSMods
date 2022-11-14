using Define;
using kzModUtils.UI.Elements;

public class FishbookCommand : AbstractCommand
{
	public int mId;

	// Token: 0x060013BE RID: 5054 RVA: 0x0004C48F File Offset: 0x0004A88F
	public FishbookCommand(FishingPointController fishing_point, int mid)
	{
		this.mFishingPointController = fishing_point;
		this.mId = mid;
	}

	// Token: 0x17000480 RID: 1152
	// (get) Token: 0x060013BF RID: 5055 RVA: 0x0004C49E File Offset: 0x0004A89E
	public FishingPointController FishingPoint
	{
		get
		{
			return this.mFishingPointController;
		}
	}

	// Token: 0x060013C0 RID: 5056 RVA: 0x0004C4A6 File Offset: 0x0004A8A6
	public override bool Execute(out ResponseModel response)
	{
		response = null;
		if (!this.IsValid()) {
			return false;
		}

/*
		(new MessageBoxBuilder())
			.SetCanvasAsParent()
			.SetPosition(new UnityEngine.Vector3(100, 300))
			.SetSize(new UnityEngine.Vector2(300, 300))
			.SetStyle(MessageBoxStyles.Blue)
			.SetTitle("Fish book")
			.SetText("Here we go")
			.Build();
*/

		response = new ResponseModel((UI.TypeEnum) 500, new FishbookUIController.Argument(this.mFishingPointController.FishingPointId));
		return true;
	}

	// Token: 0x060013C1 RID: 5057 RVA: 0x0004C4B9 File Offset: 0x0004A8B9
	public override string GetCommandName()
	{
		return "View fish book";
	}

	// Token: 0x060013C2 RID: 5058 RVA: 0x0004C4CF File Offset: 0x0004A8CF
	public override int GetButton()
	{
		return Define.Input.ActionButton.Basic_Open;
	}

	// Token: 0x060013C3 RID: 5059 RVA: 0x0004C4D3 File Offset: 0x0004A8D3
	public override ActionModel[] GetActions()
	{
		return new ActionModel[]
		{
			new ActionModel(Define.Action.Id.None)
		};
	}

	// Token: 0x060013C4 RID: 5060 RVA: 0x0004C4F8 File Offset: 0x0004A8F8
	public override Scene.Farm.StateEnum GetState()
	{
		return Scene.Farm.StateEnum.Talk;
	}

	// Token: 0x060013C5 RID: 5061 RVA: 0x0004C4FC File Offset: 0x0004A8FC
	public override bool CanExecute()
	{
		return true;
	}

	// Token: 0x060013C6 RID: 5062 RVA: 0x0004C515 File Offset: 0x0004A915
	public override string GetWarningMessage()
	{
		return "";
	}

	// Token: 0x060013C7 RID: 5063 RVA: 0x0004C52B File Offset: 0x0004A92B
	public override bool IsValid()
	{
		return true;
	}

	// Token: 0x04000BFD RID: 3069
	private FishingPointController mFishingPointController;
}
