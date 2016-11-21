using UnityEngine;
using System.Collections.Generic;


public class Combiner : MonoBehaviour
{

	public List<GameObject> Alternatives = new List<GameObject>();
	public List<string> Names = new List<string>();
	public List<int> I = new List<int>();
	public List<List<GameObject>> RelevantBuildings = new List<List<GameObject>>();

	public GameObject LastPlacedTile;

	public GameObject RelevantTile;

	private Indicator _indicator;
	private Transform _timer, _wall, _wrap, _previewPlacement;
	private TextMesh _name, _number, _name2, _number2;
	public float time, next;
	private float nextDelay;
	private float _timerWidth;
	private const float CHAR_WIDTH = 0.6f;
	private const float INDICATOR_Y_ADJUST = 8f;
	private List<GameObject> _temporaryDeleteList = new List<GameObject>();

	public bool onceNewAlts;

	void Awake()
	{
		_indicator = transform.FindChild("Indicator").GetComponent<Indicator>();
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

	// Update is called once per frame
	private void Update()
	{
		if (!_indicator.Dragging)
			time += Time.deltaTime;

		if (Alternatives.Count > 0)
		{
			// when a tile has been placed which has given a new opportunity for combination alternatives
			if (!onceNewAlts)
			{
				// placing indicator (to know what's about to get combined)
				_indicator.gameObject.SetActive(true);
				NewIndicatorPos();
				UpdateUI();

				var sfx = _previewPlacement.FindChild("sfxNewCombo").GetComponent<AudioSource>();
				sfx.pitch = 0.05f * (RelevantBuildings[0].Count - 2) + 1.0f;
				sfx.Play();

				onceNewAlts = true;
			}

			if (time > next)
			{
				// time's up - cancel process
				Cancel();
				PlayCancelSound();
			}
			else
			{
				// time left display
				var newScale = _timer.localScale;
				newScale.x = _timerWidth * ((next - time) / nextDelay);
				_timer.localScale = newScale;
			}
		}
		else
		{
			next = time + nextDelay;
		}
	}

	public void UpdateUI()
	{
		var altCount = Alternatives.Count - 1;
		var newScale = _wall.localScale;
		if (altCount >= 0)
		{
			_name.text = Names[0];
			newScale.x = Names[0].Length * CHAR_WIDTH;
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
		RelevantBuildings.Clear();

		CleanWrap();
		_indicator.gameObject.SetActive(false);

		onceNewAlts = false;
	}

	public void PlayCancelSound()
	{
		var sfx = _previewPlacement.FindChild("sfxCancel").GetComponent<AudioSource>();
		sfx.pitch = UnityEngine.Random.Range(0.8f, 0.9f);
		sfx.Play();
	}

	private void CleanWrap()
	{
		foreach (Transform child in _wrap)
		{
			if (child.gameObject.name.StartsWith("Relevant"))
				Destroy(child.gameObject);
		}
	}

	private void NewIndicatorPos()
	{
		var newPos = _indicator.transform.position;
		newPos.x = Alternatives[0].transform.position.x;
		newPos.z = Alternatives[0].transform.position.z;

		// if the tile have a high building
		newPos.y = GameSettings.BUILD_HEIGHT +
				   (Alternatives[0].transform.FindChild("Collider1").GetComponent<BoxCollider>().size.y * GameSettings.SNAP_VALUE) + INDICATOR_Y_ADJUST;

		_indicator.transform.position = newPos;

		// for lerping back to where it was if releasing before reaching threshold
		_indicator.ThisStartPosY = _indicator.transform.position.y;
	}

	public void NextAlternative()
	{
		if (Alternatives.Count > 1)
		{
			// canceling when there is still combination alternatives left
			Alternatives.RemoveAt(0);
			Names.RemoveAt(0);
			I.RemoveAt(0);
			RelevantBuildings.RemoveAt(0);

			next = time + nextDelay;

			NewIndicatorPos();
			UpdateUI();

			var sfx = _previewPlacement.FindChild("sfxUI0").GetComponent<AudioSource>();
			sfx.pitch = UnityEngine.Random.Range(0.9f, 1.2f);
			sfx.Play();
		}
		else
		{
			Cancel();
			PlayCancelSound();
		}
	}

	public void Combine()
	{
		Alternatives[0].gameObject.SendMessage("CheckForCombos", I[0]);
		Cancel();

		var sfx = _previewPlacement.FindChild("sfxCombine").GetComponent<AudioSource>();
		sfx.pitch = UnityEngine.Random.Range(1f, 1.2f);
		sfx.Play();
	}

	public void DeletePredecessors(List<GameObject> list)
	{
		foreach (var obj in list)
		{
			obj.transform.position = new Vector3(-500, -500, -500);

		}
		_temporaryDeleteList = list;
		Invoke("DestroyPredecessors", 0.05f);
	}

	private void DestroyPredecessors()
	{
		foreach (var obj in _temporaryDeleteList)
		{
			Destroy(obj);
		}
	}
}
