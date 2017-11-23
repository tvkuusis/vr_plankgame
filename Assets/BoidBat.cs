using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoidBat : MonoBehaviour {

    private BoidControl controller;
    private bool inited = false;
    private float minVelocity;
    private float maxVelocity;
    public Transform currentTarget;
    public Transform torch;

    public float speed;

    float randomness;

    Rigidbody rb;

    // rule weights 0 ... 100

    public float rule1Weight;
    public float rule2Weight;
    public float rule3Weight;
    public float rule4Weight;
    public float rule5Weight;

    void Start() {
        rb = GetComponent<Rigidbody>();
        r = Random.Range(0.00f, 0.19f);
        StartCoroutine(AnimStart());
    }

    Vector3 Calc() {

        // Rule 1: Boids try to fly towards the centre of mass of neighbouring boids.

        Vector3 centreOfMass = controller.flockCenter - transform.position;

        // Rule 2: Boids try to keep a small distance away from other boids.

        Vector3 batAvoidance = Vector3.zero;

        // Rule 3: Boids try to match velocity with near boids.

        Vector3 velocityMatching = controller.flockVelocity - rb.velocity;

        // Rule 4: Boids are steered towards a goal (waypoints)

        Vector3 towardsGoal = currentTarget.position - transform.position;

        // Rule 5: Boids especially try to avoid flames (players torch)

        Vector3 specialAvoidance;

        if (Vector3.Distance(transform.position, torch.position) < 0.5f) {
            specialAvoidance = transform.position - torch.position;
        } else {
            specialAvoidance = Vector3.zero;
        }

        // Add a bit of random

        Vector3 randomize = new Vector3((Random.value * 2) - 1, (Random.value * 2) - 1, (Random.value * 2) - 1);

        // Return the vectors together with their weights

        return centreOfMass * (rule1Weight / 100) + batAvoidance * (rule2Weight * 100) + velocityMatching * (rule3Weight / 100) + towardsGoal * (rule4Weight / 100) + specialAvoidance * (controller.rule5Weight / 100) + randomize * randomness;
    }

    void FixedUpdate() {
        if (!inited) return;

        rb.velocity = rb.velocity + Calc();

        // enforce minimum and maximum speeds for the boids

        speed = rb.velocity.magnitude;

        if (speed > maxVelocity) {
            rb.velocity = rb.velocity.normalized * maxVelocity;
        } else if (speed < minVelocity) {
            rb.velocity = rb.velocity.normalized * minVelocity;
        }

        speed = rb.velocity.magnitude;

        SetRotation();
    }

    //IEnumerator BoidSteering() {
    //    while (true) {
    //        if (inited) {
    //            rb.velocity = rb.velocity + Calc() * Time.deltaTime;

    //            // enforce minimum and maximum speeds for the boids
    //            speed = rb.velocity.magnitude;
    //            if (speed > maxVelocity) {
    //                rb.velocity = rb.velocity.normalized * maxVelocity;
    //            } else if (speed < minVelocity) {
    //                rb.velocity = rb.velocity.normalized * minVelocity;
    //            }
    //        }
    //        SetRotation();
    //        float waitTime = Random.Range(minDelay, maxDelay);
    //        yield return new WaitForSeconds(waitTime);
    //    }
    //}

    void SetRotation() {
        Vector3 dir = rb.velocity.normalized;
        transform.LookAt(transform.position + dir);
    }

    //private Vector3 Calc() {
    //    Vector3 randomize = new Vector3((Random.value * 2) - 1, (Random.value * 2) - 1, (Random.value * 2) - 1);

    //    randomize.Normalize();
    //    BatFlockControl boidController = Controller.GetComponent<BatFlockControl>();
    //    Vector3 flockCenter = boidController.flockCenter;
    //    Vector3 flockVelocity = boidController.flockVelocity;
    //    Vector3 follow = chasee.transform.localPosition;

    //    flockCenter = flockCenter - transform.localPosition;
    //    flockVelocity = flockVelocity - rb.velocity;
    //    follow = follow - transform.localPosition;

    //    return (flockCenter + flockVelocity + follow * 2 + randomize * randomness);
    //}

    public void SetController(BoidControl givenController) {
        controller = givenController;
        minVelocity = controller.minVelocity;
        maxVelocity = controller.maxVelocity;
        randomness = controller.randomness;
        currentTarget = controller.testTarget;
        rule1Weight = controller.rule1Weight;
        rule2Weight = controller.rule2Weight;
        rule3Weight = controller.rule3Weight;
        rule4Weight = controller.rule4Weight;
        rule5Weight = controller.rule5Weight;
        torch = controller.torch;
        inited = true;
    }

    public bool animStarted;
    float t;
    public float r;

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
}
