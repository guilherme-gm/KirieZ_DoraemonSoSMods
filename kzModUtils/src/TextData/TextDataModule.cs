using HarmonyLib;
using System;
using System.Collections.Generic;
using static kzModUtils.TextData.TextHelper;

namespace kzModUtils.TextData
{
	internal class TextDataModule: IModule, ICollectionModule
	{
		internal class PendingTextRegister
		{
			public string Text;
			public OnTextRegistered Callback;
		}

		#region Singleton Setup
		private static TextDataModule mInstance;

		internal static TextDataModule Instance
		{
			get {
				if (mInstance == null)
					mInstance = new TextDataModule();

				return mInstance;
			}
			private set {}
		}
		#endregion

		internal List<PendingTextRegister> TextToRegister = new List<PendingTextRegister>();

		internal Dictionary<int, TextMasterModel> TextCollection;

		private int LastRegisteredTextId = 0;

		public void Initialize()
		{
			Harmony.CreateAndPatchAll(typeof(TextDataModule));
		}

		public void Teardown()
		{
		}

		[HarmonyPatch(typeof(TextMasterCollection), "Setup")]
		[HarmonyPostfix]
		static void OnOriginalSetup(Dictionary<int, TextMasterModel> ___mTexts)
		{
			TextDataModule.Instance.TextCollection = ___mTexts;
		}

		public void Setup(ModDataSavedState state = null)
		{
			foreach (var item in TextToRegister)
			{
				if (LastRegisteredTextId >= int.MaxValue) {
					Console.WriteLine("Can not register more texts. Limit reached.");
					return;
				}

				var nextId = LastRegisteredTextId + 1;
				while (nextId < int.MaxValue && TextCollection.ContainsKey(nextId))
					nextId++;

				LastRegisteredTextId = nextId;
				if (nextId >= int.MaxValue) {
					Console.WriteLine("Can not register more texts. Limit reached.");
					return;
				}

				TextCollection.Add(nextId, new TextMasterModel(new CItemText.SItemText() {
					mId = nextId,
					mTextData = item.Text,
				}));

				item.Callback?.Invoke(nextId);
			}
		}
	}
}
