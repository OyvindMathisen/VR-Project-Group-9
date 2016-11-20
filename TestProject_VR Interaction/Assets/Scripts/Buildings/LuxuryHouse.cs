using UnityEngine;
using System.Collections.Generic;

public class LuxuryHouse : MonoBehaviour
{
	public GameObject Villa, Mansion;

	// tile size
	private float _xSize;
	private float _zSize;

	private LayerMask _tiles;
	private Combiner _combiner;
	private List<GameObject> _trashCan = new List<GameObject>();
	private List<GameObject>[] _garbageBin;
	private GameObject result;

	private int[] xPos, zPos, xAdj, zAdj, rotAdj;

	private int lh_count;
	void Awake()
	{
		_xSize = _zSize = GameSettings.SNAP_VALUE;
		_tiles = transform.GetComponent<DragAndPlace>().Tiles;
		_combiner = GameObject.Find("Combiner").GetComponent<Combiner>();

		// raycast positions (valid tiles for the combo) from "main" tile (transform.position), where "1" is 8 units
		xPos = new[] { 1, 1, -1, -1, -1, -1, 1, 1 };
		zPos = new[] { 0, 1, 0, 1, 0, 1, 1, 0 };

		// Instantiated building position and rotation adjustment
		xAdj = new[] { 0, 0, 0, 0, 0, 0, 0, 0 };
		zAdj = new[] { 0, 1, 0, 1, 0, 0, 0, 0 };
		rotAdj = new[] { 0, 90, -90, -180, 0, 0, 0, 0 };
	}

	void CheckForCombos(int I)
	{
		if (I < 0)
		{
			_trashCan.Clear();
			_garbageBin = new List<GameObject>[xPos.Length];
			for (var j = 0; j < xPos.Length; j++) _garbageBin[j] = new List<GameObject>();

			lh_count = 0;

			for (var i = 0; i < xPos.Length; i++)
			{
				RaycastHit hit;
				if (Physics.Raycast(transform.position + transform.right * _xSize * xPos[i] + new Vector3(0, 100, 0) + transform.forward * _zSize * zPos[i], Vector3.down, out hit, 100, _tiles))
				{
					if (hit.collider.tag != "Tile" || !hit.transform.GetComponent<DragAndPlace>().Dropped) continue;

					if (hit.transform.name.StartsWith("House") && i < 4)
					{
						_trashCan.Add(hit.transform.gameObject);
						result = Villa;
					}
					else if (hit.transform.name.StartsWith("Luxury house") && i >= 4 && i <= 7)
					{
						lh_count++;
						_trashCan.Add(hit.transform.gameObject);
						if (i == 7 && lh_count == 4)
						{
							result = Mansion;
						}
						else continue;
					}
					else continue;

					if (gameObject != _combiner.LastPlacedTile && !_trashCan.Contains(_combiner.LastPlacedTile)) continue;
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