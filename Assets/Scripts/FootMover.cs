using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootMover : MonoBehaviour {

    public bool originalLeftFoot;
    bool leftFoot;
    bool feetSwitched;
    public GameObject leftFootTracker;
    public GameObject rightFootTracker;
    Transform followThis;
    //GameController gc;

	void Start () {
        leftFoot = originalLeftFoot ? true : false;
        followThis = leftFoot ? leftFootTracker.transform : rightFootTracker.transform;
        //gc = GameObject.Find("GameController").GetComponent<GameController>();
	}
	
	void Update () {
        transform.position = followThis.position;
        transform.rotation = followThis.rotation;

        //if (Input.GetKeyDown(KeyCode.J)) {
        //    SwitchFoot();
        //}
	}

    public void SwitchFoot(){
        leftFoot = leftFoot ? false : true;
        followThis = leftFoot ? leftFootTracker.transform : rightFootTracker.transform;

        if(originalLeftFoot && leftFoot) {
            PlayerPrefs.SetInt("FeetSwitched", 0);
        }else if(originalLeftFoot && !leftFoot) {
            PlayerPrefs.SetInt("FeetSwitched", 1);
        }
    }
}
