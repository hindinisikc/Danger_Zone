using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
	private LevelTimer levelTimer; //Reference Level Timer Script to Start Level Timer

    // Start is called before the first frame update
    private void Start()
	{
		// Get the LevelTimer component attached to the same GameObject
        levelTimer = GetComponent<LevelTimer>();

        if (levelTimer != null)
        {
            levelTimer.StartTimer();  // Start the timer when the game starts
            Debug.Log("Run Started");
        }
        else
        {
            Debug.LogError("LevelTimer component not found on this GameObject.");
        }

    }

	private void Update()
	{
		
	}
}
