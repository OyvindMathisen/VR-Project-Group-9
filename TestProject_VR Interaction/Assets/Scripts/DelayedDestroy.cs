using UnityEngine;
using System.Collections;

public class DelayedDestroy : MonoBehaviour {

    // Use this for initialization
    public float lifetime;

    void Start()
    {
        Destroy(gameObject, lifetime);
    }
}