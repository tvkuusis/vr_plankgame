using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreatheColor : MonoBehaviour {

    public float speed;
    public float maxEmission;
    public Transform pedestal;

    Renderer rend;
    Material mat;
    Color origColor;
    Vector3 origPos;

	void Start () {
        rend = GetComponent<Renderer>();
        mat = rend.material;
        origColor = mat.GetColor("_EmissionColor");
        origPos = transform.localPosition;
        //print(origColor);
	}
	
	void Update () {
        SetEmission();
    }

    void SetEmission(){
        float emission = Mathf.PingPong(Time.time * speed, maxEmission);

        var distance = Vector3.Distance(transform.position, pedestal.position);
        print(distance);
        var emissionlevel = maxEmission - distance * 5; // hack

        Color finalColor = origColor * Mathf.LinearToGammaSpace(emissionlevel);
        mat.SetColor("_EmissionColor", finalColor);
    }
}