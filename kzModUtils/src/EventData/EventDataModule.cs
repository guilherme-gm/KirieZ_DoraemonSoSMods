using System;
using System.Collections.Generic;
using HarmonyLib;

namespace kzModUtils.EventData
{
	internal class EventDataModule: IModule, ICollectionModule
	{
		// Official last id is around 90_000_000 ; so let's give it some breath space, as we still have a lot of space
		// between 200M and 2B
		private static readonly int StartFromId = 200_000_000;

		#region Singleton Setup
		private static EventDataModule mInstance;

		internal static EventDataModule Instance
		{
			get {
				if (mInstance == null)
					mInstance = new EventDataModule();

				return mInstance;
			}
			private set {}
		}
		#endregion

		private int NextId = EventDataModule.StartFromId;

		private Dictionary<int, EventMasterModel> Events = new Dictionary<int, EventMasterModel>();

		internal List<EventConfig> EventConfigs = new List<EventConfig>();

		public void Initialize()
		{
			Harmony.CreateAndPatchAll(typeof(EventDataModule));
		}

		public void Teardown()
		{
		}

		[HarmonyPatch(typeof(EventMasterCollection), "Setup")]
		[HarmonyPostfix]
		static void OnOriginalSetup(
			Dictionary<int, EventMasterModel> ___mEvents
		)
		{
			EventDataModule.Instance.Events = ___mEvents;
		}

		private void UnregisterAll()
		{
			foreach (var item in this.EventConfigs)
				this.Events.Remove(item.GetId());
		}

		public void Setup(ModDataSavedState state = null)
		{
			this.UnregisterAll();

			var reservedIds = new HashSet<int>();
			if (state != null)
			{
				foreach (var existing in state.Events)
					reservedIds.Add(existing.Value);
			}

			this.NextId = EventDataModule.StartFromId;

			foreach (var item in this.EventConfigs)
			{
				int eventId;

				if (state == null || state.Events.TryGetValue(item.EventModId, out eventId) == false)
				{
					while (this.NextId < int.MaxValue && (this.Events.ContainsKey(this.NextId) || reservedIds.Contains(this.NextId)))
						this.NextId++;

					if (this.NextId == int.MaxValue) {
						Console.WriteLine($"Can't register more Events. Limit reached. Skipping \"{item.EventModId}\"");
						continue;
					}

					eventId = this.NextId;
				}

				this.Events.Add(eventId, item.ToEventMasterModel(eventId));
			}
		}
	}
}
