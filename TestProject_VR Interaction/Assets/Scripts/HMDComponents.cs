using UnityEngine;

public class HMDComponents : MonoBehaviour
{
    public GameObject RightGameObject;
    public GameObject LeftGameObject;
    public GameObject HeadCamera;

    private Wand _rightWand;
    private Wand _leftWand;

    void Awake()
    {
        _rightWand = RightGameObject.GetComponent<Wand>();
        _leftWand = LeftGameObject.GetComponent<Wand>();
    }

    public Wand GetRightWand()
    {
        return _rightWand;
    }

    public Wand GetLeftWand()
    {
        return _leftWand;
    }
}
