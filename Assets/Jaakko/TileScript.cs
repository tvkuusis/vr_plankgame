﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileScript : MonoBehaviour {

    float dropSpeed = 1;

    bool drop;
	
	public void Drop() {
        GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
        GetComponent<Rigidbody>().useGravity = true;
    }    
}
