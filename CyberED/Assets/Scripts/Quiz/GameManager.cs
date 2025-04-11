using UnityEngine;
using TMPro; // Import the TextMeshPro namespace
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;


public class GameManager : MonoBehaviour
{
    // Array to hold all categories (each category is a different QuestionData Scriptable Object)
    public QuestionData[] categories;

    // Reference to the selected category's QuestionData Scriptable Object
    private QuestionData selectedCategory;

    // Index to track the current question within the selected Category
    private int currentQuestionIndex = 0;

    // UI elements to display the question text, image, and reply buttons
    public TMP_Text questionText; // Use TMP_Text for TextMesh
    
    public TMP_Text finalScoreText; // Assign this in Unity Inspector
    public Image questionImage;
    public Button [] replyButtons;

    [Header("Score")]
    public ScoreManager score;
    // public Button nextButton;
    public int correctReply = 10;
    public int wrongReply = 5;
    public TextMeshPro scoreText;

    [Header("correctReplyIndex")]
    public int correctReplyIndex;
    int correctReplies;

    [Header("Game Finished Panel")]
    public GameObject GameFinished;
    void Start()
    {
        GameFinished.SetActive(false);
    //     if (nextButton != null)
    // {
    //     nextButton.gameObject.SetActive(false); // Hide the Next button at the start
    // }
    // else
    // {
    //     Debug.LogError("⚠️ Next button is not assigned in the Inspector!");
    // }

        if (categories == null || categories.Length == 0)
        {
            Debug.LogError("⚠️ Categories array is EMPTY! Assign categories in the Inspector.");
            return;
        }

        // Retrieve stored category index and ensure it's within bounds
        int selectedCategoryIndex = PlayerPrefs.GetInt("SelectedCategory", 0);
        
        if (selectedCategoryIndex < 0 || selectedCategoryIndex >= categories.Length)
        {
            Debug.LogWarning($"⚠ Invalid Category Index: {selectedCategoryIndex}. Resetting to 0.");
            selectedCategoryIndex = 0; // Reset to a valid index
            PlayerPrefs.SetInt("SelectedCategory", selectedCategoryIndex);
        }

        SelectCategory(selectedCategoryIndex);
        ResetQuizProgress();
    }

    // Method to select a category based on the player's choice
    // categoryIndex: the index of the category selected by the player
    public void SelectCategory(int categoryIndex)
    {
        // Set the selectedCategory to the chosen category's QuestionData Scriptable Object
        selectedCategory = categories[categoryIndex];

        // Reset the current question index to start from the first question in the selected category 
        currentQuestionIndex = 0;

        // Display the first question in the selected category 
        DisplayQuestion();
    }

    private void ResetQuizProgress()
    {
        currentQuestionIndex = 0; // Reset question index
        score.ResetScore(); // ✅ Reset score

        if (selectedCategory == null)
        {
            Debug.LogError("❌ Cannot reset progress: No category selected!");
            return;
        }
        PlayerPrefs.DeleteKey("LastQuestionIndex_" + selectedCategory.name); // Clear saved progress
    }
    // Method to display the current question 
    public void DisplayQuestion()
    {
        // Check if a category has been selected 
        if (selectedCategory == null || selectedCategory.questions == null || selectedCategory.questions.Length == 0)
        {
            Debug.LogError("No valid category or questions available!");
            return;
        }

        // Ensure the currentQuestionIndex is within bounds
        if (currentQuestionIndex < 0 || currentQuestionIndex >= selectedCategory.questions.Length)
        {
            Debug.LogError($"currentQuestionIndex out of range: {currentQuestionIndex}");
            return;
        }

        ResetButtons();

        // Get the current question from the selected category
        var question = selectedCategory.questions[currentQuestionIndex];

        // Set the question image in the UI (if any)
        if (question.questionImage != null)
        {
            questionImage.sprite = question.questionImage;
            questionImage.gameObject.SetActive(true);
        }
        else
        {
            questionImage.gameObject.SetActive(false);
        }

        // Set the question text in the UI 
        questionText.text = question.questionText;

        // Loop through all reply buttons and set their text to the corresponding replies
        for (int i = 0; i < replyButtons.Length; i++)
        {
            TMP_Text buttonText = replyButtons[i].GetComponentInChildren<TMP_Text>();
            buttonText.text = i < question.replies.Length ? question.replies[i] : "";
            replyButtons[i].gameObject.SetActive(i < question.replies.Length); // Disable extra buttons
        }
    }


