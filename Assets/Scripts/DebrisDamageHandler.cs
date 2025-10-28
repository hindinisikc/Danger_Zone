using UnityEngine;

public class DebrisDamageHandler : MonoBehaviour
{
	public float damageAmount = 10f; // Public property to set damage amount

	private void OnCollisionEnter(Collision collision)
	{
		if (collision.gameObject.CompareTag("Player"))
		{
			Player playerHealth = collision.gameObject.GetComponent<Player>();
			if (playerHealth != null)
			{
				playerHealth.TakeDamage(damageAmount);
				Debug.Log("Debris hit the player and dealt " + damageAmount + " damage.");
			}
		}
	}
}