using UnityEngine;
using System.Collections;


public class BuildingKeepRotation : MonoBehaviour
{
	public float yRotation = 0.0f;
	private DragAndPlace _buildingMovementScript;

	// Get the required scrips from the building.
	void Start () {
		_buildingMovementScript = GetComponent<DragAndPlace> ();
	}

	void Update ()
	{
		if (_buildingMovementScript.Dropped) return;
		transform.rotation = Quaternion.Euler (Vector3.up * yRotation);
	}

	public void RotateBuilding(DirectionLR Direction)
	{
		if (Direction == DirectionLR.Right) {
			yRotation += 90;
		} else {
			yRotation -= 90;
		}

		// Standarizes rotation between -180 to 180.
		if (yRotation > 180) yRotation -= 360;
		if (yRotation < -180) yRotation += 360;
	}

	void SaveRotation()
	{
		yRotation = transform.rotation.eulerAngles.y;
	}
}
