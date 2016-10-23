using UnityEngine;

public class SpawnThis : MonoBehaviour
{
	public Transform ObjectToSpawn;
	private Wand Rhand;

	void Awake () {
		Rhand = GameObject.Find("[CameraRig]").transform.FindChild("Controller (right)").GetComponent<Wand>();
	}

    void FixedUpdate()
    {
        transform.Rotate(0, 1, 0);
    }

	void OnTriggerStay(Collider other)
	{
		if (other.tag == "Rhand")
		{
			if (Rhand.triggerButtonDown)
			{
				var temp = Rhand.transform.position;
				temp.x = Mathf.Round(temp.x * root.SNAP_INVERSE) / root.SNAP_INVERSE;
				temp.z = Mathf.Round(temp.z * root.SNAP_INVERSE) / root.SNAP_INVERSE;

				Instantiate(ObjectToSpawn, Rhand.transform.position, Quaternion.identity);

				root.isHolding = true;
			}
		}
	}
}
