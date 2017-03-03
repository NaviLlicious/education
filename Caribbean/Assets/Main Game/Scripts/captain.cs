using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class captain : MonoBehaviour {

	public MMOCharController MMOCC;
	public openSlots currentSlots;

	public float CheckRadius;

	public float time;

	public float TimeToBoardShip;

	public ship currentShip;

	public Transform ShipTrans;

	public Rigidbody shipRigid;

	public bool isSailing;

	// Use this for initialization
	void Start () 
	{
		time = 2.0f;
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (Input.GetButtonDown ("Action")) 
		{
			CheckShip();
		}

		if (time < 1.0f) 
		{
			EnterShip ();
		}

		if (time < 6.0f && time > 3.0f) 
		{
			leaveWheel ();
		}

		if (isSailing) 
		{
			Sailing ();
		}
	}

	public void CheckShip ()
	{
		Collider[] hitcolliders = Physics.OverlapSphere (transform.position, CheckRadius);
		List<Collider> hitShips = new List<Collider> ();
		foreach (Collider s in hitcolliders) 
		{
			if (s.gameObject.tag == "Ship") 
			{
				hitShips.Add (s);
			}
		}

		if (hitShips.Count > 0) 
		{
			currentShip = hitShips [0].GetComponent<ship>();

			int count = currentShip.slots.Count;
			for(int i = 0; i < count; i++)
			{
				if (!currentShip.slots [i].Using) 
				{
					currentSlots = currentShip.slots [i];

					time = 0;

					TimeToBoardShip = 1.0f / currentShip.TimeToEnterShip;

					MMOCC.CanMove = false;

					ShipTrans = hitShips [i].transform;

					gameObject.layer = 1;

					shipRigid = ShipTrans.GetComponent<Rigidbody>();
					currentSlots.Using = true;

					i = count;
				}
			}
		}
	}

	public void EnterShip () 
	{
		transform.position = Vector3.Lerp (transform.position, currentSlots.place.transform.position, time);
		time += Time.deltaTime * TimeToBoardShip;

		if (time > 1.0f) 
		{
			transform.SetParent (currentSlots.place.transform);
			transform.localPosition = new Vector3 (0, 0, 0);

			currentSlots.place.transform.rotation = new Quaternion (0, 180 - transform.localRotation.y, 0, 0);

			if (currentSlots.Captain) 
			{
				isSailing = true;
			}
		}
	}

	public void leaveWheel ()
	{
		isSailing = false;
		transform.position = Vector3.Lerp (transform.position, currentSlots.place.transform.position + new Vector3 (0, 0, 0), time - 5);
		if (time < 5.0f) 
			time = 5.0f;
		time += Time.deltaTime * TimeToBoardShip;

		if (time > 6.0f) 
		{
			transform.parent = null;
			gameObject.layer = 1;
			MMOCC.CanMove = true;
			currentSlots.Using = false;
		}
	}

	public void Sailing()
	{

		if (ShipTrans.InverseTransformDirection (shipRigid.velocity).z < currentShip.MaxSpeed)
			shipRigid.AddForce (-Input.GetAxis("Vertical") * ShipTrans.forward * currentShip.speed);

		float rotmove = shipRigid.velocity.magnitude;

//		shipRigid.AddTorque (Input.GetAxis("Horizontal") * currentShip * ShipTrans.up * rotmove);

		Quaternion TargetRotation = new Quaternion ();
		TargetRotation = ShipTrans.rotation;

		if (ShipTrans.rotation.z < 180)
			TargetRotation.z = 0;
		else
			TargetRotation.z = 360;

		ShipTrans.rotation = Quaternion.Slerp (ShipTrans.rotation, TargetRotation, Time.smoothDeltaTime * 2.0f);
			
		if (Input.GetButtonDown ("Action")) 
			time = 5.0f;
	}
}
