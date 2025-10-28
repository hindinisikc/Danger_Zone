using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneTransition : MonoBehaviour
{
    public GameObject continueText; // Reference to the continue text UI element
    private bool isTransitioning = false; // Flag to prevent multiple transitions

    void Start()
    {
        continueText.SetActive(true); // Ensure the text is visible
    }

    void Update()
    {
        // Check for any key press or mouse click
        if (Input.anyKeyDown || Input.GetMouseButtonDown(0))
        {
            // Only allow transitioning if not already transitioning
            if (!isTransitioning)
            {
                isTransitioning = true; // Set the flag to true
                LoadNextScene(); // Call the method to load the next scene
            }
        }
    }

    void LoadNextScene()
    {
        // Replace "NextSceneName" with the actual name of the next scene
        SceneManager.LoadScene("MainMenu");
    }
}
