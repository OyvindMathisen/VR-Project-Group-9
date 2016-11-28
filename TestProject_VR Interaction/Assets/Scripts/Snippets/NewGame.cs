using UnityEngine;
using System.Collections;

public class NewGame : MonoBehaviour
{
    public LayerMask VegetationLayer;
    public GameObject HugeFogPoof;
    public Material Water;

    private DragAndPlace _buildingScript;

	void Awake ()
	{
	    _buildingScript = GetComponent<DragAndPlace>();
	}

	void Update () {
	    if (_buildingScript.ReachedHeight)
	    {
            // handle island reset
            var buildings = GameObject.FindGameObjectsWithTag("Building");
	        foreach (var b in buildings)
	        {
                foreach (Transform child in b.transform)
                {
                    if (!child.gameObject.name.StartsWith("Collider")) continue;
                    RaycastHit hit;
                    if (Physics.Raycast(child.position + Vector3.down * 80, Vector3.up, out hit, 120, VegetationLayer))
                    {
                        var vegScript = hit.transform.GetComponent<Vegetation>();
                        if (!vegScript) continue;
                        vegScript.Show();
                    }
                }
                Destroy(b);
	        }

            // various resets
            var water = GameObject.Find("Island").transform.FindChild("Water");

            water.FindChild("Congratulations").GetComponent<TextMesh>().text = "";
            GameObject.Find("MainGameObjects").transform.FindChild("ComboScreen/ComboCount/Count").GetComponent<TextMesh>().text = "0/" + GameSettings.TOTAL_COMBO_COUNT;

            water.GetComponent<MeshRenderer>().material = Water;
            var waterAnims = water.parent.FindChild("WaterAnimation").GetComponentsInChildren<SpriteRenderer>();
            foreach (var sr in waterAnims) sr.enabled = false;

            // huge poof effect
            Instantiate(HugeFogPoof);

            // delete old file then start new file
            SaveAndLoad.Delete();
            GameFile.current = new GameFile();
            SaveAndLoad.Save();

            Destroy(gameObject);
        }
	}
}
