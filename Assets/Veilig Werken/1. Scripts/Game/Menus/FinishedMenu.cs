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
        [SerializeField, Required] private Button replayButton;
        [SerializeField, Required] private Button quitButton;
        [SerializeField, Required] private TextMeshProUGUI finishedText;

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
            finishedText.SetText($"You have reached the {(isCorrectShelter ? "correct".Color(Color.green) : "incorrect".Color(Color.red))} shelter!");

            foreach (Button button in GetComponentsInChildren<Button>()) { AudioManager.Instance.Play("UI Click"); }

            replayButton.onClick.AddListener(() => { SceneManager.LoadScene(SceneManager.GetActiveScene().name); });
#if UNITY_EDITOR
            quitButton.onClick.AddListener(() => { EditorApplication.isPlaying = false; });
#else
            quitButton.onClick.AddListener(() => { Application.Quit(); });
#endif
        }
    }
}