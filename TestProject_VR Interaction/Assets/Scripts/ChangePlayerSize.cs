using UnityEngine;
using System.Collections;

public class ChangePlayerSize : MonoBehaviour
{
	public GameObject CameraRig;

    private GameObject _leftController;
	private Wand _lefthand;
	private bool _isShrunk;

	void Start ()
	{
		_leftController = HMDComponents.getLeftController();
		_lefthand = HMDComponents.getLeftWand();
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (!_lefthand.GripButtonDown) return;

	    if (_isShrunk)
	    {
	        CameraRig.transform.localScale = new Vector3(100.0f, 100.0f, 100.0f);
	        CameraRig.transform.position = new Vector3(0.0f, 0.0f, 0.0f);
			
	        _isShrunk = false;
	    }
	    else
	    {
	        CameraRig.transform.localScale = new Vector3(0.55f, 0.55f, 0.55f);
	        CameraRig.transform.position = new Vector3(0.0f, 101.0f, 5.5f);

	        _isShrunk = true;
	    }
	}
}
