using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloodManager : MonoBehaviour
{
    // Reference to the Ocean (Water) GameObject
    public GameObject ocean;  // Assign the "Ocean" GameObject in the Inspector
    public float riseSpeed = 0.1f;  // Speed at which the water level rises
    public float maxWaterLevel = 10f;  // Maximum Y value for the ocean (maximum water level)

    private float currentWaterLevel;  // Current Y position of the ocean

    void Start()
    {
        if (ocean != null)
        {
            currentWaterLevel = ocean.transform.position.y;  // Initialize the water level at the start
        }
        else
        {
            Debug.LogError("Ocean GameObject not assigned in FloodManager.");
        }
    }

    void Update()
    {
        if (ocean != null)
        {
            // Raise the water level gradually
            if (currentWaterLevel < maxWaterLevel)
            {
                currentWaterLevel += riseSpeed * Time.deltaTime;  // Increase the Y position over time
                ocean.transform.position = new Vector3(ocean.transform.position.x, currentWaterLevel, ocean.transform.position.z);  // Update the position of the ocean
            }
        }
    }
}