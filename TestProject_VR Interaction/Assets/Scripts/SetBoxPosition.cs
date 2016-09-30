using UnityEngine;
using System.Collections;

public class SetBoxPosition : MonoBehaviour {

    private Transform Camera;
    private Vector3 PositionStorage;

	// Use this for initialization
	void Awake () {
        Camera = GameObject.Find("[CameraRig]").transform.FindChild("Camera (head)").transform;
    }
	
	// Update is called once per frame
	void Update () {
        PositionStorage.z = Camera.transform.position.z;
        PositionStorage.x = Camera.transform.position.x;

        transform.position = PositionStorage;
	}
}
