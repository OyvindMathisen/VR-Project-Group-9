using UnityEngine;
using System.Collections.Generic;

public class Park : MonoBehaviour {

    public GameObject Sculpture, Playground, Estate;

    private float _xSize;
	private float _zSize;

	private LayerMask _tiles;
    private Combiner _combiner;
    private List<GameObject> _trashCan = new List<GameObject>();

    private int[] xPos, zPos, xAdj, zAdj, rotAdj;

    void Awake ()
	{
	    _xSize = _zSize = GameSettings.SNAP_VALUE;
		_tiles = transform.GetComponent<DragAndPlace>().Tiles;
	    _combiner = GameObject.Find("Combiner").GetComponent<Combiner>();

         xPos = new [] { 1, 0, -1, 0 };
         zPos = new [] { 0, 1, 0, -1 };

         xAdj = new [] { 0, 0, 0, 0 };
         zAdj = new [] { 0, 0, 0, 0 };
         rotAdj = new[] { 90, 0, -90, 180 };
    }

    void CheckForCombos(bool combine)
    {
        _trashCan.Clear();

        for (var i = 0; i < xPos.Length; i++)
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position + transform.right * _xSize * xPos[i] + new Vector3(0, 10, 0) + transform.forward * _zSize * zPos[i], Vector3.down, out hit, 10, _tiles))
            {
                if (hit.collider.tag != "Tile" || !hit.transform.GetComponent<DragAndPlace>().Placed) continue;

                GameObject result;
                if (hit.transform.name.StartsWith("Luxury house") || hit.transform.name.StartsWith("Mansion"))
                {
                    _trashCan.Add(hit.transform.gameObject);
                    result = Estate;
                }
                else if (hit.transform.name.StartsWith("Park"))
                {
                    _trashCan.Add(hit.transform.gameObject);
                    result = Sculpture;
                }
                else if (hit.transform.name.StartsWith("Plaza"))
                {
                    _trashCan.Add(hit.transform.gameObject);
                    result = Playground;
                }
                else continue;

                if (combine)
                {
                    Instantiate(result, transform.position + transform.right * _xSize * xAdj[i] + transform.forward * _zSize * zAdj[i], Quaternion.Euler(0, transform.localEulerAngles.y + rotAdj[i], 0));

                    if (result == Estate)
                    {
                        Destroy(gameObject);
                    }
                    else
                    {
                        _trashCan.Add(gameObject);
                        foreach (var obj in _trashCan) Destroy(obj);
                    }                   
                }
                else
                {
                    if (gameObject == _combiner.LastPlacedTile ||
                        _trashCan.Contains(_combiner.LastPlacedTile))
                    {
                        // to prevent two possible alternatives when it's actually one
                        if (!(result == Sculpture && _combiner.LastPlacedTile != gameObject)) continue;

                        _combiner.Alternatives.Add(gameObject);
                        _combiner.Names.Add(result.name);
                    }
                }
                break;
            }
        }
    }
}