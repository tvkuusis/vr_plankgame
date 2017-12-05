using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PressurePedestal : MonoBehaviour
{

    //public UnityEvent OnRemove;
    //public UnityEvent OnPlace;

    //GameObject placedObject;

    //public float puzzleTime;
    //float timeLeft;

    //bool itemPlaced = true;


    public GameObject correctItem;
    public GameObject fakeItem;
    GameObject placedItem;
    public Transform attachedPosition;


    public GameObject bars;
    BarsActivation ba;

    bool placed;

    Vector3 origSize;


    void Start(){
        ba = bars.GetComponent<BarsActivation>();
    }

    void Update()
    {

        //if (itemPlaced) {
        //    print("täällä on kamaa");
        //    //Do nothing?
        //}
        //else {
        //    print("tääl ei oo mitään :(");
        //    //Make floor collapse?

        //    timeLeft -= Time.deltaTime;

        //    if (timeLeft <= 0) {
        //        FloorCollapse();
        //    }
        //}

    }

    private void OnTriggerEnter(Collider col)
    {
        PlaceItem(col.gameObject);
    }

    private void OnTriggerExit(Collider col)
    {
        RemoveItem(col.gameObject);
    }

    void PlaceItem(GameObject col){

        if (!placed) {
            origSize = col.transform.localScale;
            col.transform.position = attachedPosition.position;
            col.transform.rotation = attachedPosition.rotation;
            col.transform.localScale = origSize;
            col.gameObject.GetComponent<Rigidbody>().isKinematic = true;

                if (col.GetComponent<NewtonVR.NVRInteractableItem>().AttachedHand) {
                    var hand = col.GetComponent<NewtonVR.NVRInteractableItem>().AttachedHand;
                    hand.EndInteraction(col.gameObject.GetComponent<NewtonVR.NVRInteractable>());
                }

            if (col.gameObject == correctItem || col.gameObject == fakeItem) { // Placed original or fake object
                DeactivateTrap();
            }
            else { // Placed wrong object
                ActivateTrap();
            }
            placed = true;
            placedItem = col;
        }
    }

    void RemoveItem(GameObject col){
        if (placed) {
            if (col == placedItem) { // Check if leaving object is the one on the pedestal, activate trap if it is
                ActivateTrap();
            }
            placed = false;
        }
    }

    void DeactivateTrap(){
        // Do something
        ba.Deactivate();
    }

    void ActivateTrap(){
        // Do something
        ba.Activate();
    }

    //void Start

    //private void OnTriggerStay(Collider other)
    //{
    //    OnPlace.Invoke();

    //    itemPlaced = true;

    //    placedObject = other.gameObject;
    //}


    //private void OnTriggerExit(Collider other)
    //{
    //    StartCountDown();

    //    placedObject = null;

    //    OnRemove.Invoke();
    //}


    //void FloorCollapse()
    //{
    //    //floor collapses?
    //}

    //void StartCountDown()
    //{

    //    itemPlaced = false;
    //    timeLeft = puzzleTime;


    //}
}