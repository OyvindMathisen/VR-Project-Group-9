using System;
using UnityEngine;

public class DragAndPlace : MonoBehaviour
{
    public bool Placed; // if the tile is still "dragged" around (the mouse button is not released yet)
    public LayerMask Tiles;
    public float BuildingFallSpeed = 1.5f;

    private GameObject _previewPlacement;
    private GameObject _rightController;
    private Wand _controller;
    private bool _hasRotated, _oncePlaced, _reachedHeight;
    private Vector3 _lastSafePos, _distToHand; // The distance between Rhand and this building.
    private AreaCheck _areaCheck;
    private Quaternion _lastSafeRot;

    protected virtual void Awake()
	{
        _rightController = GameObject.FindWithTag("Rhand");
        _previewPlacement = GameObject.FindWithTag("PreviewPlacement");
		_controller = _rightController.GetComponent<Wand>();
        _areaCheck = _previewPlacement.GetComponent<AreaCheck>();
	    _lastSafePos = Vector3.zero;

        // if placed is true to begin with, it's probably a combined building
        if (!Placed) return;
        _reachedHeight = true;
        _oncePlaced = true;
	}

	void Start()
	{
		_distToHand = transform.position - _controller.transform.position;
		if (!Placed)
		_areaCheck.NewPreviewArea(gameObject);
        _areaCheck.DistanceToPreviewPlacement = _areaCheck.transform.position - transform.position;
    }

	protected virtual void Update()
	{
        // If the building has yet to be placed.
		if (!Placed)
		{
            _areaCheck.HeldObject = transform;

            // Collision check to prevent overlapping buildings
            if (_controller.TriggerButtonUp)
            {
                if (!_areaCheck.IsAreaFree())
                {
                    if (_lastSafePos != Vector3.zero)
                    {
                        transform.position = _lastSafePos;
                        transform.rotation = _lastSafeRot;
                    }
                    else
                    {
                        Destroy(gameObject);
                    }
                    _previewPlacement.transform.FindChild("sfxError").GetComponent<AudioSource>().Play();
                }

                Placed = true;
                _controller.IsHolding = false;
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

            _oncePlaced = false;
            _reachedHeight = false;
        }
        // If the building has been placed.
		else
		{
            if (!_oncePlaced)
            {
                SetBuildingPosition();
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
		    if (!(transform.position.y > GameSettings.BUILD_HEIGHT - 4f) || !(transform.position.y < GameSettings.BUILD_HEIGHT + 4f) || _reachedHeight) return;
		    if (_areaCheck.PreviewCount > 3)
		    {
		        var sfx = _previewPlacement.transform.FindChild("sfxPlace2").GetComponent<AudioSource>();
		        sfx.pitch = UnityEngine.Random.Range(1f, 1.2f);
		        sfx.Play();
		    }
		    else
		    {
		        var sfx = _previewPlacement.transform.FindChild("sfxPlace1").GetComponent<AudioSource>();
		        sfx.pitch = UnityEngine.Random.Range(0.9f, 1.1f);
		        sfx.Play();
		    }
		    // particle effect for every tile in placed building
		    foreach (Transform child in transform)
		    {
		        var fx = Instantiate(Resources.Load("FogExplosion", typeof(GameObject)), new Vector3(child.transform.position.x, GameSettings.BUILD_HEIGHT, child.transform.position.z), Quaternion.identity) as GameObject;
                // Check next object if FX is null.
                if (fx == null) continue;
		        foreach (var ps in fx.GetComponentsInChildren<ParticleSystem>())
		            ps.Play();
		    }
            // Runs the Child script ComboParent.OnPlaced();
            OnPlaced();
		}
    }

    // Placeholder method if child ComboParent has not been made.
    protected virtual void OnPlaced()
    {
		_reachedHeight = true;
    }

    private void SetBuildingPosition()
    {
        // Delete old previews lingering on the map
        _areaCheck.DeletePreviews();

        var snappedPosition = transform.position; // previously areaCheck's position
        snappedPosition.x = Mathf.Round(snappedPosition.x * GameSettings.SNAP_INVERSE) / GameSettings.SNAP_INVERSE;
        snappedPosition.z = Mathf.Round(snappedPosition.z * GameSettings.SNAP_INVERSE) / GameSettings.SNAP_INVERSE;
        snappedPosition.y = transform.position.y;
        transform.position = snappedPosition;

        _oncePlaced = true;
    }

	void OnTriggerStay(Collider other)
	{
	    if (_controller == null || other.tag != "Rhand" || !_controller.TriggerButtonDown || _controller.IsHolding) return;
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
