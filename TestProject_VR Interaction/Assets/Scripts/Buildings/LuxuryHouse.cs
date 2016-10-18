using UnityEngine;
using System.Collections;

public class LuxuryHouse : MonoBehaviour {

    // tile size
	private float x;
	private float z;

	private LayerMask tiles;

	public GameObject Villa;

    private bool combined;

	void Awake ()
	{
	    x = z = root.SNAP_VALUE;
	    tiles = transform.GetComponent<DragAndPlace>().tiles;
	}

	void Update () {
        if (!combined)
        {
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
                if (Physics.Raycast(transform.position + transform.right * x * xPos[i] + new Vector3(0, 10, 0) + transform.forward * z * zPos[i], Vector3.down, out hit, 10, tiles))
                {
                    if (hit.collider.tag == "Tile")
                    {
                        if (hit.transform.GetComponent<DragAndPlace>().placed)
                        {
                            if (hit.transform.name.StartsWith("House"))
                            {
                                var tile = Instantiate(Villa, transform.position + transform.right * x * xAdj[i] + transform.forward * z * zAdj[i], Quaternion.Euler(0, transform.localEulerAngles.y + rotAdj[i], 0));
                                Destroy(hit.transform.gameObject);
                                Destroy(gameObject);
                                combined = true;
                                break;
                            }
                        }
                    }
                }
            }
        }
    }
}