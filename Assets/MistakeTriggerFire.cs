using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MistakeTriggerFire : MonoBehaviour
{
    public MistakeTracker tracker;

    private void OnTriggerStay(Collider other)
    {
        Debug.Log("Tried to use elevator, adding mistake");
        tracker.IncrementMistake();
    }
}
