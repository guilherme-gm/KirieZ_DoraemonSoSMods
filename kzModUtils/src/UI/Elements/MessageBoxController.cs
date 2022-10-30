using System;
using UnityEngine;
using UnityEngine.UI;

namespace kzModUtils.UI.Elements
{
	public class MessageBoxController : MonoBehaviour {
		private Image MainBg;

		private Text MainText;

		private Image TitleBg;

		private Text TitleText;

		public void Initialize () {
			this.MainBg = this.transform.Find("BgImage").GetComponent<Image>();
			this.MainText = this.transform.Find("MainText").GetComponent<Text>();

			var titleObj = this.transform.Find("Title").transform;
			this.TitleBg = titleObj.Find("TitleImage").GetComponent<Image>();
			this.TitleText = titleObj.Find("TitleText").GetComponent<Text>();
		}

		public void SetStyle(MessageBoxStyle style)
		{
			this.MainBg.sprite = style.MainBgSprite;
			this.MainBg.color = style.MainBgColor;
			this.MainText.color = style.MainTextColor;

			this.TitleBg.sprite = style.TitleBgSprite;
			this.TitleBg.color = style.TitleBgColor;
			this.TitleText.color = style.TitleTextColor;
		}

		public void SetTitle(string title)
		{
			this.TitleText.text = title;
		}

		public void SetText(string text)
		{
			this.MainText.text = text;
		}
	}
}
