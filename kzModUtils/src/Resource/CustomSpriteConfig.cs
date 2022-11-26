using UnityEngine;

#nullable enable

namespace kzModUtils.Resource
{
	public class CustomSpriteConfig
	{
		public Sprite? Sprite { get; internal set; }

		public int AtlasId { get; internal set; }

		public int SpriteId { get; internal set; }

		public CustomSpriteConfig(int atlasId, int spriteId)
		{
			this.AtlasId = atlasId;
			this.SpriteId = spriteId;
		}

		public CustomSpriteConfig(Sprite sprite)
		{
			this.Sprite = sprite;
		}
	}
}
