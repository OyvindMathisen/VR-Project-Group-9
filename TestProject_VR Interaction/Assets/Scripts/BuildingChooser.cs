using UnityEngine;
using System.Collections;
using Immerseum.VRSimulator;

public class BuildingChooser : MonoBehaviour {

    public Transform[] Buildings, Landscapes;
    public Transform[] previewBuildings, previewLandscapes;
    public int tileType = 0;
    private int currentTileB = 0, currentTileL = 0;
    private GameObject currentPreview;
    public Transform Panel;

	private Vector3 PANEL_POS = new Vector3(-0.1977f, 0.2587f, 0.4187f);

	private Wand Lhand;
	private bool hasSwitched = false;
	
	void Awake () {
        ShowPreview(0, 0);

		Lhand = GameObject.Find("[CameraRig]").transform.FindChild("Controller (left)").GetComponent<Wand>();
	}

    // spawns the previews of the tiles in the panel
    void ShowPreview(int tileType, int tileNumber)
    {
        if (tileType == 0)
        {
            Instantiate(previewBuildings[tileNumber], transform.position + new Vector3(0f, 0.02f, 0f), Quaternion.identity, Panel);
        }
        else if (tileType == 1)
        {
			Instantiate(previewBuildings[tileNumber], transform.position + new Vector3(0f, 0.02f, 0f), Quaternion.identity, Panel);
		}
        
    }

    void Update()
    {
		/*
		// TODO: Fix this spawning the previews ontop of each other infinitly
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
			tileType = 0;
			ShowPreview(0, currentTileB);
			hasSwitched = true;
		}

		if (Lhand.touchpadDown && !hasSwitched)
		{
			tileType = 1;
			ShowPreview(0, currentTileB);
			hasSwitched = true;
		}

		if (!Lhand.touchpadRight && !Lhand.touchpadLeft && !Lhand.touchpadUp && !Lhand.touchpadDown)
		{
			hasSwitched = false;
		}
		*/

        // temp easier build mode with mouse
        if (Input.GetKey(KeyCode.Tab))
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = true;
        }

        // poorly designed tile choosing system, but it works
        if (Input.GetKeyDown(root.nextPreviewKey))
        {
            if (tileType == 0)
            {
                currentTileB++;
                if (currentTileB == Buildings.Length)
                {
                    currentTileB = 0;
                }
                ShowPreview(0, currentTileB);
            }
            else if (tileType == 1)
            {
                currentTileL++;
                if (currentTileL == Landscapes.Length)
                {
                    currentTileL = 0;
                }
                ShowPreview(1, currentTileL);
            }
        }
        if (Input.GetKeyDown(root.prevPreviewKey))
        {
            if (tileType == 0)
            {
                currentTileB--;
                if (currentTileB == -1)
                {
                    currentTileB = Buildings.Length-1;
                }
                ShowPreview(0, currentTileB);
            }
            else if (tileType == 1)
            {
                currentTileL--;
                if (currentTileL == -1)
                {
                    currentTileL = Landscapes.Length-1;
                }
                ShowPreview(1, currentTileL);
            }
        }

        // temp: switch between tiletypes
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            tileType = 0;
            ShowPreview(0, currentTileB);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            tileType = 1;
            ShowPreview(1, currentTileL);
        }
    }

    void FixedUpdate()
    {
        ControllerManager.moveController(0, transform.position + new Vector3(0, 0, 0.01f), HMDSimulator.leftController.rotation);
    }
}
