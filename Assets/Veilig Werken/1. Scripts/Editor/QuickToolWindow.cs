using UnityEditor;
using UnityEngine;

namespace MBevers.Editor
{
	public class QuickToolWindow : EditorWindow
	{
		private string emptyName;
		private string groupName;

		private Transform transformA;
		private Transform transformB;

		private void OnGUI()
		{
			#region Group Items
			EditorGUILayout.BeginHorizontal();

			groupName = EditorGUILayout.TextField("Group Name", groupName);

			if (GUILayout.Button("Group"))
			{
				var parentObject = new GameObject(groupName);

				if (Selection.activeTransform.parent != null) { parentObject.transform.SetParent(Selection.activeTransform.parent); }

				foreach (GameObject gameObject in Selection.gameObjects) { gameObject.transform.SetParent(parentObject.transform); }

				groupName = string.Empty;
			}

			EditorGUILayout.EndHorizontal();
			#endregion

			#region Create empty
			EditorGUILayout.BeginHorizontal();

			emptyName = EditorGUILayout.TextField("Empty name", emptyName);

			if (GUILayout.Button("Create"))
			{
				var gameObject = new GameObject(emptyName);

				if (Selection.activeTransform != null) { gameObject.transform.SetParent(Selection.activeTransform); }

				emptyName = string.Empty;
			}

			EditorGUILayout.EndHorizontal();
			#endregion

			#region Remove components
			EditorGUILayout.BeginHorizontal();

			if (GUILayout.Button("Remove Components") && Selection.activeTransform != null)
			{
				foreach (Component component in Selection.activeTransform.GetComponents<Component>())
				{
					if (component.GetType() == typeof(Transform)) { continue; }

					DestroyImmediate(component);
				}
			}

			EditorGUILayout.EndHorizontal();
			#endregion

			#region Number objects
			EditorGUILayout.BeginHorizontal();

			if (GUILayout.Button("Number selected objects"))
			{
				GameObject[] gameObjects = Selection.gameObjects;
				for (var i = 0; i < gameObjects.Length; i++)
				{
					GameObject gameObject = gameObjects[i];

					gameObject.name = $"{gameObjects[i].name}_{i}";
					gameObject.transform.SetAsLastSibling();
				}
			}

			EditorGUILayout.EndHorizontal();
			#endregion

			#region Log components
			EditorGUILayout.BeginHorizontal();

			if (GUILayout.Button("Log components on GameObject"))
			{
				foreach (Component component in Selection.activeGameObject.GetComponents<Component>()) { Debug.Log(component.GetType().Name); }
			}

			EditorGUILayout.EndHorizontal();
			#endregion

			#region Calculate distance between trasforms
			EditorGUILayout.BeginHorizontal();

			transformA = EditorGUILayout.ObjectField(transformA, typeof(Transform), true) as Transform;
			transformB = EditorGUILayout.ObjectField(transformB, typeof(Transform), true) as Transform;

			if (GUILayout.Button("Calculate Distance"))
			{
				if (transformA == null || transformB == null)
				{
					Debug.LogError("Make sure both fields have a transform assigned.");
					return;
				}

				Debug.Log($"The distance is {Vector3.Distance(transformA.position, transformB.position)}.");
				transformA = transformB = null;
			}

			EditorGUILayout.EndHorizontal();
			#endregion
		}

		[MenuItem("Window/Mathias' Developer Window")]
		private static void ShowWindow()
		{
			var window = GetWindow<QuickToolWindow>();
			window.titleContent = new GUIContent("Mathias' Dev");
			window.Show();
		}
	}
}