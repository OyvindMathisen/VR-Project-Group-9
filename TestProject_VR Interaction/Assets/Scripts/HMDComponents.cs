using UnityEngine;

public class HMDComponents : MonoBehaviour
{
	// Get the controllers and the HeadMountedDisplay objects.
	public GameObject RightController;
	public GameObject LeftController;
	public GameObject HeadCamera;

	static private Wand _rightWand;
	static private Wand _leftWand;

	void Awake()
	{
		_rightWand = RightController.GetComponent<Wand>();
		_leftWand = LeftController.GetComponent<Wand>();
	}

	public static GameObject GetRightController()
	{
		return _rightWand.gameObject;
	}

	public static GameObject GetLeftController()
	{
		return _leftWand.gameObject;
	}

	public static Wand GetRightWand()
	{
		return _rightWand;
	}

	public static Wand GetLeftWand()
	{
		return _leftWand;
	}
}
