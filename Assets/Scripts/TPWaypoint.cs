using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TPWaypoint : MonoBehaviour
{
    [SerializeField]
    [Tooltip("Unique name for this teleport waypoint.")]
    private string waypointName;

    private List<GameObject> tpWaypoints;  // Dynamic list of TPWaypoints

    // Public property to access the waypoint name
    public string WaypointName => waypointName; // Read-only access to the waypoint name

    private void Start()
    {
        // Find all waypoints in the scene by tag or component (assuming they all have "TPWaypoint" tag)
        tpWaypoints = new List<GameObject>(GameObject.FindGameObjectsWithTag("TPWaypoint"));
    }

    // Call this function to toggle visibility
    public void ToggleTPWaypoints(bool isVisible)
    {
        foreach (GameObject waypoint in tpWaypoints)
        {
            MeshRenderer renderer = waypoint.GetComponent<MeshRenderer>();
            if (renderer != null)
            {
                renderer.enabled = isVisible;
            }
        }
    }
}
