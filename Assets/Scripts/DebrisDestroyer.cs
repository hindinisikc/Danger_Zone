using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebrisDestroyer : MonoBehaviour
{
	public float destroyHeight = -10f;  // Height at which debris will be destroyed

	// Start is called before the first frame update
	void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
		// Check if the debris has fallen below the destroy height
		if (transform.position.y < destroyHeight)
		{
			// Destroy the debris object
			Destroy(gameObject);
		}
	}
}
