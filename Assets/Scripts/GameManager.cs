using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

    public static GameManager instance; //Creating an object singleton
    
    //Bomb variables
    public GameObject[] playerBombs;
    public int bombsRemaining;

    //Boost variables
    public Slider boostSlider;
    public float boostAmount = 100f;
    public float boostDescreaseAmount = 0.5f;
    public float boostIncreaseAmount = 0.1f;
    public bool isBoosting = false;
    public bool canRegenerateBoost = false;
    public bool isRegenerating = false;

    //Player variables
    public GameObject player;

    void Awake() {
        if (instance == null) {
            DontDestroyOnLoad(gameObject);
            instance = this;
        }
        else {
            Destroy(gameObject);
        }
        
        Scene currentScene = SceneManager.GetActiveScene();
        string sceneName = currentScene.name;
        if (sceneName == "PlayerTestScene") {
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

    void SpawnPlayer() {
        Instantiate(player, transform.position, transform.rotation);
    }

    IEnumerator RespawnPlayer() {
        yield return new WaitForSeconds(4f);
        
    }
}
