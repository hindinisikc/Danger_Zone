using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class OptionsMenu : MonoBehaviour
{
    // Reference to UI Elements
    public GameObject optionsPanel;
    public GameObject titleLogo;
    public GameObject buttonPanel;
    private bool isOptionsOpen;

    // Reference to Audio Settings
    public AudioMixer audioMixer; // Drag the GameAudioMixer here
    public Slider volumeSlider;   // Drag the volume slider here

    // Reference to Music
    public GameObject music; // Drag your Music GameObject here
    private bool isMusicOn;

    // Reference to the Tips Toggle in the Options Menu
    public Toggle tipsToggle;  // Drag the Tips Toggle here in the inspector

    private void Start()
    {
        // Initialize Options Menu
        CloseOptionsPanel();

        // Load and apply saved volume
        float savedVolume = PlayerPrefs.GetFloat("MasterVolume", 0.75f); // Default to 0.75 (Unity scale is 0.0 to 1.0)
        audioMixer.SetFloat("MasterVolume", Mathf.Log10(savedVolume) * 20); // Convert to decibels
        if (volumeSlider != null)
        {
            volumeSlider.value = savedVolume;
        }

        // Load and apply music state
        isMusicOn = PlayerPrefs.GetInt("MusicState", 1) == 1; // Default to ON (1)
        music.SetActive(isMusicOn);

        // Initialize Tips Toggle based on saved setting
        tipsToggle.isOn = GameSettings.Instance.areTipsEnabled;

        // Add listener for tips toggle
        tipsToggle.onValueChanged.AddListener(ToggleTips);
    }

    void Update()
    {
        if (isOptionsOpen == true && Input.GetKeyDown(KeyCode.Escape))
        {
            CloseOptionsPanel();
        }

        // Check if the slider is interactive
        if (volumeSlider != null && volumeSlider.interactable)
        {
            Debug.Log("Slider is interactive, value: " + volumeSlider.value);
        }
    }

    public void OnVolumeChange(float value)
    {
        if (value <= 0.0001f)
        {
            value = 0.0001f; // Prevent Log10 from breaking
        }

        // Update the AudioMixer volume
        audioMixer.SetFloat("MasterVolume", Mathf.Log10(value) * 20);

        // Save the volume setting
        PlayerPrefs.SetFloat("MasterVolume", value);
        PlayerPrefs.Save();

        // Force the slider to update to ensure it responds to all interactions
        if (volumeSlider != null)
        {
            volumeSlider.value = value;
        }
    }

    public void ToggleMusic()
    {
        // Toggle music state
        isMusicOn = !isMusicOn;
        music.SetActive(isMusicOn);

        // Save the new state
        PlayerPrefs.SetInt("MusicState", isMusicOn ? 1 : 0);
        PlayerPrefs.Save();
    }

    public void OnOptionsButtonClick()
    {
        // Toggle the visibility of the Options Panel
        optionsPanel.SetActive(!optionsPanel.activeSelf);
        titleLogo.SetActive(!optionsPanel.activeSelf);
        buttonPanel.SetActive(!optionsPanel.activeSelf);
        isOptionsOpen = true;
    }

    public void CloseOptionsPanel()
    {
        if (optionsPanel.activeSelf)
        {
            optionsPanel.SetActive(false);
            titleLogo.SetActive(true);
            buttonPanel.SetActive(true);
        }
        isOptionsOpen = false;
    }

    // This method is called when the Tips toggle is changed
    private void ToggleTips(bool isOn)
    {
        GameSettings.Instance.areTipsEnabled = isOn;
    }
}