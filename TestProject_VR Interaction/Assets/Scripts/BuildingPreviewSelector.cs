using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BuildingPreviewSelector : MoveObject
{
	[Header("Building Categories")]
	public List<GameObject> _categories;

	private int _categoryPosition = 0;
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
		// TODO: Consider changing this for a swipe.
		// Also, needs to be more visible to the player they can
		// indeed change category this way.
		if (ControllerScriptRight.TouchpadUp && !_hasChanged)
		{
			_categoryPosition++;
			SetActiveCategory();
		}

		if (ControllerScriptRight.TouchpadDown && !_hasChanged)
		{
			_categoryPosition--;
			SetActiveCategory();
		}

		// Allow the user to scroll through the list again
		// after releasing the touchpad button.
		if (!ControllerScriptRight.TouchpadUp &&
			!ControllerScriptRight.TouchpadDown)
			_hasChanged = false;
	}

	private void LeftControllerCheck()
	{
		if (ControllerScriptLeft.TouchpadUp && !_hasChanged)
		{
			_categoryPosition++;
			SetActiveCategory();
		}

		if (ControllerScriptLeft.TouchpadDown && !_hasChanged)
		{
			_categoryPosition--;
			SetActiveCategory();
		}

		if (!ControllerScriptLeft.TouchpadUp &&
			!ControllerScriptLeft.TouchpadDown)
			_hasChanged = false;
	}

	// Sets the active category based on user input.
	private void SetActiveCategory()
	{
		if (_categoryPosition < 0)
		{
			_categoryPosition = _categories.Count-1;
		}

		if (_categoryPosition > _categories.Count-1)
		{
			_categoryPosition = 0;
		}

		// Unload all the categories
		foreach (GameObject obj in _categories)
		{
			obj.gameObject.SetActive (false);
		}

		// Load the new category
		_categories [_categoryPosition].SetActive (true);

		// Prevent the user from scrolling through the list
		// once per frame.
		_hasChanged = true;

		Debug.Log ("_categoryPosition: " + _categoryPosition);
	}
}
