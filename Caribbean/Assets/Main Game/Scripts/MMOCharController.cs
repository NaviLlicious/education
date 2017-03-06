/*using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MMOCharController : MonoBehaviour 
{

	//public Transform playerCam, Player, centerPoint;

	//private float mouseX, mouseY;
	public float mouseSensitivity = 10f;
	public float mouseYPosition = 4f;

	//private float zoom;
	//public float zoomSpeed = 2;

	//public float zoomMin = -1.518f;
	//public float zoomMax = -8.94f;

	public float rotationSpeed = 5f;

	float verticalVelocity;

	public GameObject player;
	public float xOffset = 0;
	public float yOffset = 0;
	public float zOffset = 0;

	private float mouseX, mouseY;

	private float zoom;
	public float zoomSpeed;
	public float zoomMin;
	public float zoomMax;

	// Use this for initialization
	void Start () 
	{
		zoom = -21.1f;
		player = GameObject.FindGameObjectWithTag ("Player");
	}


	// Update is called once per frame
	void Update () 
	{

		if (player != null) {
			transform.position = new Vector3 (player.transform.position.x, player.transform.position.y, -10);
		} else {
			Debug.Log ("Player not set correctly.");
		}


			zoom += Input.GetAxis ("Mouse ScrollWheel") * zoomSpeed;

			if (zoom > zoomMin)
				zoom = zoomMin;

			if (zoom < zoomMax)
				zoom = zoomMax;

			//playerCam.transform.localPosition = new Vector3 (0, 0, zoom);

			if (Input.GetMouseButton (1)) {
				mouseX += Input.GetAxis ("Mouse X");
				mouseY -= Input.GetAxis ("Mouse Y");
			}

			mouseY = Mathf.Clamp (mouseY, -60f, 60f);
			//playerCam.LookAt (centerPoint);
			//player.localRotation = Quaternion.Euler (mouseY, mouseX, 0);

	}

	void FixedUpdate()
	{
		
	}
}*/