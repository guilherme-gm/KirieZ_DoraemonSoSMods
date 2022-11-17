using System.Collections.Generic;

namespace kzModUtils
{
	internal class ModDataSavedState
	{
		/**
		 * AAABBPP
		 * A - Major
		 * B - Minor
		 * P - Patch
		 *
		 * E.g.: 100 -> 0.1.0 ; 101 -> 0.1.01
		 */
		public static readonly int CurrentVersion = 500;

		public int Version { get; set; } = CurrentVersion;

		public Dictionary<string, int> Events { get; set; } = new Dictionary<string, int>();

		public Dictionary<string, int> Items { get; set; } = new Dictionary<string, int>();
	}
}
