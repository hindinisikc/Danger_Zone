using Ignis;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireObject : MonoBehaviour
{
    private FlammableObject flammableObject;

    void Start()
    {
        flammableObject = GetComponent<FlammableObject>();

        if (flammableObject == null)
        {
            Debug.LogError("FireObject requires a FlammableObject script on the same GameObject.");
        }

        // Optionally, adjust collider size to account for flameCatchAreaAddition
        Collider collider = GetComponent<Collider>();
        if (collider != null && collider is BoxCollider boxCollider)
        {
            boxCollider.size += flammableObject.flameCatchAreaAddition;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        // Trigger only if the object is on fire
        if (flammableObject != null && flammableObject.onFire)
        {
            if (other.CompareTag("Player"))
            {
                PlayerFireMechanic playerFireMechanic = other.GetComponent<PlayerFireMechanic>();

                if (playerFireMechanic != null)
                {
                    playerFireMechanic.Ignite();
                }
            }
        }
    }
}