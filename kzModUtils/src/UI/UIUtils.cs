using System;
using System.Collections.Generic;
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
		 * Padding (multiplier) applied to menu ranges, this limits how many
		 * custom tabs there may be between 2 original tabs.
		 */
		internal static int MenuRangePadding = 100;

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
		 * Event triggered when the Title UI (Main menu) is ready to receive UI elements.
		 *
		 * This is an internal event that is called before OnTitleUIReady.
		 * Used to generate internal prefabs before other mods starts using them.
		 */
		internal static event EventHandler<TitleUIReadyEventArgs> PreOnTitleUIReady;

		/**
		 * Event triggered when the Title UI (Main menu) is ready to receive UI elements.
		 *
		 * To be used by mods that want to extend the game's UI
		 */
		public static event EventHandler<TitleUIReadyEventArgs> OnTitleUIReady;

		/**
		 * Event triggered when the Game UI is ready to receive UI elements.
		 *
		 * To be used by mods that want to extend the game's UI
		 */
		public static event EventHandler<GameUIReadyEventArgs> OnGameUIReady;

		/**
		 * List of custom tabs for game's menu (M key)
		 */
		internal static SortedList<int, CustomGameMenuTabConfig> CustomMenuTabs = new SortedList<int, CustomGameMenuTabConfig>();

		/**
		 * Closes any dialog that is currently open.
		 */
		public static void CloseDialog()
		{
			// SingletonMonoBehaviour<UIManager>.Instance.Pop(null);
			SingletonMonoBehaviour<UIManager>.Instance.PopExceptForBottom(null);
		}

		internal static void FireTitleUIReady(TitleUIReadyEventArgs args)
		{
			UIUtils.PreOnTitleUIReady?.Invoke(null, args);
			UIUtils.OnTitleUIReady?.Invoke(null, args);
		}

		internal static void FireGameUIReady(GameUIReadyEventArgs args)
		{
			UIUtils.OnGameUIReady?.Invoke(null, args);
		}

		public static Sprite GetUISprite(UISprite sprite)
		{
			return UIAssets.Sprites.GetValue(sprite, null);
		}

		/**
		 * Registers a custom game menu tab (M key)
		 */
		public static void RegisterGameMenuTab(CustomGameMenuTabConfig config)
		{
			int lowerRange = ((int) config.After) * MenuRangePadding;
			int upperRange = (((int) config.After) + 1) * MenuRangePadding;

			int idx = lowerRange;
			while (idx < upperRange && CustomMenuTabs.ContainsKey(idx))
				idx++;

			if (idx == upperRange) {
				PluginLogger.LogError($"Can't add custom menu tab ''. Maximum custom menus between '{config.After}' and '{(CustomGameMenuTabConfig.GameMenuTabs) ((int) config.After + 1)}' reached.");
				return;
			}

			CustomMenuTabs.Add(idx, config);
		}
	}
}
