using UnityEngine;

public class SpawnThis : MonoBehaviour
{
	public Transform ObjectToSpawn;
	private Wand Rhand;
	private bool ObjectSpawned = false;
	private GameObject go;
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
				Debug.Log("NewPreviewArea from SpawnThis");
				//GameObject.Find("PreviewPlacement").GetComponent<AreaCheck>().Invoke("NewPreviewArea", 0.02f);

				// go = GameObject
				Instantiate(ObjectToSpawn, gameObject.transform.position, Quaternion.identity);

				root.isHolding = true;
			}
		}
	}
}
