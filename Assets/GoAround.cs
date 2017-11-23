using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoAround : MonoBehaviour {

    public float radius;
    public float speed;
    public float radiusY;
    float s;
    float x;
    float y;
    float z;
    float startX;
    float startY;
    float startZ;

    void Start () {
        startX = transform.position.x;
        startY = transform.position.y;
        startZ = transform.position.z;
        //sin(0) = 0
        //cos(0) = 1
    }
	
	void Update () {
        s += speed * Time.deltaTime;
        x = Mathf.Sin(s) * radius;
        z = Mathf.Cos(s) * radius;
        y = Mathf.Sin(s) * radiusY;
        transform.position = new Vector3(x + startX, y + startY, z);
    }
}
