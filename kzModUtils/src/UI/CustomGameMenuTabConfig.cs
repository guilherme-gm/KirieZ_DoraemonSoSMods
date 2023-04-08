using kzModUtils.Resource;

namespace kzModUtils.UI
{
	public class CustomGameMenuTabConfig
	{
		/**
		 * Tab display name
		 */
		public string Name;

		/**
		 * Tab display sprite (The text in the tab comes from Name, it is not batched into the sprite)
		 */
		public CustomSpriteConfig Sprite;

		/**
		 * UIPartController that is in the root of the menu content prefab.
		 * This will be registered into the menu system AND the object linked to it will be
		 * added to the menu. So it MUST be in the root of your menu page
		 */
		public MenuContentUIPartController MenuPrefabController;
	}
}
