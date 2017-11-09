using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatVertically : MonoBehaviour {

	Vector3 origPos;
	public float floatSpeed;
	public float amplitude;
	float y = 0;
	float offset;
	// Use this for initialization
	void Start () {
		origPos = transform.position;
		offset = Random.Range (0f, 10f);
	}
	
	// Update is called once per frame
	void Update () {
		y = Mathf.Sin(Time.time * floatSpeed + offset);
		transform.position = new Vector3 (transform.position.x, origPos.y + y * amplitude, transform.position.z);
	}
}
