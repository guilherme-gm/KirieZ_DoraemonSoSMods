using Fishbook.Entities;
using kzModUtils.Resource;
using kzModUtils.UI;
using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace Fishbook.UI
{
	internal class FishbookItemUIPartController : UIPartController, IPointerEnterHandler
	{
		private Image BorderImage;

		private Image FishSprite;

		private Image CrownSprite;

		private Text FishName;

		private Text FishShadow;

		private Text Completion;

		public int Index { get; private set; }

		public FishbookFish Fish { get; private set; }

		public event EventHandler OnHover;

		public void Initialize(int index, FishbookFish fish)
		{
			this.Index = index;

			var FishObject = this.transform?.Find("Fish")?.transform;
			if (FishObject == null)
			{
				Console.WriteLine("Failed to init FishbookItemUI - FishObject not found");
				return;
			}

			this.FishSprite = FishObject?.Find("Sprite")?.GetComponent<Image>();
			this.CrownSprite = FishObject?.Find("Crown")?.GetComponent<Image>();
			this.FishName = FishObject?.Find("Name")?.GetComponent<Text>();
			this.FishShadow = this.transform?.Find("Shadow")?.GetComponent<Text>();
			this.Completion = this.transform?.Find("Completion")?.GetComponent<Text>();
			this.BorderImage = this.gameObject.GetComponent<Image>();

			this.CrownSprite.sprite = UIUtils.GetUISprite(UISprite.Icon_Crown_01);
			this.CrownSprite.Deactivate();

			this.Deselect();

			if (fish.FoundOnce)
			{
				SingletonMonoBehaviour<AtlasFactory>.Instance.LoadSprite(
				fish.Item.AtlasId, fish.Item.SpriteId, delegate (UnityEngine.Sprite sprite)
				{
					this.FishSprite.sprite = sprite;
				});
				this.FishName.text = ResourceUtils.GetText(fish.Item.NameId);

				switch (fish.Shadow)
				{
					case ShadowSize.Small:
						this.FishShadow.text = "Small";
						break;

					case ShadowSize.Medium:
						this.FishShadow.text = "Medium";
						break;

					case ShadowSize.Large:
						this.FishShadow.text = "Large";
						break;
				}

				var fish1Data = SingletonMonoBehaviour<UserManager>.Instance.User.FishPicturebookModel.GetPicturebookInfo(fish.Item.Id);
				var fish2Data = SingletonMonoBehaviour<UserManager>.Instance.User.FishPicturebook2Model.GetPicturebookInfo(fish.Item.Id);
				var targetData = fish1Data == null ? fish2Data : fish1Data;

				if (targetData != null && targetData.MaxQuality == Define.Item.MAX_QUALITY)
					this.CrownSprite.Activate();
			}
			else
			{
				this.FishSprite.sprite = null;
				this.FishName.text = "???";
				this.FishShadow.text = "???";
			}

			int totalCount = fish.GetConditionsCount();
			int completeCount = fish.GetCompletedConditionsCount();
			int percent = (totalCount > 0 ? ((int) (((float) completeCount/totalCount) * 100f)) : 100);
			this.Completion.text = $"{percent}%";

			this.Fish = fish;
		}

		public void OnPointerEnter(PointerEventData data)
		{
			this.OnHover?.Invoke(this, null);
		}

		public void Select()
		{
			this.BorderImage.color = new Color32(255, 0, 0, 255);
		}

		public void Deselect()
		{
			this.BorderImage.color = new Color32(255, 0, 0, 0);
		}
	}
}
