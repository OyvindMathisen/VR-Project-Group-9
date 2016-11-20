using UnityEngine;

public class ConstantBuildingRotation : MonoBehaviour
{
	// Standard values. Will not rotate.
	// Change this in the Unity editor.
	public int xRotation = 0;
	public int yRotation = 0;
	public int zRotation = 0;

	void FixedUpdate()
	{
		transform.Rotate(xRotation, yRotation, zRotation);
	}
}
