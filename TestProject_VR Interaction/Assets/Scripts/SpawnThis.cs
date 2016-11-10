using UnityEngine;

public class SpawnThis : MonoBehaviour
{
	public Transform ObjectToSpawn;

	[Header("Initial Spawn Rotation")]
	public float XRotation = 0;
	public float YRotation = 0;
	public float ZRotation = 0;

	private Wand _controller;

	void Start ()
    {
		_controller = HMDComponents.getRightWand();
	}

	void OnTriggerStay(Collider other)
	{
	    if (other.tag != "Rhand" || !_controller.TriggerButtonDown) return;

	    var currentTile = Instantiate(ObjectToSpawn, _controller.transform.position, Quaternion.identity) as GameObject;
		if (!currentTile) // Is currentTile is null.
		{
			Debug.Log("Unable to instansiate object. Instansiation cancelled.");
			return;
		}
		currentTile.transform.Rotate(new Vector3(XRotation, YRotation, ZRotation)); // Rotate the building the requested amount.
	    _controller.IsHolding = true;
	}
}
