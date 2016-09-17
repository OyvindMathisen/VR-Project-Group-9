using System;
using UnityEngine;

public class DragAndPlace : MonoBehaviour
{
    private float BUILD_HEIGHT = 1.25f, BUILD_HEIGHT_LERP = 0.15f;
    private bool placed = false;
    private float SNAP_VALUE = 0.16f;
    float snapInverse;
    void Awake()
    {
        snapInverse = 1 / SNAP_VALUE;
    }
    void Update()
    {
        if (!placed)
        {
            if (Input.GetMouseButtonUp(1))
            {
                transform.Rotate(0, 90, 0);
            }
            if (Input.GetMouseButtonUp(0))
            {
                // snap to ground level
                //var temp = transform.position;
                //temp.y = BUILD_HEIGHT;
                //transform.position = temp;

                placed = true;
            }
        }
    }
    void FixedUpdate()
    {
        if (!placed)
        {
            Vector3 curScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, root._screenPoint.z);
            Vector3 curPosition = Camera.main.ScreenToWorldPoint(curScreenPoint) + root._offset;
            //float currentY = Mathf.Round(curPosition.y);
            float currentX = Mathf.Round(curPosition.x * snapInverse) / snapInverse;
            float currentZ = Mathf.Round(curPosition.z * snapInverse) / snapInverse;

            //decimal currentX = Math.Round((decimal)curPosition.x, 2, MidpointRounding.AwayFromZero);
            //decimal currentZ = Math.Round((decimal)curPosition.z, 2, MidpointRounding.AwayFromZero);
            gameObject.transform.position = new Vector3(currentX, curPosition.y, currentZ);
        }
        else
        {
            if (transform.position.y != BUILD_HEIGHT)
            {
                var temp = transform.position;
                temp.y = Mathf.Lerp(transform.position.y, BUILD_HEIGHT, BUILD_HEIGHT_LERP);
                transform.position = temp;
            }
        }
    }

    void OnMouseDown()
    {
        Destroy(gameObject);
    }
}
