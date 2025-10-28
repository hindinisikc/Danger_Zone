using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelFailedTrigger : MonoBehaviour
{
    public GameObject levelFailedCanvas;
    public GameObject hudCanvas;
    public TextMeshProUGUI timeTakenText;
    public TextMeshProUGUI playerHPText;
    public TextMeshProUGUI mistakesMadeText;
    public MistakeTracker mistakesTracker;
    public GameObject pauseMenu;
    public GameObject mapView;
    public GameObject objectivesTab;
    public GameObject debugConsole;
    public GameObject gameOverScreen;

    private LevelTimer levelTimer;
    private bool levelFailed = false;
    private bool isLevelFailed = false;

    public GameDataManager gameDataManager;
    public MenuSelector menuSelector;

    public Button levelSelectButton;
    public Button quitToMainMenuButton;
    public Button quitGameButton;

    private void Start()
    {
        levelTimer = FindObjectOfType<LevelTimer>();
        if (levelTimer == null)
        {
            Debug.LogError("LevelTimer not found in the scene.");
        }

        levelFailedCanvas.SetActive(false);

        levelSelectButton.onClick.AddListener(LoadLevelSelect);
        quitToMainMenuButton.onClick.AddListener(LoadMainMenu);
        quitGameButton.onClick.AddListener(QuitGame);
    }

    void Update()
    {
        if (isLevelFailed && Input.anyKeyDown)
        {
            return;
        }
    }

    public void DisplayLevelFailedScreen(float playerHP)
    {
        if (hudCanvas != null) hudCanvas.SetActive(false);
        if (pauseMenu != null) pauseMenu.SetActive(false);
        if (mapView != null) mapView.SetActive(false);
        if (objectivesTab != null) objectivesTab.SetActive(false);
        if (debugConsole != null) debugConsole.SetActive(false);

        levelFailed = true;

        if (gameOverScreen != null) gameOverScreen.SetActive(false);

        levelFailedCanvas.SetActive(true);

        float totalTime = levelTimer.GetFinalTime();
        int minutes = Mathf.FloorToInt(totalTime / 60);
        int seconds = Mathf.FloorToInt(totalTime % 60);

        timeTakenText.text = $"{minutes:00}:{seconds:00}";
        playerHPText.text = playerHP.ToString();
        mistakesMadeText.text = mistakesTracker.GetMistakeCount().ToString();

        Time.timeScale = 0f;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        gameDataManager.SaveData(); // Ensure level data is saved
    }

    public void LoadLevelSelect()
    {
        Time.timeScale = 1f;
        UnityEngine.SceneManagement.SceneManager.LoadSceneAsync("LevelSelectionMenu");
    }

    public void LoadMainMenu()
    {
        Time.timeScale = 1f;
        UnityEngine.SceneManagement.SceneManager.LoadSceneAsync("MainMenu");
    }

    public void QuitGame()
    {
        menuSelector.ShowQuitModal();
    }
}