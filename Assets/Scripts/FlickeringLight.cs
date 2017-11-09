using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlickeringLight : MonoBehaviour {

    Light light;
    float origIntensity;
    float currentIntensity;
    public float minIntensityHigh = 0.9f;
    public float minIntensityLow = 0.8f;
    public float maxIntensityHigh = 1.0f;
    public float maxIntensityLow = 0.9f;
    public float flickerSpeed = 1;
    bool dimming = true;
    float currentMinAlpha;
    float currentMaxAlpha;


	void Start () {
        light = GetComponent<Light>();
        origIntensity = light.intensity;
        RandomizeMinAlpha();
        RandomizeMaxAlpha();
        currentIntensity = currentMaxAlpha;
	}

	void Update () {
        
        if (dimming) {
            currentIntensity -= Time.deltaTime * flickerSpeed;
            if(currentIntensity < currentMinAlpha) {
                currentIntensity = currentMinAlpha;
                dimming = false;
                RandomizeMinAlpha();
            }
        }
        else {
            currentIntensity += Time.deltaTime * flickerSpeed;
            if(currentIntensity >= currentMaxAlpha) {
                currentIntensity = currentMaxAlpha;
                dimming = true;
                RandomizeMaxAlpha();
            }
        }
		SetIntensity ();
	}

    void RandomizeMinAlpha(){
        currentMinAlpha = Random.Range(minIntensityLow, minIntensityHigh);
    }

    void RandomizeMaxAlpha(){
        currentMaxAlpha = Random.Range(maxIntensityLow, maxIntensityHigh);
    }

	void SetIntensity(){
        light.intensity = currentIntensity;
	}
}
