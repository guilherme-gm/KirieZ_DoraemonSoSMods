namespace kzModUtils.ItemData
{
	public class CustomItemConfig
	{
		public string ModItemID { get; set; }

		public string Name { get; set; }

		internal int NameId { get; set; }

		public string Description { get; set; }

		internal int DescriptionId { get; set; }

		public bool IsImportant { get; set; }

		public Define.Item.CategoryEnum Category { get; set; }

		public int AtlasId { get; set; }

		public int ResourceId { get; set; }

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
				// Sprite
				mAtlasId = 20101, // ATLAS_ID,
				mResourceId = 2130, // FIXME: What is exactly this used for?
				mSpriteId = 1002130, // 1,
			});
		}
	}
}
