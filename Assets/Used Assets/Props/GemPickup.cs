using UnityEngine;
using UnityEngine.SceneManagement;

public class GemPickup : MonoBehaviour
{
    public GameObject winUI;               // Assign your UI object in the Inspector
    public float delayBeforeMainMenu = 2f; // Delay before returning to menu

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (winUI != null)
                winUI.SetActive(true);

            Destroy(other.gameObject); // Optional: remove player

            // Delay before going to main menu
            Invoke(nameof(GoToMainMenu), delayBeforeMainMenu);
        }
    }

    private void GoToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
