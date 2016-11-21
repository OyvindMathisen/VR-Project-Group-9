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

	[Header("Others")]
	public float TouchpadDeadZone = 0.1f;
	public bool IsHolding = false;

	public AreaCheck AreaCheck;

	private GameObject _heldObject;

	private SteamVR_Controller.Device Controller { get { return SteamVR_Controller.Input((int)_trackedObj.index); } }
	private SteamVR_TrackedObject _trackedObj;
	private SteamVR_Controller.Device _device;

	// Objects entering controller space.
	private List<GameObject> _objectsWithinReach = new List<GameObject>();
	private List<SpawnThis> _spawnObjectsWithinReach = new List<SpawnThis>();

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

		HandleControls();
	}

	// Adds objects to the list of interactable ones
	// as they enter the trigger for the controller.
	void OnTriggerEnter(Collider other)
	{
		// Ensures that the object can be moved and that
		// it doesnt add any duplicates to the list.
		var spawnScript = other.transform.parent.GetComponent<SpawnThis>();
		if (spawnScript != null &&
			!_spawnObjectsWithinReach.Contains(spawnScript))
		{
			_spawnObjectsWithinReach.Add(spawnScript);
		}

		if (RecursiveSearch<MoveObject>(other.gameObject) != null &&
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
		var spawnScript = other.transform.parent.GetComponent<SpawnThis>();
		if (spawnScript != null &&
			_spawnObjectsWithinReach.Contains(spawnScript))
		{
			_spawnObjectsWithinReach.Remove(spawnScript);
		}
			
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
			TouchpadUp = touchpad.y > TouchpadDeadZone;
			TouchpadDown = touchpad.y < -TouchpadDeadZone;
			TouchpadRight = touchpad.x > TouchpadDeadZone;
			TouchpadLeft = touchpad.x < -TouchpadDeadZone;
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
	// NOTE: Optimally the script would get these states directly on need, rather than saving them as bools like this.
	void CheckButtonStates()
	{
		GripButtonDown = Controller.GetPressDown(gripButton);
		GripButtonUp = Controller.GetPressUp(gripButton);
		GripButtonPressed = Controller.GetPress(gripButton);

		TriggerButtonDown = Controller.GetPressDown(triggerButton);
		TriggerButtonUp = Controller.GetPressUp(triggerButton);
		TriggerButtonPressed = Controller.GetPress(triggerButton);
	}

	// Checks the controllers states.
	void HandleControls()
	{
		if (Controller.GetPressDown(triggerButton)) OnTriggerDown();

		if (Controller.GetPressUp(triggerButton)) OnTriggerUp();

		if (Controller.GetPressDown(SteamVR_Controller.ButtonMask.Touchpad)) OnTouchPadPress();
	}

	// Handler for when the user presses the trigger down.
	void OnTriggerDown()
	{
		if (IsHolding) return;
		// If the user is hovering over the preview tiles.
		if (_spawnObjectsWithinReach.Count > 0)
		{
			var spawnObj = _spawnObjectsWithinReach[0].SpawnMyObject(this);
			Grab(spawnObj); return;
		}
		// If the user is hovering over any moveable object.
		if (_objectsWithinReach.Count > 0)
		{
			Grab(_objectsWithinReach[0]);
		}
	}

	// Handler for when user lifts the trigger.
	void OnTriggerUp()
	{
		if (!IsHolding) return;
		Drop();
	}

	// Handler for touchpad presses.
	void OnTouchPadPress()
	{
		// Checks where the player pressed on the touchpad.
		var axis = (_device.GetAxis());
		if (axis.x < -TouchpadDeadZone) // User pressed left
		{
			Rotate(DirectionLR.Left);
		}
		else if (axis.x > TouchpadDeadZone) // User pressed right
		{
			Rotate(DirectionLR.Right);
		}
	}

	// Handler for when a user rotates an object.
	void Rotate(DirectionLR dir)
	{
		if (!IsHolding) return; // Makes sure the object is only turned if its held.
		var heldScript = RecursiveSearch<DragAndPlace>(_heldObject); // Gets the script for the object.
		if (!heldScript) return; // Null check to avoid a NullRef.
		heldScript.Rotate(dir);
	}

	// Handler for when the user grabs an object.
	void Grab(GameObject grabObject)
	{
		// Check if the object, or any of its parents contains the script.
		if (RecursiveSearch<MoveObject>(grabObject) == null) return;

		// Tells the object the wand that is holding the object.
		RecursiveSearch<MoveObject>(grabObject).GrabMe(this);

		_heldObject = grabObject;
		IsHolding = true;
	}

	// Handler for when the user drops an object.
	public void Drop()
	{
		// Check if the requested object has the script (Done in RecursiveSearch), and makes it drop the item.
		if (!RecursiveSearch<MoveObject>(_heldObject).DropMe(this)) return;

		var heldScript = RecursiveSearch<DragAndPlace>(_heldObject);
		if (heldScript) // Null check to avoid a NullRef.
		{
			heldScript.FinalRotation();// Prevents the building from being wobbled to the side on release.
		}

		_heldObject = null;
		IsHolding = false;
		CleanLists();
	}

	// Iterates through all parent objects until the requested script <T> is found, or no parent is left.
	T RecursiveSearch<T>(GameObject objectIn)
	{
		var obj = objectIn;
		while (obj != null)
		{
			if (obj.GetComponent<T>() != null) // If the object has the script...
			{
				return obj.GetComponent<T>(); // ...return the script
			}
			if (obj.transform.parent) // If there is no script, we check if it has a parent.
			{
				obj = obj.transform.parent.gameObject; // And set object to it, and continue the loop.
			}
			else // If no parent is found, we break.
			{
				break;
			}
		}
		return default(T); // Return default, or basicly null.
	}

	// Empties the list to avoid lingering functions.
	private void CleanLists()
	{
		_spawnObjectsWithinReach.Clear();
		_objectsWithinReach.Clear();
	}
}