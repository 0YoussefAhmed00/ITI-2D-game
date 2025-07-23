using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("Scene References")]
    public Transform coinParent;
    public ChestOpener chest;
    public KeyStatusUI keyStatusUI; // Add this line

    private int coinsRemaining;
    private bool hasKey = false;
    private bool keyUpgraded = false;

    private void Awake()
    {
        coinsRemaining = coinParent.childCount;
        if (keyStatusUI != null)
            keyStatusUI.ShowNoKey();
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
        if (keyStatusUI != null)
            keyStatusUI.ShowNormalKey();
        TryOpenChest();
    }

    public void OnKeyUpgraded()
    {
        Debug.Log("Key upgraded!");
        keyUpgraded = true;
        if (keyStatusUI != null)
            keyStatusUI.ShowBlueKey();
        TryOpenChest();
    }

    private void TryOpenChest()
    {
        // No longer auto-opens chest
        // if (coinsRemaining <= 0 && hasKey && keyUpgraded)
        // {
        //     Debug.Log("Opening chest!");
        //     chest.OpenChest();
        // }
    }

    public bool HasKey()
    {
        return hasKey;
    }

    public bool IsKeyUpgraded()
    {
        return keyUpgraded;
    }

    public bool AllCoinsCollected()
    {
        return coinsRemaining <= 0;
    }
}
