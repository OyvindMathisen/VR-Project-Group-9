using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class AreaCheck : MonoBehaviour
{
    public GameObject RightHand;

    public LayerMask Tiles;
    public GameObject Preview;
    public int PreviewCount;
	private Vector3 _curPosition;
    private const float OffsetY = -4;

	void Update ()
	{
		_curPosition = RightHand.transform.position;

	    float currentX = Mathf.Round(_curPosition.x * GameSettings.SNAP_INVERSE) / GameSettings.SNAP_INVERSE;
		float currentZ = Mathf.Round(_curPosition.z * GameSettings.SNAP_INVERSE) / GameSettings.SNAP_INVERSE;

		transform.position = new Vector3(currentX, GameSettings.BUILD_HEIGHT-OffsetY, currentZ);
	}

    public void NewPreviewArea(GameObject currentGameObject)
    {
		DeletePreviews();
		foreach (Transform child in currentGameObject.transform)
		{
			var newPreview = Instantiate(Preview, new Vector3(child.transform.position.x, GameSettings.BUILD_HEIGHT, child.transform.position.z), Quaternion.identity) as GameObject;
            // Check if newPreview is null. If so, skip over this part.
		    if (newPreview == null) continue;
			newPreview.transform.parent = GameObject.Find("PreviewPlacement").transform.FindChild("Wrap");
			newPreview.transform.localScale = Vector3.one;
		}
	}

	public void DeletePreviews()
    {
        foreach (Transform child in transform.FindChild("Wrap"))
        {
            if (child.gameObject.name.StartsWith("Preview"))
                Destroy(child.gameObject);
        }
        PreviewCount = 0;
    }

    public bool IsAreaFree()
    {
        foreach (Transform child in transform.Cast<Transform>().Where(child => child.gameObject.name.StartsWith("Preview")))
        {
            RaycastHit hit;
            if (!Physics.Raycast(child.position + new Vector3(0, -20, 0), Vector3.up, out hit, Mathf.Infinity, Tiles)) continue;
            if (hit.collider.tag != "Tile") continue;
            if (hit.transform.GetComponent<DragAndPlace>().Placed)
                return false;
        }
        return true;
    }

    /* // Old, alternative solution.
    public bool IsAreaFree()
    {
        foreach (Transform child in transform)
        {
            if (child.gameObject.name.StartsWith("Preview"))
            {
                RaycastHit hit;
                if (Physics.Raycast(child.position + new Vector3(0, -20, 0), Vector3.up, out hit, Mathf.Infinity, Tiles))
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
    */
}
