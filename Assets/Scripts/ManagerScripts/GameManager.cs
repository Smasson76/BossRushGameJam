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

    public GameObject spawnCam;

    public bool canSpawnPlayer;
    
    public enum SceneType {
        mainMenu,
        withPlayer,
    }

    public delegate void SceneChange(SceneType sceneType);
    public event SceneChange OnSceneChange;

    public delegate void PlayerSpawn(PlayerController newPlayer);
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
        SceneSetup();
    }
    
    public void LoadScene(int sceneIndex) {
        SceneManager.LoadScene(sceneIndex);
        StartCoroutine(SceneSetup());
    }

    private IEnumerator SceneSetup() {
        yield return new WaitForSeconds(1);
        SceneType sceneType = GetCurrentSceneType();
        switch(sceneType) {
            case SceneType.mainMenu:
                break;
            case SceneType.withPlayer:
                spawnCam.SetActive(true);
                canSpawnPlayer = true;
                break;
        }
        if (OnSceneChange != null) {
            OnSceneChange(sceneType);
        }
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
                    if (OnPlayerSpawn != null) OnPlayerSpawn(playerController);
                }
            }
        }
    }

    private SceneType GetCurrentSceneType() {
        switch (SceneManager.GetActiveScene().name) {
        case "MainMenu":
            return SceneType.mainMenu;
        default:
            return SceneType.withPlayer;
        }
    }

    public Transform GetPlayer() {
        return currentPlayer.transform.Find("Sphere");
    }
}
