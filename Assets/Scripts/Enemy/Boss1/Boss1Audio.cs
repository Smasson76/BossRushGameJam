using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss1Audio : MonoBehaviour {
    
    [FMODUnity.EventRef] public string[] audioEvents;
    public FMOD.Studio.EventInstance[] instances;
    public enum Sound {
        ArmMove,
        ArmDestroy,
    }

    //Note to Nic: We can always set up the Rigidbodies manually.
    //Because we want the sounds playing in other places besides the "bossRb" the whole time.
    //Like, we might want a sound on the arm instead.
    private Rigidbody bossArm;

    public void InitInstances(Rigidbody bossRigidbody) {
        bossArm = bossRigidbody;
        int soundCount = audioEvents.Length;
        instances = new FMOD.Studio.EventInstance[soundCount];
        for (int i = 0; i < soundCount; i++) {
            instances[i] = FMODUnity.RuntimeManager.CreateInstance(audioEvents[i]);
            instances[i].set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(bossArm.transform, bossArm));
        }
    }

    public void ArmMove() {
        FMODUnity.RuntimeManager.AttachInstanceToGameObject(instances[(int)Sound.ArmMove], bossArm.transform, bossArm);
        instances[(int)Sound.ArmMove].start();
    }

    public void ArmDestroy() {
        FMODUnity.RuntimeManager.AttachInstanceToGameObject(instances[(int)Sound.ArmDestroy], bossArm.transform, bossArm);
        instances[(int)Sound.ArmDestroy].start();
    }
}
