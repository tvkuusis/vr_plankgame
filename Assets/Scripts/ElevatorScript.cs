using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevatorScript : MonoBehaviour {

    public Transform startPosition;
    public Transform midPosition;
    public Transform endPosition;
	AudioSource[] audios;
	AudioSource moveSound;
	AudioSource stopSound;

    GameController gc;
    TowerController tc;

    Transform currentStart;
    Transform currentEnd;

    public float duration = 5f;
    //public float speed = 1f;
    float startTime;

    float movedDistance = 0;
    public bool activated;

    public GameObject pillar;

	void Start () {
		audios = GetComponents<AudioSource> ();
		moveSound = audios [0];
		stopSound = audios [1];
        gc = GameObject.Find("GameController").GetComponent<GameController>();
        tc = GameObject.Find("Tower").GetComponent<TowerController>();
        transform.position = midPosition.position;
        currentStart = midPosition;
        currentEnd = midPosition;
		//moveSound.Play ();
	}

	void Update () {
        if (Input.GetKeyDown(KeyCode.E)) {
            //ActivateElevator();
        }

        if (activated) {
            MoveElevator();
        }
	}

    void MoveElevator(){
        //movedDistance += Time.deltaTime * speed;
        float t = (Time.time - startTime) / duration;
        movedDistance = Mathf.SmoothStep(0, 1, t);

        transform.position = Vector3.Lerp(currentStart.position, currentEnd.position, movedDistance);

        if(transform.position == currentEnd.position) {
			movedDistance = 0;
            activated = false;
            pillar.SetActive(false);
			moveSound.Stop ();
			stopSound.Play ();
        }
    }

    void MoveToPlayer(){
		moveSound.Play ();
        currentStart = startPosition;
        currentEnd = midPosition;
    }

    public void MoveToLevelEnd(){
        tc.MovePlayerWithElevator();
        currentStart = currentEnd;
        currentEnd = endPosition;
        ActivateElevator();
    }

    public void ActivateElevator(){
        activated = true;
        startTime = Time.time;
		moveSound.Play ();
    }


}
