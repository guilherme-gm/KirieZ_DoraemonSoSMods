using kzModUtils.Resource;
using kzModUtils.UI.Elements;
using kzModUtils.UI.Events;
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

		internal static Dictionary<UISprite, Sprite> Sprites { get; private set; } = new Dictionary<UISprite, Sprite>();

		private static List<Texture2D> LoadedTextures = new List<Texture2D>();

		internal static void Initialize()
		{
			LoadSprites();
			MessageBoxBuilder.SetupStyles();

			UIUtils.PreOnTitleUIReady += (object sender, TitleUIReadyEventArgs args) => {
				LoadElements();
			};
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

		internal static void RunLoader(IElementLoader loader)
		{
			try {
				var result = loader.Load();
				if (result == null)
					return;

				foreach (var kind in result.Keys)
				{
					var obj = result.GetValue(kind);
					obj.SetActive(false);

					Prefabs.Add(kind, obj);
				}
			} catch (Exception error) {
				PluginLogger.LogError($"UIAssets::RunLoader: Failed to load '{loader.GetType()}'. Error: {error}");
			}
		}

		internal static void LoadElements()
		{
			UIElementsAsset = AssetBundle.LoadFromFile(ResourceUtils.GetAssetBundlePath(ResourceType.Plugin, "kzModUtils/UIElements", ""));
			Prefabs.Add(ElementType.BackgroundImage, UIElementsAsset.LoadAsset<GameObject>("BackgroundImage"));
			Prefabs.Add(ElementType.Text, UIElementsAsset.LoadAsset<GameObject>("TextElement"));
			Prefabs.Add(ElementType.MessageBox, UIElementsAsset.LoadAsset<GameObject>("MessageBox"));

			RunLoader(new GameMenuPartsLoader()); // ScrollableArea
			RunLoader(new HorizontalMenuBoxLoader()); // HorizontalMenuBox
			RunLoader(new ToggleGroupLoader()); // ToggleGroup
		}

		internal static void LoadSprites()
		{
			Dictionary<string, UISprite> spritesToLoad = new Dictionary<string, UISprite>()
			{
				{ "Icon_Crown_01", UISprite.Icon_Crown_01 },
				{ "Square_R_02", UISprite.Square_R_02 },
				{ "Square_R_05", UISprite.Square_R_05 },
			};

			var uiAtlasAsset = AssetBundle.LoadFromFile(
				ResourceUtils.GetAssetBundlePath(ResourceType.StreamingAsset, $"/atlas/atlas_{Define.Sprite.UI_ATLAS_ID}")
			);
			if (uiAtlasAsset == null)
				throw new Exception("UI Atlas asset not found.");

			var uiAtlas = uiAtlasAsset.LoadAsset<SpriteAtlas>($"assets/resources/atlas/atlas_{Define.Sprite.UI_ATLAS_ID}.spriteatlas");
			if (uiAtlas == null)
				throw new Exception("UI Sprite Atlas not found.");

			Sprite[] sprites = new Sprite[uiAtlas.spriteCount];
			uiAtlas.GetSprites(sprites);

			for (int i = 0; i < sprites.Length; i++)
			{
				if (sprites[i] == null)
					continue;

				var name = sprites[i].name.Replace("(Clone)", "");
				if (spritesToLoad.TryGetValue(name, out var spriteId))
				{
					// We have to create a copy of the texture and sprite so we
					// can free the original asset afterwards
					Sprite originalSprite = UnityEngine.Object.Instantiate(sprites[i]);
					var originalTexture = originalSprite.texture;

					RenderTexture tmp = RenderTexture.GetTemporary(originalTexture.width, originalTexture.height, 0, RenderTextureFormat.Default, RenderTextureReadWrite.Linear);
					Graphics.Blit(originalTexture, tmp);
					RenderTexture originalRender = RenderTexture.active;
					RenderTexture.active = tmp;

					// ReadPixels consider bottom-left as (0,0) , while textureRect considers top-left as (0,0), so we have to convert
					// y value.
					Texture2D spriteTexture = new Texture2D((int) originalSprite.textureRect.width, (int) originalSprite.textureRect.height);
					spriteTexture.ReadPixels(new Rect(originalSprite.textureRect.x, originalTexture.height - (int) originalSprite.textureRect.y - (int) originalSprite.textureRect.height, (int) originalSprite.textureRect.width, (int) originalSprite.textureRect.height), 0, 0);
					spriteTexture.Apply();

					RenderTexture.active = originalRender;
					RenderTexture.ReleaseTemporary(tmp);

					var finalSprite = Sprite.Create(spriteTexture, new Rect(0, 0, originalSprite.textureRect.width, originalSprite.textureRect.height), originalSprite.pivot, originalSprite.pixelsPerUnit, 0, SpriteMeshType.Tight, originalSprite.border);

					Sprites.Add(spriteId, finalSprite);
					LoadedTextures.Add(spriteTexture);
				}
			}

			uiAtlasAsset.Unload(true);
		}
	}
}
