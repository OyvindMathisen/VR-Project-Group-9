using UnityEngine;
using System.Collections;

public class MoveObject : MonoBehaviour {

	public GameObject HMD;
	private HMDComponents _HMDscript;
	private GameObject _rightController;
	private Wand _rightWand;

	private Vector3 _distanceToHand;

	private Quaternion _startRotation;
	private Quaternion _currentRotation;

	bool holdingObject;

	void Start () {
		_HMDscript = HMD.GetComponent<HMDComponents>();
		_rightController = _HMDscript.RightGameObject;
		_rightWand = _HMDscript.GetRightWand();
	}
	
	// Update is called once per frame
	void Update () {
		if (holdingObject) {
			transform.position = _rightController.transform.position + _distanceToHand;
			// transform.rotation = _rightController.transform.rotation;
			transform.rotation = transform.rotation * _startRotation;
		}

		if (_rightWand.TriggerButtonUp)
			holdingObject = false;
	}

	void OnTriggerStay(Collider other) {
		if (other.tag == "Rhand" && _rightWand.TriggerButtonPressed) {
			if (holdingObject)
				return;
			_distanceToHand = transform.position - _rightController.transform.position;
			_startRotation = _rightController.transform.rotation;
			holdingObject = true;
			return;
		}

		holdingObject = false;
	}
}
