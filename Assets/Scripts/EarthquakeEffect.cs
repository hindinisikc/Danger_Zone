using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EarthquakeEffect : MonoBehaviour
{
	public float earthquakeDuration = 5.0f;   // Duration of the earthquake effect in seconds
	public float earthquakeMagnitude = 0.2f;  // Magnitude of the earthquake effect, controlling the intensity of shaking
	private bool isShaking = false;           // Flag indicating if the earthquake effect is currently active
	private float shakeEndTime;               // Time when the earthquake effect should end
	private Transform cameraTransform;        // Reference to the main camera's transform
	private Vector3 originalCameraPosition;   // Original position of the main camera before shaking begins


	private Player player;  // Reference to the player
    private float damageInterval = 1f; // Interval for taking damage (1 second)
    private float lastDamageTime; // Tracks the last time damage was applied

	private HashSet<Collider> triggeredColliders = new HashSet<Collider>();

	// Start is called before the first frame update
	void Start()
	{
		// Get the main camera's transform
		cameraTransform = Camera.main?.transform;

		// Check if the main camera transform is found
		if (cameraTransform == null)
		{
			Debug.LogError("Main Camera is not found. Please ensure the camera is tagged as 'MainCamera'.");
		}
		else
		{
			// Store the original position of the camera
			originalCameraPosition = cameraTransform.localPosition;
		}

		// Find the player reference
		player = FindObjectOfType<Player>();
	}

	// Update is called once per frame
	void Update()
	{
		// If the earthquake effect is active and the camera transform is not null
		if (isShaking && cameraTransform != null)
		{
			// Check if the current time is before the shake end time
			if (Time.time < shakeEndTime)
			{
				// Generate a random shake position within a sphere and clamp the y-axis to control vertical shake
				Vector3 shakePosition = Random.insideUnitSphere * earthquakeMagnitude;
				shakePosition.y = Mathf.Clamp(shakePosition.y, -earthquakeMagnitude, earthquakeMagnitude);

				// Apply the shake position to the camera's local position
				cameraTransform.localPosition = originalCameraPosition + shakePosition;
			}
			else
			{
				// End the earthquake effect and reset the camera to its original position
				isShaking = false;
				cameraTransform.localPosition = originalCameraPosition;
			}
		}

		// Apply earthquake damage to the player if not in cover
		if (isShaking && player != null && Time.time - lastDamageTime >= damageInterval)
		{
			if (!player.IsInCover()) // Check if player is not in cover
			{
				player.TakeDamage(1);  // Apply 1 damage to the player
				lastDamageTime = Time.time; // Update last damage time
			}
		}
	}

	// Method to start the earthquake effect
	public void StartEarthquake()
	{
		// Check if the camera transform is assigned
		if (cameraTransform != null)
		{
			// Log to confirm that earthquake is starting
			Debug.Log("Starting Earthquake Effect!");

			// Activate the earthquake effect
			isShaking = true;
			shakeEndTime = Time.time + earthquakeDuration; // Calculate the end time of the earthquake effect
		}
		else
		{
			// Log an error if the camera transform is not assigned
			Debug.LogError("Cannot start earthquake because the camera is not assigned.");
		}
	}

	// Trigger method to detect player collision and trigger the earthquake
	private void OnTriggerEnter(Collider other)
	{
		// Check if the object that collided is the player and if this collider has not triggered the earthquake before
		if (other.CompareTag("Player") && !triggeredColliders.Contains(other))
		{
			// Mark this collider as having triggered the earthquake
			triggeredColliders.Add(other);

			// Start the earthquake effect
			StartEarthquake();
		}
	}

	// Optional: If you want to reset the triggered colliders when they leave the trigger
	private void OnTriggerExit(Collider other)
	{
		// Optionally clear the collider from the set when it exits the trigger zone
		if (other.CompareTag("Player"))
		{
			triggeredColliders.Remove(other);
		}
	}

}

