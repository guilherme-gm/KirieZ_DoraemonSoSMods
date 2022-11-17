using System;
using System.Collections.Generic;
using HarmonyLib;

namespace kzModUtils.EventData
{
	internal class EventDataModule: IModule, ICollectionModule
	{
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

		// Official last id is around 90_000_000 ; so let's give it some breath space, as we still have a lot of space
		// between 200M and 2B
		private int NextId = 200_000_000;

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

		public void Setup()
		{
			foreach (var item in this.EventConfigs)
			{
				if (this.NextId == int.MaxValue) {
					Console.WriteLine($"Can't register more Events. Limit reached. Skipping \"{item.EventModId}\"");
					continue;
				}

				this.Events.Add(this.NextId, item.ToEventMasterModel(this.NextId));
				this.NextId++;
			}
		}
	}
}
