using UnityEngine;
using System.Collections;

public class Indicator : MonoBehaviour {

    private Combiner _combiner;
	void Awake () {
        _combiner = transform.parent.GetComponent<Combiner>();
	}

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Tile")
            if (!_combiner.Triggered.Contains(other.gameObject))
                _combiner.Triggered.Add(other.gameObject);
    }

    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Tile")
            _combiner.Triggered.Remove(other.gameObject);
    }
}
