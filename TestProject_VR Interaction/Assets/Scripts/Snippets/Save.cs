using UnityEngine;
using System.Collections;

public class Save : MonoBehaviour
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
	        Destroy(gameObject);
	    }
	}
}
