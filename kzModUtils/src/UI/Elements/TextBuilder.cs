using System;
using UnityEngine;

namespace kzModUtils.UI.Elements
{
	public class TextBuilder : BaseUIElementBuilder<TextBuilder>
	{
		protected string Text = null;

		protected int FontSize = -1;

		protected TextAnchor Align = TextAnchor.UpperLeft;

		public TextBuilder() : base(ElementType.Text) { }

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

		public UnityEngine.UI.Text Build(Transform parentTransform = null)
		{
			var textObj = base.BuildBase(parentTransform).GetComponent<UnityEngine.UI.Text>();
			if (textObj == null) {
				Console.WriteLine("TextBuilder: Could not find text component.");
				throw new Exception("Could not find text component.");
			}

			if (Text != null)
				textObj.text = Text;

			if (FontSize != -1)
				textObj.fontSize = FontSize;

			textObj.alignment = Align;

			return textObj;
		}
	}
}
