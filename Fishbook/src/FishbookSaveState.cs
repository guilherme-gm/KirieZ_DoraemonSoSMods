using Fishbook.Entities;
using System.Collections.Generic;
using System;

namespace Fishbook
{
	[Serializable]
	public class FishbookSaveState
	{
		[Serializable]
		public class FishbookSaveStateItem
		{
			public bool FoundOnce;

			public Dictionary<int, FishPointCondtion> Conditions { get; set; }

			public FishbookSaveStateItem()
			{
				this.Conditions = new Dictionary<int, FishPointCondtion>();
			}
		}

		public Dictionary<string, FishbookSaveStateItem> State { get; set; }

		public FishbookSaveState() {
			this.State = new Dictionary<string, FishbookSaveStateItem>();
		}
	}
}
