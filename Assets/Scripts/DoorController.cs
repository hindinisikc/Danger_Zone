using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DoorController : MonoBehaviour
{
	public Vector3 openPositionOffset = new Vector3(2f, 0, 0);   // Offset from the closed position to the open position
	public float openSpeed = 2.0f;   // Speed at which the door opens and closes
	public TextMeshProUGUI interactionPrompt;   // UI text prompting the player to interact with the door

	private Vector3 closedPosition;   // Position of the door when closed
	private Vector3 openPosition;     // Target position of the door when open
	public bool isPlayerNear = false; // Flag indicating if the player is near the door
	private bool isDoorOpen = false;  // Flag indicating if the door is currently open
	[SerializeField] public bool isJammed = false; // Flag indicating if the door is jammed

	[SerializeField] private AudioSource doorOpenAudioSource = null;
	[SerializeField] private float openDelay = 0.8f;

	[SerializeField] private AudioSource doorCloseAudioSource = null;
	[SerializeField] private float closeDelay = 0.8f;

	void Start()
	{
		closedPosition = transform.position;
		openPosition = closedPosition + openPositionOffset;

		// Disable interaction prompt at the start if it's assigned
		if (interactionPrompt != null)
		{
			interactionPrompt.enabled = false;
		}
	}

	void Update()
	{
		// Check if the player is near the door and presses the interact key (E key)
		if (isPlayerNear && Input.GetKeyDown(KeyCode.E))
		{
			if (isJammed)
			{
				// Show a different prompt or play a sound to indicate the door is jammed
				if (interactionPrompt != null)
				{
					interactionPrompt.text = "The door is jammed!";
				}
				Debug.Log("Door is jammed and cannot be opened.");
			}
			else
			{
				// Toggle door state: if open, close it; if closed, open it
				if (isDoorOpen)
				{
					StartCoroutine(CloseDoor());
					doorCloseAudioSource.PlayDelayed(closeDelay);
				}
				else
				{
					StartCoroutine(OpenDoor());
					doorOpenAudioSource.PlayDelayed(openDelay);
				}
			}
		}
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Player"))
		{
			isPlayerNear = true;

			// Enable and update interaction prompt text if it's assigned
			if (interactionPrompt != null)
			{
				interactionPrompt.enabled = true;
				interactionPrompt.text = isJammed ? "The door is jammed!" : "Press E to interact with the door";
			}
		}
	}

	private void OnTriggerExit(Collider other)
	{
		if (other.CompareTag("Player"))
		{
			isPlayerNear = false;

			// Disable interaction prompt if it's assigned
			if (interactionPrompt != null)
			{
				interactionPrompt.enabled = false;
			}
		}
	}

	private IEnumerator OpenDoor()
	{
		isDoorOpen = true;
		while (Vector3.Distance(transform.position, openPosition) > 0.01f)
		{
			transform.position = Vector3.Lerp(transform.position, openPosition, Time.deltaTime * openSpeed);
			yield return null;
		}
		transform.position = openPosition; // Ensure door reaches exact open position
	}

	private IEnumerator CloseDoor()
	{
		isDoorOpen = false;
		while (Vector3.Distance(transform.position, closedPosition) > 0.01f)
		{
			transform.position = Vector3.Lerp(transform.position, closedPosition, Time.deltaTime * openSpeed);
			yield return null;
		}
		transform.position = closedPosition; // Ensure door reaches exact closed position
	}

	public void UnjamDoor()
	{
		if (isJammed)
		{
			isJammed = false; // Unjam the door
			Debug.Log("The door has been unjammed using the crowbar!");

			// Update the interaction prompt if assigned
			if (interactionPrompt != null)
			{
				interactionPrompt.text = "Press E to interact with the door";
			}
		}
	}

}