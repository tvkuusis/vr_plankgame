using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFallScript : MonoBehaviour {

    public bool grounded;
	MeshRenderer rend;
	Color origC;

	void Awake(){
		rend = GetComponentInParent<MeshRenderer> ();
		origC = rend.material.color;
	}

    void OnTriggerStay(Collider col){
        if (col.CompareTag("Ground") && !grounded) {
            grounded = true;
            //print(gameObject.transform.parent.name +  " grounded.");
			rend.material.color = origC;
			//rend.material.color = new Color (0, 1, 0, 1);
        }
    }

    void OnTriggerExit(Collider col){
        if (col.CompareTag("Ground") && grounded) {
            grounded = false;
            //print(gameObject.transform.parent.name + " not grounded.");
			//rend.material.color = new Color (1, 0, 0, 1);
			rend.material.color = new Color (origC.r, origC.g, origC.b, 0.6f);
        }
    }
}
