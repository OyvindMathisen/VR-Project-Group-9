using UnityEngine;
using System.Collections.Generic;

public class ComboTracker : MonoBehaviour
{

    public GameObject New;
    private Transform _indicator;
    private Vector3 _storedIndPos;
    private List<string> _combosDone = new List<string>();
	void Awake () {
	    _indicator = GameObject.Find("Combiner").transform.FindChild("Indicator");
    }

    public void CheckIfNew(string buildingName)
    {
        if (!_combosDone.Contains(buildingName))
        {
            _combosDone.Add(buildingName);
            Invoke("SpawnPopup", 1f);
            _storedIndPos = _indicator.position;
            // TODO: keep player updated of _combosDone.Count (out of around 30?) e.g. on the palette
        }
    }

    private void SpawnPopup()
    {
        Instantiate(New, _storedIndPos + Vector3.up * 14, Quaternion.identity);
    }
}
