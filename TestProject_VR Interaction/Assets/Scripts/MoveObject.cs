using UnityEngine;
using System.Collections;

public class MoveObject : MonoBehaviour
{
	protected Wand _controllerScriptRight;
	protected Wand _controllerScriptLeft;

	// Local check if object is being held, to avoid
	// that grabbing other objects affect the check.
	protected bool _leftHolding;
	protected bool _rightHolding;

    // Use this for initialization
    void Start()
    {
        _controllerScriptRight = HMDComponents.getRightWand();
		_controllerScriptLeft = HMDComponents.getLeftWand();
    }

    // Update is called once per frame
	protected virtual void Update()
    {
		if (!_rightHolding && !_leftHolding) return; // Should only run if the object is actually held.

		CheckForTriggerUp ();

		// If both controllers release the object after the check
		// Then we unbind it from our controllers.
		if (!_rightHolding && !_leftHolding) 
		{
			transform.parent = null;
		}
    }

	protected void CheckForTriggerUp() // Checks if one of the controllers has released the object.
	{
		if (_controllerScriptRight.TriggerButtonUp)
		{
			_controllerScriptRight.IsHolding = false;
			_rightHolding = false;
		}

		if (_controllerScriptLeft.TriggerButtonUp)
		{
			_controllerScriptLeft.IsHolding = false;
			_leftHolding = false;
		}
	}

	// The wand script on the controllers tells the object which one is holding it.
	public void WarnObjectItsHeld(Wand controller)
	{
		if (controller == _controllerScriptRight)
		{
			// Only one of the controllers can hold
			// the object at once.
			_rightHolding = true;
			_leftHolding = false;
		}
		else if (controller == _controllerScriptLeft)
		{
			_leftHolding = true;
			_rightHolding = false;
		}
	}
}
