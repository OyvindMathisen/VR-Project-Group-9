using UnityEngine;
using System.Collections.Generic;

public class Popup : MonoBehaviour
{
    public GameObject SfxPop;

    private Wand _controllerR, _controllerL;
    private List<GameObject> _triggered = new List<GameObject>();

    private float _yAdd, _spinValue;
    private Transform _cam;
    float _timeDisappear, _timer;
    
	void Awake ()
	{
        _cam = GameObject.FindWithTag("MainCamera").transform;
        _yAdd = 4f;
	    _spinValue = 30f;
        _timeDisappear = 4f;
	}

    void Start()
    {
        _controllerR = HMDComponents.GetRightWand();
        _controllerL = HMDComponents.GetLeftWand();
    }
	
	void Update ()
	{
        _timer += Time.deltaTime;
        // rotate faster and faster
        foreach (Transform child in transform) // For one child
        {
            child.Rotate(0, 0, -_spinValue * Time.deltaTime);
        }
        // transform.Rotate(0, 0, -_spinValue * Time.deltaTime);
	    _spinValue += _spinValue * Time.deltaTime;

        // go upwards
        var temp = transform.position;
	    temp.y += Time.deltaTime*_yAdd;
	    transform.position = temp;

        // disappear when reached height with smoke and sfx
	    if (_timer > _timeDisappear)
	    {
            var fx = Instantiate(Resources.Load("FogExplosion", typeof(GameObject)), transform.position, Quaternion.identity) as GameObject;
	        if (fx != null)
	        {
                foreach (var ps in fx.GetComponentsInChildren<ParticleSystem>())
                    ps.Play();
            }

            Instantiate(SfxPop, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }

        // if grabbing this; poof
	    if (_triggered.Count > 0)
            if (_controllerR.TriggerButtonDown || _controllerL.TriggerButtonDown)
                _timer += _timeDisappear;

    }
    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Rhand" || other.tag == "Lhand")
        {
            _triggered.Add(other.gameObject);
        }
    }
    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Rhand" || other.tag == "Lhand")
        {
            _triggered.Remove(other.gameObject);
        }
    }
}
