using System.Collections;
using System.Collections.Generic;
using TMPro;
using TMPro.Examples;
using UnityEngine;
using UnityEngine.UI;


public class Player : MonoBehaviour
{
	public DebugConsole debugConsole; // Reference to Debug Console for Testing
	public LevelFailedTrigger failedTrigger; // Reference to the Level Failed Trigger
	public LevelTimer levelTimer; // Reference to the LevelTimer

    public static bool isGameOver = false; // Global flag to indicate game-over state

    // Health and UI-related fields
    public float maxHealth = 100f;
	public float currentHealth;
	public TextMeshProUGUI healthText;
	public GameObject gameOverScreen;
	public Image damageIndicator;
	public Color damageColor = Color.red;
	public float maxDamageAlpha = 0.8f;
	public float minDamageAlpha = 0.0f;
	public UIController uiController;
	public AudioClip healSoundClip;  // The healing sound clip
	


	//Fall Damage Fields
	public float fallDamageThreshold = 2f; // Height from which damage is taken
    public float fallDamageMultiplier = 2f; // Multiplier for damage based on height
    private float lastYPosition; // Track the player's last Y position

    // Movement and crouching fields
    public CharacterController controller;
	public float normalSpeed = 5f;
	public float crouchSpeed = 2f;
	public float speed;
	public float crouchHeight = 1f;
	private float originalHeight;
	public float gravity = -9.81f;
	public float jumpHeight = 1f;
	public Transform groundCheck;
	public float groundDistance = 0.4f;
	public LayerMask groundMask;
	private bool isCrouching = false;

	// Private variables
	Vector3 velocity;
	bool isGrounded;
	private float originalSpeed;

	// Footstep variables
	public GameObject footstep;
	private bool isMoving = false;
	public AudioSource audioSource; // Reference to the audio source
	public AudioClip footstepClip;  // Footstep sound clip
	public AudioClip jumpClip;      // Jump sound clip
	public float footstepInterval = 0.1f; // Time between footsteps
	private float footstepTimer = 0f;

	// Stance Indicator
	public Image stanceIndicator;  // Drag your new Image component here
	public TextMeshProUGUI stanceName; // Reference to the stanceName Text Component
	public Sprite standingSprite;  // Drag your standing PNG here
	public Sprite crouchingSprite; // Drag your crouching PNG here
	public Sprite coverSprite; // Drag your cover PNG here
	private bool isInCover = false; // to track if the player is in cover

	public float pickupRange = 3f;  // The range in which the player can pick up items
	private GameObject itemInRange; // Reference to the item in range

	
	private GameObject equippedItemInstance; // The instance of the currently equipped item
	private PlayerPickUpDrop pickUpDropScript; // Reference to PlayerPickUpDrop script

	// Status Effect Controller
	public StatusEffectController statusEffect; // Reference to the Status Effect Controller


	public bool hasMedkit= false; // Medkit

	private string equippedItemTag;
	private FireExtinguisherHandler extinguisherHandler;


	void Start()
	{
		pickUpDropScript = GetComponent<PlayerPickUpDrop>(); // Get the reference
		// Health initialization
		currentHealth = maxHealth;
		UpdateHealthUI();
		gameOverScreen.SetActive(false);
		isGameOver = false;
		Debug.Log("Is Game Over: " + isGameOver);

		if (damageIndicator != null)
		{
			damageIndicator.color = new Color(damageColor.r, damageColor.g, damageColor.b, minDamageAlpha);
		}

		// Initialize the audio source in Start method
		audioSource = GetComponent<AudioSource>();

		// Ensure we have a default heal sound clip
		if (audioSource == null)
		{
			audioSource = gameObject.AddComponent<AudioSource>(); // Add AudioSource component if missing
		}

		// Movement initialization
		controller = GetComponent<CharacterController>();
		originalHeight = controller.height;
		originalSpeed = normalSpeed;

		footstep.SetActive(false); // Ensure footstep is off at the start

		// Set default stance to standing
		stanceIndicator.sprite = standingSprite;
		stanceName.text = "Standing";

        //Initialize lastYPosition
        lastYPosition = transform.position.y;

        if (statusEffect == null)
        {
            Debug.LogError("StatusEffectController is not assigned!");
        }



    }

