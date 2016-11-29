using UnityEngine;
using System.Collections.Generic;

public class Market : MonoBehaviour
{
	public GameObject FishMarket;

	private float _xSize;
	private float _zSize;

	private LayerMask _tiles;
	private Combiner _combiner;
    private ComboTracker _comboTracker;
    private List<GameObject> _trashCan = new List<GameObject>();
	private List<GameObject>[] _garbageBin;
	private GameObject result;
    private DragAndPlace _buildingScript;

	private int[] xPos, zPos, xAdj, zAdj, rotAdj;

    private bool _checkFlag;
    private int _fishMarketCount;

    void Awake()
	{
		_xSize = _zSize = GameSettings.SNAP_VALUE;
        _buildingScript = GetComponent<DragAndPlace>();
		_tiles = _buildingScript.Tiles;
		_combiner = GameObject.Find("Combiner").GetComponent<Combiner>();
        _comboTracker = _combiner.transform.GetComponent<ComboTracker>();

        xPos = new[] { 0 };
		zPos = new[] { 0 };

		xAdj = new[] { 0 };
		zAdj = new[] { 0 };
		rotAdj = new[] { 0 };
	}

    void Update()
    {
        if (transform.position.x > 31 && transform.position.z > 15)
        {
            if (_buildingScript.ReachedHeight && !_checkFlag)
            {
                CheckForCombos(-2);
                _checkFlag = true;
            }
            else if (!_buildingScript.ReachedHeight)
            {
                _fishMarketCount = 0;
                _checkFlag = false;
            }
        }
    }

	void CheckForCombos(int I)
	{
		if (I < 0)
		{
			_trashCan.Clear();
			_garbageBin = new List<GameObject>[xPos.Length];
			for (var j = 0; j < xPos.Length; j++) _garbageBin[j] = new List<GameObject>();

			for (var i = 0; i < xPos.Length; i++)
			{
				RaycastHit hit;
				if (Physics.Raycast(transform.position + transform.right * _xSize * xPos[i] + new Vector3(0, 100, 0) + transform.forward * _zSize * zPos[i], Vector3.down, out hit, 100, _tiles))
				{
					if (hit.collider.tag != "Tile" || !hit.transform.GetComponent<DragAndPlace>().Dropped) continue;

					if (transform.position.x > 31 && transform.position.z > 15)
					{
                        _trashCan.Add(hit.transform.gameObject);
						result = FishMarket;
                        _fishMarketCount++;
                    }
					else continue;

					if (gameObject != _combiner.LastPlacedTile && !_trashCan.Contains(_combiner.LastPlacedTile))
                    {
                        _trashCan.Clear();
                        continue;
                    }

                    if (result == FishMarket && _fishMarketCount > 1) continue;

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

			if (result != FishMarket)
			{
				_garbageBin[I].Add(gameObject);
				_combiner.DeletePredecessors(_garbageBin[I]);
			}
			else
			{
				_garbageBin[I].Clear();
				_garbageBin[I].Add(gameObject);
				_combiner.DeletePredecessors(_garbageBin[I]);
			}
            _comboTracker.CheckIfNew(result.name);
        }
	}
}