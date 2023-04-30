using HarmonyLib;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace kzModUtils.Resource
{
	internal class ResourceModule: IModule
	{
		internal static readonly int ATLAS_ID = 99_999;

		/**
		 * This is where item id starts, as there are some parts of original code that
		 * uses the item id instead of the sprite id, we are duplicating references here
		 * and getting into this range would be dangerous.
		 * IF we ever need to get in there, we should be carefull with load/unload based on save data.
		 */
		internal static readonly int MAX_ID = 10_00000;

		#region Singleton Setup
		private static ResourceModule mInstance;

		internal static ResourceModule Instance
		{
			get {
				if (mInstance == null)
					mInstance = new ResourceModule();

				return mInstance;
			}
			private set {}
		}
		#endregion

		private int NextId = 1;

		private Dictionary<int, Sprite> Atlas = new Dictionary<int, Sprite>();

		public void Initialize()
		{
			Harmony.CreateAndPatchAll(typeof(ResourceModule));
		}

		public void Teardown()
		{
		}

		internal SpriteConfig RegisterSprite(Sprite sprite)
		{
			while (this.NextId < MAX_ID && this.Atlas.ContainsKey(this.NextId))
				this.NextId++;

			if (NextId >= MAX_ID)
			{
				Console.WriteLine("RegisterSprite: Maximum sprites reached.");
				return null;
			}

			var config = new SpriteConfig(sprite) {
				AtlasId = ATLAS_ID,
				SpriteId = NextId,
			};

			this.Atlas.Add(NextId, sprite);
			return config;
		}

		internal bool RegisterSprite(int id, Sprite sprite)
		{
			if (this.Atlas.ContainsKey(id))
			{
				Console.WriteLine($"RegisterSprite: Sprite {id} already exists.");
				return false;
			}

			this.Atlas.Add(id, sprite);
			return true;
		}

		internal void RemoveSprite(int id)
		{
			this.Atlas.Remove(id);
		}

		[HarmonyPatch(typeof(AtlasFactory), "LoadSprite")]
		[HarmonyPrefix]
		static void OnLoadSprite(
			int atlas_id,
			int sprite_id,
			Action<UnityEngine.Sprite> loaded_callback,
			ref bool __runOriginal
		)
		{
			if (atlas_id != ATLAS_ID) {
				return;
			}

			var sprite = ResourceModule.Instance.Atlas.GetValue(sprite_id, null);

			__runOriginal = false;
			loaded_callback(sprite);
		}

	}
}
