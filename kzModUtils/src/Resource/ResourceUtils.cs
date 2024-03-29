using System;
using System.Collections.Generic;
using UnityEngine;

namespace kzModUtils.Resource
{
	/**
	 * Module related to game resources
	 */
	public static class ResourceUtils
	{
		private static readonly string Platform = "Win";

		private static readonly Dictionary<ResourceType, string> ResourcePaths = new Dictionary<ResourceType, string>()
		{
			{ ResourceType.Data, $"{Application.dataPath}/AssetBunde/{Platform}" },
			{ ResourceType.StreamingAsset, $"{Application.streamingAssetsPath}/AssetBundle/{Platform}" },
			{
				ResourceType.Plugin,
				(Application.platform == RuntimePlatform.OSXPlayer
					? $"{Application.dataPath}/../../BepInEx/plugins"
					: (
						Application.platform == RuntimePlatform.WindowsPlayer
							? $"{Application.dataPath}/../BepInEx/plugins"
							: Application.dataPath
					)
				)
			},
		};

		/**
		 * Retrieves text message from text dictionary (TextMaster).
		 */
		public static string GetText(int textId)
		{
			return SingletonMonoBehaviour<MasterManager>.Instance.TextMaster.GetText(textId);
		}

		/**
		 * Loads the sprite for character and calls callback with it
		 *
		 * @TODO: This will need to be reworked if this lib starts supporting custom Characters
		 *
		 * @param character the CharacterID which we want the sprite of
		 * @param callback function to be called when the sprite is returned
		 */
		public static void GetCharacterSprite(Define.Character.Id character, Action<Sprite> callback)
		{
			int spriteId = MasterManager.Instance.CharacterMaster.GetCharacter((int) character).SpriteId;
			SingletonMonoBehaviour<AtlasFactory>.Instance.LoadSprite(Define.Sprite.CHARACTER_ATLAS_ID, spriteId, callback);
		}

		/**
		 * Returns the path to load an AssetBundle.
		 * @param resourceType the kind of resource to load, this will determine the start of the path.
		 *        note that "Plugin" type simply gets you to BepInEx plugins folder.
		 * @param resourceName the name of the resource file to be loaded
		 * @param extension file extension, defaults to "jp"
		 * @returns the path to use to load this asset
		 */
		public static string GetAssetBundlePath(ResourceType resourceType, string resourceName, string extension = "jp")
		{
			string basePath = ResourcePaths[resourceType];

			if (extension.Length > 0)
				return $"{basePath}/{resourceName}.{extension}";

			return $"{basePath}/{resourceName}";
		}

		/**
		 * Registers a sprite into a custom atlas that may be used for mods.
		 *
		 * Sprites registered through this method will be automatically handled when original code calls LoadSprite.
		 * @param sprite the sprite to be registered
		 * @returns a "SpriteConfig" that contains information about the atlas.
		 *          use the provided atlas id / sprite id in your code.
		 */
		public static SpriteConfig RegisterSprite(Sprite sprite)
		{
			return ResourceModule.Instance.RegisterSprite(sprite);
		}
	}
}
