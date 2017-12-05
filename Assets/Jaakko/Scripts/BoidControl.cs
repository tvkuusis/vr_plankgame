using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoidControl : MonoBehaviour {

    public float minVelocity = 5;
    public float maxVelocity = 20;
    public int flockSize = 20;
    public GameObject batPrefab;

    public Vector3 flockCenter;
    public Vector3 flockVelocity;
    public Vector3 playerPos;
    public Transform player;

    public GameObject[] boids;

    Collider controlCollider;

    public Transform testTarget;
    public Transform torch;

    public float cOfMass;
    public float boidAvoid;
    public float vMatch;
    public float toGoal;
    public float torchAvoid;
    public float playerAvoid;
    public float randomness = 1;

    public bool setNewWeights;
    public bool newWeightsSet;

    public bool boidsCreated;

    void Start() {
        controlCollider = GetComponent<Collider>();
        StartCoroutine(CreateBoids());
    }

    public void BatsAreDone() {
        for (int i = boids.Length - 1; i >= 0; i--) {
            boids[i].SetActive(false);
        }
        gameObject.SetActive(false);
    }

    IEnumerator CreateBoids() {
        boids = new GameObject[flockSize];
        for (var i = 0; i < flockSize; i++) {
            Vector3 position = new Vector3(
                Random.value * controlCollider.bounds.size.x,
                Random.value * controlCollider.bounds.size.y,
                Random.value * controlCollider.bounds.size.z
            ) - controlCollider.bounds.extents;

            GameObject boid = Instantiate(batPrefab, position + transform.position, Quaternion.identity, transform);
            boid.GetComponent<BoidBat>().SetController(this);
            boids[i] = boid;
            yield return new WaitForSeconds(Time.deltaTime);
        }
        boidsCreated = true;
        newWeightsSet = true;
    }

    void SetNewWeights() {
        newWeightsSet = false;

        // set them here

        for (var i = 0; i < flockSize; i++) {
            boids[i].GetComponent<BoidBat>().NewWeights();
        }

        newWeightsSet = true;
        setNewWeights = false;
    }

    void Update() {

        if (!boidsCreated) return;

        if (setNewWeights) SetNewWeights();

        Vector3 theCenter = Vector3.zero;
        Vector3 theVelocity = Vector3.zero;
        playerPos = player.position;

        for (int i = 0; i < boids.Length; i++) {
            theCenter = theCenter + boids[i].transform.localPosition;
            theVelocity = theVelocity + boids[i].GetComponent<Rigidbody>().velocity;
        }

        flockCenter = theCenter / (flockSize);
        flockVelocity = theVelocity / (flockSize);
    }
}
