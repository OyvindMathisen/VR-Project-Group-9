using UnityEngine;

public class HMDComponents : MonoBehaviour
{
    public GameObject RightGameObject;
    public GameObject LeftGameObject;
    public GameObject HeadCamera;

    private Wand RightWand;
    private Wand LeftWand;

    void Awake()
    {
        RightWand = RightGameObject.GetComponent<Wand>();
        LeftWand = LeftGameObject.GetComponent<Wand>();
    }

    Wand getRightWand()
    {
        return RightWand;
    }

    Wand getLeftWand()
    {
        return LeftWand;
    }
}
