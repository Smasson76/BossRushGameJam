using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuSounds : MonoBehaviour {
    
    [FMODUnity.EventRef] public string[] audioEvents;
    public FMOD.Studio.EventInstance[] instance;

    public void DifficultySelection(int difficulty) {
        if (difficulty == 1) {
            instance[0] = FMODUnity.RuntimeManager.CreateInstance(audioEvents[0]);
            instance[0].set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(gameObject));
            instance[0].start();
        }
        else if (difficulty == 2) {
            instance[1] = FMODUnity.RuntimeManager.CreateInstance(audioEvents[1]);
            instance[1].set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(gameObject));
            instance[1].start();
        }
        else if (difficulty == 3) {
            instance[2] = FMODUnity.RuntimeManager.CreateInstance(audioEvents[2]);
            instance[2].set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(gameObject));
            instance[2].start();
        }
    }
}
