using VeiligWerken;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Prototyping
{
#if UNITY_EDITOR
    public class DeveloperShortcuts : MonoBehaviour
    {
        private readonly Vector2 buttonSize = new Vector2(150, 25);

        private void OnGUI()
        {
            if(GUI.Button(new Rect(new Vector2(10, Screen.height - buttonSize.y * 0.1f - buttonSize.y), buttonSize), "Reload scene"))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }
    }
#endif
}