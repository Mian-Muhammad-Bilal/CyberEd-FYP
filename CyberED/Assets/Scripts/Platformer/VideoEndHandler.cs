using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class VideoEndHandler : MonoBehaviour
{
    public VideoPlayer videoPlayer;  // Assign the VideoPlayer in Inspector
    public GameObject buttonGameObject; // Assign the Button GameObject in Inspector

    // void Start()
    // {
    //     buttonGameObject.SetActive(false); // Hide the button initially
    //     videoPlayer.loopPointReached += OnVideoFinished; // Subscribe to event
    // }

    // void OnVideoFinished(VideoPlayer vp)
    // {
    //     buttonGameObject.SetActive(true); // Show the button when video finishes
    // }

    public void LoadNextScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1); // Load the next scene
    }
    public void LoadMainMenu()
    {
        SceneManager.LoadScene("MainMenu"); // Load the next scene
    }
}
