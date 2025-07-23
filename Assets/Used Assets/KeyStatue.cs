using UnityEngine;

public class KeyStatue : MonoBehaviour, IInteractable
{
    public GameManager gameManager;
    public GameObject normalKeyVisual;
    public GameObject blueKeyVisual;

    private bool upgraded = false;

    public void Interact()
    {
        if (upgraded) return;
        if (!gameManager.HasKey()) return;

        gameManager.OnKeyUpgraded();
        upgraded = true;

        if (normalKeyVisual != null) normalKeyVisual.SetActive(false);
        //if (blueKeyVisual != null) blueKeyVisual.SetActive(true);
    }
}