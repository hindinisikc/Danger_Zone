using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenuPanel; // Reference to the pause menu panel
    public GameObject hudCanvas; // Reference to the HUD canvas

    // Public references for buttons
    public Button resumeButton;
    public Button levelSelectButton;
    public Button quitToMainMenuButton;
    public Button quitGameButton;

    // Reference to Menu Selector for Quit Modal
    public MenuSelector menuSelector;

    public GameOverScreen gameOverScreen; // Reference to the GameOverScreen to check if the game is over

    private bool isPaused = false; // Flag to track if the game is paused

    void Start()
    {
        // Ensure the pause menu is inactive at the start
        pauseMenuPanel.SetActive(false);

        // Add listeners for button clicks
        resumeButton.onClick.AddListener(Resume);
        levelSelectButton.onClick.AddListener(LoadLevelSelect);
        quitToMainMenuButton.onClick.AddListener(LoadMainMenu);
        quitGameButton.onClick.AddListener(QuitGame);
    }

    void Update()
    {
        // Check for the Escape key press
        if (Input.GetKeyDown(KeyCode.Escape) && !gameOverScreen.IsGameOver())
        {
            if (isPaused)
            {
                Resume(); // Resume the game if it's currently paused
            }
            else
            {
                Pause(); // Pause the game if it's currently running
            }
        }
    }

    void Pause()
    {
        pauseMenuPanel.SetActive(true); // Show the pause menu
        hudCanvas.SetActive(false); // Hide the HUD
        Time.timeScale = 0f; // Pause the game
        isPaused = true; // Set the paused flag to true

        // Show the cursor and make it interactable
        Cursor.lockState = CursorLockMode.None; // Unlock cursor
        Cursor.visible = true; // Make cursor visible
        Debug.Log("Game Paused");
    }

    public void Resume()
    {
        pauseMenuPanel.SetActive(false); // Hide the pause menu
        hudCanvas.SetActive(true); // Show the HUD
        Time.timeScale = 1f; // Resume the game
        isPaused = false; // Set the paused flag to false

        // Hide the cursor and lock it back to the game
        Cursor.lockState = CursorLockMode.Locked; // Lock cursor
        Cursor.visible = false; // Hide cursor
        Debug.Log("Game Resumed");
    }

    // Implement these methods based on your scene management
    public void LoadLevelSelect()
    {
        // Load Level Select scene (replace "LevelSelectScene" with your actual scene name)
        UnityEngine.SceneManagement.SceneManager.LoadSceneAsync("LevelSelectionMenu");
        Debug.Log("Loading Level Select");
    }

    public void LoadMainMenu()
    {
        // Load Main Menu scene (replace "MainMenuScene" with your actual scene name)
        UnityEngine.SceneManagement.SceneManager.LoadSceneAsync("MainMenu");
    }

    public void QuitGame()
    {
        menuSelector.ShowQuitModal(); // Show Quit Modal instead of automatically quitting
    }
}