	void Update()
	{
        if (Player.isGameOver)
        {
            return; // Block all input during game over
        }

        HandleMovement();
		HandleCrouch();
		UpdateDamageIndicator();
		HandleFootsteps();
		HandleCover();
		HandleItemPickup(); // New method for handling item pickups
		CheckFallDamage();
		HandleCrowbarUsage();
		HandleItemUse();

		// Equip items using number keys 1-8
		for (int i = 0; i < 9; i++)
		{
			if (Input.GetKeyDown(KeyCode.Alpha1 + i))
			{
				Inventory.instance.EquipItem(i); // Equip item in slot i
			}
		}

		// Check for healing action
		if (Input.GetKeyDown(KeyCode.H) && hasMedkit)
		{
			Heal(20); // Heal for 20 HP
		}

	}

	// Method to handle player movement and jumping
	void HandleMovement()
    {
        // Check if the DebugConsole is open
        if (debugConsole != null && debugConsole.GetIsConsoleOpen())
        {
            return; // Do not process movement if console is open
        }

        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        // Calculate movement direction
        Vector3 move = transform.right * x + transform.forward * z;

        // Move the player
        controller.Move(move * speed * Time.deltaTime);

        // Handle jumping
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
			PlayJumpSound();
			isGrounded = false;
		}

		// Simulate checking if player is grounded (replace with actual check)
		if (transform.position.y <= 0.1f) // Example condition
		{
			isGrounded = true;
		}

		// Handle footstep sounds
		if (isGrounded && (Mathf.Abs(Input.GetAxis("Horizontal")) > 0 || Mathf.Abs(Input.GetAxis("Vertical")) > 0))
		{
			footstepTimer += Time.deltaTime;

			if (footstepTimer >= footstepInterval)
			{
				PlayFootstepSound();
				footstepTimer = 0f;
			}
		}

