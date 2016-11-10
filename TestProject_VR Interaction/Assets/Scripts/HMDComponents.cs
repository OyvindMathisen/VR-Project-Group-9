using UnityEngine;

public class HMDComponents : MonoBehaviour
{
    public GameObject RightController;
    public GameObject LeftController;
    public GameObject HeadCamera;

    static private Wand RightWand;
    static private Wand LeftWand;

    void Awake()
    {
        RightWand = RightController.GetComponent<Wand>();
        LeftWand = LeftController.GetComponent<Wand>();
    }

	public static GameObject getRightController()
	{
		return RightWand.gameObject;
	}

	public static GameObject getLeftController()
	{
		return LeftWand.gameObject;
	}

    public static Wand getRightWand()
    {
        return RightWand;
    }

    public static Wand getLeftWand()
    {
        return LeftWand;
    }
}
