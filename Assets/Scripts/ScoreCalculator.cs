using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreCalculator : MonoBehaviour
{
    public float NoOfNPCs = 5;
    public float NoOfQuests = 5;

    private float playerScore = 0;

    public float CalculateScore(float NPCsHelped, float playerEndHP, float timeTaken, float questsCompleted, float mistakesMade)
    {
        float HPScore = 0;
        if (playerEndHP >= 91)
            HPScore = 10;
        else if (playerEndHP >= 71)
            HPScore = 7;
        else
            HPScore = 5;

        float percentageHelped = (float)NPCsHelped / NoOfNPCs * 100;
        float NPCScore = 0;
        if (percentageHelped == 100)
            NPCScore = 15;
        else if (percentageHelped >= 75)
            NPCScore = 10;
        else if (percentageHelped >= 50)
            NPCScore = 7;
        else
            NPCScore = 5;

        float timeScore = timeTaken <= 5 ? 10 : (timeTaken <= 7 ? 7 : (timeTaken <= 10 ? 5 : 3));

        float percentageCompleted = (float)questsCompleted / NoOfQuests * 100;
        float QuestScore = 0;
        if (percentageCompleted == 100)
            QuestScore = 10;
        else if (percentageCompleted >= 75)
            QuestScore = 7;
        else if (percentageCompleted >= 50)
            QuestScore = 5;
        else
            QuestScore = 3;

        float mistakeScore = mistakesMade == 0 ? 15 : (mistakesMade <= 3 ? 10 : (mistakesMade <= 6 ? 7 : 5));

        playerScore = 40 + (HPScore + NPCScore + timeScore + QuestScore + mistakeScore);
        return playerScore;
    }

    public void DisplayScore()
    {
        Debug.Log("Player Score: " + playerScore);
    }
}

