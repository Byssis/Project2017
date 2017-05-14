﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(LineRenderer))]
[RequireComponent(typeof(Transform))]
public class LaserRay : MonoBehaviour
{
    public Color Color { get; private set; }
    public int BounceValue;
    private int startBounceValue = 100;
    private LineRenderer laserRay;
    private Renderer renderer;
    private Transform tf; // ts = transform

    private Vector3 hitPoint;
    private Vector3 hitNormal;
    public Vector3 HitPoint { get { return hitPoint; } }
    public Vector3 HitNormal { get { return hitNormal; } }
	public Vector3 dir;

	public Component lastHitInteractable;

    public int ID;

    public bool onStack;

	// Use this for initialization
    void Start()
    {

    }
    public void SetColor(int newBounceValue, Color newColor)
    {
        float deacreaseRelativeToValue = (float)(newBounceValue) / (float)startBounceValue;
        Color = new Color(newColor.r, newColor.g, newColor.b, 1f * deacreaseRelativeToValue);
        renderer = GetComponent<Renderer>();
        renderer.material.SetColor("_MKGlowColor", Color);
        renderer.material.SetColor("_MKGlowTexColor", Color);
        renderer.material.SetColor("_Color", Color);
        renderer.material.SetColor("_TintColor", Color);
    }
    public void LateUpdate()
    {
    }
    public void GenerateLaserRay()
    {
        laserRay = GetComponent<LineRenderer>();
        tf = GetComponent<Transform>();
        Vector3 direction = tf.forward;
        Ray ray = new Ray(tf.position, direction);
        RaycastHit hit;
		dir = tf.forward;
        laserRay.SetPosition(0, tf.position);
        //Material material = laserRay.material;
        //material.color = Color;
        //kolla om vi får träff
        if (Physics.Raycast(ray, out hit))
		{
            //sätt publika egenskaper som normal och träffpunkts
            hitPoint = hit.point;
            hitNormal = hit.normal;
            //sätt slutpunkt på lasern
            laserRay.SetPosition(1, hitPoint);
            // kolla om komponenten ugör en interactable
            Component interactableObj = hit.collider.GetComponentInParent(typeof(IInteractables));
            
			if (interactableObj != null) {
				//generera event för laserträff
				lastHitInteractable = interactableObj;

				((IInteractables)interactableObj).HandleLaserCollision (this);

			} else {
				lastHitInteractable = null;
			}
        }
        else{
         laserRay.SetPosition(1, ray.GetPoint(100)); 
        }

        //System.Threading.Thread.Sleep(1000);
        
        //Debug.Log("färdig");
    }
	public void UpdateObject(){
		if(lastHitInteractable !=null)
		((IInteractables)lastHitInteractable).HandleUpdate ();	
	}
}
﻿