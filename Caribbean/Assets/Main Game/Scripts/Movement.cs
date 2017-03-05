using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(CapsuleCollider))]

public class Movement : MonoBehaviour {

	Animator anim;

	bool isWalking = false;
	bool isJumping = true;

	const float walkSpeed = .25f;

	void Awake() 
	{
		anim = GetComponent<Animator> ();
	}

	// Use this for initialization
	void Start () 
	{
		
	}
	
	// Update is called once per frame
	void Update () 
	{
		Walking ();	
		Turning ();
		Running();
		Jump ();
	}

	void Turning()
	{
		anim.SetFloat ("Turning", Input.GetAxis ("Horizontal"));
	}

	void Walking()
	{
		if (Input.GetKeyDown (KeyCode.Backslash)) 
		{
			//Toggle Walk
			isWalking = !isWalking;
			anim.SetBool ("Walk", isWalking);
		}
	}

	void Running()
	{
		if (anim.GetBool ("Walk"))
		{
			anim.SetFloat ("MoveX", Mathf.Clamp (Input.GetAxis ("MoveX"), -walkSpeed, walkSpeed));
			anim.SetFloat ("MoveZ", Mathf.Clamp (Input.GetAxis ("MoveZ"), -walkSpeed, walkSpeed));
		} 
		else 
		{
			anim.SetFloat ("MoveZ", Input.GetAxis ("MoveZ"));
			anim.SetFloat ("MoveX", Input.GetAxis ("MoveX"));
		}
	}

	void Jump()
	{
		/*if (Input.GetKeyDown (KeyCode.Space)) 
		{
			anim.SetTrigger ("Jump");
		}*/

		if (Input.GetButtonDown ("Jump") && !isJumping) 
		{
			anim.SetTrigger ("Jump");
		}

		if (anim.GetCurrentAnimatorStateInfo (0).IsName ("JumpAnim")) {
			isJumping = true;
		} else {
			isJumping = false;
		}
	}
}
