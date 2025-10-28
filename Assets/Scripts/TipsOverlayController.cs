using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class TipsOverlayController : MonoBehaviour
{
    public static TipsOverlayController _instance;
    
    public GameObject tipsOverlay;  // The UI element for the tips overlay
    public Image tipImage;  // Image for displaying tip
    public TextMeshProUGUI tipText;  // Text for displaying tip
    public Button closeButton;  // Close button to close the overlay

    public UIController controller;
    public GameObject tipLibrary;

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
    }

    private void Start()
    {
        // Ensure the overlay is hidden initially
        tipsOverlay.SetActive(false);

        // Ensure the Tip Library is hidden initially
        tipLibrary.SetActive(false);

        // Add listener for close button
        closeButton.onClick.AddListener(CloseTipsOverlay);
    }

    private void Update()
    {
        // Close the overlay with E key if it's active
        if (tipsOverlay.activeSelf && Input.GetKeyDown(KeyCode.E))
        {
            CloseTipsOverlay();
        }
    }

    public void ShowTipsOverlay(Sprite newImage, string newText)
    {
        // Update the content of the overlay
        tipImage.sprite = newImage;
        tipText.text = newText;

        // Show the overlay
        tipsOverlay.SetActive(true);
        controller.CloseHUD();
    }

    public void CloseTipsOverlay()
    {
        //Clear Content of TipOverlay
        tipImage.sprite = null;
        tipText.text = string.Empty;
        
        // Hide the overlay
        tipsOverlay.SetActive(false);
        controller.OpenHUD();
    }
}