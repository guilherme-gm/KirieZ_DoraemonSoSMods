namespace kzModUtils.EventData
{
	public static class EventHelper
	{
		internal static void RegisterEvent(EventConfig config)
		{
			EventDataModule.Instance.EventConfigs.Add(config);
		}
	}
}
