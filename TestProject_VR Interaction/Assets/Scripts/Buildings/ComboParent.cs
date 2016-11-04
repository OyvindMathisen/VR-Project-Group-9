using UnityEngine;
using System.Collections.Generic;

public abstract class ComboParent : DragAndPlace
{
    // tile size
    private float _xSize;
    private float _zSize;
    protected LayerMask _tiles;

    // raycast positions (valid tiles for the combo) from "main" tile (transform.position), where "1" is 8 units
    public int[] xPos;
    public int[] zPos;

    // Instantiated building position and rotation adjustment
    protected int[] xAdj;
    protected int[] zAdj;
    protected int[] rotAdj;

    protected List<ComboParent> neighbours;

    // Use this for initialization
    protected override void Awake()
    {
        base.Awake();
        neighbours = new List<ComboParent>(); // List of the nightbours around and their ComboParent script.
        _xSize = _zSize = GameSettings.SNAP_VALUE;
    }
    protected override void OnPlaced()
    {
        base.OnPlaced();

        /*
         * Check the tiles around the building if there is a valid combination.
         * If there is no combo to make, tell the buildings to check if they can make
         * a combination now.
         */
        if (CheckSurroundingTiles()) return;
        AlertSurroundingTiles();
    }
    protected override void Update()
    {
        base.Update();

        // Debug to check if raycasting has been done correctly.
        for (var i = 0; i < xPos.Length; i++)
        {
            Debug.DrawLine(transform.position + transform.right * _xSize * xPos[i] + new Vector3(0, 10, 0) + transform.forward * _zSize * zPos[i], new Vector3(1, 0, 0), Color.red);
        }
    }
    public virtual bool CheckSurroundingTiles()
    {
        neighbours.Clear(); // Reset list to avoid lingering "ghost nightbours".
        for (var i = 0; i < xPos.Length; i++)
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position + transform.right * _xSize * xPos[i] + new Vector3(0, 10, 0) + transform.forward * _zSize * zPos[i], Vector3.down, out hit, 10, _tiles))
            {
                var componentHit = hit.transform.GetComponent<ComboParent>();
                if(componentHit != null && componentHit.Placed) // Change Placed to hasLanded or equivelent.
                {
                    var comboResult = MakeCombo(hit.transform.name); // Checks for valid combo on the name
                    if(comboResult != null) // Instansiate the combo if the MakeCombo returns a valid gameObject.
                    {
                        Instantiate(comboResult, transform.position + transform.right * _xSize * xAdj[i] + transform.forward * _zSize * zAdj[i], Quaternion.Euler(0, transform.localEulerAngles.y + rotAdj[i], 0));
                        Destroy(hit.transform.gameObject);
                        Destroy(gameObject);
                        return true;
                    }
                    else // No combo found? Add the building to our list of nightbours to warn.
                    {
                        neighbours.Add(componentHit);
                    }
                }
            }
        }
        return false;
    }

    private void AlertSurroundingTiles()
    {
        // Tell all the surrounding, and valid tiles, to check if they can make a combo now.
        foreach(ComboParent neighbour in neighbours)
        {
            neighbour.CheckSurroundingTiles();
        }
    }
    protected abstract GameObject MakeCombo(string buildingName); // Abstract method that needs to be implemented in the spesific building script.
}
