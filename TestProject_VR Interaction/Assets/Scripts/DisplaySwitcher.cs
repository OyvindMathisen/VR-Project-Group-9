using UnityEngine;
using System.Collections.Generic;

public class DisplaySwitcher : MoveObject
{
    [Header("Switchable Objects")]
    public List<GameObject> Categories;

    private int _categoryPosition = 0;

	// Changes the category based on what button was pushed.
	public void ChangeCategory(ButtonType direction)
	{

		if (direction == ButtonType.Right)
		{
			_categoryPosition++;
			SetActiveCategory();
		}

		if (direction == ButtonType.Left)
		{
			_categoryPosition--;
			SetActiveCategory();
		}
	}

    // Sets the active category based on user input.
    private void SetActiveCategory()
    {
        if (_categoryPosition < 0)
        {
            _categoryPosition = Categories.Count - 1;
        }

        if (_categoryPosition > Categories.Count - 1)
        {
            _categoryPosition = 0;
        }

        // Unload all the categories
        foreach (GameObject obj in Categories)
        {
            obj.gameObject.SetActive(false);
        }

        // Load the new category
        Categories[_categoryPosition].SetActive(true);
    }
}
