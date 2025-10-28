using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectEquippable : MonoBehaviour
{
	public bool isSpecialItem; // Toggle if it's a special item (appears on the right)

	[SerializeField] private Vector3 equippedRotation = new Vector3(0f, 90f, 0f); // Custom rotation only for equippable items
	// Start is called before the first frame update
	void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
		
	}

	public void ApplyEquippedRotation(Transform targetTransform)
	{
		targetTransform.rotation = Quaternion.Euler(equippedRotation);
	}

}
