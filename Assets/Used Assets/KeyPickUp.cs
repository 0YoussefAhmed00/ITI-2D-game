using UnityEngine;

public class KeyPickUp : MonoBehaviour, IInteractable
{
    public GameManager gameManager;

    public void Interact()
    {
        gameManager.OnKeyCollected();
        Destroy(gameObject);
    }
}
