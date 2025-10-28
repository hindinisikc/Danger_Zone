using SWS;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

public class TriggerEarthquake : MonoBehaviour
{
	private bool hasTriggered = false;

	// Start is called before the first frame update
	void Start()
	{

	}

	// Update is called once per frame
	void Update()
	{

	}

	private void OnTriggerEnter(Collider other)
	{
		// Ensure the earthquake effect triggers only once when the player enters the trigger zone
		if (!hasTriggered && other.CompareTag("Player"))
		{
			// Find and trigger the earthquake effect
			EarthquakeEffect earthquakeEffect = FindObjectOfType<EarthquakeEffect>();
			if (earthquakeEffect != null)
			{
				Debug.Log("Starting earthquake effect...");
				earthquakeEffect.StartEarthquake();
			}
			else
			{
				Debug.LogWarning("EarthquakeEffect script not found in the scene.");
			}

			// Find and start spawning debris
			DebrisSpawner debrisSpawner = FindObjectOfType<DebrisSpawner>();
			if (debrisSpawner != null)
			{
				debrisSpawner.StartSpawning();
			}
			else
			{
				Debug.LogWarning("DebrisSpawner script not found in the scene.");
			}

			// Destroy the game object after triggering the earthquake
			Destroy(gameObject);
		}
	}
}