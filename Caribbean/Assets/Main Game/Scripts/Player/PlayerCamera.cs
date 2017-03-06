using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Helper;

//This script is included with the PlayerCharacter script. Includes and governs the behaviour of this instance's
// ability to move the camera around the environment using an FSM.

public class PlayerCamera : MonoBehaviour {

	#region Public Fields & Properties

	#endregion


	#region Private Fields & Properties

	//Used to position the camera
	private Vector3 _cameraNormalPosition = new Vector3(-0.0001983927f, -0.2322373f, -0.005075848f);

	//Used to rotate camera
	[SerializeField]
	private float sensitivity = 5f;
	[SerializeField]
	public float minimumAngle = -40f;
	[SerializeField]
	public float maximumAngle = 60f;

	private float rotationY = 0f;

	private Transform _camera;
	private Transform _player;

	private PlayerCharacter _pc;

	private CameraState _state = CameraState.Normal;

	private CameraTargetObject _cameraTargetObject;
	private CameraMountPoint _cameraMountPoint;

	#endregion

	#region Getters and Setters

	//Use this for initialization
	public CameraState CameraState
	{
		get{return _state;}
	}

	#endregion

	#region System Methods

	// Use this for initialization
	void Start () 
	{
		NetworkView networkView = GetComponent<NetworkView> ();

		if (networkView.isMine || Network.peerType == NetworkPeerType.Disconnected) 
		{
			_pc = this.GetComponent<PlayerCharacter> ();
			_camera = GameObject.FindGameObjectWithTag (GameTag.PlayerCamera).transform;
			_player = this.transform;

			//Create an object at runtime for the camera to look at.
			_cameraTargetObject = new CameraTargetObject();
			_cameraTargetObject.Init ("Camera Target", new Vector3 (0f, 1f, 0f), new GameObject ().transform, _player.transform);

			//Create an empty object at runtime for the camera to sit on.
			_cameraMountPoint = new CameraMountPoint ();
			_cameraMountPoint.Init ("Camera Mount", _cameraNormalPosition, new GameObject ().transform, _cameraTargetObject.XForm);

			_camera.parent = _cameraTargetObject.XForm.parent;
		} 
		else
		{
			enabled = false;
		}
	}
	
	// Update is called once per frame
	void Update ()
	{
		
	}

	void LateUpdate()
	{
		switch (_state) 
		{
		case CameraState.Normal:

			RotateCamera ();

			_camera.position = _cameraMountPoint.XForm.position;
			_camera.LookAt (_cameraTargetObject.XForm);

			break;

			case CameraState.Target:

			break;
		}
	}

	#endregion

	#region Custom Methods

	public void RotateCamera()
	{
		rotationY -= Input.GetAxis (PlayerInput.RightY) * sensitivity;
		rotationY = Mathf.Clamp (rotationY, minimumAngle, maximumAngle);

		_cameraTargetObject.XForm.localEulerAngles = new Vector3 (-rotationY, _cameraTargetObject.XForm.localEulerAngles.y, 0);
	}

	#endregion
}
