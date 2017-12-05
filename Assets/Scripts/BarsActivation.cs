using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarsActivation : MonoBehaviour {

    public Transform start;
    public Transform end;

    Transform currentStart;
    Transform currentEnd;

    public float duration = 5f;

    float startTime;
    public bool activated;

    float movedDistance = 0;

	void Start () {
        transform.localPosition = start.localPosition;
	}
	
	void Update () {
        if (Input.GetKeyDown(KeyCode.P)) {
            Activate();
        }
        if (Input.GetKeyDown(KeyCode.U)) {
            Deactivate();
        }
        MoveBars();
	}

    private void OnDrawGizmos(){
        Gizmos.color = new Color(1, 0, 0, 0.5f);
        Gizmos.DrawSphere(start.position, 0.05f);
        Gizmos.DrawSphere(end.position, 0.05f);
    }

    void MoveBars(){
        if (activated) {
            float t = (Time.time - startTime) / duration;
            movedDistance = Mathf.SmoothStep(0, 1, t);

            transform.localPosition = Vector3.Lerp(currentStart.localPosition, currentEnd.localPosition, movedDistance);
            print(movedDistance);
            if(transform.localPosition.y == currentEnd.localPosition.y) {
                Disable();

                if(currentEnd == end) {
                    print("dead");
                }
            }
        }
    }

    public void Activate()
    {
        currentStart = transform;
        currentEnd = end;
        activated = true;
        startTime = Time.time;
        //moveSound.Play();
    }

    public void Deactivate(){
        currentStart = transform;
        currentEnd = start;
        activated = true;
        startTime = Time.time;
    }

    void Disable()
    {
        activated = false;
    }

}
