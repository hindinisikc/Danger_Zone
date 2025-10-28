using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(Rigidbody))]
public class Fracture : MonoBehaviour
{
	public TriggerOptions triggerOptions;       // Options for triggering fractures
	public FractureOptions fractureOptions;     // Options for fracturing behavior
	public RefractureOptions refractureOptions; // Options for refracturing behavior
	public CallbackOptions callbackOptions;     // Options for callbacks on fracture events

	/// <summary>
	/// The number of times this fragment has been re-fractured.
	/// </summary>
	[HideInInspector]
	public int currentRefractureCount = 0;

	/// <summary>
	/// Collector object that stores the produced fragments
	/// </summary>
	private GameObject fragmentRoot;

	[ContextMenu("Print Mesh Info")]
	public void PrintMeshInfo()
	{
		var mesh = GetComponent<MeshFilter>().mesh;
		Debug.Log("Positions");

		var positions = mesh.vertices;
		var normals = mesh.normals;
		var uvs = mesh.uv;

		for (int i = 0; i < positions.Length; i++)
		{
			Debug.Log($"Vertex {i}");
			Debug.Log($"POS | X: {positions[i].x} Y: {positions[i].y} Z: {positions[i].z}");
			Debug.Log($"NRM | X: {normals[i].x} Y: {normals[i].y} Z: {normals[i].z} LEN: {normals[i].magnitude}");
			Debug.Log($"UV  | U: {uvs[i].x} V: {uvs[i].y}");
			Debug.Log("");
		}
	}

	public void CauseFracture()
	{
		callbackOptions.CallOnFracture(null, gameObject, transform.position);
		ComputeFracture();
	}

	void OnValidate()
	{
		// Validate parent scale for correct rendering of fragments
		if (transform.parent != null)
		{
			var scale = transform.parent.localScale;
			if ((scale.x != scale.y) || (scale.x != scale.z) || (scale.y != scale.z))
			{
				Debug.LogWarning($"Warning: Parent transform of fractured object must be uniformly scaled in all axes or fragments will not render correctly.", transform);
			}
		}
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
					ComputeFracture();
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
				ComputeFracture();
			}
		}
	}

	void Update()
	{
		if (triggerOptions.triggerType == TriggerType.Keyboard)
		{
			if (Input.GetKeyDown(triggerOptions.triggerKey))
			{
				callbackOptions.CallOnFracture(null, gameObject, transform.position);
				ComputeFracture();
			}
		}
	}

	/// <summary>
	/// Compute the fracture and create the fragments
	/// </summary>
	private void ComputeFracture()
	{
		var mesh = GetComponent<MeshFilter>().sharedMesh;

		if (mesh != null)
		{
			if (fragmentRoot == null)
			{
				fragmentRoot = new GameObject($"{name}Fragments");
				fragmentRoot.transform.SetParent(transform.parent);
				fragmentRoot.transform.position = transform.position;
				fragmentRoot.transform.rotation = transform.rotation;
				fragmentRoot.transform.localScale = Vector3.one;
			}

			var fragmentTemplate = CreateFragmentTemplate();

			if (fractureOptions.asynchronous)
			{
				StartCoroutine(Fragmenter.FractureAsync(
					gameObject,
					fractureOptions,
					fragmentTemplate,
					fragmentRoot.transform,
					() =>
					{
						Destroy(fragmentTemplate);
						gameObject.SetActive(false);

						if ((currentRefractureCount == 0) ||
							(currentRefractureCount > 0 && refractureOptions.invokeCallbacks))
						{
							callbackOptions.onCompleted?.Invoke();
						}
					}
				));
			}
			else
			{
				Fragmenter.Fracture(gameObject,
									fractureOptions,
									fragmentTemplate,
									fragmentRoot.transform);

				// Add DebrisDamage component to each fragment
				foreach (Transform child in fragmentRoot.transform)
				{
					var debrisDamage = child.gameObject.AddComponent<DebrisDamage>();
					debrisDamage.damageAmount = 100f; // Set the damage amount as needed
				}

				Destroy(fragmentTemplate);
				gameObject.SetActive(false);

				if ((currentRefractureCount == 0) ||
					(currentRefractureCount > 0 && refractureOptions.invokeCallbacks))
				{
					callbackOptions.onCompleted?.Invoke();
				}
			}
		}
	}

	/// <summary>
	/// Creates a template object from which each fragment will derive
	/// </summary>
	private GameObject CreateFragmentTemplate()
	{
		GameObject obj = new GameObject();
		obj.name = "Fragment";
		obj.tag = tag;

		obj.AddComponent<MeshFilter>();

		var meshRenderer = obj.AddComponent<MeshRenderer>();
		meshRenderer.sharedMaterials = new Material[2] {
			GetComponent<MeshRenderer>().sharedMaterial,
			fractureOptions.insideMaterial
		};

		var thisCollider = GetComponent<Collider>();
		var fragmentCollider = obj.AddComponent<MeshCollider>();
		fragmentCollider.convex = true;
		fragmentCollider.sharedMaterial = thisCollider.sharedMaterial;
		fragmentCollider.isTrigger = thisCollider.isTrigger;

		var thisRigidBody = GetComponent<Rigidbody>();
		var fragmentRigidBody = obj.AddComponent<Rigidbody>();
		fragmentRigidBody.velocity = thisRigidBody.velocity;
		fragmentRigidBody.angularVelocity = thisRigidBody.angularVelocity;
		fragmentRigidBody.drag = thisRigidBody.drag;
		fragmentRigidBody.angularDrag = thisRigidBody.angularDrag;
		fragmentRigidBody.useGravity = thisRigidBody.useGravity;

		if (refractureOptions.enableRefracturing &&
		   (currentRefractureCount < refractureOptions.maxRefractureCount))
		{
			CopyFractureComponent(obj);
		}

		return obj;
	}

	/// <summary>
	/// Copies this component to another GameObject
	/// </summary>
	private void CopyFractureComponent(GameObject obj)
	{
		var fractureComponent = obj.AddComponent<Fracture>();

		fractureComponent.triggerOptions = triggerOptions;
		fractureComponent.fractureOptions = fractureOptions;
		fractureComponent.refractureOptions = refractureOptions;
		fractureComponent.callbackOptions = callbackOptions;
		fractureComponent.currentRefractureCount = currentRefractureCount + 1;
		fractureComponent.fragmentRoot = fragmentRoot;
	}
}