using UnityEngine;

public class Wand : MonoBehaviour
{
    private Valve.VR.EVRButtonId gripButton = Valve.VR.EVRButtonId.k_EButton_Grip;
    public bool gripButtonDown = false;
    public bool gripButtonUp = false;
    public bool gripButtonPressed = false;

    private Valve.VR.EVRButtonId triggerButton = Valve.VR.EVRButtonId.k_EButton_SteamVR_Trigger;
    public bool triggerButtonDown = false;
    public bool triggerButtonUp = false;
    public bool triggerButtonPressed = false;

	public bool touchpadUp = false;
	public bool touchpadDown = false;
	public bool touchpadRight = false;
	public bool touchpadLeft = false;

	public float touchpadDetectLimit = 0.7f;

    private SteamVR_Controller.Device controller { get { return SteamVR_Controller.Input((int)trackedObj.index); } }
    private SteamVR_TrackedObject trackedObj;
	private SteamVR_Controller.Device device;

    void Start()
    {
        trackedObj = GetComponent<SteamVR_TrackedObject>();
    }

	void FixedUpdate()
	{
		device = SteamVR_Controller.Input((int)trackedObj.index);
	}

    void Update()
    {
        if (controller == null)
        {
            Debug.Log("Controller not initialized");
            return;
        }

		if (device.GetPress(SteamVR_Controller.ButtonMask.Touchpad))
		{
			Vector2 touchpad = (device.GetAxis(Valve.VR.EVRButtonId.k_EButton_Axis0));
			touchpadUp = touchpad.y > touchpadDetectLimit;
			touchpadDown = touchpad.y < -touchpadDetectLimit;
			touchpadRight = touchpad.x > touchpadDetectLimit;
			touchpadLeft = touchpad.x < -touchpadDetectLimit;
		}

		if (!device.GetPress(SteamVR_Controller.ButtonMask.Touchpad))
        {
			touchpadUp = false;
			touchpadDown = false;
			touchpadRight = false;
			touchpadLeft = false;
		}

		gripButtonDown = controller.GetPressDown(gripButton);
        gripButtonUp = controller.GetPressUp(gripButton);
        gripButtonPressed = controller.GetPress(gripButton);

        triggerButtonDown = controller.GetPressDown(triggerButton);
        triggerButtonUp = controller.GetPressUp(triggerButton);
        triggerButtonPressed = controller.GetPress(triggerButton);
    }
}