using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelCompleteTrigger : MonoBehaviour
{
    public GameObject levelEndCanvas;    // Reference to the Level End Screen Canvas
    public GameObject hudCanvas;         // Reference to the HUD Canvas
    public TextMeshProUGUI timeTakenText;  // Reference to the Completion Time Text
    public TextMeshProUGUI playerHPText;   // Reference to the Player HP Text
    public TextMeshProUGUI mistakesMadeText;  // Reference to the Mistakes Made Text
    public MistakeTracker mistakesTracker; // Reference to the Mistake Tracker
    public GameDataManager gameDataManager;
    public TextMeshProUGUI totalScoreText;   // Reference to the Total Score Text

    public string levelName;
    private string completionStatus;

    public GameObject pauseMenu;          // Reference to Pause Menu UI
    public GameObject mapView;            // Reference to Map View UI
    public GameObject objectivesTab;      // Reference to Objectives Tab UI

    private LevelTimer levelTimer;  // Reference to the LevelTimer
    private bool levelCompleted = false;  // To track if the level has been completed
    private bool isLevelComplete = false; // Flag to block keyboard inputs
    private int baseScore = 40;  // Starting base score

    void Start()
    {
        // Find the LevelTimer instance in the scene
        levelTimer = FindObjectOfType<LevelTimer>();
        if (levelTimer == null)
        {
            Debug.LogError("LevelTimer not found in the scene.");
        }
        else
        {
            levelTimer.StartTimer();
        }

        // Make sure the Level End Canvas is inactive at the start
        levelEndCanvas.SetActive(false);
        Debug.Log("LevelCompleteScreen Turned Off");
    }

    void Update()
    {
        // Disable all keyboard input when level is completed
        if (isLevelComplete)
        {
            // Block all key presses
            if (Input.anyKeyDown)
            {
                return; // Ignore all key inputs
            }
        }
        else
        {
            // Allow normal game input when level isn't completed
            // Your existing input controls for the Pause Menu, Map View, and Objectives Tab here
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Check if the object entering the trigger is tagged as "Player"
        if (other.CompareTag("Player") && !levelCompleted)
        {
            levelCompleted = true;  // Mark the level as completed

            // Access the Player script to get current health
            Player player = other.GetComponent<Player>();
            if (player != null)
            {
                float currentHP = player.GetCurrentHealth();
                int finalScore = CalculateFinalScore(currentHP);
                DisplayLevelEndScreen(currentHP, finalScore);
                gameDataManager.SaveData();
            }
            else
            {
                Debug.LogError("Player component not found on the player!");
            }
        }
    }

    private void DisplayLevelEndScreen(float playerHP, int totalScore)
    {
        // Deactivate the HUD Canvas
        if (hudCanvas != null)
        {
            hudCanvas.SetActive(false);
        }

        // Disable Pause Menu, Map View, and Objectives Tab
        if (pauseMenu != null) pauseMenu.SetActive(false);
        if (mapView != null) mapView.SetActive(false);
        if (objectivesTab != null) objectivesTab.SetActive(false);

        // Activate the level end canvas
        levelEndCanvas.SetActive(true);

        // Time taken (in minutes)
        float totalTime = levelTimer.GetFinalTime();
        int minutes = Mathf.FloorToInt(totalTime / 60);
        int seconds = Mathf.FloorToInt(totalTime % 60);
        Debug.Log("Is Timer Running?: " + levelTimer.IsTimerRunning());

        // Update the text fields with the player's stats
        timeTakenText.text = minutes.ToString("00") + ":" + seconds.ToString("00");  // Display minutes and seconds
        playerHPText.text = playerHP.ToString();  // Display player's HP
        mistakesMadeText.text = mistakesTracker.GetMistakeCount().ToString();  // Display mistakes made
        totalScoreText.text = totalScore.ToString();  // Display total score

        // Pause the game and block other input
        Time.timeScale = 0f; // Pause the game

        // Unlock the cursor and show it
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public string GetCurrentLevelName()
    {
        return levelName;
    }

    public string GetCompletionStatus()
    {
        completionStatus = "Completed";
        return completionStatus;
    }

    public int CalculateFinalScore(float playerEndHP)
    {
        // Time taken (converted to minutes)
        float totalTime = levelTimer.GetFinalTime();
        int minutesTaken = Mathf.FloorToInt(totalTime / 60);

        // Mistakes made (retrieved from PlayerPrefs or another method)
        int mistakesMade = mistakesTracker.GetMistakeCount(); //PlayerPrefs.GetInt("MistakesMade");

        // HP Score Calculation
        int HPScore = 0;
        if (playerEndHP >= 76) HPScore = 20;
        else if (playerEndHP >= 51) HPScore = 15;
        else if (playerEndHP >= 26) HPScore = 10;
        else if (playerEndHP >= 1) HPScore = 5;

        // Time Score Calculation
        int timeScore = 0;
        if (minutesTaken <= 5) timeScore = 20;
        else if (minutesTaken <= 7) timeScore = 15;
        else if (minutesTaken <= 10) timeScore = 10;
        else timeScore = 5;

        // Mistakes Score Calculation
        int mistakeScore = 0;
        if (mistakesMade == 0) mistakeScore = 20;
        else if (mistakesMade <= 3) mistakeScore = 15;
        else if (mistakesMade <= 6) mistakeScore = 10;
        else mistakeScore = 5;

        // Total Score Calculation
        int totalScore = baseScore + HPScore + timeScore + mistakeScore;
        return totalScore;
    }

    public void ResumeGame()
    {
        // Unpause the game and hide the Level End Screen (just in case you need this function later)
        Time.timeScale = 1f;
        levelEndCanvas.SetActive(false);
    }

    // Button Functions
    public void QuitToLevelSelect()
    {
        Time.timeScale = 1f;
        // Load Level Select Scene
        UnityEngine.SceneManagement.SceneManager.LoadScene("LevelSelectionMenu");
    }

    public void QuitToMainMenu()
    {
        Time.timeScale = 1f;
        // Load Main Menu Scene
        UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
