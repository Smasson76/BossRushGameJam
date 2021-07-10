using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour {
    
    public static AudioManager instance; //Creating a singleton object

    //This allows us to use EventRef attribute and will present the designer with the UI for selecting events
    [FMODUnity.EventRef]
    public string PlayerStateEvent = "";

    //EventInstance class will allow us to manage an event over it's lifetime. Including Starting, stopping, and changing parameters
    FMOD.Studio.EventInstance playerState;

    //These events are one shot sounds. They are sounds that have a finite length. 
    //We do not store an EventInstance to manage the sounds. Once started they will play to completion.
    [FMODUnity.EventRef]
    public string DeathEvent = "";
    [FMODUnity.EventRef]
    public string HealEvent = "";

    //One shot event system that will have a tracked state and take action when it ends. Could also change parameter values over the lifetime.
    [FMODUnity.EventRef]
    public string PlayerIntroEvent = "";
    FMOD.Studio.EventInstance playerIntro;

    void Awake() {
        if (instance == null) {
            DontDestroyOnLoad(gameObject);
            instance = this;
        }
        else {
            Destroy(gameObject);
        }
    }

    void Start() {
        playerState = FMODUnity.RuntimeManager.CreateInstance(PlayerStateEvent);
        //playerState.start();

        FMODUnity.RuntimeManager.AttachInstanceToGameObject(playerState, GetComponent<Transform>());
    }

    public void IgnoreEW() {
        /*playerState = FMODUnity.RuntimeManager.CreateInstance(PlayerStateEvent);
        playerState.start();

        playerIntro = FMODUnity.RuntimeManager.CreateInstance(PlayerIntroEvent);
        playerIntro.start();

        FMODUnity.RuntimeManager.AttachInstanceToGameObject(playerIntro, GetComponent<Transform>());*/
    }

    public void DeathSound() {

        //FMODUnity.RuntimeManager.PlayOneShot(DeathEvent, transform.position);
        playerState.start();
        //playerState.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
    }
}
