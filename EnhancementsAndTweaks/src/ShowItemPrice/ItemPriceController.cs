using UnityEngine;
using kzModUtils.UI.Elements;

namespace EnhancementsAndTweaks.ShowItemPrice
{
	public class ItemPriceController : MonoBehaviour
	{
		private MessageBoxController MessageBox;

		public void Initialize(MessageBoxController controller)
		{
			this.MessageBox = controller;
		}

		public void SetItem(ItemModel item)
		{
			if (item == null)
			{
				this.Deactivate();
				return;
			}

			if (item.SellingPrice == -1)
				this.MessageBox.SetText("Unsellable");
			else if (item.Master.HasQuality)
				this.MessageBox.SetText($"Base price:\n   {item.Master.BaseSellingPrice}\nSell price:\n   {item.SellingPrice}");
			else
				this.MessageBox.SetText($"Sell price:\n   {item.SellingPrice}");

			this.Activate();
		}
	}
}
