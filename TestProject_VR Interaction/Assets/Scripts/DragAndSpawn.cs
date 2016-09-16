using UnityEngine;

public class DragAndSpawn : MonoBehaviour
{
	public Transform Ball;

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
		_screenPoint = Camera.main.WorldToScreenPoint(gameObject.transform.position);
		_offset = gameObject.transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, _screenPoint.z));
	}

	void OnMouseDrag()
	{
		if (_isSpawner)
		{
			Instantiate(Ball, gameObject.transform.position, Quaternion.identity);

			// Change color of the ball you grabbed, changing it to fully visable.
			Color c = gameObject.GetComponent<Renderer>().material.color;
			c.a = 1f;
			gameObject.GetComponent<Renderer>().material.color = c;

		}
		_isSpawner = false;

		Vector3 curScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, _screenPoint.z);
		Vector3 curPosition = Camera.main.ScreenToWorldPoint(curScreenPoint) + _offset;
		transform.position = curPosition;
	}
}
