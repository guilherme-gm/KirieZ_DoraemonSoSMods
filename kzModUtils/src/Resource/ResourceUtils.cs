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
	}
}
