using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Helper;

//This script is attached to the player and includes all dependancies that are required in order for the
//character controller system to function. No scripts included in this instance should ever be absent.

// Imcludes: PlayerCamera.cs and PlayerMotor.cs
//PlayerCamera.cs governs the behaviour of the camera and is only active on this instance.
//PlayerMotor.cs governs the behavious of the player's movement and animation.

[RequireComponent(typeof(NetworkView))] //Allows both Multiplayer and Singleplayer use.
[RequireComponent(typeof(CharacterController))] //Creates a character controller on object.
[RequireComponent(typeof(Animator))] //Creates an Animator on object if one doesn't already exist.
[RequireComponent(typeof(PlayerMotor))] //Includes PlayerMotor.cs on object.
[RequireComponent(typeof(PlayerCamera))] //Includes PlayerCamera.cs on object.

[AddComponentMenu("APP/PlayerCharacter")]

public class PlayerCharacter : MonoBehaviour 
{

	#region Public Fields & Properties

	#endregion

	#region

	private CharacterController _controller;

	private Animator _animator;
	private RuntimeAnimatorController _animatorController;

	#endregion

	#region Getters & Setters

	//Gets the Animator component.
	//Value = "The animator".

	public Animator Animator
	{
		get {return this._animator;}
	}

	public CharacterController Controller
	{
		get {return this._controller;}
	}

	#endregion

	#region System Methods

	void Awake()
	{
		
		_animator = this.GetComponent<Animator> ();
		_controller = this.GetComponent < CharacterController> ();

	}

	// Use this for initialization
	void Start () 
	{
		//This is used because standard "networkView" is depraciated in Unity 5
		NetworkView networkView = GetComponent<NetworkView> ();

		//Ensure that a networkView component exists.
		if (networkView != null) 
		{
			//Ensure that initialization only executes if this is a valid instance.
			if (networkView.isMine || Network.peerType == NetworkPeerType.Disconnected) 
			{
				//Load in the animator controller at runtime
				_animatorController = Resources.Load(Resource.AnimatorController) as RuntimeAnimatorController;
				_animator.runtimeAnimatorController = _animatorController;

				_controller.center = new Vector3 (0f, 1f, 0f);
				_controller.height = 1.8f;
			} 
			else 
			{
				enabled = false;
			}
		}
		else 
		{
			Debug.Log ("You somehow managed to fuck up and start the game without a NetworkView component. Sucks to be you dickhead!");
		}
	}
	
	// Update is called once per frame
	void Update () 
	{

	}

	#endregion
}
