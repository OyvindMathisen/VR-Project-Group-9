using UnityEngine;
using System.Collections;

public class Sign : MonoBehaviour {

    private Transform _cam, _indicator;
    private bool _activeFlag;
    private int _timesEnabled;
    private SpriteRenderer _sr;
    void Awake () {
        _cam = GameObject.FindWithTag("MainCamera").transform;
        _indicator = transform.parent.FindChild("Indicator");
        _sr = GetComponent<SpriteRenderer>();
    }
	
	void Update () {

	    _sr.enabled = _indicator.gameObject.activeSelf;

        // the fourth time this is visible; destroy
	    if (_sr.enabled && _activeFlag)
	    {
	        var temp = _indicator.position;
	        temp.y += 13;
	        transform.position = temp;

	        _timesEnabled++;
	        _activeFlag = false;
	    }
	    else if (!_sr.enabled)
	    {
	        _activeFlag = true;
        }
	    if (_timesEnabled > 3)
	    {
            // TODO: save game to not show this again
	        Destroy(gameObject);
	    }

        // always be rotated in the direction of the camera (y axis)
        transform.LookAt(new Vector3(_cam.position.x, transform.position.y, _cam.position.z));
    }
}
