using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class OverlayData
{
    public string objectName;      // Name of the object or place
    public string description;     // Information about the object
    public Sprite objectImage;     // Image of the object or place
}

public class OverlayManager : MonoBehaviour
{
    public GameObject overlayPanel;              // The overlay UI panel
    public TextMeshProUGUI objectNameText;       // Text for the object's name
    public TextMeshProUGUI descriptionText;      // Text for object description
    public Image objectImage;                    // Image for the object/place
    public TextMeshProUGUI exitPromptText;       // "Press any key to exit" text

    public OverlayData[] overlays;               // Array of overlay data
    public bool isOverlayActive = false;

    void Start()
    {
        overlayPanel.SetActive(false);           // Ensure the overlay is hidden on start
    }

    //void Update()
    //{
    //    if (isOverlayActive && Input.GetKey(KeyCode.L))
    //    {
    //        CloseOverlay();
    //    }
    //}

    public void ShowOverlay(int overlayIndex)
    {
        Debug.Log("ShowOverlay called with index: " + overlayIndex); // Debug log to see if this is called

        if (overlayIndex < 0 || overlayIndex >= overlays.Length)
        {
            Debug.LogError("Invalid overlay index.");
            return;
        }

        OverlayData data = overlays[overlayIndex];
        objectNameText.text = data.objectName;
        descriptionText.text = data.description;
        objectImage.sprite = data.objectImage;
        exitPromptText.text = "Press any key to exit";

        Debug.Log("Overlay data: " + data.objectName + ", " + data.description);
        Debug.Log("Object image: " + (data.objectImage != null ? "Valid" : "Null"));


        overlayPanel.SetActive(true); // Ensure this line is reached
        Debug.Log("Overlay panel activated."); // Log to confirm activation
        Time.timeScale = 0f;  // Pause the game
        isOverlayActive = true;
    }


    public void CloseOverlay()
    {
        overlayPanel.SetActive(false);
        Time.timeScale = 1f;  // Resume the game
        isOverlayActive = false;
    }
}
