using UnityEngine;
using System.Collections.Generic;

public class ComboTracker : MonoBehaviour
{

    public GameObject New, MainGameObjects;
    public Material Lava;

    private Transform _indicator, _water;
    private Vector3 _storedIndPos;
    public List<string> CombosDone = new List<string>();
    private TextMesh _comboCount;
	void Awake ()
    {
	    _indicator = transform.FindChild("Indicator");
	    _comboCount = MainGameObjects.transform.FindChild("ComboScreen/ComboCount/Count").GetComponent<TextMesh>();
        _water = GameObject.Find("Island").transform.FindChild("Water");
    }

    void Start ()
    {
        UpdateCount();
        if (CombosDone.Count >= GameSettings.TOTAL_COMBO_COUNT)
            AllCombosDone();
    }

    public void CheckIfNew(string buildingName)
    {
        if (!CombosDone.Contains(buildingName))
        {
            CombosDone.Add(buildingName);
            Invoke("SpawnPopup", 1f);
            _storedIndPos = _indicator.position;

            UpdateCount();

            if (CombosDone.Count == GameSettings.TOTAL_COMBO_COUNT)
            {
                Invoke("AchievementGet", 2.75f);
                Invoke("AllCombosDone", 2.75f);
            }

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

    private void AllCombosDone()
    {
        // when all combos have been found/done, change water to lava
        _water.GetComponent<MeshRenderer>().material = Lava;
        var waterAnims = _water.parent.FindChild("WaterAnimation").GetComponentsInChildren<SpriteRenderer>();
        _water.parent.FindChild("WaterfallSteam/sfxWaterfall").GetComponent<AudioSource>().Stop();
        foreach (var anim in waterAnims) anim.enabled = false;
    }

    private void AchievementGet()
    {
        GetComponent<AudioSource>().Play();
        _water.FindChild("Congratulations").GetComponent<TextMesh>().text = "CONGRATULATIONS!\nALL COMBOS DONE";
    }
}
