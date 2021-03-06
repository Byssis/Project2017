﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
//Use with Targets, A master object with Targets as children
public class TargetMaster : MonoBehaviour {

	private Target[] Targets;

	void Start () {
		//Get all Target-scripts children-targets
		Targets = GetComponentsInChildren<Target> ();
	}
	

	// Update is called late in every frame to make sure all target hits are registered correctly
	void LateUpdate () {

		//Check if all are hit
		bool nextLevel = false;

		//Get all current hits for targets on level
		for(int i = 0; i < Targets.Length; i++){

			//Debug.Log ("Target " + i + " hit: " + Targets[i].hit);

			if (Targets[i].hit) {
				nextLevel = true;
			} else {
				nextLevel = false;
				break;
			}
		}

		//if all are hit go to next scene
		if (nextLevel) {
			//next scene code here...
            Scene activeScene = SceneManager.GetActiveScene();
            SceneManager.LoadScene(activeScene.buildIndex + 1);
			Debug.Log ("Entering next scene");
		}

	}
}
