using UnityEngine;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;

public class Vegetation : MonoBehaviour
{
    private List<GameObject> _triggered = new List<GameObject>();
    private List<DragAndPlace> _tileScripts = new List<DragAndPlace>();

    private List<MeshRenderer> _childrenMR = new List<MeshRenderer>();
    private bool _fadeBack;
    private float _alpha;
    private Shader _withShadow, _withoutShadow;

    private float _addedAlpha;
    void Awake ()
	{
	    _childrenMR = transform.GetComponentsInChildren<MeshRenderer>().ToList();
        // need to change shaders because the standard shader is not showing the transparent models correctly
	    _withShadow = Shader.Find("Standard");
	    _withoutShadow = Shader.Find("Transparent/VertexLitWithZ");
	}

	void Update ()
	{
        if (_fadeBack && _alpha < 1)
        {
            SetChildrenAlpha(_alpha);
            _alpha += _addedAlpha * Time.deltaTime;
            _addedAlpha += 0.001f;
            if (_alpha > 0.8f)
            {
                _fadeBack = false;
                SetChildrenShader(true);
                _addedAlpha = 0.001f;
            }
        }
    }

    private void SetChildrenAlpha(float alpha)
    {
        foreach (var childMR in _childrenMR)
        {
            var oldColor = childMR.material.color;
            var color = new Color(oldColor.r, oldColor.g, oldColor.b, alpha);
            childMR.material.SetColor("_Color", color);
        }
    }

    private void SetChildrenShader(bool with)
    {
        foreach (var childMR in _childrenMR)
            childMR.material.shader = with ? _withShadow : _withoutShadow;
    }

    public void Hide()
    {
        CancelInvoke();
        _fadeBack = false;
        Debug.Log("hide!");
        _alpha = 0;
        SetChildrenShader(false);
        SetChildrenAlpha(_alpha);
    }

    public void Show()
    {
        CancelInvoke();
        Debug.Log("show!");
        Invoke("FadeBack", Random.Range(12f, 24f));
    }

    private void FadeBack()
    {
        _fadeBack = true;
    }
}
