using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileDropper : MonoBehaviour {

    GameObject[] tiles;
    public TileScript[] tileScripts;

    bool drop;
    int index;
    float t;
    public float startInterval = 0.3f;
    float interval;
    public float minInterval;

    float intervalDecreaseStep;

    public TileScript[] certainTiles;

    public float certainTimeWindow;
    float ct;
    bool certainDrop;
    float ctInterval;
    int ctIndex;
    bool certainTilesDropped;

    void Start () {
        StartCoroutine(InitTiles());
    }

    IEnumerator InitTiles() {
        yield return StartCoroutine(GetTiles());
        yield return new WaitForSeconds(0.5f);
        yield return StartCoroutine(OrderTiles());
        print("Tiles initialized for TileDropper");
    }

    IEnumerator GetTiles() {
        tiles = GameObject.FindGameObjectsWithTag("Tile");
        yield return new WaitForSeconds(0.1f);
    }

    IEnumerator OrderTiles() {
        // temporary lists
        List<TileScript> tileScriptList = new List<TileScript>() { };
        List<GameObject> tileList = new List<GameObject>() { };

        // fill the list we remove the closest GameObjects from
        foreach (GameObject go in tiles) {
            tileList.Add(go);
        }
        yield return new WaitForSeconds(0.1f);

        for (int i = 0; i < tiles.Length; i++) {
            // temporary closest GameObject
            GameObject tempGO = tiles[i];
            // shortest distance
            float dist = Mathf.Infinity;

            for (int j = 0; j < tileList.Count; j++) {
                if (Vector3.Distance(transform.position, tileList[j].transform.position) < dist) {
                    tempGO = tileList[j];
                    dist = Vector3.Distance(transform.position, tileList[j].transform.position);
                }
            }

            tileScriptList.Add(tempGO.GetComponent<TileScript>());
            tileList.Remove(tempGO);
        }

        yield return new WaitForSeconds(0.1f);

        if (tiles.Length != tileScriptList.Count) {
            print("something wrong");
        } else {
            tileScripts = tileScriptList.ToArray();
            tileScriptList.Clear();
            tileList.Clear();
        }        
    }

    public void DropTiles() {
        interval = startInterval;
        intervalDecreaseStep = interval;
        drop = true;
    }

    public void DropCertainTiles() {
        print("drop certain tiles");
        ctInterval = certainTimeWindow / certainTiles.Length;
        certainDrop = true;
    }

    void Update () {

        if (Input.GetKeyDown(KeyCode.D)) DropTiles();
        if (Input.GetKeyDown(KeyCode.C)) DropCertainTiles();

        if (drop) {
            t += Time.deltaTime;

            if (t >= interval) {
                tileScripts[index].Drop();
                index++;
                if (index >= tileScripts.Length) drop = false;
                t -= interval;
                if (interval > minInterval) {
                    interval -= intervalDecreaseStep;
                } else if (interval < minInterval) {
                    interval = minInterval;
                }
            }
        }

        if (certainDrop) {
            ct += Time.deltaTime;
            if (ct > ctInterval) {
                certainTiles[ctIndex].Drop();
                ctIndex++;
                if (ctIndex >= certainTiles.Length) certainDrop = false;
                ct -= ctInterval;
            }
        }
	}
}
