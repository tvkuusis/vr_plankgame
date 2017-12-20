using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerTile : MonoBehaviour {

    float timeToTrigger = 2;
    public float t;

    public TileDropper td;
    WallShaker wallShaker;

    public bool playerInTrigger;
    public bool tileDropCalled;

    public bool dropCertainTiles;

    Color origColor;
    Renderer rend;
    Material mat;

	void Start () {
        //td = GameObject.Find("TileDropper").GetComponent<TileDropper>();
        wallShaker = GameObject.Find("WallShaker").GetComponent<WallShaker>();
        rend = GetComponentInChildren<Renderer>();
        mat = rend.material;
        origColor = mat.GetColor("_EmissionColor");
    }
	
	void Update () {
		if (playerInTrigger && !tileDropCalled) {
            t += Time.deltaTime;
            Color newColor = Color.red * Mathf.LinearToGammaSpace(t / 20f);
            mat.SetColor("_EmissionColor", newColor);

            if (t >= timeToTrigger) {
                td.DropTiles( transform.position );
                tileDropCalled = true;
            }
        }else if (tileDropCalled) {
            mat.SetColor("_EmissionColor", origColor);
        }
    }

    void Rumble(bool startStop) {
        if (startStop) {
            print("Play rumble audio + dustFromRoof particles");
            wallShaker.StartShake();
        } else {
            print("Stop rumble audio");
            wallShaker.EndShake();
        }        
    }

    public void PlayerStepsOnOffTile(bool on) {
        Debug.Log("PlayerStepsOnOffTile()");
        if (tileDropCalled) return;
        if (on) {
            if (dropCertainTiles) {
                td.DropCertainTiles();
                tileDropCalled = true;
            } else if (!playerInTrigger) {
                Rumble(true);
                playerInTrigger = true;
                print("playerintrigger = true");
            }
        } else {
            if (playerInTrigger) {
                Rumble(false);
                t = 0;
                playerInTrigger = false;
                mat.SetColor("_EmissionColor", origColor);
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
