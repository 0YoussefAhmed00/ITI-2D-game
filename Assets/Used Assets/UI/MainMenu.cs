using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [Header("Menu Buttons")]
    [SerializeField] private Button startButton;
    [SerializeField] private Button controlsButton;
    [SerializeField] private Button exitButton;

    [Header("Controls Screen")]
    [SerializeField] private GameObject menuPanel;       // Container for Start/Controls/Exit
    [SerializeField] private GameObject controlsPanel;   // Your “Controls” UI
    [SerializeField] private Button backButton;          // “Back” button on the controls screen

    private void Start()
    {
        // Wire up menu buttons
        startButton.onClick.AddListener(OnStartClicked);
        controlsButton.onClick.AddListener(ShowControls);
        exitButton.onClick.AddListener(OnExitClicked);

        // Wire up back button
        backButton.onClick.AddListener(ShowMenu);

        // Initial state: menu visible, controls hidden
        menuPanel.SetActive(true);
        controlsPanel.SetActive(false);
    }

    private void Update()
    {
        // If controls screen is open, allow Esc to go back
        if (controlsPanel.activeSelf && Input.GetKeyDown(KeyCode.Escape))
        {
            ShowMenu();
        }
    }

    private void OnStartClicked()
    {
        if (SceneExists("Level1"))
            SceneManager.LoadScene("Level1");
        // else silently do nothing (or show your own UI warning)
    }

    private void OnExitClicked()
    {
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }

    private void ShowControls()
    {
        menuPanel.SetActive(false);
        controlsPanel.SetActive(true);
    }

    private void ShowMenu()
    {
        controlsPanel.SetActive(false);
        menuPanel.SetActive(true);
    }

    // Helper to check if a scene is in Build Settings
    private bool SceneExists(string sceneName)
    {
        for (int i = 0; i < SceneManager.sceneCountInBuildSettings; i++)
        {
            string path = SceneUtility.GetScenePathByBuildIndex(i);
            string name = System.IO.Path.GetFileNameWithoutExtension(path);
            if (name == sceneName)
                return true;
        }
        return false;
    }
}
