using UnityEngine;
using System.Collections;

public class ChestOpener : MonoBehaviour, IInteractable
{
    [Header("Sprites")]
    public Sprite closedSprite;
    public Sprite openSprite;

    [Header("Gem Settings")]
    public GameObject gemObject;
    public float gemDelay = 0.1f;
    public Vector2 launchForce = new Vector2(1f, 5f);
    public float moveDuration = 1f;

    private SpriteRenderer spriteRenderer;
    private bool isOpened = false;

    [Header("Audio")]
    public AudioSource chestAudioSource; // Drag & assign in Inspector (clip + disable Play on Awake)

    public GameManager gameManager;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = closedSprite;

        if (gemObject != null)
        {
            gemObject.SetActive(false);

            Collider2D col = gemObject.GetComponent<Collider2D>();
            if (col != null)
                col.enabled = false;
        }
    }

    public void Interact()
    {
        if (isOpened) return;

        if (gameManager != null &&
            gameManager.AllCoinsCollected() &&
            gameManager.HasKey() &&
            gameManager.IsKeyUpgraded())
        {
            OpenChest();
        }
        else
        {
            Debug.Log("Chest is locked. Collect all coins, the key, and upgrade the key first.");
        }
    }

    public void OpenChest()
    {
        if (isOpened) return;
        isOpened = true;

        spriteRenderer.sprite = openSprite;

        if (chestAudioSource != null)
            chestAudioSource.Play(); // Play sound once

        if (gemObject != null)
            StartCoroutine(LaunchAndEnableGem());
    }

    private IEnumerator LaunchAndEnableGem()
    {
        yield return new WaitForSeconds(gemDelay);

        gemObject.SetActive(true);

        Rigidbody2D rb = gemObject.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;
            rb.gravityScale = 1f;
            rb.AddForce(launchForce, ForceMode2D.Impulse);
        }

        yield return new WaitForSeconds(moveDuration);

        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;
            rb.angularVelocity = 0f;
            rb.gravityScale = 0f;
            rb.bodyType = RigidbodyType2D.Kinematic;
        }

        Collider2D col = gemObject.GetComponent<Collider2D>();
        if (col != null)
            col.enabled = true;
    }
}
