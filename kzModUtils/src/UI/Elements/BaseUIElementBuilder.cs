using System;
using UnityEngine;

namespace kzModUtils.UI.Elements
{
	public abstract class BaseUIElementBuilder<BuilderClass>
		where BuilderClass : BaseUIElementBuilder<BuilderClass>
	{
		protected ElementType Type;

		protected Transform Parent = null;

		protected Vector3 Position = Vector3.zero;

		protected Vector2 Size = Vector2.zero;

		public BaseUIElementBuilder(ElementType elementType)
		{
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
			if (!UIModule.UIPrefabs.TryGetValue(Type, out GameObject prefab))
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

			var uiElement = uiObject.GetComponent<UnityEngine.UI.Graphic>();

			if (Position != Vector3.zero)
				uiElement.rectTransform.anchoredPosition = Position;

			if (Size != Vector2.zero)
				uiElement.rectTransform.sizeDelta = Size;

			return uiObject;
		}
	}
}
