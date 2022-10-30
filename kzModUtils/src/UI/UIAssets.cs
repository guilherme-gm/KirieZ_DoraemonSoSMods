using kzModUtils.Resource;
using kzModUtils.UI.Elements;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

namespace kzModUtils.UI
{
	internal static class UIAssets
	{
		private static AssetBundle UIElementsAsset;

		internal static Dictionary<ElementType, GameObject> Prefabs { get; private set; } = new Dictionary<ElementType, GameObject>();

		internal static void Initialize()
		{
			LoadElements();
		}

		internal static void Teardown()
		{
			foreach (var prefab in Prefabs.Values)
			{
				GameObject.Destroy(prefab);
			}
			Prefabs = new Dictionary<ElementType, GameObject>();

			UIElementsAsset.Unload(true);
		}

		internal static void LoadElements()
		{
			UIElementsAsset = AssetBundle.LoadFromFile(ResourceUtils.GetAssetBundlePath(ResourceType.Plugin, "kzModUtils/UIElements", ""));
			Prefabs.Add(ElementType.BackgroundImage, UIElementsAsset.LoadAsset<GameObject>("BackgroundImage"));
			Prefabs.Add(ElementType.Text, UIElementsAsset.LoadAsset<GameObject>("TextElement"));
		}

	}
}
