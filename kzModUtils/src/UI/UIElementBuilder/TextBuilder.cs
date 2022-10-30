using UnityEngine;

namespace kzModUtils.UI.UIElementBuilder
{
	public class TextBuilder : BaseUIElementBuilder<UnityEngine.UI.Text, TextBuilder>
	{
		protected string Text = null;

		protected int FontSize = -1;

		protected TextAnchor Align = TextAnchor.UpperLeft;

		public TextBuilder() : base(UIElementType.Text) { }

		public TextBuilder SetText(string text)
		{
			Text = text;
			return this;
		}

		public TextBuilder SetFontSize(int size)
		{
			FontSize = size;
			return this;
		}

		public TextBuilder SetAlignment(TextAnchor align)
		{
			Align = align;
			return this;
		}

		public override UnityEngine.UI.Text Build(Transform parentTransform = null)
		{
			var textObj = base.Build(parentTransform);

			if (Text != null)
				textObj.text = Text;

			if (FontSize != -1)
				textObj.fontSize = FontSize;

			textObj.alignment = Align;

			return textObj;
		}
	}
}
