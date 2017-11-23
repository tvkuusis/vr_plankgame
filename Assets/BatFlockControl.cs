using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatFlockControl : MonoBehaviour {

    public float minVelocity = 5;
    public float maxVelocity = 20;
    public float randomness = 1;
    public int flockSize = 20;
    public GameObject prefab;
    public GameObject chasee;

    public Vector3 flockCenter;
    public Vector3 flockVelocity;

    private GameObject[] boids;

    Collider controlCollider;

    public float minDelay;
    public float maxDelay;

    void Start() {
        controlCollider = GetComponent<Collider>();
        boids = new GameObject[flockSize];
        for (var i = 0; i < flockSize; i++) {
            Vector3 position = new Vector3(
                Random.value * controlCollider.bounds.size.x,
                Random.value * controlCollider.bounds.size.y,
                Random.value * controlCollider.bounds.size.z
            ) - controlCollider.bounds.extents;

            GameObject boid = Instantiate(prefab, transform.position, transform.rotation) as GameObject;
            boid.transform.parent = transform;
            boid.transform.localPosition = position;
            boid.GetComponent<Bat>().SetController(gameObject);
            boids[i] = boid;
        }
    }

    void Update() {
        Vector3 theCenter = Vector3.zero;
        Vector3 theVelocity = Vector3.zero;

        foreach (GameObject boid in boids) {
            theCenter = theCenter + boid.transform.localPosition;
            theVelocity = theVelocity + boid.GetComponent<Rigidbody>().velocity;
        }

        flockCenter = theCenter / (flockSize);
        flockVelocity = theVelocity / (flockSize);
    }
}
