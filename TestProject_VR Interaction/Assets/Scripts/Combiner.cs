using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System;


public class Combiner : MonoBehaviour
{

    public List<GameObject> Alternatives = new List<GameObject>();
    public List<string> Names = new List<string>();
    public List<int> I = new List<int>();
    public List<List<GameObject>> RelevantBuildings = new List<List<GameObject>>();

    public GameObject LastPlacedTile;

    public GameObject RelevantTile;

    public List<GameObject> Triggered = new List<GameObject>();

    private Transform _indicator, _timer, _wall, _wrap, _previewPlacement;
    private TextMesh _name, _number, _name2, _number2;
    public float time, next;
    private float nextDelay;
    private float _timerWidth;
    private const float CharacterWidth = 0.6f;

    public bool onceNewAlts, onceNoAlts;

    private GameObject _rightController;
    private Wand _controller;

    // only temporary for developing on pc (so GripButtonDown can be triggered once)
    private bool tempReset;

    private const float INDICATOR_HEIGHT = 110f;
    void Awake()
    {
        _indicator = transform.FindChild("Indicator");
        _indicator.gameObject.SetActive(false);
        _timer = _indicator.transform.FindChild("Timer");
        _wall = _indicator.transform.FindChild("Wall");
        _name = _indicator.transform.FindChild("Name").GetComponent<TextMesh>();
        _name2 = _indicator.transform.FindChild("Name2").GetComponent<TextMesh>();
        _number = _indicator.transform.FindChild("Number").GetComponent<TextMesh>();
        _number2 = _indicator.transform.FindChild("Number2").GetComponent<TextMesh>();
        _wrap = transform.FindChild("Wrap");
        _previewPlacement = GameObject.FindWithTag("PreviewPlacement").transform;

        _timerWidth = _timer.localScale.x;
        nextDelay = GameSettings.COMBO_DECISION_TIME;
    }

	void Start() {
		_rightController = HMDComponents.getRightController();
		_controller = HMDComponents.getRightWand();
	}

    // Update is called once per frame
    private void Update()
    {
        // TODO: atm it's just rotating, but we could make it so it's always facing camera (if that's better, we don't know yet)
        _indicator.Rotate(0, 160 * Time.deltaTime, 0);
        
        time += Time.deltaTime;

        // if the tile have a high building
        if (Triggered.Count > 0)
        {
            var newPos = _indicator.transform.position;
            newPos.y = Mathf.Lerp(_indicator.transform.position.y, _indicator.transform.position.y + 3, 0.05f);
            _indicator.transform.position = newPos;
        }

        if (Alternatives.Count > 0)
        {
            // when a tile has been placed which has given a new opportunity for combination alternatives
            if (!onceNewAlts)
            {
                // placing indicator (to know what's about to get combined)
                _indicator.gameObject.SetActive(true);
                NewIndPos();

                UpdateUI();

                var sfx = _previewPlacement.FindChild("sfxNewCombo").GetComponent<AudioSource>();
                sfx.pitch = 0.05f * (RelevantBuildings[0].Count-2) + 1.0f;
                sfx.Play();

                onceNoAlts = false;
                onceNewAlts = true;
            }

            if (time > next)
            {
                // time's up - the combination have been decided
                Alternatives[0].gameObject.SendMessage("CheckForCombos", I[0]);
                Alternatives.Clear();
                Names.Clear();
                I.Clear();
                RelevantBuildings.Clear();
                CleanWrap();

                var sfx = _previewPlacement.FindChild("sfxCombine").GetComponent<AudioSource>();
                sfx.pitch = UnityEngine.Random.Range(1f, 1.2f);
                sfx.Play();
            }
            else
            {
                // time left display
                var newScale = _timer.localScale;
                newScale.x = _timerWidth * ((next - time) / nextDelay);
                _timer.localScale = newScale;
            }

            if (_controller.GripButtonDown && Alternatives.Count > 1)
            {
                // canceling when there is still combination alternatives left
                if (!tempReset)
                {
                    Alternatives.RemoveAt(0);
                    Names.RemoveAt(0);
                    I.RemoveAt(0);
                    RelevantBuildings.RemoveAt(0);

                    next = time + nextDelay;
                    NewIndPos();
                    UpdateUI();
                    tempReset = true;

                    var sfx = _previewPlacement.FindChild("sfxUI1").GetComponent<AudioSource>();
                    sfx.pitch = UnityEngine.Random.Range(0.9f, 1.2f);
                    sfx.Play();
                }
            }
            else if (_controller.GripButtonDown)
            {
                // the last alternative was canceled
                if (!tempReset)
                {
                    Alternatives.RemoveAt(0);
                    Names.RemoveAt(0);
                    I.RemoveAt(0);
                    RelevantBuildings.RemoveAt(0);

                    UpdateUI();
                    tempReset = true;

                    var sfx = _previewPlacement.FindChild("sfxCancel").GetComponent<AudioSource>();
                    sfx.pitch = UnityEngine.Random.Range(0.8f, 1.0f);
                    sfx.Play();
                }
            }

            if (_controller.GripButtonUp)
            {
                tempReset = false;
            }
        }
        else
        {
            next = time + nextDelay;
            if (!onceNoAlts)
            {
                _indicator.gameObject.SetActive(false);

                onceNewAlts = false;
                onceNoAlts = true;
            }
        }
    }

    private void NewIndPos()
    {
        var newPos = _indicator.position;
        newPos.x = Alternatives[0].transform.position.x;
        newPos.z = Alternatives[0].transform.position.z;
        _indicator.position = newPos;
    }

    private void UpdateUI()
    {
        var altCount = Alternatives.Count - 1;
        var newScale = _wall.localScale;
        if (altCount >= 0)
        {
            _name.text = Names[0];
            newScale.x = Names[0].Length * CharacterWidth;
            _number.text = "" + altCount;
        }
        else
        {
            _name.text = "";
        }
        if (altCount == 0) _number.text = "";

        _name2.text = _name.text;
        _number2.text = _number.text;

        _wall.localScale = newScale;

        var temp = _indicator.localPosition;
        temp.y = INDICATOR_HEIGHT;
        _indicator.localPosition = temp;

        // showing new relevant tiles for current combo alternative
        CleanWrap();

        if (RelevantBuildings.Count <= 0) return;
        foreach (var obj in RelevantBuildings[0])
        {
            foreach (Transform child in obj.transform)
            {
                if (child.name.StartsWith("Collider"))
                {
                    var newObj = Instantiate(RelevantTile);
                    newObj.transform.parent = _wrap;
                    newObj.transform.localScale = Vector3.one;
                    newObj.transform.position = new Vector3(child.transform.position.x,
                        GameSettings.BUILD_HEIGHT + GameSettings.PREVIEW_HEIGHT_ADJUST, child.transform.position.z);
                }
            }
        }
    }

    public void Cancel()
    {
        Alternatives.Clear();
        Names.Clear();
        I.Clear();
        onceNoAlts = false;
        onceNewAlts = false;

        _indicator.gameObject.SetActive(false);
        var temp = _indicator.localPosition;
        temp.y = INDICATOR_HEIGHT;
        _indicator.localPosition = temp;

        CleanWrap();
    }

    private void CleanWrap()
    {
        foreach (Transform child in _wrap)
        {
            if (child.gameObject.name.StartsWith("Relevant"))
                Destroy(child.gameObject);
        }
    }

    
}
