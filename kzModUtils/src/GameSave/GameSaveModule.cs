using HarmonyLib;
using System.Collections.Generic;

namespace kzModUtils.GameSave
{
	internal class GameSaveModule: IModule
	{
		#region Singleton Setup
		private static GameSaveModule mInstance;

		internal static GameSaveModule Instance
		{
			get
			{
				if (mInstance == null)
					mInstance = new GameSaveModule();

				return mInstance;
			}
			private set { }
		}
		#endregion

		internal List<ISaveDataHandler> SaveHandlers = new List<ISaveDataHandler>();

		public void Initialize()
		{
			Harmony.CreateAndPatchAll(typeof(PatchLoadGame));
			Harmony.CreateAndPatchAll(typeof(PatchSaveGame));
		}

		public void Teardown()
		{

		}
	}
}
