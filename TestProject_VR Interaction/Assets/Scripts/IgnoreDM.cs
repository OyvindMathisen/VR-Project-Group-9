using UnityEngine;
using System.Collections;

public class IgnoreDM : MonoBehaviour {
    public int renderQueue = 1;
	void Awake () {
        GetComponent<MeshRenderer>().material.renderQueue = renderQueue;
	}
}
