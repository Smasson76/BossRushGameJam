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
    public GameObject quitBox;
    Animator quitAnim;
    public GameObject camera;
    Animator cameraAnim;
    
    void Awake() {
        if (instance == null) instance = this;

        playerInput = GameObject.Find("GameManager").GetComponent<PlayerInput>();
        soundAnim = soundValve.GetComponent<Animator>();
        musicAnim = musicValve.GetComponent<Animator>();
        cameraAnim = camera.GetComponent<Animator>();
        quitAnim = quitBox.GetComponent<Animator>();
        soundAnim.SetBool("Trigger", true);
        musicAnim.SetBool("Trigger", true);
        AudioManager.instance.MusicEvents("MainMenu");
    }

    public void Click() {
        RaycastHit hit;
        Vector2 mousePos = playerInput.GetPointerPos();
        Ray ray = Camera.main.ScreenPointToRay(mousePos);
        if (Physics.Raycast(ray, out hit)) {
            switch(hit.transform.gameObject.tag) {
                case "ChargeStation":
                    SwitchDifficulty(difficulty);
                    break;
                case "SoundValve":
                    if (soundAnim.GetBool("Trigger") == true) {
                        AudioManager.instance.canPlaySounds = false;
                        soundAnim.SetBool("Trigger", false);
                    }
                    else if (soundAnim.GetBool("Trigger") == false) {
                        AudioManager.instance.canPlaySounds = true;
                        soundAnim.SetBool("Trigger", true);
                    }
                    AudioManager.instance.MenuButtonEvents("Valve");
                    break;
                case "MusicValve":
                    if (musicAnim.GetBool("Trigger") == true) {
                        musicAnim.SetBool("Trigger", false);
                        AudioManager.instance.canPlayMusic = false;
                    }
                    else if (musicAnim.GetBool("Trigger") == false) {
                        musicAnim.SetBool("Trigger", true);
                        AudioManager.instance.canPlayMusic = true;
                    }
                    AudioManager.instance.MusicEvents("MainMenu");
                    AudioManager.instance.MenuButtonEvents("Valve");
                    break;
                case "StartBall":
                    AudioManager.instance.MenuButtonEvents("PlayGame");
                    StartCoroutine(PlayGame());
                    break;
                case "ExitBox":
                    quitAnim.SetTrigger("TriggerExitBox");
                    Application.Quit();
                    AudioManager.instance.MenuButtonEvents("ExitGame");
                    break;
                default:
                    Debug.Log("Clicked an object with no tag");
                    break;
            }
        }
    }

    IEnumerator PlayGame() {
        cameraAnim.SetTrigger("TriggerExit");
        yield return new WaitForSeconds(3f);
        GameManager.instance.LoadScene("PlayerTestScene");
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
}
