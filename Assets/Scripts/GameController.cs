﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour {

	public bool playerInSpawnRoom = true;
	public GameObject spawnroom;

	bool falling;
	public GameObject player;
    public GameObject elevator;
	public GameObject[] gameScenePlanks;
	public GameObject gameRoom;
    ElevatorScript es;
	public float fallAcceleration = 9.81f;
	float velocity = 0;
	float yDist = 0;
	Vector3 newPosition;

    public GameObject leftFoot;
    public GameObject rightFoot;
    //public GameObject leftFootModel;
    //public GameObject rightFootModel;
    public GameObject leftFootCalibModel;
    public GameObject rightFootCalibModel;
	public GameObject leftFootCalibOffset;
	public GameObject rightFootCalibOffset;
    public GameObject playerBlinder;
	public GameObject fallSwitch;
	public GameObject fallColliders;
	bool fallEnabled;
    Renderer blinder;
    bool fadeout;
    bool fadein;
    public float fadeSpeed = 0.5f;
    float blinderAlpha = 0;
    Color blinderOrigColor;

	Transform leftFootPosOnStart;
	Transform rightFootPosOnStart;

    public Transform torchSpawn;
    public GameObject torchPrefab;

    [HideInInspector]
	public string[] correctSpinnerCharacters;
    public bool[] spinnerStates;
	bool elevatorActivated;

	float t = 1f;
	bool feetLoaded; // true after the game has positioned the feet models to saved positions

	[HideInInspector]
	public bool leftFootInPosition;
	[HideInInspector]
	public bool rightFootInposition;

	AudioSource[] audios;
	AudioSource ambientSound;
	AudioSource windSound;
	AudioSource thumpSound;
	AudioSource pressSound;
	AudioSource spinnerSound;


	// Use this for initialization
	void Start () {
        correctSpinnerCharacters = new string[3];
        spinnerStates = new bool[3];
        for(int i = 0; i < correctSpinnerCharacters.Length; i++) {
            correctSpinnerCharacters[i] = ReturnRandomAlphabet();
            print("Spinner " + i + " correct: " + correctSpinnerCharacters[i]);
        }
        //GameObject.Find("Clue Generator").GetComponent<ClueGenerator>().AssignClues(correctSpinnerCharacters[0], correctSpinnerCharacters[1], correctSpinnerCharacters[2]);

        if (!playerInSpawnRoom) {
			InstantiateTorch ();
		}
        es = elevator.GetComponent<ElevatorScript>();
        blinder = playerBlinder.GetComponent<Renderer>();
        blinderOrigColor = blinder.material.color;
		if (playerInSpawnRoom) {
			newPosition = new Vector3 (spawnroom.transform.position.x, spawnroom.transform.position.y, spawnroom.transform.position.z);
			player.transform.position = newPosition;
		}

		audios = GetComponents<AudioSource> ();
		ambientSound = audios [0];
		windSound = audios [1];
		thumpSound = audios [2];
		pressSound = audios [3];
		spinnerSound = audios [4];
	}
	
	// Update is called once per frame
	void Update () {

		if (t > 0) {
			t -= Time.deltaTime;
		} else if (t <= 0 && !feetLoaded) {
			
			//LoadFeetPositions();
			feetLoaded = true;
		}

		if (playerInSpawnRoom) {
			newPosition = new Vector3 (spawnroom.transform.position.x, spawnroom.transform.position.y, spawnroom.transform.position.z);
			player.transform.position = newPosition;
            if (Input.GetKeyDown(KeyCode.M)) {
                CalibrateFeetPositions();
            }else if (Input.GetKeyDown(KeyCode.L)) {
                LoadFeetPositions();
            }
			if (fallSwitch.GetComponent<NVRSwitchModified> ().CurrentState == true) {
				fallEnabled = true;
			} else {
				fallEnabled = false;
			}
		} else {
			if (falling) {
				velocity -= fallAcceleration * Time.deltaTime;
				yDist += velocity * Time.deltaTime;
				newPosition = new Vector3 (player.transform.position.x, yDist, player.transform.position.z);
				player.transform.position = newPosition;
			}
			CheckSpinnerStates ();
		}

		if (Input.GetKeyDown (KeyCode.R)) {
			ResetScene ();
		}

		if (Input.GetKeyDown (KeyCode.F)) {
			PlayerFall ();
		}
        if (Input.GetKeyDown(KeyCode.Q)) {
			QuitGame ();    
		}

        if (fadeout) {
            blinderAlpha += Time.deltaTime * fadeSpeed;
            blinder.material.color = new Color(blinderOrigColor.r, blinderOrigColor.g, blinderOrigColor.b, blinderAlpha);
            if (blinderAlpha > 1) {
                StartGame();
                fadeout = false;
                fadein = true;
                blinderAlpha = 1.5f;
            }
        }

        if (fadein) {
            blinderAlpha -= Time.deltaTime;
            blinder.material.color = new Color(blinderOrigColor.r, blinderOrigColor.g, blinderOrigColor.b, blinderAlpha);
            if(blinderAlpha < 0) {
                fadein = false;
                blinderAlpha = 0;
            }
        }

	}

	public void ResetScene(){
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
	}

	public void QuitGame(){
		Application.Quit ();
	}

	public void PlayerFall(){
		falling = true;
	}

    public void FallCheck(){
        bool left = leftFoot.GetComponentInChildren<FootScript>().grounded;
        bool right = rightFoot.GetComponentInChildren<FootScript>().grounded;

		if(!left && !right && !falling) {
            PlayerFall();
			windSound.Play ();
        }
    }

    public void InstantiateTorch()
    {
        var torch = Instantiate(torchPrefab, torchSpawn);
		torch.transform.SetParent (GameObject.Find("Tower").transform);
    }

    public void SetSpinnerLetter(string letter, int spinner){
        if(letter == correctSpinnerCharacters[spinner]) {
            spinnerStates[spinner] = true;
        }
        else {
            spinnerStates[spinner] = false;
        }


    }

    void CheckSpinnerStates(){
		if(spinnerStates[0] && spinnerStates[1] && spinnerStates[2] && !elevatorActivated) {
            print("Correct letters");
            es.ActivateElevator();
			elevatorActivated = true;
            // + Do something to the sliders
        }
    }

    public void MovePlayerWithElevator(){
		fallColliders.SetActive (false);
        player.transform.SetParent(elevator.transform);
    }

    public void CalibrateFeetPositions(){
		pressSound.Play ();
		leftFoot.SetActive (true);
		rightFoot.SetActive (true);
		leftFootCalibOffset.transform.position = leftFootCalibModel.transform.position;
		leftFoot.transform.position = new Vector3(leftFoot.transform.position.x, 0, leftFoot.transform.position.z);
		leftFootCalibOffset.transform.rotation = leftFootCalibModel.transform.rotation;
        rightFootCalibOffset.transform.position = rightFootCalibModel.transform.position;
		rightFoot.transform.position = new Vector3(rightFoot.transform.position.x, 0, rightFoot.transform.position.z);
		rightFootCalibOffset.transform.rotation = rightFootCalibModel.transform.rotation;

        print("Feet calibrated");
        SaveFeetPositions();
        LoadFeetPositions();
    }

    void SaveFeetPositions(){
		PlayerPrefs.SetFloat("LeftFootXPos", leftFootCalibOffset.transform.position.x);
		PlayerPrefs.SetFloat("LeftFootYPos", leftFootCalibOffset.transform.position.y);
		PlayerPrefs.SetFloat("LeftFootZPos", leftFootCalibOffset.transform.position.z);

		PlayerPrefs.SetFloat("RightFootXPos", rightFootCalibOffset.transform.position.x);
		PlayerPrefs.SetFloat("RightFootYPos", rightFootCalibOffset.transform.position.y);
		PlayerPrefs.SetFloat("RightFootZPos", rightFootCalibOffset.transform.position.z);

		PlayerPrefs.SetFloat("LeftFootXRot", leftFootCalibOffset.transform.localRotation.x);
		PlayerPrefs.SetFloat("LeftFootYRot", leftFootCalibOffset.transform.localRotation.y);
		PlayerPrefs.SetFloat("LeftFootZRot", leftFootCalibOffset.transform.localRotation.z);

		PlayerPrefs.SetFloat("RightFootXRot", rightFootCalibOffset.transform.localRotation.x);
		PlayerPrefs.SetFloat("RightFootYRot", rightFootCalibOffset.transform.localRotation.y);
		PlayerPrefs.SetFloat("RightFootZRot", rightFootCalibOffset.transform.localRotation.z);

        print("Feet positions saved!");
    }

	void LoadFeetPositions(){
        // Temp variables
        float x = 0; float y = 0; float z = 0;
        float j = 0; float k = 0; float l = 0;

        if (PlayerPrefs.HasKey("LeftFootXPos") && PlayerPrefs.HasKey("LeftFootYPos") && PlayerPrefs.HasKey("LeftFootZPos") && PlayerPrefs.HasKey("LeftFootXRot") && PlayerPrefs.HasKey("LeftFootYRot") && PlayerPrefs.HasKey("LeftFootZRot")) {
            x = PlayerPrefs.GetFloat("LeftFootXPos");
            y = PlayerPrefs.GetFloat("LeftFootYPos");
            z = PlayerPrefs.GetFloat("LeftFootZPos");
            j = PlayerPrefs.GetFloat("LeftFootXRot");
            k = PlayerPrefs.GetFloat("LeftFootYRot");
            l = PlayerPrefs.GetFloat("LeftFootZRot");

			leftFootCalibOffset.transform.position = new Vector3(x, y, z);
			leftFootCalibOffset.transform.rotation = Quaternion.Euler(j, k, l);
			leftFoot.transform.position = new Vector3(leftFoot.transform.position.x, leftFootCalibModel.transform.position.y, leftFoot.transform.position.z);
			leftFootCalibOffset.transform.rotation = Quaternion.Euler(leftFootCalibOffset.transform.rotation.x, k, leftFootCalibOffset.transform.rotation.z);
			leftFootCalibOffset.transform.forward = leftFootCalibModel.transform.forward;
            print("Left foot position loaded from memory.");
        }
        else {
            print("No saved position found for left foot.");
        }
        x = 0; y = 0; z = 0;
        j = 0; k = 0; l = 0;

        if (PlayerPrefs.HasKey("RightFootXPos") && PlayerPrefs.HasKey("RightFootYPos") && PlayerPrefs.HasKey("RightFootZPos") && PlayerPrefs.HasKey("RightFootXRot") && PlayerPrefs.HasKey("RightFootYRot") && PlayerPrefs.HasKey("RightFootZRot")) {
            x = PlayerPrefs.GetFloat("RightFootXPos");
            y = PlayerPrefs.GetFloat("RightFootYPos");
            z = PlayerPrefs.GetFloat("RightFootZPos");
            j = PlayerPrefs.GetFloat("RightFootXRot");
            k = PlayerPrefs.GetFloat("RightFootYRot");
            l = PlayerPrefs.GetFloat("RightFootZRot");

			rightFootCalibOffset.transform.position = new Vector3(x, y, z);
			rightFootCalibOffset.transform.rotation = Quaternion.Euler(j, k, l);
			rightFoot.transform.position = new Vector3(rightFoot.transform.position.x, rightFootCalibModel.transform.position.y, rightFoot.transform.position.z);
			rightFootCalibOffset.transform.rotation = Quaternion.Euler(rightFootCalibOffset.transform.rotation.x, k, rightFootCalibOffset.transform.rotation.z);
			rightFootCalibOffset.transform.forward = rightFootCalibModel.transform.forward;
            print("Right foot position loaded from memory.");
        }
        else {
            print("No saved position found for right foot.");
        }
    }

	public void StartGame(){ // take room number as a variable if there's more than one room
		if (playerInSpawnRoom) {
			if (leftFootInPosition && rightFootInposition) {
				pressSound.Play ();
				print ("Game started");
				spawnroom.SetActive (false);
				playerInSpawnRoom = false;
				gameRoom.SetActive (true);
				ambientSound.Play ();
				if (fallEnabled) {
					fallColliders.SetActive (true);
				} else {
					fallColliders.SetActive (false);
				}
                GameObject.Find("Clue Generator").GetComponent<ClueGenerator>().AssignClues(correctSpinnerCharacters[0], correctSpinnerCharacters[1], correctSpinnerCharacters[2]);
				InstantiateTorch ();
				foreach (GameObject plank in gameScenePlanks) {
					plank.GetComponent<PositionCalibration> ().LoadPosition ();
				}
			} else {
				print ("Feet not in correct position");
			}
		} else if (!playerInSpawnRoom) {
			SceneManager.LoadScene (SceneManager.GetActiveScene ().buildIndex);
			//print ("Back to spawn room");
			//spawnroom.SetActive (true);
			//playerInSpawnRoom = true;
			//gameRoom.SetActive (false);
		}
	}

    public void StartFading(){
        fadeout = true;
    }

    string ReturnRandomAlphabet(){
        //string alphabets = "ABCDEFghijklmnopqrstuVwxYz*"; egytian
		string alphabets = "ABCDEFGHIJKLMNOPQRSTUVWXYZ?";
        char c = alphabets[Random.Range(0, alphabets.Length)];
        return c.ToString();
    }

	public void PlayerHitsBottom(){
		windSound.Stop ();
		thumpSound.Play ();
		StartFading ();
	}
}
