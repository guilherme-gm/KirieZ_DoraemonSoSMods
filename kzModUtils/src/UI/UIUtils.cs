using System;
using kzModUtils.UI.Events;
using UnityEngine;

namespace kzModUtils.UI
{
	/**
	 * Utilitary class to deal with UI processes
	 */
	public static class UIUtils
	{
		/**
		 * Event Log controller.
		 * Event Log is responsible for displaying quick status updates in the bottom left part of the screen,
		 * such as item obtained or stamina info.
		 */
		public static EventLogUIPartController EventLog { get; internal set; } = null;

		/**
		 * Game's main UI Canvas.
		 * Usually you want to add your UI elements as a child of this canvas. Specially if it is an element that is permanently on screen.
		 */
		public static GameObject CommonUICanvas { get; internal set; } = null;

		/**
		 * Event triggered when the Game UI is ready to receive UI elements.
		 *
		 * To be used by mods that want to extend the game's UI
		 */
		public static event EventHandler<GameUIReadyEventArgs> OnGameUIReady;

		/**
		 * Closes any dialog that is currently open.
		 */
		public static void CloseDialog()
		{
			// SingletonMonoBehaviour<UIManager>.Instance.Pop(null);
			SingletonMonoBehaviour<UIManager>.Instance.PopExceptForBottom(null);
		}

		internal static void FireGameUIReady(GameUIReadyEventArgs args)
		{
			UIUtils.OnGameUIReady?.Invoke(null, args);
		}
	}
}
