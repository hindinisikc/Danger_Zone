using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class GameOverScreen : MonoBehaviour
{
    public LevelFailedTrigger trigger;
    public Player player;
    public CanvasGroup canvasGroup;
    public AudioSource gameOverSFX;

    // References to UI elements to deactivate
    public GameObject pauseMenu;
    public GameObject hudCanvas;
    public GameObject debugConsole;
    public GameObject objectivesTab;
    public GameObject mapView;

    private float fadeDuration = 1.0f;
    private bool isGameOver = false; // Track game-over state

    void Update()
    {
        // Block any input (like Escape for Pause Menu) during game over state
        if (isGameOver)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                return; // Prevent opening Pause Menu during Game Over
            }
        }
    }

    public void OnContinueButtonClicked()
    {
        if (player != null && trigger != null)
        {
            trigger.DisplayLevelFailedScreen(player.GetCurrentHealth());
        }
        else
        {
            Debug.LogError("Player or LevelFailedTrigger reference missing!");
        }
    }

    public bool IsGameOver()
    {
        return isGameOver;
    }

    public void ShowGameOverScreen()
    {
        // Play the Game Over sound effect
        gameOverSFX.Play();

        // Deactivate unwanted UI elements
        if (pauseMenu != null) pauseMenu.SetActive(false);
        if (hudCanvas != null) hudCanvas.SetActive(false);
        if (debugConsole != null) debugConsole.SetActive(false);
        if (objectivesTab != null) objectivesTab.SetActive(false);
        if (mapView != null) mapView.SetActive(false);

        // Set the game-over state to true to block input
        isGameOver = true;

        // Trigger the fade-in coroutine
        StartCoroutine(FadeInGameOverScreen());
    }

    private IEnumerator FadeInGameOverScreen()
    {
        canvasGroup.alpha = 0f; // Start with the screen invisible
        canvasGroup.gameObject.SetActive(true); // Ensure it's active
        float time = 0f;

        while (time < fadeDuration)
        {
            time += Time.unscaledDeltaTime;
            canvasGroup.alpha = Mathf.Lerp(0f, 1f, time / fadeDuration); // Gradual fade-in
            yield return null;
        }

        canvasGroup.alpha = 1f; // Ensure the screen ends fully visible
    }
}