using UnityEngine;

public class DragAndSpawn : MonoBehaviour
{
	public Transform ObjectToSpawn;

	private bool _isSpawner = true;
	private Vector3 _screenPoint;
	private Vector3 _offset;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnMouseDown()
	{
		// TODO: Change this to own written code. This is only here for testing.
		// Code from: http://answers.unity3d.com/questions/12322/drag-gameobject-with-mouse.html By: MarkGX
		_screenPoint = Camera.main.WorldToScreenPoint(gameObject.transform.position);
		_offset = gameObject.transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, _screenPoint.z));
	}

	void OnMouseDrag()
	{
		if (_isSpawner)
		{
			Instantiate(ObjectToSpawn, gameObject.transform.position, Quaternion.identity);

			// Change color of the ball you grabbed, changing it to fully visable.
			Color color = gameObject.GetComponent<Renderer>().material.color;
			color.a = 1f;
			gameObject.GetComponent<Renderer>().material.color = color;

		}
		_isSpawner = false;

		// TODO: Change to own written code. Same as on OnMouseDown()
		Vector3 curScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, _screenPoint.z);
		Vector3 curPosition = Camera.main.ScreenToWorldPoint(curScreenPoint) + _offset;
		transform.position = curPosition;
	}

	void OnMouseUp()
	{
		// Snap to grid. Currently 1 unity unit is the grid we're based on.
		float currentX = Mathf.Round(gameObject.transform.position.x);
		float currentY = Mathf.Round(gameObject.transform.position.y);
		float currentZ = Mathf.Round(gameObject.transform.position.z);

		gameObject.transform.position = new Vector3(currentX, currentY, currentZ);
	}
}
