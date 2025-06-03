using UnityEngine;
using UnityEngine.SceneManagement;

public class StartupLoader : MonoBehaviour
{
    void Start()
    {
        SceneManager.LoadScene("MainMenu"); // or whatever your first level is
    }
}
