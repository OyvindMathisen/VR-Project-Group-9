using UnityEngine;
using System.Collections.Generic;

public class ComboViewSelector : MoveObject
{
	public List<GameObject> Comboes = new List<GameObject>();

	private int _comboNumber = 0;
	private bool _hasChanged;

	// Update is called once per frame
	void Update()
	{
		if (!Holder) return;
		ControllerCheck();
	}

	// Checks for input from right controller, and resets category changing if needed.
	private void ControllerCheck()
	{
		if (Holder.TouchpadRight && !_hasChanged)
		{
			_comboNumber++;
			SetActiveComboInstruction();
		}

		if (Holder.TouchpadLeft && !_hasChanged)
		{
			_comboNumber--;
			SetActiveComboInstruction();
		}

		// Allow the user to scroll through the list again
		// after releasing the touchpad button.
		if (!Holder.TouchpadRight &&
			!Holder.TouchpadLeft)
			_hasChanged = false;
	}

	private void SetActiveComboInstruction()
	{
		// Prevents the combonumber to overflow or underflow over/under the amount of
		// comboes we can show.
		if (_comboNumber < 0)
		{
			_comboNumber = Comboes.Count - 1;
		}

		if (_comboNumber > Comboes.Count - 1)
		{
			_comboNumber = 0;
		}

		// Hides all of the comboscreenes...
		foreach (GameObject screens in Comboes)
		{
			screens.SetActive(false);
		}

		// ...before showing the right one.
		Comboes[_comboNumber].SetActive(true);

		_hasChanged = true;
	}
}
