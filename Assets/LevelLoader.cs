using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class LevelLoader : MonoBehaviour
{
    public GameObject loadingScreenCanvas;      // Reference to the loading screen Canvas
    public Slider progressBar;                 // Reference to the progress bar (optional)
    public TextMeshProUGUI progressText;       // Optional text to show the progress percentage
    public TextMeshProUGUI loadingLevelText;   // Text that shows loading status
    public TextMeshProUGUI levelNameText;      // Reference to the LevelNameText

    public Image backgroundImage;              // Reference to the UI background image
    public Sprite[] levelBackgrounds;          // Array of level-specific background images

    private string[] levelNames = { "Earthquake Level", "Fire Level", "Flood Level" }; // Level names array

    public void LoadLevel(int sceneIndex)
    {
        if (loadingScreenCanvas == null || levelNameText == null || backgroundImage == null)
        {
            Debug.LogError("One or more required references are missing!");
            return;
        }

        // Show the loading screen
        loadingScreenCanvas.SetActive(true);

        // Update the level name text
        int adjustedSceneIndex = sceneIndex - 3; // Adjust index based on your scene setup
        if (adjustedSceneIndex >= 0 && adjustedSceneIndex < levelNames.Length)
        {
            levelNameText.text = levelNames[adjustedSceneIndex];
        }
        else
        {
            Debug.LogError("Scene index out of range for level names.");
        }

        // Update the background image
        if (adjustedSceneIndex >= 0 && adjustedSceneIndex < levelBackgrounds.Length)
        {
            backgroundImage.sprite = levelBackgrounds[adjustedSceneIndex];
        }
        else
        {
            Debug.LogWarning("Scene index out of range for level backgrounds. Using default background.");
        }

        // Start loading the level
        StartCoroutine(LoadSceneAsync(sceneIndex));
    }

    private IEnumerator LoadSceneAsync(int sceneIndex)
    {
        // Activate the loading screen canvas
        loadingScreenCanvas.SetActive(true);

        // Start loading the scene asynchronously
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneIndex);

        // Prevent the scene from activating immediately after loading
        operation.allowSceneActivation = false;

        // Update the progress bar and text while loading
        while (!operation.isDone)
        {
            // Get the loading progress, ranging from 0 to 1
            float progress = Mathf.Clamp01(operation.progress / 0.9f);

            // Update progress bar and text
            progressBar.value = progress;
            progressText.text = $"{(progress * 100):0}%";

            // Check if the load is complete
            if (operation.progress >= 0.9f)
            {
                // Allow scene activation
                operation.allowSceneActivation = true;
                loadingLevelText.text = "Loading Complete";
            }

            yield return null; // Wait until the next frame
        }

        // Deactivate loading screen canvas once the scene is loaded
        loadingScreenCanvas.SetActive(false);
    }
}