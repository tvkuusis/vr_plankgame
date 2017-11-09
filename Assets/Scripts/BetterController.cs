using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SteamVR_TrackedObject))]
public class BetterController : MonoBehaviour {

	private SteamVR_TrackedObject trackedObject;
	private SteamVR_Controller.Device device;
	PositionCalibration pc;

	public Transform calibrationPoint;

	void Awake(){
		pc = GameObject.Find("Plank").GetComponent<PositionCalibration>();
		trackedObject = GetComponent<SteamVR_TrackedObject>();
	}

	void Update(){
		device = SteamVR_Controller.Input ((int)trackedObject.index);

		if (device.GetAxis ().x != 0 || device.GetAxis ().y != 0) {
			Debug.Log (device.GetAxis ().x + " " + device.GetAxis ().y);
		}

		if (device.GetTouch (SteamVR_Controller.ButtonMask.Trigger)) {
			print ("Trigger touched");
		}

		if (device.GetPressDown (SteamVR_Controller.ButtonMask.Trigger)) {
			if (pc.calibrating) {
				pc.CalibratePosition(calibrationPoint);
				device.TriggerHapticPulse (1000);
			}
			Debug.Log ("Trigger " + device.index + " pressed");
		}
	}

	void OnTriggerStay(Collider col){
		//print("Collided with " + col.name);

		if (device.GetTouch (SteamVR_Controller.ButtonMask.Trigger)) {
			col.attachedRigidbody.isKinematic = true;
			col.gameObject.transform.SetParent(gameObject.transform);
		}

		if (device.GetTouchUp (SteamVR_Controller.ButtonMask.Trigger)) {
			col.gameObject.transform.SetParent (null);
			col.attachedRigidbody.isKinematic = false;
			TossObject (col.attachedRigidbody);
		}
	}

	void TossObject(Rigidbody rb){
		Transform origin = trackedObject.origin ? trackedObject.origin : trackedObject.transform.parent;
		if (origin != null) {
			rb.velocity = origin.TransformVector (device.velocity);
			rb.angularVelocity = origin.TransformVector (device.angularVelocity);
		} else {
			rb.velocity = device.velocity;
			rb.angularVelocity = device.angularVelocity;
		}
	}
}
