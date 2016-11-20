using UnityEngine;
using System.Collections;

public class ChangePlayerSize : MonoBehaviour
{
	[Header("Main CameraRig Object")]
	public GameObject CameraRig;

	[Header("Player Size Variables")]
	public Vector3 NormalPlayerSize = new Vector3(100.0f, 100.0f, 100.0f);
	public Vector3 ShrunkenPlayerSize = new Vector3(0.55f, 0.55f, 0.55f);

	public Vector3 NormalPlayerSizePosition = new Vector3(0.0f, 0.0f, 0.0f);
	public Vector3 ShrunkenPlayerSizePositon = new Vector3(0.0f, 101.0f, 5.5f);

	private Wand _lefthand;
	private bool _isShrunk;

	void Start()
	{
		_lefthand = HMDComponents.GetLeftWand();
	}

	// Update is called once per frame
	void Update()
	{
		// Checks if the gripbutton on the lefthand has been pressed.
		// TODO: Move responcibillity over to the controller itself,
		// and make it tell the script to fire.
		if (!_lefthand.GripButtonDown) return;

		// If the player has been shrunk, unshrink them.
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
