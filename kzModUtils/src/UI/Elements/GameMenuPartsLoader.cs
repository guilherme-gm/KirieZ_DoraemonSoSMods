using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace kzModUtils.UI.Elements
{
	public class GameMenuPartsLoader: IElementLoader
	{
		#region Scrollable Area
		private void SetupScrollableArea(GameObject prefab)
		{
			// Typo in "Scroll_Contetns" is intentional -- this is how it is in-game
			var tableContents = prefab.transform.Find("Scroll_Contetns");
			for (int i = tableContents.childCount - 1; i >= 0; i--)
				GameObject.Destroy(tableContents.GetChild(i).gameObject);

			// Add HorizontalLayoutGroup to the overall area so we are sure whatever content there is,
			// ScrollBar will always come at the end
			var tableLayout = prefab.AddComponent<HorizontalLayoutGroup>();
			tableLayout.childForceExpandWidth = false;

			// Make the content area grow automatically so we can fit every child element automatically
			tableContents.gameObject.AddComponent<ContentSizeFitter>()
				.verticalFit = ContentSizeFitter.FitMode.PreferredSize;

			// Add Vertical Layout Group to content area so every new list item gets added to the end
			// in the proper position
			var layoutGroup = tableContents.gameObject.AddComponent<VerticalLayoutGroup>();
			layoutGroup.childForceExpandHeight = false;
			layoutGroup.childControlHeight = false;

			// Force Scrollbar to always be 12px, this way HorizontalLayoutGroup can do its magic
			var scroll = prefab.transform.Find("Scrollbar");
			var scrollLayoutElement = scroll.gameObject.AddComponent<LayoutElement>();
			scrollLayoutElement.preferredWidth = 12;
			scrollLayoutElement.minWidth = 12;
		}

		private GameObject GetScrollableAreaPrefab(GameObject gameMenu)
		{
			var tableView = gameMenu
				?.transform
				?.Find("Controller")
				?.Find("Body")
				?.Find("Contents")
				?.Find("UI_Menu_VillagerList")
				?.Find("Controller")
				?.Find("Table_View");

			if (tableView == null) {
				PluginLogger.LogError("Could not find Villagers list.");
				return null;
			}

			var newTable = GameObject.Instantiate(tableView.gameObject);
			this.SetupScrollableArea(newTable);

			return newTable;
		}
		#endregion

		#region Small Menu Separator
		private void SetupSmallMenuSeparator(GameObject separator)
		{
			var controller = separator.AddComponent<SmallMenuSeparatorController>();
			var textTitle = separator.transform
				.Find("Text_Title").GetComponent<Text>();
			var titleSprite = textTitle.transform
				.Find("Sprite_Icon").GetComponent<Image>();
			controller.Setup(textTitle, titleSprite);

			controller.SetText("");

			titleSprite.gameObject.SetActive(false);
		}

		private GameObject GetSmallMenuSeparatorPrefab(GameObject gameMenu)
		{
			var separator = gameMenu
				?.transform
				?.Find("Controller")
				?.Find("Body")
				?.Find("Contents")
				?.Find("UI_Menu_System")
				?.Find("Controller")
				?.Find("Contents_01")
				?.Find("Title");

			if (separator == null) {
				PluginLogger.LogError("Could not find Settings list.");
				return null;
			}

			var newSeparator = GameObject.Instantiate(separator.gameObject);
			this.SetupSmallMenuSeparator(newSeparator);

			return newSeparator;
		}
		#endregion

		Dictionary<ElementType, GameObject> IElementLoader.Load()
		{
			var gameMenu = (SingletonMonoBehaviour<UIFactory>.Instance.Create(Define.UI.PartTypeEnum.Menu, null) as MenuUIPartController)
				.gameObject;

			Dictionary<ElementType, GameObject> dict = new Dictionary<ElementType, GameObject>();
			dict.Add(ElementType.ScrollableArea, GetScrollableAreaPrefab(gameMenu));
			dict.Add(ElementType.SmallMenuSeparator, GetSmallMenuSeparatorPrefab(gameMenu));

			GameObject.Destroy(gameMenu);
			return dict;
		}
	}
}
