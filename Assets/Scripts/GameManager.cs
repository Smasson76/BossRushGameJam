using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

    public static GameManager instance; //Creating an object singleton
    
    //Bomb variables
    public GameObject[] playerBombs;
    public int bombsRemaining;

    //Slider variables
    public Slider boostSlider;
    public float boostAmount = 100f;
    public float boostDescreaseAmount = 0.5f;
    public float boostIncreaseAmount = 0.1f;
    public bool isBoosting;
    public bool canRegenerateBoost;

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
        //Here we will update the lives remaining

        boostSlider.value = boostAmount;

        if (isBoosting) canRegenerateBoost = false;
        else canRegenerateBoost = true;

        if (boostAmount <= 50 && canRegenerateBoost) {
            StartCoroutine(RegainBoost());
        }
    }

    IEnumerator RegainBoost() {
        while (boostAmount < 100) {
            yield return new WaitForSeconds(2f);
            boostAmount += boostIncreaseAmount;
        }
    }
}
