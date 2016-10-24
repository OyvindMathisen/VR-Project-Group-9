using UnityEngine;

public class SpawnThis : MonoBehaviour
{
	public Transform ObjectToSpawn;
    private GameObject _rightHand;
	private Wand _controller;

	void Awake ()
    {
        _rightHand = GameObject.FindWithTag("Rhand");
		_controller = _rightHand.GetComponent<Wand>();
	}

    void FixedUpdate()
    {
        transform.Rotate(0, 1, 0);
    }

	void OnTriggerStay(Collider other)
	{
	    if (other.tag != "Rhand" || !_controller.TriggerButtonDown) return;

	    // var objectPosition = _controller.transform.position;
	    // objectPosition.x = Mathf.Round(objectPosition.x * root.SNAP_INVERSE) / root.SNAP_INVERSE;
	    // objectPosition.z = Mathf.Round(objectPosition.z * root.SNAP_INVERSE) / root.SNAP_INVERSE;

	    Instantiate(ObjectToSpawn, _controller.transform.position, Quaternion.identity);

	    _controller.IsHolding = true;
	}
}
