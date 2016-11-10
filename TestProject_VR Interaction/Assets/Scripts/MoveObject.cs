using UnityEngine;
using System.Collections;

public class MoveObject : MonoBehaviour
{
	protected Wand ControllerScriptRight;
	protected Wand ControllerScriptLeft;

	// Local check if object is being held, to avoid
	// that grabbing other objects affect the check.
	protected bool LeftHolding;
	protected bool RightHolding;

    // Use this for initialization
    void Start()
    {
        ControllerScriptRight = HMDComponents.getRightWand();
		ControllerScriptLeft = HMDComponents.getLeftWand();
    }

    // Update is called once per frame
	protected virtual void Update()
    {
		if (!RightHolding && !LeftHolding) return; // Should only run if the object is actually held.

		CheckForTriggerUp ();

		// If both controllers release the object after the check
		// Then we unbind it from our controllers.
		if (!RightHolding && !LeftHolding) 
		{
			transform.parent = null;
		}
    }

	protected void CheckForTriggerUp() // Checks if one of the controllers has released the object.
	{
		if (ControllerScriptRight.TriggerButtonUp)
		{
			ControllerScriptRight.IsHolding = false;
			RightHolding = false;
		}

		if (ControllerScriptLeft.TriggerButtonUp)
		{
			ControllerScriptLeft.IsHolding = false;
			LeftHolding = false;
		}
	}

	// The wand script on the controllers tells the object which one is holding it.
	public void WarnObjectItsHeld(Wand controller)
	{
		if (controller == ControllerScriptRight)
		{
			// Only one of the controllers can hold
			// the object at once.
			RightHolding = true;
			LeftHolding = false;
		}
		else if (controller == ControllerScriptLeft)
		{
			LeftHolding = true;
			RightHolding = false;
		}
	}
}
