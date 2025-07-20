using UnityEngine;
using UnityEngine.UI;

public class MessagePrefab : MonoBehaviour
{
    [SerializeField] private Text messageText;
    [SerializeField] private Image backgroundImage;
    [SerializeField] private Text timestampText;
    
    public void SetupMessage(string content, bool isUser, string timestamp = "")
    {
        if (messageText != null)
        {
            messageText.text = content;
            messageText.alignment = isUser ? TextAnchor.MiddleRight : TextAnchor.MiddleLeft;
        }
        
        if (backgroundImage != null)
        {
            backgroundImage.color = isUser ? new Color(0.2f, 0.6f, 1f, 0.8f) : new Color(0.3f, 0.3f, 0.3f, 0.8f);
        }
        
        if (timestampText != null && !string.IsNullOrEmpty(timestamp))
        {
            timestampText.text = timestamp;
            timestampText.alignment = isUser ? TextAnchor.MiddleRight : TextAnchor.MiddleLeft;
        }
        
        // Set the message alignment
        RectTransform rectTransform = GetComponent<RectTransform>();
        if (rectTransform != null)
        {
            // Align user messages to the right, AI messages to the left
            if (isUser)
            {
                rectTransform.anchorMin = new Vector2(0.5f, 0f);
                rectTransform.anchorMax = new Vector2(1f, 1f);
                rectTransform.offsetMin = new Vector2(50f, 0f);
                rectTransform.offsetMax = new Vector2(-10f, 0f);
            }
            else
            {
                rectTransform.anchorMin = new Vector2(0f, 0f);
                rectTransform.anchorMax = new Vector2(0.5f, 1f);
                rectTransform.offsetMin = new Vector2(10f, 0f);
                rectTransform.offsetMax = new Vector2(-50f, 0f);
            }
        }
    }
} 