using System;
using System.Collections.Generic;
using UnityEngine;

namespace kzModUtils.UI.Elements
{
	public class MessageBoxBuilder : BaseUIElementBuilder<MessageBoxBuilder>
	{
		private static Dictionary<MessageBoxStyles, MessageBoxStyle> Styles;

		internal static void SetupStyles()
		{
			Styles = new Dictionary<MessageBoxStyles, MessageBoxStyle>();
			Styles.Add(MessageBoxStyles.Brown, new MessageBoxStyle() {
				MainBgSprite = UIAssets.Sprites[UISprite.Square_R_02],
				MainBgColor = new Color32(214, 171, 121, 160),
				MainTextColor = new Color32(60, 38, 20, 255),
				TitleBgSprite = UIAssets.Sprites[UISprite.Square_R_05],
				TitleBgColor = new Color32(137, 83, 26, 255),
				TitleTextColor = new Color32(255, 247, 217, 255),
			});

			Styles.Add(MessageBoxStyles.Blue, new MessageBoxStyle() {
				MainBgSprite = UIAssets.Sprites[UISprite.Square_R_02],
				MainBgColor = new Color32(149, 206, 253, 160),
				MainTextColor = new Color32(20, 30, 60, 255),
				TitleBgSprite = UIAssets.Sprites[UISprite.Square_R_05],
				TitleBgColor = new Color32(31, 96, 161, 255),
				TitleTextColor = new Color32(255, 255, 255, 255),
			});

			Styles.Add(MessageBoxStyles.Green, new MessageBoxStyle() {
				MainBgSprite = UIAssets.Sprites[UISprite.Square_R_02],
				MainBgColor = new Color32(196, 219, 129, 160),
				MainTextColor = new Color32(46, 60, 20, 255),
				TitleBgSprite = UIAssets.Sprites[UISprite.Square_R_05],
				TitleBgColor = new Color32(80, 137, 26, 255),
				TitleTextColor = new Color32(255, 255, 255, 255),
			});

			Styles.Add(MessageBoxStyles.Red, new MessageBoxStyle() {
				MainBgSprite = UIAssets.Sprites[UISprite.Square_R_02],
				MainBgColor = new Color32(255, 199, 178, 160),
				MainTextColor = new Color32(60, 33, 20, 255),
				TitleBgSprite = UIAssets.Sprites[UISprite.Square_R_05],
				TitleBgColor = new Color32(161, 50, 31, 255),
				TitleTextColor = new Color32(255, 255, 255, 255),
			});

			Styles.Add(MessageBoxStyles.Purple, new MessageBoxStyle() {
				MainBgSprite = UIAssets.Sprites[UISprite.Square_R_02],
				MainBgColor = new Color32(234, 178, 255, 160),
				MainTextColor = new Color32(60, 33, 20, 255),
				TitleBgSprite = UIAssets.Sprites[UISprite.Square_R_05],
				TitleBgColor = new Color32(163, 54, 193, 255),
				TitleTextColor = new Color32(255, 255, 255, 255),
			});
		}

		private string Title = "";

		private string Text = "";

		private MessageBoxStyle Style = null;

		public MessageBoxBuilder(): base(ElementType.MessageBox) { }

		public MessageBoxBuilder SetTitle(string title)
		{
			this.Title = title;
			return this;
		}

		public MessageBoxBuilder SetText(string text)
		{
			this.Text = text;
			return this;
		}

		public MessageBoxBuilder SetStyle(MessageBoxStyles style)
		{
			this.Style = MakeStyle(style);
			return this;
		}

		public MessageBoxBuilder SetStyle(MessageBoxStyle style)
		{
			this.Style = style;
			return this;
		}

		private MessageBoxStyle MakeStyle(MessageBoxStyles styleName)
		{
			if (!Styles.TryGetValue(styleName, out var refStyle))
				throw new Exception($"Could not find style '{styleName}' in Styles list.");

			return new MessageBoxStyle()
			{
				MainBgSprite = refStyle.MainBgSprite,
				MainBgColor = refStyle.MainBgColor,
				MainTextColor = refStyle.MainTextColor,
				TitleBgColor = refStyle.TitleBgColor,
				TitleBgSprite = refStyle.TitleBgSprite,
				TitleTextColor = refStyle.TitleTextColor,
			};
		}

		public MessageBoxController Build(Transform parent = null)
		{
			var baseObj = this.BuildBase(parent);
			var controller = baseObj.AddComponent<MessageBoxController>();
			controller.Initialize();

			controller.SetTitle(Title);
			controller.SetText(Text);
			if (Style == null)
				Style = MakeStyle(MessageBoxStyles.Brown);
			controller.SetStyle(Style);

			return controller;
		}
	}
}
