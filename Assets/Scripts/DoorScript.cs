using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorScript : MonoBehaviour
{
	public Transform doorTransform; // The Transform of the door object
	public Vector3 openRotationInward;   // Rotation when the door opens inward
	public Vector3 openRotationOutward;  // Rotation when the door opens outward
	public Vector3 closedRotation;       // Rotation when the door is closed
	public float rotationSpeed = 2f;     // Speed at which the door rotates

	private bool isOpen = false;         // Is the door currently open?
	private Quaternion targetRotation;  // Target rotation for the door
	private bool playerNearby = false;  // Is the player near the door?
	private Transform playerTransform;  // Reference to the player's Transform


	// Start is called before the first frame update
	void Start()
    {
		// Ensure the door starts at the closed rotation
		doorTransform.localRotation = Quaternion.Euler(closedRotation);
		targetRotation = Quaternion.Euler(closedRotation);
	}

    // Update is called once per frame
    void Update()
    {
		// Smoothly rotate the door toward the target rotation
		doorTransform.localRotation = Quaternion.Lerp(doorTransform.localRotation, targetRotation, Time.deltaTime * rotationSpeed);

		// Check if the player presses the "E" key and is near the door
		if (playerNearby && Input.GetKeyDown(KeyCode.E))
		{
			ToggleDoor();
		}
	}

	public void ToggleDoor()
	{
		// Toggle the door state
		isOpen = !isOpen;

		if (isOpen)
		{
			// Determine whether to open inward or outward based on player's position
			Vector3 playerToDoor = playerTransform.position - doorTransform.position;
			Vector3 doorForward = doorTransform.forward;

			bool openInward = Vector3.Dot(playerToDoor, doorForward) > 0; // Positive dot product means the player is in front of the door
			targetRotation = openInward ? Quaternion.Euler(openRotationInward) : Quaternion.Euler(openRotationOutward);
		}
		else
		{
			// Close the door
			targetRotation = Quaternion.Euler(closedRotation);
		}
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Player"))
		{
			playerNearby = true; // Player is near the door
			playerTransform = other.transform; // Cache the player's Transform
		}
	}

	private void OnTriggerExit(Collider other)
	{
		if (other.CompareTag("Player"))
		{
			playerNearby = false; // Player left the door area
			playerTransform = null; // Clear the player's Transform
		}
	}

}
