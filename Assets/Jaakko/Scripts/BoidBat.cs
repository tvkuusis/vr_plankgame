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


    Rigidbody rb;

    // rule weights 0 ... 100

    public float rule1Weight;
    public float rule2Weight;
    public float rule3Weight;
    public float rule4Weight;
    public float rule5Weight;
    public float rule6Weight;
    float randomness;
    float distFromOtherBoids;

    public Transform[] otherBoids;

    void Start() {
        rb = GetComponent<Rigidbody>();
        GetComponent<Animator>().SetTrigger("startAnimation");
    }

    Vector3 Calc() {

        // Rule 1: Boids try to fly towards the centre of mass of neighbouring boids.

        Vector3 centreOfMass = (controller.flockCenter - transform.position).normalized;

        // Rule 2: Boids try to keep a small distance away from other boids.

        Vector3 batAvoidance = Vector3.zero;
        Transform closestBoid = otherBoids[0];
        for (int i = 0; i < otherBoids.Length; i++) {
            if ((transform.position - otherBoids[i].position).magnitude < (transform.position - closestBoid.position).magnitude) {
                closestBoid = otherBoids[i];
            }
        }
        if ((transform.position - closestBoid.position).magnitude < distFromOtherBoids) {
            batAvoidance = (closestBoid.position - transform.position) * -2;
        }

        // Rule 3: Boids try to match velocity with near boids.

        Vector3 velocityMatching = controller.flockVelocity - rb.velocity;

        // Rule 4: Boids are steered towards a goal (waypoints)

        Vector3 towardsGoal = (currentTarget.position - transform.position).normalized;

        // Rule 5: Boids especially try to avoid flames (players torch)

        Vector3 specialAvoidance;

        if (Vector3.Distance(transform.position, torch.position) < 0.5f) {
            specialAvoidance = (transform.position - torch.position).normalized;
        } else {
            specialAvoidance = Vector3.zero;
        }

        // Rule 6: Boids try to avoid the player

        Vector3 playerAvoidance = (transform.position - controller.playerPos).normalized;

        // Add a bit of random

        Vector3 randomize = new Vector3((Random.value * 2) - 1, (Random.value * 2) - 1, (Random.value * 2) - 1);

        // Return the vectors together with their weights

        return centreOfMass * (rule1Weight / 100) +
            batAvoidance * (rule2Weight * 100) +
            velocityMatching * (rule3Weight / 100) +
            towardsGoal * (rule4Weight / 100) +
            specialAvoidance * (rule5Weight / 100) +
            randomize * (randomness / 100) +
            playerAvoidance * (rule6Weight / 100);
    }

    public void NewWeights() {
        if (controller == null) {
            Debug.LogError("controller is null", gameObject);
            return;
        }
        randomness = controller.randomness;
        rule1Weight = controller.cOfMass;
        rule2Weight = controller.boidAvoid;
        rule3Weight = controller.vMatch;
        rule4Weight = controller.toGoal;
        rule5Weight = controller.torchAvoid;
        rule6Weight = controller.playerAvoid;
        distFromOtherBoids = controller.distFromOtherBoids;
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

    void SetRotation() {
        Vector3 dir = rb.velocity.normalized;
        transform.LookAt(transform.position + dir);
    }

    public void SetController(BoidControl givenController) {
        controller = givenController;
        minVelocity = controller.minVelocity;
        maxVelocity = controller.maxVelocity;
        randomness = controller.randomness;
        currentTarget = controller.testTarget;
        rule1Weight = controller.cOfMass;
        rule2Weight = controller.boidAvoid;
        rule3Weight = controller.vMatch;
        rule4Weight = controller.toGoal;
        rule5Weight = controller.torchAvoid;
        rule6Weight = controller.playerAvoid;
        torch = controller.torch;
        distFromOtherBoids = controller.distFromOtherBoids;
    }

    public void SetOtherBoidsArray() {

        List<Transform> boidsList = new List<Transform>() { };

        for (int i = 0; i < controller.boids.Length; i++) {
            boidsList.Add(controller.boids[i].transform);
        }
        boidsList.Remove(transform);

        otherBoids = boidsList.ToArray();

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
