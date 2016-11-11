using UnityEngine;

public class SpawnThis : MonoBehaviour
{
	public GameObject ObjectToSpawn;
	private Wand _controller;

	void Start ()
    {
		_controller = HMDComponents.getRightWand();
	}

	void OnTriggerStay(Collider other)
	{
	    if (other.tag != "Rhand" || !_controller.TriggerButtonDown) return;

		Instantiate (ObjectToSpawn, _controller.transform.position, Quaternion.identity);
	    _controller.IsHolding = true;
	}
}
