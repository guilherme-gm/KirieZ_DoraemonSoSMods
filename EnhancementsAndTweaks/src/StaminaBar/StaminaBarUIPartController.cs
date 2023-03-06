using UnityEngine;

namespace EnhancementsAndTweaks.StaminaBar
{
	public class StaminaBarUIPartController : MonoBehaviour
	{
		private static UnityEngine.UI.Text StaminaText;

		public void Initialize()
		{
			StaminaText = this.gameObject.GetComponentInChildren(typeof(UnityEngine.UI.Text)).GetComponent<UnityEngine.UI.Text>();
			UpdateText();
		}

		public void UpdateText()
		{
			if (StaminaText == null)
				return;

			var staminaModel = SingletonMonoBehaviour<UserManager>.Instance.User.Player.Stamina;
			if (staminaModel == null)
				return;

			if (staminaModel.IsEmpty)
			{
				StaminaText.text = "Stamina: ----";
				return;
			}

			if (!staminaModel.CanConsume)
			{
				StaminaText.text = "Stamina: ∞";
				return;
			}

			StaminaText.text = $"Stamina: {staminaModel.Now}/{staminaModel.Max}";
		}
	}
}
