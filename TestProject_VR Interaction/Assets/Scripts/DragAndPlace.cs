using System;
using UnityEngine;
using System.Collections.Generic;

public class DragAndPlace : MonoBehaviour
{
    public bool Placed; // if the tile is still "dragged" around (the mouse button is not released yet)
    public LayerMask Tiles;
    public float BuildingFallSpeed = 1.5f;
	public bool ReachedHeight;

    private GameObject _previewPlacement;
    private Wand _controller;
    private bool _hasRotated, _oncePlaced, _onceNotPlaced, _placedWrong;
    private Vector3 _lastSafePos, _distToHand; // The distance between Rhand and this building.
    private AreaCheck _areaCheck;
    private Quaternion _lastSafeRot;
    private Combiner _combiner;

    void Awake()
	{
        _previewPlacement = GameObject.FindWithTag("PreviewPlacement");
        _areaCheck = _previewPlacement.GetComponent<AreaCheck>();
	    _lastSafePos = Vector3.zero;

        _combiner = GameObject.Find("Combiner").GetComponent<Combiner>();

        transform.parent = GameObject.Find("Tiles").transform;

        // if placed is true to begin with, it's probably a combined building
        if (!Placed) return;
		ReachedHeight = true;
        _oncePlaced = true;
	}

	void Start()
	{
		_controller = HMDComponents.getRightWand();

		_distToHand = transform.position - _controller.transform.position;
		if (!Placed)
		_areaCheck.NewPreviewArea(gameObject);
        _areaCheck.DistanceToPreviewPlacement = _areaCheck.transform.position - transform.position;
    }

