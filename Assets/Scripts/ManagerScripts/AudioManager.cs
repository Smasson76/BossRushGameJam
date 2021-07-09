using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour {
    
    public static AudioManager instance; //Creating a singleton object

    //This is for all of the clicks, switched, and buttons in the Main Menu
    [FMODUnity.EventRef]
    public string[] menuClickEvents;

    //This is for all the music in the game
    [FMODUnity.EventRef]
    public string[] musicEvents;
    FMOD.Studio.EventInstance musicState;

    //This is for all the music in the game
    [FMODUnity.EventRef]
    public string[] menuAmbienceEvents;
    FMOD.Studio.EventInstance menuAmbienceState;

    //This is for the player sound effects
    [FMODUnity.EventRef]
    public string PlayerStateEvent = "";
    FMOD.Studio.EventInstance playerState;

    void Awake() {
        if (instance == null) {
            DontDestroyOnLoad(gameObject);
            instance = this;
        }
        else {
            Destroy(gameObject);
        }
    }

    public void MenuButtonEvents(string clickEvent) {
        switch (clickEvent) {
        case "EasyMode":
            FMODUnity.RuntimeManager.PlayOneShot(menuClickEvents[0], transform.position);
            break;
        case "MediumMode":
            FMODUnity.RuntimeManager.PlayOneShot(menuClickEvents[1], transform.position);
            break;
        case "HardMode":
            FMODUnity.RuntimeManager.PlayOneShot(menuClickEvents[2], transform.position);
            break;
        case "Valve":
            FMODUnity.RuntimeManager.PlayOneShot(menuClickEvents[3], transform.position);
            break;
        case "ExitGame":
            FMODUnity.RuntimeManager.PlayOneShot(menuClickEvents[4], transform.position);
            break;
        case "PlayGame":
            FMODUnity.RuntimeManager.PlayOneShot(menuClickEvents[5], transform.position);
            break;
        default:
            break;
        }
    }

    public void MusicEvents(string sceneEvent) {
        switch (sceneEvent) {
        case "MainMenu":
            musicState = FMODUnity.RuntimeManager.CreateInstance(musicEvents[0]);
            musicState.start();
            menuAmbienceState = FMODUnity.RuntimeManager.CreateInstance(menuAmbienceEvents[0]);
            menuAmbienceState.start();
            break;
        default:
            break;
        }
    }
}
