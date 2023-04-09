using System.Collections.Generic;
using UnityEngine;

namespace kzModUtils.UI.Elements
{
	public class HorizontalMenuBoxBuilder : BaseUIElementBuilder<HorizontalMenuBoxBuilder>
	{
		public string Title = null;

		public List<GameObject> Children = new List<GameObject>();

		public HorizontalMenuBoxBuilder(): base(ElementType.HorizontalMenuBox) { }

		public HorizontalMenuBoxBuilder SetTitle(string title)
		{
			this.Title = title;
			return this;
		}

		public HorizontalMenuBoxBuilder AddChild(GameObject child)
		{
			this.Children.Add(child);
			return this;
		}

		public HorizontalMenuBoxController Build(Transform parent = null)
		{
			var menuController = this.BuildBase(parent)
				.GetComponent<HorizontalMenuBoxController>();

			if (this.Title != null)
				menuController.SetTitle(this.Title);

			foreach (var item in this.Children)
				item.transform.SetParent(menuController.transform);

			menuController.gameObject.SetActive(true);

			return menuController;
		}
	}
}
