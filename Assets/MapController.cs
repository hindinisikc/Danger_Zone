using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapController : MonoBehaviour
{
    public GameObject fullMap;
    // Start is called before the first frame update
    void Start()
    {
        // Map is set to Inactive by default
        Debug.Log("Map Defaulted to Inactive");
        fullMap.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        // Check for Keypress
        if (Input.GetKeyDown(KeyCode.M))
        {
            if (fullMap.activeSelf)
            {
                Debug.Log("Map Closed");
                fullMap.SetActive(false);
            }
            else
            {
                Debug.Log("Map Opened");
                fullMap.SetActive(true);
            }
        }
    }
}
