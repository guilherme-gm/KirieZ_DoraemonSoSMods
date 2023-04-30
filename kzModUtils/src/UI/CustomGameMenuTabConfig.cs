using kzModUtils.Resource;

namespace kzModUtils.UI
{
	public class CustomGameMenuTabConfig
	{
		public enum GameMenuTabs {
			MenuStart = -1,
			MiniMap = 0,
			Status = 1,
			CowSheep = 2,
			Hen = 3,
			Villager = 4,
			Helper = 5,
			Quest = 6,
			ImportantItems = 7,
			Calendar = 8,
			Stamp = 9,
			System = 10,
		}

		/**
		 * Internal tab identifier (not currently used)
		 * It is recommended that this gets prefixed by a reference to your mod name.
		 *
		 * @remarks This may be used in the future if we want to implement menus that
		 *          are aware about other menus (e.g.: Position this after Mod X menu)
		 *          But this is NOT implemented right now and the field is simply ignored.
		 */
		public string Id;

		/**
		 * Tab display name
		 */
		public string Name;

		/**
		 * Tab display sprite (The text in the tab comes from Name, it is not batched into the sprite)
		 */
		public SpriteConfig Sprite;

		/**
		 * UIPartController that is in the root of the menu content prefab.
		 * This will be registered into the menu system AND the object linked to it will be
		 * added to the menu. So it MUST be in the root of your menu page
		 */
		public MenuContentUIPartController MenuPrefabController;

		/**
		 * After which original tab the new one should be
		 */
		public GameMenuTabs After = GameMenuTabs.System;
	}
}
