using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallShaker : MonoBehaviour {

    public Transform[] transformsToShake;
    public AudioSource[] audioSources;
    float[] transformYheights;

    public float shakeAmount = 1;
    public float shakeSpeed = 1;
    float dir = 1;
    float startY;

    public bool shake;

	void Start () {
        if (transformsToShake.Length == 0) {
            Debug.LogWarning("transformsToShake.Length = 0");
        } else {
            List<float> floatList = new List<float>() { };
            for (int i = 0; i < transformsToShake.Length; i++) {
                floatList.Add(transformsToShake[i].position.y);
            }
            transformYheights = floatList.ToArray();
            //Invoke("StartShake", 2);
        }
	}

    public void StartShake() {
        if (transformsToShake.Length == 0) {
            Debug.LogWarning("transformsToShake.Length = 0");
        } else {
            startY = transformsToShake[0].position.y;
            shake = true;
        }

        if (audioSources.Length == 0) {
            Debug.LogWarning("audioSources.Length = 0");
        } else {
            for (int i = 0; i < audioSources.Length; i++) {
                audioSources[i].Play();
            }
        }

        //Invoke("EndShake", 2.8f);
    }

    public void EndShake() {
        shake = false;
        if (audioSources.Length == 0) {
            Debug.LogWarning("audioSources.Length = 0");
        } else {
            for (int i = 0; i < audioSources.Length; i++) {
                audioSources[i].Stop();
            }
        }
    }

    void Shake() {

        if (transformsToShake[0].position.y > startY + (shakeAmount / 2)) {
            dir = -1;
        } else if (transformsToShake[0].position.y < startY - (shakeAmount / 2)) {
            dir = 1;
        }

        for (int i = 0; i < transformsToShake.Length; i++) {
            float y = shakeSpeed * Time.deltaTime * dir;
            transformsToShake[i].position += new Vector3(0, y, 0);
        }
    }

	void Update () {
        if (!shake) return;

        Shake();
    }
}
