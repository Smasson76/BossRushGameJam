using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour {

    public static GameManager instance; //Creating an object singleton
    
    [Header("Bomb Variables")]
    public GameObject[] playerBombs;
    public int bombsRemaining;

    [Header("Public GameObjects")]
    public Slider boostSlider;
    public GameObject playerPrefab;
    public PlayerInput playerInput;

    GameObject spawnCam;

    public bool canSpawnPlayer;
    
    public enum SceneType {
        mainMenu,
        withPlayer,
    }

    public delegate void PlayerSpawn();
    public event PlayerSpawn OnPlayerSpawn;
    private GameObject currentPlayer;
    private PlayerController playerController;

    void Awake() {
        if (instance == null) {
            DontDestroyOnLoad(gameObject);
            instance = this;
        }
        else {
            Destroy(gameObject);
        }

        spawnCam = this.transform.GetChild(0).gameObject;
        spawnCam.SetActive(false);
    }

    void Start() {
        if (GetCurrentScene() == SceneType.withPlayer) {
            spawnCam.SetActive(true);
            canSpawnPlayer = true;
        }
    }

    void Update() {
        if (boostSlider == null) return;
        //boostSlider.value = playerController.GetBoostRemaining();
    }

    public void SpawnPlayer() {
        spawnCam.SetActive(true);
        RaycastHit hit;
        Vector2 mousePos = playerInput.GetPointerPos();
        Ray ray = spawnCam.GetComponent<Camera>().ScreenPointToRay(mousePos);
        if (Physics.Raycast(ray, out hit)) {
            if (hit.transform.gameObject.tag == "Ground") {
                if (canSpawnPlayer) {
                    canSpawnPlayer = false;
                    spawnCam.SetActive(false);
                    currentPlayer = Instantiate(playerPrefab, hit.point + Vector3.up * 2, Quaternion.identity);
                    playerController = currentPlayer.GetComponent<PlayerController>();
                    playerController.playerInput = playerInput;
                    playerInput.NewPlayer(playerController);
                    if (OnPlayerSpawn != null) OnPlayerSpawn();
                }
            }
        }
    }

    public SceneType GetCurrentScene() {
        switch (SceneManager.GetActiveScene().name) {
        case "MainMenu":
            return SceneType.mainMenu;
        case "PlayerTestScene":
            return SceneType.withPlayer;
        case "GameScene":
            return SceneType.withPlayer;
        default:
            return SceneType.withPlayer;
        }
    }
    public Transform GetPlayer() {
        return currentPlayer.transform.Find("Sphere");
    }
}