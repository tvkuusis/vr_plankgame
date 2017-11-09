using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallChecker : MonoBehaviour {

    GameController gc;

    void Start(){
        gc = GameObject.Find("GameController").GetComponent<GameController>();
    }

    void OnTriggerStay(Collider col){
        if (col.CompareTag("Player")) {
            gc.FallCheck();
        }
    }
}
