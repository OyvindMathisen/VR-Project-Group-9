﻿using UnityEngine;

public class TrashcanScript : MonoBehaviour
{
	private GameObject parent;

	void OnTriggerEnter(Collider col)
	{
		if (col.tag != "Tile") return; // Prevents accidentally deleting anything but buildings.
		parent = col.transform.parent.gameObject;
		var script = parent.GetComponent<DragAndPlace>();

		// If the tile has no DragAndPlace script, return its
		// name and stop the execution of the script.
		if (!script)
		{
			Debug.Log("Error. " + col.name + " could not get DragAndPlace script.");
			return;
		}

		if (!script.Dropped || script.ReachedHeight) return; // Only delete objects currently falling.

		// Allows the controller to register the object leaving the players hand before
		// deleting, preventing a controller lock.
		parent.transform.position = new Vector3(-30000, 0, -30000);
		Invoke("funcDestroy", 0.05f);
	}

	void funcDestroy()
	{
		Destroy(parent);
	}
}
