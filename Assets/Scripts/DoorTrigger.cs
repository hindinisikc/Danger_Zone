using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DoorTrigger : MonoBehaviour
{
	private DoorController doorController;  // Reference to the DoorController script attached to the parent GameObject

	// Start is called before the first frame update
	void Start()
	{
		// Get the DoorController component from the parent GameObject
		doorController = GetComponentInParent<DoorController>();
	}

	// Triggered when another collider enters the trigger collider attached to this GameObject
	private void OnTriggerEnter(Collider other)
	{
		// Check if the entering collider belongs to the player
		if (other.CompareTag("Player"))
		{
			doorController.isPlayerNear = true;  // Set the isPlayerNear flag in the DoorController to true
			Debug.Log("Player entered the trigger area");  // Log message to console
		}
	}

	// Triggered when another collider exits the trigger collider attached to this GameObject
	private void OnTriggerExit(Collider other)
	{
		// Check if the exiting collider belongs to the player
		if (other.CompareTag("Player"))
		{
			doorController.isPlayerNear = false;  // Set the isPlayerNear flag in the DoorController to false
			Debug.Log("Player exited the trigger area");  // Log message to console
		}
	}

	// Update is called once per frame
	void Update()
	{
		// This method is currently empty. You can add update logic here if needed.
	}
}
