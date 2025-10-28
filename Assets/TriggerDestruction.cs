using RayFire;
using UnityEngine;

public class RayfireActivator : MonoBehaviour
{
    private Animator animator;

    // Reference to the Rayfire object (the one you want to activate)
    public RayfireRigid rayfireObject;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Check if the object colliding is the player or any specific object
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayActivationAnimation();

            // Trigger the Rayfire effect
            if (rayfireObject != null)
            {
                rayfireObject.Demolish();
            }
        }
    }

    private void PlayActivationAnimation()
    {
        if (animator != null)
        {
            animator.SetTrigger("Activate"); // Use the parameter name for your animation
        }
    }
}
