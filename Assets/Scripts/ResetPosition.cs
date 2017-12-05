using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetPosition : MonoBehaviour
{

    public GameObject exitCollider;
    Vector3 pos;
    Quaternion rot;

    void Start()
    {
        GetComponent<Rigidbody>().velocity = Vector3.zero;
        GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
        pos = transform.localPosition;
        rot = transform.localRotation;
    }

    void OnTriggerExit(Collider col)
    {
        if (col.gameObject == exitCollider) {
            transform.localPosition = pos;
            transform.localRotation = rot;
            GetComponent<Rigidbody>().velocity = Vector3.zero;
            GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
        }
    }
}
