namespace kzModUtils.ItemData
{
	public class ItemHelper
	{
		public delegate void OnItemRegistered(CustomItem item);

		private static ItemHelper mInstance;

		public static ItemHelper Instance
		{
			get
			{
				if (mInstance == null)
					mInstance = new ItemHelper();

				return mInstance;
			}
			private set { }
		}

		public void RegisterItem(CustomItemConfig itemConfig, OnItemRegistered callback)
		{
			itemConfig.Callback = callback;
			TextData.TextHelper.RegisterText(itemConfig.Name, (int id) => { itemConfig.NameId = id; });
			TextData.TextHelper.RegisterText(itemConfig.Description, (int id) => { itemConfig.DescriptionId = id; });
			ItemModule.Instance.CustomItemConfigs.Add(itemConfig);
		}
	}
}