		// Apply gravity
		velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }


    // Method to handle crouching input and behavior
    void HandleCrouch()
	{
		if (Input.GetKeyDown(KeyCode.C))
		{
			isCrouching = !isCrouching;
			UpdateStanceIndicator();
		}

		if (isCrouching)
		{
			controller.height = Mathf.Lerp(controller.height, crouchHeight, Time.deltaTime * 8);
			speed = crouchSpeed;
		}
		else
		{
			controller.height = Mathf.Lerp(controller.height, originalHeight, Time.deltaTime * 8);
			speed = originalSpeed;
		}
	}

	// Method to handle cover

	void HandleCover()
	{
		if (isCrouching && Input.GetKeyDown(KeyCode.Q))
		{
			// When Q is pressed while crouching, switch to cover stance
			isInCover = true;
			UpdateStanceIndicator(true);
		}
		else if (!isCrouching)
		{
			isInCover = false;
			UpdateStanceIndicator(false);
		}
	}

	void UpdateStanceIndicator(bool inCover = false)
	{
		if (inCover)
		{
			stanceIndicator.sprite = coverSprite; // Display cover icon
			stanceName.text = "Covering";
		}
		else if (isCrouching)
		{
			stanceIndicator.sprite = crouchingSprite; // Display crouching icon
			stanceName.text = "Ducking";
		}
		else
		{
			stanceIndicator.sprite = standingSprite; // Display standing icon
			stanceName.text = "Standing";
		}
	}

	// Method to check if the player is in cover
	public bool IsInCover()
	{
		return isInCover;
	}



    // Method to apply damage to the player
    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        Debug.Log("Player took damage: " + damage + ". Current health: " + currentHealth);
        UpdateHealthUI();

        if (statusEffect != null)
        {
            Debug.Log("TakeDamage called, Damage: " + damage);
            statusEffect.AddStatusEffect("Damaged"); // Add the "Damaged" status effect when taking damage
        }

        if (currentHealth <= 0)
        {
            Die();
        }
    }

	// Method to Heal the Player
    public void AddHealth(float healthToAdd)
    {
        // Increase health but do not exceed maxHealth
        currentHealth = Mathf.Min(currentHealth + healthToAdd, maxHealth);
        Debug.Log("Player healed: " + healthToAdd + ". Current health: " + currentHealth);
        UpdateHealthUI();
        if (currentHealth > 0 && currentHealth < maxHealth)
        {
            statusEffect.ShowHealedStatus(2f); // Show for 2 seconds
        }

        if (statusEffect != null)
        {
            Debug.Log("AddHealth called, Health Added: " + healthToAdd);
            statusEffect.RemoveStatusEffect("Damaged"); // Remove the "Damaged" status effect if healing removes damage
            statusEffect.AddStatusEffect("Healed");    // Add the "Healed" status effect
        }
    }


    // Method to update the health UI text
    private void UpdateHealthUI()
    {
        if (healthText != null)
        {
            healthText.text = "" + currentHealth;
        }
    }

    // Method to update the damage indicator UI
    private void UpdateDamageIndicator()
	{
		if (damageIndicator != null)
		{
			float healthPercentage = currentHealth / maxHealth;
			float targetAlpha = Mathf.Lerp(maxDamageAlpha, minDamageAlpha, healthPercentage);
			damageIndicator.color = new Color(damageColor.r, damageColor.g, damageColor.b, targetAlpha);
		}
	}

    // Method to handle player death
    public void Die()
    {
        if (isGameOver) return; // Prevent re-triggering "Die" logic

        Debug.Log("Player died.");
        isGameOver = true; // Set game-over state
        uiController.CloseHUD(); // Ensure HUD is turned off

        // Activate the Game Over Screen and trigger fade-in
        gameOverScreen.SetActive(true);
        gameOverScreen.GetComponent<GameOverScreen>().ShowGameOverScreen();  // Call the fade-in and sound effect
    }

    public float GetCurrentHealth()
	{
		return currentHealth;
	}

	// Handle footstep activation
	private void HandleFootsteps()
	{
		// Check if any movement key is pressed
		if (Input.GetKey("w") || Input.GetKey("s") || Input.GetKey("a") || Input.GetKey("d"))
		{
			if (!isMoving) // Ensure footstep is active only once when movement starts
			{
				footsteps();
				isMoving = true;
			}
		}
		else
		{
			if (isMoving) // Ensure footstep is inactive only once when movement stops
			{
				StopFootsteps();
				isMoving = false;
			}
		}
	}

	void HandleItemPickup()
	{
		if (Input.GetKeyDown(KeyCode.E) && itemInRange != null)
		{
			// Access the Item script on the object
			Item item = itemInRange.GetComponent<Item>();
			if (item != null)
			{
				// Add item to inventory
				Inventory.instance.AddItem(item);
				Destroy(itemInRange); // Remove the item from the scene
			}
		}
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Pickup"))
		{
			itemInRange = other.gameObject;
		}
	}

	private void OnTriggerExit(Collider other)
	{
		if (other.CompareTag("Pickup"))
		{
			itemInRange = null;
		}
	}

	// Activate footsteps
	private void footsteps()
	{
		footstep.SetActive(true);
	}

	// Deactivate footsteps
	private void StopFootsteps()
	{
		footstep.SetActive(false);
	}

    private void CheckFallDamage()
    {
        float fallDistance = lastYPosition - transform.position.y; // Calculate fall distance

        if (fallDistance > fallDamageThreshold) // Check if the fall distance exceeds the threshold
        {
            // Calculate fall damage
            float damage = (fallDistance - fallDamageThreshold) * fallDamageMultiplier;

            // Apply damage to the player
            TakeDamage(damage);
			Debug.Log("Player Damaged from Fall");
        }

        lastYPosition = transform.position.y; // Update lastYPosition for next frame
    }

    public void Teleport(Vector3 newPosition)
    {
        transform.position = newPosition;
    }

	// Method for Healing Player with Medkit
	public void Heal(float healAmount)
	{
		AddHealth(25);
		UpdateHealthUI();
		hasMedkit = false; // Set hasMedkit to false after using it
		Debug.Log("Player healed: " + healAmount + ". Current health: " + currentHealth);
		// Play the heal sound effect
		if (healSoundClip != null)
		{
			audioSource.PlayOneShot(healSoundClip); // Play the healing sound
		}
	}

	// Check if the player is holding a crowbar and handle its usage
	private void HandleCrowbarUsage()
	{
		if (pickUpDropScript != null && pickUpDropScript.IsHoldingCrowbar())
		{
			if (Input.GetKeyDown(KeyCode.Mouse0)) // Left-click to use the crowbar
			{
				Debug.Log("Crowbar used!");

				// Find the nearest door
				DoorController nearestDoor = FindNearestDoor();

				if (nearestDoor != null && nearestDoor.isPlayerNear && nearestDoor.isJammed)
				{
					nearestDoor.UnjamDoor(); // Use the crowbar to unjam the door
				}
			}
		}
	}
	private DoorController FindNearestDoor()
    {
        DoorController[] doors = FindObjectsOfType<DoorController>();
        DoorController nearestDoor = null;
        float closestDistance = Mathf.Infinity;

        foreach (DoorController door in doors)
        {
            float distance = Vector3.Distance(transform.position, door.transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                nearestDoor = door;
            }
        }

        return nearestDoor;
    }

	public void SetEquippedItem(string itemTag)
	{
		equippedItemTag = itemTag;

		if (itemTag == "Fire Extinguisher")
		{
			// Find all fire extinguishers and set their handlers
			FireExtinguisherHandler[] extinguishers = FindObjectsOfType<FireExtinguisherHandler>();
			foreach (var extinguisher in extinguishers)
			{
				extinguisher.enabled = true; // Enable the handler for all extinguishers
			}
		}
		else
		{
			// Disable all fire extinguisher handlers
			FireExtinguisherHandler[] extinguishers = FindObjectsOfType<FireExtinguisherHandler>();
			foreach (var extinguisher in extinguishers)
			{
				extinguisher.enabled = false; // Disable the handler
			}

			extinguisherHandler = null;
		}
	}

	private void HandleItemUse()
	{
		float activationRange = 1f; // Define the range within which the extinguisher can be used

		// Check if the player pressed the left mouse button
		if (Input.GetMouseButtonDown(0))
		{
			// Find all fire extinguishers in the scene
			GameObject[] fireExtinguishers = GameObject.FindGameObjectsWithTag("Fire Extinguisher");

			foreach (GameObject extinguisher in fireExtinguishers)
			{
				// Check the distance between the player and the extinguisher
				float distance = Vector3.Distance(transform.position, extinguisher.transform.position);
				if (distance <= activationRange)
				{
					// Get the FireExtinguisherHandler component
					FireExtinguisherHandler handler = extinguisher.GetComponent<FireExtinguisherHandler>();

					if (handler != null)
					{
						// Toggle the particle system
						handler.ToggleParticleSystem();
					}
				}

				Debug.Log($"Extinguisher in range: {extinguisher.name}, Distance: {distance}");
			}
		}
	}

	private void PlayFootstepSound()
	{
		if (!audioSource.isPlaying)
		{
			audioSource.clip = footstepClip;
			audioSource.Play();
		}
	}

	private void PlayJumpSound()
	{
		audioSource.Stop(); // Stop any currently playing sound
		audioSource.PlayOneShot(jumpClip);
	}

}