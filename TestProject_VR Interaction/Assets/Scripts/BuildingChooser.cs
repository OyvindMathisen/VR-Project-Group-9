using UnityEngine;
using System.Collections;

public class BuildingChooser : MonoBehaviour {

    public Transform[] Buildings, Landscapes;
    public Transform[] previewBuildings, previewLandscapes;
    public int tileType = 0;
    private int currentTileB = 0, currentTileL = 0;
    private GameObject currentPreview;
    public Transform Panel;
	void Awake () {
        ShowPreview(0, 0);
    }

    // spawns the previews of the tiles in the panel
    void ShowPreview(int tileType, int tileNumber)
    {
        if (tileType == 0)
        {
            Instantiate(previewBuildings[tileNumber], transform.position + new Vector3(-0.2275f, 0.2925f, 0.4f), Quaternion.Euler(0, -30, 0), Panel);
        }
        else if (tileType == 1)
        {
            Instantiate(previewLandscapes[tileNumber], transform.position + new Vector3(-0.2275f, 0.2925f, 0.4f), Quaternion.Euler(0, -30, 0), Panel);
        }
        
    }

    void Update()
    {
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
}
