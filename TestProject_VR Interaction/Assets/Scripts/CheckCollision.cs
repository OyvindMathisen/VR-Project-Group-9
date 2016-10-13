using UnityEngine;
using System.Collections;

public class CheckCollision : MonoBehaviour {

	public GameObject currentTileCollide;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public GameObject getTile()
	{
		return currentTileCollide;
	}

	public void setTile(GameObject newObject)
	{
		currentTileCollide = newObject;
	}

	void OnTriggerStay(Collider other)
	{
		currentTileCollide = other.gameObject;
	}

	void OnTriggerExit(Collider other)
	{
		currentTileCollide = null;
	}
}
