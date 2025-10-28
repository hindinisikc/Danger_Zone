using UnityEngine;
using UnityEngine.Events; // Include this namespace to use UnityEvent

public class EventTrigger : MonoBehaviour
{
    // Create a UnityEvent to allow for custom actions in the inspector
    public UnityEvent onEnter; // Event to trigger on entering the collider

    // This method is called when another collider enters the trigger collider
    private void OnTriggerEnter(Collider other)
    {
        // Check if the collider has a specific tag (e.g., "Player")
        if (other.CompareTag("Player"))
        {
            // Invoke the event when the player enters the trigger
            onEnter.Invoke();
        }
    }
}
