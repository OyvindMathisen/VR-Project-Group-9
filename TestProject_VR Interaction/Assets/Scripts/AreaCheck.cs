using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AreaCheck : MonoBehaviour {

	public GameObject[] Checks;
	private CheckCollision[] checkCollisions = new CheckCollision[8];
	private GameObject[] buildings = new GameObject[8];
	private string[] bNames = new string[8];

	public GameObject Villa;

	void Awake()
	{
		for (var i = 0; i < 8; i++)
		{
			checkCollisions[i] = Checks[i].GetComponent<CheckCollision>();
		}
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void CheckForValidCombo(GameObject obj)
	{
		//var temp[] = new GameObject[8];
		//temp[1].transform

		for (var i = 0; i < 8; i++)
		{
		    if (checkCollisions[i].getTile() != null)
		    {
		        buildings[i] = checkCollisions[i].getTile();

		        var temp = buildings[i].name.Split('(');
		        bNames[i] = temp[0];
		    }
		}

		var objTemp = obj.name.Split('(');
		var objName = objTemp[0];

	    switch (objName)
	    {
	        case "House":
	            if (bNames[0] == "LuxuryHouse" && bNames[1] == "LuxuryHouse")
	            {
	                var tile = Instantiate(Villa, transform.position + Vector3.left * 8, Quaternion.Euler(0, 180, 0));
	                Destroy(buildings[0].gameObject);
	                Destroy(obj);
	            }
	            else if (bNames[1] == "LuxuryHouse" && bNames[2] == "LuxuryHouse")
	            {
	                var tile = Instantiate(Villa, transform.position + Vector3.forward * 8, Quaternion.Euler(0, 0, 0));
	                Destroy(buildings[1].gameObject);
	                Destroy(obj);
	            }
	            else if (bNames[2] == "LuxuryHouse" && bNames[3] == "LuxuryHouse")
	            {
	                var tile = Instantiate(Villa, transform.position + Vector3.forward * 8, Quaternion.Euler(0, 270, 0));
	                Destroy(buildings[2].gameObject);
	                Destroy(obj);
	            }
	            else if (bNames[3] == "LuxuryHouse" && bNames[4] == "LuxuryHouse")
	            {
	                var tile = Instantiate(Villa, transform.position + Vector3.right * 8, Quaternion.Euler(0, 90, 0));
	                Destroy(buildings[3].gameObject);
	                Destroy(obj);
	            }
	            else if (bNames[4] == "LuxuryHouse" && bNames[5] == "LuxuryHouse")
	            {
	                var tile = Instantiate(Villa, transform.position + Vector3.right * 8, Quaternion.Euler(0, 0, 0));
	                Destroy(buildings[4].gameObject);
	                Destroy(obj);
	            }
	            else if (bNames[5] == "LuxuryHouse" && bNames[6] == "LuxuryHouse")
	            {
	                var tile = Instantiate(Villa, transform.position + Vector3.back * 8, Quaternion.Euler(0, 180, 0));
	                Destroy(buildings[5].gameObject);
	                Destroy(obj);
	            }
	            else if (bNames[6] == "LuxuryHouse" && bNames[7] == "LuxuryHouse")
	            {
	                var tile = Instantiate(Villa, transform.position + Vector3.back * 8, Quaternion.Euler(0, 90, 0));
	                Destroy(buildings[6].gameObject);
	                Destroy(obj);
	            }
	            else if (bNames[7] == "LuxuryHouse" && bNames[0] == "LuxuryHouse")
	            {
	                var tile = Instantiate(Villa, transform.position + Vector3.left * 8, Quaternion.Euler(0, 270, 0));
	                Destroy(buildings[7].gameObject);
	                Destroy(obj);
	            }
	            break;
	        case "LuxuryHouse":

	            break;
	    }

		for (var j = 0; j < bNames.Length; j++) {
			buildings[j] = null;
			bNames[j] = "";
		}

    }
}
