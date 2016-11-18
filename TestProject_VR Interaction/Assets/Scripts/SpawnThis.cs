using UnityEngine;

public class SpawnThis : MonoBehaviour
{
	public GameObject ObjectToSpawn;

	// Spawns and returns the object.
	public GameObject SpawnMyObject(Wand controller){
		var spawnedObject = Instantiate (ObjectToSpawn, controller.transform.position, Quaternion.identity) as GameObject;
		controller.IsHolding = true;
		return spawnedObject;
	}
}
