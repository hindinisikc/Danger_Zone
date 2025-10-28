using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ignis;
public class FireExtinguisherHandler : MonoBehaviour
{
	[SerializeField] private ParticleSystem fireExtinguisherPS;
	[SerializeField] private Transform playerCameraTransform; // Reference to the player's camera
	[SerializeField] private LayerMask flammableLayer; // Layer for flammable objects
	[SerializeField] private float maxUsageTime = 10f; // Maximum time the extinguisher can be used
	[SerializeField] private AudioSource fireExtinguisherAudioSource; // AudioSource for fire extinguisher sound
	[SerializeField] private AudioClip fireExtinguisherSound; // Sound when using fire extinguisher

	private bool isParticleSystemActive;
	private float remainingUsageTime;


	// Start is called before the first frame update
	void Start()
    {
		if (fireExtinguisherPS == null)
		{
			Debug.LogError("FireExtinguisherHandler: ParticleSystem not assigned!");
		}

		if (playerCameraTransform == null)
		{
			Debug.LogError("FireExtinguisherHandler: Player Camera Transform not assigned!");
		}

		if (fireExtinguisherAudioSource == null)
		{
			Debug.LogError("FireExtinguisherHandler: AudioSource not assigned!");
		}

		if (fireExtinguisherSound == null)
		{
			Debug.LogError("FireExtinguisherHandler: AudioClip not assigned!");
		}

		fireExtinguisherPS.Stop();
		isParticleSystemActive = false;
		remainingUsageTime = maxUsageTime; // Initialize usage time
	}

    // Update is called once per frame
    void Update()
    {
		if (isParticleSystemActive)
		{
			if (remainingUsageTime > 0)
			{
				// Align the particle system's rotation with the player's camera forward direction
				AlignParticleSystemWithCamera();
				CheckForCollisions();

				// Decrease the remaining usage time
				remainingUsageTime -= Time.deltaTime;
				Debug.Log($"Remaining Usage Time: {remainingUsageTime:F2} seconds");

				if (remainingUsageTime <= 0)
				{
					remainingUsageTime = 0;
					ToggleParticleSystem(); // Automatically turn off when time runs out
					Debug.Log("Extinguisher is out of usage time!");
				}
			}
		}
	}

	public void ToggleParticleSystem()
	{
		if (isParticleSystemActive)
		{
			fireExtinguisherPS.Stop();
			fireExtinguisherAudioSource.Stop(); // This ensures the sound stops when the extinguisher is turned off
		}
		else if (remainingUsageTime > 0) // Only allow activation if there is remaining time
		{
			fireExtinguisherPS.Play();
			fireExtinguisherAudioSource.clip = fireExtinguisherSound;
			fireExtinguisherAudioSource.loop = true; // Enable looping
			fireExtinguisherAudioSource.Play(); // Start the looping sound
		}
		else
		{
			Debug.Log("Cannot use extinguisher; no remaining usage time!");
		}

		isParticleSystemActive = !isParticleSystemActive;
	}

	private void AlignParticleSystemWithCamera()
	{
		// Rotate the particle system to match the camera's forward direction
		fireExtinguisherPS.transform.rotation = Quaternion.LookRotation(playerCameraTransform.forward);
	}

	private void CheckForCollisions()
	{
		// Create a sphere in front of the particle system to detect collisions
		float range = 2f; // Adjust as needed
		Vector3 position = fireExtinguisherPS.transform.position + fireExtinguisherPS.transform.forward * range;

		Collider[] hitColliders = Physics.OverlapSphere(position, range, flammableLayer);
		foreach (Collider hitCollider in hitColliders)
		{
			FlammableObject flammable = hitCollider.GetComponent<FlammableObject>();
			if (flammable != null && flammable.IsOnFire())
			{
				Debug.Log("Extinguishing fire on: " + flammable.gameObject.name);
				flammable.ResetObj(); // Extinguish the fire
				Destroy(flammable.gameObject); // Destroy the object
			}
		}
	}

	public void ResetUsageTime()
	{
		remainingUsageTime = maxUsageTime; // Call this method to reset usage time if needed
		Debug.Log("Fire extinguisher usage time reset!");
	}


}
