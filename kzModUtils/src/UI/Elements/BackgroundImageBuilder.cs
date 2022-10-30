using UnityEngine;

namespace kzModUtils.UI.Elements
{
	public class BackgroundImageBuilder : BaseUIElementBuilder<BackgroundImageBuilder>
	{
		public BackgroundImageBuilder(): base(ElementType.BackgroundImage) { }

		public UnityEngine.UI.Image Build(Transform parent = null)
		{
			return this.BuildBase(parent).GetComponent<UnityEngine.UI.Image>();
		}
	}
}
