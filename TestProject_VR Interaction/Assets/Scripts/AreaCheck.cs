using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AreaCheck : MonoBehaviour {

	public GameObject[] checks;
	private CheckCollision[] checkCollisions = new CheckCollision[8];
	private GameObject[] buildings = new GameObject[8];
	private string[] bNames = new string[8];

	public GameObject Villa;

	void Awake()
	{
		for (int i = 0; i < 8; i++)
		{
			checkCollisions[i] = checks[i].GetComponent<CheckCollision>();
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

		for (int i = 0; i < 8; i++)
		{
			if (checkCollisions[i].getTile() != null)
			{
				buildings[i] = checkCollisions[i].getTile();

				string[] temp = buildings[i].name.Split('(');
				string name = temp[0];
				bNames[i] = name;
			}
		}

		string[] objTemp = obj.name.Split('(');
		string objName = objTemp[0];

		switch (objName)
		{
			case "House":
				if (bNames[0] == "LuxuryHouse" && bNames[1] == "LuxuryHouse")
				{
					GameObject tile = Instantiate(Villa, transform.position, Quaternion.Euler(0, 0, 0)) as GameObject;
					Destroy(buildings[0].gameObject);
					Destroy(obj);
					Debug.Log("This worked0");
				}
				else if (bNames[1] == "LuxuryHouse" && bNames[2] == "LuxuryHouse")
				{
					GameObject tile = Instantiate(Villa, transform.position, Quaternion.Euler(0, 90, 0)) as GameObject;
					Destroy(buildings[1].gameObject);
					Destroy(obj);
					Debug.Log("This worked1");
				}
				else if (bNames[2] == "LuxuryHouse" && bNames[3] == "LuxuryHouse")
				{
					GameObject tile = Instantiate(Villa, transform.position, Quaternion.Euler(0, 180, 0)) as GameObject;
					Destroy(buildings[2].gameObject);
					Destroy(obj);
					Debug.Log("This worked2");
				}
				else if (bNames[3] == "LuxuryHouse" && bNames[4] == "LuxuryHouse")
				{
					GameObject tile = Instantiate(Villa, transform.position, Quaternion.Euler(0, 270, 0)) as GameObject;
					Destroy(buildings[3].gameObject);
					Destroy(obj);
					Debug.Log("This worked3");
				}
				else if (bNames[4] == "LuxuryHouse" && bNames[5] == "LuxuryHouse")
				{
					GameObject tile = Instantiate(Villa, transform.position, Quaternion.Euler(0, 270, 0)) as GameObject;
					Destroy(buildings[4].gameObject);
					Destroy(obj);
					Debug.Log("This worked4");
				}
				else if (bNames[5] == "LuxuryHouse" && bNames[6] == "LuxuryHouse")
				{
					GameObject tile = Instantiate(Villa, transform.position, Quaternion.Euler(0, 270, 0)) as GameObject;
					Destroy(buildings[5].gameObject);
					Destroy(obj);
					Debug.Log("This worked5");
				}
				else if (bNames[6] == "LuxuryHouse" && bNames[7] == "LuxuryHouse")
				{
					GameObject tile = Instantiate(Villa, transform.position, Quaternion.Euler(0, 270, 0)) as GameObject;
					Destroy(buildings[6].gameObject);
					Destroy(obj);
					Debug.Log("This worked6");
				}
				else if (bNames[7] == "LuxuryHouse" && bNames[0] == "LuxuryHouse")
				{
					GameObject tile = Instantiate(Villa, transform.position, Quaternion.Euler(0, 270, 0)) as GameObject;
					Destroy(buildings[7].gameObject);
					Destroy(obj);
					Debug.Log("This worked7");
				}
				break;
			case "LuxuryHouse":

				break;
		}

		for (int j = 0; j < bNames.Length; j++) {
			buildings[j] = null;
			bNames[j] = "";
		}

	}
}
