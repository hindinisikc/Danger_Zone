using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectGrabbable : MonoBehaviour
{
	private Rigidbody objectRigidbody;
	private Transform objectGrabPointTransform;

	// This is a default rotation if needed
	[SerializeField] private Vector3 grabbedRotation = Vector3.zero;

	private void Awake()
	{
		objectRigidbody = GetComponent<Rigidbody>();
	}

	public void Grab(Transform objectGrabPointTransform)
	{
		this.objectGrabPointTransform = objectGrabPointTransform;
		objectRigidbody.useGravity = false;

		// Apply the default rotation for any grabbable object
		transform.rotation = Quaternion.Euler(grabbedRotation);
	}

	public void Drop()
	{
		this.objectGrabPointTransform = null;
		objectRigidbody.useGravity = true;
	}

	private void FixedUpdate()
	{
		if (objectGrabPointTransform != null)
		{
			float lerpSpeed = 10f;
			Vector3 newPosition = Vector3.Lerp(transform.position, objectGrabPointTransform.position, Time.deltaTime * lerpSpeed);
			objectRigidbody.MovePosition(newPosition);
		}
	}
}