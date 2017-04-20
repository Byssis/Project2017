﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Klass för att hantera reflektion map. spegeln
/// </summary>
[RequireComponent(typeof(Transform))]
public class Mirror : MonoBehaviour, IInteractables
{
    private GameObject prev;
    public GameObject ray;
    public int bounceValue;
    private Transform objTransform;

    public float mirrWidth; // x component
    public float mirrHeight; // y component
    public float mirrDepth;  // z component

    // Use this for initialization
    void Start () {
        objTransform = GetComponent<Transform>();
        mirrWidth = 6;
        mirrHeight = 3;
        mirrDepth = 1;
        transform.localScale = new Vector3(mirrWidth, mirrHeight, mirrDepth);
    }
	
	// Update is called once per frame
	void Update () {
        Destroy(prev);
    }

    public void HandleLaserCollision(LaserRay laserHit)
    {        
        //reflektera map. laserns riktningsvektor samt ytans normal
        Vector3 direction = Vector3.Reflect(laserHit.transform.forward,laserHit.HitNormal);
        prev = Instantiate(ray, laserHit.HitPoint, Quaternion.LookRotation(direction));
        LaserRay newLaser = prev.GetComponent<LaserRay>();
        //sätt färg och minska bouncevalue
        newLaser.BounceValue = laserHit.BounceValue- bounceValue;
        newLaser.Color = laserHit.Color;
         
    }
}
