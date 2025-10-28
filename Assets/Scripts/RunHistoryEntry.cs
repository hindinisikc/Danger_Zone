using System;
using UnityEngine;
using TMPro;

public class RunHistoryEntry : MonoBehaviour
{
    // Cached TMP_Text references for improved performance
    private TMP_Text completionStatusText;
    private TMP_Text completionTimeText;
    private TMP_Text dateAndTimeText;
    private TMP_Text mistakesMadeText;
    private TMP_Text npcQuestCompletedText;
    private TMP_Text playerEndHPText;
    private TMP_Text totalScoreText;

    void Awake()
    {
        // Cache the TMP_Text components to improve performance (no need to call Find every time)
        completionStatusText = transform.Find("CompletionStatus")?.GetComponent<TMP_Text>();
        completionTimeText = transform.Find("CompletionTime")?.GetComponent<TMP_Text>();
        dateAndTimeText = transform.Find("DateAndTime")?.GetComponent<TMP_Text>();
        mistakesMadeText = transform.Find("MistakesMade")?.GetComponent<TMP_Text>();
        npcQuestCompletedText = transform.Find("NPCQuestCompleted")?.GetComponent<TMP_Text>();
        playerEndHPText = transform.Find("PlayerEndHP")?.GetComponent<TMP_Text>();
        totalScoreText = transform.Find("TotalScore")?.GetComponent<TMP_Text>();

        // Log errors if any of the components are missing
        if (!completionStatusText) Debug.LogError("CompletionStatus text field not found.");
        if (!completionTimeText) Debug.LogError("CompletionTime text field not found.");
        if (!dateAndTimeText) Debug.LogError("DateAndTime text field not found.");
        if (!mistakesMadeText) Debug.LogError("MistakesMade text field not found.");
        if (!npcQuestCompletedText) Debug.LogError("NPCQuestCompleted text field not found.");
        if (!playerEndHPText) Debug.LogError("PlayerEndHP text field not found.");
        if (!totalScoreText) Debug.LogError("TotalScore text field not found.");
    }

    public void PopulateFields(string completionStatus, string completionTime, string dateAndTime,
                               string mistakesMade, string npcQuestCompleted, string playerEndHP, int totalScore)
    {
        // Format DateAndTime to show MM-DD-YYYY and Hour:Minute (HH:mm)
        string formattedDateAndTime = FormatDateAndTime(dateAndTime);

        // Assign values to the fields if they exist
        if (completionStatusText) completionStatusText.text = completionStatus;
        if (completionTimeText) completionTimeText.text = completionTime;
        if (dateAndTimeText) dateAndTimeText.text = formattedDateAndTime;
        if (mistakesMadeText) mistakesMadeText.text = mistakesMade;
        if (npcQuestCompletedText) npcQuestCompletedText.text = npcQuestCompleted;
        if (playerEndHPText) playerEndHPText.text = playerEndHP;
        if (totalScoreText) totalScoreText.text = totalScore.ToString();
    }

    // Method to format the DateAndTime string to MM-DD-YYYY Hour:Minute (MM-DD-YYYY HH:mm)
    private string FormatDateAndTime(string dateAndTime)
    {
        try
        {
            // Attempt to parse the dateAndTime string
            DateTime parsedDate;
            if (DateTime.TryParse(dateAndTime, out parsedDate))
            {
                // Format the Date as MM-DD-YYYY and Time as HH:mm
                return parsedDate.ToString("MM-dd-yyyy HH:mm");
            }
            else
            {
                // If the format is invalid, return the original string
                Debug.LogWarning("Invalid DateAndTime format: " + dateAndTime);
                return dateAndTime;
            }
        }
        catch (Exception ex)
        {
            Debug.LogError("Error parsing DateAndTime: " + ex.Message);
            return dateAndTime;  // Return the original string if an error occurs
        }
    }
}