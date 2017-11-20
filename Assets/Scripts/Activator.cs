using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Activator : MonoBehaviour {

    public GameObject item;
    public Transform attachedPosition;
    public UnityEvent whenPlaced;
    Vector3 origSize;

    void Start () {
        origSize = item.transform.localScale;
    }
	
	void Update () {
        if (Input.GetKeyDown(KeyCode.P)) {
            PlaceItem(item);
        }
	}

    private void OnTriggerEnter(Collider col){
        if(col.gameObject == item) {
            PlaceItem(col.gameObject);
        }
    }

    void PlaceItem(GameObject col){
        col.transform.position = attachedPosition.position;
        col.transform.rotation = attachedPosition.rotation;
        item.transform.localScale = origSize;
        col.transform.parent = this.transform;
        col.GetComponent<Rigidbody>().isKinematic = true;
        col.GetComponent<MeshCollider>().enabled = false;
        GetComponent<Animator>().enabled = true;
    }

    public void Activate(){
        whenPlaced.Invoke();
    }
}
