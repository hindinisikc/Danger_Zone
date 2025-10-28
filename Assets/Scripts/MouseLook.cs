using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseLook : MonoBehaviour
{
	public float mouseSensitivity = 700f; // Sensitivity of mouse movement
	public Transform playerBody; // Reference to the player's body or character

	private float xRotation = 0f; // Current rotation around the x-axis (up and down)

	// Start is called before the first frame update
	void Start()
	{
		Cursor.lockState = CursorLockMode.Locked; // Locks the cursor to the center of the screen
	}

	// Update is called once per frame
	void Update()
	{
		// Get mouse input for X and Y axis
		float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
		float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

		// Adjust rotation around the x-axis (up and down)
		xRotation -= mouseY;
		xRotation = Mathf.Clamp(xRotation, -90f, 90f); // Clamp to prevent over-rotation

		// Apply rotation to the camera (or head)
		transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

		// Rotate the entire player body (or character) around the y-axis (left and right)
		playerBody.Rotate(Vector3.up * mouseX);
	}
}