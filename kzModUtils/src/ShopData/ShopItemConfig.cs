using kzModUtils.ItemData;
using kzModUtils.EventData;

#nullable enable

namespace kzModUtils.ShopData
{
	public class ShopItemConfig
	{
		public ShopId TargetShop { get; set; }

		public IdHolder<CustomItemConfig> Item { get; set; }

		public int Price { get; set; }

		public IdHolder<EventConfig>? SellOnceEvent { get; set; } = null;

		internal int? shopId { get; set; } = null;

		public ShopItemConfig(IdHolder<CustomItemConfig> item) {
			this.Item = item;
		}

		internal ShopMasterModel ToShopMasterModel(int id)
		{
			this.shopId = id;

			return new ShopMasterModel(new CVarietyShopData.SVarietyShopData() {
				mVarietyId = id,
				mItemId = this.Item.Id,
				mIsFirstYear = this.TargetShop == ShopId.VarietyShopFirstYear,
				mPrice = this.Price,
				mDLCIndex = -1,
				mSeason = -1,
			});
		}
	}
}
