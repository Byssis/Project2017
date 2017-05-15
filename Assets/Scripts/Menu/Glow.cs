﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Glow : MonoBehaviour {

    public bool glow;
    public Selectable toggleButton;

	// Use this for initialization
	void Start () {
        // Check to see if we have stored a value 
        int savedValue = MemoryManager.LoadGlow();
        if (savedValue == 2)
            glow = false;
        else
            glow = true;
        toggleButton.GetComponent<Toggle>().isOn = glow;
    }
	
    /*
     * updateToggleValue will take a bool as argument that corresponds to toggle button value for glow effects
     * true -> write 1 to memory
     * false -> write 2 to memory
     */
	public void updateToggleValue(bool toggleValue)
    {
        glow = toggleButton.GetComponent<Toggle>().isOn;
        int glowValue;
        if (glow){
            glowValue = 1;
        } else
            glowValue = 2;

        MemoryManager.WriteGlow2Memory(glowValue);
    }
}