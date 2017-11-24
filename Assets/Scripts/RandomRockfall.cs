using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomRockfall : MonoBehaviour {

    public float maxPosX;
    public float minPosX;
    public float maxPosZ;
    public float minPosZ;

    public float height;

    public float minWaitTime;

    public float maxWaitTime;

    public List<GameObject> rockSelection;



    // Use this for initialization
    void Start()
    {

        //rockSelection = new List<GameObject>();

        StartCoroutine(RockFall());

    }

    // Update is called once per frame
    void Update()
    {

    }


    IEnumerator RockFall()
    {
        while (true)
        {
            Vector3 location = new Vector3(Random.Range(minPosX, maxPosX), height, Random.Range(minPosZ, maxPosZ));

            var randomRock = Random.Range(0, rockSelection.Count);

            float rockAmount = Random.Range(0, rockSelection.Count);

            for (int i = 0; i < rockAmount; i++)
            {

                Vector3 offset = new Vector3(Random.Range(10, 50), 0, Random.Range(10, 50));

                Instantiate(rockSelection[randomRock], location + offset, Quaternion.identity);

            }

            float waitTime = Random.Range(minWaitTime, maxWaitTime);

            yield return new WaitForSeconds(waitTime);
        }
    }
}
