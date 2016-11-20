using UnityEngine;
using System.Collections.Generic;

public class AreaCheck : MonoBehaviour
{
	public GameObject OurHand, Preview;
	public int PreviewCount; // not used atm
	public Vector3 DistanceToPreviewPlacement;
	public Transform HeldObject;

	private Vector3 _curPosition;
	private const float OffsetY = -4;
	private Wand _controller;
	private Transform _wrap;

	private float _currentX, _currentZ;
	private int _oldCurrentX, _oldCurrentZ;

	public LayerMask VegetationLayer;
	public LayerMask Tiles;

	public List<Transform> FeaturedVegTiles = new List<Transform>();

	void Awake()
	{
		_wrap = transform.FindChild("Wrap");
		DistanceToPreviewPlacement = Vector3.zero;
	}

	void Start()
	{
		_controller = OurHand.GetComponent<Wand>();
		_controller.AreaCheck = this;
	}

	void Update()
	{
		if (_controller.IsHolding && HeldObject != null)
			_curPosition = HeldObject.position + DistanceToPreviewPlacement;
		else
			_curPosition = OurHand.transform.position;

		_currentX = Mathf.Round(_curPosition.x * GameSettings.SNAP_INVERSE) / GameSettings.SNAP_INVERSE;
		_currentZ = Mathf.Round(_curPosition.z * GameSettings.SNAP_INVERSE) / GameSettings.SNAP_INVERSE;

		transform.position = new Vector3(_currentX, GameSettings.BUILD_HEIGHT - OffsetY, _currentZ);
	}

	void LateUpdate()
	{
		// when moving the preview tiles to another area on the grid
		if ((int)_currentX != _oldCurrentX || (int)_currentZ != _oldCurrentZ)
		{
			// list up vegetation tiles at the current preview tiles positions
			FeaturedVegTiles.Clear();
			foreach (Transform child in _wrap)
			{
				if (!child.gameObject.name.StartsWith("Preview")) continue;
				RaycastHit hit;
				if (Physics.Raycast(child.position + Vector3.down * 80, Vector3.up, out hit, 120, VegetationLayer))
				{
					FeaturedVegTiles.Add(hit.transform);
				}
			}
			_oldCurrentX = (int)_currentX;
			_oldCurrentZ = (int)_currentZ;
		}
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
			newPreview.transform.position = new Vector3(child.transform.position.x, GameSettings.BUILD_HEIGHT +
														GameSettings.PREVIEW_HEIGHT_ADJUST, child.transform.position.z);
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
					hit.transform.GetComponent<DragAndPlace>().Dropped)
					return false;
			}
		}
		return true;
	}
}
