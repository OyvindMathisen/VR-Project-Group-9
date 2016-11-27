using UnityEngine;
using System.Collections;

public class Exit : MonoBehaviour
{

    private DragAndPlace _buildingScript;
	void Awake ()
	{
	    _buildingScript = GetComponent<DragAndPlace>();
	}

	void Update () {
	    if (_buildingScript.ReachedHeight)
	    {
            GameDataHandler.Save();
            Invoke("QuitGame", 0.05f);
	    }
	}

    void QuitGame()
    {
        Application.Quit();
    }
}
