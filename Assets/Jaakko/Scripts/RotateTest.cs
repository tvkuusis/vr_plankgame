using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateTest : MonoBehaviour {

	// Use this for initialization
	void Start () {
        int r = Random.Range(0, 2);
        transform.Rotate(Vector3.up, 180 * r);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
