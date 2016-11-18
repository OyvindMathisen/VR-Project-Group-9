using UnityEngine;
using System.Collections.Generic;

public class Pond : MonoBehaviour {

    public GameObject SwimmingPool, Fountain, WaterTreatment;

    private float _xSize;
	private float _zSize;

	private LayerMask _tiles;
    private Combiner _combiner;
    private List<GameObject> _trashCan = new List<GameObject>();
    private List<GameObject>[] _garbageBin;
    private GameObject result;

    private int[] xPos, zPos, xAdj, zAdj, rotAdj;

    private int p_count, f_count, f2_count;

	void Awake ()
	{
	    _xSize = _zSize = GameSettings.SNAP_VALUE;
		_tiles = transform.GetComponent<DragAndPlace>().Tiles;
	    _combiner = GameObject.Find("Combiner").GetComponent<Combiner>();

        xPos = new[] { 1, 1, 0, 1, 1, 0, 1,  0, -1,  0,   1,  1,  0 };
        zPos = new[] { 0, 1, 1, 0, 1, 1, 0,  1,  0, -1,   0, -1, -1 };

        xAdj = new[] { 0, 0, 0, 0, 0, 0, 0,  0,  0,  0,   0,  0,  0 };
        zAdj = new[] { 0, 0, 0, 0, 0, 0, 0,  0,  0,  0,   0,  0,  0 };
      rotAdj = new[] { 0, 0, 0, 0, 0, 0, 90, 0, -90, 180, 0,  0,  90 };
    }

    void CheckForCombos(int I)
    {
        if (I < 0)
        {
            _trashCan.Clear();
            _garbageBin = new List<GameObject>[xPos.Length];
            for (var j = 0; j < xPos.Length; j++) _garbageBin[j] = new List<GameObject>();
            p_count = 0;
            f_count = 0;
            f2_count = 0;

            for (var i = 0; i < xPos.Length; i++)
            {
                RaycastHit hit;
                if (Physics.Raycast(transform.position + transform.right * _xSize * xPos[i] + new Vector3(0, 100, 0) + transform.forward * _zSize * zPos[i], Vector3.down, out hit, 100, _tiles))
                {
                    if (hit.collider.tag != "Tile" || !hit.transform.GetComponent<DragAndPlace>().Placed) continue;

                    if (hit.transform.name.StartsWith("Pond") && i < 3)
                    {
                        _trashCan.Add(hit.transform.gameObject);
                        p_count++;
                        if (p_count == 3)
                        {
                            result = WaterTreatment;
                        }
                        else continue;
                    }
                    else if (((i == 3 || i == 5) && hit.transform.name.StartsWith("Park"))
                        || (i == 4 && hit.transform.name.StartsWith("Pond")))
                    {
                        _trashCan.Add(hit.transform.gameObject);
                        f_count++;
                        if (f_count == 3)
                        {
                            result = Fountain;
                        }
                        else continue;
                    }
                    else if (hit.transform.name.StartsWith("Pond") && i > 5 && i < 10)
                    {
                        _trashCan.Clear();
                        _trashCan.Add(hit.transform.gameObject);
                        result = SwimmingPool;
                    }
                    else if (((i == 10 || i == 12) && hit.transform.name.StartsWith("Park"))
                        || (i == 11 && hit.transform.name.StartsWith("Pond")))
                    {
                        _trashCan.Add(hit.transform.gameObject);
                        f2_count++;
                        if (f2_count == 3)
                        {
                            result = Fountain;
                        }
                        else continue;
                    }
                    else continue;

                    if (gameObject != _combiner.LastPlacedTile && !_trashCan.Contains(_combiner.LastPlacedTile)) continue;
                    if (result == SwimmingPool && _combiner.LastPlacedTile != gameObject) continue;
                    _combiner.Alternatives.Add(gameObject);
                    _combiner.Names.Add(result.name);
                    _combiner.I.Add(i);

                    _trashCan.Add(gameObject);
                    _combiner.RelevantBuildings.Add(new List<GameObject>(_trashCan));
                    _garbageBin[i].AddRange(_trashCan);
                    _trashCan.Clear();
                }
            }
        }
        else
        {
            Instantiate(result, transform.position + transform.right * _xSize * xAdj[I] + transform.forward * _zSize * zAdj[I], Quaternion.Euler(0, transform.localEulerAngles.y + rotAdj[I], 0));
            foreach (var obj in _garbageBin[I]) Destroy(obj);
        }
    }
}