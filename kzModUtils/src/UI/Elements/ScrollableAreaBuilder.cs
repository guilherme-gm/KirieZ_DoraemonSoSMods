using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace kzModUtils.UI.Elements
{
	public class ScrollableAreaBuilder : BaseUIElementBuilder<ScrollableAreaBuilder>
	{
		public List<GameObject> Children = new List<GameObject>();

		public ScrollableAreaBuilder(): base(ElementType.ScrollableArea) { }

		public ScrollableAreaBuilder AddChild(GameObject child)
		{
			this.Children.Add(child);
			return this;
		}

		public GameObject Build(Transform parent = null)
		{
			var scrollableArea = this.BuildBase(parent);

			var content = scrollableArea.transform.Find("Scroll_Contetns");

			foreach (var item in this.Children)
				item.transform.SetParent(content.transform);

			var scrollbarRect = scrollableArea.transform.Find("Scrollbar")
				.GetComponent<RectTransform>();

			if (this.Size != Vector2.zero)
				scrollbarRect.sizeDelta = new Vector2(scrollbarRect.sizeDelta.x, this.Size.y);

			scrollableArea.GetComponent<UITableViewController>().Initialize();
			scrollableArea.gameObject.SetActive(true);

			// Unity doesn't seem to handle script generated layout groups very well, force it to rebuild
			// so elements get their proper spacing. Hopefully this will not hurt performance...
			LayoutRebuilder.ForceRebuildLayoutImmediate(scrollableArea.GetComponent<RectTransform>());

			return scrollableArea;
		}
	}
}
