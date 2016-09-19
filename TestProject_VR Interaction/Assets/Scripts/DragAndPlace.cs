using System;
using UnityEngine;

public class DragAndPlace : MonoBehaviour
{
    private float BUILD_HEIGHT = 1.0f, BUILD_HEIGHT_LERP = 0.15f; // make the tiles stay at a certain height
    private bool placed = false; // if the tile is still "dragged" around (the mouse button is not released yet)
    private float SNAP_VALUE = 0.16f; // important for choosing grid size (do not edit unless you edit the tile sizes)
    float snapInverse;
	private Wand Rhand;

	private bool hasRotated = false;

	void Awake()
    {
		snapInverse = 1 / SNAP_VALUE;
		Rhand = GameObject.Find("[CameraRig]").transform.FindChild("Controller (right)").GetComponent<Wand>();
	}
	void Update()
	{
		if (!placed)
		{
			if (Rhand.triggerButtonUp)
			{
				// snap to ground level
				//var temp = transform.position;
				//temp.y = BUILD_HEIGHT;
				//transform.position = temp;
				placed = true;
			}

			if (Rhand.touchpadRight && !hasRotated)
			{
				transform.Rotate(0, -90, 0);
				hasRotated = true;
			}
			
			if (Rhand.touchpadLeft && !hasRotated)
			{
				transform.Rotate(0, 90, 0);
				hasRotated = true;
			}

			if (Rhand.gripButtonDown)
			{
				Destroy(gameObject);
			}

			// To prevent the building from rotating at the speed of light
			if (!Rhand.touchpadLeft && !Rhand.touchpadRight)
			{
				hasRotated = false;
			}
		}
	}
    void FixedUpdate()
    {
		if (!placed)
        {
            //Vector3 curScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, root._screenPoint.z);
			Vector3 curPosition = Rhand.transform.position; //Camera.main.ScreenToWorldPoint(curScreenPoint) + root._offset;

            float currentX = Mathf.Round(curPosition.x * snapInverse) / snapInverse;
            float currentZ = Mathf.Round(curPosition.z * snapInverse) / snapInverse;

            //float currentX = Mathf.Round(curPosition.y);
            //decimal currentX = Math.Round((decimal)curPosition.x, 2, MidpointRounding.AwayFromZero);

            transform.position = new Vector3(currentX, curPosition.y, currentZ);
        }
        else
        {
            if (transform.position.y != BUILD_HEIGHT)
            {
                var temp = transform.position;
                temp.y = Mathf.Lerp(transform.position.y, BUILD_HEIGHT, BUILD_HEIGHT_LERP);
                transform.position = temp;
            }
        }
    }

	void OnTriggerStay(Collider other)
	{
		if (other.tag == "Rhand" && Rhand.triggerButtonDown)
		{
			placed = false;
		}
	}
}
