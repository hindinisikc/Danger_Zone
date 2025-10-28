using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuSelector : MonoBehaviour
{
    public GameObject quitModal; // Reference the Quit Modal prefab (with overlay inside)
    public FirebaseAuthLoginManager loginManager;
    public GameDataManager gameDataManager;
    public FirebaseLogout firebaseLogout;

    void Start()
    {
        // Ensure the quit modal is hidden initially
        if (quitModal != null)
            quitModal.SetActive(false);
    }

    public void OpenScene()
    {
        SceneManager.LoadSceneAsync("LevelSelectionMenu");
    }

    public void PreviousScene()
    {
        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex - 1);
    }

    public void BackToMainMenu()
    {
        SceneManager.LoadSceneAsync("MainMenu");
    }

    public void OnLoginButtonPressed()
    {
        loginManager.Login();
    }
    
    public void LoginSuccess()
    {
        SceneManager.LoadSceneAsync("TitleScene");
    }

    // Show the quit confirmation modal with overlay
    public void ShowQuitModal()
    {
        if (quitModal != null)
            quitModal.SetActive(true);
    }

    // Hide the quit modal and overlay together
    public void HideQuitModal()
    {
        if (quitModal != null)
            quitModal.SetActive(false);
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        //firebaseLogout.LogOut();
#else
        Application.Quit();
        //firebaseLogout.LogOut();
#endif
    }
}