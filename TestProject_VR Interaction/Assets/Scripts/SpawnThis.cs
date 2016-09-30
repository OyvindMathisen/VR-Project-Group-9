using UnityEngine;

public class SpawnThis : MonoBehaviour
{
	public Transform ObjectToSpawn;
	private Wand Rhand;
	private bool ObjectSpawned = false;
	// Use this for initialization
	void Awake () {
        transform.rotation = root.previewRotation;

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
			if (Rhand.triggerButtonDown && !ObjectSpawned)
			{
				Instantiate(ObjectToSpawn, gameObject.transform.position, Quaternion.identity);
			}
		}
	}
}
