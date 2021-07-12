using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAudio : MonoBehaviour {
    [FMODUnity.EventRef] public string[] audioEvents;
    public FMOD.Studio.EventInstance[] instances;
    public enum Sound {
        GrappleLaunch,
        BoostStart,
        BoostLoop,
        GrappleReelOut,
        GrappleReelIn,
        PlayerDeath,
        GrappleHit,
        GrappleRehouse,
        GrappleUngrasp,
        DeathGibs,
        ImpactHard,
        ImpactSoft,
        Movement,
        Parachute,
        EngageAlert,
    }

    private Transform playerTransform;

    public void InitInstances(Transform transform) {
        playerTransform = transform;
        int soundCount = audioEvents.Length;
        instances = new FMOD.Studio.EventInstance[soundCount];
        for (int i = 0; i < soundCount; i++) {
            instances[i] = FMODUnity.RuntimeManager.CreateInstance(audioEvents[i]);
            instances[i].set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(playerTransform));
        }
    }



    public void BoostStart() {
        FMODUnity.RuntimeManager.AttachInstanceToGameObject(instances[(int)Sound.BoostStart], playerTransform);
        instances[(int)Sound.BoostStart].start();
        FMODUnity.RuntimeManager.AttachInstanceToGameObject(instances[(int)Sound.BoostLoop], playerTransform);
        instances[(int)Sound.BoostLoop].start();
    }

    public void BoostEnd() {
        instances[(int)Sound.BoostLoop].stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
    }

    public void GrappleReelOutStart() {
        FMODUnity.RuntimeManager.AttachInstanceToGameObject(instances[(int)Sound.GrappleLaunch], playerTransform);
        instances[(int)Sound.GrappleLaunch].start();
        FMODUnity.RuntimeManager.AttachInstanceToGameObject(instances[(int)Sound.GrappleReelOut], playerTransform);
        instances[(int)Sound.GrappleReelOut].start();
    }

    public void GrappleReelOutEnd() {
        instances[(int)Sound.GrappleReelOut].stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
    }

    public void GrappleHit(Vector3 grapplePoint) {
        instances[(int)Sound.GrappleHit].set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(grapplePoint));
        instances[(int)Sound.GrappleHit].start();
        instances[(int)Sound.EngageAlert].set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(playerTransform));
        instances[(int)Sound.EngageAlert].start();
    }

    public void GrappleReelInStart (Vector3 grapplePoint) {
        FMODUnity.RuntimeManager.AttachInstanceToGameObject(instances[(int)Sound.GrappleReelIn], playerTransform);
        instances[(int)Sound.GrappleReelIn].start();
        instances[(int)Sound.GrappleUngrasp].set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(grapplePoint));
        instances[(int)Sound.GrappleUngrasp].start();
        GrappleReelOutEnd();
    }

    public void GrappleReelInEnd() {
        instances[(int)Sound.GrappleReelIn].stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        FMODUnity.RuntimeManager.AttachInstanceToGameObject(instances[(int)Sound.GrappleRehouse], playerTransform);
        instances[(int)Sound.GrappleRehouse].start();
    }

    public void PlayerDeath() {
        BoostEnd();
        GrappleReelInEnd();
        GrappleReelOutEnd();
        instances[(int)Sound.PlayerDeath].set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(playerTransform));
        instances[(int)Sound.PlayerDeath].start();
    }

    public void SlowStart() {
        Debug.Log("Parachuting");
        FMODUnity.RuntimeManager.AttachInstanceToGameObject(instances[(int)Sound.Parachute], playerTransform);
        instances[(int)Sound.GrappleReelIn].start();
    }

    public void SlowEnd() {
        instances[(int)Sound.Parachute].stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
    }
}