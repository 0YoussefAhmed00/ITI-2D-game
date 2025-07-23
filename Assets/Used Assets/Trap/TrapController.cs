using UnityEngine;

public class TrapController : MonoBehaviour
{
    [Header("Trap Settings")]
    public Animator trapAnimator;
    public float deathDelay = 0.5f; // Set this to match your trap animation duration

    private bool triggered = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!triggered && other.CompareTag("Player"))
        {
            triggered = true;
            PlayTrapAnimation();

            PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                StartCoroutine(KillPlayerAfterDelay(playerHealth));
            }
        }
    }

    private void PlayTrapAnimation()
    {
        if (trapAnimator != null)
        {
            trapAnimator.SetTrigger("Active");
        }
    }

    private System.Collections.IEnumerator KillPlayerAfterDelay(PlayerHealth playerHealth)
    {
        yield return new WaitForSeconds(deathDelay);
        playerHealth.KillPlayer();
    }
}
