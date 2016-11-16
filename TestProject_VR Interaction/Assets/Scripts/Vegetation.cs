using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class Vegetation : MonoBehaviour
{
    public List<GameObject> _triggered = new List<GameObject>();
    public List<DragAndPlace> _tileScripts = new List<DragAndPlace>();
    public bool placed;

    public List<MeshRenderer> _childrenMR = new List<MeshRenderer>();
    public bool _fadeBack;
    public float _alpha;
	void Awake ()
	{
	    _childrenMR = transform.GetComponentsInChildren<MeshRenderer>().ToList();
	}

	void Update ()
	{
	    if (!placed)
	    {
	        if (_tileScripts.Count == 0) return;
            foreach (var script in _tileScripts)
	        {
	            if (script.Placed)
	            {
                    _alpha = 0;
                    SetChildrenAlpha();
                    _fadeBack = false;
                    placed = true;
	                break;
	            }
	        }
	    }

	    if (_fadeBack && _alpha < 1)
	    {
	        SetChildrenAlpha();
	        _alpha += 0.1f*Time.deltaTime;
	    }
	}

    private void SetChildrenAlpha()
    {
        foreach (var childMR in _childrenMR)
        {
            var oldColor = childMR.material.color;
            var color = new Color(oldColor.r, oldColor.g, oldColor.b, _alpha);
            childMR.material.SetColor("_Color", color);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag != "Tile") return;
        var script = other.transform.parent.GetComponent<DragAndPlace>();
        if (!script) return;
        if (!_triggered.Contains(other.gameObject))
        {
            _triggered.Add(other.gameObject);
            _tileScripts.Add(script);
        }

        placed = false;
        CancelInvoke();
    }
    void OnTriggerExit(Collider other)
    {
        if (other.tag != "Tile") return;
        var script = other.transform.parent.GetComponent<DragAndPlace>();
        if (!script) return;

        _triggered.Remove(other.gameObject);
        _tileScripts.Remove(script);
        Invoke("FadeBack", Random.Range(12f, 24f));
    }

    private void FadeBack()
    {
        _fadeBack = true;
    }
}
