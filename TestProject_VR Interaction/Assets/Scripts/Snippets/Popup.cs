using UnityEngine;

public class Popup : MonoBehaviour
{
    public GameObject SfxPop;
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
            
	}
}
