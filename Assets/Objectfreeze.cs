using UnityEngine;

public class RigidbodyTurnStatic : MonoBehaviour
{
    private Rigidbody rb;
    public float velocityThreshold = 0.1f; // Minimum velocity to consider "stopped"
    public float stopDelay = 1f;          // Time delay before making it static
    private float timer = 0f;             // Timer to count time below the threshold

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        // Check if the Rigidbody's velocity is below the threshold
        if (rb.velocity.magnitude < velocityThreshold && rb.angularVelocity.magnitude < velocityThreshold)
        {
            timer += Time.deltaTime;

            // If the object remains below the threshold for the stop delay, make it static
            if (timer >= stopDelay)
            {
                MakeStatic();
            }
        }
        else
        {
            // Reset the timer if the object is moving
            timer = 0f;
        }
    }

    void MakeStatic()
    {
        rb.isKinematic = true; // Disable Rigidbody physics simulation
        rb.useGravity = false; // Optional: Disable gravity to prevent further movement
        enabled = false;       // Disable this script as it is no longer needed
    }
}
