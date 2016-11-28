using UnityEngine;
using System.Collections;

[System.Serializable]
public class GameFile
{
    public static GameFile current;
    public Building[] buildings;
    public string[] combosDone;

    public GameFile()
    {
        buildings = new Building[256];
        for (var i = 0; i < buildings.Length; i++)
            buildings[i] = new Building();

        combosDone = new string[64];
        for (var i = 0; i < combosDone.Length; i++)
            combosDone[i] = "";
    }

}