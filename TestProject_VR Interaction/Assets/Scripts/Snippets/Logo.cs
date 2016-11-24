using UnityEngine;
using System.Collections;

public class Logo : MonoBehaviour {

    private Transform _cam;
	void Awake ()
	{
	    _cam = GameObject.FindWithTag("MainCamera").transform;
	}

	void Update ()
	{
        // always be visible to the camera in the horizon
	    var temp = transform.eulerAngles;
	    temp.y = _cam.eulerAngles.y;
        transform.eulerAngles = temp;
    }
}
