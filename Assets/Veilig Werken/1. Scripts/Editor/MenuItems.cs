using UnityEditor;
using UnityEngine;

namespace MBevers.Editor
{
	public class MenuItems : MonoBehaviour
	{
		[MenuItem("GameObject/MBevers/UI/Menu", false, 10)]
		private static void CreateMenuObject(MenuCommand menuCommand)
		{
			var menuRoot = new GameObject("Menu");
			GameObjectUtility.SetParentAndAlign(menuRoot, menuCommand.context as GameObject);
			Undo.RegisterCreatedObjectUndo(menuRoot, "Create " + menuRoot.name);
			var rootRectTransform = menuRoot.AddComponent<RectTransform>();
			rootRectTransform.anchorMin = Vector2.zero;
			rootRectTransform.anchorMax = Vector2.one;
			rootRectTransform.sizeDelta = Vector2.zero;

			var menuContent = new GameObject("Content");
			menuContent.transform.SetParent(menuRoot.transform);
			var contentRectTransform = menuContent.AddComponent<RectTransform>();	
			contentRectTransform.anchorMin = Vector2.zero;
			contentRectTransform.anchorMax = Vector2.one;
			contentRectTransform.sizeDelta = Vector2.one;
			contentRectTransform.anchoredPosition = Vector2.zero;
			contentRectTransform.localScale = Vector3.one;
			
			Selection.activeObject = menuRoot;
		}
	}
}