using BepInEx;
using HarmonyLib;
using kzModUtils.UI;
using System;
using System.Collections.Generic;

namespace kzModUtils
{
	[BepInPlugin("io.github.guilherme-gm.DoraemonSoSMods.kzModUtils", "kz Mod Utils", PluginInfo.PLUGIN_VERSION)]
	public class Plugin : BaseUnityPlugin
	{
		private static Plugin Instance;

		private IModule[] Modules = new IModule[]
		{
			Resource.ResourceModule.Instance,
			TimeModule.Instance,
			UIModule.Instance,
			TextData.TextDataModule.Instance,
			ItemData.ItemModule.Instance,
			EventData.EventDataModule.Instance,
			ShopData.ShopDataModule.Instance,
			GameSave.GameSaveModule.Instance,
		};

		private ICollectionModule[] CollectionModules = new ICollectionModule[]
		{
			TextData.TextDataModule.Instance,
			ItemData.ItemModule.Instance, // Requires text
			EventData.EventDataModule.Instance, // Requires text
			ShopData.ShopDataModule.Instance, // Requires item / event
		};

		private void RemoveFromInventory(ItemSlotModel inventory, int itemId)
		{
			for (var slotIdx = 0; slotIdx < inventory.Slots.Length; slotIdx++)
			{
				var slot = inventory.Slots[slotIdx];

				// Don' t use other options because they check for Master, which won't exist if item doesn't exists.
				if (slot?.Id == itemId)
					inventory.ReduceSlotItem(slotIdx, slot.Count);
			}
		}

		private void OnModStateLoaded(object sender, ModDataSaveHandler.LoadEventArgs args)
		{
			foreach (var mod in Instance.CollectionModules)
				mod.Setup(args.State);

			var loadedEvents = new List<string>();
			foreach (var item in args.State.Events)
				loadedEvents.Add(item.Key);

			foreach (var item in EventData.EventDataModule.Instance.EventConfigs)
				loadedEvents.Remove(item.EventModId);

			foreach (var oldEvent in loadedEvents) {
				Console.WriteLine($"Event \"{oldEvent}\" (ID: {args.State.Events[oldEvent]}) was removed from the game DB but found on your save. Forcing its removal.");
				SingletonMonoBehaviour<UserManager>.Instance.User.FinishedEvent.Unregister(args.State.Events[oldEvent]);
			}

			var loadedItems = new List<string>();
			foreach (var item in args.State.Items)
				loadedItems.Add(item.Key);

			foreach (var item in ItemData.ItemModule.Instance.CustomItemConfigs)
				loadedItems.Remove(item.ModItemID);

			foreach (var oldItem in loadedItems)
			{
				var itemId = args.State.Items[oldItem];
				Console.WriteLine($"Item \"{oldItem}\" (ID: {itemId}) was removed from the game DB but found on your save. Forcing its removal.");
				RemoveFromInventory(SingletonMonoBehaviour<UserManager>.Instance.User.Inventory, itemId);
				RemoveFromInventory(SingletonMonoBehaviour<UserManager>.Instance.User.Refrigerator, itemId);
				RemoveFromInventory(SingletonMonoBehaviour<UserManager>.Instance.User.Chest, itemId);
				RemoveFromInventory(SingletonMonoBehaviour<UserManager>.Instance.User.MaterialStorage, itemId);
				RemoveFromInventory(SingletonMonoBehaviour<UserManager>.Instance.User.ShippingBox, itemId);
				RemoveFromInventory(SingletonMonoBehaviour<UserManager>.Instance.User.ImportantItemSlots, itemId);
			}
		}

		private void Awake()
		{
			Plugin.Instance = this;

			// Plugin startup logic
			Logger.LogInfo($"Plugin {PluginInfo.PLUGIN_GUID} is loaded!");

			Harmony.CreateAndPatchAll(typeof(Plugin));

			foreach (var mod in Modules)
				mod.Initialize();

			var saveHandler = new ModDataSaveHandler();
			saveHandler.OnLoad += this.OnModStateLoaded;
			GameSave.GameSaveHelper.RegisterSaveHandler(saveHandler);
		}

		private void Destroy()
		{
			foreach (var mod in Modules)
				mod.Teardown();
		}

		[HarmonyPatch(typeof(MasterManager), "SetupMasters")]
		[HarmonyPostfix]
		private static void OnMasterCollectionSetup()
		{
			foreach (var mod in Instance.CollectionModules)
				mod.Setup();
		}
	}
}
