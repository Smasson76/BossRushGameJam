using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MainMenuManager : MonoBehaviour {

    public static MainMenuManager instance;

    public PlayerInput playerInput;

    public GameObject[] levelDifficultyObjects;
    public int difficulty = 1;

    public GameObject soundValve;
    Animator soundAnim;
    public GameObject musicValve;
    Animator musicAnim;
    
    void Awake() {
        if (instance == null) instance = this;

        playerInput = this.GetComponent<PlayerInput>();
        soundAnim = soundValve.GetComponent<Animator>();
        musicAnim = musicValve.GetComponent<Animator>();
        soundAnim.SetBool("Trigger", true);
        musicAnim.SetBool("Trigger", true);
    }

    public void ChargeStation() {
        RaycastHit hit;
        Vector2 mousePos = playerInput.GetPointerPos();
        Ray ray = Camera.main.ScreenPointToRay(mousePos);
        if (Physics.Raycast(ray, out hit)) {
            if (hit.transform.gameObject.tag == "ChargeStation") {
                SwitchDifficulty(difficulty);
            }
        }
    }

    void SwitchDifficulty(int difficultyChange) {
        if (difficultyChange == 1) {
            levelDifficultyObjects[0].SetActive(true);
            levelDifficultyObjects[1].SetActive(false);
            levelDifficultyObjects[2].SetActive(false);
            difficulty = 2;
        }
        else if (difficultyChange == 2) {
            levelDifficultyObjects[0].SetActive(false);
            levelDifficultyObjects[1].SetActive(true);
            levelDifficultyObjects[2].SetActive(false);
            difficulty = 3;
        }
        else if (difficultyChange == 3) {
            levelDifficultyObjects[0].SetActive(false);
            levelDifficultyObjects[1].SetActive(false);
            levelDifficultyObjects[2].SetActive(true);
            difficulty = 1;
        }
    }

    public void SoundChanger() {
        RaycastHit hit;
        Vector2 mousePos = playerInput.GetPointerPos();
        Ray ray = Camera.main.ScreenPointToRay(mousePos);
        if (Physics.Raycast(ray, out hit)) {
            Debug.Log(hit.transform);
            if (hit.transform.gameObject.tag == "SoundValve") {
                if (soundAnim.GetBool("Trigger") == true) {
                    soundAnim.SetBool("Trigger", false);
                }
                else if (soundAnim.GetBool("Trigger") == false) {
                    soundAnim.SetBool("Trigger", true);
                }
            }
        }
    }

    public void MusicChanger() {
        RaycastHit hit;
        Vector2 mousePos = playerInput.GetPointerPos();
        Ray ray = Camera.main.ScreenPointToRay(mousePos);
        if (Physics.Raycast(ray, out hit)) {
            if (hit.transform.gameObject.tag == "MusicValve") {
                if (musicAnim.GetBool("Trigger") == true) {
                    musicAnim.SetBool("Trigger", false);
                }
                else if (musicAnim.GetBool("Trigger") == false) {
                    musicAnim.SetBool("Trigger", true);
                }
            }
        }
    }
}
