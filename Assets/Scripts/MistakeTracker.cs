using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MistakeTracker : MonoBehaviour
{
    private int mistakeCount = 0;

    // Increment mistakes by 1
    public void IncrementMistake()
    {
        mistakeCount++;
    }

    // Add a custom number of mistakes
    public void AddMistakes(int count)
    {
        if (count > 0)
        {
            mistakeCount += count;
            Debug.Log($"Added {count} mistakes. Total mistakes: {mistakeCount}");
        }
    }

    // Getter for mistake count if needed elsewhere
    public int GetMistakeCount()
    {
        return mistakeCount;
    }
}
