using System;
using UnityEngine;

public class DragAndPlace : MonoBehaviour
{
	private float BUILD_HEIGHT = 100.0f, BUILD_HEIGHT_LERP = 15.0f; // make the tiles stay at a certain height
	public bool placed; // if the tile is still "dragged" around (the mouse button is not released yet)
    private bool oncePlaced, onceNotPlaced;
    private float SNAP_VALUE = 8.0f; // important for choosing grid size (do not edit unless you edit the tile sizes)
	float snapInverse;
	private Wand Rhand;
	private bool hasRotated;
	public LayerMask tiles;

	private GameObject previewPlacement;

    private AreaCheck areaCheck;

    private Vector3 lastSafePos;
    private Quaternion lastSafeRot;

	void Awake()
	{
		snapInverse = 1 / SNAP_VALUE;
		Rhand = GameObject.Find("[CameraRig]").transform.FindChild("Controller (right)").GetComponent<Wand>();

		previewPlacement = GameObject.Find("PreviewPlacement");

        areaCheck = previewPlacement.GetComponent<AreaCheck>();

	    lastSafePos = Vector3.zero;

		placed = false;
	}

	void Start()
	{
		areaCheck.NewPreviewArea(gameObject);
	}

	void Update()
	{
		if (!placed)
		{
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
			    else
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
		}
		else
		{
            onceNotPlaced = false;
        }

		// TODO: Merge with other !placed above
		if (!placed)
		{
			Vector3 curPosition = Rhand.transform.position; 

			//float currentX = Mathf.Round(curPosition.x * snapInverse) / snapInverse;
			//float currentZ = Mathf.Round(curPosition.z * snapInverse) / snapInverse;

			//previewPlacement.transform.position = new Vector3(currentX, BUILD_HEIGHT, currentZ);

			var temp = transform.position;
			temp.x = Mathf.Lerp(transform.position.x, curPosition.x, 0.15f);
			temp.y = Mathf.Lerp(transform.position.y, curPosition.y, 0.15f);
			temp.z = Mathf.Lerp(transform.position.z, curPosition.z, 0.15f);
			transform.position = temp;

			oncePlaced = false;
		}
		else
		{
			if (transform.position.y != BUILD_HEIGHT)
			{
				var temp = transform.position;
				//temp.x = Mathf.Round(temp.x * snapInverse) / snapInverse;
				//temp.z = Mathf.Round(temp.z * snapInverse) / snapInverse;
				temp.y = Mathf.Lerp(transform.position.y, BUILD_HEIGHT, BUILD_HEIGHT_LERP);
				transform.position = temp;
			}
            if (!oncePlaced)
            {
                // run once when placed
                areaCheck.DeletePreviews();
                oncePlaced = true;
            }
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
