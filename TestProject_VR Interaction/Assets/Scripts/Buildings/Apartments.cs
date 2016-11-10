using UnityEngine;
using System.Collections.Generic;

public class Apartments : MonoBehaviour {

    public GameObject Skyscraper, Hotel;

    private float _xSize;
	private float _zSize;

	private LayerMask _tiles;
    private Combiner _combiner;
    private List<GameObject> _trashCan = new List<GameObject>();

    private int[] xPos, zPos, xAdj, zAdj, rotAdj;

    private int a_count, a2_count;

	void Awake ()
	{
	    _xSize = _zSize = GameSettings.SNAP_VALUE;
		_tiles = transform.GetComponent<DragAndPlace>().Tiles;
	    _combiner = GameObject.Find("Combiner").GetComponent<Combiner>();

         xPos = new [] { 0, 1, 1, 0, 0, 1, 2, 2, 2, 0, 1 };
         zPos = new [] { 1, 1, 0, 1, 2, 2, 2, 1, 0, 1, 1 };

         xAdj = new [] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
         zAdj = new [] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
        rotAdj = new[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
    }

    void CheckForCombos(bool combine)
    {
        _trashCan.Clear();
        a_count = 0;
        a2_count = 0;

        for (var i = 0; i < xPos.Length; i++)
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position + transform.right * _xSize * xPos[i] + new Vector3(0, 10, 0) + transform.forward * _zSize * zPos[i], Vector3.down, out hit, 10, _tiles))
            {
                if (hit.collider.tag != "Tile" || !hit.transform.GetComponent<DragAndPlace>().Placed) continue;

                GameObject result;

                if (hit.transform.name.StartsWith("Apartments") && i < 3)
                {
                    a_count++;
                    _trashCan.Add(hit.transform.gameObject);
                    if (i == 2 && a_count == 3)
                    {
                        result = Skyscraper;
                    }
                    else continue;
                }
                else if (hit.transform.name.StartsWith("Apartments") && i < 10)
                {
                    
                    _trashCan.Add(hit.transform.gameObject);
                    a2_count++;
                    continue;
                }
                else if (hit.transform.name.StartsWith("Company") && i == 10)
                {
                    _trashCan.Add(hit.transform.gameObject);
                    if (a2_count == 7)
                    {
                        result = Hotel;
                    }
                    else continue;
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