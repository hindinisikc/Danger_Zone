using Ignis;
using System.Collections;
using UnityEngine;

public class PlayerFireMechanic : MonoBehaviour
{
    public float fireDuration = 5f; // Maximum duration of burning effect each time it's applied
    public int damagePerSecond = 5; // Flat damage applied per second while burning
    public GameObject fireEffect; // Visual fire effect on the player
    public AudioClip fireDamageSFX; // Sound effect played when taking damage from fire
    public KeyCode extinguishKey = KeyCode.E; // Key to extinguish the fire
    public float extinguishDelay = 3f; // Delay before the fire is extinguished naturally after leaving the fire area

    [Tooltip("Is the Burning Status Active on the Player?")]
    public bool isBurning = false;
    private float burnTimer = 0f;
    private float damageTimer = 0f; // Timer to control damage application
    private float extinguishDelayTimer = 0f; // Timer for natural extinguishing delay
    private Player player; // Reference to Player script (assumes it handles health)
    private AudioSource audioSource; // Audio source to play sound effects
    private bool inFireArea = false; // Tracks if the player is in the fire area

    public StatusEffectController statusEffect; // Reference to the Status Effect Controller

    void Start()
    {
        player = GetComponent<Player>();
        audioSource = GetComponent<AudioSource>();

        if (player == null)
        {
            Debug.LogError("PlayerFireMechanic requires a Player script on the same GameObject.");
        }

        if (audioSource == null)
        {
            Debug.LogError("PlayerFireMechanic requires an AudioSource component on the same GameObject.");
        }
    }

    void Update()
    {
        if (isBurning)
        {
            burnTimer -= Time.deltaTime;

            // Apply flat damage once per second
            damageTimer += Time.deltaTime;
            if (damageTimer >= 1f)
            {
                ApplyDamage();
                damageTimer = 0f;
            }

            // Start the extinguish delay timer if the player leaves the fire area
            if (!inFireArea)
            {
                extinguishDelayTimer -= Time.deltaTime;

                if (extinguishDelayTimer <= 0f)
                {
                    ExtinguishFire();
                }
            }

            // Allow the player to extinguish fire manually
            if (Input.GetKeyDown(extinguishKey))
            {
                ExtinguishFire();
            }
        }

        // If the player is still in the fire area and not burning, reignite
        if (!isBurning && inFireArea)
        {
            Ignite();
        }
    }

    public void Ignite()
    {
        if (!isBurning)
        {
            isBurning = true;
            burnTimer = fireDuration;
            damageTimer = 0f; // Reset damage timer
            extinguishDelayTimer = extinguishDelay; // Reset the extinguish delay timer

            // Activate fire visual effect
            if (fireEffect != null)
            {
                fireEffect.SetActive(true);
            }

            Debug.Log("Player is now burning!");
        }
    }

    public void ExtinguishFire()
    {
        if (isBurning)
        {
            isBurning = false;

            // Deactivate fire visual effect
            if (fireEffect != null)
            {
                fireEffect.SetActive(false);
            }

            Debug.Log("Player has extinguished the fire!");

        }
    }

    private void ApplyDamage()
    {
        if (player != null)
        {
            player.TakeDamage(damagePerSecond);
            Debug.Log($"Player takes {damagePerSecond} damage from fire.");
        }

        // Play the damage sound effect
        if (fireDamageSFX != null && audioSource != null)
        {
            audioSource.PlayOneShot(fireDamageSFX);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("FireObject"))
        {
            inFireArea = true;
            if (!isBurning)
            {
                Ignite();
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("FireObject"))
        {
            inFireArea = false;
            extinguishDelayTimer = extinguishDelay; // Start the delay timer when leaving the fire area
        }
    }
}