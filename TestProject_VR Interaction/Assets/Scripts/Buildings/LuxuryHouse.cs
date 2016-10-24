using UnityEngine;
using System.Collections;

public class LuxuryHouse : MonoBehaviour {

    public GameObject Villa;

    // tile size
    private float _xPos;
	private float _zPos;

	private LayerMask _tiles;
    private bool _combined;

	void Awake ()
	{
	    _xPos = _zPos = GameSettings.SNAP_VALUE;
	    _tiles = transform.GetComponent<DragAndPlace>().Tiles;
	}

	void Update () {
	    if (_combined) return;

	    // raycast positions (valid tiles for the combo) from "main" tile (transform.position), where "1" is 8 units
	    int[] xPos = { 1, 1, -1, -1 };
	    int[] zPos = { 0, 1, 0, 1 };

	    // Instantiated building position and rotation adjustment
	    int[] xAdj = { 0, 0, 0, 0 }; // <- (atm this is useless, but probably need for other kinds of combos)
	    int[] zAdj = { 0, 1, 0, 1 };
	    int[] rotAdj = { 0, 90, -90, -180 };

	    for (var i = 0; i < xPos.Length; i++)
	    {
	        RaycastHit hit;
	        if (Physics.Raycast(transform.position + transform.right * _xPos * xPos[i] + new Vector3(0, 10, 0) + transform.forward * _zPos * zPos[i], Vector3.down, out hit, 10, _tiles))
	        {
	            if (hit.collider.tag != "Tile" || !hit.transform.GetComponent<DragAndPlace>().Placed || !hit.transform.name.StartsWith("House")) continue;
	            var tile = Instantiate(Villa, transform.position + transform.right * _xPos * xAdj[i] + transform.forward * _zPos * zAdj[i], Quaternion.Euler(0, transform.localEulerAngles.y + rotAdj[i], 0));
	            Destroy(hit.transform.gameObject);
	            Destroy(gameObject);
	            _combined = true;
	            break;
	        }
	    }
	}
}