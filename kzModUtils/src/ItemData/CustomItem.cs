namespace kzModUtils.ItemData
{
	public class CustomItem: ItemMasterModel
	{
		public string ModItemId { get; private set; }

		public CustomItem(string id, CItemData.SItemData data): base(data)
		{
			this.ModItemId = id;
		}
	}
}
