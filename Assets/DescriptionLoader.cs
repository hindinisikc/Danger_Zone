using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelSelectionOverlay : MonoBehaviour
{
    public GameObject levelOverlay; // Reference to the overlay panel
    public Text levelDescriptionText; // Reference to the description text
    public Button startLevelButton; // Reference to the Start Level button
    public Button backButton; // Reference to the Back button
    public GameObject levelSelectionMenu; // Reference to the level selection menu (carousel, buttons, etc.)

    // Method to activate the overlay and set the description
    public void ShowLevelOverlay(int selectedLevel)
    {
        levelOverlay.SetActive(true); // Show the overlay
        levelSelectionMenu.SetActive(false); // Hide the level selection menu
        UpdateDescription(selectedLevel); // Update the description based on selected level
    }

    // Method to update the description based on the selected level
    private void UpdateDescription(int level)
    {
        switch (level)
        {
            case 1:
                levelDescriptionText.text = "Escape the crumbling school building, evading falling debris and helping people along the way.";
                break;
            case 2:
                levelDescriptionText.text = "Survive the zombie apocalypse while searching for the legendary katana to save your village.";
                break;
            case 3:
                levelDescriptionText.text = "Navigate through treacherous terrains and defeat powerful foes to reclaim your honor.";
                break;
            default:
                levelDescriptionText.text = "Select a level to begin your journey!";
                break;
        }
    }

    // Method for starting the level
    public void StartLevel()
    {
        // Load the selected level here (use level index to determine which one)
        // Example: UnityEngine.SceneManagement.SceneManager.LoadScene("Level" + selectedLevel);
    }

    // Method to go back to the level selection carousel
    public void BackToCarousel()
    {
        levelOverlay.SetActive(false); // Hide the overlay
        levelSelectionMenu.SetActive(true); // Show the level selection menu
        // Optionally: Reset any values or states if necessary
    }
}

