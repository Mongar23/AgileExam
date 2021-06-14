using System.Collections;
using MBevers;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace VeiligWerken.AlarmEditor
{
	/// <summary>
	///     <para>
	///         Created by: Mathias on 6/9/2021.
	///     </para>
	/// </summary>
	public class AlarmEditorGeneralUI : Singleton<AlarmEditorGeneralUI>
	{
		private const float INFO_SHOW_TIME = 5.0f;

		private TextMeshProUGUI infoText = null;

		protected override void Awake()
		{
			base.Awake();

			foreach (Button button in FindObjectsOfType<Button>()) { button.onClick.AddListener(() => AudioManager.Instance.Play("UI Click")); }

			infoText = CachedTransform.Find("InfoText").GetComponent<TextMeshProUGUI>();
			infoText.SetText(string.Empty);

			CachedTransform.Find("BackButton").GetComponent<Button>().onClick.AddListener(() => SceneManager.LoadScene(0));
		}

		public IEnumerator InfoMessageCoroutine(string message)
		{
			// Set the text to the message.
			infoText.SetText(message);

			// Clear the text after 5 seconds.
			yield return new WaitForSeconds(INFO_SHOW_TIME);
			infoText.SetText(string.Empty);
		}
	}
}