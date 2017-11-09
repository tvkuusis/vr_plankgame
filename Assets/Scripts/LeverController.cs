using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace NewtonVR
{
    public class LeverController : MonoBehaviour {
    //public GameObject RocketPrefab;
    public NVRLever Control;
    public UnityEvent onEngage;

    private GameObject RocketInstance;

    private void Awake()
    {
        StartCoroutine(Spawn());
    }

    private void Update()
    {
        if (Control.LeverEngaged == true) {
            StartCoroutine(DoSomething());
        }
    }

    public IEnumerator DoSomething()
    {
            //Rigidbody rb = RocketInstance.GetComponent<Rigidbody>();
            //rb.AddRelativeForce(new Vector3(0, 1000, 0), ForceMode.Force);

			onEngage.Invoke();

        yield return new WaitForSeconds(0.5f);

        StartCoroutine(Spawn());
    }

    private IEnumerator Spawn()
    {
            //RocketInstance = (GameObject)GameObject.Instantiate(RocketPrefab, this.transform.position, this.transform.rotation);
            //RocketInstance.GetComponent<Rigidbody>().isKinematic = true;
            //RocketInstance.GetComponent<NVRInteractableItem>().CanAttach = false;

            //Vector3 startScale = Vector3.one * 0.1f;
            //Vector3 endScale = Vector3.one;

            float startTime = Time.time;
            float overTime = 0.5f;
            float stopTime = startTime + overTime;

            while (Time.time < stopTime) {
                //RocketInstance.transform.localScale = Vector3.Lerp(startScale, endScale, (Time.time - startTime) / overTime);
                yield return null;
            }

            //RocketInstance.GetComponent<Rigidbody>().isKinematic = false;
            //RocketInstance.GetComponent<NVRInteractableItem>().CanAttach = true;
        }
}
}