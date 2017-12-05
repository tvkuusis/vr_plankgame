using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomRockfall : MonoBehaviour {

    //public float maxPosX;
    //public float minPosX;
    //public float maxPosZ;
    //public float minPosZ;

    float height;

    public float minWaitTime;
    public float maxWaitTime;

    public float minTorque;
    public float maxTorque;

    public float destroyTime;

    public Transform start;
    public Transform end;

    public List<GameObject> rockSelection;



    // Use this for initialization
    void Start()
    {
        height = transform.localPosition.y;
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
            Vector3 location = new Vector3(Random.Range(start.localPosition.x, end.localPosition.x), height, Random.Range(start.localPosition.z, end.localPosition.z));

            var randomRock = Random.Range(0, rockSelection.Count);

            float rockAmount = Random.Range(0, rockSelection.Count);

            for (int i = 0; i < rockAmount; i++)
            {

                //Vector3 offset = new Vector3(Random.Range(10, 50), 0, Random.Range(10, 50));

                var rock = Instantiate(rockSelection[randomRock], location, Quaternion.identity);
                rock.GetComponent<Rigidbody>().AddTorque(new Vector3(Random.Range(minTorque, maxTorque), Random.Range(minTorque, maxTorque), Random.Range(minTorque, maxTorque)));
                Destroy(rock, destroyTime);
            }

            float waitTime = Random.Range(minWaitTime, maxWaitTime);

            yield return new WaitForSeconds(waitTime);
        }
    }

    private void OnDrawGizmos() {
        Gizmos.color = new Color(0, 1, 0.5f, 0.9f);
        Gizmos.DrawLine(start.position, new Vector3(end.position.x, start.position.y, start.position.z));
        Gizmos.DrawLine(start.position, new Vector3(start.position.x, start.position.y, end.position.z));

        Gizmos.DrawLine(end.position, new Vector3(start.position.x, end.position.y, end.position.z));
        Gizmos.DrawLine(end.position, new Vector3(end.position.x, end.position.y, start.position.z));
    }
}
