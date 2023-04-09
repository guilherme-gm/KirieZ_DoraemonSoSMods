using UnityEngine;

namespace kzModUtils.UI.Elements
{
	public class SmallMenuSeparatorBuilder : BaseUIElementBuilder<SmallMenuSeparatorBuilder>
	{
		public string Title = "";

		public Sprite Sprite = null;

		public SmallMenuSeparatorBuilder(): base(ElementType.SmallMenuSeparator) { }

		public SmallMenuSeparatorBuilder SetTitle(string title)
		{
			this.Title = title;
			return this;
		}

		public SmallMenuSeparatorBuilder SetSprite(Sprite sprite)
		{
			this.Sprite = sprite;
			return this;
		}

		public SmallMenuSeparatorController Build(Transform parent = null)
		{
			var separatorController = this.BuildBase(parent)
				.GetComponent<SmallMenuSeparatorController>();

			separatorController.SetText(this.Title);

			if (this.Sprite != null)
				separatorController.SetSprite(this.Sprite);

			separatorController.gameObject.SetActive(true);

			return separatorController;
		}
	}
}
