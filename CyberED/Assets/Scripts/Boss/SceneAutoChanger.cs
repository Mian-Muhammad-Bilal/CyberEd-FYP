using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneAutoChanger : MonoBehaviour
{
    public float sceneChangeDelay = 60f; // Set in Inspector or default to 30
    //public string nextSceneName;

    void Start()
    {
        Invoke("LoadNextScene", sceneChangeDelay);
    }

    void LoadNextScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
