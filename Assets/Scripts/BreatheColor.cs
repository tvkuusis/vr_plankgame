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

	void Start () {
        rend = GetComponent<Renderer>();
        mat = rend.material;
        origColor = mat.GetColor("_EmissionColor");
        //print(origColor);
	}
	
	void Update () {
        float emission = Mathf.PingPong(Time.time * speed, maxEmission);
        //Color baseColor = origColor; //Replace this with whatever you want for your base color at emission level '1'
        //Color basecol = new Color(1, 0, 0, emission); // someValue adjust the scale of emission
        //Color finalColor = origColor * Mathf.LinearToGammaSpace(emission);
        //mat.SetColor("_EmissionColor", finalColor);

        var distance = 1 / Vector3.Distance(transform.position, pedestal.position);
        var dist_scaled = distance / 170;
        //var dist_n = Mathf.InverseLerp(0, 150, distance); // hack
        //print(dist_n);
        Color finalColor = origColor * Mathf.LinearToGammaSpace(dist_scaled);
        mat.SetColor("_EmissionColor", finalColor);


    }
}