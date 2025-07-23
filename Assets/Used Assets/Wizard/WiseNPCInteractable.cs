using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class WiseNPCInteractable : MonoBehaviour, IInteractable
{
    public GameObject npcUI;
    public static bool IsNPCUIOpen = false;

    private PlayerInput playerInput;
    private TMP_InputField inputField;

    private bool waitingForKeyRelease = false;

    private void Start()
    {
        if (npcUI != null)
            npcUI.SetActive(false);

        IsNPCUIOpen = false;

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerInput = player.GetComponent<PlayerInput>();
        }

        if (npcUI != null)
        {
            inputField = npcUI.GetComponentInChildren<TMP_InputField>();
        }
    }

    public void Interact()
    {
        if (npcUI != null && !IsNPCUIOpen)
        {
            npcUI.SetActive(true);
            IsNPCUIOpen = true;
            waitingForKeyRelease = true;

            if (playerInput != null)
                playerInput.enabled = false;
        }
    }

    public void CloseNPCUI()
    {
        if (npcUI != null)
        {
            npcUI.SetActive(false);
            IsNPCUIOpen = false;
            waitingForKeyRelease = false;

            if (playerInput != null)
                playerInput.enabled = true;
        }
    }

    private void Update()
    {
        if (IsNPCUIOpen)
        {
            if (Keyboard.current.escapeKey.wasPressedThisFrame)
            {
                CloseNPCUI();
                return;
            }

            // Wait for player to release E before activating input field
            if (waitingForKeyRelease && !Keyboard.current.eKey.isPressed)
            {
                waitingForKeyRelease = false;

                if (inputField != null)
                {
                    inputField.ActivateInputField();
                }
            }
        }
    }
}
