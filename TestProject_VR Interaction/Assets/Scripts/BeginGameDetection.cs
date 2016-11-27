using UnityEngine;
using System.Collections.Generic;

public class BeginGameDetection : MonoBehaviour
{
	public GameObject MainGameObjects;
	public GameObject MainMenuObjects;

    public bool continueGame;

	private List<GameObject> _objectsInGameStartArea = new List<GameObject>();
    void Start()
    {
        if (continueGame && !SaveAndLoad.FileExists())
            gameObject.SetActive(false);
    }

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
        // TODO: Find alternative, non Find method.
        GameObject[] gameObjectArray = GameObject.FindGameObjectsWithTag("Start");

        foreach (GameObject startBlocks in gameObjectArray)
            Destroy(startBlocks);

		// Turns off main menu mode and turns on the game.
		MainGameObjects.SetActive(true);
		MainMenuObjects.SetActive(false);

        if (continueGame)
        {
            SaveAndLoad.Load();
            GameDataHandler.Save();
        }
        else
        {
            SaveAndLoad.Delete();
            GameFile.current = new GameFile();
            SaveAndLoad.Save();
        }

        // so the tutorial movie won't continue playing (it will even if the object isn't active)
        transform.parent.FindChild("TutorialScreen").FindChild("Movie").GetComponent<Movie>().StopMovie();

		Destroy(_objectsInGameStartArea[0]); // Cleanup of the object after tutorial is done.
	}

	// Adds all blocks entering the start-game field into a list.
	void OnTriggerEnter(Collider other)
	{
		var gameObjectWithScript = other.transform.parent.gameObject;

        // Check why this does not trigger.
        if (other.tag != "Tile" && !_objectsInGameStartArea.Contains(gameObjectWithScript)) return;
		_objectsInGameStartArea.Add(other.transform.parent.gameObject);
	}

	// Removes them, in case you drag a block away from the start-game field.
	void OnTriggerExit(Collider other)
	{
		var gameObjectWithScript = other.transform.parent.gameObject;

        if (other.tag != "Tile" && !_objectsInGameStartArea.Contains(gameObjectWithScript)) return;
		_objectsInGameStartArea.Remove(other.transform.parent.gameObject);
	}
}
