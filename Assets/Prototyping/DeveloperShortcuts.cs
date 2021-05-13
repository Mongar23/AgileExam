using UnityEngine;
using UnityEngine.SceneManagement;

namespace Prototyping
{
#if UNITY_EDITOR
    public class DeveloperShortcuts : MonoBehaviour
    {
        private readonly Vector2 buttonSize = new Vector2(100, 25);

        private void OnGUI()
        {
            if(GUI.Button(new Rect(10, Screen.height - buttonSize.y * 0.1f - buttonSize.y, buttonSize.x, buttonSize.y), "Reload scene")) { SceneManager.LoadScene(1); }
        }
    }
#endif
}