using System.Collections.Generic;
using UnityEngine;

namespace kzModUtils.UI.Elements
{
	interface IElementLoader {
		internal Dictionary<ElementType, GameObject> Load();
	}
}
