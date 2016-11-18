using UnityEngine;
using System.Collections;

public class ChangePlayerSize : MonoBehaviour
{
	public GameObject CameraRig;

	public Vector3 NormalPlayerSize = new Vector3(100.0f, 100.0f, 100.0f);
	public Vector3 ShrunkenPlayerSize = new Vector3(0.55f, 0.55f, 0.55f);

	public Vector3 NormalPlayerSizePosition = new Vector3(0.0f, 0.0f, 0.0f);
	public Vector3 ShrunkenPlayerSizePositon = new Vector3(0.0f, 101.0f, 5.5f);

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
			CameraRig.transform.localScale = NormalPlayerSize;
			CameraRig.transform.position = NormalPlayerSizePosition;
			
	        _isShrunk = false;
	    }
	    else
	    {
			CameraRig.transform.localScale = ShrunkenPlayerSize;
			CameraRig.transform.position = ShrunkenPlayerSizePositon;

	        _isShrunk = true;
	    }
	}
}
