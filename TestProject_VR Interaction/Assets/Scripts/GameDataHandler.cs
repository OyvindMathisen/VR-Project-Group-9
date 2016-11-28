using System;
using UnityEngine;
using System.Collections;
using System.Linq;

public class GameDataHandler : MonoBehaviour
{
    public static LayerMask VegetationLayer;
    public LayerMask VegetationLayerNonStatic;

    void Awake()
    {
        VegetationLayer = VegetationLayerNonStatic;
    }

    public static void Save()
    {
        var buildings = GameObject.FindGameObjectsWithTag("Building");
        for (var i = 0; i < buildings.Length; i++)
        {        
            var script = buildings[i].GetComponent<DragAndPlace>();
            if (!script) continue;
            if (script.keepFalling) continue;
            string[] name = buildings[i].name.Split('(');
            GameFile.current.buildings[i] = new Building(name[0], buildings[i].transform.position, buildings[i].transform.eulerAngles);
        }
            
        SaveAndLoad.Save();
    }

    public static void Continue()
    {
        foreach (var g in SaveAndLoad.savedGames)
        {
            GameFile.current = g;
            break; // until we add possibility for multiple save files (most likely not)
        }

        foreach (var building in GameFile.current.buildings)
        {
            if (building.name == "") break;

            // instantiate all the buildings from saved file
            string prefabPath = "Buildings/" + building.name;
            var b = Instantiate(Resources.Load(prefabPath, typeof(GameObject)), new Vector3(building.posX, GameSettings.BUILD_HEIGHT, building.posZ), Quaternion.Euler(building.rotX, building.rotY, building.rotZ)) as GameObject;

            var script = b.GetComponent<DragAndPlace>();
            if (!script) continue; // If script is null.
            script.Dropped = true;
            script.ReachedHeight = true;

            foreach (Transform child in b.transform)
            {
                if (!child.gameObject.name.StartsWith("Collider")) continue;
                RaycastHit hit;
                if (Physics.Raycast(child.position + Vector3.down * 80, Vector3.up, out hit, 120, VegetationLayer))
                {
                    var vegScript = hit.transform.GetComponent<Vegetation>();
                    if (!vegScript) continue;
                    vegScript.Hide();
                }
            }
        }

        // load in which combos have already been done
        var comboTracker = GameObject.Find("Combiner").GetComponent<ComboTracker>();
        foreach (var text in GameFile.current.combosDone)
            if (text.Length > 0)
                comboTracker.CombosDone.Add(text);


        comboTracker.UpdateCount();
    }
}
