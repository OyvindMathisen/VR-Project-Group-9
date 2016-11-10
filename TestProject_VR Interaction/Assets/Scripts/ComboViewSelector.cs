using UnityEngine;
using System.Collections;

public class ComboViewSelector : MoveObject {

	// temporary value
	// replace with the method to get all comboes
	private int _maxCombo = 10;

	private int _comboNumber = 0;
	private bool _hasChanged;
	
	// Update is called once per frame
	override protected void Update ()
	{
		base.Update ();

		if (RightHolding)
			RightControllerCheck();

		if (LeftHolding)
			LeftControllerCheck();
	}

	// Checks for input from right controller, and resets category changing if needed.
	private void RightControllerCheck()
	{
		if (ControllerScriptRight.TouchpadRight && !_hasChanged)
		{
			_comboNumber++;
			SetActiveComboInstruction();
		}

		if (ControllerScriptRight.TouchpadLeft && !_hasChanged)
		{
			_comboNumber--;
			SetActiveComboInstruction();
		}

		// Allow the user to scroll through the list again
		// after releasing the touchpad button.
		if (!ControllerScriptRight.TouchpadRight &&
			!ControllerScriptRight.TouchpadLeft)
			_hasChanged = false;
	}

	private void LeftControllerCheck()
	{
		if (ControllerScriptLeft.TouchpadRight && !_hasChanged)
		{
			_comboNumber++;
			SetActiveComboInstruction();
		}

		if (ControllerScriptLeft.TouchpadLeft && !_hasChanged)
		{
			_comboNumber--;
			SetActiveComboInstruction();
		}

		if (!ControllerScriptLeft.TouchpadRight &&
			!ControllerScriptLeft.TouchpadLeft)
			_hasChanged = false;
	}

	private void SetActiveComboInstruction()
	{
		if (_comboNumber < 0)
		{
			_comboNumber = _maxCombo;
		}

		if (_comboNumber > _maxCombo)
		{
			_comboNumber = 0;
		}

		// TODO: Make the function display the correct combo.
		Debug.Log("Combo number: " + _comboNumber + " is now displayed");

		_hasChanged = true;
	}
}
