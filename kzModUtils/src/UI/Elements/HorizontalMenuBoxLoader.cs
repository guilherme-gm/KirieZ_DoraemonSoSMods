using UnityEngine;
using UnityEngine.UI;

namespace kzModUtils.UI.Elements
{
	public class HorizontalMenuBoxLoader: IElementLoader
	{
		private GameObject GetOriginalBox()
		{
			var graphicsMenu = (SingletonMonoBehaviour<UIFactory>.Instance.Create(Define.UI.PartTypeEnum.GraphicOption, null) as GraphicOptionUIPartController)
				.gameObject;

			var graphicsToggleBox1 = graphicsMenu
				?.transform
				?.Find("Controller")
				?.Find("Window")
				?.Find("Contents_01");

			if (graphicsToggleBox1 == null) {
				PluginLogger.LogError("HorizontalMenuBoxBuilder::GetOriginalBox: Could not find Graphic's Toggle Box 1.");
				return null;
			}

			for (int i = graphicsToggleBox1.childCount - 1; i >= 0; i--) {
				var child = graphicsToggleBox1.GetChild(i);
				if (child.name.StartsWith("Button_")) {
					child.SetParent(null);
					GameObject.Destroy(child.gameObject);
				}
			}

			var newBox = GameObject.Instantiate(graphicsToggleBox1.gameObject);
			GameObject.Destroy(graphicsMenu);

			return newBox;
		}

		private void SetupBackground(GameObject prefab)
		{
			var bgObject = prefab.transform.Find("Sprite_Base").gameObject;

			// Ensure the BG doesn't get affected by Horizontal Layout
			var bgLayoutElement = bgObject.AddComponent<LayoutElement>();
			bgLayoutElement.ignoreLayout = true;

			// Make the BG cover the entire parent object and scale with it
			var bgRectTransform = bgObject.GetComponent<RectTransform>();
			bgRectTransform.anchorMin = new Vector2(0, 0);
			bgRectTransform.anchorMax = new Vector2(1, 1);
			bgRectTransform.sizeDelta = new Vector2(0, 0);
		}

		private GameObject SetupTitle(GameObject prefab)
		{
			var titleObject = prefab.transform.Find("Text_Title").gameObject;

			// Gives it a preferred width so several elements may have the same size
			var layoutElement = titleObject.AddComponent<LayoutElement>();
			layoutElement.preferredWidth = 250;

			titleObject.SetActive(false);

			return titleObject;
		}

		private void SetupPrefab(GameObject prefab)
		{
			GameObject.Destroy(prefab.GetComponent<GraphicOptionContentsController>());
			GameObject.Destroy(prefab.GetComponent<ToggleGroup>());

			// Allow component to grow vertically
			var contentFitter = prefab.AddComponent<ContentSizeFitter>();
			contentFitter.verticalFit = ContentSizeFitter.FitMode.PreferredSize;

			// Setup Horizontal Layout so elements get positioned automatically
			var mainLayoutGroup = prefab.AddComponent<HorizontalLayoutGroup>();
			mainLayoutGroup.padding = new RectOffset(10, 10, 10, 10);
			mainLayoutGroup.spacing = 10;
			mainLayoutGroup.childAlignment = TextAnchor.MiddleLeft;
			mainLayoutGroup.childForceExpandWidth = false;
			mainLayoutGroup.childForceExpandHeight = true;
			mainLayoutGroup.childControlHeight = true;
			mainLayoutGroup.childControlWidth = true;

			SetupBackground(prefab);
			var titleObject = SetupTitle(prefab);

			prefab.AddComponent<HorizontalMenuBoxController>()
				.Initialize(titleObject.GetComponent<Text>());
		}

		GameObject IElementLoader.Load()
		{
			var menuBox = GetOriginalBox();
			if (menuBox == null) {
				PluginLogger.LogError("HorizontalMenuBoxBuilder::Setup: Could not find original Box");
				return null;
			}

			SetupPrefab(menuBox);
			return menuBox;
		}
	}
}