    // Method to handle when a player selects a reply

    public void OnReplySelected(int replyIndex)
    {
        // Check if a category has been selected
        if (selectedCategory == null || selectedCategory.questions == null || selectedCategory.questions.Length == 0)
        {
            Debug.LogError("No valid category or questions available!");
            return;
        }

        // Ensure the currentQuestionIndex is within bounds
        if (currentQuestionIndex < 0 || currentQuestionIndex >= selectedCategory.questions.Length)
        {
            Debug.LogError($"currentQuestionIndex out of range: {currentQuestionIndex}");
            return;
        }

        Question currentQuestion = selectedCategory.questions[currentQuestionIndex];

        // Ensure the replyIndex is within the valid range
        if (replyIndex < 0 || replyIndex >= currentQuestion.replies.Length)
        {
            Debug.LogError($"replyIndex out of range: {replyIndex}");
            return;
        }

        // Check if the selected reply is correct
        if (replyIndex == currentQuestion.correctReplyIndex)
        {
            score.AddScore(correctReply);
            correctReplies++;
            Debug.Log("Correct Reply!");
        }
        else
        {
            score.SubtractScore(wrongReply);
            Debug.Log("Wrong Reply!");
        }

        // Move to the next question
        currentQuestionIndex++;
        SaveProgress();

        Debug.Log($"Current Question Index: {currentQuestionIndex}/{selectedCategory.questions.Length}");
        
        // Only display the next question if it exists
        if (currentQuestionIndex < selectedCategory.questions.Length)
        {
            DisplayQuestion();
        }
        else
        {
            ShowGameFinishedPanel();
            Debug.Log("Quiz Finished!");
        }
    }



    //Call this method when you want to show the correct reply
    public void ShowCorrectReply()
    {
        correctReplyIndex = selectedCategory.questions[currentQuestionIndex].correctReplyIndex;

        // Loop through all buttons
        for (int i = 0; i < replyButtons.Length; i++)
        {
            if (i == correctReplyIndex)
            {
                replyButtons[i].interactable = true; // Show the correct button
            }
            else
            {
                replyButtons[i].interactable = false; // Hide the incorrect buttons
            }
        }       
    }

    public void ResetButtons()
    {
        foreach (var button in replyButtons)
        {
            button.interactable = true;
        }
    }

    public void ShowGameFinishedPanel()
    {
        GameFinished.SetActive(true);

        if (ScoreManager.instance != null) // Ensure ScoreManager exists
        {
            int finalScore = ScoreManager.instance.score; // Get the actual score
            finalScoreText.text = finalScore.ToString(); // Display only the score
            Debug.Log($"🎯 Game Over! Final Score: {finalScore}");

            ScoreManager.instance.ResetScore(); // Reset the score properly
        }
        else
        {
            Debug.LogError("❌ ScoreManager instance is null! Make sure it is in the scene.");
            finalScoreText.text = "0"; // Display 0 if ScoreManager is missing
        }
        StartCoroutine(LoadNextSceneAfterDelay(3f)); // Wait 3 seconds and load the next scene
        // Invoke("LoadNextScene", 3f); // Wait 3 seconds, then load next scene
        // nextButton.gameObject.SetActive(true); // Show the next button
    }

     private IEnumerator LoadNextSceneAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
    // public void LoadNextScene()
    // {
    //     SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    // }



    private void SaveProgress()
    {
        PlayerPrefs.SetInt("LastQuestionIndex_" + selectedCategory.name, currentQuestionIndex);
        PlayerPrefs.Save();
    }

    // Load the saved progress for the selected category
    private void LoadProgress(int categoryIndex)
    {
        string categoryName = categories [categoryIndex].name;
        currentQuestionIndex = PlayerPrefs.GetInt("LastQuestionIndex_" + categoryName, 0);
    
        // Start the quiz from the Loaded progress
        DisplayQuestion();
    }
    public void ResetGame()
    {
        currentQuestionIndex = 0; // Reset question index
        score.ResetScore(); // ✅ Reset the score before restarting
        PlayerPrefs.DeleteKey("LastQuestionIndex_" + selectedCategory.name); // Reset quiz progress
        GameFinished.SetActive(false); // Hide the game finished panel
        DisplayQuestion(); // Start the quiz from the beginning
    }


}

