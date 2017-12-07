using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class ObjectSpawner : MonoBehaviour {

    public GameObject spawnedObject;
    public float destroyTimer;
    public Transform parentObject;

    GameObject spawned;
    Transform spawnPosition;
    BoxCollider exitArea;   // Gameobject is destroyed if it leaves exitArea

	void Start () {
        exitArea = GetComponent<BoxCollider>();
        var c = transform.GetChild(0).gameObject;
        spawnPosition = c.transform;
        c.transform.GetChild(0).gameObject.GetComponent<MeshRenderer>().enabled = false;        // Set visualiser off
        SpawnObject();
    }

    void OnTriggerExit(Collider col){
        //print("Something left collider");
        if(col.gameObject == spawned) {
            print("object left collider");
            Destroy(col.gameObject, destroyTimer);
            SpawnObject();
        }
    }

    void SpawnObject(){
        spawned = Instantiate(spawnedObject);
        spawned.transform.localPosition = spawnPosition.transform.position;
        spawned.transform.rotation = spawnPosition.rotation;
        spawned.transform.parent = parentObject.transform;
        //spawnedObject.transform.localScale = new Vector3(1, 1, 1);
        //spawnedObject = spawned;
    }
}
