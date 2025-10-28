using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class LevelCompleteUI : MonoBehaviour
{
    public UIDocument uiDocument;
    private ScoreCalculator scoreCalculator;  // Reference to the ScoreCalculator
    private GameDataManager gameDataManager; // Reference to the GameDataManager

    void Start()
    {
        // Get the root VisualElement of the UI
        VisualElement root = uiDocument.rootVisualElement;

        // Find the buttons by their names from UI Builder
        Button viewLeaderboardButton = root.Q<Button>("ViewLeaderboard");
        Button retryLevelButton = root.Q<Button>("RetryLevel");
        Button levelSelectButton = root.Q<Button>("LevelSelect");
        Button mainMenuButton = root.Q<Button>("MainMenu");

        // Attach event listeners for each button to load the appropriate scene
        viewLeaderboardButton.clicked += () => LoadScene("LDB");
        retryLevelButton.clicked += () => LoadScene("DemoLevel");
        levelSelectButton.clicked += () => LoadScene("LevelSelectionMenu");
        mainMenuButton.clicked += () => LoadScene("MainMenu");

        // Find the ScoreCalculator instance in the scene
        scoreCalculator = FindObjectOfType<ScoreCalculator>();
        if (scoreCalculator == null)
        {
            Debug.LogError("ScoreCalculator not found in the scene.");
        }

        // Display the Player HP
        DisplayPlayerHP(root);

        // Display the Time Taken
        DisplayTimeTaken(root);

        // Display the number of NPCs Helped
        DisplayNPCsHelped(root);

        // Display the number of Quests Completed
        DisplayQuestsCompleted(root);

        //Display the number of mistakes made
        DisplayMistakesMade(root);

        // Display the Total Score
        DisplayTotalScore(root);

        //gameDataManager = GameObject.FindObjectOfType<GameDataManager>();
        //gameDataManager.SaveData();
    }

    // Function to load a scene based on the scene name
    void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    // Function to display the player HP on the "HPLabel"
    void DisplayPlayerHP(VisualElement root)
    {
        // Retrieve the player HP from PlayerPrefs
        float playerHP = PlayerPrefs.GetFloat("PlayerHealth", 0f);  // Default to 0f if not found

        // Find the "HPLabel" by its name in the UI
        Label playerHPLabel = root.Q<Label>("HPLabel");

        // Check if the label exists and update its text
        if (playerHPLabel != null)
        {
            playerHPLabel.text = playerHP.ToString();
            Debug.Log("PlayerHP: " + playerHP);
        }
        else
        {
            Debug.LogError("HPLabel not found in the UI.");
        }
    }

    // Function to display the player HP on the "HPLabel"
    void DisplayNPCsHelped(VisualElement root)
    {
        // Retrieve the number of NPCs helped from PlayerPrefs
        float NPCsHelped = PlayerPrefs.GetFloat("NPCsHelped", 0f);  // Default to 0f if not found

        // Find the "HPLabel" by its name in the UI
        Label NPCsHelpedLabel = root.Q<Label>("NPCsHelpedLabel");

        // Check if the label exists and update its text
        if (NPCsHelpedLabel != null)
        {
            NPCsHelpedLabel.text = NPCsHelped.ToString();
            Debug.Log("Number of NPCs Helped: " + NPCsHelped);
        }
        else
        {
            Debug.LogError("NPCsHelpedLabel not found in the UI.");
        }
    }

    // Function to display the final time on the "TimeTakenLabel"
    void DisplayTimeTaken(VisualElement root)
    {
        // Retrieve the final time from PlayerPrefs
        float finalTime = PlayerPrefs.GetFloat("FinalTime", 0f) /60;  // Default to 0f if not found

        // Find the "TotalTimeLabel" by its name in the UI
        Label timeTakenLabel = root.Q<Label>("TotalTimeLabel");

        // Check if the label exists and update its text
        if (timeTakenLabel != null)
        {
            timeTakenLabel.text = finalTime.ToString("F2") + " minutes";
            Debug.Log("Displayed Time: " + finalTime);
        }
        else
        {
            Debug.LogError("TimeTakenLabel not found in the UI.");
        }
    }

    // Function to display the number of Quests Completed on the "QuestsCompletedLabel"
    void DisplayQuestsCompleted(VisualElement root)
    {
        // Retrieve the number of Quests Completed from PlayerPrefs
        float questsCompleted = PlayerPrefs.GetFloat("QuestsCompleted", 0f);  // Default to 0f if not found

        // Find the "QuestsCompletedLabel" by its name in the UI
        Label questsCompletedLabel = root.Q<Label>("QuestsCompletedLabel");

        // Check if the label exists and update its text
        if (questsCompletedLabel != null)
        {
            questsCompletedLabel.text = questsCompleted.ToString();
            Debug.Log("Quests Completed: " + questsCompleted);
        }
        else
        {
            Debug.LogError("QuestsCompletedLabel not found in the UI.");
        }
    }

    // Function to display the number of mistakes made on the "MistakesLabel"
    void DisplayMistakesMade(VisualElement root)
    {
        // Retrieve the number of mistakes made from PlayerPrefs
        float mistakesMade = PlayerPrefs.GetFloat("MistakesMade", 0f);  // Default to 0f if not found

        // Find the "MistakesLabel" by its name in the UI
        Label mistakesMadeLabel = root.Q<Label>("MistakesLabel");

        // Check if the label exists and update its text
        if (mistakesMadeLabel != null)
        {
            mistakesMadeLabel.text = mistakesMade.ToString();
            Debug.Log("Mistakes Made: " + mistakesMade);
        }
        else
        {
            Debug.LogError("MistakesLabel not found in the UI.");
        }
    }

    // Function to display the Total Score on the "TotalScoreLabel"
    void DisplayTotalScore(VisualElement root)
    {
        // Retrieve the required data from PlayerPrefs
        float NPCsHelped = PlayerPrefs.GetFloat("NPCsHelped", 0f);
        float currentHP = PlayerPrefs.GetFloat("PlayerHealth", 0f);
        float totalTime = PlayerPrefs.GetFloat("FinalTime", 0f) / 60;
        float questsCompleted = PlayerPrefs.GetFloat("QuestsCompleted", 0f);
        float mistakesMade = PlayerPrefs.GetFloat("MistakesMade", 0f);
        float totalscore = PlayerPrefs.GetFloat("MistakesMade", 0f);

        if (scoreCalculator == null)
        {
            Debug.LogError("ScoreCalculator is not initialized.");
            return; // Prevent further execution if scoreCalculator is null
        }

        // Call CalculateScore in ScoreCalculator with the collected data
        float totalScore = scoreCalculator.CalculateScore(
            NPCsHelped,
            currentHP,
            totalTime,
            questsCompleted,
            mistakesMade
        );

        // Store TotalScore in PlayerPrefs
        PlayerPrefs.SetFloat("TotalScore", totalScore);
        Debug.Log("Total Score: " + totalScore);

        // Find the "TotalScoreLabel" by its name in the UI
        Label totalScoreLabel = root.Q<Label>("TotalScoreLabel");

        // Check if the label exists and update its text
        if (totalScoreLabel != null)
        {
            totalScoreLabel.text = totalScore.ToString();
            Debug.Log("Player Level Score: " + totalScore);
        }
        else
        {
            Debug.LogError("TotalScoreLabel not found in the UI.");
        }
    }



}
