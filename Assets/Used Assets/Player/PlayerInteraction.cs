using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteraction : MonoBehaviour
{
    private GameObject interactTarget; 

    private PlayerInput playerInput;
    private InputAction interactAction;

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        interactAction = playerInput.actions["Interact"];
    }

    private void OnEnable()
    {
        interactAction.performed += OnInteract;
    }

    private void OnDisable()
    {
        interactAction.performed -= OnInteract;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Key") || other.CompareTag("Statue") || other.CompareTag("Chest") || other.CompareTag("NPC"))
            interactTarget = other.gameObject;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (interactTarget == other.gameObject)
        {
            var npc = interactTarget.GetComponent<WiseNPCInteractable>();
            if (npc != null && npc.npcUI != null)
            {
                npc.npcUI.SetActive(false);
                WiseNPCInteractable.IsNPCUIOpen = false; // Hide panel and reset flag
            }
            interactTarget = null;
        }
    }



    private void OnInteract(InputAction.CallbackContext ctx)
    {
        if (interactTarget == null) return;

        var interactable = interactTarget.GetComponent<IInteractable>();
        if (interactable != null)
        {
            interactable.Interact();
        }
    }
}
