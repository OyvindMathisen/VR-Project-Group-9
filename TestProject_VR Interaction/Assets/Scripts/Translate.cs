using UnityEngine;
using System.Collections;

public class Translate : MonoBehaviour {
    public float speedX, speedY, speedZ;
	// Update is called once per frame
	void Update () {
        var temp = transform.position;
        temp.x -= speedX * Time.deltaTime;
        temp.y -= speedY * Time.deltaTime;
        temp.z -= speedZ * Time.deltaTime;
        transform.position = temp;
	}
}
