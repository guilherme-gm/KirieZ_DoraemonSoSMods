using System;
using System.IO;
using kzModUtils.GameSave;

#nullable enable

namespace kzModUtils
{
	internal class ModDataSaveHandler : ISaveDataHandler
	{
		internal class LoadEventArgs: EventArgs {
			internal ModDataSavedState State;

			internal LoadEventArgs(ModDataSavedState state)
			{
				this.State = state;
			}
		}

		public event EventHandler<LoadEventArgs>? OnLoad;

		public string GetSuffix()
		{
			return "kzModUtils";
		}

		public void LoadGameData(byte[]? buffer)
		{
			var state = new ModDataSavedState();
			if (buffer == null) {
				this.OnLoad?.Invoke(this, new LoadEventArgs(state));
				return;
			}

			using (var stream = new MemoryStream(buffer))
			{
				using (var reader = new BinaryReader(stream))
				{
					state.Version = reader.ReadInt32();

					var eventCount = reader.ReadInt32();
					for (var i = 0; i < eventCount; i++)
					{
						var eventModId = reader.ReadString();
						var eventId = reader.ReadInt32();
						state.Events.Add(eventModId, eventId);
					}

					var itemCount = reader.ReadInt32();
					for (var i = 0; i < itemCount; i++)
					{
						var itemModId = reader.ReadString();
						var itemId = reader.ReadInt32();
						state.Items.Add(itemModId, itemId);
					}
				}
			}

			/* If there are new versions, upgrade it here. */

			if (state.Version != ModDataSavedState.CurrentVersion)
				throw new Exception($"Version mismatch. Loaded: {state.Version} ; Current: {ModDataSavedState.CurrentVersion}");

			this.OnLoad?.Invoke(this, new LoadEventArgs(state));
		}

		public byte[] SaveGameData()
		{
			using (var stream = new MemoryStream())
			{
				using (var writer = new BinaryWriter(stream))
				{
					writer.Write(ModDataSavedState.CurrentVersion);

					writer.Write(EventData.EventDataModule.Instance.EventConfigs.Count);
					foreach (var config in EventData.EventDataModule.Instance.EventConfigs)
					{
						writer.Write(config.EventModId);
						writer.Write(config.EventId);
					}

					writer.Write(ItemData.ItemModule.Instance.CustomItemConfigs.Count);
					foreach (var config in ItemData.ItemModule.Instance.CustomItemConfigs)
					{
						writer.Write(config.ModItemID);
						writer.Write(config.ItemId);
					}
				}

				return stream.ToArray();
			}
		}
	}
}
