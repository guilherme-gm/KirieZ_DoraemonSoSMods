using System;
using UnityEngine;

namespace kzModUtils.UI.Events
{
	public class GameUIReadyEventArgs : EventArgs
	{
		public GameObject Parent { get; set; }

		public FarmTopUIController FarmTop { get; set; }

		public GameUIReadyEventArgs(GameObject parent, FarmTopUIController ctrl)
		{
			Parent = parent;
			FarmTop = ctrl;
		}
	}
}
