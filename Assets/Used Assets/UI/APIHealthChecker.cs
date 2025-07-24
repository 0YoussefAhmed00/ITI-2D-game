using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using TMPro;

public class APIChecker : MonoBehaviour
{
    [Header("Google Drive API Key File")]
    [Tooltip("Direct‑download URL: https://drive.google.com/uc?export=download&id=YOUR_FILE_ID")]
    [SerializeField] private string driveKeyUrl = "https://drive.google.com/uc?export=download&id=1MmPh6R5sJlqDtqjzCj2hf5yeZ8ZvV8g6";

    [Header("UI References")]
    [Tooltip("Parent GameObject containing the warning image + countdown")]
    public GameObject warningUI;
    [Tooltip("TextMeshProUGUI that shows just the countdown number")]
    public TextMeshProUGUI countdownText;

    [Header("Object To Hide")]
    [Tooltip("This GameObject will be deactivated while warning is visible")]
    public GameObject objectToHide;

    [Header("Settings")]
    [Tooltip("How long (seconds) to display the warning")]
    public float displayDuration = 10f;
    [Tooltip("Model endpoint to test")]
    public string modelEndpoint = "https://openrouter.ai/api/v1/chat/completions";

    [Header("Test Prompt")]
    [TextArea]
    [Tooltip("Message to send for connectivity test")]
    public string testPrompt = "Hello?";

    private string apiKey = "";
    private bool keyLoaded = false;

    private void Awake()
    {
        warningUI.SetActive(false);
        if (objectToHide != null) objectToHide.SetActive(true);
        StartCoroutine(FetchApiKey());
    }

    private IEnumerator FetchApiKey()
    {
        using var req = UnityWebRequest.Get(driveKeyUrl);
        yield return req.SendWebRequest();

        if (req.result == UnityWebRequest.Result.Success && 
            !string.IsNullOrWhiteSpace(req.downloadHandler.text))
        {
            apiKey = req.downloadHandler.text.Trim();
            keyLoaded = true;
            Debug.Log("API key loaded from Drive.");
            StartCoroutine(TestModel());
        }
        else
        {
            Debug.LogError("Failed to load API key: " + req.error);
            StartCoroutine(ShowWarning());
        }
    }

    private IEnumerator TestModel()
    {
        // Prepare a minimal payload
        var messages = new List<Message>
        {
            new Message("system", "Connectivity test."),
            new Message("user", testPrompt)
        };
        var payload = new RequestPayload(messages);
        string jsonBody = JsonUtility.ToJson(payload);

        using var req = new UnityWebRequest(modelEndpoint, "POST");
        byte[] body = System.Text.Encoding.UTF8.GetBytes(jsonBody);
        req.uploadHandler = new UploadHandlerRaw(body);
        req.downloadHandler = new DownloadHandlerBuffer();
        req.SetRequestHeader("Content-Type", "application/json");
        req.SetRequestHeader("Authorization", "Bearer " + apiKey);

        yield return req.SendWebRequest();

        if (req.result == UnityWebRequest.Result.Success)
        {
            Debug.Log("✅ Model test succeeded.");
            // do nothing – model is reachable
        }
        else
        {
            Debug.LogWarning("⚠️ Model test failed: " + req.error);
            StartCoroutine(ShowWarning());
        }
    }

    private IEnumerator ShowWarning()
    {
        // Show warning UI and hide the other object
        warningUI.SetActive(true);
        if (objectToHide != null) objectToHide.SetActive(false);

        float timeLeft = displayDuration;
        while (timeLeft > 0f)
        {
            countdownText.text = Mathf.CeilToInt(timeLeft).ToString();
            yield return new WaitForSeconds(1f);
            timeLeft -= 1f;
        }

        // Hide warning and restore the other object
        warningUI.SetActive(false);
        if (objectToHide != null) objectToHide.SetActive(true);
    }

    [System.Serializable]
    private class RequestPayload
    {
        public string model = "qwen/qwen3-30b-a3b:free";
        public Message[] messages;
        public RequestPayload(List<Message> history) => messages = history.ToArray();
    }

    [System.Serializable]
    private class Message
    {
        public string role;
        public string content;
        public Message(string r, string c) { role = r; content = c; }
    }
}
