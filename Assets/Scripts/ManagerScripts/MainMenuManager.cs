using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour {

    public static MainMenuManager instance;

    public PlayerInput playerInput;

    public GameObject[] levelDifficultyObjects;
    public int difficulty = 1;

    public GameObject soundValve;
    Animator soundAnim;
    public GameObject musicValve;
    Animator musicAnim;
    public GameObject quitBox;
    Animator quitAnim;
    
    void Awake() {
        if (instance == null) instance = this;

        playerInput = GameObject.Find("GameManager").GetComponent<PlayerInput>();
        soundAnim = soundValve.GetComponent<Animator>();
        musicAnim = musicValve.GetComponent<Animator>();
        quitAnim = quitBox.GetComponent<Animator>();
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
            AudioManager.instance.MenuButtonEvents("EasyMode");
            levelDifficultyObjects[0].SetActive(true);
            levelDifficultyObjects[1].SetActive(false);
            levelDifficultyObjects[2].SetActive(false);
            difficulty = 2;
        }
        else if (difficultyChange == 2) {
            AudioManager.instance.MenuButtonEvents("MediumMode");
            levelDifficultyObjects[0].SetActive(false);
            levelDifficultyObjects[1].SetActive(true);
            levelDifficultyObjects[2].SetActive(false);
            difficulty = 3;
        }
        else if (difficultyChange == 3) {
            AudioManager.instance.MenuButtonEvents("HardMode");
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
            if (hit.transform.gameObject.tag == "SoundValve") {
                if (soundAnim.GetBool("Trigger") == true) {
                    //Sounds should be off here
                    soundAnim.SetBool("Trigger", false);
                }
                else if (soundAnim.GetBool("Trigger") == false) {
                    //Sounds should be on here
                    soundAnim.SetBool("Trigger", true);
                }
                AudioManager.instance.MenuButtonEvents("Valve");
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
                    //Playing an audiosound here
                    //Setting music audio off here
                }
                else if (musicAnim.GetBool("Trigger") == false) {
                    musicAnim.SetBool("Trigger", true);
                    //Playing an audiosound here
                    //Setting music audio on here
                }
                AudioManager.instance.MenuButtonEvents("Valve");
            }
        }
    }

    public void PlayGame() {
        RaycastHit hit;
        Vector2 mousePos = playerInput.GetPointerPos();
        Ray ray = Camera.main.ScreenPointToRay(mousePos);
        if (Physics.Raycast(ray, out hit)) {
            if (hit.transform.gameObject.tag == "StartBall") {
                SceneManager.LoadScene(1);
                AudioManager.instance.MenuButtonEvents("PlayGame");
            }
        }
    }

    public void ExitGame() {
        RaycastHit hit;
        Vector2 mousePos = playerInput.GetPointerPos();
        Ray ray = Camera.main.ScreenPointToRay(mousePos);
        if (Physics.Raycast(ray, out hit)) {
            if (hit.transform.gameObject.tag == "ExitBox") {
                quitAnim.SetTrigger("TriggerExitBox");
                Application.Quit();
                AudioManager.instance.MenuButtonEvents("ExitGame");
            }
        }
    }
}
