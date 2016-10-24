using UnityEngine;
using System.Collections;

public class DelayedDestroy : MonoBehaviour
{
	public float lifetime = 3.0f;

	// Use this for initialization
	void Start ()
	{
	
	}
	
	// Update is called once per frame
	void Update ()
	{
		Destroy(gameObject, lifetime);
	}
}
