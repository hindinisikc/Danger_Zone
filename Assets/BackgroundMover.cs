using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundMover : MonoBehaviour
{
    public float speed = 2f;         // Speed of movement
    public float distanceX = 10f;    // Distance to move side to side
    public float distanceY = 5f;      // Distance to move up and down

    private Vector3 startPosition;    // Starting position of the background
    private float originalX;          // Original X position
    private float originalY;          // Original Y position

    void Start()
    {
        startPosition = transform.position; // Store the original position
        originalX = startPosition.x;       // Store the original X position
        originalY = startPosition.y;       // Store the original Y position
    }

    void Update()
    {
        // Calculate the new position based on sine waves for smooth side-to-side and up-and-down movement
        float newX = originalX + Mathf.Sin(Time.time * speed) * distanceX; // Horizontal movement
        float newY = originalY + Mathf.Sin(Time.time * speed) * distanceY; // Vertical movement

        transform.position = new Vector3(newX, newY, startPosition.z); // Update position
    }
}
