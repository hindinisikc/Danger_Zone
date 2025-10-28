using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public GameObject HUD;
    public GameObject PauseMenu;
    public GameObject LevelEndScreen;
    public GameObject MapView;
    public GameObject InfoOverlay;

    private void Start()
    {
        MapView.SetActive(false);
    }

    public void CloseHUD()
    {
        HUD.SetActive(false);
        //LevelEndScreen.SetActive(false);
        //MapView.SetActive(false);
        PauseMenu.SetActive(false);
        InfoOverlay.SetActive(false);
    }

    public void OpenHUD()
    {
        HUD.SetActive(true);
        //LevelEndScreen.SetActive(true);
        //MapView.SetActive(true);
        InfoOverlay.SetActive(true);
        PauseMenu.SetActive(true);
    }
}
