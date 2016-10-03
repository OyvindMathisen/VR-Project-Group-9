using UnityEngine;
using System.Collections;

public class ChangePlayerSize : MonoBehaviour {

	public GameObject CameraRig;
	public GameObject panel;

	private Wand Lhand;
	private bool isShrunk;

	void Awake ()
	{
		Lhand = GameObject.Find("[CameraRig]").transform.FindChild("Controller (left)").GetComponent<Wand>();
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (Lhand.gripButtonDown)
		{
			if (isShrunk)
			{
				CameraRig.transform.localScale = new Vector3(100.0f, 100.0f, 100.0f);
				CameraRig.transform.position = new Vector3(0.0f, 0.0f, 0.0f);

				panel.SetActive(true);
				GetComponent<SteamVR_LaserPointer>().active = false;
				GetComponent<SteamVR_Teleporter>().teleportOnClick = false;
				isShrunk = false;
			}
			else
			{
				CameraRig.transform.localScale = new Vector3(0.55f, 0.55f, 0.55f);
				CameraRig.transform.position = new Vector3(0.0f, 101.0f, 5.5f);

				panel.SetActive(false);
				GetComponent<SteamVR_LaserPointer>().active = true;
				GetComponent<SteamVR_Teleporter>().teleportOnClick = true;
				isShrunk = true;
			}
		}
	}
}
