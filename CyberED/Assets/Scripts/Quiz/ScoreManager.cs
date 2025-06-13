using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; // Import the TextMeshPro namespace to use TextMeshProUGUI

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager instance; // Singleton instance

    public int score; // Variable to store the player's current score
    public TextMeshProUGUI scoreText; // Reference to the TextMeshProUGUI component for displaying the score
    
    public static string ScoreKey = "PlayerScore";

    // Ensure only one instance exists
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Called when the script is first run
    void Start()
    {
        LoadScore();
        UpdateScoreText(); // Update the score display to show the initial score
    }

    // Adds points to the current score
    public void AddScore(int points)
    { 
        score += points; // Increase the score by the specified number of points
        UpdateScoreText(); // Update the score display to reflect the new score
        SaveScore();
    }

    // Subtracts points from the current score
    public void SubtractScore(int points)
    {
        score -= points; // Decrease the score by the specified number of points
        UpdateScoreText(); // Update the score display to reflect the new score
        SaveScore();
    }

    void UpdateScoreText()
    {
        if (scoreText != null)
        {
            scoreText.text = score.ToString(); // Update the TextMeshProUGUI component with the current score
        }
        else
        {
            Debug.LogError("⚠️ Score Text is not assigned in the Inspector!");
        }
    }

    public void SaveScore()
    {
        PlayerPrefs.SetInt(ScoreKey, score);
        PlayerPrefs.Save();
    }

    public void LoadScore()
    {
        if (PlayerPrefs.HasKey(ScoreKey))
        {
            score = PlayerPrefs.GetInt(ScoreKey);
        }
        else
        {
            score = 0;
        }
        UpdateScoreText();
    }

    public void ResetScore()
    {
        PlayerPrefs.DeleteKey(ScoreKey);
        PlayerPrefs.Save();
        score = 0;
        UpdateScoreText();
    }
}
