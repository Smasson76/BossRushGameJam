using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuSounds : MonoBehaviour {
    
    [FMODUnity.EventRef] public string[] audioEvents;
    public FMOD.Studio.EventInstance[] audioInstances;

    [FMODUnity.EventRef] public string[] musicEvents;
    public FMOD.Studio.EventInstance[] musicInstances;

    public enum Sound {
        SelectEasy,
        SelectMedium,
        SelectHard,
        Valve,
        TVSwitch,
        MenuClick,
    }

    public enum Music {
        Steam,
        Wind,
        Buzz,
    }

    public Transform ourTransform;

    void Start() {
        int soundCount = audioEvents.Length;
        audioInstances = new FMOD.Studio.EventInstance[soundCount];
        for (int i = 0; i < soundCount; i++) {
            audioInstances[i] = FMODUnity.RuntimeManager.CreateInstance(audioEvents[i]);
        }

        InitAmbienceSounds();
    }

    void InitAmbienceSounds() {
        //ourTransform = transform;
        int musicCount = musicEvents.Length;
        musicInstances = new FMOD.Studio.EventInstance[musicCount];
        for (int i = 0; i < musicCount; i++) {
            musicInstances[i] = FMODUnity.RuntimeManager.CreateInstance(musicEvents[i]);
            musicInstances[i].set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(ourTransform));
        }
        
        PlayAmbienceSounds();
    }

    void PlayAmbienceSounds() {
        FMODUnity.RuntimeManager.AttachInstanceToGameObject(musicInstances[(int)Music.Steam], ourTransform);
        musicInstances[(int)Music.Steam].start();
        /*FMODUnity.RuntimeManager.AttachInstanceToGameObject(musicInstances[(int)Music.Wind], ourTransform);
        musicInstances[(int)Music.Wind].start();
        FMODUnity.RuntimeManager.AttachInstanceToGameObject(musicInstances[(int)Music.Buzz], ourTransform);
        musicInstances[(int)Music.Buzz].start();*/
    }

    public void DifficultySelection(int difficulty) {
        if (difficulty == 1) {
            audioInstances[(int)Sound.SelectEasy].start();
        }
        else if (difficulty == 2) {
            audioInstances[(int)Sound.SelectMedium].start();
        }
        else if (difficulty == 3) {
            audioInstances[(int)Sound.SelectHard].start();
        }
    }

    public void Valve() {
        audioInstances[(int)Sound.Valve].start();
    }

    public void TVSwitch() {
        audioInstances[(int)Sound.TVSwitch].start();
    }

    public void MenuClickPlay() {
        audioInstances[(int)Sound.MenuClick].start();
    } 
}
