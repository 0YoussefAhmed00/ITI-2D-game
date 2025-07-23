using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    [Header("Death Settings")]
    public float deathDelay = 2f;

    private Animator animator;
    private bool isDead = false;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void KillPlayer()
    {
        if (isDead) return;

        isDead = true;

        // Disable player movement
        PlayerMovement movement = GetComponent<PlayerMovement>();
        if (movement != null) movement.enabled = false;

        // Disable player input
        PlayerInput input = GetComponent<PlayerInput>();
        if (input != null) input.enabled = false;

        // Play death animation
        if (animator != null)
        {
            animator.SetTrigger("Die");
        }

        // Load main menu after delay
        Invoke("LoadMainMenu", deathDelay);
    }

    private void LoadMainMenu()
    {
        SceneManager.LoadScene("MainMenu"); // Replace with your main menu scene name
    }
}