	void Update()
	{
        // If the building has yet to be placed.
		if (!Placed)
		{
            _areaCheck.HeldObject = transform;

		    if (!_onceNotPlaced)
		    {
                // TODO: improve canceling
                _combiner.Cancel();

                // to prevent raycasting self when checking for free area
                foreach (var c in GetComponentsInChildren<Collider>())
                {
                    c.enabled = false;
                }

                _oncePlaced = false;
				ReachedHeight = false;

                _onceNotPlaced = true;
		    }

            // Collision check to prevent overlapping buildings
            if (_controller.TriggerButtonUp)
            {
                Placed = true;
                _controller.IsHolding = false;

                if (!_areaCheck.IsAreaFree())
                {
                    if (_lastSafePos != Vector3.zero)
                    {
                        transform.rotation = _lastSafeRot;
                        transform.position = _lastSafePos;
                    }
                    else
                    {
                        Destroy(gameObject);
                    }
                    _placedWrong = true;
                    _previewPlacement.transform.FindChild("sfxError").GetComponent<AudioSource>().Play();
                    return;
                }
            }

			// Rotate handler, right direction
			if (_controller.TouchpadRight && !_hasRotated)
			{
                /* // Force rotation around the center of the controller.
                transform.position = Rhand.transform.position;
                areaCheck.transform.position = transform.position;
                distToHand = transform.position - Rhand.transform.position;
                _areaCheck.distanceToPreviewPlacement = _areaCheck.transform.position - transform.position;
                //*/
                var sfx = _previewPlacement.transform.FindChild("sfxRotate").GetComponent<AudioSource>();
                sfx.pitch = 1.1f;
                sfx.Play();

                transform.Rotate(0, 90, 0);
				_hasRotated = true;

				_previewPlacement.transform.FindChild("Wrap").Rotate(0, 90, 0);
			}

			// Rotate handler, left direction
			if (_controller.TouchpadLeft && !_hasRotated)
			{
                /* // Force rotation around the center of the controller.
                transform.position = Rhand.transform.position;
                areaCheck.transform.position = transform.position;
                distToHand = transform.position - Rhand.transform.position;
                _areaCheck.distanceToPreviewPlacement = _areaCheck.transform.position - transform.position;
                //*/
                var sfx = _previewPlacement.transform.FindChild("sfxRotate").GetComponent<AudioSource>();
                sfx.pitch = 0.9f;
                sfx.Play();

                transform.Rotate(0, -90, 0);
				_hasRotated = true;

				_previewPlacement.transform.FindChild("Wrap").Rotate(0, -90, 0);
			}

			// Delete the building in hand
			if (_controller.GripButtonDown)
			{
                var sfx = _previewPlacement.transform.FindChild("sfxDestroy").GetComponent<AudioSource>();
                sfx.pitch = UnityEngine.Random.Range(0.9f, 1.2f);
                sfx.Play();

                _controller.IsHolding = false;
				_areaCheck.DeletePreviews();
				Destroy(gameObject);
			}

			// To prevent the building from rotating at the speed of light
			if (!_controller.TouchpadLeft && !_controller.TouchpadRight)
			{
				_hasRotated = false;
			}

            // Forces the building to follow RightController.
			var curPosition = _controller.transform.position + _distToHand;
			var newPosition = Vector3.zero;
            newPosition.x = Mathf.Lerp(transform.position.x, curPosition.x, 0.15f);
            newPosition.y = Mathf.Lerp(transform.position.y, curPosition.y, 0.15f);
            newPosition.z = Mathf.Lerp(transform.position.z, curPosition.z, 0.15f);
            transform.position = newPosition;
        }
        // If the building has been placed.
		else
		{
            
            if (!_oncePlaced)
            {
                // Delete old previews lingering on the map
                _areaCheck.DeletePreviews();

                foreach (var c in GetComponentsInChildren<Collider>())
                {
                    c.enabled = true;
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

                _onceNotPlaced = false;
                _oncePlaced = true;
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
            sfx.pitch = UnityEngine.Random.Range(0.9f, 1.1f);
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
            CheckConnectedTilesForCombo();
            //transform.parent.BroadcastMessage("CheckForCombos", false);
		    ReachedHeight = true;
		}
    }


    List<GameObject> connectedTiles = new List<GameObject>();
    List<GameObject> markedColliders = new List<GameObject>();
    void CheckConnectedTilesForCombo()
    {
        connectedTiles.Clear();
        markedColliders.Clear();

		// If no child is found, it's an older building. No need to raycast.
		if (transform.childCount == 0)
			return;
		
		RaycastForConnectedTiles(transform.FindChild("Collider1").gameObject);

        List<GameObject> connectedTilesParents = new List<GameObject>();

        foreach (var child in connectedTiles)
        {
            if (!connectedTilesParents.Contains(child.transform.parent.gameObject))
                connectedTilesParents.Add(child.transform.parent.gameObject);
        }

        foreach (var tile in connectedTilesParents)
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
            if (Physics.Raycast(src.transform.position + src.transform.right*xSize*xPos[i] + new Vector3(0, 100, 0) + src.transform.forward*zSize*zPos[i], Vector3.down, out hit, 100, Tiles))
            {
                if (hit.collider.tag != "Tile" && !hit.transform.GetComponent<DragAndPlace>().Placed) continue;
                if (markedColliders.Contains(hit.collider.gameObject)) continue;

                gameObjects.Add(hit.collider.gameObject);
                markedColliders.Add(hit.collider.gameObject);
            }
        }

        foreach (var obj in gameObjects)
            RaycastForConnectedTiles(obj);

        connectedTiles.AddRange(gameObjects);
    }

    void OnTriggerStay(Collider other)
	{
	    if (other.tag != "Rhand" || !_controller.TriggerButtonDown || _controller.IsHolding) return;
        _areaCheck.NewPreviewArea(gameObject);
        _areaCheck.DistanceToPreviewPlacement = _areaCheck.transform.position - transform.position;
        _distToHand = transform.position - _controller.transform.position;

        if (Placed)
	    {
            _lastSafePos = transform.position;
	        _lastSafeRot = transform.rotation;
	    }

	    Placed = false;
	    _controller.IsHolding = true;
	}
}
