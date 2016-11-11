using UnityEngine;
using System.Collections.Generic;

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

	public float TouchpadDetectLimit = 0.0f;

	public bool IsHolding = false;

	private SteamVR_Controller.Device Controller { get { return SteamVR_Controller.Input((int)_trackedObj.index); } }
	private SteamVR_TrackedObject _trackedObj;
	private SteamVR_Controller.Device _device;

	// Objects entering controller space.
	private List<GameObject> _objectsWithinReach = new List<GameObject>();


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
		// Check to make sure the controller actually connected.
		if (Controller == null)
		{
			Debug.Log("Controller not initialized");
			return;
		}

		CheckTouchpadStates();
		CheckButtonStates();

		CheckIfCanGrabObject();
		//TODO: Consider moving the building movement handling to here.
	}

	// Adds objects to the list of interactable ones
	// as they enter the trigger for the controller.
	void OnTriggerEnter(Collider other)
	{
		// Ensures that the object can be moved and that
		// it doesnt add any duplicates to the list.
		if (other.GetComponent<MoveObject>() != null &&
			!_objectsWithinReach.Contains(other.gameObject))
		{
			_objectsWithinReach.Add(other.gameObject);
		}
	}

	// Removes objects from the list of interactable ones
	// if they leave the triggerzone of the controller.
	void OnTriggerExit(Collider other)
	{
		// Makes sure the list has this object before removing it.
		if (_objectsWithinReach.Contains(other.gameObject))
		{
			_objectsWithinReach.Remove(other.gameObject);
		}
	}

	// Handler for the touchpad states.
	void CheckTouchpadStates()
	{
		if (_device.GetPress(SteamVR_Controller.ButtonMask.Touchpad))
		{
			Vector2 touchpad = (_device.GetAxis());
			TouchpadUp = touchpad.y > TouchpadDetectLimit;
			TouchpadDown = touchpad.y < -TouchpadDetectLimit;
			TouchpadRight = touchpad.x > TouchpadDetectLimit;
			TouchpadLeft = touchpad.x < -TouchpadDetectLimit;
		}

		// This prevents the bool from staying true all the time,
		// and makes it only true while its pushed and held.
		if (!_device.GetPress(SteamVR_Controller.ButtonMask.Touchpad))
		{
			TouchpadUp = false;
			TouchpadDown = false;
			TouchpadRight = false;
			TouchpadLeft = false;
		}
	}

	// Sets the bool for the buttons on your controller.
	void CheckButtonStates()
	{
		GripButtonDown = Controller.GetPressDown(gripButton);
		GripButtonUp = Controller.GetPressUp(gripButton);
		GripButtonPressed = Controller.GetPress(gripButton);

		TriggerButtonDown = Controller.GetPressDown(triggerButton);
		TriggerButtonUp = Controller.GetPressUp(triggerButton);
		TriggerButtonPressed = Controller.GetPress(triggerButton);
	}

	void CheckIfCanGrabObject()
	{
		// Only grab object while holding over it, and then pressing down the trigger.
		if (!IsHolding && TriggerButtonDown && _objectsWithinReach.Count > 0)
		{
			IsHolding = true;
			_objectsWithinReach[0].transform.parent = transform;
			_objectsWithinReach [0].GetComponent<MoveObject> ().WarnObjectItsHeld (this);
		}
	}
}