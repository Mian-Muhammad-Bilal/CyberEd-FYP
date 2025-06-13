using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

public class MainMenu1 : MonoBehaviour
{
    [Header("Panels (assign in Inspector)")]
    public GameObject creditsPanel;      // Drag CreditsPanel here
    public GameObject howToPlayPanel;    // Drag HowToPlayPanel here
    public GameObject settingsPanel;     // Drag SettingsPanel here
    public GameObject profilePanel;      // Drag FirebaseAuth ProfilePanel here
    public GameObject warningPopup;      // Assign this in Inspector (a panel or TMP_Text that shows a warning)

    [Header("Settings UI Elements (assign for SettingsPanel)")]
    public Slider musicSlider;           // Drag the Music slider here
    public TMP_Dropdown graphicsDropdown;// Drag the Graphics dropdown here
    public Slider brightnessSlider;      // Drag the Brightness slider here
    public Image brightnessOverlay;      // Drag the Brightness overlay here

    [Header("Settings Save Control")]
    private bool isSettingsChanged = false;

    // Optional: If you want the brightness slider to affect the whole Canvas,
    // add a CanvasGroup on your Canvas and assign it below:
    //public CanvasGroup brightnessCanvasGroup;

    // ───────────────────────────────────────────────────────────────────────────
    // 0) LOAD / APPLY SETTINGS ON START
    // ───────────────────────────────────────────────────────────────────────────
    void Start()
    {
        if (musicSlider != null && graphicsDropdown != null && brightnessSlider != null && brightnessOverlay != null)
        {
            musicSlider.value = PlayerPrefs.GetFloat("MusicVolume", 1f);
            brightnessSlider.value = PlayerPrefs.GetFloat("Brightness", 1f);

            // Dynamically fill graphics dropdown with available quality levels
            graphicsDropdown.ClearOptions();
            graphicsDropdown.AddOptions(new List<string>(QualitySettings.names));

            int savedQuality = PlayerPrefs.GetInt("GraphicsQuality", QualitySettings.GetQualityLevel());
            graphicsDropdown.value = savedQuality;
            graphicsDropdown.RefreshShownValue();

            Debug.Log("Loaded Graphics Quality: " + QualitySettings.names[savedQuality]);

            brightnessOverlay.gameObject.SetActive(true);
            ApplyBrightness(brightnessSlider.value);
            ApplySettings();
        }
    }

    // ───────────────────────────────────────────────────────────────────────────
    // 1) OPEN PANEL METHODS (assign these to your Main Menu buttons)
    // ───────────────────────────────────────────────────────────────────────────

    /// <summary>
    /// Called by the “Credits” button in your Main Menu.
    /// </summary>
    public void OpenCreditsPanel()
    {
        if (creditsPanel != null)
            creditsPanel.SetActive(true);
    }

    /// <summary>
    /// Called by the “How To Play” button in your Main Menu.
    /// </summary>
    public void OpenHowToPlayPanel()
    {
        if (howToPlayPanel != null)
            howToPlayPanel.SetActive(true);
    }

    /// <summary>
    /// Called by the “Settings” button in your Main Menu.
    /// </summary>
    public void OpenSettingsPanel()
    {
        if (settingsPanel != null)
            settingsPanel.SetActive(true);
    }

    /// <summary>
    /// Called by the “Profile” button in your Main Menu.
    /// </summary>
    public void OpenProfilePanel()
    {
        if (profilePanel != null)
        {
            profilePanel.SetActive(true);

            // If you need to pull in the user’s data immediately,
            // you can call your FirebaseController here. For example:
            //
            // var ctrl = profilePanel.GetComponentInChildren<FirebaseController>();
            // if (ctrl != null) ctrl.RefreshProfileInfo();
            //
            // (Adjust this call to whatever your FirebaseController uses
            // to populate name/email fields, etc.)
        }
    }

    /// <summary>
    /// Load the next scene (Play button).
    /// </summary>
    public void Play()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    /// <summary>
    /// Quit application (Exit button).
    /// </summary>
    public void Exit()
    {
        Application.Quit();
    }

    // ───────────────────────────────────────────────────────────────────────────
    // 2) CLOSE PANEL METHODS (assign these to each panel’s × button)
    // ───────────────────────────────────────────────────────────────────────────

