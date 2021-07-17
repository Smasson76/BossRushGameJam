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

    MainMenuSounds mainMenuSounds;
    
    void Awake() {
        if (instance == null) instance = this;

        playerInput = GameObject.Find("GameManager").GetComponent<PlayerInput>();
        soundAnim = soundValve.GetComponent<Animator>();
        musicAnim = musicValve.GetComponent<Animator>();
        cameraAnim = camera.GetComponent<Animator>();
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
                        mainMenuSounds.canPlaySounds = false;
                        soundAnim.SetBool("Trigger", false);
                    }
                    else if (soundAnim.GetBool("Trigger") == false) {
                        mainMenuSounds.canPlaySounds = true;
                        soundAnim.SetBool("Trigger", true);
                    }
                    mainMenuSounds.Valve();
                    break;
                case "MusicValve":
                    if (musicAnim.GetBool("Trigger") == true) {
                        musicAnim.SetBool("Trigger", false);
                        mainMenuSounds.canPlayMusic = false;
                    }
                    else if (musicAnim.GetBool("Trigger") == false) {
                        musicAnim.SetBool("Trigger", true);
                        mainMenuSounds.canPlayMusic = true;
                    }
                    mainMenuSounds.PlayAmbienceSounds();
                    mainMenuSounds.Valve();
                    break;
                case "StartBall":
                    mainMenuSounds.MenuClickPlay();
                    StartCoroutine(PlayGame());
                    break;
                case "ExitBox":
                    quitAnim.SetTrigger("TriggerExitBox");
                    Application.Quit();
                    mainMenuSounds.TVSwitch();
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
