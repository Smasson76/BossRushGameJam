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
    public Scene currentScene;
    public PlayerInput playerInput;


    private PlayerController playerController;

    void Awake() {
        if (instance == null) {
            DontDestroyOnLoad(gameObject);
            instance = this;
        }
        else {
            Destroy(gameObject);
        }
        
        currentScene = SceneManager.GetActiveScene();
        if (currentScene.name == "PlayerTestScene") {
            SpawnPlayer();
        }
    }

    void Update() {
        if (boostSlider == null) return;
        boostSlider.value = playerController.GetBoostRemaining();
    }


    public void SpawnPlayer() {
        playerController = Instantiate(playerPrefab, transform.position, transform.rotation).GetComponent<PlayerController>();
        playerController.playerInput = playerInput;
        playerInput.NewPlayer(playerController);
        Debug.Log("SpawnPlayer");
    }
}
