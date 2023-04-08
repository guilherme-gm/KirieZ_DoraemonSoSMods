using UnityEngine;

namespace kzModUtils.UI.Elements
{
	public class ToggleMenuBoxController: MonoBehaviour
	{
		[SerializeField]
		private HorizontalMenuBoxController boxController;

		[SerializeField]
		private ToggleGroupController toggleGroup;

		public int selectedIndex { get; private set; }

		public void Initialize(HorizontalMenuBoxController boxController, ToggleGroupController toggleGroup)
		{
			this.boxController = boxController;
			this.toggleGroup = toggleGroup;

			this.toggleGroup.SetChangeToggleValueAction(delegate (ToggleGroupController controller, int idx, bool value) {
				if (value)
					selectedIndex = idx;
			});
		}

		public void SetTitle(string text)
		{
			this.boxController.SetTitle(text);
		}

		public void SetSelected(int index)
		{
			this.toggleGroup.SetToggleOn(index);
		}
	}
}
