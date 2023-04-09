using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace kzModUtils.UI.Elements
{
	public class ToggleGroupLoader: IElementLoader
	{
		private GameObject GetOriginalButton()
		{
			var graphicsMenu = (SingletonMonoBehaviour<UIFactory>.Instance.Create(Define.UI.PartTypeEnum.GraphicOption, null) as GraphicOptionUIPartController)
				.gameObject;

			var toggleButton = graphicsMenu
				?.transform
				?.Find("Controller")
				?.Find("Window")
				?.Find("Contents_01")
				?.Find("Button_01");

			if (toggleButton == null) {
				PluginLogger.LogError("ToggleGroupLoader::GetOriginalButton: Could not find Graphic's Toggle Button.");
				return null;
			}

			var newButton = GameObject.Instantiate(toggleButton.gameObject);
			GameObject.Destroy(graphicsMenu);

			return newButton;
		}

		private void SetupPrefab(GameObject buttonHolder, GameObject button)
		{
			// Make toggle buttons group grow automatically
			buttonHolder.AddComponent<LayoutElement>()
				.flexibleWidth = 1;

			// Make toggle buttons get organized in a Grid
			var holderGrid = buttonHolder.AddComponent<GridLayoutGroup>();
			holderGrid.cellSize = new Vector2(140, 50);
			holderGrid.spacing = new Vector2(10, 10);
			holderGrid.childAlignment = TextAnchor.UpperCenter;

			// Setup toggle Group
			var toggleGroup = buttonHolder.AddComponent<ToggleGroup>();
			toggleGroup.allowSwitchOff = false;

			button.transform
				.GetChild(0)
				.GetComponent<ToggleButtonController>()
				.ToggleOff();

			button.GetComponent<Toggle>().group = toggleGroup;

			var groupController = buttonHolder.AddComponent<ToggleGroupController>();
			groupController.Setup(button);
		}

		Dictionary<ElementType, GameObject> IElementLoader.Load()
		{
			var originalButton = GetOriginalButton();
			if (originalButton == null) {
				PluginLogger.LogError("ToggleGroupLoader::Setup: Could not find original Button");
				return null;
			}

			var prefab = new GameObject("ButtonsHolder", typeof(RectTransform));

			SetupPrefab(prefab, originalButton);

			var elementDict = new Dictionary<ElementType, GameObject>();
			elementDict.Add(ElementType.ToggleGroupBuilder, prefab);
			return elementDict;
		}
	}
}
