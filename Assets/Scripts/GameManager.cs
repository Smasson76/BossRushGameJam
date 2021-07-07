using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

    public static GameManager instance; //Creating an object singleton
    
    [Header("Bomb Variables")]
    public GameObject[] playerBombs;
    public int bombsRemaining;

    [Header("Public GameObjects")]
    public Slider boostSlider;
    public GameObject playerPrefab;
    public PlayerInput playerInput;
    
    public enum SceneType {
        mainMenu,
        playerTestScene,
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
    }

    void Start() {
        if (GetCurrentScene() == SceneType.playerTestScene) {
            SpawnPlayer();
        }
    }

    void Update() {
        if (boostSlider == null) return;
        boostSlider.value = playerController.GetBoostRemaining();
    }

    public void SpawnPlayer() {
        currentPlayer = Instantiate(playerPrefab, transform.position, transform.rotation);
        playerController = currentPlayer.GetComponent<PlayerController>();
        playerController.playerInput = playerInput;
        playerInput.NewPlayer(playerController);
        if (OnPlayerSpawn != null) OnPlayerSpawn();
    }

    public SceneType GetCurrentScene() {
        switch (SceneManager.GetActiveScene().name) {
        case "MainMenu":
            return SceneType.mainMenu;
        case "PlayerTestScene":
            return SceneType.playerTestScene;
        default:
            throw new System.Exception("Unhandled Scene Name");
        }
    }
    public Transform GetPlayer() {
        return currentPlayer.transform.Find("Sphere");
    }
}
