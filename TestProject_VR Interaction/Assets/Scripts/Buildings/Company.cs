using UnityEngine;
using System.Collections.Generic;

public class Company : MonoBehaviour {

    public GameObject Restaurant;

    private float _xSize;
	private float _zSize;

	private LayerMask _tiles;
    private Combiner _combiner;
    private List<GameObject> _trashCan = new List<GameObject>();

    private int[] xPos, zPos, xAdj, zAdj, rotAdj;

    private int r_num;
	void Awake ()
	{
	    _xSize = _zSize = GameSettings.SNAP_VALUE;
		_tiles = transform.GetComponent<DragAndPlace>().Tiles;
	    _combiner = GameObject.Find("Combiner").GetComponent<Combiner>();

         xPos = new [] { 0, 1, -1,  0, 1, -1 };
         zPos = new [] { 1, 0,  0, -1, 0,  0 };

         xAdj = new [] { 0, 0, 0, 0, 0, 0 };
         zAdj = new [] { 0, 0, 0, 0, 0, 0 };
        rotAdj = new[] { 0, 0, -90, 0, 90, 180 };
    }

    void CheckForCombos(bool combine)
    {
        _trashCan.Clear();
        r_num = -2;

        for (var i = 0; i < xPos.Length; i++)
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position + transform.right * _xSize * xPos[i] + new Vector3(0, 10, 0) + transform.forward * _zSize * zPos[i], Vector3.down, out hit, 10, _tiles))
            {
                if (hit.collider.tag != "Tile" || !hit.transform.GetComponent<DragAndPlace>().Placed) continue;

                GameObject result;
                if (hit.transform.name.StartsWith("Cafe") && (i == 0 || i == 3))
                {
                    _trashCan.Add(hit.transform.gameObject);
                    r_num = i;
                    continue;
                    
                }
                if (hit.transform.name.StartsWith("Cafe") &&
                    (r_num == 0 && (i == 1 || i == 2)) || (r_num == 3 && (i == 4 || i == 5)))
                {
                    _trashCan.Add(hit.transform.gameObject);
                    result = Restaurant;
                }
                
                else continue;

                if (combine)
                {
                    Instantiate(result, transform.position + transform.right * _xSize * xAdj[i] + transform.forward * _zSize * zAdj[i], Quaternion.Euler(0, transform.localEulerAngles.y + rotAdj[i], 0));

                    _trashCan.Add(gameObject);
                    foreach (var obj in _trashCan) Destroy(obj);
                }
                else
                {
                    if (gameObject == _combiner.LastPlacedTile ||
                        _trashCan.Contains(_combiner.LastPlacedTile))
                    {
                        _combiner.Alternatives.Add(gameObject);
                        _combiner.Names.Add(result.name);
                    }
                }
                break;
            }
        }
    }
}