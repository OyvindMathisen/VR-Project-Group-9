using UnityEngine;

public class Wand : MonoBehaviour
{
    private Valve.VR.EVRButtonId gripButton = Valve.VR.EVRButtonId.k_EButton_Grip;
    private Valve.VR.EVRButtonId triggerButton = Valve.VR.EVRButtonId.k_EButton_SteamVR_Trigger;

    [Header("Gripbutton States")]
    public bool GripButtonDown = false;
    public bool GripButtonUp = false;
    public bool GripButtonPressed = false;

    [Header("Triggerbutton States")]
    public bool TriggerButtonDown = false;
    public bool TriggerButtonUp = false;
    public bool TriggerButtonPressed = false;

    [Header("Touchpad States")]
    public bool TouchpadUp = false;
    public bool TouchpadDown = false;
    public bool TouchpadRight = false;
    public bool TouchpadLeft = false;

    public float TouchpadDetectLimit = 0.5f;

    public bool IsHolding = false;

    private SteamVR_Controller.Device Controller { get { return SteamVR_Controller.Input((int)_trackedObj.index); } }
    private SteamVR_TrackedObject _trackedObj;
    private SteamVR_Controller.Device _device;

    void Start()
    {
        _trackedObj = GetComponent<SteamVR_TrackedObject>();
    }

    void FixedUpdate()
    {
        _device = SteamVR_Controller.Input((int)_trackedObj.index);
    }

    void Update()
    {
        if (Controller == null)
        {
            Debug.Log("Controller not initialized");
            return;
        }

        if (_device.GetPress(SteamVR_Controller.ButtonMask.Touchpad))
        {
            Vector2 touchpad = (_device.GetAxis());
            TouchpadUp = touchpad.y > TouchpadDetectLimit;
            TouchpadDown = touchpad.y < -TouchpadDetectLimit;
            TouchpadRight = touchpad.x > TouchpadDetectLimit;
            TouchpadLeft = touchpad.x < -TouchpadDetectLimit;
        }

        if (!_device.GetPress(SteamVR_Controller.ButtonMask.Touchpad))
        {
            TouchpadUp = false;
            TouchpadDown = false;
            TouchpadRight = false;
            TouchpadLeft = false;
        }

        GripButtonDown = Controller.GetPressDown(gripButton);
        GripButtonUp = Controller.GetPressUp(gripButton);
        GripButtonPressed = Controller.GetPress(gripButton);

        TriggerButtonDown = Controller.GetPressDown(triggerButton);
        TriggerButtonUp = Controller.GetPressUp(triggerButton);
        TriggerButtonPressed = Controller.GetPress(triggerButton);
    }
}