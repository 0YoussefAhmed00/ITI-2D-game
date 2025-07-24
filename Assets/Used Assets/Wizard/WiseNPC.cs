using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using TMPro;

public class WiseNPC : MonoBehaviour
{
    [Header("Qwen Config")]
    [TextArea(4, 10)]
    public string systemPrompt = "You are an old, weary, and mystical NPC named YU. You once were a soldier from another realm, but have lived in this realm for over two decades, unable to return. Your body is frail and too old for quests, but your mind holds secrets of this world. You speak rarely, only when asked, and in cryptic riddles or poetic metaphors.\r\n" +
        "You do not reveal direct answers; instead, you give hints, analogies, or fables. You never repeat yourself. You are wise and patient but do not waste words. You respect the player’s journey and believe discovery is part of their destiny.\r\n" +
        "The player is a fellow soldier from another realm, and you feel a sense of duty to aid them subtly—not by holding their hand, but by lighting a path through words.\r\n" +
        "Only answer what is asked. Do not speak unless spoken to.\r\n";

    [Header("UI References")]
    public TMP_InputField playerInputField;
    public TMP_Text npcTextOutput;

    [Header("Google Drive Key File")]
    [Tooltip("Use a link like: https://drive.google.com/uc?export=download&id=YOUR_FILE_ID")]
    public string driveKeyUrl = "https://drive.google.com/file/d/1MmPh6R5sJlqDtqjzCj2hf5yeZ8ZvV8g6/view?usp=drive_link";

    [Header("Typewriter Settings")]
    [SerializeField] private float charactersPerSecond = 20f;
    [SerializeField] private float punctuationDelay = 0.5f;

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip typingClip;

    private List<Message> conversationHistory = new List<Message>();
    private Coroutine typingCoroutine;
    private Coroutine loadingCoroutine;
    private Coroutine sleepingCoroutine;
    private bool hasInteracted = false;

    private bool keyLoaded = false;
    private string apiKey;

    private void Start()
    {
        Debug.Log("WiseNPC Initialized.");
        npcTextOutput.text = "Loading API key...";
        playerInputField.onEndEdit.AddListener(HandleSubmit);
        StartCoroutine(FetchApiKey());
    }

    private IEnumerator FetchApiKey()
    {
        using UnityWebRequest req = UnityWebRequest.Get(driveKeyUrl);
        yield return req.SendWebRequest();

        if (req.result == UnityWebRequest.Result.Success)
        {
            apiKey = req.downloadHandler.text.Trim();
            keyLoaded = true;
            npcTextOutput.text = "";
            sleepingCoroutine = StartCoroutine(ShowSleeping());
            ResetConversationHistory();
            Debug.Log("API key loaded from Drive.");
        }
        else
        {
            Debug.LogError("Failed to load API key: " + req.error);
            npcTextOutput.text = "Error loading key.";
        }
    }

    public void ResetConversationHistory()
    {
        conversationHistory.Clear();
        Debug.Log("NPC memory reset - all previous conversations forgotten");
    }

    private void HandleSubmit(string text)
    {
        if (!string.IsNullOrWhiteSpace(text))
            AskNPC(text);
    }

    public void AskNPC(string playerInput)
    {
        if (!keyLoaded)
        {
            Debug.LogWarning("Still loading API key...");
            return;
        }

        if (sleepingCoroutine != null) StopCoroutine(sleepingCoroutine);
        if (loadingCoroutine != null) StopCoroutine(loadingCoroutine);

        hasInteracted = true;
        loadingCoroutine = StartCoroutine(ShowLoadingDots());
        StartCoroutine(SendToQwen(playerInput));

        playerInputField.text = string.Empty;
        playerInputField.ActivateInputField();
    }

    private IEnumerator SendToQwen(string playerInput)
    {
        conversationHistory.Clear();
        conversationHistory.Add(new Message("system", systemPrompt));
        conversationHistory.Add(new Message("user", playerInput));

        string url = "https://openrouter.ai/api/v1/chat/completions";
        var payload = new RequestPayload(conversationHistory);
        string jsonBody = JsonUtility.ToJson(payload);

        using UnityWebRequest request = new UnityWebRequest(url, "POST")
        {
            uploadHandler = new UploadHandlerRaw(System.Text.Encoding.UTF8.GetBytes(jsonBody)),
            downloadHandler = new DownloadHandlerBuffer()
        };
        request.SetRequestHeader("Content-Type", "application/json");
        request.SetRequestHeader("Authorization", "Bearer " + apiKey);

        yield return request.SendWebRequest();

        if (loadingCoroutine != null) StopCoroutine(loadingCoroutine);

        if (request.result == UnityWebRequest.Result.Success)
        {
            var response = JsonUtility.FromJson<QwenResponse>(request.downloadHandler.text);
            StartTypingEffect(response.choices[0].message.content.Trim());
        }
        else
        {
            Debug.LogError("Request Failed: " + request.error);
            npcTextOutput.text = "The winds are silent... (error)";
        }
    }

    private IEnumerator ShowSleeping()
    {
        string[] sleepStates = { "z", "zZ", "zZzZ", "zZzZzZz...", "zZz", "zZ" };
        int index = 0;
        while (!hasInteracted)
        {
            npcTextOutput.text = sleepStates[index % sleepStates.Length];
            index++;
            yield return new WaitForSeconds(0.6f);
        }
    }

    private IEnumerator ShowLoadingDots()
    {
        string[] loadingStates = { ".", "..", "..." };
        int idx = 0;
        while (true)
        {
            npcTextOutput.text = loadingStates[idx % loadingStates.Length];
            idx++;
            yield return new WaitForSeconds(0.5f);
        }
    }

    private void StartTypingEffect(string fullText)
    {
        if (typingCoroutine != null) StopCoroutine(typingCoroutine);

        if (typingClip != null && audioSource != null)
        {
            audioSource.clip = typingClip;
            audioSource.loop = true;
            audioSource.Play();
        }

        typingCoroutine = StartCoroutine(TypeText(fullText));
    }

    private IEnumerator TypeText(string fullText)
    {
        npcTextOutput.text = "";
        float delay = 1f / charactersPerSecond;

        foreach (char c in fullText)
        {
            npcTextOutput.text += c;
            if (IsPunctuation(c))
                yield return new WaitForSeconds(punctuationDelay);
            else
                yield return new WaitForSeconds(delay);
        }

        if (audioSource != null && audioSource.isPlaying)
            audioSource.Stop();

        typingCoroutine = null;
    }

    private bool IsPunctuation(char c)
        => c == '.' || c == '!' || c == '?' || c == ',' || c == ';' || c == ':';

    [System.Serializable]
    public class RequestPayload
    {
        public string model = "qwen/qwen3-30b-a3b:free";
        public Message[] messages;
        public RequestPayload(List<Message> history) => messages = history.ToArray();
    }

    [System.Serializable]
    public class Message
    {
        public string role, content;
        public Message(string r, string c) { role = r; content = c; }
    }

    [System.Serializable]
    public class QwenResponse
    {
        public Choice[] choices;
        [System.Serializable] public class Choice { public Message message; }
    }
}
