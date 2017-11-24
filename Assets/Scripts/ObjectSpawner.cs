using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class ObjectSpawner : MonoBehaviour {

    public GameObject spawnedObject;
    public float destroyTimer;

    BoxCollider exitArea;   // Gameobject is destroyed if it leaves exitArea

	void Start () {
        exitArea = GetComponent<BoxCollider>();
        transform.GetChild(0).gameObject.SetActive(false);        // Set visualiser off
    }

	void Update () {
		
	}

    void OnTriggerExit(Collider col){
        if(col.gameObject == spawnedObject) {
            Destroy(col, destroyTimer);
        }
    }

    void SpawnObject(){

    }
}
