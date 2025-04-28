using GroqApiLibrary;
using System;
using System.Collections;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Text;

public class Chat_Bot_Manager : MonoBehaviour
{
    // UI
    [SerializeField] Button sendButton; // send the jsonRequest
    [SerializeField] InputField inputField; // user will write a jsonRequest
    [SerializeField] Text responseText; // AI will respond

    string apiKey = "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";
    string model = "meta-llama/llama-4-scout-17b-16e-instruct";
    string webApi = "https://api.groq.com/openai/v1/chat/completions"; // Required

    private void Start() 
    {
        sendButton.onClick.AddListener(Send);
    }
    void Send()
    {
        string userInput = inputField.text;

        if(!string.IsNullOrEmpty(userInput))
        {
            StartCoroutine(SendMessage(userInput));
        }
        else
        {
            responseText.text = "Please type something!";
        }

    }
    // Request & Response
    IEnumerator SendMessage(string messageInput)
    {
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
                    ["content"] = "You are a helpful assistant."
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

        if(req.result == UnityWebRequest.Result.Success)
        {
            JObject jsonReponse = JObject.Parse(req.downloadHandler.text);

            string messageRes = jsonReponse["choices"]?[0]?["message"]?["content"]?.ToString();

            responseText.text = !string.IsNullOrEmpty(messageRes) ? "AI: " + messageRes : "No response received.";
        }
        else
        {
            responseText.text = $"Error: {req.error}";
        }
    }
}