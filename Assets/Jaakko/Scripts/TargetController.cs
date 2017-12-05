using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetController : MonoBehaviour {

    public float targetSpeed;

    public Transform[] waypoints;

    public BoidControl bc;

    public Transform goAroundWaypoint;
    GoAround goAround;

    public bool stopGoAround;

    bool skipOnce;

    bool started;

	void Start () {
        goAround = GetComponent<GoAround>();
		if (waypoints.Length < 1) {
            Debug.LogError("Target's waypoints.Length = 0 :: " + this.name);
        } else {
            
            transform.position = bc.transform.position;
        }
	}

    public void StartBatsAndTarget()
    {
        if (started) return;
        StartCoroutine(TargetMove());
        started = true;
    }

    IEnumerator TargetMove() {

        while (!bc.boidsCreated) yield return null;
        bc.StartBats();
        Debug.Log("TargetMove() :: " + this.name);

        for (int i = 0; i < waypoints.Length; i++) {
            print(waypoints[i].name);
            //if (waypoints[i] == goAroundWaypoint && !stopGoAround) {                
            //    yield return StartCoroutine(GoAround());
            //}

            if (!skipOnce) {
                while (transform.position != waypoints[i].position) {
                    if (Vector3.Distance(transform.position, waypoints[i].position) > targetSpeed * Time.deltaTime * 2) {
                        transform.position += (waypoints[i].position - transform.position).normalized * targetSpeed * Time.deltaTime;
                    } else if (Vector3.Distance(transform.position, waypoints[i].position) <= targetSpeed * Time.deltaTime) {
                        if (waypoints[i] == goAroundWaypoint && !stopGoAround) {
                            yield return StartCoroutine(GoAround(waypoints[i].position));
                        } else {
                            transform.position = waypoints[i].position;
                        }                        
                    } else if (Vector3.Distance(transform.position, waypoints[i].position) < targetSpeed * Time.deltaTime * 2) {
                        transform.position += (waypoints[i].position - transform.position).normalized * targetSpeed * Time.deltaTime * 1.01f;
                    }
                    yield return null;
                }
            } else {
                skipOnce = false;
            }            
        }
        Debug.Log("Target's waypoints complete :: " + this.name);
        bc.BatsAreDone();
        gameObject.SetActive(false);
    }

    IEnumerator GoAround(Vector3 pos) {
        transform.position = pos;
        //Debug.LogError("just for the pause");
        goAround.StartCircling();

        while (!stopGoAround) yield return null;
        goAround.StopCircling();
        skipOnce = true;
    }

    public void StopGoingAround() {
        stopGoAround = true;
    }

	void Update () {
		
	}
}
