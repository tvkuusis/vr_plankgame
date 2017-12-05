using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerController : MonoBehaviour {

    public GameObject player;
    public Transform torchSpawn;
    public GameObject torchPrefab;
    public GameObject elevator;
    public GameObject[] fallColliders;
    public GameObject clueGenerator;
    public GameObject[] planks;

    [HideInInspector]
    public string[] correctSpinnerSymbols;
    public bool[] spinnerStates;
    bool elevatorActivated;

    ElevatorScript es;
    GameController gc;

    AudioSource[] audios;
    AudioSource ambientSound;
    AudioSource spinnerSound;

    void Start() {
        es = elevator.GetComponent<ElevatorScript>();
        gc = GameObject.Find("GameController").GetComponent<GameController>();


            if (gc.fallEnabled) {
                ToggleFallColliders(true);
            }else {
                ToggleFallColliders(false);
            }

        foreach (GameObject plank in planks) {
            plank.GetComponent<PositionCalibration>().LoadPosition();
        }

        //InstantiateTorch();
        //RandomizeSpinnerSymbols();
        //GameObject.Find("Clue Generator").GetComponent<ClueGenerator>().AssignClues(correctSpinnerSymbols[0], correctSpinnerSymbols[1], correctSpinnerSymbols[2]);

        audios = GetComponents<AudioSource>();
        ambientSound = audios[0];
        spinnerSound = audios[1];
    }
	
	void Update () {
        //CheckSpinnerStates();
    }

    public void InstantiateTorch()
    {
        var torch = Instantiate(torchPrefab, torchSpawn);
        torch.transform.SetParent(GameObject.Find("Tower").transform);
    }

    string ReturnRandomAlphabet()
    {
        //string alphabets = "ABCDEFghijklmnopqrstuVwxYz*"; hieroglyphs
        string alphabets = "ABCDEFGHIJKLMNOPQRSTUVWXYZ?";
        char c = alphabets[Random.Range(0, alphabets.Length)];
        return c.ToString();
    }

    void RandomizeSpinnerSymbols()
    {
        correctSpinnerSymbols = new string[3];
        spinnerStates = new bool[3];
        for (int i = 0; i < correctSpinnerSymbols.Length; i++) {
            correctSpinnerSymbols[i] = ReturnRandomAlphabet();
            print("Spinner " + i + " correct: " + correctSpinnerSymbols[i]);
        }
    }

    void CheckSpinnerStates()
    {
        if (spinnerStates[0] && spinnerStates[1] && spinnerStates[2] && !elevatorActivated) {
            print("Correct letters");
            es.MoveToLevelEnd();
            elevatorActivated = true;
        }
    }

    public void ToggleFallColliders(bool enabled){
        for (int i = 0; i < fallColliders.Length; i++) {
            if (enabled && gc.fallEnabled) {
                fallColliders[i].SetActive(true);
            }
            else {
                fallColliders[i].SetActive(false);
            }
        }
    }

    public void MovePlayerWithElevator()
    {
        ToggleFallColliders(false);
        player.transform.SetParent(elevator.transform);
    }

    public void UpdateSpinnerStates(string letter, int spinner)
    {
        if (letter == correctSpinnerSymbols[spinner]) {
            spinnerStates[spinner] = true;
        }
        else {
            spinnerStates[spinner] = false;
        }
    }
}
