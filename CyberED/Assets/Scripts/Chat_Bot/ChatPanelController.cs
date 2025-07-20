using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ChatPanelController : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private Button triggerButton;
    [SerializeField] private RectTransform chatPanel;
    [SerializeField] private RectTransform triggerButtonRect;
    
    [Header("Animation Settings")]
    [SerializeField] private float animationDuration = 0.5f;
    [SerializeField] private float slideDistance = 800f;
    [SerializeField] private AnimationCurve animationCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);
    
    [Header("Panel States")]
    [SerializeField] private bool isPanelVisible = false;
    
    private Vector2 panelHiddenPosition;
    private Vector2 panelVisiblePosition;
    private Vector2 buttonHiddenPosition;
    private Vector2 buttonVisiblePosition;
    
    private void Start()
    {
        // Calculate positions
        SetupPositions();
        
        // Start with panel hidden
        SetPanelHidden();
    }
    
    private void SetupPositions()
    {
        if (chatPanel != null)
        {
            // Panel slides from left to center
            panelHiddenPosition = new Vector2(-slideDistance, chatPanel.anchoredPosition.y);
            panelVisiblePosition = chatPanel.anchoredPosition;
        }
        
        if (triggerButtonRect != null)
        {
            // Button moves with the panel
            buttonHiddenPosition = new Vector2(-slideDistance + 100f, triggerButtonRect.anchoredPosition.y);
            buttonVisiblePosition = triggerButtonRect.anchoredPosition;
        }
    }
    
    public void TogglePanel()
    {
        if (isPanelVisible)
        {
            HidePanel();
        }
        else
        {
            ShowPanel();
        }
    }
    
    public void ShowPanel()
    {
        if (!isPanelVisible)
        {
            isPanelVisible = true;
            StartCoroutine(AnimatePanel(true));
        }
    }
    
    public void HidePanel()
    {
        if (isPanelVisible)
        {
            isPanelVisible = false;
            StartCoroutine(AnimatePanel(false));
        }
    }
    
    private IEnumerator AnimatePanel(bool show)
    {
        float elapsedTime = 0f;
        Vector2 startPos = show ? panelHiddenPosition : panelVisiblePosition;
        Vector2 endPos = show ? panelVisiblePosition : panelHiddenPosition;
        Vector2 buttonStartPos = show ? buttonHiddenPosition : buttonVisiblePosition;
        Vector2 buttonEndPos = show ? buttonVisiblePosition : buttonHiddenPosition;
        
        while (elapsedTime < animationDuration)
        {
            elapsedTime += Time.deltaTime;
            float progress = elapsedTime / animationDuration;
            float curveValue = animationCurve.Evaluate(progress);
            
            // Animate panel
            if (chatPanel != null)
            {
                chatPanel.anchoredPosition = Vector2.Lerp(startPos, endPos, curveValue);
            }
            
            // Animate button
            if (triggerButtonRect != null)
            {
                triggerButtonRect.anchoredPosition = Vector2.Lerp(buttonStartPos, buttonEndPos, curveValue);
            }
            
            yield return null;
        }
        
        // Ensure final position is exact
        if (chatPanel != null)
        {
            chatPanel.anchoredPosition = endPos;
        }
        
        if (triggerButtonRect != null)
        {
            triggerButtonRect.anchoredPosition = buttonEndPos;
        }
    }
    
    private void SetPanelHidden()
    {
        if (chatPanel != null)
        {
            chatPanel.anchoredPosition = panelHiddenPosition;
        }
        
        if (triggerButtonRect != null)
        {
            triggerButtonRect.anchoredPosition = buttonHiddenPosition;
        }
        
        isPanelVisible = false;
    }
    
    // Public method to check panel state
    public bool IsPanelVisible()
    {
        return isPanelVisible;
    }
} 