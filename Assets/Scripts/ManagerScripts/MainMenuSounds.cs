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

    public Transform ballTransform;
    public Transform camTransform;

    public bool canPlayMusic = true;
    public bool canPlaySounds = true;

    void Start() {
        int soundCount = audioEvents.Length;
        audioInstances = new FMOD.Studio.EventInstance[soundCount];
        for (int i = 0; i < soundCount; i++) {
            audioInstances[i] = FMODUnity.RuntimeManager.CreateInstance(audioEvents[i]);
        }

        InitAmbienceSounds();
    }

    public void InitAmbienceSounds() {
        int musicCount = musicEvents.Length;
        musicInstances = new FMOD.Studio.EventInstance[musicCount];
        for (int i = 0; i < musicCount; i++) {
            musicInstances[i] = FMODUnity.RuntimeManager.CreateInstance(musicEvents[i]);
            musicInstances[i].set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(ballTransform));
        }
        
        PlayAmbienceSounds();
    }

    public void PlayAmbienceSounds() {
        if (canPlayMusic) {
            FMODUnity.RuntimeManager.AttachInstanceToGameObject(musicInstances[(int)Music.Steam], ballTransform);
            musicInstances[(int)Music.Steam].start();
            FMODUnity.RuntimeManager.AttachInstanceToGameObject(musicInstances[(int)Music.Wind], camTransform);
            musicInstances[(int)Music.Wind].start();
            FMODUnity.RuntimeManager.AttachInstanceToGameObject(musicInstances[(int)Music.Buzz], ballTransform);
            musicInstances[(int)Music.Buzz].start();
        }
        else {
            musicInstances[(int)Music.Steam].stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
            musicInstances[(int)Music.Wind].stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
            musicInstances[(int)Music.Buzz].stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        }
    }

    public void DifficultySelection(int difficulty) {
        if (canPlaySounds) {
            if (difficulty == 1)
                audioInstances[(int)Sound.SelectEasy].start();
            else if (difficulty == 2)
                audioInstances[(int)Sound.SelectMedium].start();
            else if (difficulty == 3)
                audioInstances[(int)Sound.SelectHard].start();
        }
    }

    public void Valve() {
        if (canPlaySounds) audioInstances[(int)Sound.Valve].start();
    }

    public void TVSwitch() {
        if (canPlaySounds) audioInstances[(int)Sound.TVSwitch].start();
    }

    public void MenuClickPlay() {
        if (canPlaySounds) audioInstances[(int)Sound.MenuClick].start();
    } 
}
