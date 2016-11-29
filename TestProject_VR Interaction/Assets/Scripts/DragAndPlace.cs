using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(BuildingKeepRotation))]
public class DragAndPlace : MoveObject
{
	public bool Dropped; // if the tile is still "dragged" around (the mouse button is not released yet)
	public LayerMask Tiles, VegetationLayer, TilesInHand;
	public float BuildingFallSpeed = 1.5f;
	public bool ReachedHeight;
    public bool keepFalling;

    public bool IsMarket;

    private GameObject _previewPlacement, _previewPlacementL, _previewPlacementR;
	private bool _hasRotated, _oncePlaced, _onceNotPlaced, _placedWrong, _hasEverBeenPlaced;
	private Vector3 _lastSafePos, _distToHand; // The distance between Rhand and this building.
	private Quaternion _lastSafeRot;
	private Combiner _combiner;
    private const float FALL_DISTANCE = 1000;

	private AreaCheck _areaCheck;

    private bool _isRightHand;

	private BuildingKeepRotation _buildingRotation;

	private List<GameObject> _connectedColliders = new List<GameObject>();
	private List<GameObject> _markedColliders = new List<GameObject>();

	void Awake()
	{
		_previewPlacementL = GameObject.FindWithTag("PreviewPlacementLeft");
        _previewPlacementR = GameObject.FindWithTag("PreviewPlacementRight");
        _combiner = GameObject.Find("Combiner").GetComponent<Combiner>();
		transform.parent = GameObject.Find("Tiles").transform;

		_lastSafePos = Vector3.zero;

        // TODO: (temporary) for without headset development
	    _areaCheck = GameObject.Find("PreviewPlacementRight").GetComponent<AreaCheck>();

		// if placed is true to begin with, it's probably a combined building
		if (!Dropped) return;
		ReachedHeight = true;
		_oncePlaced = true;
	}

	protected void Start()
	{
		//base.Start ();
		_buildingRotation = GetComponent<BuildingKeepRotation>();
        _buildingRotation.yRotation = transform.eulerAngles.y; // Ensures the yRot is correct if a combo is made.

        if (!Dropped)
		{
			_areaCheck.NewPreviewArea(gameObject);
			_areaCheck.DistanceToPreviewPlacement = Holder.AreaCheck.transform.position - transform.position;
		}
	}

	void Update()
	{
		// If the building has yet to be placed.
		if (!Dropped)
		{
			Holder.AreaCheck.HeldObject = transform;

			if (!_onceNotPlaced)
			{
				if (_combiner.Alternatives.Count > 0)
				{
					_combiner.Cancel();
					_combiner.PlayCancelSound();
				}

				_oncePlaced = false;
				ReachedHeight = false;
			    keepFalling = false;

				_onceNotPlaced = true;
			}
        }
		// If the building has been released.
		else
		{
            if (!_oncePlaced)
			{
                Place();
                _oncePlaced = true;
            }

            if (keepFalling)
            {
                transform.Translate(0, -BuildingFallSpeed, 0);

                if (transform.position.y < -FALL_DISTANCE) Destroy(gameObject);
                return;
            }

            // while the building is not at specified height
            if (!transform.position.y.Equals(GameSettings.BUILD_HEIGHT))
			{
				transform.Translate(0, -BuildingFallSpeed, 0);

				// If the building is below the intended height.
				if (transform.position.y < GameSettings.BUILD_HEIGHT + 1f)
				{
					var newPosition = transform.position;
					newPosition.y = GameSettings.BUILD_HEIGHT;
					transform.position = newPosition;
				}
			}
			// Preformed when a building lands on the intended height.
			if (!(transform.position.y > GameSettings.BUILD_HEIGHT - 4f) || !(transform.position.y < GameSettings.BUILD_HEIGHT + 4f) || ReachedHeight) return;

			var sfx = _previewPlacement.transform.FindChild("sfxPlace1").GetComponent<AudioSource>();
			sfx.pitch = Random.Range(0.9f, 1.1f);
			sfx.Play();

			// particle effect for every tile in placed building
			foreach (Transform child in transform)
			{
				var fx = Instantiate(Resources.Load("FogExplosion", typeof(GameObject)), new Vector3(child.transform.position.x, GameSettings.BUILD_HEIGHT, child.transform.position.z), Quaternion.identity) as GameObject;
				// Check next object if FX is null.
				if (fx == null) continue;
				foreach (var ps in fx.GetComponentsInChildren<ParticleSystem>())
					ps.Play();
			}

			GameSettings.NewestLandedPosition = gameObject.transform.position;
			_combiner.LastPlacedTile = gameObject;

            // cancel combination process
			if (_combiner.Alternatives.Count > 0)
			{
				_combiner.onceNewAlts = false;
				_combiner.PlayCancelSound();
			}

			CheckConnectedTilesForCombo();
			ReachedHeight = true;
		}
	}

	void Place()
	{
        // hide vegetation at the building area
		foreach (Transform tile in _areaCheck.FeaturedVegTiles)
		{
			var script = tile.transform.GetComponent<Vegetation>();
			if (!script) continue;
			script.Hide();
		}

		// Delete old previews lingering on the map
		_areaCheck.DeletePreviews();

		// to prevent raycasting self when checking for free area
		foreach (var c in GetComponentsInChildren<Collider>())
		{
            c.gameObject.layer = LayerMask.NameToLayer("Tiles");
        }

		if (_placedWrong)
		{
			ReachedHeight = true;
			_placedWrong = false;
		}
		else
		{
			// set building position
			var snappedPosition = transform.position;
			snappedPosition.x = Mathf.Round(snappedPosition.x * GameSettings.SNAP_INVERSE) / GameSettings.SNAP_INVERSE;
			snappedPosition.z = Mathf.Round(snappedPosition.z * GameSettings.SNAP_INVERSE) / GameSettings.SNAP_INVERSE;
			snappedPosition.y = transform.position.y;
			transform.position = snappedPosition;
		}

		_hasEverBeenPlaced = true;
		_onceNotPlaced = false;
	}

