using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerController : MonoBehaviour {

    public Transform torchSpawn;
    public GameObject torchPrefab;

    // Use this for initialization
    void Start () {
        InstantiateTorch();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void InstantiateTorch()
    {
        var torch = Instantiate(torchPrefab, torchSpawn);
        torch.transform.SetParent(GameObject.Find("Tower").transform);
    }
}
