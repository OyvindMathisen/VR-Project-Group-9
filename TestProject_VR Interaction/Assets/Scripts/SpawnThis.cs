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

	//void OnMouseDown()
	//{
	//	// TODO: Change this to own written code. This is only here for testing.
	//	root._screenPoint = Camera.main.WorldToScreenPoint(gameObject.transform.position);
	//	root._offset = gameObject.transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, root._screenPoint.z));
 //       Instantiate(ObjectToSpawn, gameObject.transform.position, Quaternion.identity);
 //   }

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
