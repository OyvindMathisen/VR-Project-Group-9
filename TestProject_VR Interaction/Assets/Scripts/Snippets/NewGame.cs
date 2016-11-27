using UnityEngine;
using System.Collections;

public class NewGame : MonoBehaviour
{
    public LayerMask VegetationLayer;
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

            // delete old and start new file
            SaveAndLoad.Delete();
            GameFile.current = new GameFile();
            SaveAndLoad.Save();

            Destroy(gameObject);
        }
	}
}
