﻿using UnityEngine;
using System.Collections;
using Immerseum.VRSimulator;

public class BuildingChooser : MonoBehaviour {

    public Transform[] Buildings, Landscapes;
    public Transform[] previewBuildings, previewLandscapes;
	public int tileType = 0;
    private int currentTileB, currentTileL = 0;
    private GameObject currentPreview;
    public Transform Panel;

	private Wand Lhand;
	private bool hasSwitched = false;
	
	void Awake () {
        ShowPreview(0, 0);

		Lhand = GameObject.Find("[CameraRig]").transform.FindChild("Controller (left)").GetComponent<Wand>();
	}

    // spawns the previews of the tiles in the panel
    void ShowPreview(int tileType_internal, int tileNumber)
    {
		// Remove old copies of the preview tile from the left hand.
        Transform[] allChildren = transform.parent.GetComponentsInChildren<Transform>();
        foreach (Transform child in allChildren)
        {
            if (child.tag == "PreviewTile")
            {
                Destroy(child.gameObject);
            }
        }

        if (tileType_internal == 0)
        {
            Instantiate(previewBuildings[tileNumber], transform.position + new Vector3(0f, 0.02f, 0f), Quaternion.identity, Panel);
        }

        else if (tileType_internal == 1)
        {
            Instantiate(previewLandscapes[tileNumber], transform.position + new Vector3(0f, 0.02f, 0f), Quaternion.identity, Panel);
		}
        
    }

    void Update()
    {
		// Changing building in your selector
		if (Lhand.touchpadRight && !hasSwitched)
		{
			switch (tileType)
			{
				case 0:
					currentTileB++;
					if (currentTileB == Buildings.Length)
					{
						currentTileB = 0;
					}
					ShowPreview(0, currentTileB);
					hasSwitched = true;
					break;
				case 1:
					currentTileL++;
					if (currentTileL == Landscapes.Length)
					{
						currentTileL = 0;
					}
                    ShowPreview(1, currentTileL);
					hasSwitched = true;
					break;
			}
		}

		if (Lhand.touchpadLeft && !hasSwitched)
		{
			switch (tileType)
			{
				case 0:
					currentTileB--;
					if (currentTileB == -1)
					{
						currentTileB = Buildings.Length - 1;
					}
					ShowPreview(0, currentTileB);
					hasSwitched = true;
					break;
				case 1:
					currentTileL--;
					if (currentTileL == -1)
					{
						currentTileL = Landscapes.Length - 1;
					}
					ShowPreview(1, currentTileL);
					hasSwitched = true;
					break;
			}
		}

		if (Lhand.touchpadUp && !hasSwitched)
		{
			tileType++;
			if (tileType >= 2)
				tileType = 0;
			ShowPreview(tileType, currentTileB);
			hasSwitched = true;
		}

		if (Lhand.touchpadDown && !hasSwitched)
        {
			tileType--;
			if (tileType <= -1)
				tileType = 1;
            ShowPreview(tileType, currentTileL);
            hasSwitched = true;
		}

		// Prevents scrolling through the menu stupidly quickly
		if (!Lhand.touchpadRight && !Lhand.touchpadLeft && !Lhand.touchpadUp && !Lhand.touchpadDown)
		{
			hasSwitched = false;
		}
    }
}
