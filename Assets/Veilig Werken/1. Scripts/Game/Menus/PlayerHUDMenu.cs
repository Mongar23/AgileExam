using MBevers;
using MBevers.Menus;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Debug = System.Diagnostics.Debug;

namespace VeiligWerken.Menus
{
	/// <summary>
	///     <para>Created by Mathias on 12-05-2021</para>
	/// </summary>
	public class PlayerHUDMenu : Menu
	{
		private Button openMapButton = null;
		private GameObject infoMessage = null;
		private RectTransform compassArrow = null;
		private TextMeshProUGUI infoMessageText = null;

		protected override void Start()
		{
			IsHUD = true;

			base.Start();

			compassArrow = Content.FindInAllChildren("Arrow") as RectTransform;
			Debug.Assert(compassArrow != null, "compassArrow is null");
			compassArrow.rotation = Quaternion.Euler(Vector3.back * GameManager.Instance.WindDirection);

			openMapButton = Content.GetComponentInChildren<Button>();
			openMapButton.onClick.AddListener(() => MenuManager.Instance.OpenMenu<FloorPlanMenu>());
			openMapButton.gameObject.SetActive(false);

			infoMessage = FindInAllChildren("InfoMessage").gameObject;
			infoMessageText = infoMessage.GetComponentInChildren<TextMeshProUGUI>();
			infoMessageText.SetText("The next questions will not be based on this alarm!".Color("#FFEDA4"));

			MenuManager.Instance.GetMenu<QuizMenu>().QuizCompletedEvent += OnQuizCompleted;
			AudioManager.Instance.AlarmSequenceDoneEvent += OnAlarmSequenceDone;
		}

		protected override bool CanBeOpened() => !MenuManager.Instance.IsAnyOpen;
		protected override bool CanBeClosed() => true;

		private void OnAlarmSequenceDone()
		{
			AudioManager.Instance.AlarmSequenceDoneEvent -= OnAlarmSequenceDone;
			infoMessage.SetActive(false);
		}

		private void OnQuizCompleted() { openMapButton.gameObject.SetActive(true); }
	}
}