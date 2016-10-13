using System;
using UnityEngine;

public class DragAndPlace : MonoBehaviour
{
	private float BUILD_HEIGHT = 100.0f, BUILD_HEIGHT_LERP = 15.0f; // make the tiles stay at a certain height
	public bool placed = false; // if the tile is still "dragged" around (the mouse button is not released yet)
	private float SNAP_VALUE = 8.0f; // important for choosing grid size (do not edit unless you edit the tile sizes)
	float snapInverse;
	private Wand Rhand;
	private bool hasRotated = false;
	public LayerMask tiles;
	private Collider ownCollider;

	private GameObject previewPlacement;
	private MeshRenderer previewMesh;

	void Awake()
	{
		snapInverse = 1 / SNAP_VALUE;
		Rhand = GameObject.Find("[CameraRig]").transform.FindChild("Controller (right)").GetComponent<Wand>();

		ownCollider = GetComponent<Collider>();
		previewPlacement = GameObject.Find("PreviewPlacement");
		previewMesh = previewPlacement.GetComponent<MeshRenderer>();
		previewMesh.enabled = false;
	}
	void Update()
	{
		if (!placed)
		{
			// Collision check to prevent overlapping buildings
			if (Rhand.triggerButtonUp)
			{
				RaycastHit hit;
				if (Physics.Raycast(previewPlacement.transform.position + new Vector3(0, -10, 0), Vector3.up, out hit, Mathf.Infinity, tiles))
				{
					if (hit.collider != ownCollider)
					{
						if (hit.collider.tag == "Tile")
						{
							Destroy(gameObject);
						}
					}
				}

				if (Physics.Raycast(previewPlacement.transform.position + new Vector3(0, 10, 0), Vector3.down, out hit, Mathf.Infinity, tiles))
				{
					if (hit.collider != ownCollider)
					{
						if (hit.collider.tag == "Tile")
						{
							Destroy(gameObject);
						}
					}
				}

				transform.position = Rhand.transform.position;

				previewMesh.enabled = false;

				placed = true;
				root.isHolding = false;
			}

			// Rotate handler, right direction
			if (Rhand.touchpadRight && !hasRotated)
			{
				transform.Rotate(0, -90, 0);
				hasRotated = true;
			}

			// Rotate handler, left direction
			if (Rhand.touchpadLeft && !hasRotated)
			{
				transform.Rotate(0, 90, 0);
				hasRotated = true;
			}

			// Delete the building in hand
			if (Rhand.gripButtonDown)
			{
				root.isHolding = false;
				previewMesh.enabled = false;
				Destroy(gameObject);
			}

			// To prevent the building from rotating at the speed of light
			if (!Rhand.touchpadLeft && !Rhand.touchpadRight)
			{
				hasRotated = false;
			}
		}

		// TODO: Merge with other !placed above
		if (!placed)
		{
			Vector3 curPosition = Rhand.transform.position; 

			float currentX = Mathf.Round(curPosition.x * snapInverse) / snapInverse;
			float currentZ = Mathf.Round(curPosition.z * snapInverse) / snapInverse;

			previewMesh.enabled = true;
			previewPlacement.transform.position = new Vector3(currentX, BUILD_HEIGHT, currentZ);

			var temp = transform.position;
			temp.x = Mathf.Lerp(transform.position.x, curPosition.x, 0.15f);
			temp.y = Mathf.Lerp(transform.position.y, curPosition.y, 0.15f);
			temp.z = Mathf.Lerp(transform.position.z, curPosition.z, 0.15f);
			transform.position = temp;
		}
		else
		{
			if (transform.position.y != BUILD_HEIGHT)
			{
				var temp = transform.position;
				temp.x = Mathf.Round(temp.x * snapInverse) / snapInverse;
				temp.z = Mathf.Round(temp.z * snapInverse) / snapInverse;
				temp.y = Mathf.Lerp(transform.position.y, BUILD_HEIGHT, BUILD_HEIGHT_LERP);
				transform.position = temp;

				AreaCheck ac = previewPlacement.GetComponent<AreaCheck>();
				ac.CheckForValidCombo(gameObject);
			}
		}
	}

	void OnTriggerStay(Collider other)
	{
		if (other.tag == "Rhand" && Rhand.triggerButtonDown && !root.isHolding)
		{
			placed = false;
			root.isHolding = true;
		}
	}
}
