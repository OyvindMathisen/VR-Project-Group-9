﻿using UnityEngine;

public class SpawnThis : MonoBehaviour
{
	public Transform ObjectToSpawn;

	// Use this for initialization
	void Awake () {
        transform.rotation = root.previewRotation;
    }
	
	// Update is called once per frame
	void Update () {
        // temporary solution for getting rid of the tile that was already there when you choose a new one
	    if (Input.GetKeyDown(root.nextPreviewKey)
            || Input.GetKeyDown(root.prevPreviewKey)
            || Input.GetKeyDown(KeyCode.Alpha1)
            || Input.GetKeyDown(KeyCode.Alpha2))
        {
            Destroy(gameObject);
            root.previewRotation = transform.rotation;
        }
	}

    void FixedUpdate()
    {
        transform.Rotate(0, 1, 0);
    }

	void OnMouseDown()
	{
		// TODO: Change this to own written code. This is only here for testing.
		root._screenPoint = Camera.main.WorldToScreenPoint(gameObject.transform.position);
		root._offset = gameObject.transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, root._screenPoint.z));
        Instantiate(ObjectToSpawn, gameObject.transform.position, Quaternion.identity);
    }
}
