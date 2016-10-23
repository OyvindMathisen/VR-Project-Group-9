using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AreaCheck : MonoBehaviour {

	private CheckCollision[] checkCollisions = new CheckCollision[8];
	private GameObject[] buildings = new GameObject[8];
	private string[] bNames = new string[8];
    public LayerMask tiles;

    public GameObject Preview;
    public GameObject Villa;

    private bool onceNotHolding;
    public int previewCount;

	private Vector3 curPosition;
	private Wand Rhand;

    private const float offsetY = -4;


    void Awake()
	{
		Rhand = GameObject.Find("[CameraRig]").transform.FindChild("Controller (right)").GetComponent<Wand>();
		root.currentDrag = Rhand.transform; // Setting currentDrag to a value to prevent accessing a blank variable.
	}

	void Update ()
	{
		// TODO: Tile is placed one off in the opposite Z direction of the side you grab the tile. This needs to be fixed.

		if (root.isHolding && root.currentDrag != null)
			curPosition = root.currentDrag.position + root.distToPP;
		else
			curPosition = Rhand.transform.position;

	    float currentX = Mathf.Round(curPosition.x * root.SNAP_INVERSE) / root.SNAP_INVERSE;
		float currentZ = Mathf.Round(curPosition.z * root.SNAP_INVERSE) / root.SNAP_INVERSE;

		transform.position = new Vector3(currentX, root.BUILD_HEIGHT-offsetY, currentZ);
	}

	/*
	* @Parameter The building (cg, current gameobject) you're scanning for it's children to find out the position of the preview meshes.
	*/
    public void NewPreviewArea(GameObject cg)
    {
		DeletePreviews();
		foreach (Transform child in cg.transform)
		{
			var newPreview = Instantiate(Preview, new Vector3(child.transform.position.x, root.BUILD_HEIGHT, child.transform.position.z), Quaternion.identity) as GameObject;
			newPreview.transform.parent = GameObject.Find("PreviewPlacement").transform.FindChild("Wrap");
			newPreview.transform.localScale = Vector3.one;
		}

		/*
        const int y = 200;
        int x = -16, z = -16;

        for (var i = 0; i < 25; i++)
        {
			// checking for buildings held from above
            RaycastHit hit;
			Debug.Log("v");
			if (Physics.Raycast(transform.position + new Vector3(x, y, z), Vector3.down, out hit, Mathf.Infinity, tiles))
            {
				// if the object is a tile
				Debug.Log("0v");
				if (hit.collider.tag == "Tile")
                {
					// and if the object held is not placed yet
					Debug.Log("1v");
					if (!hit.transform.GetComponent<DragAndPlace>().placed)
                    {
						Debug.Log("2v");
						var newPreview = Instantiate(Preview, transform.position + new Vector3(x, 0, z), Quaternion.identity) as GameObject;
                        newPreview.transform.parent = GameObject.Find("PreviewPlacement").transform.FindChild("Wrap");
                        newPreview.transform.localScale = Vector3.one;
                        previewCount++;
                    }
                }
            }

            x += 8;
            if (x != 16) continue;
            z += 8;
            x = -16;
			
        }

        onceNotHolding = false;
		*/
	}

	public void DeletePreviews()
    {
        foreach (Transform child in transform.FindChild("Wrap"))
        {
            if (child.gameObject.name.StartsWith("Preview"))
                Destroy(child.gameObject);
        }
        previewCount = 0;
    }

    public bool IsAreaFree()
    {
        foreach (Transform child in transform)
        {
            if (child.gameObject.name.StartsWith("Preview"))
            {
                RaycastHit hit;
                if (Physics.Raycast(child.position + new Vector3(0, -20, 0), Vector3.up, out hit, Mathf.Infinity, tiles))
                {
                    if (hit.collider.tag == "Tile")
                    {
                        if (hit.transform.GetComponent<DragAndPlace>().placed)
                        {
                            return false;
                        }
                    }
                }
            }
        }
        return true;
    }

	/*public void CheckForValidCombo(GameObject obj)
	{
		//var temp[] = new GameObject[8];
		//temp[1].transform

		for (var i = 0; i < 8; i++)
		{
		    if (checkCollisions[i].getTile() != null)
		    {
		        buildings[i] = checkCollisions[i].getTile();

		        var temp = buildings[i].name.Split('(');
		        bNames[i] = temp[0];
		    }
		}

		var objTemp = obj.name.Split('(');
		var objName = objTemp[0];

	    switch (objName)
	    {
	        case "House":
	            if (bNames[0] == "LuxuryHouse" && bNames[1] == "LuxuryHouse")
	            {
	                var tile = Instantiate(Villa, transform.position + Vector3.left * 8, Quaternion.Euler(0, 180, 0));
	                Destroy(buildings[0].gameObject);
	                Destroy(obj);
	            }
	            else if (bNames[1] == "LuxuryHouse" && bNames[2] == "LuxuryHouse")
	            {
	                var tile = Instantiate(Villa, transform.position + Vector3.forward * 8, Quaternion.Euler(0, 0, 0));
	                Destroy(buildings[1].gameObject);
	                Destroy(obj);
	            }
	            else if (bNames[2] == "LuxuryHouse" && bNames[3] == "LuxuryHouse")
	            {
	                var tile = Instantiate(Villa, transform.position + Vector3.forward * 8, Quaternion.Euler(0, 270, 0));
	                Destroy(buildings[2].gameObject);
	                Destroy(obj);
	            }
	            else if (bNames[3] == "LuxuryHouse" && bNames[4] == "LuxuryHouse")
	            {
	                var tile = Instantiate(Villa, transform.position + Vector3.right * 8, Quaternion.Euler(0, 90, 0));
	                Destroy(buildings[3].gameObject);
	                Destroy(obj);
	            }
	            else if (bNames[4] == "LuxuryHouse" && bNames[5] == "LuxuryHouse")
	            {
	                var tile = Instantiate(Villa, transform.position + Vector3.right * 8, Quaternion.Euler(0, 0, 0));
	                Destroy(buildings[4].gameObject);
	                Destroy(obj);
	            }
	            else if (bNames[5] == "LuxuryHouse" && bNames[6] == "LuxuryHouse")
	            {
	                var tile = Instantiate(Villa, transform.position + Vector3.back * 8, Quaternion.Euler(0, 180, 0));
	                Destroy(buildings[5].gameObject);
	                Destroy(obj);
	            }
	            else if (bNames[6] == "LuxuryHouse" && bNames[7] == "LuxuryHouse")
	            {
	                var tile = Instantiate(Villa, transform.position + Vector3.back * 8, Quaternion.Euler(0, 90, 0));
	                Destroy(buildings[6].gameObject);
	                Destroy(obj);
	            }
	            else if (bNames[7] == "LuxuryHouse" && bNames[0] == "LuxuryHouse")
	            {
	                var tile = Instantiate(Villa, transform.position + Vector3.left * 8, Quaternion.Euler(0, 270, 0));
	                Destroy(buildings[7].gameObject);
	                Destroy(obj);
	            }
	            break;
	        case "LuxuryHouse":

	            break;
	    }

		for (var j = 0; j < bNames.Length; j++) {
			buildings[j] = null;
			bNames[j] = "";
		}

    }*/
}
