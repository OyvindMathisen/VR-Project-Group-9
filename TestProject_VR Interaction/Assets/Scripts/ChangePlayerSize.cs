using UnityEngine;
using System.Collections;

public class ChangePlayerSize : MonoBehaviour {

	public GameObject CameraRig;
	public GameObject Panel;
    public GameObject LeftController;

	private Wand _Lhand;
	private bool _isShrunk;

    private SteamVR_LaserPointer _laser;
    private SteamVR_Teleporter _teleport;

	void Awake ()
	{
		_Lhand = LeftController.GetComponent<Wand>();
	    _laser = LeftController.GetComponent<SteamVR_LaserPointer>();
	    _teleport = LeftController.GetComponent<SteamVR_Teleporter>();
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update ()
	{
	    if (!_Lhand.GripButtonDown) return;

	    if (_isShrunk)
	    {
	        CameraRig.transform.localScale = new Vector3(100.0f, 100.0f, 100.0f);
	        CameraRig.transform.position = new Vector3(0.0f, 0.0f, 0.0f);

	        Panel.SetActive(true);
	        _laser.active = false;
	        _teleport.teleportOnClick = false;
	        _isShrunk = false;
	    }
	    else
	    {
	        CameraRig.transform.localScale = new Vector3(0.55f, 0.55f, 0.55f);
	        CameraRig.transform.position = new Vector3(0.0f, 101.0f, 5.5f);

	        Panel.SetActive(false);
	        _laser.active = true;
	        _teleport.teleportOnClick = true;
	        _isShrunk = true;
	    }
	}
}
