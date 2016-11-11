using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System;


public class Combiner : MonoBehaviour
{

    public List<GameObject> Alternatives = new List<GameObject>();
    public List<string> Names = new List<string>();

    public GameObject LastPlacedTile;

    private Transform _indicator, _timer, _wall;
    private TextMesh _name, _number, _name2, _number2;
    public float time, next;
    private const float nextDelay = 2.5f;
    private float _timerWidth;
    private const float CharacterWidth = 0.6f;

    public bool onceNewAlts, onceNoAlts;

    private GameObject _rightController;
    private Wand _controller;

    // only temporary for developing on pc (so GripButtonDown can be triggered once)
    private bool tempReset;

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

        _timerWidth = _timer.localScale.x;

    }

	void Start() {
		_rightController = HMDComponents.getRightController();
		_controller = HMDComponents.getRightWand();
	}

    // Update is called once per frame
    private void Update()
    {
        _indicator.Rotate(0, 160 * Time.deltaTime, 0);
        // TODO: erstatt med _indicator.LookAt(camera); med temp slik at kun y verdien.
        time += Time.deltaTime;

        if (Alternatives.Count > 0)
        {
            // when a tile has been placed which has given a new opportunity for combination alternatives
            if (!onceNewAlts)
            {
                Debug.Log("new combos!");

                // placing indicator (to know what's about to get combined)
                _indicator.gameObject.SetActive(true);
                NewIndPos();

                UpdateWall();

                onceNoAlts = false;
                onceNewAlts = true;
            }

            if (time > next)
            {
                Alternatives[0].gameObject.SendMessage("CheckForCombos", true);
                Alternatives.Clear();
                Names.Clear();
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
                if (!tempReset)
                {
                    Alternatives.RemoveAt(0);
                    Names.RemoveAt(0);

                    next = time + nextDelay;
                    NewIndPos();
                    UpdateWall();
                    tempReset = true;
                }
            }
            else if (_controller.GripButtonDown)
            {
                if (!tempReset)
                {
                    Alternatives.RemoveAt(0);
                    Names.RemoveAt(0);

                    UpdateWall();
                    tempReset = true;
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

    private void UpdateWall()
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
    }

    public void Cancel()
    {
        Alternatives.Clear();
        Names.Clear();
        onceNoAlts = false;
        onceNewAlts = false;
        _indicator.gameObject.SetActive(false);
    }
}
