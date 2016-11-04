using UnityEngine;
using System.Collections;
using System.Security.Cryptography;

public class ComboParent : DragAndPlace
{
    public GameObject ComboResult;

    // tile size
    private float _xSize;
    private float _zSize;
    protected LayerMask _tiles;

    // raycast positions (valid tiles for the combo) from "main" tile (transform.position), where "1" is 8 units
    public int[] xPos;
    public int[] zPos;

    // Instantiated building position and rotation adjustment
    protected int[] xAdj;
    protected int[] zAdj;
    protected int[] rotAdj;
    // Use this for initialization
    protected virtual void Awake()
    {
        _xSize = _zSize = GameSettings.SNAP_VALUE;
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    protected override void OnPlaced()
    {
        base.OnPlaced();

        if (CheckSurroundingTiles()) return;
        AlertSurroundingTiles();
    }

    public virtual bool CheckSurroundingTiles()
    {
        for (var i = 0; i < xPos.Length; i++)
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position + transform.right * _xSize * xPos[i] + new Vector3(0, 10, 0) + transform.forward * _zSize * zPos[i], Vector3.down, out hit, 10, _tiles))
            {
                if (hit.collider.tag != "Tile" || !hit.transform.GetComponent<DragAndPlace>().Placed || !hit.transform.name.StartsWith("House")) continue;
                Instantiate(ComboResult, transform.position + transform.right * _xSize * xAdj[i] + transform.forward * _zSize * zAdj[i], Quaternion.Euler(0, transform.localEulerAngles.y + rotAdj[i], 0));
                Destroy(hit.transform.gameObject);
                Destroy(gameObject);
                return true;
            }
        }
        return false;
    }

    private void AlertSurroundingTiles()
    {
        
    }
}
