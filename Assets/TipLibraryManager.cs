using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TipLibraryManager : MonoBehaviour
{
    public static TipLibraryManager Instance;

    public GameObject tipLibraryUI;   // The TipLibrary UI Panel
    public Image tipImage;           // Displays the tip image
    public TextMeshProUGUI tipText;  // Displays the tip text
    public TextMeshProUGUI tipNumber; // Displays the "Tip X of Y" text
    public Button nextButton;        // Button to navigate to the next tip
    public Button previousButton;    // Button to navigate to the previous tip
    public Button closeButton;       // Button to close the TipLibrary

    public UIController uiController;

    private List<Tip> tips = new List<Tip>(); // Stores all collected tips
    private int currentIndex = 0;            // Tracks the currently displayed tip

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    private void Start()
    {
        tipLibraryUI.SetActive(false); // Ensure the library UI is hidden initially

        // Assign button click listeners
        nextButton.onClick.AddListener(() => NavigateTips(1));
        previousButton.onClick.AddListener(() => NavigateTips(-1));
        closeButton.onClick.AddListener(ToggleTipLibrary);
    }

    private void Update()
    {
        // Toggle TipLibrary UI with the T key
        if (Input.GetKeyDown(KeyCode.T))
        {
            ToggleTipLibrary();
        }

        // Go to Next Tip
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            NavigateTips(1);
        }

        // Go to Previous Tip
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            NavigateTips(-1);
        }
    }

    public void AddTip(Tip newTip)
    {
        // Avoid adding duplicate tips
        if (!tips.Exists(t => t.id == newTip.id))
        {
            tips.Add(newTip);

            // Sort the tips by their numeric order
            tips.Sort((a, b) => a.id.CompareTo(b.id));
        }
    }

    public void ToggleTipLibrary()
    {
        bool isLibraryCurrentlyActive = tipLibraryUI.activeSelf; // Check the current state
        tipLibraryUI.SetActive(!isLibraryCurrentlyActive); // Toggle the state

        if (tipLibraryUI.activeSelf) // TipLibrary is now open
        {
            uiController.CloseHUD(); // Close the HUD
            if (tips.Count > 0)
            {
                ShowTip(currentIndex); // Show the current tip
            }
        }
        else // TipLibrary is now closed
        {
            uiController.OpenHUD(); // Reopen the HUD
        }
    }

    private void NavigateTips(int direction)
    {
        if (tips.Count == 0) return;

        // Update the index with wrapping
        currentIndex = (currentIndex + direction + tips.Count) % tips.Count;
        ShowTip(currentIndex);
    }

    private void ShowTip(int index)
    {
        if (index < 0 || index >= tips.Count) return;

        // Update the UI elements
        tipImage.sprite = tips[index].image;
        tipText.text = tips[index].text;

        // Update TipNumber (e.g., "Tip 2 of 5")
        tipNumber.text = $"Tip {index + 1} of {tips.Count}";
    }
}

[System.Serializable]
public class Tip
{
    public int id;           // Numeric order of the tip
    public Sprite image;     // Tip image
    public string text;      // Tip text
}