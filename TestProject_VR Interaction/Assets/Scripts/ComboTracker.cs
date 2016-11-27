using UnityEngine;
using System.Collections.Generic;

public class ComboTracker : MonoBehaviour
{

    public GameObject New;
    private Transform _indicator;
    private Vector3 _storedIndPos;
    public List<string> CombosDone = new List<string>();
    private TextMesh _comboCount;
	void Awake () {
	    _indicator = transform.FindChild("Indicator");
	    _comboCount = GameObject.Find("MainGameObjects").transform.FindChild("ComboScreen/ComboCount/Count").GetComponent<TextMesh>();
	    UpdateCount();
	}

    public void CheckIfNew(string buildingName)
    {
        if (!CombosDone.Contains(buildingName))
        {
            CombosDone.Add(buildingName);
            Invoke("SpawnPopup", 1f);
            _storedIndPos = _indicator.position;

            UpdateCount();

            GameFile.current.combosDone = CombosDone.ToArray();
        }
    }

    private void SpawnPopup()
    {
        Instantiate(New, _storedIndPos + Vector3.up * 14, Quaternion.identity);
    }

    public void UpdateCount()
    {
        // keeping player updated of how many combos are "found"
        _comboCount.text = CombosDone.Count + "/" + GameSettings.TOTAL_COMBO_COUNT;
    }
}
