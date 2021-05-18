using MBevers;
using MBevers.Menus;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Menu = MBevers.Menus.Menu;

namespace VeiligWerken.Menus
{
    /// <summary>
    ///     This <see cref="MBevers.Menus.Menu" /> is opened when the game is finished.
    ///     <para>Created by Mathias on 17-05-2021</para>
    /// </summary>
    public class FinishedMenu : Menu
    {
        [SerializeField] [Required] private Button replayButton;
        [SerializeField] [Required] private Button quitButton;

        protected override void Start()
        {
            base.Start();

            replayButton.onClick.AddListener(() =>
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
                AudioManager.Instance.Play("UI Click");
            });
#if UNITY_EDITOR
            quitButton.onClick.AddListener(() =>
            {
                EditorApplication.isPlaying = false;
                AudioManager.Instance.Play("UI Click");
            });
#else
            quitButton.onClick.AddListener(() =>
            {
                Application.Quit();
                AudioManager.Instance.Play("UI Click");
            });
#endif
        }

        public override bool CanBeOpened() => !MenuManager.Instance.IsAnyOpen;
        public override bool CanBeClosed() => true;
    }
}