using UnityEngine;
using System.Linq;

public class BuildingChooser : MonoBehaviour
{
    public Transform[] Buildings, Landscapes, PreviewBuildings, PreviewLandscapes;
	public int TileType = 0;
    private int _currentTileB, _currentTileL = 0;
    public Transform Panel;

    public GameObject LeftController;
	private Wand _Lhand;
	private bool _hasSwitched = false;
	
	void Awake ()
    {
        ShowPreview(0, 0);
		_Lhand = LeftController.GetComponent<Wand>();
	}

    // spawns the previews of the tiles in the panel
    void ShowPreview(int tileTypeInternal, int tileNumber)
    {
		// Remove old copies of the preview tile from the left hand.
        Transform[] allChildren = transform.parent.GetComponentsInChildren<Transform>();
        foreach (Transform child in allChildren.Where(child => child.tag == "PreviewTile"))
        {
            Destroy(child.gameObject);
        }

        switch (tileTypeInternal)
        {
            case 0:
                Instantiate(PreviewBuildings[tileNumber], transform.position + new Vector3(0f, 0.02f, 0f), Quaternion.identity, Panel);
                break;
            case 1:
                Instantiate(PreviewLandscapes[tileNumber], transform.position + new Vector3(0f, 0.02f, 0f), Quaternion.identity, Panel);
                break;
        }
        
    }

    void Update()
    {
		// Changing building in your selector
		if (_Lhand.TouchpadRight && !_hasSwitched)
		{
			switch (TileType)
			{
				case 0:
					_currentTileB++;
					if (_currentTileB == Buildings.Length)
					{
						_currentTileB = 0;
					}
					ShowPreview(0, _currentTileB);
					_hasSwitched = true;
					break;
				case 1:
					_currentTileL++;
					if (_currentTileL == Landscapes.Length)
					{
						_currentTileL = 0;
					}
                    ShowPreview(1, _currentTileL);
					_hasSwitched = true;
					break;
			}
		}

		if (_Lhand.TouchpadLeft && !_hasSwitched)
		{
			switch (TileType)
			{
				case 0:
					_currentTileB--;
					if (_currentTileB == -1)
					{
						_currentTileB = Buildings.Length - 1;
					}
					ShowPreview(0, _currentTileB);
					_hasSwitched = true;
					break;
				case 1:
					_currentTileL--;
					if (_currentTileL == -1)
					{
						_currentTileL = Landscapes.Length - 1;
					}
					ShowPreview(1, _currentTileL);
					_hasSwitched = true;
					break;
			}
		}

		if (_Lhand.TouchpadUp && !_hasSwitched)
		{
			TileType++;
			if (TileType >= 2)
				TileType = 0;
			ShowPreview(TileType, _currentTileB);
			_hasSwitched = true;
		}

		if (_Lhand.TouchpadDown && !_hasSwitched)
        {
			TileType--;
			if (TileType <= -1)
				TileType = 1;
            ShowPreview(TileType, _currentTileL);
            _hasSwitched = true;
		}

		// Prevents scrolling through the menu once per frame.
		if (!_Lhand.TouchpadRight && !_Lhand.TouchpadLeft && !_Lhand.TouchpadUp && !_Lhand.TouchpadDown)
		{
			_hasSwitched = false;
		}
    }
}
