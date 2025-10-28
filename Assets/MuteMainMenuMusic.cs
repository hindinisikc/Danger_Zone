using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MuteMainMenuMusic : MonoBehaviour
{
    // Reference to Music Game Object
    public GameObject music;
    public bool isMusicOn = true;
    public void ToggleMusic()
    {
        if(isMusicOn == true)
        {
            music.SetActive(false);
            isMusicOn = false;
        }
        else
        {
            music.SetActive(true);
            isMusicOn = true;
        }
    }

    public void OnOptionsClick()
    {
        ToggleMusic();
    }
}
