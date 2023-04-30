using kzModUtils.Resource;
using System;

namespace kzModUtils.ItemData
{
	public class CustomItemConfig: IIdentifiableConfig
	{
		public string ModItemID { get; set; }

		internal int ItemId { get; set; }

		public string Name { get; set; }

		internal int NameId { get; set; }

		public string Description { get; set; }

		internal int DescriptionId { get; set; }

		public bool IsImportant { get; set; }

		public Define.Item.CategoryEnum Category { get; set; }

		public SpriteConfig Sprite { get; set; }

		public int ResourceId { get; set; }

		[Obsolete("AtlasId is part of the old direct approach. Please use Sprite property.")]
		public int AtlasId { get; set; }

		[Obsolete("SpriteId is part of the old direct approach. Please use Sprite property.")]
		public int SpriteId { get; set; }

		public ItemHelper.OnItemRegistered Callback { get; set; }

		internal CustomItem ToMasterModel(int itemId)
		{
			return new CustomItem(this.ModItemID, new CItemData.SItemData() {
				mItemId = itemId,
				mNameId = this.NameId,
				mDescriptionId = this.DescriptionId,
				mHasQuality = false,
				mIngredientsPoint = -1,
				mIsImportantItem = true,
				mIsIngredients = false,
				// Category-specific
				mFoodGroup = -1,
				mFurnitureId = -1,
				mCropId = -1,
				mOrnamentId = -1,
				mPrice = -1,
				mRecover = -1,
				mToolId = -1,
				// Category-specific / Size display
				mMaxSize = 0,
				mMinSize = 0,
				// Resource
				mResourceId = this.ResourceId, // FIXME: What is exactly this used for?

// Ignore obsolete warning here, as we need it to keep backward compatibility
#pragma warning disable 618

				// Sprite
				mAtlasId = (this.Sprite == null ? this.AtlasId : this.Sprite.AtlasId),
				mSpriteId = (this.Sprite == null ? this.SpriteId : this.Sprite.SpriteId),

// Restore obsolete warning
#pragma warning restore 618
			});
		}

		public int GetId()
		{
			return this.ItemId;
		}
	}
}
