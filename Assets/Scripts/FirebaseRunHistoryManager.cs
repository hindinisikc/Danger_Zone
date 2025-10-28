using UnityEngine;
using UnityEngine.SceneManagement; // Required for SceneManager
using Firebase;
using Firebase.Database;
using Firebase.Extensions;
using Firebase.Auth;

public class FirebaseRunHistoryManager : MonoBehaviour
{
    public GameObject runHistoryPrefab;  // Prefab for the Run History entry
    public Transform contentPanel;      // The content panel of the ScrollView

    private DatabaseReference databaseRef;
    private string currentPlayerName;

    private void Start()
    {
        // Initialize Firebase
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsCompletedSuccessfully)
            {
                databaseRef = FirebaseDatabase.DefaultInstance.RootReference;
                Debug.Log("Firebase initialized successfully.");

                // Initialize currentPlayerName based on the logged-in user
                InitializePlayerName();

                // Call the appropriate function based on the current scene
                HandleSceneSpecificRunHistory();
            }
            else
            {
                Debug.LogError("Failed to initialize Firebase: " + task.Exception);
            }
        });
    }

    private void InitializePlayerName()
    {
        // Check if the user is authenticated using Firebase Authentication
        FirebaseUser user = FirebaseAuth.DefaultInstance.CurrentUser;

        if (user != null)
        {
            currentPlayerName = user.DisplayName ?? user.UserId;  // Use display name or user ID if display name is not set
            Debug.Log("Current player name initialized: " + currentPlayerName);
        }
        else
        {
            Debug.LogError("No user is logged in. Set a default player name.");
            currentPlayerName = "Admin";  // You can set a default player name if needed
        }
    }

    private void HandleSceneSpecificRunHistory()
    {
        string sceneName = SceneManager.GetActiveScene().name;
        Debug.Log("Current Scene Detected: " + sceneName);

        switch (sceneName)
        {
            case "EQ_RunHistory":
                FetchEQRunHistory();
                break;

            case "Fire_RunHistory":
                FetchFireRunHistory();
                break;

            case "Flood_RunHistory":
                FetchFloodRunHistory();
                break;

            default:
                Debug.LogWarning("No matching run history function for the scene: " + sceneName);
                break;
        }
    }

    public void FetchEQRunHistory()
    {
        FetchRunHistoryForLevel("gameDataEQ");
    }

    public void FetchFireRunHistory()
    {
        FetchRunHistoryForLevel("gameDataFire");
    }

    public void FetchFloodRunHistory()
    {
        FetchRunHistoryForLevel("gameDataFlood");
    }

    private void FetchRunHistoryForLevel(string databasePath)
    {
        if (databaseRef != null && !string.IsNullOrEmpty(currentPlayerName))
        {
            databaseRef.Child(databasePath)
                .OrderByChild("PlayerName")
                .EqualTo(currentPlayerName)
                .GetValueAsync().ContinueWithOnMainThread(task =>
                {
                    if (task.IsCompleted)
                    {
                        DataSnapshot snapshot = task.Result;

                        Debug.Log($"Snapshot Raw Data for {databasePath}: {snapshot.GetRawJsonValue()}");

                        if (snapshot.ChildrenCount > 0)
                        {
                            Debug.Log($"Found {snapshot.ChildrenCount} entries for player: {currentPlayerName} in {databasePath}");

                            // Iterate through all matching entries
                            foreach (DataSnapshot entrySnapshot in snapshot.Children)
                            {
                                string completionStatus = entrySnapshot.Child("CompletionStatus").Value?.ToString() ?? "Unknown";
                                string rawCompletionTime = entrySnapshot.Child("CompletionTime").Value?.ToString() ?? "0";
                                string dateAndTime = entrySnapshot.Child("DateAndTime").Value?.ToString() ?? "Unknown";
                                string mistakesMade = entrySnapshot.Child("MistakesMade").Value?.ToString() ?? "0";
                                string npcQuestCompleted = entrySnapshot.Child("NPCQuestCompleted").Value?.ToString() ?? "0";
                                string playerEndHP = entrySnapshot.Child("PlayerEndHP").Value?.ToString() ?? "0";
                                int totalScore = int.TryParse(entrySnapshot.Child("TotalScore").Value?.ToString(), out int score) ? score : 0;

                                // Parsing the completion time into minutes:seconds format
                                float totalSeconds;
                                string completionTime;
                                if (float.TryParse(rawCompletionTime, out totalSeconds))
                                {
                                    int minutes = Mathf.FloorToInt(totalSeconds) / 60;
                                    int seconds = Mathf.FloorToInt(totalSeconds) % 60;
                                    completionTime = $"{minutes:D2}:{seconds:D2}";
                                }
                                else
                                {
                                    Debug.LogWarning("Failed to parse CompletionTime. Setting to default '00:00'.");
                                    completionTime = "00:00";
                                }

                                // Call AddRunHistoryEntry to display the data in the UI
                                AddRunHistoryEntry(completionStatus, completionTime, dateAndTime, mistakesMade, npcQuestCompleted, playerEndHP, totalScore);
                            }
                        }
                        else
                        {
                            Debug.Log("No entries found for player.");
                        }
                    }
                    else
                    {
                        Debug.LogError("Error fetching Firebase data: " + task.Exception);
                    }
                });
        }
        else
        {
            Debug.LogError("Database reference is null or player name is not set.");
        }
    }

    private void AddRunHistoryEntry(string completionStatus, string completionTime, string dateAndTime,
                                    string mistakesMade, string npcQuestCompleted, string playerEndHP, int totalScore)
    {
        if (runHistoryPrefab == null || contentPanel == null)
        {
            Debug.LogError("RunHistoryPrefab or ContentPanel is not assigned in the Inspector.");
            return;
        }

        // Instantiate prefab and populate fields
        GameObject newEntry = Instantiate(runHistoryPrefab, contentPanel);
        RunHistoryEntry entryScript = newEntry.GetComponent<RunHistoryEntry>();

        if (entryScript != null)
        {
            entryScript.PopulateFields(completionStatus, completionTime, dateAndTime,
                                       mistakesMade, npcQuestCompleted, playerEndHP, totalScore);
        }
        else
        {
            Debug.LogError("RunHistoryEntry script is missing on the prefab.");
        }
    }
}