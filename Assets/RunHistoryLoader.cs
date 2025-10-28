using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RunHistoryLoader : MonoBehaviour
{
    // Start is called before the first frame update
    public void OpenEQRunHistory()
    {
        SceneManager.LoadSceneAsync("EQ_RunHistory");
    }

    public void OpenFireRunHistory()
    {
        SceneManager.LoadSceneAsync("Fire_RunHistory");
    }

    public void OpenFloodRunHistory()
    {
        SceneManager.LoadSceneAsync("Flood_RunHistory");
    }

    public void BackToLevelSelect()
    {
        SceneManager.LoadSceneAsync("LevelSelectionMenu");
    }
}
