using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bat : MonoBehaviour {

    private GameObject Controller;
    private bool inited = false;
    private float minVelocity;
    private float maxVelocity;
    private float randomness;
    private GameObject chasee;
    Rigidbody rb;

    float minDelay = 0.3f;
    float maxDelay = 0.5f;
    public float speed;

    public bool animStarted;
    float t;
    public float r;

    void Start() {
        r = Random.Range(0.00f, 0.19f);
        rb = GetComponent<Rigidbody>();
        StartCoroutine(AnimStart());
        StartCoroutine("BoidSteering");
    }

    IEnumerator AnimStart() {
        while (!animStarted) {
            t += Time.deltaTime;
            if (t >= r) {
                GetComponent<Animator>().SetTrigger("startAnimation");
                animStarted = true;
            }
            yield return null;
        }
    }

    IEnumerator BoidSteering() {
        while (true) {
            if (inited) {
                rb.velocity = rb.velocity + Calc() * Time.deltaTime;

                // enforce minimum and maximum speeds for the boids
                speed = rb.velocity.magnitude;
                if (speed > maxVelocity) {
                    rb.velocity = rb.velocity.normalized * maxVelocity;
                } else if (speed < minVelocity) {
                    rb.velocity = rb.velocity.normalized * minVelocity;
                }
            }
            SetRotation();
            float waitTime = Random.Range(minDelay, maxDelay);
            yield return new WaitForSeconds(waitTime);
        }
    }

    void SetRotation() {
        Vector3 dir = rb.velocity.normalized;
        transform.LookAt(transform.position + dir);
    }

    private Vector3 Calc() {
        Vector3 randomize = new Vector3((Random.value * 2) - 1, (Random.value * 2) - 1, (Random.value * 2) - 1);

        randomize.Normalize();
        BatFlockControl boidController = Controller.GetComponent<BatFlockControl>();
        Vector3 flockCenter = boidController.flockCenter;
        Vector3 flockVelocity = boidController.flockVelocity;
        Vector3 follow = chasee.transform.localPosition;

        flockCenter = flockCenter - transform.localPosition;
        flockVelocity = flockVelocity - rb.velocity;
        follow = follow - transform.localPosition;

        return (flockCenter + flockVelocity + follow * 2 + randomize * randomness);
    }

    public void SetController(GameObject theController) {
        Controller = theController;
        BatFlockControl boidController = Controller.GetComponent<BatFlockControl>();
        minDelay = boidController.minDelay;
        maxDelay = boidController.maxDelay;
        minVelocity = boidController.minVelocity;
        maxVelocity = boidController.maxVelocity;
        randomness = boidController.randomness;
        chasee = boidController.chasee;
        inited = true;
    }
}
