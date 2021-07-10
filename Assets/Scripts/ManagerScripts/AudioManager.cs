using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour {
    
    public static AudioManager instance; //Creating a singleton object

    //This is for all of the clicks, switched, and buttons in the Main Menu
    [FMODUnity.EventRef]
    public string[] menuClickEvents;
    FMOD.Studio.EventInstance menuClickState;

    //This is for all the music in the game
    [FMODUnity.EventRef]
    public string[] musicEvents;
    FMOD.Studio.EventInstance musicState;

    //This is for all the menu ambience in the game
    [FMODUnity.EventRef]
    public string[] menuAmbienceEvents;
    public FMOD.Studio.EventInstance menuAmbienceState;

    //This is for the player sound effects
    [FMODUnity.EventRef]
    public string[] PlayerStateEvent;
    public FMOD.Studio.EventInstance playerState;

    public bool canPlayMusic = true;
    public bool canPlaySounds = true;

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
        if (canPlaySounds) {
            switch (clickEvent) {
                case "EasyMode":
                    menuClickState.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
                    menuClickState = FMODUnity.RuntimeManager.CreateInstance(menuClickEvents[0]);
                    menuClickState.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(gameObject));
                    menuClickState.start();
                    break;
                case "MediumMode":
                    menuClickState.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
                    menuClickState = FMODUnity.RuntimeManager.CreateInstance(menuClickEvents[1]);
                    menuClickState.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(gameObject));
                    menuClickState.start();
                    break;
                case "HardMode":
                    menuClickState.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
                    menuClickState = FMODUnity.RuntimeManager.CreateInstance(menuClickEvents[2]);
                    menuClickState.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(gameObject));
                    menuClickState.start();
                    break;
                case "Valve":
                    menuClickState.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
                    menuClickState = FMODUnity.RuntimeManager.CreateInstance(menuClickEvents[3]);
                    menuClickState.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(gameObject));
                    menuClickState.start();
                    break;
                case "ExitGame":
                    menuClickState.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
                    menuClickState = FMODUnity.RuntimeManager.CreateInstance(menuClickEvents[4]);
                    menuClickState.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(gameObject));
                    menuClickState.start();
                    break;
                case "PlayGame":
                    menuClickState.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
                    menuClickState = FMODUnity.RuntimeManager.CreateInstance(menuClickEvents[5]);
                    menuClickState.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(gameObject));
                    menuClickState.start();
                    break;
                default:
                    break;
            }
        }
    }

    public void MusicEvents(string sceneEvent) {
        if (canPlayMusic) {
            switch (sceneEvent) {
                case "MainMenu":
                    //musicState = FMODUnity.RuntimeManager.CreateInstance(musicEvents[0]);
                    //musicState.start();
                    menuAmbienceState = FMODUnity.RuntimeManager.CreateInstance(menuAmbienceEvents[0]);
                    menuAmbienceState.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(gameObject));
                    menuAmbienceState.start();
                    menuAmbienceState = FMODUnity.RuntimeManager.CreateInstance(menuAmbienceEvents[1]);
                    menuAmbienceState.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(gameObject));
                    menuAmbienceState.start();
                    menuAmbienceState = FMODUnity.RuntimeManager.CreateInstance(menuAmbienceEvents[2]);
                    menuAmbienceState.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(gameObject));
                    menuAmbienceState.start();
                    break;
                default:
                    break;
            }
        }
    }

    public void PlayerMovement() {
        GameObject player = GameObject.Find("Player");
        FMOD.Studio.PLAYBACK_STATE playbackState;
        playerState.getPlaybackState(out playbackState);
        if (playbackState != FMOD.Studio.PLAYBACK_STATE.PLAYING) {
            playerState = FMODUnity.RuntimeManager.CreateInstance(PlayerStateEvent[0]);
            playerState.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(gameObject));
            FMODUnity.RuntimeManager.AttachInstanceToGameObject(playerState, player.GetComponent<Transform>());
            playerState.start();
        }
    }

    public void PlayerBoost() {
        GameObject player = GameObject.Find("Player");
        FMOD.Studio.PLAYBACK_STATE playbackState;
        playerState.getPlaybackState(out playbackState);
        if (playbackState != FMOD.Studio.PLAYBACK_STATE.PLAYING) {
            playerState = FMODUnity.RuntimeManager.CreateInstance(PlayerStateEvent[2]);
            playerState.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(gameObject));
            FMODUnity.RuntimeManager.AttachInstanceToGameObject(playerState, player.GetComponent<Transform>());
            playerState.start();
        }
        else if (playbackState == FMOD.Studio.PLAYBACK_STATE.PLAYING) {
           playerState.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
        }
    }

    public void PlayerGrapple() {
        GameObject player = GameObject.Find("Player");
        playerState.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(gameObject));
        FMODUnity.RuntimeManager.AttachInstanceToGameObject(playerState, player.GetComponent<Transform>());
        FMODUnity.RuntimeManager.PlayOneShot(PlayerStateEvent[3], player.transform.position);
    }

    public void PlayerReelReturn() {
        GameObject player = GameObject.Find("Player");
        playerState.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(gameObject));
        FMODUnity.RuntimeManager.AttachInstanceToGameObject(playerState, player.GetComponent<Transform>());
        FMODUnity.RuntimeManager.PlayOneShot(PlayerStateEvent[4], player.transform.position);
    }

    public void PlayerImpact() {
        GameObject player = GameObject.Find("Player");
        playerState.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(gameObject));
        FMODUnity.RuntimeManager.AttachInstanceToGameObject(playerState, player.GetComponent<Transform>());
        FMODUnity.RuntimeManager.PlayOneShot(PlayerStateEvent[5], player.transform.position);
    }
    
    public void StopAllPlayerEvents() {
        playerState.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
    }
}