    /// <summary>
    /// Closes the Credits panel.
    /// </summary>
    public void CloseCreditsPanel()
    {
        if (creditsPanel != null)
        {
            creditsPanel.SetActive(false);
            Time.timeScale = 1f; // resume game/time if you had paused
        }
    }

    /// <summary>
    /// Closes the How To Play panel.
    /// </summary>
    public void CloseHowToPlayPanel()
    {
        if (howToPlayPanel != null)
        {
            howToPlayPanel.SetActive(false);
            Time.timeScale = 1f;
        }
    }

    /// <summary>
    /// Closes the Settings panel.
    /// </summary>
    public void CloseSettingsPanel()
    {
        if (settingsPanel != null)
        {
            if (isSettingsChanged)
            {
                if (warningPopup != null)
                    warningPopup.SetActive(true); // Show the warning panel or text
            }
            else
            {
                settingsPanel.SetActive(false);
                // Time.timeScale = 1f;
            }
        }
    }


    /// <summary>
    /// Closes the Profile panel.
    /// </summary>
    public void CloseProfilePanel()
    {
        if (profilePanel != null)
        {
            profilePanel.SetActive(false);
            Time.timeScale = 1f;
        }
    }

    // ───────────────────────────────────────────────────────────────────────────
    // 3) SETTINGS‐RELATED METHODS (copied/adapted from SettingsManager)
    // ───────────────────────────────────────────────────────────────────────────

    /// <summary>
    /// Call this on Music slider’s OnValueChanged(float).
    /// </summary>
    public void OnMusicSliderChanged(float value)
    {
        AudioListener.volume = value;
        isSettingsChanged = true;

    }

    /// <summary>
    /// Call this on Graphics dropdown’s OnValueChanged(int).
    /// </summary>
    public void OnGraphicsDropdownChanged(int index)
    {
        QualitySettings.SetQualityLevel(index, true);
        Debug.Log("Graphics quality set to: " + QualitySettings.names[index] + " (Level " + index + ")");
        isSettingsChanged = true;

    }

    /// <summary>
    /// Call this on Brightness slider’s OnValueChanged(float).
    /// </summary>
    public void OnBrightnessChanged(float value)
    {
        Color color = brightnessOverlay.color;

        // Optional: clamp value between 0 and 1 just in case
        value = Mathf.Clamp01(value);

        // Brightness range logic — 0 = dark, 1 = bright
        // Let's say full black is at alpha 0.5, not 1 (which is too dark)
        color.a = Mathf.Lerp(0f, 0.5f, 1f - value);

        brightnessOverlay.color = color;
        isSettingsChanged = true;

    }


    private void ApplyBrightness(float sliderValue)
    {
        if (brightnessOverlay != null)
        {
            float inverted = 1f - sliderValue;
            brightnessOverlay.color = new Color(0, 0, 0, inverted);
        }
    }

    /// <summary>
    /// Saves current slider/dropdown values into PlayerPrefs and reapplies.
    /// Call this on your “Save” button’s OnClick().
    /// </summary>
    public void SaveSettings()
    {
        if (musicSlider != null)
            PlayerPrefs.SetFloat("MusicVolume", musicSlider.value);

        if (graphicsDropdown != null)
            PlayerPrefs.SetInt("GraphicsQuality", graphicsDropdown.value);
        graphicsDropdown.RefreshShownValue();

        if (brightnessSlider != null)
            PlayerPrefs.SetFloat("Brightness", brightnessSlider.value);

        PlayerPrefs.Save();
        ApplySettings();
        isSettingsChanged = false; // Reset the flag after saving
    }

    /// <summary>
    /// Loads PlayerPrefs values into Audio, Quality, CanvasGroup.
    /// Called at Start and after SaveSettings().
    /// </summary>
    private void ApplySettings()
    {
        if (musicSlider != null)
            AudioListener.volume = musicSlider.value;

        if (graphicsDropdown != null)
            QualitySettings.SetQualityLevel(graphicsDropdown.value);

        if (brightnessOverlay != null)
        {
            float invertedBrightness = 1f - brightnessSlider.value;
            brightnessOverlay.color = new Color(0, 0, 0, invertedBrightness);
        }
    }
    
    public void CancelCloseAttempt()
    {
        if (warningPopup != null)
            warningPopup.SetActive(false);
    }

}
