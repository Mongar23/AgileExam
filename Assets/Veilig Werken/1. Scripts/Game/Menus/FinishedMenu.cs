using MBevers;
using MBevers.Menus;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using VeiligWerken.Events;
using Menu = MBevers.Menus.Menu;

namespace VeiligWerken.Menus
{
	/// <summary>
	///     This <see cref="MBevers.Menus.Menu" /> is opened when the game is finished.
	///     <para>Created by Mathias on 17-05-2021</para>
	/// </summary>
	public class FinishedMenu : Menu
	{
		private const int CORRECT_ANSWERS_THRESHOLD = 3;

		[SerializeField, Required] private Button replayButton;
		[SerializeField, Required] private Button quitButton;
		[SerializeField, Required] private TextMeshProUGUI resultText;

		public PlayerEnteredShelterEvent PlayerEnteredShelterEvent { get; } = new PlayerEnteredShelterEvent();

		protected override void Start()
		{
			base.Start();

			PlayerEnteredShelterEvent.AddListener(OnPlayerEnteredShelter);
		}

		protected override bool CanBeOpened() => !MenuManager.Instance.IsAnyOpen;
		protected override bool CanBeClosed() => true;

		private void OnPlayerEnteredShelter(bool isCorrectShelter)
		{
			resultText.SetText(GenerateResultText(isCorrectShelter));

			foreach (Button button in GetComponentsInChildren<Button>()) { AudioManager.Instance.Play("UI Click"); }

			replayButton.onClick.AddListener(() => { SceneManager.LoadScene(SceneManager.GetActiveScene().name); });
#if UNITY_EDITOR
			quitButton.onClick.AddListener(() => { EditorApplication.isPlaying = false; });
#else
            quitButton.onClick.AddListener(() => { SceneManager.LoadScene(0); });
#endif
		}

		private static string GenerateResultText(bool isCorrectShelter)
		{
			bool hasPassedTest = isCorrectShelter && GameManager.Instance.CorrectAnsweredQuestions >= CORRECT_ANSWERS_THRESHOLD;

			var shelterResult = $"You have reached the {(isCorrectShelter ? "correct".Color("#DCFFDC") : "incorrect".Color("#FFDCDC"))} shelter. ";
			var questionsResult = $"You have answered {GameManager.Instance.CorrectAnsweredQuestions.ToString().Bold()} correctly.";
			var overallResult = $"\n\nYou have {(hasPassedTest ? "passed".Color(Color.green) : "failed".Color(Color.red))} the test.";

			return shelterResult + questionsResult + overallResult;
		}
	}
}