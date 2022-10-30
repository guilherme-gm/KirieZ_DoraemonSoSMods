using System;
using UnityEngine;

namespace kzModUtils.UI.UIElementBuilder
{
	public abstract class BaseUIElementBuilder<ElementClass, BuilderClass>
		where ElementClass : UnityEngine.UI.Graphic
		where BuilderClass : BaseUIElementBuilder<ElementClass, BuilderClass>
	{
		protected UIElementType ElementType;

		protected Transform Parent = null;

		protected Vector3 Position = Vector3.zero;

		protected Vector2 Size = Vector2.zero;

		public BaseUIElementBuilder(UIElementType elementType)
		{
			ElementType = elementType;
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

		public virtual ElementClass Build(Transform parent = null)
		{
			if (!UIModule.UIPrefabs.TryGetValue(ElementType, out GameObject prefab))
			{
				Console.WriteLine($"{ElementType}: prefab not found.");
				return default;
			}

			Transform parentTransform = (parent != null ? parent : Parent);

			GameObject uiObject;
			if (parentTransform != null)
				uiObject = GameObject.Instantiate(prefab, parentTransform);
			else
				uiObject = GameObject.Instantiate(prefab);

			var uiElement = uiObject.GetComponent<ElementClass>();

			if (Position != Vector3.zero)
				uiElement.rectTransform.anchoredPosition = Position;

			if (Size != Vector2.zero)
				uiElement.rectTransform.sizeDelta = Size;

			return uiElement;
		}
	}
}
