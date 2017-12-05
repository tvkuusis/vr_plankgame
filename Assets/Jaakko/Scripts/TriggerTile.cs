using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerTile : MonoBehaviour {

    float timeToTrigger = 2;
    float t;

    TileDropper td;

    bool playerInTrigger;
    bool tileDropCalled;

    public bool dropCertainTiles;    

	void Start () {
        td = GameObject.Find("TileDropper").GetComponent<TileDropper>();
	}
	
	void Update () {
		if (playerInTrigger && !tileDropCalled) {
            t += Time.deltaTime;
            if (t >= timeToTrigger) {
                td.DropTiles();
                tileDropCalled = true;
            }
        }
	}

    void Rumble(bool startStop) {
        if (startStop) {
            print("Play rumble audio + dustFromRoof particles");
        } else {
            print("Stop rumble audio");
        }        
    }

    public void PlayerStepsOnOffTile(bool on) {
        if (tileDropCalled) return;
        if (on) {
            if (dropCertainTiles) {
                td.DropCertainTiles();
                tileDropCalled = true;
            } else if (!playerInTrigger) {
                Rumble(true);
                playerInTrigger = true;
            }
        } else {
            if (playerInTrigger) {
                Rumble(false);
                t = 0;
                playerInTrigger = false;
            }
        }
    }

    //private void OnTriggerEnter(Collider c) {
    //    if (tileDropCalled || c.tag != "Player") return;
    //    if (dropCertainTiles) {
    //        td.DropCertainTiles();
    //        tileDropCalled = true;
    //    } else if (!playerInTrigger) {
    //        Rumble(true);
    //        playerInTrigger = true;
    //    }

    //}

    //private void OnTriggerExit(Collider c) {
    //    if (tileDropCalled) return;
    //    if (c.tag == "Player" && playerInTrigger) {
    //        Rumble(false);
    //        t = 0;
    //        playerInTrigger = false;
    //    }
    //}
}
