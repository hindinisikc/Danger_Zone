using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class TipTriggerArea : MonoBehaviour
{
    // Public properties for the tip content
    public Sprite areaImage;  // Image for this trigger area
    public string areaText;   // Text for this trigger area

    // Internal flag to check if the tip has already been shown
    private bool isTriggered = false;

    private void OnTriggerEnter(Collider other)
    {
        // Check if the collider is the player and the tip hasn't been triggered yet
        if (other.CompareTag("Player") && !isTriggered /*&& GameSettings.Instance.areTipsEnabled*/)
        {
            // Mark as triggered to prevent reactivation
            isTriggered = true;

            // Create a new Tip object and add it to the TipLibraryManager
            Tip triggeredTip = new Tip { id = int.Parse(gameObject.name), image = areaImage, text = areaText };
            TipLibraryManager.Instance.AddTip(triggeredTip);

            // Show the tip using the overlay controller
            TipsOverlayController._instance.ShowTipsOverlay(areaImage, areaText);
        }
    }
}