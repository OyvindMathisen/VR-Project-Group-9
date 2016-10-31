using UnityEngine;
using System.Collections;

public class LuxuryHouse : MonoBehaviour {

    public GameObject Villa;

    // tile size
    private float _xSize;
	private float _zSize;

	private LayerMask _tiles;
    private bool _combined;
	private AreaCheck _areaCheck;

	void Awake ()
	{
	    _xSize = _zSize = GameSettings.SNAP_VALUE;
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
			if (Physics.Raycast(transform.position + transform.right * _xSize * xPos[i] + new Vector3(0, 10, 0) + transform.forward * _zSize * zPos[i], Vector3.down, out hit, 10, _tiles))
	        {
	            if (hit.collider.tag != "Tile" || !hit.transform.GetComponent<DragAndPlace>().Placed || !hit.transform.name.StartsWith("House")) continue;
				Instantiate(Villa, transform.position + transform.right * _xSize * xAdj[i] + transform.forward * _zSize * zAdj[i], Quaternion.Euler(0, transform.localEulerAngles.y + rotAdj[i], 0));
	            Destroy(hit.transform.gameObject);
	            Destroy(gameObject);
	            _combined = true;
	            break;
	        }
	    }
	}
}