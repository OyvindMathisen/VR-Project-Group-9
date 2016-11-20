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
			Debug.Log("Error. " + col.name + " could not get DragAndPlace script.");
			return;
		}

		if (!script.Dropped || script.ReachedHeight) return; // Only delete objects currently falling.
		Destroy(parent);
	}

}
