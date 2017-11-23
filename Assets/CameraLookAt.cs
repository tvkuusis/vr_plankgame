using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraLookAt : MonoBehaviour {

    public BoidControl bc;

    void LateUpdate() {
        if (bc) {
            transform.LookAt(bc.flockCenter);
        }
    }
}
