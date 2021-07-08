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
    
    public enum SceneType {
        mainMenu,
        playerTestScene,
        gameScene,
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
        if (GetCurrentScene() == SceneType.gameScene) {
            SpawnPlayer();
        }
    }

    void Update() {
        if (boostSlider == null) return;
        boostSlider.value = playerController.GetBoostRemaining();
    }

    public void SpawnPlayer() {
        spawnCam.SetActive(true);
        Debug.Log("SpawnPlayer Function Called");
        RaycastHit hit;
        Vector2 mousePos = playerInput.GetPointerPos();
        Ray ray = Camera.main.ScreenPointToRay(mousePos);
        if (Physics.Raycast(ray, out hit)) {
            Debug.Log(hit.transform);
            if (hit.transform.gameObject.tag == "Ground") {
                Debug.Log("Ground hit");
            }
        }


        /*currentPlayer = Instantiate(playerPrefab, transform.position, transform.rotation);
        playerController = currentPlayer.GetComponent<PlayerController>();
        playerController.playerInput = playerInput;
        playerInput.NewPlayer(playerController);
        if (OnPlayerSpawn != null) OnPlayerSpawn();*/
    }

    public SceneType GetCurrentScene() {
        switch (SceneManager.GetActiveScene().name) {
        case "MainMenu":
            return SceneType.mainMenu;
        case "PlayerTestScene":
            return SceneType.playerTestScene;
        case "GameScene":
            return SceneType.gameScene;
        default:
            throw new System.Exception("Unhandled Scene Name");
        }
    }
    public Transform GetPlayer() {
        return currentPlayer.transform.Find("Sphere");
    }
}
