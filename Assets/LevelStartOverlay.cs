using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelStartOverlay : MonoBehaviour
{
    public GameObject startCanvas;      // The canvas displaying the start message
    public Image levelStartText;   // Image for the intro message
    public CanvasGroup startCanvasGroup;     // CanvasGroup for fading
    public GameObject hudCanvas;        // Canvas for the HUD
    public GameObject mapCanvas;        // Canvas for the Map View
    public float displayDuration = 3f;  // Duration the message stays on screen
    public float fadeDuration = 1f;          // Duration of the fade-out effect

    void Start()
    {
        ShowIntroMessage();
    }

    private void ShowIntroMessage()
    {
        // Enable the intro message and disable other canvases
        startCanvas.SetActive(true);
        hudCanvas.SetActive(false);
        mapCanvas.SetActive(false);

        // Start the fade-out coroutine after the display duration
        StartCoroutine(FadeOutIntro());
    }


    private IEnumerator FadeOutIntro()
    {
        // Wait for the display duration
        yield return new WaitForSeconds(displayDuration);

        // Fade out the CanvasGroup alpha
        float elapsed = 0f;
        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            startCanvasGroup.alpha = Mathf.Lerp(1f, 0f, elapsed / fadeDuration);
            yield return null;
        }

        // Ensure it's fully transparent at the end
        startCanvasGroup.alpha = 0f;
        startCanvas.SetActive(false);

        // Enable other canvases after the fade-out is complete
        hudCanvas.SetActive(true);
        mapCanvas.SetActive(true);
    }
}

