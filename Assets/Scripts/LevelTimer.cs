using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelTimer : MonoBehaviour
{
    private float levelTime = 0f;  // Time passed in the level
    private bool isTimerRunning = false;  // To track if the timer is active

    // Call this to start the timer at the beginning of the level
    public void StartTimer()
    {
        levelTime = 0f;
        isTimerRunning = true;
    }

    // Call this when the player reaches the end to stop the timer
    public void StopTimer()
    {
        isTimerRunning = false;
    }

    // Call this to get the final time (e.g., for displaying at the end)
    public float GetFinalTime()
    {
        return levelTime;
    }

    void Update()
    {
        // If the timer is running, increase the time by the delta time between frames
        if (isTimerRunning)
        {
            levelTime += Time.deltaTime;
        }
    }

    public bool IsTimerRunning()
    {
        return isTimerRunning;
    }
}
