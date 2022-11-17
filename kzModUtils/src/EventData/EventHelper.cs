namespace kzModUtils.EventData
{
	public static class EventHelper
	{
		public static void RegisterEvent(EventConfig config)
		{
			EventDataModule.Instance.EventConfigs.Add(config);
		}
	}
}
