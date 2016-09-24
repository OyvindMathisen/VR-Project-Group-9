using UnityEngine;
using System.Collections;

public class HideHighTiles : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Tile")
        {
            other.GetComponent<MeshRenderer>().enabled = false;
        }
    }
    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Tile")
        {
            other.GetComponent<MeshRenderer>().enabled = true;
        }
    }
}
