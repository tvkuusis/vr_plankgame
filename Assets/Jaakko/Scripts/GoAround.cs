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

    bool doCircles;

    public Transform goAroundWaypoint;

    void Start () {
        //startX = transform.position.x;
        //startY = transform.position.y;
        //startZ = transform.position.z;

        startX = goAroundWaypoint.position.x;
        startY = goAroundWaypoint.position.y;
        startZ = goAroundWaypoint.position.z;

        //sin(0) = 0
        //cos(0) = 1
    }

    public void StartCircling() {
        if (!doCircles) doCircles = true;
    }

    public void StopCircling() {
        if (doCircles) doCircles = false;
    }
	
	void Update () {
        if (!doCircles) return;
        s += speed * Time.deltaTime;
        x = Mathf.Sin(s) * radius;
        z = Mathf.Cos(s) * -radius;
        y = Mathf.Sin(s * 5) * radiusY;
        transform.position = new Vector3(x + startX, y + startY, z);
    }
}
