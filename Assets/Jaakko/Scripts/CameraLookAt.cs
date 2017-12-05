using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraLookAt : MonoBehaviour {

    public BoidControl bc;
    public Transform target;

    public void Start() {
        if (bc) target = bc.testTarget;
    }

    private void Update() {
        if (target) transform.LookAt(target);
    }

    //void LateUpdate() {
    //    if (bc) {
    //        transform.LookAt(bc.flockCenter);
    //    }
    //}
}
