using UnityEngine;

public class SpawnThis : MonoBehaviour
{
	public Transform ObjectToSpawn;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        // temp solution
	    if (Input.GetKeyDown(root.nextPreviewKey)
            || Input.GetKeyDown(root.prevPreviewKey)
            || Input.GetKeyDown(KeyCode.Alpha1)
            || Input.GetKeyDown(KeyCode.Alpha2))
        {
            Destroy(gameObject);
        }
	}

    void FixedUpdate()
    {
    }

	void OnMouseDown()
	{
		// TODO: Change this to own written code. This is only here for testing.
		// Code from: http://answers.unity3d.com/questions/12322/drag-gameobject-with-mouse.html By: MarkGX
		root._screenPoint = Camera.main.WorldToScreenPoint(gameObject.transform.position);
		root._offset = gameObject.transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, root._screenPoint.z));
        Instantiate(ObjectToSpawn, gameObject.transform.position, Quaternion.identity);
    }
}
