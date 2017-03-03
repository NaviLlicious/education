using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ship : MonoBehaviour {

	public List<openSlots> slots = new List<openSlots> ();

	public float speed;
	public float MaxSpeed;

	public float TimeToEnterShip = 1.0f;

	public float rotationSpeed;

	// Use this for initialization
	void Start () 
	{
		
	}
	
	// Update is called once per frame
	void Update () 
	{
		
	}
}

[System.Serializable]
public class openSlots
{
	public string name;
	public GameObject place;
	public bool Captain;
	public bool Using;
}
