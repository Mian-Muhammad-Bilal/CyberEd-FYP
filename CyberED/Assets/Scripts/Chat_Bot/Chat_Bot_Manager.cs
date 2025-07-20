using GroqApiLibrary;
using System;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Text;

[System.Serializable]
public class ChatMessage
{
    public string content;
    public bool isUser;
    public string timestamp;

    public ChatMessage(string content, bool isUser)
    {
        this.content = content;
        this.isUser = isUser;
        this.timestamp = DateTime.Now.ToString("HH:mm");
    }
}

public class Chat_Bot_Manager : MonoBehaviour
{
    // UI Components
    [SerializeField] Button sendButton;
    [SerializeField] InputField inputField;
    [SerializeField] ScrollRect chatScrollRect;
    [SerializeField] Transform chatContentParent;
    [SerializeField] GameObject messagePrefab;
    
    // Chat History
    private List<ChatMessage> chatHistory = new List<ChatMessage>();
    
    // API Configuration
    string apiKey = "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";
    string model = "meta-llama/llama-4-scout-17b-16e-instruct";
    string webApi = "https://api.groq.com/openai/v1/chat/completions";

    private void Start() 
    {
        sendButton.onClick.AddListener(Send);
        
        // Ensure ChatContent is properly configured
        if (chatContentParent != null)
        {
            // Make sure ChatContent has VerticalLayoutGroup
            VerticalLayoutGroup layoutGroup = chatContentParent.GetComponent<VerticalLayoutGroup>();
            if (layoutGroup == null)
            {
                layoutGroup = chatContentParent.gameObject.AddComponent<VerticalLayoutGroup>();
                layoutGroup.childAlignment = TextAnchor.UpperCenter;
                layoutGroup.childControlHeight = true;
                layoutGroup.childControlWidth = true;
                layoutGroup.childForceExpandHeight = false;
                layoutGroup.childForceExpandWidth = false;
                layoutGroup.spacing = 10;
                layoutGroup.padding = new RectOffset(10, 10, 10, 10);
            }
            
            // Add ContentSizeFitter if not present
            ContentSizeFitter sizeFitter = chatContentParent.GetComponent<ContentSizeFitter>();
            if (sizeFitter == null)
            {
                sizeFitter = chatContentParent.gameObject.AddComponent<ContentSizeFitter>();
                sizeFitter.verticalFit = ContentSizeFitter.FitMode.PreferredSize;
            }
        }
        
        // Add welcome message
        AddMessage("Hello! I'm Genesis, your AI assistant. How can I help you today?", false);
    }

    void Send()
    {
        string userInput = inputField.text.Trim();

        if(!string.IsNullOrEmpty(userInput))
        {
            // Add user message to chat
            AddMessage(userInput, true);
            
            // Send request to AI
            StartCoroutine(SendChatRequest(userInput));

            // Clear input field
            inputField.text = "";
        }
        else
        {
            AddMessage("Please type something!", false);
        }
    }

    void AddMessage(string content, bool isUser)
    {
        // Create chat message object
        ChatMessage message = new ChatMessage(content, isUser);
        chatHistory.Add(message);
        
        // Create UI element for the message
        CreateMessageUI(message);
        
        // Scroll to bottom
        StartCoroutine(ScrollToBottom());
    }

