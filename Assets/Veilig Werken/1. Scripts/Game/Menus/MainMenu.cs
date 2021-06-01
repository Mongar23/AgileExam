using System.Collections;
using MBevers;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using VeiligWerken;

public class MainMenu : ExtendedMonoBehaviour
{
    [SerializeField, Required] private Transform mainMenuPanel;
    private Image loadingBarImage = null;

    private void Awake()
    {
        // Setting all the text from the project info.
        mainMenuPanel.Find("Title").GetComponent<TextMeshProUGUI>().SetText(Application.productName.SplitCamelCase());
        mainMenuPanel.Find("Subtitle").GetComponent<TextMeshProUGUI>().SetText($"Made by: {Application.companyName}");
        mainMenuPanel.Find("Version").GetComponent<TextMeshProUGUI>().SetText($"v{Application.version}");

        // Assigning all the button click-events.
        mainMenuPanel.FindInAllChildren("StartButton").GetComponent<Button>().onClick.AddListener(() => StartCoroutine(StartGame()));
        mainMenuPanel.FindInAllChildren("QuitButton").GetComponent<Button>().onClick.AddListener(Application.Quit);
        foreach (Button button in FindObjectsOfType<Button>()) { button.onClick.AddListener(() => AudioManager.Instance.Play("UI Click")); }
        
        // Setting the non serialized to the editor variables.
        loadingBarImage = mainMenuPanel.Find("LoadingBar").GetComponent<Image>();
        
        // Disable some game objects.
        CachedTransform.Find("ControlsMenu").gameObject.SetActive(false);
        loadingBarImage.gameObject.SetActive(false);
    }

    private IEnumerator StartGame()
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(1);
        loadingBarImage.gameObject.SetActive(true);
        
        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / 0.9f);
            loadingBarImage.fillAmount = progress;
            yield return null;
        }
    }
}