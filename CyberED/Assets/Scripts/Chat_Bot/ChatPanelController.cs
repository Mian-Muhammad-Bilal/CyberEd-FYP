using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ChatPanelController : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private Button triggerButton;
    [SerializeField] private RectTransform chatPanel;
    
    [Header("Animation Settings")]
    [SerializeField] private float animationDuration = 0.5f;
    [SerializeField] private float slideDistance = 8000f;
    [SerializeField] private AnimationCurve animationCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);
    
    [Header("Panel States")]
    [SerializeField] private bool isPanelVisible = false;
    
    private Vector2 panelHiddenPosition;
    private Vector2 panelVisiblePosition;
    
    private void Start()
    {
        SetupPositions();
        SetPanelHidden();
    }
    
    private void SetupPositions()
    {
        if (chatPanel != null)
        {
            panelVisiblePosition = chatPanel.anchoredPosition;
            panelHiddenPosition = new Vector2(panelVisiblePosition.x - 850f, panelVisiblePosition.y);

            Debug.Log($"Panel visible position: {panelVisiblePosition}");
            Debug.Log($"Panel hidden position: {panelHiddenPosition}");
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

            // ⏸ Pause the game
            Time.timeScale = 0f;

            StartCoroutine(AnimatePanel(true));
        }
    }
    
    public void HidePanel()
    {
        if (isPanelVisible)
        {
            isPanelVisible = false;

            // ▶️ Resume the game
            Time.timeScale = 1f;

            StartCoroutine(AnimatePanel(false));
        }
    }
    
    private IEnumerator AnimatePanel(bool show)
    {
        float elapsedTime = 0f;
        Vector2 startPos = show ? panelHiddenPosition : panelVisiblePosition;
        Vector2 endPos = show ? panelVisiblePosition : panelHiddenPosition;

        Debug.Log($"Animation: show={show}, startPos={startPos}, endPos={endPos}");

        while (elapsedTime < animationDuration)
        {
            elapsedTime += Time.unscaledDeltaTime; // Use unscaled time to keep animating while paused
            float progress = elapsedTime / animationDuration;
            float curveValue = animationCurve.Evaluate(progress);

            if (chatPanel != null)
            {
                Vector2 newPos = Vector2.Lerp(startPos, endPos, curveValue);
                chatPanel.anchoredPosition = newPos;

                if (elapsedTime % 0.1f < Time.unscaledDeltaTime)
                {
                    Debug.Log($"Current position: {chatPanel.anchoredPosition}");
                }
            }

            yield return null;
        }

        if (chatPanel != null)
        {
            chatPanel.anchoredPosition = endPos;
            Debug.Log($"Final position: {chatPanel.anchoredPosition}");
        }
    }
    
    private void SetPanelHidden()
    {
        if (chatPanel != null)
        {
            chatPanel.anchoredPosition = panelHiddenPosition;
        }

        isPanelVisible = false;
        Time.timeScale = 1f; // Ensure game isn't paused on start
    }

    public bool IsPanelVisible()
    {
        return isPanelVisible;
    }
}
