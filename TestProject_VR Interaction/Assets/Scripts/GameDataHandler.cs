using System;
using UnityEngine;
using System.Collections;

public class GameDataHandler : MonoBehaviour
{
    public LayerMask VegetationLayer;
	void Awake ()
	{
        
	    //if (SaveAndLoad.savedGames.Count > 0)
	    //{
     //       // TODO: load when starting up the scene (remember to show maingameobjects and hide gamemenu?)
     //       //Continue();
     //   }
     //   else
     //   {
            
     //   }
	}

	void Update ()
	{
        // TODO: replace F5 and F6 with actual ways to load and save in VR
        if (Input.GetKeyDown(KeyCode.F5))
	    {
            Debug.Log("Saving!");
	        Save();
	    }
        if (Input.GetKeyDown(KeyCode.F6))
        {
            Debug.Log("Loading!");
            Continue();
        }
    }

    public void Save()
    {
        var buildings = GameObject.FindGameObjectsWithTag("Building");
        for (var i = 0; i < buildings.Length; i++)
        {
            var script = buildings[i].GetComponent<DragAndPlace>();
            if (!script) continue;
            if (script.keepFalling) continue;
            string[] name = buildings[i].name.Split('(');
            Debug.Log(name);
                GameFile.current.buildings[i] = new Building(name[0], buildings[i].transform.position, buildings[i].transform.eulerAngles);
        }
            
        SaveAndLoad.Save();
    }

    public void Continue()
    {
        foreach (var g in SaveAndLoad.savedGames)
        {
            GameFile.current = g;
            break; // until we add possibility for multiple save files
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

            // TODO: test if this actually works
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
    }
}
