using UnityEngine;
using System.Collections;

public class TrashcanScript : MonoBehaviour
{
    void OnTriggerEnter(Collider col)
    {
        if (col.tag != "Tile") return; // Prevents accidentally deleting anything but buildings.
		GameObject parent = col.transform.parent.gameObject;
        var script = parent.GetComponent<DragAndPlace>();

		// If the tile has no DragAndPlace script, return its
		// name and stop the execution of the script.
		if (!script)
		{
			Debug.Log ("Error. " + col.name + " could not get DragAndPlace script.");
			return;
		}

        if (!script.Placed || script.ReachedHeight) return; // Only delete objects currently falling.
		Destroy(parent);
    }

	/*
	// This method requires not having the colliders be on the child objects.
	// There is currently no need to have the colliders be there either. Simply
	// leave the empty gameobjects there to get their position.
	void OnTriggerEnter(Collider col)
	{
		if (col.tag != "Tile") return; // Prevents accidentally deleting anything but buildings.
		var script = parent.GetComponent<DragAndPlace>();
		if (!script.Placed || script.ReachedHeight) return; // Only delete objects currently falling.
		Destroy(col.GameObject);
	}
	*/
}
