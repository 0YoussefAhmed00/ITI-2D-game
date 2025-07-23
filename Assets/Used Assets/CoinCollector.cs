using UnityEngine;

public class CoinCollector : MonoBehaviour
{
    public GameManager gameManager;
    public AudioSource audioSource; // Reference to audio source
    public AudioClip coinCollectSound; // Coin collection sound

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Coin"))
        {
            // Play collection sound
            if (audioSource != null && coinCollectSound != null)
            {
                audioSource.PlayOneShot(coinCollectSound);
            }

            // Notify game manager
            gameManager.OnCoinCollected(collision.gameObject);
        }
    }
}