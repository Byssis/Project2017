﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movable : MonoBehaviour
{
    private Vector3 previousPosition;
    private Quaternion prevRotate;
    private bool move; // Move mode
    private float distance; //Distance moved in display coordinates
    private float startHeight;
    private Rigidbody rigidb;
    private Vector3 distanceOffset;
    private Vector3 rayPoint;
    private Ray r;
    private Vector3 firstTouchPos;
    public float moveHeight;
    public float offsetTouch;
    private MirrorInactive activateButton;
    private bool prevMove;

    // Can be applied to any object with a rigidbody
    void Start()
    {
        this.rigidb = GetComponent<Rigidbody>();
        GetComponent<Rigidbody>().freezeRotation = true;
        move = false;
        previousPosition = rigidb.position;
        startHeight = rigidb.position.y;
        offsetTouch = 1.2f;
        if (transform.parent != null)
			activateButton = transform.parent.GetComponentInChildren<MirrorInactive> ();
    }

    private void Update()
    {
        this.rigidb.velocity = Vector3.zero;
        this.rigidb.freezeRotation = true;
        if(!move)transform.position = new Vector3(transform.position.x, startHeight, transform.position.z);
    }

    private void OnMouseDown()
    {
        //Debug.Log (activateButton + " 10");
        if (activateButton == null || activateButton.IsActivated())
        {
            //	Debug.Log (activateButton + "0");
            if (GameManager.gameMode == GameManager.GameMode.mirrorMode)
            { // If in mirror mode, pick up mirror
                firstTouchPos = Input.mousePosition;
                previousPosition = rigidb.position;
                prevRotate = transform.rotation;
                move = true;
                updateTouchPoint();
                distanceOffset = rayPoint - previousPosition;
            }
        }
    }

    private void OnMouseDrag()
    {
        //Debug.Log (activateButton + "1");
        // calc difference between first touch and the next touch
        GetComponent<Rotate>().rotated = false;
        float diff = Vector3.Distance(firstTouchPos, Input.mousePosition);
        if (move && GameManager.gameMode == GameManager.GameMode.mirrorMode && diff > offsetTouch)
        {   //If in move,mirror mode and greater than offset, enable to move object
            updateTouchPoint();
            rigidb.position = rayPoint - distanceOffset;
            rigidb.position = new Vector3(rigidb.position.x, startHeight, rigidb.position.z);
            GetComponent<Collider>().isTrigger = true;
        }
    }

    private void OnMouseUp()
    {
        if (GameManager.gameMode == GameManager.GameMode.mirrorMode) //If in mirror mode
        {
            //Debug.Log (activateButton + "2");
            rigidb.position = new Vector3(rigidb.position.x, startHeight, rigidb.position.z);
            move = false;
            GetComponent<Collider>().isTrigger = false;
        }
    }

    private void updateTouchPoint()
    {
        r = Camera.main.ScreenPointToRay(Input.mousePosition);
        distance = Vector3.Distance(rigidb.position, Camera.main.transform.position);
        rayPoint = r.GetPoint(distance);
    }

    private void OnCollisionStay(Collision col)
    {
        //Move to previous position if collision
        //        GetComponent<Rigidbody>().position = previousPosition;
        //        GetComponent<Rigidbody>().velocity = Vector3.zero;
        /*if (col.collider.name.Equals("Plane"))
        {
            Debug.Log("Plane");
        }
        else
        {
            Debug.Log("Bajs");
        }*/
    }

    public bool getMove()
    {
        return move;
    }
}