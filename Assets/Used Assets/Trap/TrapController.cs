using UnityEngine;

public class TrapController : MonoBehaviour
{
    [Header("Trap Settings")]
    public Animator trapAnimator;
    public float deathDelay = 0.5f; // Set this to match your trap animation duration

    private bool triggered = false;
    private Rigidbody2D trapRigidbody;
    private int nonPlayerContacts = 0;

    private void Start()
    {
        trapRigidbody = GetComponent<Rigidbody2D>();
        if (trapRigidbody != null)
            trapRigidbody.isKinematic = true;
    }

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
        else if (!other.CompareTag("Player") && trapRigidbody != null)
        {
            nonPlayerContacts++;
            trapRigidbody.isKinematic = false;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Player") && trapRigidbody != null)
        {
            nonPlayerContacts = Mathf.Max(0, nonPlayerContacts - 1);
            if (nonPlayerContacts == 0)
            {
                trapRigidbody.isKinematic = true;
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