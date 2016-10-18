using System;
using UnityEngine;

public class DragAndPlace : MonoBehaviour
{
	public bool placed; // if the tile is still "dragged" around (the mouse button is not released yet)
    private bool oncePlaced, onceNotPlaced, reachedHeight;
	private Wand Rhand;
	private bool hasRotated;
	public LayerMask tiles;

	private GameObject previewPlacement;

    private AreaCheck areaCheck;

    private Vector3 lastSafePos;
    private Quaternion lastSafeRot;

	void Awake()
	{
		Rhand = GameObject.Find("[CameraRig]").transform.FindChild("Controller (right)").GetComponent<Wand>();

		previewPlacement = GameObject.Find("PreviewPlacement");

        areaCheck = previewPlacement.GetComponent<AreaCheck>();

	    lastSafePos = Vector3.zero;

        // if placed is true to begin with, it's probably a combined building
	    if (placed)
	    {
	        reachedHeight = true;
	        oncePlaced = true;
	    }
	}

	void Start()
	{
		areaCheck.NewPreviewArea(gameObject);
	}

	void Update()
	{
		if (!placed)
		{
            if (!onceNotPlaced)
            {
                // run once when still in air
                root.currentDrag = transform;
                onceNotPlaced = true;
            }

            // Collision check to prevent overlapping buildings
            if (Rhand.triggerButtonUp)
			{
                if (!areaCheck.IsAreaFree())
			    {
			        if (lastSafePos != Vector3.zero)
			        {
                        transform.position = lastSafePos;
                        transform.rotation = lastSafeRot;
                    }
			        else
			        {
                        Destroy(gameObject);
                    }
                    previewPlacement.transform.FindChild("sfxError").GetComponent<AudioSource>().Play();
                }

				//transform.position = Rhand.transform.position;

				placed = true;
				root.isHolding = false;
			}

			// Rotate handler, right direction
			if (Rhand.touchpadRight && !hasRotated)
			{
                var sfx = previewPlacement.transform.FindChild("sfxRotate").GetComponent<AudioSource>();
                sfx.pitch = 1.1f;
                sfx.Play();

                transform.Rotate(0, -90, 0);
				hasRotated = true;

				previewPlacement.transform.FindChild("Wrap").Rotate(0, -90, 0);
				//areaCheck.DeletePreviews();
			}

			// Rotate handler, left direction
			if (Rhand.touchpadLeft && !hasRotated)
			{
                var sfx = previewPlacement.transform.FindChild("sfxRotate").GetComponent<AudioSource>();
                sfx.pitch = 0.9f;
                sfx.Play();

                transform.Rotate(0, 90, 0);
				hasRotated = true;

				previewPlacement.transform.FindChild("Wrap").Rotate(0, 90, 0);
				//areaCheck.DeletePreviews();
				//areaCheck.NewPreviewArea();
			}

			// Delete the building in hand
			if (Rhand.gripButtonDown)
			{
                var sfx = previewPlacement.transform.FindChild("sfxDestroy").GetComponent<AudioSource>();
                sfx.pitch = UnityEngine.Random.Range(0.9f, 1.2f);
                sfx.Play();

                root.isHolding = false;
				areaCheck.DeletePreviews();
				Destroy(gameObject);
			}

			// To prevent the building from rotating at the speed of light
			if (!Rhand.touchpadLeft && !Rhand.touchpadRight)
			{
				hasRotated = false;
			}

            // This follows Rhand 
            Vector3 curPosition = Rhand.transform.position;
            var temp = transform.position;
            temp.x = Mathf.Lerp(transform.position.x, curPosition.x, 0.15f);
            temp.y = Mathf.Lerp(transform.position.y, curPosition.y, 0.15f);
            temp.z = Mathf.Lerp(transform.position.z, curPosition.z, 0.15f);
            transform.position = temp;

            oncePlaced = false;
            reachedHeight = false;
        }
		else
		{
            if (transform.position.y != root.BUILD_HEIGHT)
            {
                transform.Translate(0, -0.8f, 0);
                if (transform.position.y < root.BUILD_HEIGHT + 1f)
                {
                    var temp = transform.position;
                    temp.y = root.BUILD_HEIGHT;
                    transform.position = temp;
                }
            }
            // When a building lands
            if (transform.position.y > root.BUILD_HEIGHT - 4f && transform.position.y < root.BUILD_HEIGHT + 4f && !reachedHeight)
            {
                if (areaCheck.previewCount > 3)
                {
                    var sfx = previewPlacement.transform.FindChild("sfxPlace2").GetComponent<AudioSource>();
                    sfx.pitch = UnityEngine.Random.Range(1f, 1.2f);
                    sfx.Play();
                }
                else
                {
                    var sfx = previewPlacement.transform.FindChild("sfxPlace1").GetComponent<AudioSource>();
                    sfx.pitch = UnityEngine.Random.Range(0.9f, 1.1f);
                    sfx.Play();
                }
                // particle effect for every tile in placed building
                foreach (Transform child in transform)
                {
                    var fx = Instantiate(Resources.Load("FogExplosion", typeof(GameObject)), new Vector3(child.transform.position.x, root.BUILD_HEIGHT, child.transform.position.z), Quaternion.identity) as GameObject;
                    foreach (var ps in fx.GetComponentsInChildren<ParticleSystem>())
                        ps.Play();
                }
                reachedHeight = true;
            }
            if (!oncePlaced)
            {
                // run once when placed
                var temp = areaCheck.transform.position; // transform.position
                temp.x = Mathf.Round(temp.x * root.SNAP_INVERSE) / root.SNAP_INVERSE;
                temp.z = Mathf.Round(temp.z * root.SNAP_INVERSE) / root.SNAP_INVERSE;
                transform.position = temp;

                areaCheck.DeletePreviews();
                oncePlaced = true;
            }

            onceNotPlaced = false;
        }
    }

	void OnTriggerStay(Collider other)
	{
		if (other.tag == "Rhand" && Rhand.triggerButtonDown && !root.isHolding)
		{
			Debug.Log("NewPreviewArea from DragAndPlace");
			// areaCheck.Invoke("NewPreviewArea", 0.02f);
			areaCheck.NewPreviewArea(gameObject);
			
			if (placed)
		    {
		        lastSafePos = transform.position;
		        lastSafeRot = transform.rotation;
            }

            placed = false;
			root.isHolding = true;
		}
	}
}