	public void FinalRotation()
	{
		_buildingRotation.FinalizeRotation();
	}

	public void Rotate(DirectionLR direction)
	{
		// Get the sound from the _previewPlacement object.
		var sfx = _previewPlacement.transform.FindChild("sfxRotate").GetComponent<AudioSource>();

		_buildingRotation.RotateBuilding(direction);
		//_hasRotated = true;

		// Set sound to play based on direction rotated.
		if (direction == DirectionLR.Right)
		{
			_previewPlacement.transform.FindChild("Wrap").Rotate(0, 90, 0);
			sfx.pitch = 1.1f;
		}
		else
		{
			_previewPlacement.transform.FindChild("Wrap").Rotate(0, -90, 0);
			sfx.pitch = 0.9f;
		}
		// Plays the sound
		sfx.Play();
	}


	public override void GrabMe(Wand controller)
	{
        foreach (var c in GetComponentsInChildren<Collider>())
        {
            c.gameObject.layer = LayerMask.NameToLayer("TilesInHand");
        }
        base.GrabMe(controller);
		OnGrab();
	}

	public override bool DropMe(Wand controller)
	{
		if (controller != Holder) return false;
		Dropped = true;

		HandleArea();

		return base.DropMe(controller);
	}

	public void OnGrab()
	{
		_areaCheck = Holder.AreaCheck;

        // To keep track of what hand is holding the building.
        _isRightHand = (_areaCheck.transform.name == "PreviewPlacementRight");
        _previewPlacement = _isRightHand ? _previewPlacementR : _previewPlacementL;

        Holder.AreaCheck.NewPreviewArea(gameObject);
		Holder.AreaCheck.DistanceToPreviewPlacement = Holder.AreaCheck.transform.position - transform.position;

		// start fade-in on vegetation where the building has been
		foreach (Transform child in transform)
		{
			if (!child.name.StartsWith("Collider")) continue;
			RaycastHit hit;
			if (!Physics.Raycast(child.position + Vector3.down * 20, Vector3.up, out hit, 30, VegetationLayer)) continue;

			var script = hit.transform.GetComponent<Vegetation>();
			if (!script) continue;
			script.Show();
		}

	    var sfx = _previewPlacement.transform.FindChild("sfxPickup").GetComponent<AudioSource>();
        sfx.pitch = Random.Range(0.9f, 1.1f);
        sfx.Play();

        _lastSafePos = transform.position;
		_lastSafeRot = transform.rotation;

		Dropped = false;
	}

	void CheckConnectedTilesForCombo()
	{
		_connectedColliders.Clear();
		_markedColliders.Clear();

		// If no child is found, it's an older building. No need to raycast.
		if (transform.childCount == 0)
			return;

		RaycastForConnectedTiles(transform.FindChild("Collider1").gameObject);

		var connectedBuildings = new List<GameObject>();

		foreach (var cc in _connectedColliders)
		{
			if (!connectedBuildings.Contains(cc.transform.parent.gameObject))
				connectedBuildings.Add(cc.transform.parent.gameObject);
		}

		foreach (var tile in connectedBuildings)
		{
			tile.transform.SendMessage("CheckForCombos", -2, SendMessageOptions.DontRequireReceiver);
		}
	}

	void RaycastForConnectedTiles(GameObject src)
	{
		var gameObjects = new List<GameObject>();
		float xSize;
		var zSize = xSize = GameSettings.SNAP_VALUE;

		int[] xPos = { 1, 0, -1, 0 };
		int[] zPos = { 0, 1, 0, -1 };

		for (var i = 0; i < 4; i++)
		{
			RaycastHit hit;
			if (Physics.Raycast(src.transform.position + src.transform.right * xSize * xPos[i] + new Vector3(0, 100, 0) + src.transform.forward * zSize * zPos[i], Vector3.down, out hit, 100, Tiles))
			{
				if (hit.collider.tag != "Tile") continue;
				var script = hit.transform.GetComponent<DragAndPlace>();
				if (!script) continue; // If script is null.
				if (!script.Dropped) continue;
				if (_markedColliders.Contains(hit.collider.gameObject)) continue;

				gameObjects.Add(hit.collider.gameObject);
				_markedColliders.Add(hit.collider.gameObject);
			}
		}

		foreach (var obj in gameObjects)
			RaycastForConnectedTiles(obj);

		_connectedColliders.AddRange(gameObjects);
	}

	void HandleArea(bool skipCheckArea = false)
	{
	    if (!skipCheckArea)
	    {
	        switch (_areaCheck.GetAreaStatus())
	        {
                case "Available":
                    if (_areaCheck.IsAreaFree()) return;
	                break;
                case "Nothing":
	                keepFalling = true;
	                return;
                case "Occupied":
                case "Overlap":
                    // proceed
                    break;
            }
	    }

	    if (_hasEverBeenPlaced)
		{
			transform.rotation = _lastSafeRot;
			transform.position = _lastSafePos;
		}
		else
		{
			_areaCheck.DeletePreviews();
			Destroy(gameObject);
		}
		_placedWrong = true;
		_previewPlacement.transform.FindChild("sfxError").GetComponent<AudioSource>().Play();
	}
}
