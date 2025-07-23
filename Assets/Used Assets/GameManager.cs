using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("Scene References")]
    public Transform coinParent;
    public ChestOpener chest;
    public KeyStatusUI keyStatusUI;

    [Header("Audio")]
    public AudioSource backgroundMusic;
    public AudioSource keyCollectedAudio;  // Assign in Inspector
    public AudioSource keyUpgradedAudio;   // Assign in Inspector

    private int coinsRemaining;
    private bool hasKey = false;
    private bool keyUpgraded = false;

    private void Awake()
    {
        coinsRemaining = coinParent.childCount;

        if (keyStatusUI != null)
            keyStatusUI.ShowNoKey();

        if (backgroundMusic != null && !backgroundMusic.isPlaying)
        {
            backgroundMusic.loop = true;
            backgroundMusic.Play();
        }
    }

    public void OnCoinCollected(GameObject coin)
    {
        Destroy(coin);
        coinsRemaining--;

        TryOpenChest();
    }

    public void OnKeyCollected()
    {
        Debug.Log("Key collected!");
        hasKey = true;

        if (keyCollectedAudio != null)
            keyCollectedAudio.Play();

        if (keyStatusUI != null)
            keyStatusUI.ShowNormalKey();

        TryOpenChest();
    }

    public void OnKeyUpgraded()
    {
        Debug.Log("Key upgraded!");
        keyUpgraded = true;

        if (keyUpgradedAudio != null)
            keyUpgradedAudio.Play();

        if (keyStatusUI != null)
            keyStatusUI.ShowBlueKey();

        TryOpenChest();
    }

    public bool HasKey() => hasKey;
    public bool IsKeyUpgraded() => keyUpgraded;
    public bool AllCoinsCollected() => coinsRemaining <= 0;

    private void TryOpenChest()
    {
        // Chest auto-opening is disabled
    }
}
