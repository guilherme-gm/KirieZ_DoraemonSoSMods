using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace kzModUtils.UI.Elements
{
	public class ToggleGroupController: MonoBehaviour
	{
		[SerializeField]
		private GameObject buttonPrefab;

		[SerializeField]
		private ToggleButtonController[] toggleButtons;

		internal void Setup(GameObject buttonPrefab)
		{
			this.buttonPrefab = buttonPrefab;
			this.toggleButtons = new ToggleButtonController[0];
		}

		public void Initialize(List<string> buttons)
		{
			var toggleGroup = this.GetComponent<ToggleGroup>();

			List<ToggleButtonController> controllers = new List<ToggleButtonController>();
			foreach (var text in buttons) {
				var newButtonObject = GameObject.Instantiate(buttonPrefab, this.gameObject.transform);
				newButtonObject.GetComponent<Toggle>()
					.group = toggleGroup;

				var btnController = newButtonObject.transform.GetChild(0).GetComponent<ToggleButtonController>();
				btnController.SetText(text);

				controllers.Add(btnController);
			}

			this.toggleButtons = controllers.ToArray();

			for (int i = 0; i < this.toggleButtons.Length; i++)
				this.SetButtonEnable(i, true);
		}

		public void SetButtonText(int index, string text)
		{
			if (index < 0 || index >= this.toggleButtons.Length || this.toggleButtons[index] == null)
				return;

			this.toggleButtons[index].SetText(text);
		}

		public void SetButtonEnable(int index, bool is_enable)
		{
			this.toggleButtons[index].SetEnable(is_enable);
		}

		public void SetChangeToggleValueAction(Action<ToggleGroupController, int, bool> changeValueCallback)
		{
			for (int i = 0; i < this.toggleButtons.Length; i++) {
				int index = i;
				this.toggleButtons[i].SetOnChangeValue(delegate(bool isOn) {
					changeValueCallback(this, index, isOn);
				});
			}
		}

		public void SetToggleOn(int index)
		{
			if (index < 0 || index >= this.toggleButtons.Length || this.toggleButtons[index] == null)
				return;

			this.toggleButtons[index].ToggleOn();
		}

		public int GetSelectedIndex()
		{
			for (int i = 0; i < this.toggleButtons.Length; i++) {
				if (this.toggleButtons[i].GetIsOn())
					return i;
			}

			return -1;
		}
	}
}
