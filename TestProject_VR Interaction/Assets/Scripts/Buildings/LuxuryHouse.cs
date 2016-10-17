using UnityEngine;
using System.Collections;

public class LuxuryHouse : MonoBehaviour {

	private float x = 8;
	private float y = 200;
	private float z = 8;

	private LayerMask tiles = 9;

	public GameObject Villa;

	private bool f;

	// Use this for initialization
	void Awake () {
		
	}
	
	// Update is called once per frame
	void Update () {
		RaycastHit hit;

		if (Physics.Raycast(transform.position + new Vector3(x * -1, y, z), Vector3.down, out hit, Mathf.Infinity, tiles.value))
		{
			if (hit.collider.tag == "Tile")
			{
				if (hit.transform.GetComponent<DragAndPlace>().placed)
				{
					if (hit.transform.name.StartsWith("House"))
					{
						var tile = Instantiate(Villa, transform.position, Quaternion.Euler(0, 180, 0));
						f = true;
					}
				}
			}
		}
		if (Physics.Raycast(transform.position + new Vector3(x * -1, y, z * x * -1), Vector3.down, out hit, Mathf.Infinity, tiles.value))
		{
			if (hit.collider.tag == "Tile")
			{
				if (hit.transform.GetComponent<DragAndPlace>().placed)
				{
					if (hit.transform.name.StartsWith("House"))
					{
						var tile = Instantiate(Villa, transform.position, Quaternion.Euler(0, 180, 0));
						f = true;
					}
				}
			}
		}
		if (Physics.Raycast(transform.position + new Vector3(x * 2, y, z), Vector3.down, out hit, Mathf.Infinity, tiles.value))
		{
			if (hit.collider.tag == "Tile")
			{
				if (hit.transform.GetComponent<DragAndPlace>().placed)
				{
					if (hit.transform.name.StartsWith("House"))
					{
						var tile = Instantiate(Villa, transform.position, Quaternion.Euler(0, 180, 0));
						f = true;
					}
				}
			}
		}
		if (Physics.Raycast(transform.position + new Vector3(x * 2, y, z * -1), Vector3.down, out hit, Mathf.Infinity, tiles.value))
		{
			if (hit.collider.tag == "Tile")
			{
				if (hit.transform.GetComponent<DragAndPlace>().placed)
				{
					if (hit.transform.name.StartsWith("House"))
					{
						var tile = Instantiate(Villa, transform.position, Quaternion.Euler(0, 180, 0));
						f = true;
					}
				}
			}
		}
		if (f)
		{
			Destroy(hit.transform.gameObject);
			Destroy(gameObject);
		}
	}
}
