using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MistakeTrigger : MonoBehaviour
{
    private MistakeTracker mistakeTracker;
    private bool mistakeRecorded = false;

    private void Start()
    {
        // Find the MistakeTracker in the scene
        mistakeTracker = FindObjectOfType<MistakeTracker>();
    }

    private void OnTriggerEnter(Collider other)
    {
        // Check if the player entered the trigger area
        if (other.CompareTag("Player"))
        {
            // Assuming the player has a PlayerCover script with an isInCover property
            Player player = other.GetComponent<Player>();

            // If the player is not in cover, record a mistake
            if (player != null && !player.IsInCover() && !mistakeRecorded)
            {
                mistakeTracker.IncrementMistake();
                mistakeRecorded = true;
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Player player = other.GetComponent<Player>();

            // Check if player enters cover, reset the mistake state
            if (player != null && player.IsInCover())
            {
                mistakeRecorded = false;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Reset mistake recording upon exit
            mistakeRecorded = false;
        }
    }
}
