using System;
using UnityEngine;

public class DragWorld : MonoBehaviour
{
    private bool placed = false; // if the tile is still "dragged" around (the mouse button is not released yet)
    private Wand Rhand;
    private Collider ownCollider;

    private Vector3 referencePoint;
    private Vector3 movementForce;

    private Vector3 temp;

    void Awake()
    {
        Rhand = GameObject.Find("[CameraRig]").transform.FindChild("Controller (right)").GetComponent<Wand>();

        ownCollider = GetComponent<Collider>();
    }
    void Update()
    {
        if (Rhand.triggerButtonDown)
        {
            referencePoint = Rhand.transform.position;
        }

        if (Rhand.triggerButtonPressed)
        {
            movementForce = Rhand.transform.position - referencePoint;

            temp.z += movementForce.z/24;
            temp.x += movementForce.x/24;

            transform.position = temp;
        }
    }
}
