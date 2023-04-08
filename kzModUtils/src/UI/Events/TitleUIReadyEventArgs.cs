using System;
using UnityEngine;

namespace kzModUtils.UI.Events
{
	public class TitleUIReadyEventArgs : EventArgs
	{
		public TitleTopUIController TitleTop { get; set; }

		public GameObject TitleTopControllerObject { get; set; }

		public TitleUIReadyEventArgs(TitleTopUIController ctrl, GameObject topControllerObject)
		{
			TitleTop = ctrl;
			TitleTopControllerObject = topControllerObject;
		}
	}
}
