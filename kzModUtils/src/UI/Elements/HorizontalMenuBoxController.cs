using UnityEngine;
using UnityEngine.UI;

namespace kzModUtils.UI.Elements
{
	public class HorizontalMenuBoxController: MonoBehaviour
	{
		[SerializeField]
		private Text titleText;

		public void Initialize(Text titleText)
		{
			this.titleText = titleText;
		}

		public void SetTitle(string text)
		{
			this.titleText.text = text;
			this.titleText.gameObject.SetActive(true);
		}
	}
}
