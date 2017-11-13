using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnRoomStartFeet : MonoBehaviour {

	public bool thisIsLeftFoot;

	public GameObject leftStartFoot;
	public GameObject rightStartFoot;

	public bool footInPlace;
	GameController gc;

	Renderer rend;
	Color origC;

	void Start () {
		gc = GameObject.Find ("GameController").GetComponent<GameController>();
		rend = GetComponent<Renderer> ();
		origC = rend.material.color;
	}


	void OnTriggerEnter(Collider col){
		if(thisIsLeftFoot){
			if (col.transform.name == "Foot Left Model"){
				footInPlace = true;
				//print ("Left foot entered start position");
				gc.leftFootInPosition = true;
				rend.material.color = Color.green;
		    }
		}else{
			if (col.transform.name == "Foot Right Model"){
				footInPlace = true;
				//print ("Right foot entered start position");
				gc.rightFootInposition = true;
				rend.material.color = Color.green;
			}
		}
	}

	void OnTriggerExit(Collider col){
		if(thisIsLeftFoot){
			if (col.transform.name == "Foot Left Model"){
				footInPlace = false;
				//print ("Left foot left start position");
				gc.leftFootInPosition = false;
				rend.material.color = origC;
			}
		}else{
			if (col.transform.name == "Foot Right Model"){
				footInPlace = false;
				//print ("Right foot left start position");
				gc.rightFootInposition = false;
				rend.material.color = origC;
			}
		}
	}
}