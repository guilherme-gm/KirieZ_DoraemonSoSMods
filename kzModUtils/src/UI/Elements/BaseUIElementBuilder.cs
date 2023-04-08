using System;
using UnityEngine;

namespace kzModUtils.UI.Elements
{
	public abstract class BaseUIElementBuilder<BuilderClass>
		where BuilderClass : BaseUIElementBuilder<BuilderClass>
	{
		protected ElementType Type;

		public Transform Parent { get; set; }

		protected Vector3 _Position = Vector3.zero;

		public Vector3 Position {
			get { return _Position; }
			set { _Position = this.ConvertPosition(value); }
		}

		public Vector2 Size { get; set; }

		public BaseUIElementBuilder(ElementType elementType)
		{
			this.Size = Vector2.zero;
			this.Parent = null;
			Type = elementType;
		}

		protected Vector3 ConvertPosition(Vector3 pos)
		{
			return new Vector3(pos.x, -1 * pos.y, pos.z);
		}

		public BuilderClass SetPosition(Vector3 position)
		{
			Position = this.ConvertPosition(position);
			return this as BuilderClass;
		}

		public BuilderClass SetSize(Vector2 size)
		{
			Size = size;
			return this as BuilderClass;
		}

		public BuilderClass SetParent(Transform parent)
		{
			Parent = parent;
			return this as BuilderClass;
		}

		public BuilderClass SetCanvasAsParent()
		{
			if (UIUtils.CommonUICanvas == null)
			{
				throw new Exception("Can't create element without parent before Common Canvas is created.");
			}

			Parent = UIUtils.CommonUICanvas.transform;
			return this as BuilderClass;
		}

		protected virtual GameObject BuildBase(Transform parent = null)
		{
			if (!UIAssets.Prefabs.TryGetValue(Type, out GameObject prefab))
			{
				Console.WriteLine($"{Type}: prefab not found.");
				return default;
			}

			Transform parentTransform = (parent != null ? parent : Parent);

			GameObject uiObject;
			if (parentTransform != null)
				uiObject = GameObject.Instantiate(prefab, parentTransform);
			else
				uiObject = GameObject.Instantiate(prefab);

			var rectTransform = uiObject.GetComponent<RectTransform>();

			if (Position != Vector3.zero)
				rectTransform.anchoredPosition = Position;

			if (Size != Vector2.zero)
				rectTransform.sizeDelta = Size;

			return uiObject;
		}
	}
}
