using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstFloorMapTrigger : MonoBehaviour
{
    public MapSwitcher switcher;

    private void OnTriggerEnter(Collider other)
    {
        switcher.SwitchToFirstFloor();
    }

}
