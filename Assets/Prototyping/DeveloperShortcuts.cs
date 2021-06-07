using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using VeiligWerken;
using VeiligWerken.Prototyping;

namespace Prototyping
{
#if UNITY_EDITOR
	public class DeveloperShortcuts : MonoBehaviour
	{
		private readonly Vector2 buttonSize = new Vector2(150, 25);

		private void OnGUI()
		{
			if (GUI.Button(new Rect(new Vector2(10, Screen.height - buttonSize.y * 0.1f - buttonSize.y), buttonSize), "Reload scene"))
			{
				SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
			}

			if (GUI.Button(new Rect(new Vector2(10, Screen.height - buttonSize.y * 1.15f - buttonSize.y), buttonSize), "Skip alarm"))
			{
				AudioManager.Instance.StopAllCoroutines();
				AudioManager.Instance.AlarmSequenceDoneEvent.Invoke();
			}

			if (GUI.Button(new Rect(new Vector2(10, Screen.height - buttonSize.y * 2.2f - buttonSize.y), buttonSize), "Load json"))
			{
				var alarms = JSONManager.Instance.ReadFromJSON<Dictionary<string, Alarm>>(Application.persistentDataPath + @"\alarms.json");
				foreach (KeyValuePair<string, Alarm> kvp in alarms) { Debug.Log($"{kvp.Key}: {kvp.Value}"); }
			}
		}
	}
#endif
}