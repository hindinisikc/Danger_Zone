using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(Rigidbody))]
public class Fracture : MonoBehaviour
{
	public TriggerOptions triggerOptions;
	public FractureOptions fractureOptions;
	public RefractureOptions refractureOptions;
	public CallbackOptions callbackOptions;

	[HideInInspector]
	public int currentRefractureCount = 0;

	private GameObject fragmentRoot;

	public void CauseFracture()
	{
		callbackOptions.CallOnFracture(null, gameObject, transform.position);
		this.ComputeFracture();
	}

	void OnCollisionEnter(Collision collision)
	{
		if (triggerOptions.triggerType == TriggerType.Collision)
		{
			if (collision.contactCount > 0)
			{
				var contact = collision.contacts[0];
				float collisionForce = collision.impulse.magnitude / Time.fixedDeltaTime;
				bool tagAllowed = triggerOptions.IsTagAllowed(contact.otherCollider.gameObject.tag);

				if (collisionForce > triggerOptions.minimumCollisionForce &&
				   (triggerOptions.filterCollisionsByTag && tagAllowed))
				{
					callbackOptions.CallOnFracture(contact.otherCollider, gameObject, contact.point);
					this.ComputeFracture();
				}
			}
		}
	}

	void OnTriggerEnter(Collider collider)
	{
		if (triggerOptions.triggerType == TriggerType.Trigger)
		{
			bool tagAllowed = triggerOptions.IsTagAllowed(collider.gameObject.tag);

			if (triggerOptions.filterCollisionsByTag && tagAllowed)
			{
				callbackOptions.CallOnFracture(collider, gameObject, transform.position);
				this.ComputeFracture();
			}
		}
	}

	private void ComputeFracture()
	{
		var mesh = this.GetComponent<MeshFilter>().sharedMesh;

		if (mesh != null)
		{
			if (this.fragmentRoot == null)
			{
				this.fragmentRoot = new GameObject($"{this.name}Fragments");
				this.fragmentRoot.transform.SetParent(this.transform.parent);
				this.fragmentRoot.transform.position = this.transform.position;
				this.fragmentRoot.transform.rotation = this.transform.rotation;
				this.fragmentRoot.transform.localScale = Vector3.one;
			}

			var fragmentTemplate = CreateFragmentTemplate();

			if (fractureOptions.asynchronous)
			{
				StartCoroutine(Fragmenter.FractureAsync(
					this.gameObject,
					this.fractureOptions,
					fragmentTemplate,
					this.fragmentRoot.transform,
					() =>
					{
						GameObject.Destroy(fragmentTemplate);
						this.gameObject.SetActive(false);

						if ((this.currentRefractureCount == 0) ||
							(this.currentRefractureCount > 0 && this.refractureOptions.invokeCallbacks))
						{
							if (callbackOptions.onCompleted != null)
							{
								callbackOptions.onCompleted.Invoke();
							}
						}
					}
				));
			}
			else
			{
				Fragmenter.Fracture(this.gameObject,
									this.fractureOptions,
									fragmentTemplate,
									this.fragmentRoot.transform);

				GameObject.Destroy(fragmentTemplate);
				this.gameObject.SetActive(false);

				if ((this.currentRefractureCount == 0) ||
					(this.currentRefractureCount > 0 && this.refractureOptions.invokeCallbacks))
				{
					if (callbackOptions.onCompleted != null)
					{
						callbackOptions.onCompleted.Invoke();
					}
				}
			}
		}
	}

	private GameObject CreateFragmentTemplate()
	{
		GameObject obj = new GameObject();
		obj.name = "Fragment";
		obj.tag = this.tag;

		// Add MeshFilter and MeshRenderer components
		obj.AddComponent<MeshFilter>();
		var meshRenderer = obj.AddComponent<MeshRenderer>();
		meshRenderer.sharedMaterials = new Material[2] {
			this.GetComponent<MeshRenderer>().sharedMaterial,
			this.fractureOptions.insideMaterial
		};

		// Add MeshCollider
		var thisCollider = this.GetComponent<Collider>();
		var fragmentCollider = obj.AddComponent<MeshCollider>();
		fragmentCollider.convex = true;
		fragmentCollider.sharedMaterial = thisCollider.sharedMaterial;
		fragmentCollider.isTrigger = thisCollider.isTrigger;

		// Add Rigidbody
		var thisRigidBody = this.GetComponent<Rigidbody>();
		var fragmentRigidBody = obj.AddComponent<Rigidbody>();
		fragmentRigidBody.velocity = thisRigidBody.velocity;
		fragmentRigidBody.angularVelocity = thisRigidBody.angularVelocity;
		fragmentRigidBody.drag = thisRigidBody.drag;
		fragmentRigidBody.angularDrag = thisRigidBody.angularDrag;
		fragmentRigidBody.useGravity = true;
		fragmentRigidBody.isKinematic = false;

		return obj;
	}
}