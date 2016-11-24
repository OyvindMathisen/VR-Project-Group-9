using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Cloud : MonoBehaviour
{

	private float rng1, rng2;
	private bool reached1, reached2;
	void Awake()
	{
		rng1 = transform.position.y + UnityEngine.Random.Range(-20f, 0f);
		rng2 = transform.position.y + UnityEngine.Random.Range(0f, 20f);

		reached2 = true;
	}

	void Update()
	{
		if (!reached1)
		{
			var newPos = transform.position;
			newPos.y -= 4f * Time.deltaTime;
			transform.position = newPos;
			if (transform.position.y < rng1 + 1)
			{
				reached2 = false;
				reached1 = true;
			}
		}
		else if (!reached2)
		{
			var newPos = transform.position;
			newPos.y += 4f * Time.deltaTime;
			transform.position = newPos;
			if (transform.position.y > rng2 - 1)
			{
				reached1 = false;
				reached2 = true;
			}
		}
	}
}
