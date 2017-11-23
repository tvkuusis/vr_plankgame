using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoidControl : MonoBehaviour {

    public float minVelocity = 5;
    public float maxVelocity = 20;
    public float randomness = 1;
    public int flockSize = 20;
    public GameObject batPrefab;

    public Vector3 flockCenter;
    public Vector3 flockVelocity;

    public GameObject[] boids;

    Collider controlCollider;

    public Transform testTarget;
    public Transform torch;

    public float rule1Weight;
    public float rule2Weight;
    public float rule3Weight;
    public float rule4Weight;
    public float rule5Weight;

    void Start() {
        controlCollider = GetComponent<Collider>();
        boids = new GameObject[flockSize];
        for (var i = 0; i < flockSize; i++) {
            Vector3 position = new Vector3(
                Random.value * controlCollider.bounds.size.x,
                Random.value * controlCollider.bounds.size.y,
                Random.value * controlCollider.bounds.size.z
            ) - controlCollider.bounds.extents;

            //GameObject boid = Instantiate(prefab, transform.position, transform.rotation) as GameObject;
            GameObject boid = Instantiate(batPrefab, position, Quaternion.identity, transform);
            boid.GetComponent<BoidBat>().SetController(this);
            boids[i] = boid;
        }
    }

    void Update() {

        Vector3 theCenter = Vector3.zero;
        Vector3 theVelocity = Vector3.zero;

        for (int i = 0; i < boids.Length; i++) {
            theCenter = theCenter + boids[i].transform.localPosition;
            theVelocity = theVelocity + boids[i].GetComponent<Rigidbody>().velocity;
        }

        flockCenter = theCenter / (flockSize);
        flockVelocity = theVelocity / (flockSize);
    }
}
