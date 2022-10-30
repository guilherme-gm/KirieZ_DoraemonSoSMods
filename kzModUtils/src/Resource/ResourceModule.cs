
namespace kzModUtils.Resource
{
	/**
	 * Module related to game resources
	 */
	public static class ResourceUtils
	{
		/**
		 * Retrieves text message from text dictionary (TextMaster).
		 */
		public static string GetText(int textId)
		{
			return SingletonMonoBehaviour<MasterManager>.Instance.TextMaster.GetText(textId);
		}
	}
}
