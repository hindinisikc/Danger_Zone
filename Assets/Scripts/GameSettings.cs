using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSettings : MonoBehaviour  // Renamed class to match the file
{
    public static GameSettings Instance;

    // This variable will store whether tips are enabled or not
    public bool areTipsEnabled = true;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            Debug.Log("GameSettings initialized.");
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
