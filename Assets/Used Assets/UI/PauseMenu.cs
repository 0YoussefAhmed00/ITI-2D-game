using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenuUI;
    private bool isPaused = false;
    private PlayerInput playerInput;

    private void Start()
    {
        // Find player input
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerInput = player.GetComponent<PlayerInput>();
        }

        // Ensure pause menu is hidden at start
        if (pauseMenuUI != null)
        {
            pauseMenuUI.SetActive(false);
        }
    }

    private void Update()
    {
        // Only process pause input when not in chat
        if (!WiseNPCInteractable.IsNPCUIOpen && Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            if (isPaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }

    public void PauseGame()
    {
        isPaused = true;
        Time.timeScale = 0f; // Freeze game time

        if (pauseMenuUI != null)
        {
            pauseMenuUI.SetActive(true);
        }

        // Disable player input
        if (playerInput != null)
        {
            playerInput.enabled = false;
        }
    }

    public void ResumeGame()
    {
        isPaused = false;
        Time.timeScale = 1f; // Resume game time

        if (pauseMenuUI != null)
        {
            pauseMenuUI.SetActive(false);
        }

        // Re-enable player input
        if (playerInput != null)
        {
            playerInput.enabled = true;
        }
    }

    public void ExitToMainMenu()
    {
        Time.timeScale = 1f; // Ensure time is reset
        SceneManager.LoadScene("MainMenu"); // Replace with your main menu scene name
    }

    public void RetryLevel()
    {
        Time.timeScale = 1f; // Ensure time is reset
        SceneManager.LoadScene("Level1");  // Reload the Level1 scene
    }
}
