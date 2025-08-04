using UnityEngine;
using UnityEngine.SceneManagement;

public class FinalChoice : MonoBehaviour
{
    [Header("Button References")]
    public UnityEngine.UI.Button finishButton;
    public UnityEngine.UI.Button repairButton;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Set up button listeners
        if (finishButton != null)
        {
            finishButton.onClick.AddListener(OnFinishButtonPressed);
        }
        
        if (repairButton != null)
        {
            repairButton.onClick.AddListener(OnRepairButtonPressed);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// Handles the finish button press - loads the next scene
    /// </summary>
    public void OnFinishButtonPressed()
    {
        // Get the current scene index and load the next one
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = currentSceneIndex + 1;
        
        // Check if the next scene index is valid
        if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(nextSceneIndex);
        }
        else
        {
            Debug.LogWarning("No next scene available. Current scene is the last one in build settings.");
        }
    }

    /// <summary>
    /// Handles the repair button press - loads scene "THE END" or scene index 24
    /// </summary>
    public void OnRepairButtonPressed()
    {
        // Try to load scene by name first
        if (SceneUtility.GetBuildIndexByScenePath("Assets/Scenes/THE END.unity") != -1)
        {
            SceneManager.LoadScene("THE END");
        }
        // If scene name doesn't exist, try by index 24
        else if (24 < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(24);
        }
        else
        {
            Debug.LogError("Scene 'THE END' or scene index 24 not found in build settings!");
        }
    }

    /// <summary>
    /// Alternative method to load scene by name (can be called from UI buttons directly)
    /// </summary>
    public void LoadSceneByName(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    /// <summary>
    /// Alternative method to load scene by index (can be called from UI buttons directly)
    /// </summary>
    public void LoadSceneByIndex(int sceneIndex)
    {
        if (sceneIndex >= 0 && sceneIndex < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(sceneIndex);
        }
        else
        {
            Debug.LogError($"Scene index {sceneIndex} is out of range!");
        }
    }
}
