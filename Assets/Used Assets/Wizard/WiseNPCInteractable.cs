using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class WiseNPCInteractable : MonoBehaviour, IInteractable
{
    public GameObject npcUI;
    public static bool IsNPCUIOpen = false;

    private PlayerInput playerInput;
    private TMP_InputField inputField;

    private void Start()
    {
        if (npcUI != null)
            npcUI.SetActive(false);

        IsNPCUIOpen = false;

        // Get player input reference
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerInput = player.GetComponent<PlayerInput>();
        }

        // Get reference to input field
        if (npcUI != null)
        {
            inputField = npcUI.GetComponentInChildren<TMP_InputField>();
        }
    }

    public void Interact()
    {
        if (npcUI != null)
        {
            npcUI.SetActive(true);
            IsNPCUIOpen = true;

            // Disable player input
            if (playerInput != null)
                playerInput.enabled = false;

            // Focus input field
            if (inputField != null)
                inputField.ActivateInputField();
        }
    }

    public void CloseNPCUI()
    {
        if (npcUI != null)
        {
            npcUI.SetActive(false);
            IsNPCUIOpen = false;

            // Re-enable player input
            if (playerInput != null)
                playerInput.enabled = true;
        }
    }

    private void Update()
    {
        // Only handle ESC for chat if UI is open
        if (IsNPCUIOpen && Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            CloseNPCUI();
        }
    }
}