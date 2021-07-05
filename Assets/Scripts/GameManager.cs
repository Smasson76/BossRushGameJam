using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

    void Awake() {
        if (instance == null) {
            DontDestroyOnLoad(gameObject);
            instance = this;
        }
        else {
            Destroy(gameObject);
        }
        boostSlider.value = boostAmount;
    }

    void Update() {
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
        while (boostAmount < 100) {
            yield return new WaitForSeconds(0.1f);
            boostAmount += boostIncreaseAmount;
        }
        isRegenerating = false;
    }
}