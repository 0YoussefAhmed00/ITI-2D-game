using UnityEngine;
using UnityEngine.SceneManagement;

public class GemPickup : MonoBehaviour
{
    public GameObject winUI;               // Assign your UI object in the Inspector
    public float delayBeforeMainMenu = 5f; // Delay before returning to menu
    public AudioSource pickupSound;        // Drag & assign AudioSource with clip in Inspector

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (pickupSound != null)
                pickupSound.Play(); // Play sound once

            if (winUI != null)
                winUI.SetActive(true);

            Destroy(other.gameObject); // Optional: remove player

            Invoke(nameof(GoToMainMenu), delayBeforeMainMenu);
        }
    }

    private void GoToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
