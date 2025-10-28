using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinimapPlayerTracker : MonoBehaviour
{
    public RectTransform playerIcon; // Assign the UI Image for the player icon
    public Transform playerTransform; // Reference to the player
    public Camera minimapCamera; // Reference to the Minimap Camera

    void Update()
    {
        // Get player's position relative to the Minimap Camera
        Vector3 viewportPos = minimapCamera.WorldToViewportPoint(playerTransform.position);

        // Convert to UI position
        playerIcon.anchoredPosition = new Vector2(
            viewportPos.x * playerIcon.parent.GetComponent<RectTransform>().sizeDelta.x,
            viewportPos.y * playerIcon.parent.GetComponent<RectTransform>().sizeDelta.y
        );

        playerIcon.rotation = Quaternion.Euler(0, 0, -playerTransform.eulerAngles.y);
    }
}
