using Fishbook.Entities;
using System.IO;
using kzModUtils.GameSave;

#nullable enable

namespace Fishbook
{
	internal class FishbookSaveDataHandler : ISaveDataHandler
	{
		public string GetSuffix()
		{
			return "fishbook";
		}

		public void LoadGameData(byte[]? buffer)
		{
			if (buffer == null) {
				FishbookBook.Instance.Initialize();
				return;
			}

			FishbookSaveState saveState = new FishbookSaveState();

			using (var stream = new MemoryStream(buffer)) {
				using (BinaryReader reader = new BinaryReader(stream)) {
					var stateCount = reader.ReadInt32();
					for (var i = 0; i < stateCount; i++) {
						var state = new FishbookSaveState.FishbookSaveStateItem();
						saveState.State.Add(reader.ReadString(), state);

						state.FoundOnce = reader.ReadBoolean();

						var conditionCount = reader.ReadInt32();
						for (var j = 0; j < conditionCount; j++) {
							var conditionId = reader.ReadInt32();
							var conditionValue = (FishPointCondtion) reader.ReadInt32();

							state.Conditions.Add(conditionId, conditionValue);
						}
					}
				}
			}

			FishbookBook.Instance.Initialize(saveState);
		}

		public byte[] SaveGameData()
		{
			var state = FishbookBook.Instance.GetSaveState();

			using (var stream = new MemoryStream())
			{
				using (var writer = new BinaryWriter(stream))
				{
					writer.Write(state.State.Count);
					foreach (var item in state.State)
					{
						var stateId = item.Key;
						var stateValue = item.Value;

						writer.Write(stateId);
						writer.Write(stateValue.FoundOnce);
						writer.Write(stateValue.Conditions.Count);
						foreach (var cond in stateValue.Conditions)
						{
							writer.Write(cond.Key);
							writer.Write((int)cond.Value);
						}
					}
				}

				return stream.ToArray();
			}
		}
	}
}
