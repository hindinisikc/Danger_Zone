using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCQuestTracker : MonoBehaviour
{
    private int NPCQuestCompleted = 0;

    // Increment NPCQuestCompleted
    public void IncrementNPCQuestComplete()
    {
        NPCQuestCompleted++;
    }

    // Add Custom NPCQuestCompleted
    public void AddNPCQuestCompleted(int count)
    {
        if (count > 0)
        {
            NPCQuestCompleted += count;
            Debug.Log($"Added {count} mistakes. Total mistakes: {NPCQuestCompleted}");
        }
    }

    // Getter for NPCQuestCompleted number
    public int GetNPCQuestCompleted()
    {
        return NPCQuestCompleted;
    }
}
