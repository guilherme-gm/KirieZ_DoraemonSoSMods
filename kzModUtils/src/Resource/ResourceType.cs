namespace kzModUtils.Resource
{
	/**
	 * What kind of asset this is.
	 * Determines which folder to look for
	 */
	public enum ResourceType
	{
		Data = 1,
		/**
		 * Streaming asset data, most of the *.jp files are in there.
		 */
		StreamingAsset,
		/**
		 * Plugin defined data.
		 * Note: plugin name not included.
		 */
		Plugin,
	}
}
