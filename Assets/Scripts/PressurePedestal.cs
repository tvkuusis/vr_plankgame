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

    void PlaceItem(GameObject col){
        if (col.gameObject == correctItem || col.gameObject == fakeItem) {
            if (col.GetComponent<NewtonVR.NVRInteractableItem>().AttachedHand) {
                var hand = col.GetComponent<NewtonVR.NVRInteractableItem>().AttachedHand;
                hand.EndInteraction(col.gameObject.GetComponent<NewtonVR.NVRInteractable>());
            }
            ReturnToStart();
        }
        else {
            StartFalling();
        }
    }


    void ReturnToStart(){
        // Do something
    }

    void StartFalling(){
        // Do something
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