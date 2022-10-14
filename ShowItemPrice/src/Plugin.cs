using BepInEx;
using HarmonyLib;

namespace ShowItemPrice
{
	[BepInPlugin("io.github.guilherme-gm.DoraemonSoSMods.showItemPrice", "Show Item Price", PluginInfo.PLUGIN_VERSION)]
	public class Plugin : BaseUnityPlugin
	{
		private void Awake()
		{
			// Plugin startup logic
			Logger.LogInfo($"Plugin {PluginInfo.PLUGIN_GUID} is loaded!");

			Harmony.CreateAndPatchAll(typeof(Plugin));
		}

		[HarmonyPatch(typeof(ItemExplainController), "SetItem")]
		[HarmonyPrefix]
		static bool SetItemWithPrice(
			ItemExplainController __instance,
			ItemModel item,
			ref UnityEngine.UI.Text ___mTitleText,
			ref UnityEngine.UI.Text ___mInfoText,
			ref RateIconController ___mItemQuality,
			ref UnityEngine.UI.Text ___mItemNumText
		)
		{
			if (item == null)
			{
				if (___mTitleText != null)
				{
					___mTitleText.text = string.Empty;
				}
				if (___mInfoText != null)
				{
					___mInfoText.text = string.Empty;
				}
				if (___mItemNumText != null)
				{
					___mItemNumText.transform.parent.gameObject.Deactivate();
				}
				if (___mItemQuality != null)
				{
					___mItemQuality.Deactivate();
				}
				__instance.Deactivate();
			}
			else
			{
				if (___mTitleText != null)
				{
					___mTitleText.text = item.Name;
				}
				if (___mInfoText != null)
				{
					___mInfoText.text = item.Description;
					if (item.Size > 0f)
					{
						UnityEngine.UI.Text text = ___mInfoText;
						text.text = text.text + "\n" + SingletonMonoBehaviour<MasterManager>.Instance.TextMaster.GetText(Define.Text.Common.ID_0000_0056) + TextUtility.GetSizeString(item.Id, item.Size);
					}
				}
				if (___mItemNumText != null)
				{
					___mItemNumText.transform.parent.gameObject.Activate();
					___mItemNumText.text = item.Count.ToString();
				}
				if (___mItemQuality != null)
				{
					if (item.Master.HasQuality)
					{
						___mItemQuality.SetRate(item.Quality);
						___mItemQuality.Activate();
					}
					else
					{
						___mItemQuality.Deactivate();
					}
				}

				if (item.SellingPrice != -1)
				{
					___mInfoText.text = ___mInfoText.text + "\nSell price: " + item.SellingPrice + " (Base: " + item.Master.BaseSellingPrice + ")";
				}
				__instance.Activate();
			}

			return false;
		}
	}
}
