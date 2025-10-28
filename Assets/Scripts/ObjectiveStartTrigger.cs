using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectiveStartTrigger : MonoBehaviour
{
    public int objectiveID; // Set this to the ID of the Escape the Danger Zone objective

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // Ensure your player has the "Player" tag
        {
            ObjectivesManager.Instance.ActivateObjective(objectiveID);
            Destroy(gameObject); // Destroy the trigger if it's a one-time activation
        }
    }
}
