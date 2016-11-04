using UnityEngine;
using System.Collections;
using System;

public class LuxuryHouse : ComboParent {

    public GameObject resultVilla; // Result of villa combo.
    // public GameObject OtherResult; // Example if you can make several combinations with this building

	protected override void Awake ()
	{
        base.Awake();
		_tiles = transform.GetComponent<DragAndPlace>().Tiles;

        // Positions to check
        xPos = new[] { 1, 1, -1, -1 };
        zPos = new [] { 0, 1, 0, 1 };

        xAdj = new[] { 0, 0, 0, 0 }; // <- (atm this is useless, but probably need for other kinds of combos)
        zAdj = new[] { 0, 1, 0, 1 };
        rotAdj = new[] { 0, 90, -90, -180 };

    }
    protected override GameObject MakeCombo(string buildingName)
    {
        // Needs one if statement per possible combination.
        if (buildingName.StartsWith("House"))
        {
            return resultVilla;
        }
        return null;
    }
}