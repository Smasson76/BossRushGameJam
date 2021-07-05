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

    [Header("Boost Variables")]
    public Slider boostSlider;
    public float boostAmount = 100f;
    public float boostDescreaseAmount = 0.5f;
    public float boostIncreaseAmount = 0.1f;
    public bool isBoosting = false;
    public bool canRegenerateBoost = false;
    public bool isRegenerating = false;

    [Header("Public GameObjects")]
    public GameObject player;
    public Scene currentScene;

    public PlayerInput playerInput;

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
        boostSlider.value = boostAmount;

        if (isBoosting) canRegenerateBoost = false;
        else canRegenerateBoost = true;

        if (boostAmount < 100 && canRegenerateBoost && !isRegenerating) {
            isRegenerating = true;
            StartCoroutine(RegainBoost());
        }
    }

    IEnumerator RegainBoost() {
        yield return new WaitForSeconds(3f);
        while (boostAmount < 100 && !isBoosting) {
            yield return new WaitForSeconds(0.1f);
            boostAmount += boostIncreaseAmount;
        }
        isRegenerating = false;
    }

    public void SpawnPlayer() {
        Instantiate(player, transform.position, transform.rotation);
        Debug.Log("SpawnPlayer");
    }
}
