using System.Collections;
using UnityEngine;

public class SpawnEffect : MonoBehaviour
{
    [Header("Flash Settings")]
    public GameObject spriteObject;       // The sprite to flash
    public int flashCount = 6;            // Number of flashes (on/off = 1 flash)
    public float flashInterval = 0.1f;    // Time between each on/off

    [Header("Sound")]
    public AudioSource audioSource;       // Plays the spawn sound

    private void Awake()
    {
        if (spriteObject != null)
        {
            if (audioSource != null && audioSource.clip != null)
                audioSource.Play();

            StartCoroutine(FlashSprite());
        }
    }

    private IEnumerator FlashSprite()
    {
        for (int i = 0; i < flashCount; i++)
        {
            spriteObject.SetActive(false);
            yield return new WaitForSeconds(flashInterval);
            spriteObject.SetActive(true);
            yield return new WaitForSeconds(flashInterval);
        }
    }
}
