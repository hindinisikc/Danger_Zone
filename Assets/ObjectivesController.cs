using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectivesController : MonoBehaviour
{
    public GameObject miniObjs;
    public GameObject fullObjs;
    public UIController controller;
    public GameOverScreen gameOverScreen;

    // Start is called before the first frame update
    void Start()
    {
        // Map is set to Inactive by default
        miniObjs.SetActive(true);
        fullObjs.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        // Check for Keypress
        if (Input.GetKeyDown(KeyCode.Tab) && !gameOverScreen.IsGameOver())
        {
            if (miniObjs.activeSelf)
            {
                OpenObjectivesScreen();
            }
            else
            {
                CloseObjectivesScreen();
            }
        }
    }

    void OpenObjectivesScreen()
    {
        fullObjs.SetActive(true);
        miniObjs.SetActive(false);
        Time.timeScale = 0f; // Pauses the game
        Cursor.lockState = CursorLockMode.None; // Unlocks the cursor
        Cursor.visible = true;
    }

    void CloseObjectivesScreen()
    {
        fullObjs.SetActive(false);
        miniObjs.SetActive(true);
        Time.timeScale = 1f; // Resumes the game
        Cursor.lockState = CursorLockMode.Locked; // Locks the cursor back to the center
        Cursor.visible = false;
    }
}
