using UnityEngine;
using System.Collections;

public class ControllerAnimations : MonoBehaviour
{
    // TODO: in the lab: check if this needs to be true
    public bool Big;

    private Vector3 _scale1 = new Vector3(0.037373f, 0.01164968f, 0.01412111f); // old y: 0.01164968f
    private Vector3 _scale2 = new Vector3(0.05982443f, 0.01164968f, 0.02260421f); // old y: 0.0186481f - 0.005200639f
    private Vector3 _scaleBall1, _scaleBall2;
    private float _posBall1, _posBall2;

    private Wand _controller;
    private Transform _ball;
    private MeshRenderer _mr, _ballMR;
    private bool _posDone1, _posDone2;
    private const float SMOOTH = 0.33f;

    void Awake ()
    {
        _scale2 *= 1.1f;
        transform.localScale = _scale2;

        if (Big)
        {
            _scale1 *= 100;
            _scale2 *= 100;
            _scaleBall1 *= 100;
            _scaleBall2 *= 100;
        }

        _controller = transform.parent.parent.GetComponent<Wand>();
        _ball = transform.parent.FindChild("Ball").FindChild("ball");
        _ballMR = _ball.GetComponent<MeshRenderer>();
        _mr = transform.FindChild("circle").GetComponent<MeshRenderer>();

        _scaleBall1 = _ball.transform.localScale;
        _scaleBall2 = _scaleBall1*4;
        _posBall1 = _ball.localPosition.z;
        _posBall2 = _ball.localPosition.z*3;

        _posDone2 = true;


    }

	void Update ()
	{
        // visual animated effects for the look and feel of picking up and dropping objects with the controllers

        if (_controller.TriggerButtonPressed)
        {
            // circle scale (small)
            var newScale = transform.localScale;
            newScale.x = Mathf.Lerp(transform.localScale.x, _scale1.x, SMOOTH);
            newScale.y = Mathf.Lerp(transform.localScale.y, _scale1.y, SMOOTH);
            newScale.z = Mathf.Lerp(transform.localScale.z, _scale1.z, SMOOTH);
            transform.localScale = newScale;

            // circle color (red)
            var oldColor = _mr.material.color;
            var red = Mathf.Lerp(_mr.material.color.r, 1, SMOOTH);
            var color = new Color(red, 0, 0, oldColor.a);
            _mr.material.SetColor("_Color", color);
        }
        else
        {
            // circle scale (big)
            var newScale = transform.localScale;
            newScale.x = Mathf.Lerp(transform.localScale.x, _scale2.x, SMOOTH);
            newScale.y = Mathf.Lerp(transform.localScale.y, _scale2.y, SMOOTH);
            newScale.z = Mathf.Lerp(transform.localScale.z, _scale2.z, SMOOTH);
            transform.localScale = newScale;

            // circle color (black)
            var oldColor = _mr.material.color;
            var color = new Color(0.17f, 0.17f, 0.17f, oldColor.a);
            _mr.material.SetColor("_Color", color);
        }

        if (_controller.IsHolding)
	    {
	        if (!_posDone1)
	        {
                // ball scale (big)
                var newScaleBall = _ball.localScale;
                newScaleBall.x = Mathf.Lerp(_ball.localScale.x, _scaleBall2.x, SMOOTH);
                newScaleBall.y = Mathf.Lerp(_ball.localScale.y, _scaleBall2.y, SMOOTH);
                newScaleBall.z = Mathf.Lerp(_ball.localScale.z, _scaleBall2.z, SMOOTH);
                _ball.localScale = newScaleBall;

                // ball position (forwards)
                var newPosBall = _ball.transform.localPosition;
                newPosBall.z = Mathf.Lerp(_ball.localPosition.z, _posBall2, SMOOTH);
                _ball.transform.localPosition = newPosBall;

                // ball alpha (invisible)
                var oldColorBall = _ballMR.material.color;
                var alphaBall = Mathf.Lerp(_ballMR.material.color.a, 0, SMOOTH);
                var colorBall = new Color(oldColorBall.r, oldColorBall.g, oldColorBall.b, alphaBall);
                _ballMR.material.SetColor("_Color", colorBall);

                // for performance optimalization
                _posDone2 = false;
                if (transform.localScale.x <= _scale1.x * 0.95f) _posDone1 = true;
            }
	    }
	    else
	    {
            // wild ball rotation for visual purpose
            _ball.Rotate(_ball.transform.up, Time.deltaTime * 25);
            _ball.Rotate(_ball.transform.forward, Time.deltaTime * -25);

            if (!_posDone2)
            {
                // ball scale (small)
                var newScaleBall = _ball.localScale;
                newScaleBall.x = Mathf.Lerp(_ball.localScale.x, _scaleBall1.x, SMOOTH);
                newScaleBall.y = Mathf.Lerp(_ball.localScale.y, _scaleBall1.y, SMOOTH);
                newScaleBall.z = Mathf.Lerp(_ball.localScale.z, _scaleBall1.z, SMOOTH);
                _ball.localScale = newScaleBall;

                // ball position (backwards)
                var newPosBall = _ball.transform.localPosition;
                newPosBall.z = Mathf.Lerp(_ball.localPosition.z, _posBall1, SMOOTH);
                _ball.transform.localPosition = newPosBall;

                // ball alpha (visible)
                var oldColorBall = _ballMR.material.color;
                var alphaBall = Mathf.Lerp(_ballMR.material.color.a, 1, SMOOTH);
                var colorBall = new Color(oldColorBall.r, oldColorBall.g, oldColorBall.b, alphaBall);
                _ballMR.material.SetColor("_Color", colorBall);

                // for performance optimalization
                _posDone1 = false;
                if (transform.localScale.x >= _scale2.x * 0.95f) _posDone2 = false;
            }
        }
	}
}
