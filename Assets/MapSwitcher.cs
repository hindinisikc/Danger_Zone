using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapSwitcher : MonoBehaviour
{
    public GameObject secondFloorMap;
    public GameObject firstFloorMap;
    
    // Start is called before the first frame update
    void Start()
    {
        secondFloorMap.SetActive(true);
        firstFloorMap.SetActive(true);
    }

    public void SwitchToFirstFloor()
    {
        Debug.Log("Exited Second Floor, switching to First Floor Map");
        secondFloorMap.SetActive(false);
        firstFloorMap.SetActive(true);
    }

    public void SwitchToSecondFloor()
    {
        Debug.Log("Entered Second Floor, switching to Second Floor Map");
        secondFloorMap.SetActive(true);
        firstFloorMap.SetActive(false);
    }
}
