import System.Collections.Generic;

public class UniStormSetupEditor_JS extends EditorWindow 
{
	var UniStormSystem : UniStormWeatherSystem_JS = null;
	var UniStormGameObject : GameObject  = null;
	var PlayerObject : GameObject = null;
	var PlayerCamera : Camera = null;
	var PlayerCameraComponent : Camera = null;

	var yes : boolean = false;
	var no : boolean = false;

	var secs : float = 1f;
	var startVal : float = 0f;
	var progress : float = 0f;
	
	@MenuItem ("Window/UniStorm/Auto Setup Player/JavaScript")
	public static function ShowWindow()
	{
		EditorWindow.GetWindow(typeof(UniStormSetupEditor_JS));
	}

	public function OnSceneGUI()
	{
		if (Input.GetKeyDown(KeyCode.U))
		{
			ShowWindow();
			Debug.Log("Button!");
		}
	}

	function OnInspectorUpdate() {
		Repaint();
	}

	function OnGUI()
	{
		GUILayout.Label ("UniStorm Auto Player Setup - v1.0", EditorStyles.boldLabel);

		EditorGUILayout.HelpBox("UniStorm's new Auto Player Setup makes it easier than ever to setup custom characters with UniStorm. Simply attach your player and UniStorm will handle the rest. If you have 2 cameras, UniStorm will also handle this. UniStorm will auto assign and position all particle effects, setup and assign the camera, and apply them to UniStorm.", MessageType.None, true);

		BeginWindows();
		EditorGUILayout.HelpBox("You will need to assign your Player Object here in order for the Auto Assign system to properly apply all needed components. You will then be able to press the Auto Setup Player button.", MessageType.None, true);
		//PlayerObject = (GameObject)EditorGUI.ObjectField(new Rect(3, 160, position.width - 6, 16), "Player Object", PlayerObject, typeof(GameObject));
		PlayerObject = EditorGUILayout.ObjectField("Player Object", PlayerObject, GameObject, true);
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
			PlayerCamera = EditorGUILayout.ObjectField("PlayerObject", PlayerCamera, Camera, true);
			EndWindows();
		}

		EditorGUI.BeginDisabledGroup (yes == false && no == false || yes == true && no == true || PlayerObject == null || PlayerCamera == null && yes == true);

		GUILayout.Space(15);

		EditorGUILayout.HelpBox("Auto Setup Player will attach several particle effects to your player. This system will also alter the Far Clipping Plane on your camera. \n\nNote: If you already have UniStorm's particle effects on your player, you will need to remove them for the auto setup system to continue.", MessageType.None, true);

