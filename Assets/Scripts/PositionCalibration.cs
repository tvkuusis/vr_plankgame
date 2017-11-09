using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class PositionCalibration : MonoBehaviour {

    public Transform startPosition;
    public Transform endPosition;
    int pointsCalibrated = 0;
	public GameObject startMarker;
	public GameObject endMarker;
    public float zeroY;

	public bool calibrating;
	public GameObject calibrationPole;
	float startPointDist;
	float endPointDist;

	void Start(){
		startMarker = GameObject.Find ("Start");
		endMarker = GameObject.Find ("End");

		LoadPosition ();
	}

	void Update () {
        if (Input.GetKeyDown(KeyCode.C)) {
            StartCalibration();
        }

        Debug.DrawLine(startPosition.position, endPosition.position, Color.red);
    }

    public void CalibratePosition(Transform newPosition){
        if(pointsCalibrated == 0) {
			//newPosition.position = new Vector3 (newPosition.position.x, 0, newPosition.position.z);
            startPosition = newPosition;
			PlayerPrefs.SetFloat ("PlankStartX", newPosition.transform.position.x);
			PlayerPrefs.SetFloat ("PlankStartY", newPosition.transform.position.y);
			PlayerPrefs.SetFloat ("PlankStartZ", newPosition.transform.position.z);
            pointsCalibrated++;
            print("Position " + newPosition + " added as start position.");
			startMarker.transform.position = startPosition.position;
        }else if (pointsCalibrated == 1) {
			//newPosition.position = new Vector3 (newPosition.position.x, 0, newPosition.position.z);
            endPosition = newPosition;
			PlayerPrefs.SetFloat ("PlankEndX", newPosition.transform.position.x);
			PlayerPrefs.SetFloat ("PlankEndY", newPosition.transform.position.y);
			PlayerPrefs.SetFloat ("PlankEndZ", newPosition.transform.position.z);
            print("Position " + newPosition + " added as end position.");
			endMarker.transform.position = endPosition.position;
			print("Calibration ended");
			PositionObject ();
        }
    }

	public void StartCalibration(){
        print("Calibration started");
        calibrating = true;
        pointsCalibrated = 0;
	}

	public void PositionObject(){
        Vector3 newStart = Vector3.zero;
        Vector3 newEnd = Vector3.zero;
		if (calibrationPole) {
			startPointDist = Vector3.Distance (startMarker.transform.position, calibrationPole.transform.position);
			endPointDist = Vector3.Distance (endMarker.transform.position, calibrationPole.transform.position);
		}
		if(startPointDist < endPointDist && calibrationPole) {
            newStart = endMarker.transform.position;
            newEnd = startMarker.transform.position;
        }
        else {
            newStart = startMarker.transform.position;
            newEnd = endMarker.transform.position;
        }

        // Position plank in midpoint between startposition and endposition
		Vector3 midPoint = new Vector3((newStart.x + newEnd.x) / 2, 0, (newStart.z + newEnd.z) / 2);
		transform.localPosition = midPoint;

		// Rotate object toward the startposition and fix the angle by 90 degrees
		transform.LookAt(new Vector3(newStart.x, zeroY, newStart.z));
		//transform.right = transform.forward;
		//transform.rotation = Quaternion.Euler(transform.rotation.x, transform.rotation.y, 0);
		pointsCalibrated = 0;
		calibrating = false;
	}
		
	public void LoadPosition(){
		// Temp variables
		float x = 0;
		float y = 0;
		float z = 0;

		if (PlayerPrefs.HasKey ("PlankStartX") && PlayerPrefs.HasKey ("PlankStartY") && PlayerPrefs.HasKey ("PlankStartZ")) {
			x = PlayerPrefs.GetFloat ("PlankStartX");
			y = PlayerPrefs.GetFloat ("PlankStartY");
			z = PlayerPrefs.GetFloat ("PlankStartZ");
			startMarker.transform.position = new Vector3 (x, y, z);
			print ("Plank start position loaded from memory");
		} else {
			print ("No saved start position for plank found, using default position");
		}

		x = 0; y = 0; z = 0;

		if (PlayerPrefs.HasKey ("PlankEndX") && PlayerPrefs.HasKey ("PlankEndY") && PlayerPrefs.HasKey ("PlankEndZ")) {
			x = PlayerPrefs.GetFloat ("PlankEndX");
			y = PlayerPrefs.GetFloat ("PlankEndY");
			z = PlayerPrefs.GetFloat ("PlankEndZ");
			endMarker.transform.position = new Vector3 (x, y, z);
			print ("Plank end position loaded from memory");
		} else {
			print ("No saved end position for plank found, using default position");
		}

		PositionObject ();
	}
}
