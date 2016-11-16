using UnityEngine;
using System.Collections;

public class Indicator : MonoBehaviour
{
    public bool Dragging;
    public float ThisStartPosY;

    private const float THRESHOLD_UP = 3, THRESHOLD_DOWN = -3;

    private Combiner _combiner;
    private Wand _controllerR, _controllerL;
    private MeshRenderer _arrowMR;
    private Transform _arrowMesh;

    private bool _stayR, _stayL;
    private bool _rNotL, _resetPos;
    private float _ctrlBeginPosY, _thisBeginPosY;
    private float _arrowWidth;

    void Awake () {
        _combiner = transform.parent.GetComponent<Combiner>();

        _arrowMesh = transform.FindChild("Arrow").transform.FindChild("Mesh");
        _arrowMR = _arrowMesh.GetComponent<MeshRenderer>();
        _arrowWidth = _arrowMesh.localScale.y;
    }

    void Start()
    {
        _controllerR = HMDComponents.getRightWand();
        _controllerL = HMDComponents.getLeftWand();
    }

    void Update()
    {
        if (_stayR && _controllerR.TriggerButtonDown)
        {
            _ctrlBeginPosY = _controllerR.transform.position.y;
            _thisBeginPosY = transform.position.y;
            _rNotL = true;
            Dragging = true;
        }
        else if (_stayL && _controllerL.TriggerButtonDown)
        {
            _ctrlBeginPosY = _controllerL.transform.position.y;
            _thisBeginPosY = transform.position.y;
            _rNotL = false;
            Dragging = true;
        }  
        if (_controllerR.TriggerButtonUp || _controllerL.TriggerButtonUp)
        {
            if (Dragging)
            {
                _resetPos = true;
                Dragging = false;
            }
        }

        // when dragging the indicator up or down to decide what to do
        if (Dragging)
        {
            var currentY = (_rNotL ? _controllerR.transform.position.y
                : _controllerL.transform.position.y) - _ctrlBeginPosY;

            var newPos = transform.position;
            newPos.y = _thisBeginPosY + currentY;
            transform.position = newPos;

            if (transform.position.y > ThisStartPosY)
            {
                var oldColor = _arrowMR.material.color;
                var alpha = 1 - ((transform.position.y - ThisStartPosY) / THRESHOLD_UP);
                var color = new Color(oldColor.r, oldColor.g, oldColor.b, alpha);
               _arrowMR.material.SetColor("_Color", color);
            }
            else
            {
                var newScale = _arrowMesh.localScale;
                var percent = ((transform.position.y - ThisStartPosY) / THRESHOLD_DOWN);
                newScale.y = _arrowWidth + 0.45f*percent;
                newScale.z = _arrowWidth + 0.9f * percent;
                _arrowMesh.localScale = newScale;
            }

            if (transform.position.y > ThisStartPosY + THRESHOLD_UP)
            {
                var oldColor = _arrowMR.material.color;
                var color = new Color(oldColor.r, oldColor.g, oldColor.b, 1);
                _arrowMR.material.SetColor("_Color", color);

                _combiner.NextAlternative();
                _resetPos = true;
                Dragging = false;
            }
            else if (transform.position.y < ThisStartPosY + THRESHOLD_DOWN)
            {
                var newScale = _arrowMesh.localScale;
                newScale.y = _arrowWidth;
                newScale.z = _arrowWidth;
                _arrowMesh.localScale = newScale;

                _combiner.Combine();
                Dragging = false;
            }
        }
        else
        {
            // TODO: atm it's just rotating, but we could make it so it's always facing camera (if that's better, we don't know yet)
            transform.Rotate(0, 160 * Time.deltaTime, 0);

            // lerping height back to normal
            if (_resetPos)
            {
                var newPos2 = transform.position;
                newPos2.y = Mathf.Lerp(transform.position.y, ThisStartPosY, 0.022f);
                transform.position = newPos2;
                if ((transform.position.y < ThisStartPosY + 0.001f)
                    && (transform.position.y > ThisStartPosY - 0.001f))
                {
                    _resetPos = false;
                }
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Rhand")
            _stayR = true;

        if (other.tag == "Lhand")
            _stayL = true;
    }

    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Rhand")
            _stayR = false;

        if (other.tag == "Lhand")
            _stayL = false;
    }
}
