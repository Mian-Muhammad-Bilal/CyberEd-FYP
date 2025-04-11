using UnityEngine;
using UnityEngine.SceneManagement;

public class BG_MusicManager : MonoBehaviour
{
    private static BG_MusicManager instance;
    private AudioSource audioSource;

    private string[] storyScenes = { "AFTER 5 LEVELS", "AFTER 10 LEVELS", "AFTER 15 LEVELS", "AFTER BOOS LEVEL", "backstory", "THE END","IF NOVA DESTROYED","IF NOVA REAPIRED" }; // List of story scenes

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            audioSource = GetComponent<AudioSource>();

            if (audioSource != null)
            {
                audioSource.loop = true;
                audioSource.Play();
            }

            SceneManager.sceneLoaded += OnSceneLoaded; // Listen for scene changes
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (IsStoryScene(scene.name))
        {
            if (audioSource.isPlaying)
                audioSource.Pause(); // Pause music in story scenes
        }
        else
        {
            if (!audioSource.isPlaying)
                audioSource.UnPause(); // Resume music in normal scenes
        }
    }

    private bool IsStoryScene(string sceneName)
    {
        foreach (string storyScene in storyScenes)
        {
            if (sceneName == storyScene)
                return true;
        }
        return false;
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded; // Clean up event listener
    }
}
