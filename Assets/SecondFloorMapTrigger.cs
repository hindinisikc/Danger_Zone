using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecondFloorMapTrigger : MonoBehaviour
{
    public MapSwitcher switcher;

    private void OnTriggerEnter(Collider other)
    {
        switcher.SwitchToSecondFloor();
    }
}
