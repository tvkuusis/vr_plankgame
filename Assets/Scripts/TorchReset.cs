using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TorchReset : MonoBehaviour {

    //GameController gc;
    TowerController tc;
	public float destroyTime = 5f;
	bool leftPlayArea;

	void Start () {
        //gc = GameObject.Find ("GameController").GetComponent<GameController> ();
        tc = GameObject.Find("Tower").GetComponent<TowerController>();
	}

	void Update () {
		var distance = Vector3.Distance(Vector3.zero, transform.position);
		if (distance > 4 && !leftPlayArea) {
			leftPlayArea = true;
            //(gc.InstantiateTorch ();
            tc.InstantiateTorch();
			Destroy (gameObject, destroyTime);
		}
		//print (distance);
	}
}
