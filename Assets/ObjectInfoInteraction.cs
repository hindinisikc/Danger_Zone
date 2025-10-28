using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ObjectInfoInteraction : MonoBehaviour
{
    public float interactionRange = 5f;      // Max distance to interact with the object
    public LayerMask interactableLayer;      // Layer for interactable objects
    private OverlayManager overlayManager;   // Reference to the OverlayManager
    private int currentObjectIndex = -1;     // Current object's overlay index

    void Start()
    {
        overlayManager = FindObjectOfType<OverlayManager>();  // Find the OverlayManager in the scene
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            if (overlayManager != null && overlayManager.isOverlayActive) // Assuming isOverlayActive is public in OverlayManager
            {
                overlayManager.CloseOverlay(); // Close the overlay if it's currently open
            }
            else
            {
                RaycastHit hit;
                // Perform a raycast from the camera's position
                if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, interactionRange))
                {
                    // Check if the hit object has the ObjectOverlayIndex component
                    ObjectOverlayIndex objectOverlayIndex = hit.collider.GetComponent<ObjectOverlayIndex>();

                    // Check if the object has the "ObjectInfo" tag
                    if (objectOverlayIndex != null && hit.collider.CompareTag("ObjectInfo"))
                    {
                        Debug.Log("Overlay object found: " + hit.collider.gameObject.name);
                        overlayManager.ShowOverlay(objectOverlayIndex.overlayIndex); // Show overlay with the correct index
                    }
                    else
                    {
                        Debug.Log("Hit object does not have ObjectOverlayIndex component or correct tag.");
                    }
                }
                else
                {
                    Debug.Log("No ObjectInfo objects hit.");
                }
            }
        }
    }
}

