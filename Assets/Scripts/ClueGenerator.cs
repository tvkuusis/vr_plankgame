using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClueGenerator : MonoBehaviour {

    public GameObject[] clues;
    GameController gc;


    // Use this for initialization
    void Start() {

    }

    public void AssignClues(string clue1, string clue2, string clue3){
        //gc = GameObject.Find("GameController").GetComponent<GameController>();
        ShuffleArray(clues);
        //clues[0].GetComponent<TextMesh>().text = gc.correctSpinnerCharacters[0];
        clues[0].GetComponent<TextMesh>().text = clue1;
        clues[0].GetComponent<MeshRenderer>().material.color = Color.red;
        //clues[1].GetComponent<TextMesh>().text = gc.correctSpinnerCharacters[1];
        clues[1].GetComponent<TextMesh>().text = clue2;
        clues[1].GetComponent<MeshRenderer>().material.color = Color.green;
        //clues[2].GetComponent<TextMesh>().text = gc.correctSpinnerCharacters[2];
        clues[2].GetComponent<TextMesh>().text = clue3;
        clues[2].GetComponent<MeshRenderer>().material.color = Color.blue;
    }

    public static void ShuffleArray<T>(T[] arr)
    {
        for (int i = arr.Length - 1; i > 0; i--) {
            int r = Random.Range(0, i);
            T tmp = arr[i];
            arr[i] = arr[r];
            arr[r] = tmp;
        }
    }
}
