using UnityEngine;
using System.Collections.Generic;

public class BeginGameDetection : MonoBehaviour
{
	public GameObject MainGameObjects;
	public GameObject MainMenuObjects;
	public GameObject TilesLocation;

	private List<GameObject> _objectsInGameStartArea = new List<GameObject>();

	// Update is called once per frame
	void Update()
	{
		if (_objectsInGameStartArea.Count <= 0) return; // Prevents checking empty list.
														// Ensures only blocks at the right height can start the game.
		if (_objectsInGameStartArea[0].GetComponent<DragAndPlace>().ReachedHeight) // Note. This is a getcomponent in an update function. BE CAREFUL.
		{
			LaunchGame();
		}
	}

	void LaunchGame()
	{
		// Currently not functioning, since we cannot put the tilelocation transform onto the object.
		/*
		// Deletes excess buildings if the player has placed more than minimum.
		if (TilesLocation.transform.childCount > 0)
		{
			foreach (Transform child in TilesLocation.transform)
			{
				Destroy(child.gameObject);
			}
		}
		*/

		// Turns off main menu mode and turns on the game.
		MainGameObjects.SetActive(true);
		MainMenuObjects.SetActive(false);

		Destroy(gameObject); // Cleanup of the object after tutorial is done.
	}

	// Adds all blocks entering the start-game field into a list.
	void OnTriggerEnter(Collider other)
	{
		var gameObjectWithScript = other.transform.parent.gameObject;

        // Check why this does not trigger.
        // TODO: CHECK IF CORRECT TAG
        if (other.tag != "Building" && !_objectsInGameStartArea.Contains(gameObjectWithScript)) return;
		_objectsInGameStartArea.Add(other.transform.parent.gameObject);
	}

	// Removes them, in case you drag a block away from the start-game field.
	void OnTriggerExit(Collider other)
	{
		var gameObjectWithScript = other.transform.parent.gameObject;
        // TODO: CHECK IF CORRECT TAG
        if (other.tag != "Building" && !_objectsInGameStartArea.Contains(gameObjectWithScript)) return;
		_objectsInGameStartArea.Remove(other.transform.parent.gameObject);
	}
}
