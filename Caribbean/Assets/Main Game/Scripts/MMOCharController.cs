using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MMOCharController : MonoBehaviour 
{

	public Transform playerCam, character, centerPoint;

	private float mouseX, mouseY;
	public float mouseSensitivity = 10f;
	public float mouseYPosition = 4f;

	CharacterController player;

	private float moveFB, moveLR;
	public float moveSpeed = 2f;

	private float zoom;
	public float zoomSpeed = 2;

	public float zoomMin = -1.518f;
	public float zoomMax = -8.94f;

	public float rotationSpeed = 5f;

	float verticalVelocity;

	public float jumpDist = 5f;

	bool canJump;

	//ShipScriptVariable Begin

	public bool CanMove;

	//ShipScriptVariable End

	// Use this for initialization
	void Start () 
	{
		player = GetComponent<CharacterController> ();
		zoom = -21.1f;
		CanMove = true;
	}


	// Update is called once per frame
	void Update () 
	{


			zoom += Input.GetAxis ("Mouse ScrollWheel") * zoomSpeed;

			if (zoom > zoomMin)
				zoom = zoomMin;

			if (zoom < zoomMax)
				zoom = zoomMax;

			playerCam.transform.localPosition = new Vector3 (0, 0, zoom);

			if (Input.GetMouseButton (1)) {
				mouseX += Input.GetAxis ("Mouse X");
				mouseY -= Input.GetAxis ("Mouse Y");
			}

			mouseY = Mathf.Clamp (mouseY, -60f, 60f);
			playerCam.LookAt (centerPoint);
			centerPoint.localRotation = Quaternion.Euler (mouseY, mouseX, 0);

			moveFB = Input.GetAxis ("Vertical") * moveSpeed;
			moveLR = Input.GetAxis ("Horizontal") * moveSpeed;

		if(CanMove){

				Vector3 movement = new Vector3 (moveLR, verticalVelocity, moveFB);
				movement = character.rotation * movement;
				character.GetComponent<CharacterController> ().Move (movement * Time.deltaTime);
				centerPoint.position = new Vector3 (character.position.x, character.position.y + mouseYPosition, character.position.z);

				if (Input.GetAxis ("Vertical") > 0 | Input.GetAxis ("Vertical") < 0) {

					Quaternion turnAngle = Quaternion.Euler (0, centerPoint.eulerAngles.y, 0);

					character.rotation = Quaternion.Slerp (character.rotation, turnAngle, Time.deltaTime * rotationSpeed);

				}
			
				if (Input.GetKeyDown ("space")) {
					Debug.Log ("Jumped");
					verticalVelocity += jumpDist;
					canJump = false;
				}

				/*if (player.isGrounded == true) 
			{
				if (Input.GetKeyDown (KeyCode.Space)) 
				{
					verticalVelocity += jumpDist;
					canJump = false;
					Debug.Log ("Jumped");
				}
			}*/
		}

	}

	void FixedUpdate()
	{
		if (CanMove) {
			if (canJump = false) {
				if (player.isGrounded == true) {
					canJump = true;
				}
			}

			if (player.isGrounded == false) {
				verticalVelocity += Physics.gravity.y * Time.deltaTime;
			} else {
				verticalVelocity = 0f;
			}
		}
	}
}