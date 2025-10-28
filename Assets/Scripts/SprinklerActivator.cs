using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SprinklerActivator : MonoBehaviour
{
	public string sprinklerTag = "Sprinkler"; // Tag for sprinkler objects
	public KeyCode activationKey = KeyCode.E; // Key to activate sprinklers
	public float activationRange = 3f; // Range within which the player can activate sprinklers

	private Transform playerTransform; // Reference to the player's transform

	public LayerMask flammableLayer; // Layer mask for flammable objects
	public float detectionRadius = 50f; // Radius to detect flammable objects

	// Start is called before the first frame update
	void Start()
    {
		// Find the player in the scene
		GameObject player = GameObject.FindWithTag("Player");
		if (player != null)
		{
			playerTransform = player.transform;
		}
		else
		{
			Debug.LogError("Player not found in the scene. Make sure the Player object has the 'Player' tag.");
		}
	}

    // Update is called once per frame
    void Update()
    {
		if (playerTransform != null && Vector3.Distance(playerTransform.position, transform.position) <= activationRange)
		{
			if (Input.GetKeyDown(activationKey))
			{
				ActivateSprinklers();
				DestroyFlammableObjects();
			}
		}
	}

	void ActivateSprinklers()
	{
		GameObject[] sprinklers = GameObject.FindGameObjectsWithTag(sprinklerTag);
		foreach (GameObject sprinkler in sprinklers)
		{
			ParticleSystem ps = sprinkler.GetComponent<ParticleSystem>();
			if (ps != null)
			{
				ps.Play(); // Activate the particle system
			}
		}

		Debug.Log("Sprinklers activated!");
	}

	void DestroyFlammableObjects()
	{
		// Find all objects in the scene within the detection radius on the flammable layer
		Collider[] flammableObjects = Physics.OverlapSphere(transform.position, detectionRadius, flammableLayer);

		foreach (Collider obj in flammableObjects)
		{
			Debug.Log($"Destroying flammable object: {obj.gameObject.name}");
			Destroy(obj.gameObject);
		}

		Debug.Log("Flammable objects destroyed!");
	}


}
