using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPickUpDrop : MonoBehaviour
{
    [SerializeField] private Transform playerCameraTransform;
    [SerializeField] private Transform objectGrabPointTransform;
    [SerializeField] private Transform specialGrabPointTransform; // The special grab point for equippable items
    [SerializeField] private LayerMask pickUpLayerMask;
    private ObjectGrabbable objectGrabbable;
    private ObjectEquippable objectEquippable; // To track if an object is equippable

    private Player playerScript;

    [SerializeField] private string crowbarTag = "Crowbar";

    private void Start()
    {
        playerScript = GetComponent<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        // Change from E to G key for dropping items
        if (Input.GetKeyDown(KeyCode.G))
        {
            if (objectGrabbable != null)
            {
                // Drop the item if we are holding something
                objectGrabbable.Drop();
                objectGrabbable = null;
                objectEquippable = null; // Reset the equippable reference
            }
        }

        // Handle item pickup functionality with 'E'
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (objectGrabbable == null)
            {
                // Not carrying an object, try to grab
                float pickUpDistance = 4f;

                if (Physics.Raycast(playerCameraTransform.position, playerCameraTransform.forward, out RaycastHit raycastHit, pickUpDistance))
                {
                    if (raycastHit.transform.CompareTag("Medkit")) // Check for Medkit
                    {
                        playerScript.hasMedkit = true; // Add medkit to player
                        Destroy(raycastHit.transform.gameObject); // Remove medkit from scene
                        Debug.Log("Medkit picked up");
                    }

                    if (raycastHit.transform.TryGetComponent(out objectGrabbable))
                    {
                        // Check if it's an equippable item
                        if (raycastHit.transform.TryGetComponent(out objectEquippable))
                        {
                            // Use the special grab point for equippable items
                            objectGrabbable.Grab(specialGrabPointTransform);

                            Rigidbody rb = raycastHit.transform.GetComponent<Rigidbody>();
                            if (rb != null)
                            {
                                // Freeze rotation to prevent it from rotating while held
                                rb.freezeRotation = true;
                            }
                        }
                        else
                        {
                            // Regular grab for non-equippable items
                            objectGrabbable.Grab(objectGrabPointTransform);
                        }
                    }

                    if (raycastHit.transform.CompareTag("Fire Extinguisher"))
                    {
                        if (raycastHit.transform.TryGetComponent(out objectGrabbable))
                        {
                            objectGrabbable.Grab(specialGrabPointTransform);

                            // Check if it's the fire extinguisher
                            if (raycastHit.transform.CompareTag("Fire Extinguisher"))
                            {
                                playerScript.SetEquippedItem("Fire Extinguisher");
                            }
                        }
                    }
                }
            }
        }

        // Optional debug or testing for holding the crowbar
        if (Input.GetKeyDown(KeyCode.T)) // Example key to test
        {
            Debug.Log("Is holding crowbar: " + IsHoldingCrowbar());
        }
    }
  

    public bool IsHoldingCrowbar()
    {
        return objectGrabbable != null && objectGrabbable.CompareTag(crowbarTag);
    }
}