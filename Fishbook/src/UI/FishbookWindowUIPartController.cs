using Fishbook.Entities;
using System;
using UnityEngine;

namespace Fishbook.UI
{
	internal class FishbookWindowUIPartController : UIPartController
	{
		public GameObject InputArea { get; private set; }

		public GameObject ListArea { get; private set; }

		private RectTransform ListAreaTransform { get; set; }

		private GameObject ScrollView { get; set; }

		private GameObject DetailsObject { get; set; }

		private RectTransform ScrollViewTransform { get; set; }

		public event EventHandler OnUp;

		public event EventHandler OnDown;

		public event EventHandler OnToggleSeason;

		public void Initialize()
		{
			this.InputArea = this.transform
				.Find("Input").gameObject;
			this.ScrollView = this.transform
				.Find("Scroll View")?.gameObject;
			this.ListArea = this.transform
				.Find("Scroll View")?.transform?
				.Find("Viewport")?.transform?
				.Find("Content")?.gameObject;
			this.ListAreaTransform = this.ListArea.gameObject.GetComponent<RectTransform>();
			this.ScrollViewTransform = this.ScrollView.transform.GetComponent<RectTransform>();
			this.DetailsObject = this.transform.Find("Details").gameObject;
		}

		public override void CancelAction()
		{
			base.Close();
			SingletonMonoBehaviour<UIManager>.Instance.Pop(null);
		}

		public override void SubmitAction()
		{
			base.SubmitAction();
			this.OnToggleSeason?.Invoke(this, null);
		}

		public override void MoveDownAction()
		{
			base.MoveDownAction();
			this.OnDown?.Invoke(this, null);
		}

		public override void MoveUpAction()
		{
			base.MoveUpAction();
			this.OnUp?.Invoke(this, null);
		}

		public void SnapTo(RectTransform target)
		{
			Canvas.ForceUpdateCanvases();

			var areaHeight = this.ScrollViewTransform.sizeDelta.y;
			var currentScrollTop = -1 * this.ListAreaTransform.anchoredPosition.y;
			var currentScrollBottom = currentScrollTop - areaHeight;

			var targetTop = target.anchoredPosition.y;
			var targetBottom = target.anchoredPosition.y - target.sizeDelta.y;

			float heightChange = 0;
			if (targetBottom < currentScrollBottom) {
				heightChange = targetBottom - currentScrollBottom - 2;
			} else if (targetTop > currentScrollTop) {
				heightChange = targetTop - currentScrollTop + 2;
			}

			this.ListAreaTransform.anchoredPosition = new Vector2(
				this.ListAreaTransform.anchoredPosition.x,
				this.ListAreaTransform.anchoredPosition.y - heightChange
			);
		}

		public void SetFish(FishbookFish fish)
		{
			foreach (Transform line in this.DetailsObject.transform)
				GameObject.Destroy(line.gameObject);

			foreach (var cond in fish.Conditions)
			{
				foreach (var condText in cond.Value.GetConditionTexts())
				{
					(new kzModUtils.UI.Elements.TextBuilder())
						.SetParent(this.DetailsObject.transform)
						.SetText(condText)
						.SetSize(new Vector2(240, 20))
						.SetFontSize(18)
						.Build();
				}
			}
		}
	}
}
