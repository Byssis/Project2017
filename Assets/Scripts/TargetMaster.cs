﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Use with Target Master, A master object with Targets as children
public class TargetMaster : MonoBehaviour {

	private Target[] Targets;
	private bool[] hits;

	// Use this for initialization
	void Start () {

		Targets = GetComponentsInChildren<Target> ();
		hits = new bool[Targets.Length];
		//Debug.Log ("Get all children");

	}
	
	// Update is called late in every frame to make sure all target hits are registered correctly
	void LateUpdate () {
		
		//Debug.Log ("Getting all " + Targets.Length + " hits.");

		//Get all current hits for targets on level
		for(int i = 0; i < Targets.Length; i++){
			Target targScript = Targets [i];
			hits [i] = targScript.hit;
			//Debug.Log ("Target " + i + " hit: " + hits[i]);

		}
		//Check if all are hit (Optimize later)
		bool nextLevel = false;
		foreach(bool h in hits){
			if (!h) {
				nextLevel = false;
				break;
			} else {
				nextLevel = true;
			}
			//Debug.Log ("Should go to next level: " + nextLevel);
		}

		//if all are hit go to next scene
		if (nextLevel) {
			//next scene code here...
			Debug.Log ("Entering next scene");
		} //else
			//Debug.Log ("No new scene");

	}
}
