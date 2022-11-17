using kzModUtils.EventData;

namespace kzModUtils.ShopData
{
	public class ShopHelper
	{
		/**
		 * Registers a event config for BuyOnce events.
		 */
		public static void RegisterBuyOnceEvent(EventConfig eventConfig)
		{
			// It is defined here because Event api is still too incomplete and
			// we are doing some hacks for that one too.
			EventHelper.RegisterEvent(eventConfig);
		}

		public static void RegisterItemToSell(ShopItemConfig config)
		{
			ShopDataModule.Instance.ShopConfigs.Add(config);
		}
	}
}
