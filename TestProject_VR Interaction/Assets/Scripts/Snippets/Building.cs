using System;
using UnityEngine;
using System.Collections;

[System.Serializable]
public class Building
{

    public string name;
    public float posX, posY, posZ;
    public float rotX, rotY, rotZ;

    public Building()
    {
        name = "";
        posX = posY = posZ = 0;
        rotX = rotY = rotZ = 0;
    }
    public Building(string name, Vector3 pos, Vector3 rot)
    {
        this.name = name;

        posX = pos.x;
        posY = pos.y;
        posZ = pos.z;

        rotX = rot.x;
        rotY = rot.y;
        rotZ = rot.z;
    }
}