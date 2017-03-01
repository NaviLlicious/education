using UnityEditor;
using UnityEngine;
using System.Collections;

public class UniStormSetupEditor : EditorWindow
{
	public UniStormWeatherSystem_C UniStormSystem = null;
	public GameObject UniStormGameObject = null;
	public GameObject PlayerObject = null;
	public Camera PlayerCamera = null;

	bool yes = false;
	bool no = false;

	public float secs = 1f;
	public float startVal = 0f;
	public float progress = 0f;

	[MenuItem("Window/UniStorm/Auto Setup Player/C#")]
	public static void ShowWindow()
	{
		EditorWindow.GetWindow(typeof(UniStormSetupEditor));
	}

	public void OnSceneGUI()
	{
		if (Input.GetKeyDown(KeyCode.U))
		{
			ShowWindow();
			Debug.Log("Button!");
		}
	}

	void OnInspectorUpdate() {
		Repaint();
	}

	void OnGUI()
	{
		GUILayout.Label ("UniStorm Auto Player Setup - v1.0", EditorStyles.boldLabel);

		EditorGUILayout.HelpBox("UniStorm's new Auto Player Setup makes it easier than ever to setup custom characters with UniStorm. Simply attach your player and UniStorm will handle the rest. If you have 2 cameras, UniStorm will also handle this. UniStorm will auto assign and position all particle effects, setup and assign the camera, and apply them to UniStorm.", MessageType.None, true);

		BeginWindows();
		EditorGUILayout.HelpBox("You will need to assign your Player Object here in order for the Auto Assign system to properly apply all needed components. You will then be able to press the Auto Setup Player button.", MessageType.None, true);
		//PlayerObject = (GameObject)EditorGUI.ObjectField(new Rect(3, 160, position.width - 6, 16), "Player Object", PlayerObject, typeof(GameObject));
		PlayerObject = (GameObject)EditorGUILayout.ObjectField("Player Object", PlayerObject, typeof(GameObject), true);
		EndWindows();

		GUILayout.Space(15);

		BeginWindows();
		EditorGUILayout.HelpBox("Does your player have more than 1 camera?", MessageType.None, true);
		EndWindows();

		yes = EditorGUILayout.Toggle ("Yes", yes);
		no = EditorGUILayout.Toggle ("No", no);

		if (yes)
		{
			GUILayout.Space(15);

			BeginWindows();
			EditorGUILayout.HelpBox("Assign your scene rendering camera here. This would be the camera that renders the level, not the camera that renders any FPS weapon. For example, if you were using UFPS, you would assign the FPSCamera object.", MessageType.None, true);
			//PlayerCamera = (Camera)EditorGUI.ObjectField(new Rect(3, 180, position.width - 6, 16), "Player Camera", PlayerCamera, typeof(Camera));
			PlayerCamera = (Camera)EditorGUILayout.ObjectField("PlayerObject", PlayerCamera, typeof(Camera), true);
			EndWindows();
		}

		EditorGUI.BeginDisabledGroup (yes == false && no == false || yes == true && no == true || PlayerObject == null || PlayerCamera == null && yes == true);

		GUILayout.Space(15);

		EditorGUILayout.HelpBox("Auto Setup Player will attach several particle effects to your player. This system will also alter the Far Clipping Plane on your camera. \n\nNote: If you already have UniStorm's particle effects on your player, you will need to remove them for the auto setup system to continue.", MessageType.None, true);

		if (GUILayout.Button("Auto Setup Player") && GameObject.Find("Rain/Splashes") == null)
		{
			startVal = (float)EditorApplication.timeSinceStartup;


			UniStormGameObject = GameObject.Find("UniStormSystemEditor");
			UniStormSystem = UniStormGameObject.GetComponent<UniStormWeatherSystem_C>();

			if (yes)
			{
				PlayerCamera.farClipPlane = 18200;
				UniStormSystem.cameraObject = PlayerCamera.gameObject;
			}

			if (no)
			{
				Camera PlayerCameraComponent = PlayerObject.GetComponentInChildren<Camera>();
				PlayerCameraComponent.farClipPlane = 18200;
				UniStormSystem.cameraObject = PlayerCameraComponent.gameObject;
			}


			ParticleSystem RainPrefab = (ParticleSystem) AssetDatabase.LoadAssetAtPath("Assets/UniStorm (Desktop)/Prefabs (Desktop)/ParticleEffects/Rain.prefab", typeof(ParticleSystem));
			ParticleSystem Rain = ((ParticleSystem)Instantiate (RainPrefab, PlayerObject.transform.position, PlayerObject.transform.rotation));
			Rain.gameObject.name = "Rain";
			Rain.gameObject.transform.parent = PlayerObject.transform;
			Rain.gameObject.transform.localPosition = new Vector3 (0,30,0);
			Rain.gameObject.transform.localRotation = Quaternion.Euler (270,0,0);
			UniStormSystem.rain = Rain;
			UniStormSystem.rainSplashes = GameObject.Find("Rain/Splashes").GetComponent<ParticleSystem>();

			ParticleSystem RainMistPrefab = (ParticleSystem) AssetDatabase.LoadAssetAtPath("Assets/UniStorm (Desktop)/Prefabs (Desktop)/ParticleEffects/Rain Mist.prefab", typeof(ParticleSystem));
			ParticleSystem RainMist = ((ParticleSystem)Instantiate (RainMistPrefab, PlayerObject.transform.position, PlayerObject.transform.rotation));
			RainMist.gameObject.name = "Rain Mist";
			RainMist.gameObject.transform.parent = PlayerObject.transform;
			RainMist.gameObject.transform.localPosition = new Vector3 (0,37,0);
			RainMist.gameObject.transform.localRotation = Quaternion.Euler (270,0,0);
			UniStormSystem.rainMist = RainMist;

			GameObject MistFogPrefab = (GameObject) AssetDatabase.LoadAssetAtPath("Assets/UniStorm (Desktop)/Prefabs (Desktop)/ParticleEffects/Rain Streaks.prefab", typeof(GameObject));
			GameObject MistFog = ((GameObject)Instantiate (MistFogPrefab, PlayerObject.transform.position, PlayerObject.transform.rotation));
			MistFog.gameObject.name = "Rain Streaks";
			MistFog.gameObject.transform.parent = PlayerObject.transform;
			MistFog.gameObject.transform.localPosition = new Vector3 (0,180,0);
			MistFog.gameObject.transform.localRotation = Quaternion.Euler (0,0,0);
			UniStormSystem.mistFog = MistFog;

			ParticleSystem SnowPrefab = (ParticleSystem) AssetDatabase.LoadAssetAtPath("Assets/UniStorm (Desktop)/Prefabs (Desktop)/ParticleEffects/Snow.prefab", typeof(ParticleSystem));
			ParticleSystem Snow = ((ParticleSystem)Instantiate (SnowPrefab, PlayerObject.transform.position, PlayerObject.transform.rotation));
			Snow.gameObject.name = "Snow";
			Snow.gameObject.transform.parent = PlayerObject.transform;
			Snow.gameObject.transform.localPosition = new Vector3 (0,25,0);
			Snow.gameObject.transform.localRotation = Quaternion.Euler (270,0,0);
			UniStormSystem.snow = Snow;

			ParticleSystem SnowDustPrefab = (ParticleSystem) AssetDatabase.LoadAssetAtPath("Assets/UniStorm (Desktop)/Prefabs (Desktop)/ParticleEffects/Snow Dust.prefab", typeof(ParticleSystem));
			ParticleSystem SnowDust = ((ParticleSystem)Instantiate (SnowDustPrefab, PlayerObject.transform.position, PlayerObject.transform.rotation));
			SnowDust.gameObject.name = "Snow Dust";
			SnowDust.gameObject.transform.parent = PlayerObject.transform;
			SnowDust.gameObject.transform.localPosition = new Vector3 (0,37,0);
			SnowDust.gameObject.transform.localRotation = Quaternion.Euler (270,0,0);
			UniStormSystem.snowMistFog = SnowDust;

			ParticleSystem ButterfliesPrefab = (ParticleSystem) AssetDatabase.LoadAssetAtPath("Assets/UniStorm (Desktop)/Prefabs (Desktop)/ParticleEffects/Lightning Bugs.prefab", typeof(ParticleSystem));
			ParticleSystem Butterflies = ((ParticleSystem)Instantiate (ButterfliesPrefab, PlayerObject.transform.position, PlayerObject.transform.rotation));
			Butterflies.gameObject.name = "Lightning Bugs";
			Butterflies.gameObject.transform.parent = PlayerObject.transform;
			Butterflies.gameObject.transform.localPosition = new Vector3 (0,7.8f,0);
			Butterflies.gameObject.transform.localRotation = Quaternion.Euler (270,0,0);
			UniStormSystem.butterflies = Butterflies;

			ParticleSystem WindyLeavesPrefab = (ParticleSystem) AssetDatabase.LoadAssetAtPath("Assets/UniStorm (Desktop)/Prefabs (Desktop)/ParticleEffects/Fall Leaves.prefab", typeof(ParticleSystem));
			ParticleSystem WindyLeaves = ((ParticleSystem)Instantiate (WindyLeavesPrefab, PlayerObject.transform.position, PlayerObject.transform.rotation));
			WindyLeaves.gameObject.name = "Windy Leaves";
			WindyLeaves.gameObject.transform.parent = PlayerObject.transform;
			WindyLeaves.gameObject.transform.localPosition = new Vector3 (0,46,0);
			WindyLeaves.gameObject.transform.localRotation = Quaternion.Euler (270,0,0);
			UniStormSystem.windyLeaves = WindyLeaves;

			Transform LightningSpawnPrefab = (Transform) AssetDatabase.LoadAssetAtPath("Assets/UniStorm (Desktop)/Prefabs (Desktop)/ParticleEffects/Lightning Position.prefab", typeof(Transform));
			Transform LightningSpawn = ((Transform)Instantiate (LightningSpawnPrefab, PlayerObject.transform.position, PlayerObject.transform.rotation));
			LightningSpawn.gameObject.name = "Lightning Position";
			LightningSpawn.gameObject.transform.parent = PlayerObject.transform;
			LightningSpawn.gameObject.transform.localPosition = new Vector3 (-2,27,5);
			LightningSpawn.gameObject.transform.localRotation = Quaternion.Euler (0,0,0);
			UniStormSystem.lightningSpawn = LightningSpawn;


			//Unity 5.1 and below
			//Rain = (GameObject) Resources.LoadAssetAtPath("Assets/Artwork/mymodel.fbx", typeof(ParticleSystem));

		}

		
		if (progress < secs)
			EditorUtility.DisplayProgressBar("Auto Setting Up Player for UniStorm", "Auto assigning needed components...", progress / secs);
		else
			EditorUtility.ClearProgressBar();
		progress = (float)(EditorApplication.timeSinceStartup - startVal);

		//secs = EditorGUILayout.FloatField("Time to wait:", secs);
	}


}