		if (GUILayout.Button("Auto Setup Player") && GameObject.Find("Rain/Splashes") == null)
		{
			startVal = EditorApplication.timeSinceStartup;


			UniStormGameObject = GameObject.Find("UniStormSystemEditor");
			UniStormSystem = UniStormGameObject.GetComponent(UniStormWeatherSystem_JS);

			if (yes)
			{
				PlayerCamera.farClipPlane = 18200;
				UniStormSystem.cameraObject = PlayerCamera.gameObject;
			}

			if (no)
			{
				PlayerCameraComponent = PlayerObject.GetComponentInChildren(Camera);
				PlayerCameraComponent.farClipPlane = 18200;
				UniStormSystem.cameraObject = PlayerCameraComponent.gameObject;
			}
			
			//var t: Texture2D = AssetDatabase.LoadAssetAtPath("Assets/Textures/texture.jpg", Texture2D) as Texture2D;

			var RainPrefab : ParticleSystem = AssetDatabase.LoadAssetAtPath("Assets/UniStorm (Desktop)/Prefabs (Desktop)/ParticleEffects/Rain.prefab", ParticleSystem) as ParticleSystem;
			var Rain : ParticleSystem = Instantiate (RainPrefab, PlayerObject.transform.position, PlayerObject.transform.rotation);
			Rain.gameObject.name = "Rain";
			Rain.gameObject.transform.parent = PlayerObject.transform;
			Rain.gameObject.transform.localPosition = new Vector3 (0,30,0);
			Rain.gameObject.transform.localRotation = Quaternion.Euler (270,0,0);
			UniStormSystem.rain = Rain;
			UniStormSystem.rainSplashes = GameObject.Find("Rain/Splashes").GetComponent(ParticleSystem);

			var RainMistPrefab : ParticleSystem = AssetDatabase.LoadAssetAtPath("Assets/UniStorm (Desktop)/Prefabs (Desktop)/ParticleEffects/Rain Mist.prefab", ParticleSystem) as ParticleSystem;
			var RainMist : ParticleSystem = Instantiate (RainMistPrefab, PlayerObject.transform.position, PlayerObject.transform.rotation);
			RainMist.gameObject.name = "Rain Mist";
			RainMist.gameObject.transform.parent = PlayerObject.transform;
			RainMist.gameObject.transform.localPosition = new Vector3 (0,37,0);
			RainMist.gameObject.transform.localRotation = Quaternion.Euler (270,0,0);
			UniStormSystem.rainMist = RainMist;

			var MistFogPrefab : GameObject = AssetDatabase.LoadAssetAtPath("Assets/UniStorm (Desktop)/Prefabs (Desktop)/ParticleEffects/Rain Streaks.prefab", GameObject) as GameObject;
			var MistFog : GameObject = Instantiate (MistFogPrefab, PlayerObject.transform.position, PlayerObject.transform.rotation);
			MistFog.gameObject.name = "Rain Streaks";
			MistFog.gameObject.transform.parent = PlayerObject.transform;
			MistFog.gameObject.transform.localPosition = new Vector3 (0,180,0);
			MistFog.gameObject.transform.localRotation = Quaternion.Euler (0,0,0);
			UniStormSystem.mistFog = MistFog;

			var SnowPrefab : ParticleSystem = AssetDatabase.LoadAssetAtPath("Assets/UniStorm (Desktop)/Prefabs (Desktop)/ParticleEffects/Snow.prefab", ParticleSystem) as ParticleSystem;
			var Snow : ParticleSystem = Instantiate (SnowPrefab, PlayerObject.transform.position, PlayerObject.transform.rotation);
			Snow.gameObject.name = "Snow";
			Snow.gameObject.transform.parent = PlayerObject.transform;
			Snow.gameObject.transform.localPosition = new Vector3 (0,25,0);
			Snow.gameObject.transform.localRotation = Quaternion.Euler (270,0,0);
			UniStormSystem.snow = Snow;

			var SnowDustPrefab : ParticleSystem = AssetDatabase.LoadAssetAtPath("Assets/UniStorm (Desktop)/Prefabs (Desktop)/ParticleEffects/Snow Dust.prefab", ParticleSystem) as ParticleSystem;
			var SnowDust : ParticleSystem = Instantiate (SnowDustPrefab, PlayerObject.transform.position, PlayerObject.transform.rotation);
			SnowDust.gameObject.name = "Snow Dust";
			SnowDust.gameObject.transform.parent = PlayerObject.transform;
			SnowDust.gameObject.transform.localPosition = new Vector3 (0,37,0);
			SnowDust.gameObject.transform.localRotation = Quaternion.Euler (270,0,0);
			UniStormSystem.snowMistFog = SnowDust;

			var ButterfliesPrefab : ParticleSystem = AssetDatabase.LoadAssetAtPath("Assets/UniStorm (Desktop)/Prefabs (Desktop)/ParticleEffects/Lightning Bugs.prefab", ParticleSystem) as ParticleSystem;
			var Butterflies : ParticleSystem = Instantiate (ButterfliesPrefab, PlayerObject.transform.position, PlayerObject.transform.rotation);
			Butterflies.gameObject.name = "Lightning Bugs";
			Butterflies.gameObject.transform.parent = PlayerObject.transform;
			Butterflies.gameObject.transform.localPosition = new Vector3 (0,7.8f,0);
			Butterflies.gameObject.transform.localRotation = Quaternion.Euler (270,0,0);
			UniStormSystem.butterflies = Butterflies;

			var WindyLeavesPrefab : ParticleSystem = AssetDatabase.LoadAssetAtPath("Assets/UniStorm (Desktop)/Prefabs (Desktop)/ParticleEffects/Fall Leaves.prefab", ParticleSystem) as ParticleSystem;
			var WindyLeaves : ParticleSystem = Instantiate (WindyLeavesPrefab, PlayerObject.transform.position, PlayerObject.transform.rotation);
			WindyLeaves.gameObject.name = "Windy Leaves";
			WindyLeaves.gameObject.transform.parent = PlayerObject.transform;
			WindyLeaves.gameObject.transform.localPosition = new Vector3 (0,46,0);
			WindyLeaves.gameObject.transform.localRotation = Quaternion.Euler (270,0,0);
			UniStormSystem.windyLeaves = WindyLeaves;

			var LightningSpawnPrefab : Transform = AssetDatabase.LoadAssetAtPath("Assets/UniStorm (Desktop)/Prefabs (Desktop)/ParticleEffects/Lightning Position.prefab", Transform) as Transform;
			var LightningSpawn : Transform = Instantiate (LightningSpawnPrefab, PlayerObject.transform.position, PlayerObject.transform.rotation);
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
		progress = (EditorApplication.timeSinceStartup - startVal);

		//secs = EditorGUILayout.FloatField("Time to wait:", secs);
	}
	}

