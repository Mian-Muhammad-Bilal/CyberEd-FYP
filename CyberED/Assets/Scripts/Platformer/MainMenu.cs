using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class MainMenu1: MonoBehaviour
{
    public void Play()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1); // Load the next scene
        // SceneManager.LoadScene("level1");
        //  SceneManager.LoadScene("Quiz");
    }
    
    //public void ResetQuiz()
    //{ 
   //     PlayerPrefs.DeleteAll(); 
    //}
    public void Exit()
    {
        Application.Quit();
    }
}