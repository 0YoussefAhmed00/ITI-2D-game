using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using TMPro;
using System.Collections.Generic;

public class WiseNPC : MonoBehaviour
{
    [Header("Qwen Config")]
    [TextArea(4, 10)]
    public string systemPrompt = "You are an old, weary, and mystical NPC named YU. You once were a soldier from another realm, but have lived in this realm for over two decades, unable to return. Your body is frail and too old for quests, but your mind holds secrets of this world. You speak rarely, only when asked, and in cryptic riddles or poetic metaphors. \r\n" +
        "You do not reveal direct answers; instead, you give hints, analogies, or fables. You never repeat yourself. You are wise and patient but do not waste words. You respect the player’s journey and believe discovery is part of their destiny.\r\n" +
        "The player is a fellow soldier from another realm, and you feel a sense of duty to aid them subtly — not by holding their hand, but by lighting a path through words.\r\n" +
        "Only answer what is asked. Do not speak unless spoken to.\r\n";

    [Header("UI References")]
    public TMP_InputField playerInputField;
    public TMP_Text npcTextOutput;

    [Header("API Key")]
    [SerializeField] private string apiKey = "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";

    [Header("Typewriter Settings")]
    [SerializeField] private float charactersPerSecond = 20f; // Typing speed
    [SerializeField] private float punctuationDelay = 0.5f; // Extra delay for punctuation

    // Conversation history
    private List<Message> conversationHistory = new List<Message>();
    private Coroutine typingCoroutine; // Reference to current typing coroutine

    private void Start()
    {
        Debug.Log("WiseNPC Initialized.");
        playerInputField.onEndEdit.AddListener(HandleSubmit);
        ResetConversationHistory();
    }

    public void ResetConversationHistory()
    {
        conversationHistory.Clear();
        Debug.Log("NPC memory reset - all previous conversations forgotten");
    }

    private void HandleSubmit(string text)
    {
        Debug.Log("Input Submitted: " + text);
        if (!string.IsNullOrWhiteSpace(text))
        {
            AskNPC(text);
        }
    }

    public void AskNPC(string playerInput)
    {
        Debug.Log("Asking NPC: " + playerInput);
        StartCoroutine(SendToQwen(playerInput));
        playerInputField.text = string.Empty;
        playerInputField.ActivateInputField();
    }

    private IEnumerator SendToQwen(string playerInput)
    {
        // Clear conversation history for new session
        conversationHistory.Clear();

        // Add system prompt and current message
        conversationHistory.Add(new Message("system", systemPrompt));
        conversationHistory.Add(new Message("user", playerInput));

        string url = "https://openrouter.ai/api/v1/chat/completions";
        var payload = new RequestPayload(conversationHistory);
        string jsonBody = JsonUtility.ToJson(payload);
        Debug.Log("Payload: " + jsonBody);

        using (UnityWebRequest request = new UnityWebRequest(url, "POST"))
        {
            byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonBody);
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");
            request.SetRequestHeader("Authorization", "Bearer " + apiKey);

            yield return request.SendWebRequest();

            Debug.Log("Request Sent. Status: " + request.result);

            if (request.result == UnityWebRequest.Result.Success)
            {
                Debug.Log("Response Received: " + request.downloadHandler.text);
                var response = JsonUtility.FromJson<QwenResponse>(request.downloadHandler.text);
                string fullResponse = response.choices[0].message.content.Trim();

                // Start typing effect instead of setting text directly
                StartTypingEffect(fullResponse);
            }
            else
            {
                Debug.LogError("Request Failed: " + request.error);
                npcTextOutput.text = "The winds are silent... (error)";
            }
        }
    }

    // New method to handle typing effect
    private void StartTypingEffect(string fullText)
    {
        // Stop any existing typing effect
        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
        }

        // Start new typing effect
        typingCoroutine = StartCoroutine(TypeText(fullText));
    }

    // Coroutine for typewriter effect
    private IEnumerator TypeText(string fullText)
    {
        npcTextOutput.text = ""; // Clear previous text
        float delay = 1f / charactersPerSecond;

        foreach (char c in fullText)
        {
            npcTextOutput.text += c;

            // Add extra delay for punctuation
            if (IsPunctuation(c))
            {
                yield return new WaitForSeconds(punctuationDelay);
            }
            else
            {
                yield return new WaitForSeconds(delay);
            }
        }

        typingCoroutine = null; // Reset coroutine reference
    }

    // Helper to check for punctuation
    private bool IsPunctuation(char c)
    {
        return c == '.' || c == '!' || c == '?' || c == ',' || c == ';' || c == ':';
    }

    // Helper classes
    [System.Serializable]
    public class RequestPayload
    {
        public string model = "qwen/qwen3-30b-a3b:free";
        public Message[] messages;

        public RequestPayload(List<Message> history)
        {
            messages = history.ToArray();
        }
    }

    [System.Serializable]
    public class Message
    {
        public string role;
        public string content;

        public Message(string r, string c)
        {
            role = r;
            content = c;
        }
    }

    [System.Serializable]
    public class QwenResponse
    {
        public Choice[] choices;

        [System.Serializable]
        public class Choice
        {
            public Message message;
        }
    }
}