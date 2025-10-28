using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class LevelSelector : MonoBehaviour
{
    public LevelLoader levelLoader;

    [System.Serializable]
    public class LevelData
    {
        public string levelTitle;       // Title of the level
        public string levelDescription; // Description of the level
        public Sprite levelImage;       // Image of the level
        public int sceneIndex;          // Scene index for this level
    }

    public GameObject levelDescriptionOverlay; // Overlay to show level details
    public GameObject levelCarousel;           // The level selection carousel
    public GameObject backButton;              // Back button to main menu
    public TextMeshProUGUI selectLevelText;    // Select Level Text\
    public Canvas loadingScreenCanvas;

    public TextMeshProUGUI levelTitleText;     // Displays level title in overlay
    public TextMeshProUGUI levelDescriptionText; // Displays level description
    public Image levelImage;                   // Displays level image
    public Button startButton;                 // Button to start the level
    public Button backToCarouselButton;        // Back button to return to carousel

    public Button EQRunHistoryButton;
    public Button FireRunHistoryButton;
    public Button FloodRunHistoryButton;

    public LevelData[] levels; // Array containing all level data

    // Ensure that only the level carousel is visible on start
    private void Start()
    {
        levelDescriptionOverlay.SetActive(false); // Hide the overlay at start
        levelCarousel.SetActive(true);            // Show the level carousel
        backButton.SetActive(true);               // Ensure the back button is visible
        selectLevelText.gameObject.SetActive(true);
        backToCarouselButton.gameObject.SetActive(false);
        loadingScreenCanvas.gameObject.SetActive(false);
        HideViewRunHistoryButtons();
        
    }

    // Called when a level is selected from the carousel
    public void OnLevelSelected(int levelIndex)
    {
        // Existing debug logs
        Debug.Log("Selected Level Index: " + levelIndex);
        Debug.Log("Total Levels: " + levels.Length);

        // Index check
        if (levelIndex < 0 || levelIndex >= levels.Length)
        {
            Debug.LogError("Invalid level index selected.");
            return;
        }

        LevelData selectedLevel = levels[levelIndex];

        // Setting text and image
        levelTitleText.text = selectedLevel.levelTitle;
        levelDescriptionText.text = selectedLevel.levelDescription;
        levelImage.sprite = selectedLevel.levelImage;

        startButton.onClick.RemoveAllListeners();
        startButton.onClick.AddListener(() => StartLevel(selectedLevel.sceneIndex));

        levelDescriptionOverlay.SetActive(true);  // Activate overlay
        levelCarousel.SetActive(false);            // Deactivate carousel
        backButton.SetActive(false);               // Deactivate back button
        selectLevelText.gameObject.SetActive(false); // Deactivate Select Level Text
        backToCarouselButton.gameObject.SetActive(true); // Activate Back to Carousel Button
        ShowViewRunHistoryButton(levelIndex);

        // Additional debug log
        Debug.Log("Overlay activated: " + levelDescriptionOverlay.activeSelf);

        backToCarouselButton.onClick.RemoveAllListeners();
        backToCarouselButton.onClick.AddListener(BackToCarousel);
    }



    // Start the selected level
    private void StartLevel(int sceneIndex)
    {
        Debug.Log("StartLevel Activated, Scene Index: " + sceneIndex);
        // Begin loading the selected level
        levelLoader.LoadLevel(sceneIndex);
    }

    private void ShowViewRunHistoryButton(int sceneIndex)
    {
        switch (sceneIndex)
        {
            case 0:
                ShowEQRunHistoryButton();
                Debug.Log("EQRunHistoryButton Active");
                break;
            case 1:
                ShowFireRunHistoryButton();
                Debug.Log("FireRunHistoryButton Active");
                break;
            case 2:
                ShowFloodRunHistoryButton();
                Debug.Log("FloodRunHistoryButton Active");
                break;
        }
    }

    private void ShowEQRunHistoryButton()
    {
        EQRunHistoryButton.gameObject.SetActive(true);
        FireRunHistoryButton.gameObject.SetActive(false);
        FloodRunHistoryButton.gameObject.SetActive(false);
    }

    private void ShowFireRunHistoryButton()
    {
        EQRunHistoryButton.gameObject.SetActive(false);
        FireRunHistoryButton.gameObject.SetActive(true);
        FloodRunHistoryButton.gameObject.SetActive(false);
    }
    private void ShowFloodRunHistoryButton()
    {
        EQRunHistoryButton.gameObject.SetActive(false);
        FireRunHistoryButton.gameObject.SetActive(false);
        FloodRunHistoryButton.gameObject.SetActive(true);
    }

    private void HideViewRunHistoryButtons()
    {
        EQRunHistoryButton.gameObject.SetActive(false);
        FireRunHistoryButton.gameObject.SetActive(false);
        FloodRunHistoryButton.gameObject.SetActive(false);
    }

    // Return to the level carousel from the overlay
    private void BackToCarousel()
    {
        levelDescriptionOverlay.SetActive(false); // Hide the overlay
        levelCarousel.SetActive(true);            // Show the carousel again
        backButton.SetActive(true);               // Show the back button
        selectLevelText.gameObject.SetActive(true);
        backToCarouselButton.gameObject.SetActive(false);
    }
}
