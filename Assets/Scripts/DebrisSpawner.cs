using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebrisSpawner : MonoBehaviour
{
	public GameObject debrisPrefab;  // Prefab of the debris object to spawn
	public int numberOfDebris = 50;  // Number of debris objects to spawn
	public Vector3 minSpawnPosition; // Minimum corner of the spawn area
	public Vector3 maxSpawnPosition; // Maximum corner of the spawn area
	public int debrisPerFrame = 10;  // Number of debris objects to spawn per frame

	// Method to start spawning debris
	public void StartSpawning()
	{
		StartCoroutine(SpawnDebris());
	}

	// Coroutine to spawn debris
	private IEnumerator SpawnDebris()
	{
		Debug.Log("Spawning Debris...");

		int spawned = 0;

		while (spawned < numberOfDebris)
		{
			for (int i = 0; i < debrisPerFrame && spawned < numberOfDebris; i++)
			{
				// Calculate a random spawn position within the defined bounds
				Vector3 spawnPosition = new Vector3(
					Random.Range(minSpawnPosition.x, maxSpawnPosition.x),
					Random.Range(minSpawnPosition.y, maxSpawnPosition.y),
					Random.Range(minSpawnPosition.z, maxSpawnPosition.z)
				);

				// Instantiate a debris object at the calculated spawn position with no rotation
				Instantiate(debrisPrefab, spawnPosition, Quaternion.identity);
				spawned++;
			}

			// Wait for one frame before continuing the loop to spawn more debris
			yield return null;
		}
	}
}