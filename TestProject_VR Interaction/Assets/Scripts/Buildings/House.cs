using UnityEngine;
using System.Collections;
using System;

public class House : ComboParent {
    protected override void Awake()
    {
        base.Awake();
        _tiles = transform.GetComponent<DragAndPlace>().Tiles;

        //TODO These are the values from LuxuryHouse and has to be set correctly for the default house
        xPos = new[] { 1, 1, -1, -1 };
        zPos = new[] { 0, 1, 0, 1 };

        xAdj = new[] { 0, 0, 0, 0 }; // <- (atm this is useless, but probably need for other kinds of combos)
        zAdj = new[] { 0, 1, 0, 1 };
        rotAdj = new[] { 0, 90, -90, -180 };
    }
    protected override GameObject MakeCombo(string buildingName)
    {
        return null;
    }
}
