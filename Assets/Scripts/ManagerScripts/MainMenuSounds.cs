using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuSounds : MonoBehaviour {
    
    [FMODUnity.EventRef] public string[] audioEvents;
    public FMOD.Studio.EventInstance[] instances;

    public enum Sound {
        SelectEasy,
        SelectMedium,
        SelectHard,
        Valve,
        TVSwitch,
        MenuClick
    }

    void Start() {
        int soundCount = audioEvents.Length;
        instances = new FMOD.Studio.EventInstance[soundCount];
        for (int i = 0; i < soundCount; i++) {
            instances[i] = FMODUnity.RuntimeManager.CreateInstance(audioEvents[i]);
        }
    }

    public void DifficultySelection(int difficulty) {
        if (difficulty == 1) {
            instances[(int)Sound.SelectEasy].start();
        }
        else if (difficulty == 2) {
            instances[(int)Sound.SelectMedium].start();
        }
        else if (difficulty == 3) {
            instances[(int)Sound.SelectHard].start();
        }
    }

    public void Valve() {
        instances[(int)Sound.Valve].start();
    }
}
