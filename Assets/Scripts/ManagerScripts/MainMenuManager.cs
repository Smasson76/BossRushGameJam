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

    MainMenuSounds mainMenuSounds;
    
    void Awake() {
        if (instance == null) instance = this;

        playerInput = GameObject.Find("GameManager").GetComponent<PlayerInput>();
        soundAnim = soundValve.GetComponent<Animator>();
        musicAnim = musicValve.GetComponent<Animator>();
        quitAnim = quitBox.GetComponent<Animator>();
        soundAnim.SetBool("Trigger", true);
        musicAnim.SetBool("Trigger", true);
        mainMenuSounds = this.gameObject.GetComponent<MainMenuSounds>();
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
                        //AudioManager.instance.canPlaySounds = false;
                        soundAnim.SetBool("Trigger", false);
                    }
                    else if (soundAnim.GetBool("Trigger") == false) {
                        //AudioManager.instance.canPlaySounds = true;
                        soundAnim.SetBool("Trigger", true);
                    }
                    //AudioManager.instance.MenuButtonEvents("Valve");
                    break;
                case "MusicValve":
                    if (musicAnim.GetBool("Trigger") == true) {
                        musicAnim.SetBool("Trigger", false);
                        //AudioManager.instance.canPlayMusic = false;
                    }
                    else if (musicAnim.GetBool("Trigger") == false) {
                        musicAnim.SetBool("Trigger", true);
                        //AudioManager.instance.canPlayMusic = true;
                    }
                    //AudioManager.instance.MusicEvents("MainMenu");
                    //AudioManager.instance.MenuButtonEvents("Valve");
                    break;
                case "StartBall":
                    GameManager.instance.LoadScene("PlayerTestScene");
                    //AudioManager.instance.MenuButtonEvents("PlayGame");
                    break;
                case "ExitBox":
                    quitAnim.SetTrigger("TriggerExitBox");
                    Application.Quit();
                    //AudioManager.instance.MenuButtonEvents("ExitGame");
                    break;
                default:
                    Debug.Log("Clicked an object with no tag");
                    break;
            }
        }
    }
    void SwitchDifficulty(int difficultyChange) {
        if (difficultyChange == 1) {
            mainMenuSounds.DifficultySelection(1);
            levelDifficultyObjects[0].SetActive(true);
            levelDifficultyObjects[1].SetActive(false);
            levelDifficultyObjects[2].SetActive(false);
            difficulty = 2;
        }
        else if (difficultyChange == 2) {
            mainMenuSounds.DifficultySelection(2);
            levelDifficultyObjects[0].SetActive(false);
            levelDifficultyObjects[1].SetActive(true);
            levelDifficultyObjects[2].SetActive(false);
            difficulty = 3;
        }
        else if (difficultyChange == 3) {
            mainMenuSounds.DifficultySelection(3);
            levelDifficultyObjects[0].SetActive(false);
            levelDifficultyObjects[1].SetActive(false);
            levelDifficultyObjects[2].SetActive(true);
            difficulty = 1;
        }
    }
}
