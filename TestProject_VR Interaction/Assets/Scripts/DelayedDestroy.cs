using UnityEngine;

public class DelayedDestroy : MonoBehaviour
{
	public float Lifetime = 3.0f;
	
	// Update is called once per frame
	void Update ()
	{
		Destroy(gameObject, Lifetime);
	}
}
