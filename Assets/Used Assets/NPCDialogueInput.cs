using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class NPCDialogueInput : MonoBehaviour
{
    [Header("UI Components")]
    public GameObject inputPanel;           // Panel containing TMP_InputField
    public TMP_InputField inputField;       // The InputField for player questions

    [Header("Player Input")]
    public PlayerInput playerInput;         // Reference to the new Input System PlayerInput
    public string playerActionMap = "Player";   // Name of the gameplay action map
    public string uiActionMap = "UI";           // Name of the UI action map

    private bool isTalking = false;

    void Update()
    {
        if (isTalking && Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            CloseDialogue();
        }
    }

    /// <summary>
    /// Call this to open the dialogue input when interacting with YU.
    /// </summary>
    public void StartDialogue()
    {
        Debug.Log("Dialogue started.");
        isTalking = true;
        inputPanel.SetActive(true);
        inputField.text = string.Empty;
        inputField.ActivateInputField();

        // Switch to UI map so gameplay input stops
        if (playerInput != null)
        {
            playerInput.SwitchCurrentActionMap(uiActionMap);
            Debug.Log("Switched to UI action map");
        }
    }

    /// <summary>
    /// Call this to close the dialogue input, re-enable gameplay.
    /// </summary>
    public void CloseDialogue()
    {
        Debug.Log("Dialogue closed.");
        isTalking = false;
        inputPanel.SetActive(false);

        // Switch back to Player map
        if (playerInput != null)
        {
            playerInput.SwitchCurrentActionMap(playerActionMap);
            Debug.Log("Switched to Player action map");
        }
    }

    /// <summary>
    /// Hook this up to InputField.onEndEdit to send question on Enter.
    /// </summary>
    public void OnInputSubmit(string text)
    {
        if (!isTalking) return;
        if (Keyboard.current.enterKey.wasPressedThisFrame && !string.IsNullOrWhiteSpace(text))
        {
            Debug.Log("Submitting question: " + text);
            // TODO: Call your WiseNPC.AskNPC(text) here

            inputField.text = string.Empty;
            inputField.ActivateInputField();
        }
    }
}