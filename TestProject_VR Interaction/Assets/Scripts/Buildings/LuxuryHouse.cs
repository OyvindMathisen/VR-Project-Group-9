using UnityEngine;
using System.Collections;

public class LuxuryHouse : ComboParent {

    public GameObject Villa;


	private AreaCheck _areaCheck;

	protected override void Awake ()
	{
        base.Awake();
		_tiles = transform.GetComponent<DragAndPlace>().Tiles;
        xPos = new[] { 1, 1, -1, -1 };
        zPos = new [] { 0, 1, 0, 1 };

        xAdj = new[] { 0, 0, 0, 0 }; // <- (atm this is useless, but probably need for other kinds of combos)
        zAdj = new[] { 0, 1, 0, 1 };
        rotAdj = new[] { 0, 90, -90, -180 };
    }
}