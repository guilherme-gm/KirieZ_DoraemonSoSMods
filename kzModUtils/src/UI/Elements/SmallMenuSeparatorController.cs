using UnityEngine;
using UnityEngine.UI;

namespace kzModUtils.UI.Elements
{
	public class SmallMenuSeparatorController: MonoBehaviour
	{
		[SerializeField]
		private Text titleText;

		[SerializeField]
		private Image titleSprite;

		internal void Setup(Text titleText, Image titleSprite)
		{
			this.titleText = titleText;
			this.titleSprite = titleSprite;
		}

		public void SetText(string text)
		{
			if (this.titleText == null)
				return;

			this.titleText.text = text;
		}

		public void SetSprite(Sprite sprite)
		{
			if (this.titleSprite == null)
				return;

			this.titleSprite.sprite = sprite;
			this.titleSprite.gameObject.SetActive(true);
		}
	}
}
