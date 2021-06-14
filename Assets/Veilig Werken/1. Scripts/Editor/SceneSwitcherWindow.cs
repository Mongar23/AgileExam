using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace VeiligWerken.Editor
{
	public class SceneSwitcherWindow : EditorWindow
	{
		public enum ScenesSource
		{
			Assets,
			BuildSettings
		}

		protected OpenSceneMode openSceneMode = OpenSceneMode.Single;
		protected ScenesSource scenesSource = ScenesSource.Assets;

		protected Vector2 scrollPosition;

		[MenuItem("Tools/Scene Switcher")]
		public static void Init()
		{
			var window = GetWindow<SceneSwitcherWindow>("Scene Switcher");
			window.Show();
		}

		public virtual void Open(string path)
		{
			if (!EditorSceneManager.EnsureUntitledSceneHasBeenSaved("You don't have saved the Untitled Scene, Do you want to leave?")) { return; }

			EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo();
			EditorSceneManager.OpenScene(path, openSceneMode);
		}

		protected virtual void OnGUI()
		{
			var buildScenes = new List<EditorBuildSettingsScene>(EditorBuildSettings.scenes);
			scenesSource = (ScenesSource)EditorGUILayout.EnumPopup("Scenes Source", scenesSource);
			openSceneMode = (OpenSceneMode)EditorGUILayout.EnumPopup("Open Scene Mode", openSceneMode);
			GUILayout.Label("Scenes", EditorStyles.boldLabel);
			string[] guids = AssetDatabase.FindAssets("t:Scene");
			scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);
			EditorGUILayout.BeginVertical();
			foreach (string t in guids)
			{
				string path = AssetDatabase.GUIDToAssetPath(t);
				var sceneAsset = AssetDatabase.LoadAssetAtPath<SceneAsset>(path);
				EditorBuildSettingsScene buildScene = buildScenes.Find(editorBuildScene => editorBuildScene.path == path);
				Scene scene = SceneManager.GetSceneByPath(path);
				bool isOpen = scene.IsValid() && scene.isLoaded;
				GUI.enabled = !isOpen;
				if (scenesSource == ScenesSource.Assets)
				{
					if (GUILayout.Button(sceneAsset.name)) { Open(path); }
				}
				else
				{
					if (buildScene != null)
					{
						if (GUILayout.Button(sceneAsset.name)) { Open(path); }
					}
				}

				GUI.enabled = true;
			}

			if (GUILayout.Button("Create New Scene"))
			{
				Scene newScene = EditorSceneManager.NewScene(NewSceneSetup.DefaultGameObjects, NewSceneMode.Single);
				EditorSceneManager.SaveScene(newScene);
			}

			EditorGUILayout.EndVertical();
			EditorGUILayout.EndScrollView();
		}
	}
}