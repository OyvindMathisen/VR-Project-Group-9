﻿using UnityEngine;
using System.Collections.Generic;

public class Venue : MonoBehaviour
{
	public GameObject Hall, Store;

	private float _xSize;
	private float _zSize;

	private LayerMask _tiles;
	private Combiner _combiner;
    private ComboTracker _comboTracker;
    private List<GameObject> _trashCan = new List<GameObject>();
	private List<GameObject>[] _garbageBin;
	private GameObject result;

	private int[] xPos, zPos, xAdj, zAdj, rotAdj;

	void Awake()
	{
		_xSize = _zSize = GameSettings.SNAP_VALUE;
		_tiles = transform.GetComponent<DragAndPlace>().Tiles;
		_combiner = GameObject.Find("Combiner").GetComponent<Combiner>();
        _comboTracker = _combiner.transform.GetComponent<ComboTracker>();

        xPos = new[] { 1, 0, -1, 0 };
		zPos = new[] { 0, 1, 0, -1 };

		xAdj = new[] { 0, 0, 0, 0 };
		zAdj = new[] { 0, 0, 0, 0 };
		rotAdj = new[] { 90, 0, -90, 180 };
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

					if (hit.transform.name.StartsWith("Venue"))
					{
						_trashCan.Add(hit.transform.gameObject);
						result = Hall;
					}
					else if (hit.transform.name.StartsWith("Market"))
					{
						_trashCan.Add(hit.transform.gameObject);
						result = Store;
					}
					else continue;

					if (gameObject != _combiner.LastPlacedTile && !_trashCan.Contains(_combiner.LastPlacedTile))
                    {
                        _trashCan.Clear();
                        continue;
                    }
                    // to prevent two possible alternatives when it's actually one
                    if (result == Hall && _combiner.LastPlacedTile != gameObject) continue;

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
			_combiner.DeletePredecessors(_garbageBin[I]);
            _comboTracker.CheckIfNew(result.name);
        }
	}
}