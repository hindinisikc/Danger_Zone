using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebrisDamage : MonoBehaviour
{
	// Damage amount that the debris will inflict on the player
	public float damageAmount = 10f;
	public float coverDamageReduction = 0.5f;

	// Called when a collision occurs
	private void OnCollisionEnter(Collision collision)
	{
		// Check if the collided object has the tag "Player"
		if (collision.gameObject.CompareTag("Player"))
		{
			// Get the Player component attached to the collided object
			Player player = collision.gameObject.GetComponent<Player>();

			// Ensure that the Player component exists on the collided object
			if (player != null)
			{
				// Check if the player is in cover
				if (player.IsInCover())
				{
					// Mitigate damage while in cover (reduce damage by 50%)
					float reducedDamage = damageAmount * coverDamageReduction;
					player.TakeDamage(reducedDamage);
					Debug.Log("Debris hit the player but damage was reduced to " + reducedDamage + " due to cover.");
				}
				else
				{
					// Full damage if the player is not in cover
					player.TakeDamage(damageAmount);
					Debug.Log("Debris hit the player and dealt " + damageAmount + " damage.");
				}
			}
		}
	}
}
