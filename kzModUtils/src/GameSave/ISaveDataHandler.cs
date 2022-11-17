#nullable enable

namespace kzModUtils.GameSave
{
	/**
	 * Implements a mod-specific save process.
	 *
	 * This interface is meant to be used by mods that want to persist custom data between game loads.
	 * The registered implementation (via GameSaveHelper.RegisterSaveHandler) will be used to generate a
	 * file named "<save file>_<suffix>.dat" together with the game original save file.
	 */
	public interface ISaveDataHandler
	{
		/**
		 * Suffix to be appended to the filename to differ this save part from other mods.
		 * Will make the file name "<saveGameX>_<GetSuffix()>
		 */
		public string GetSuffix();

		/**
		 * Called when saving the game.
		 *
		 * Implementation should serialize everything as it sees fit and return a byte array.
		 */
		public byte[] SaveGameData();

		/**
		 * Called when loading the game.
		 *
		 * Implementation should deserialize buffer back into its actual data.
		 * buffer may be null if the file doesn't exists (e.g. the mod was never saved in this game file)
		 */
		public void LoadGameData(byte[]? buffer);
	}
}
