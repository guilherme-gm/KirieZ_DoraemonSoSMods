using UnityEngine;

#nullable enable

namespace kzModUtils.Resource
{
	public class SpriteConfig
	{
		public Sprite? Sprite { get; internal set; }

		public int AtlasId { get; internal set; }

		public int SpriteId { get; internal set; }

		public SpriteConfig(int atlasId, int spriteId)
		{
			this.AtlasId = atlasId;
			this.SpriteId = spriteId;
		}

		public SpriteConfig(Sprite sprite)
		{
			this.Sprite = sprite;
		}
	}
}
