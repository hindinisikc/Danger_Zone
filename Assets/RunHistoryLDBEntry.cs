using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RunHistoryLDBEntry : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI playerNameText;
    [SerializeField] private TextMeshProUGUI dateAndTimeText;
    [SerializeField] private TextMeshProUGUI npcQuestsCompletedText;
    [SerializeField] private TextMeshProUGUI mistakesMadeText;
    [SerializeField] private TextMeshProUGUI playerEndHPText;
    [SerializeField] private TextMeshProUGUI completionTimeText;
    [SerializeField] private TextMeshProUGUI totalScoreText;

    /// <summary>
    /// Sets the data for this leaderboard entry.
    /// </summary>
    public void SetEntryData(RunHistoryEntryData data)
    {
        if (playerNameText != null) playerNameText.text = data.PlayerName;
        if (dateAndTimeText != null) dateAndTimeText.text = data.DateAndTime;
        if (npcQuestsCompletedText != null) npcQuestsCompletedText.text = data.NPCQuestsCompleted;
        if (mistakesMadeText != null) mistakesMadeText.text = data.MistakesMade;
        if (playerEndHPText != null) playerEndHPText.text = data.PlayerEndHP;
        if (completionTimeText != null) completionTimeText.text = data.CompletionTime.ToString("F2");
        if (totalScoreText != null) totalScoreText.text = data.TotalScore.ToString();
    }
}
