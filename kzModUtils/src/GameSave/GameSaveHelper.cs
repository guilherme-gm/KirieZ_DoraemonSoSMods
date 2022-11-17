namespace kzModUtils.GameSave
{
	public static class GameSaveHelper
	{
		public static void RegisterSaveHandler(ISaveDataHandler handler)
		{
			GameSaveModule.Instance.SaveHandlers.Add(handler);
		}
	}
}
