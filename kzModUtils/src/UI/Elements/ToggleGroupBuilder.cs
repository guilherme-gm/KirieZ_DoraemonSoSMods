using System.Collections.Generic;
using UnityEngine;

namespace kzModUtils.UI.Elements
{
	public class ToggleGroupBuilder : BaseUIElementBuilder<ToggleGroupBuilder>
	{
		public List<string> Options = new List<string>();

		public ToggleGroupBuilder(): base(ElementType.ToggleGroupBuilder) { }

		public ToggleGroupBuilder AddOption(string text)
		{
			this.Options.Add(text);
			return this;
		}

		public ToggleGroupController Build(Transform parent = null)
		{
			var groupController = this.BuildBase(parent)
				.GetComponent<ToggleGroupController>();

			groupController.Initialize(this.Options);

			groupController.gameObject.SetActive(true);

			return groupController;
		}
	}
}
