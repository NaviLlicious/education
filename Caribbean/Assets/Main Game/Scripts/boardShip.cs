using UnityEngine;
using System.Collections;
using UnityStandardAssets.Vehicles.Car;

public class boardShip : MonoBehaviour
{
	private bool isSailing = false;
	CarUserControl sailingScript;
	public GameObject guiObj;
	GameObject player;


	void Start()
	{
		sailingScript = GetComponent<CarUserControl>();
		player = GameObject.FindWithTag("Player");
		guiObj.SetActive(false);
	}

	// Update is called once per frame
	void OnTriggerStay(Collider other)
	{
		if (other.gameObject.tag == "Player" && isSailing == false)
		{
			guiObj.SetActive(true);
			if (Input.GetButtonDown("Action"))
			{
				guiObj.SetActive(false);
				player.transform.parent = gameObject.transform;              
				sailingScript.enabled = true;
				player.SetActive(false);
				isSailing = true;
			}
		}
	}

	void OnTriggerExit(Collider other)
	{
		if (other.gameObject.tag == "Player")
		{
			guiObj.SetActive(false);
		}
	}

	void Update()
	{
		if (isSailing == true && Input.GetButtonDown("Action"))
		{
			sailingScript.enabled = false;
			player.SetActive(true);
			player.transform.parent = null;
			isSailing = false;
		}
	}
}