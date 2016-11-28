using UnityEngine;
using System.Collections;

public class Exit : MonoBehaviour
{

    private DragAndPlace _buildingScript;
    private bool _quit;
	void Awake ()
	{
	    _buildingScript = GetComponent<DragAndPlace>();
	}

	void Update () {
	    if (_buildingScript.ReachedHeight && !_quit)
	    {
            GameDataHandler.Save();
            Invoke("QuitGame", 0.05f);
            _quit = true;
            Debug.Log("Application.Quit() have been called!");
	    }
	}

    void QuitGame()
    {
        Application.Quit();
    }
}
