using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWaterDetection : MonoBehaviour
{
    public bool isInWater = false;
    public bool isDrowning = false;

    [SerializeField] private float drowningTime = 5.0f; // Time in seconds before drowning starts
    [SerializeField] private int drowningDamagePerSecond = 1; // Whole number damage per second
    private float damageTimer = 0f; // Tracks time for applying damage

    private float timeUnderwater = 0f; // Tracks how long the player has been underwater
    private float oceanYLevel; // Stores the Y-value of the Ocean GameObject

    [SerializeField] private Camera playerCamera; // Reference to the player's camera
    private Player player; // Reference to the player's health system

    private void Start()
    {
        player = GetComponent<Player>();

        // Try to find the player's camera if not assigned
        if (playerCamera == null)
        {
            playerCamera = Camera.main; // Assumes the main camera is the player's POV
        }

        if (playerCamera == null)
        {
            Debug.LogError("Player camera not assigned and no main camera found!");
        }
    }

    private void Update()
    {
        // Find the Ocean GameObject and store its Y position
        GameObject ocean = GameObject.FindWithTag("Ocean"); // Ensure the Ocean GameObject is tagged as "Ocean"
        if (ocean != null)
        {
            oceanYLevel = ocean.transform.position.y;
        }
        else
        {
            Debug.LogError("Ocean GameObject not found! Ensure it is tagged as 'Ocean'.");
        }

        if (isInWater && playerCamera != null)
        {
            // Check if the camera is underwater
            if (playerCamera.transform.position.y < oceanYLevel)
            {
                timeUnderwater += Time.deltaTime;

                if (timeUnderwater >= drowningTime)
                {
                    isDrowning = true;

                    // Increment damage timer
                    damageTimer += Time.deltaTime;

                    // Apply damage once per second
                    if (damageTimer >= 1f)
                    {
                        ApplyDrowningDamage();
                        damageTimer = 0f; // Reset damage timer
                    }
                }
            }
            else
            {
                // Reset the timer when the player surfaces
                timeUnderwater = 0f;
                isDrowning = false;
                damageTimer = 0f; // Reset damage timer
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ocean")) // Ensure the Ocean GameObject has a trigger collider
        {
            isInWater = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Ocean"))
        {
            isInWater = false;
            isDrowning = false;
            timeUnderwater = 0f;
            damageTimer = 0f; // Reset damage timer
        }
    }

    private void ApplyDrowningDamage()
    {
        if (player != null)
        {
            player.TakeDamage(drowningDamagePerSecond);
        }
        else
        {
            Debug.LogWarning("Player script not found! Ensure it is attached to the Player.");
        }
    }
}