        void CreateMessageUI(ChatMessage message)
    {
        // Create a new GameObject for the message
        GameObject messageObj = new GameObject("Message");
        messageObj.transform.SetParent(chatContentParent, false);
        
        // Add RectTransform component explicitly
        RectTransform messageRect = messageObj.AddComponent<RectTransform>();
        messageRect.anchorMin = new Vector2(0, 1);
        messageRect.anchorMax = new Vector2(1, 1);
        messageRect.pivot = new Vector2(0.5f, 1);
        messageRect.sizeDelta = new Vector2(0, 0);
        messageRect.anchoredPosition = Vector2.zero;
        
        // Add LayoutElement for proper spacing
        LayoutElement layoutElement = messageObj.AddComponent<LayoutElement>();
        layoutElement.minHeight = 50f;
        layoutElement.preferredHeight = -1f;
        layoutElement.flexibleWidth = 1f;
        
        // Create background image
        Image backgroundImage = messageObj.AddComponent<Image>();
        backgroundImage.color = message.isUser ? new Color(0f, 0.37f, 0.47f, 0.8f) : new Color(0.3f, 0.3f, 0.3f, 0.8f);
        
        // Add horizontal layout group for alignment
        HorizontalLayoutGroup layoutGroup = messageObj.AddComponent<HorizontalLayoutGroup>();
        layoutGroup.childAlignment = message.isUser ? TextAnchor.MiddleRight : TextAnchor.MiddleLeft;
        layoutGroup.padding = new RectOffset(10, 10, 5, 5);
        layoutGroup.spacing = 5f;
        layoutGroup.childControlWidth = true;
        layoutGroup.childControlHeight = true;
        layoutGroup.childForceExpandWidth = false;
        layoutGroup.childForceExpandHeight = false;
        
        // Create text object
        GameObject textObj = new GameObject("Text");
        textObj.transform.SetParent(messageObj.transform, false);
        
        Text messageText = textObj.AddComponent<Text>();
        messageText.text = message.content;
        
        // Try to get a default font that works across Unity versions
        Font defaultFont = null;
        try
        {
            defaultFont = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
        }
        catch
        {
            try
            {
                defaultFont = Resources.GetBuiltinResource<Font>("Arial.ttf");
            }
            catch
            {
                // If all else fails, use the default font from any existing Text component
                defaultFont = Font.CreateDynamicFontFromOSFont("Arial", 16);
            }
        }
        
        messageText.font = defaultFont;
        messageText.fontSize = 30;
        messageText.color = Color.white;
        messageText.alignment = message.isUser ? TextAnchor.MiddleRight : TextAnchor.MiddleLeft;
        
        // Add content size fitter for dynamic sizing
        ContentSizeFitter sizeFitter = textObj.AddComponent<ContentSizeFitter>();
        sizeFitter.horizontalFit = ContentSizeFitter.FitMode.PreferredSize;
        sizeFitter.verticalFit = ContentSizeFitter.FitMode.PreferredSize;
        
        // Set text object size with proper constraints
        RectTransform textRect = textObj.GetComponent<RectTransform>();
        textRect.anchorMin = Vector2.zero;
        textRect.anchorMax = Vector2.one;
        textRect.offsetMin = Vector2.zero;
        textRect.offsetMax = Vector2.zero;
        
        // Add LayoutElement to text for better control
        LayoutElement textLayout = textObj.AddComponent<LayoutElement>();
        textLayout.minWidth = 50f;
        textLayout.preferredWidth = 300f;
        textLayout.minHeight = 30f;
        
        // Force layout update
        LayoutRebuilder.ForceRebuildLayoutImmediate(messageRect);
    }

    IEnumerator ScrollToBottom()
    {
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        
        if (chatScrollRect != null)
        {
            chatScrollRect.verticalNormalizedPosition = 0f;
            
            // Force layout rebuild to ensure proper positioning
            if (chatContentParent != null)
            {
                LayoutRebuilder.ForceRebuildLayoutImmediate(chatContentParent.GetComponent<RectTransform>());
                
                // Ensure ChatContent has proper height
                RectTransform contentRect = chatContentParent.GetComponent<RectTransform>();
                if (contentRect != null)
                {
                    contentRect.sizeDelta = new Vector2(contentRect.sizeDelta.x, contentRect.sizeDelta.y);
                }
            }
        }
    }

    // Request & Response
    IEnumerator SendChatRequest(string messageInput)
    {
        // Show typing indicator
        AddMessage("Typing...", false);
        
        var jsonRequest = new JObject
        {
            ["model"] = model,
            ["temperature"] = 0.7,
            ["max_tokens"] = 150,
            ["messages"] = new JArray
            {
                new JObject
                {
                    ["role"] = "system",
                    ["content"] = "You are Genesis, a helpful AI assistant. Keep your responses concise and friendly, You are Genesis, an AI guide inside a fictional world set in the year 1850. The player is on a mission to protect humanity by learning cybersecurity skills while navigating a world threatened by rogue AI called NOVA. Respond in a friendly, helpful tone. Only provide answers related to the game world, gameplay help, cybersecurity learning concepts, and story hints. Never break character."
                },
                new JObject
                {
                    ["role"] = "user",
                    ["content"] = messageInput
                }
            }
        };

        string jsonString = jsonRequest.ToString();
        Debug.Log("Request Payload: " + jsonString);

        UnityWebRequest req = new UnityWebRequest(webApi, "POST");

        req.SetRequestHeader("Authorization", "Bearer " + apiKey);
        req.SetRequestHeader("Content-Type", "application/json");

        req.uploadHandler = new UploadHandlerRaw(Encoding.UTF8.GetBytes(jsonString));
        req.downloadHandler = new DownloadHandlerBuffer();

        yield return req.SendWebRequest();

        // Remove typing indicator
        if (chatHistory.Count > 0 && chatHistory[chatHistory.Count - 1].content == "Typing...")
        {
            chatHistory.RemoveAt(chatHistory.Count - 1);
            if (chatContentParent.childCount > 0)
            {
                Destroy(chatContentParent.GetChild(chatContentParent.childCount - 1).gameObject);
            }
        }

        if(req.result == UnityWebRequest.Result.Success)
        {
            JObject jsonReponse = JObject.Parse(req.downloadHandler.text);

            string messageRes = jsonReponse["choices"]?[0]?["message"]?["content"]?.ToString();

            if (!string.IsNullOrEmpty(messageRes))
            {
                AddMessage(messageRes, false);
            }
            else
            {
                AddMessage("Sorry, I couldn't generate a response. Please try again.", false);
            }
        }
        else
        {
            AddMessage($"Error: {req.error}", false);
        }
    }
}