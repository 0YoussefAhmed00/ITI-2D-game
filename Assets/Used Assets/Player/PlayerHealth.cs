using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    [Header("Death Settings")]
    public float deathDelay = 1f;

    private Animator animator;
    private AudioSource audioSource;
    private bool isDead = false;

    private PlayerMovement movement;
    private PlayerInput input;

    private void Start()
    {
        animator    = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        movement    = GetComponent<PlayerMovement>();
        input       = GetComponent<PlayerInput>();
    }

    public void KillPlayer()
    {
        if (isDead) return;
        isDead = true;

        // 1) Disable movement & input immediately
        if (movement != null) movement.enabled = false;
        if (input    != null) input.enabled = false;

        // 2) Play death sound (via the AudioSource on the player)
        if (audioSource != null)
            audioSource.Play();

        // 3) Flip on your “Death” bool so the Animator transitions to your death clip
        if (animator != null)
            animator.SetBool("Death", true);

        // 4) After the delay, load MainMenu
        Invoke(nameof(LoadMainMenu), deathDelay);
    }

    private void LoadMainMenu()
    {
        SceneManager.LoadScene("Level1");
    }
}
