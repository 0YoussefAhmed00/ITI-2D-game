using UnityEngine;

public class Coin : MonoBehaviour
{
    public AudioClip collectSound;
    private AudioSource audioSource;
    private bool collected = false;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
        audioSource.playOnAwake = false;
    }

    public void Collect()
    {
        if (collected) return;
        collected = true;

        // Play sound
        if (collectSound != null)
        {
            audioSource.PlayOneShot(collectSound);
        }

        // Disable visuals but keep for sound playback
        GetComponent<Renderer>().enabled = false;
        GetComponent<Collider>().enabled = false;

        // Destroy after sound finishes
        Destroy(gameObject, collectSound.length);
    }
}