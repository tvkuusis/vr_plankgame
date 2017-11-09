using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class DestoyObjects : MonoBehaviour {


    private void OnTriggerEnter(Collider c)
    {
        if (c.CompareTag("Bottom")) {
            Destroy(gameObject, 3);
        }
    }
}
