using UnityEngine;
using UnityEngine.SceneManagement;
using Firebase;
using Firebase.Database;
using Firebase.Extensions;
using System.Collections.Generic;
using System.Linq;

public class LeaderboardManager : MonoBehaviour
{
    public GameObject runHistoryLDB; // Smaller prefab for general entries
    public GameObject runHistoryLDBGold;
    public GameObject runHistoryLDBSilver;
    public GameObject runHistoryLDBBronze;
    public Transform contentPanel;

    private DatabaseReference databaseRef;

    private void Start()
    {
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsCompletedSuccessfully)
            {
                databaseRef = FirebaseDatabase.DefaultInstance.RootReference;
                Debug.Log("Firebase initialized successfully.");
                HandleSceneSpecificLeaderboard();
            }
            else
            {
                Debug.LogError("Failed to initialize Firebase: " + task.Exception);
            }
        });
    }

    private void HandleSceneSpecificLeaderboard()
    {
        string sceneName = SceneManager.GetActiveScene().name;

        switch (sceneName)
        {
            case "EQ_RunHistory":
                Debug.Log("Opening gameDataEQ");
                FetchLeaderboardData("gameDataEQ");
                break;

            case "Fire_RunHistory":
                Debug.Log("Opening gameDataFire");
                FetchLeaderboardData("gameDataFire");
                break;

            case "Flood_RunHistory":
                Debug.Log("Opening gameDataFlood");
                FetchLeaderboardData("gameDataFlood");
                break;

            default:
                Debug.LogWarning("No leaderboard functionality for the current scene.");
                break;
        }
    }

    private void FetchLeaderboardData(string databasePath)
    {
        if (databaseRef != null)
        {
            databaseRef.Child(databasePath).GetValueAsync().ContinueWithOnMainThread(task =>
            {
                if (task.IsCompleted)
                {
                    DataSnapshot snapshot = task.Result;

                    if (snapshot.ChildrenCount > 0)
                    {
                        var entries = new List<RunHistoryEntryData>();

                        foreach (DataSnapshot entrySnapshot in snapshot.Children)
                        {
                            // Parse data
                            string playerName = entrySnapshot.Child("PlayerName").Value?.ToString() ?? "Unknown";
                            string dateAndTime = entrySnapshot.Child("DateAndTime").Value?.ToString() ?? "N/A";
                            string npcQuestsCompleted = entrySnapshot.Child("NPCQuestCompleted").Value?.ToString() ?? "0";
                            string mistakesMade = entrySnapshot.Child("MistakesMade").Value?.ToString() ?? "0";
                            string playerEndHP = entrySnapshot.Child("PlayerEndHP").Value?.ToString() ?? "0";
                            float completionTime = float.TryParse(entrySnapshot.Child("CompletionTime").Value?.ToString(), out float time) ? time : float.MaxValue;
                            int totalScore = int.TryParse(entrySnapshot.Child("TotalScore").Value?.ToString(), out int score) ? score : 0;

                            entries.Add(new RunHistoryEntryData(playerName, dateAndTime, npcQuestsCompleted, mistakesMade, playerEndHP, completionTime, totalScore));
                        }

                        // Sort entries
                        entries = entries.OrderByDescending(e => e.TotalScore)
                                         .ThenBy(e => e.CompletionTime)
                                         .ToList();

                        // Display entries
                        DisplayLeaderboardEntries(entries);
                    }
                    else
                    {
                        Debug.Log("No leaderboard entries found.");
                    }
                }
                else
                {
                    Debug.LogError("Error fetching leaderboard data: " + task.Exception);
                }
            });
        }
    }

    private void DisplayLeaderboardEntries(List<RunHistoryEntryData> entries)
    {
        for (int i = 0; i < entries.Count; i++)
        {
            GameObject prefabToUse = runHistoryLDB; // Default smaller prefab

            if (i == 0)
                prefabToUse = runHistoryLDBGold;
            else if (i == 1)
                prefabToUse = runHistoryLDBSilver;
            else if (i == 2)
                prefabToUse = runHistoryLDBBronze;

            GameObject newEntry = Instantiate(prefabToUse, contentPanel);
            RunHistoryLDBEntry entryScript = newEntry.GetComponent<RunHistoryLDBEntry>();

            if (entryScript != null)
            {
                entryScript.SetEntryData(entries[i]);
            }
        }
    }
}

public class RunHistoryEntryData
{
    public string PlayerName { get; }
    public string DateAndTime { get; }
    public string NPCQuestsCompleted { get; }
    public string MistakesMade { get; }
    public string PlayerEndHP { get; }
    public float CompletionTime { get; }
    public int TotalScore { get; }

    public RunHistoryEntryData(string playerName, string dateAndTime, string npcQuestsCompleted, string mistakesMade, string playerEndHP, float completionTime, int totalScore)
    {
        PlayerName = playerName;
        DateAndTime = dateAndTime;
        NPCQuestsCompleted = npcQuestsCompleted;
        MistakesMade = mistakesMade;
        PlayerEndHP = playerEndHP;
        CompletionTime = completionTime;
        TotalScore = totalScore;
    }
}