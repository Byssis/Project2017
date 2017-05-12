<<<<<<< HEAD
﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public const string PAUSE_BTN_NAME = "PauseButton";
    public enum GameMode { laserMode, mirrorMode, none };	// gameModes
	public enum GameState {tutorial,gameRunning,endScreen, gamePaused};	// gameStates available

    public static GameMode gameMode = GameMode.none;		// init gameMode to none
    private static GameState prevGameState;					// save prev gameState
	public GameObject laserRay;
    [HideInInspector]
    public ViewController UI;                               // UI containing all panels etc.
    [HideInInspector]
    public TargetMaster targetMaster;
    [HideInInspector]
    public LaserMode laserMode;

	public static GameState gameState;
	private string levelName;
	private int tutorialIndex = 1;

    private LaserStack laserStack;
    private int numOfLasers = 20;

	// Use this for initialization
	void Start ()
	{
        /*
			"Prata med minne"


		levelName = loadName();
		loadScore();
		loadTutorialIndex();

		*/
        laserMode = GameObject.Find("LightSource").GetComponent<LaserMode>();

        laserStack = new LaserStack();
        Debug.Log(GameObject.FindObjectsOfType(typeof(IInteractables)));
  
        foreach (IInteractables inter in GameObject.FindObjectsOfType(typeof(IInteractables)))
        {
            inter.SetLasers(laserStack);
            inter.SetLaser(laserRay.GetComponent<LaserRay>());
        }
        foreach (Transform trans in GameObject.FindObjectsOfType(typeof(Transform)))
        {
            Rigidbody gameObjectsRigidBody = trans.gameObject.AddComponent<Rigidbody>(); // Add the rigidbody.
            if (gameObjectsRigidBody == null) continue; 
            //gameObjectsRigidBody.mass = 5; // Set the GO's mass to 5 via the Rigidbody.
            gameObjectsRigidBody.velocity = new Vector3(UnityEngine.Random.Range(-1.0f, 1.0f), 0, UnityEngine.Random.Range(-1.0f, 1.0f));
            //gameObjectsRigidBody.MoveRotation( Quaternion.Euler(UnityEngine.Random.Range(-10.0f, 10.0f), UnityEngine.Random.Range(-0.0f, 10.0f), UnityEngine.Random.Range(-10.0f, 10.0f)))  ;
            //floatAway(child);
            //floatAway(trans);
        }

        generateLaserStack();

        laserMode.laserStack = laserStack;
       

		// Start game with tutorial window #1
		gameState = GameState.tutorial;
        prevGameState = GameState.tutorial;
        gameMode = GameMode.none;
       
		// Access UIs components and children
		UI = GameObject.Find ("UI").GetComponent<ViewController> ();
		// Access targetMaster
		targetMaster = GameObject.Find ("TargetMaster").GetComponent<TargetMaster> ();
        // Hide mellanmeny
        UI.transform.Find("Canvas").transform.Find("MellanMeny").gameObject.SetActive(false);
		// If totrial index = -1 dont show anything, otherwise load tutorial with index tutorialIndex
		if (tutorialIndex >= 0) {
			LoadTutorial (tutorialIndex);
		}
	}

    private void generateLaserStack()
    {
        for (int i = 0; i < numOfLasers; i++)
        {
            LaserRay newLaser = Instantiate(laserRay, new Vector3(0, 0, 0), Quaternion.identity).GetComponent<LaserRay>();
            laserStack.push(newLaser);
        }
    }

    void FixedUpdate ()
	{
		// When entering gameRunning from tutorial, show gameModebutton
        if(prevGameState == GameState.tutorial && gameState == GameState.gameRunning)
        {
            gameMode = GameMode.laserMode;
            UI.ShowGameModeButton();
        }
		// Decide which code is running according to gameState
		switch (gameState) {
		// Case tutorial, do something...
		case GameState.tutorial:
            break;
			// Case gameRunning, show gamemode-button and check if level is completed
        case GameState.gameRunning:
            //visa pause-knappen
            UI.transform.Find(PAUSE_BTN_NAME).gameObject.SetActive(true);
            UI.ShowGameModeButton();
			CheckLevelCompleted ();
			break;
			// Case endScreen, check what next state is
		case GameState.endScreen:
			CheckNextState ();
			break;
		case GameState.gamePaused:
			CheckNextState ();
			break;

		default:
			break;
		}
        prevGameState = gameState; 
	}

	private void LoadTutorial (int index)
	{
		// Load info about tutorial
		string title = "title";
		string text = "sample text";
		Image icon = null;
		UI.NewTutorial (title, text, icon);
	}

	private void CheckLevelCompleted ()
	{
		// If all targets are hit, load endScreen and set gameMode = none
		if (targetMaster.CheckLevelCompleted ()) {
			LoadLevelEndScreen ();
			gameState = GameState.endScreen;
            gameMode = GameMode.none;
		}
	}

	private void LoadLevelEndScreen ()
	{
		// Load info about which level got completed
		string level = "Level";
		UI.ShowEndScreen (level, targetMaster.GetCollectables ());
	}

	private void CheckNextState ()
	{
		// Check select to decide what next state is
		switch (UI.select) {
		case ViewController.Select.menu:
			MainMenu ();
			break;
		case ViewController.Select.replay:
			Replay ();
			break;
		case ViewController.Select.next:
			NextScene ();
			break;
		default:
			break;
		}
	}

	private void NewScene (int n)
	{ // where n is offset from current scene
		Scene activeScene = SceneManager.GetActiveScene ();
		SceneManager.LoadScene (activeScene.buildIndex + n);
		Debug.Log ("Entering next scene");
	}
    	

	private void NextScene ()
	{
		// Load next level
		NewScene (1);
		gameState = GameState.gameRunning;
	}

	private void Replay ()
	{
		// Load same level again
		NewScene (0);
		gameState = GameState.gameRunning;
	}

	private void MainMenu ()
	{
        SceneManager.LoadScene("StartScene");
	}

    private void floatAway(Transform transform)
    {
        foreach(Transform child in transform)
        {
          // .velocity =  new Vector3(UnityEngine.Random.Range(-10.0f, 10.0f), UnityEngine.Random.Range(-0.0f, 10.0f), UnityEngine.Random.Range(-10.0f, 10.0f));
            Rigidbody gameObjectsRigidBody = child.gameObject.AddComponent<Rigidbody>(); // Add the rigidbody.
            //gameObjectsRigidBody.mass = 5; // Set the GO's mass to 5 via the Rigidbody.
            gameObjectsRigidBody.velocity = new Vector3(UnityEngine.Random.Range(-10.0f, 10.0f), UnityEngine.Random.Range(-0.0f, 10.0f), UnityEngine.Random.Range(-10.0f, 10.0f));
            floatAway(child);
        }
    }

}
=======

﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public const string PAUSE_BTN_NAME = "PauseButton";
    public enum GameMode { laserMode, mirrorMode, none };	// gameModes
	public enum GameState {tutorial,gameRunning,endScreen, gamePaused};	// gameStates available

    public static GameMode gameMode = GameMode.none;		// init gameMode to none
    private static GameState prevGameState;					// save prev gameState
	public GameObject laserRay;
    [HideInInspector]
    public ViewController UI;                               // UI containing all panels etc.
    [HideInInspector]
    public TargetMaster targetMaster;
    [HideInInspector]
    public LaserMode laserMode;
	public static GameState gameState;

	private int tutorialIndex;
//	private string levelName;

    private LaserStack laserStack;
    private int numOfLasers = 100;

    private float MIN_FPS = 20;

	// Use this for initialization
	void Start ()
	{
		
        //Prata med minnet
//		levelName = MemoryManager.LoadLevelName();
		tutorialIndex = MemoryManager.LoadTutorialIndex();

        laserMode = GameObject.Find("LightSource").GetComponent<LaserMode>();

        laserStack = new LaserStack();
        Debug.Log(GameObject.FindObjectsOfType(typeof(IInteractables)));
  
        foreach (IInteractables inter in GameObject.FindObjectsOfType(typeof(IInteractables)))
        {
            inter.SetLasers(laserStack);
            inter.SetLaser(laserRay.GetComponent<LaserRay>());
        }

        generateLaserStack();

        laserMode.laserStack = laserStack;
       

       
		// Access UIs components and children
		UI = GameObject.Find ("UI").GetComponent<ViewController> ();
		// Access targetMaster
		targetMaster = GameObject.Find ("TargetMaster").GetComponent<TargetMaster> ();
		// Hide mellanmeny
		UI.transform.Find("Canvas").transform.Find("MellanMeny").gameObject.SetActive (false);

        Debug.Log("tutorialIndex: " + tutorialIndex);
        Debug.Log("playedBefore: " + MemoryManager.TutorialPlayedBefore(tutorialIndex));

		// If tutorial index = -1 dont show anything, otherwise load tutorial with index tutorialIndex
		if (tutorialIndex >= 0 && MemoryManager.TutorialPlayedBefore(tutorialIndex) == false) {
			// Start game with tutorial window #1
			gameState = GameState.tutorial;
			prevGameState = GameState.tutorial;
			gameMode = GameMode.none;

			LoadTutorial (tutorialIndex);
            Debug.Log("tutorialPlayedBefore: " + MemoryManager.TutorialPlayedBefore(tutorialIndex));
            //Debug.Log(SceneManager.GetActiveScene().buildIndex);
            MemoryManager.SetTutorialPlayedBefore(tutorialIndex);
        } else {
			// Start game without tutorial
			gameState = GameState.gameRunning;
			prevGameState = GameState.gameRunning;
			gameMode = GameMode.laserMode;
		}
	}

    private void generateLaserStack()
    {
        for (int i = 0; i < numOfLasers; i++)
        {
            LaserRay newLaser = Instantiate(laserRay, new Vector3(0, 0, 0), Quaternion.identity).GetComponent<LaserRay>();
            laserStack.push(newLaser);
        }
    }

    void FixedUpdate ()
	{
		// When entering gameRunning from tutorial, show gameModebutton
        if(prevGameState == GameState.tutorial && gameState == GameState.gameRunning)
        {
            gameMode = GameMode.laserMode;
            UI.ShowGameModeButton();
        }
		// Decide which code is running according to gameState
		switch (gameState) {
		// Case tutorial, do something...
		case GameState.tutorial:
            break;
			// Case gameRunning, show gamemode-button and check if level is completed
        case GameState.gameRunning:
            //visa pause-knappen
            UI.transform.Find(PAUSE_BTN_NAME).gameObject.SetActive(true);
            UI.ShowGameModeButton();
			CheckLevelCompleted ();
			break;
			// Case endScreen, check what next state is
		case GameState.endScreen:
			CheckNextState ();
			break;
		case GameState.gamePaused:
			CheckNextState ();
			break;

		default:
			break;
		}
        prevGameState = gameState;
        RenderSettings.skybox.SetFloat("_Rotation", 2 * Time.deltaTime + RenderSettings.skybox.GetFloat("_Rotation"));
        RenderSettings.skybox.SetFloat("_Exposure", Mathf.Sin(2 * Time.deltaTime + RenderSettings.skybox.GetFloat("_Rotation"))/8.0f + 1.2f);
        //Debug.Log(skybox.GetFloat("_Exposure"));
        //RenderSettings.skybox = skybox;

    }

    private void LoadTutorial (int index)
	{
		// Load info about tutorial
		Tutorial tut;
		if (index >= 0) {
			tut = MemoryManager.LoadTutorial (index);
		} else
			tut = new Tutorial();

		UI.NewTutorial (tut.title, tut.tutorialText, tut.icon);
	}

	private void CheckLevelCompleted ()
	{
		// If all targets are hit, load endScreen and set gameMode = none
		if (targetMaster.CheckLevelCompleted ()) {
			LoadLevelEndScreen ();
			gameState = GameState.endScreen;
            gameMode = GameMode.none;
		}
	}

	private void LoadLevelEndScreen ()
	{
		// Load info about which level got completed
		string level = "Level";
		int score = targetMaster.GetCollectables ();
		MemoryManager.WriteScore2Memory (score);
		UI.ShowEndScreen (level, score);
	}

	private void CheckNextState ()
	{
		// Check select to decide what next state is
		switch (UI.select) {
		case ViewController.Select.menu:
			MainMenu ();
			break;
		case ViewController.Select.replay:
			Replay ();
			break;
		case ViewController.Select.next:
			NextScene ();
			break;
		default:
			break;
		}
	}

	private void NewScene (int n)
	{ // where n is offset from current scene
		Scene activeScene = SceneManager.GetActiveScene ();
		SceneManager.LoadScene (activeScene.buildIndex + n);
		Debug.Log ("Entering next scene");
	}
    	

	private void NextScene ()
	{
		// Load next level
		NewScene (1);
		gameState = GameState.gameRunning;
	}

	private void Replay ()
	{
		// Load same level again
		NewScene (0);
		gameState = GameState.gameRunning;
	}

	private void MainMenu ()
	{
        SceneManager.LoadScene("StartScene");
	}
		
	private void LoadLevelName(){
		string jsonString = File.ReadAllText (Application.dataPath + "/Resources/Levels.json");
		Debug.Log (jsonString);
	}


    float deltaTime = 0.0f;

    void Update()
    {
        deltaTime += (Time.deltaTime - deltaTime) * 0.1f;
    }

    void OnGUI()
    {
        int w = Screen.width, h = Screen.height;

        GUIStyle style = new GUIStyle();

        Rect rect = new Rect(0, 0, w, h * 2 / 100);
        style.alignment = TextAnchor.UpperLeft;
        style.fontSize = h * 2 / 100;
        style.normal.textColor = new Color(0.0f, 0.0f, 0.5f, 1.0f);
        float msec = deltaTime * 1000.0f;
        float fps = 1.0f / deltaTime;
        if(fps < MIN_FPS)disableGlow();
        string text = string.Format("{0:0.0} ms ({1:0.} fps)", msec, fps);
        GUI.Label(rect, text, style);
    }

    private void disableGlow()
    {
        Camera.main.GetComponent<MKGlowSystem.MKGlow>().enabled = false;
    }

}
>>>>>>> Dev
