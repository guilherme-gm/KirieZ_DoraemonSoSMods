
namespace kzModUtils
{
	/**
	 * Module related to game resources
	 */
	public class ResourceModule
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
