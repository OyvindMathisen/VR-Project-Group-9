using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class AreaCheck : MonoBehaviour
{
    public GameObject RightHand;
    public LayerMask Tiles;
    public GameObject Preview;
    public int PreviewCount; // not used atm
    public Vector3 DistanceToPreviewPlacement;
    public Transform HeldObject;
	private Vector3 _curPosition;
    private const float OffsetY = -4;
    private Wand _controller;
    private Transform _wrap;

    void Awake ()
    {
        _wrap = transform.FindChild("Wrap");
        DistanceToPreviewPlacement = Vector3.zero;
    }

	void Start ()
	{
		_controller = HMDComponents.getRightWand();
	}

	void Update ()
	{
        if (_controller.IsHolding && HeldObject != null)
            _curPosition = HeldObject.position + DistanceToPreviewPlacement;
        else
            _curPosition = RightHand.transform.position;

	    var currentX = Mathf.Round(_curPosition.x * GameSettings.SNAP_INVERSE) / GameSettings.SNAP_INVERSE;
		var currentZ = Mathf.Round(_curPosition.z * GameSettings.SNAP_INVERSE) / GameSettings.SNAP_INVERSE;

		transform.position = new Vector3(currentX, GameSettings.BUILD_HEIGHT-OffsetY, currentZ);
	}

    public void NewPreviewArea(GameObject currentGameObject)
    {
        var temp = transform.position;
        temp.x = currentGameObject.transform.position.x;
        temp.z = currentGameObject.transform.position.z;
        transform.position = temp;

        _wrap.rotation = Quaternion.identity;

        DeletePreviews();
		foreach (Transform child in currentGameObject.transform)
		{
			if (child.tag != "Tile") continue; // Only do this for objects tagged as locations for the preview area.
			var newPreview = Instantiate(Preview);
            // Check if newPreview is null. If so, skip over this part.
		    if (newPreview == null) continue;
		    newPreview.transform.parent = _wrap;
			newPreview.transform.localScale = Vector3.one;
		    newPreview.transform.position = new Vector3(child.transform.position.x, GameSettings.BUILD_HEIGHT+GameSettings.PREVIEW_HEIGHT_ADJUST, child.transform.position.z);
		    PreviewCount++;
		}
	}

	public void DeletePreviews()
    {
        foreach (Transform child in _wrap)
        {
            if (child.gameObject.name.StartsWith("Preview"))
                Destroy(child.gameObject);
        }
        PreviewCount = 0;
    }

    public bool IsAreaFree()
    {
        foreach (Transform child in _wrap)
        {
            if (child.gameObject.name.StartsWith("Preview"))
            {
                RaycastHit hit;
                if (Physics.Raycast(child.position + new Vector3(0, -100, 0), Vector3.up, out hit, Mathf.Infinity, Tiles) &&
                    hit.collider.tag == "Tile" &&
                    hit.transform.GetComponent<DragAndPlace>().Placed)
                    return false;
            }
        }
        return true;
    }
}
