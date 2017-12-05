using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameController : MonoBehaviour {

	public bool playerInSpawnRoom = true;
	public GameObject spawnroom;

	bool falling;
	public GameObject player;
    //public GameObject elevator;
	//public GameObject[] gameScenePlanks;
	public GameObject tower;
    //ElevatorScript es;
	public float fallAcceleration = 9.81f;
	float velocity = 0;
	float yDist = 0;
	Vector3 newPosition;

    public GameObject leftFootModel;
    public GameObject rightFootModel;
    public GameObject leftFootParent;
    public GameObject rightFootParent;
    //public GameObject leftFootModel;
    //public GameObject rightFootModel;
	public GameObject leftFootCalibOffset;
	public GameObject rightFootCalibOffset;
    public GameObject leftCalibPoint;
    public GameObject rightCalibPoint;
    public GameObject playerBlinder;
	public GameObject fallSwitch;
	//public GameObject fallColliders;

	public bool fallEnabled = true;
    Renderer blinder;
    bool fadeout;
    bool fadein;
    public float fadeSpeed = 0.5f;
    float blinderAlpha = 0;
    Color blinderOrigColor;

	Transform leftFootPosOnStart;
	Transform rightFootPosOnStart;

    //public Transform torchSpawn;
    //public GameObject torchPrefab;

 //   [HideInInspector]
	//public string[] correctSpinnerSymbols;
 //   public bool[] spinnerStates;
	//bool elevatorActivated;

	float t = 1f;
	bool feetLoaded; // true after the game has positioned the feet models to saved positions

	[HideInInspector]
	public bool leftFootInPosition;
	[HideInInspector]
	public bool rightFootInposition;
    bool feetSwitched;

    // Floor calibration
    public Transform calibrationPoint1;
    public Transform calibrationPoint2;

    public Text fallText;

	AudioSource[] audios;
    //AudioSource ambientSound;
    AudioSource windSound;
    AudioSource thumpSound;
    AudioSource pressSound;
    //AudioSource spinnerSound;

    private static GameController gameControllerInstance;

    //void Awake(){
    //    DontDestroyOnLoad(this);

    //    if(gameControllerInstance == null) {
    //        gameControllerInstance = this;
    //    }
    //    else {
    //        DestroyObject(gameObject);
    //    }
    //}

    private void Awake()
    {
        spawnroom.SetActive(true);
        tower.SetActive(false);
    }

    void Start () {
        //SetSpinnerSymbols();
        //es = elevator.GetComponent<ElevatorScript>();
        blinder = playerBlinder.GetComponent<Renderer>();
        blinderOrigColor = blinder.material.color;
        ToggleFallAnimation();

		//if (playerInSpawnRoom) {
		//	newPosition = new Vector3 (spawnroom.transform.position.x, spawnroom.transform.position.y, spawnroom.transform.position.z);
		//	player.transform.position = newPosition;
		//}

		audios = GetComponents<AudioSource> ();
        pressSound = audios[0];
		//ambientSound = audios [0];
		windSound = audios [1];
		thumpSound = audios [2];
        //pressSound = audios [3];
        //spinnerSound = audios [4];
	}
	
	void Update () {

		if (t > 0) {
			t -= Time.deltaTime;
		} else if (t <= 0 && !feetLoaded) {
            if (leftFootParent.activeSelf && rightFootParent.activeSelf) {
                LoadFeetPositions();
            }
            AlignRoomWithPlayer();
            feetLoaded = true;
        }

		if (playerInSpawnRoom) {
			//newPosition = new Vector3 (spawnroom.transform.position.x, spawnroom.transform.position.y, spawnroom.transform.position.z);
			//player.transform.position = newPosition;
            //if (Input.GetKeyDown(KeyCode.M)) {
            //    CalibrateFeetPositions();
            //}else if (Input.GetKeyDown(KeyCode.L)) {
            //    LoadFeetPositions();
            //}
			//if (fallSwitch.GetComponent<NVRSwitchModified> ().CurrentState == true) {
			//	fallEnabled = true;
			//} else {
			//	fallEnabled = false;
			//}
		} else {
			if (falling) {
				velocity -= fallAcceleration * Time.deltaTime;
				yDist += velocity * Time.deltaTime;
				newPosition = new Vector3 (player.transform.position.x, yDist, player.transform.position.z);
				player.transform.position = newPosition;
			}
			//CheckSpinnerStates ();
		}

        // R - Reset scene  F - Fall    Q - quit    A 

		if (Input.GetKeyDown (KeyCode.R)) {
			ResetScene ();
		}

		if (Input.GetKeyDown (KeyCode.F)) {
			PlayerFall ();
		}
        if (Input.GetKeyDown(KeyCode.Q)) {
			QuitGame ();    
		}
        if (Input.GetKeyDown(KeyCode.N)) {
            if (playerInSpawnRoom) {
                StartFading();
            }
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
        bool left = leftFootModel.GetComponentInChildren<PlayerFallScript>().grounded;
        bool right = rightFootModel.GetComponentInChildren<PlayerFallScript>().grounded;

		if(!left && !right && !falling) {
            PlayerFall();
			windSound.Play ();
        }
    }

    //public void SetSpinnerLetter(string letter, int spinner){
    //    if(letter == correctSpinnerSymbols[spinner]) {
    //        spinnerStates[spinner] = true;
    //    }
    //    else {
    //        spinnerStates[spinner] = false;
    //    }
    //}

  //  void CheckSpinnerStates(){
		//if(spinnerStates[0] && spinnerStates[1] && spinnerStates[2] && !elevatorActivated) {
  //          print("Correct letters");
  //          es.ActivateElevator();
		//	elevatorActivated = true;
  //          // + Do something to the sliders
  //      }
  //  }

  //  public void MovePlayerWithElevator(){
		//fallColliders.SetActive (false);
  //      player.transform.SetParent(elevator.transform);
  //  }

    public void CalibrateFeetPositions(){
		pressSound.Play ();
        if (leftFootParent.activeSelf && rightFootParent.activeSelf) {
            leftFootModel.SetActive(true);
            rightFootModel.SetActive(true);

            /////////////////////////
            leftFootCalibOffset.transform.position = leftCalibPoint.transform.position;
            leftFootModel.transform.position = new Vector3(leftFootModel.transform.position.x, leftFootModel.transform.position.y, leftFootModel.transform.position.z);
            leftFootCalibOffset.transform.rotation = leftCalibPoint.transform.rotation;
            //leftFootCalibOffset.transform.forward = leftFootCalibModel.transform.forward;

            rightFootCalibOffset.transform.position = rightCalibPoint.transform.position;
            rightFootModel.transform.position = new Vector3(rightFootModel.transform.position.x, rightCalibPoint.transform.position.y, rightFootModel.transform.position.z);
            rightFootCalibOffset.transform.rotation = rightCalibPoint.transform.rotation;

            print("Feet calibrated");
            SaveFeetPositions();
            //LoadFeetPositions();
        }
        else {
            print("Feet are not found. Are both foot trackers turned on and paired correctly?");
        }
    }

    void SaveFeetPositions(){

        PlayerPrefs.SetFloat("LeftFootXPos", leftFootCalibOffset.transform.localPosition.x);
		PlayerPrefs.SetFloat("LeftFootYPos", leftFootCalibOffset.transform.localPosition.y);
		PlayerPrefs.SetFloat("LeftFootZPos", leftFootCalibOffset.transform.localPosition.z);

		PlayerPrefs.SetFloat("RightFootXPos", rightFootCalibOffset.transform.localPosition.x);
		PlayerPrefs.SetFloat("RightFootYPos", rightFootCalibOffset.transform.localPosition.y);
		PlayerPrefs.SetFloat("RightFootZPos", rightFootCalibOffset.transform.localPosition.z);

        ////////////////
		PlayerPrefs.SetFloat("LeftFootXRot", leftFootCalibOffset.transform.localEulerAngles.x);
		PlayerPrefs.SetFloat("LeftFootYRot", leftFootCalibOffset.transform.localEulerAngles.y);
		PlayerPrefs.SetFloat("LeftFootZRot", leftFootCalibOffset.transform.localEulerAngles.z);
        //print(leftFootCalibOffset.transform.localEulerAngles.x + " " + leftFootCalibOffset.transform.localEulerAngles.y + " " + leftFootCalibOffset.transform.localEulerAngles.z);
        ///////////////

		PlayerPrefs.SetFloat("RightFootXRot", rightFootCalibOffset.transform.localEulerAngles.x);
		PlayerPrefs.SetFloat("RightFootYRot", rightFootCalibOffset.transform.localEulerAngles.y);
		PlayerPrefs.SetFloat("RightFootZRot", rightFootCalibOffset.transform.localEulerAngles.z);

        print("Feet positions saved!");
    }

	void LoadFeetPositions(){
        var i = PlayerPrefs.GetInt("FeetSwitched");
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

            // leftFootCalibOffset.transform.localPosition = new Vector3(x, y, z);
            leftFootCalibOffset.transform.localPosition = new Vector3(x, y, z);


            //leftFootCalibOffset.transform.rotation = Quaternion.Euler(j, k, l);
            //leftFoot.transform.position = new Vector3(leftFoot.transform.position.x, leftFootCalibModel.transform.position.y, leftFoot.transform.position.z);
            leftFootCalibOffset.transform.localEulerAngles = new Vector3(j, k, l);
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

            //rightFootCalibOffset.transform.localPosition = new Vector3(x, y, z);
            rightFootCalibOffset.transform.localPosition = new Vector3(x, y, z);

            //leftFootCalibOffset.transform.rotation = Quaternion.Euler(j, k, l);
            //leftFoot.transform.position = new Vector3(leftFoot.transform.position.x, leftFootCalibModel.transform.position.y, leftFoot.transform.position.z);
            rightFootCalibOffset.transform.localEulerAngles = new Vector3(j, k, l);
            print("Right foot position loaded from memory.");
        }
        else {
            print("No saved position found for right foot.");
        }
         
        if (i == 1) {
            GameObject.Find("Left foot").GetComponent<FootMover>().SwitchFoot();
            GameObject.Find("Right foot").GetComponent<FootMover>().SwitchFoot();
        }
    }

    public void ResetRoomOffset(){
        PlayerPrefs.SetFloat("RoomOffset", 0);
        AlignRoomWithPlayer();
    }

    public void SwitchFeet()
    {
        if(leftFootParent.activeSelf && rightFootParent.activeSelf) {
            leftFootParent.GetComponent<FootMover>().SwitchFoot();
            rightFootParent.GetComponent<FootMover>().SwitchFoot();
            print("Feet switched");
        }
        else {
            print("Feet not switched - are both feet trackers on and paired correctly?");
        }
        //GameObject.Find("Left foot").GetComponent<FootMover>().SwitchFoot();
        //GameObject.Find("Right foot").GetComponent<FootMover>().SwitchFoot();
    }

    public void CalibrateRoomOffset(){
        // Calibrate floor level to match the position of the controller that's lower (one should be in hand and one on floor at this point)
        var a = calibrationPoint1.transform.position.y;
        var b = calibrationPoint2.transform.position.y;
        var y = a < b ? a : b;

        PlayerPrefs.SetFloat("RoomOffset", y);
        AlignRoomWithPlayer();
    }

    void AlignRoomWithPlayer(){
        if (PlayerPrefs.HasKey("RoomOffset")) {
            var y = PlayerPrefs.GetFloat("RoomOffset");
            spawnroom.transform.position = new Vector3(0, y, 0);
            tower.transform.position = new Vector3(0, y, 0);
        }

    }

	public void StartGame(){ // take room number as a variable if there's more than one room
        print("Player in spawn room: " + playerInSpawnRoom);
		if (playerInSpawnRoom) {
			//if (leftFootInPosition && rightFootInposition) {
				pressSound.Play ();
				print ("Game started");
				spawnroom.SetActive (false);
				playerInSpawnRoom = false;
				tower.SetActive (true);
		} else if (!playerInSpawnRoom) {
			SceneManager.LoadScene (SceneManager.GetActiveScene ().buildIndex);
		}
	}

    public void StartFading(){
        fadeout = true;
    }

	public void PlayerHitsBottom(){
		windSound.Stop ();
		thumpSound.Play ();
		StartFading ();
	}

    public void ToggleFallAnimation(){
        print("Fall enabled: " + fallEnabled);
        if (fallEnabled) {
            fallEnabled = false;
            fallText.text = "Fall animation: off";
        }
        else {
            fallEnabled = true;
            fallText.text = "Fall animation: on";
        }
        print("Fall enabled: " + fallEnabled);
    }
}
