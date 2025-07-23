using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class DialogueInputHandler : MonoBehaviour
{
    public TMP_InputField inputField;
    public GameObject inputUI; // the UI panel that holds the input field
    public PlayerInput playerInput;

    private bool isChatting = false;

    void Update()
    {
        if (isChatting && Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            CloseChat();
        }
    }

    public void OpenChat()
    {
        isChatting = true;
        inputUI.SetActive(true);
        inputField.text = "";
        inputField.ActivateInputField();
        playerInput.DeactivateInput(); // disable player controls
    }

    public void CloseChat()
    {
        isChatting = false;
        inputUI.SetActive(false);
        playerInput.ActivateInput(); // re-enable player controls
    }
}
