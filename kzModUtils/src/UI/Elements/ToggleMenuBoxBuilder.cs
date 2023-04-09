using System.Collections.Generic;
using UnityEngine;

namespace kzModUtils.UI.Elements
{
	public class ToggleMenuBoxBuilder : BaseUIElementBuilder<ToggleMenuBoxBuilder>
	{
		public string Title = null;

		public List<string> Options = new List<string>();

		public ToggleMenuBoxBuilder(): base(ElementType.ToggleMenuBox) { }

		public ToggleMenuBoxBuilder SetTitle(string title)
		{
			this.Title = title;
			return this;
		}

		public ToggleMenuBoxBuilder AddOption(string text)
		{
			this.Options.Add(text);
			return this;
		}

		public ToggleMenuBoxController Build(Transform parent = null)
		{
			if (parent == null)
				parent = this.Parent;

			var toggleGroup = (new ToggleGroupBuilder() {
				Options = this.Options,
			}).Build();

			var box = (new HorizontalMenuBoxBuilder() {
				Parent = parent,
				Position = this.Position,
				Size = this.Size,
				Title = this.Title,
			})
				.AddChild(toggleGroup.gameObject)
				.Build();

			var toggleBoxController = box.gameObject.AddComponent<ToggleMenuBoxController>();
			toggleBoxController.Initialize(box, toggleGroup);

			box.gameObject.SetActive(true);

			return toggleBoxController;
		}
	}
}
