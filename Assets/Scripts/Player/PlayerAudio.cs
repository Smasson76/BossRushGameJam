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
        MovementInput,
        Parachute,
        EngageAlert,
        Rolling,
    }

    private Rigidbody playerRb;
    public delegate Vector3 GibsSoundCallback();

    public void InitInstances(Rigidbody playerRigidbody) {
        playerRb = playerRigidbody;
        int soundCount = audioEvents.Length;
        instances = new FMOD.Studio.EventInstance[soundCount];
        for (int i = 0; i < soundCount; i++) {
            instances[i] = FMODUnity.RuntimeManager.CreateInstance(audioEvents[i]);
            instances[i].set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(playerRb.transform, playerRb));
        }
    }



    public void BoostStart() {
        FMODUnity.RuntimeManager.AttachInstanceToGameObject(instances[(int)Sound.BoostStart], playerRb.transform, playerRb);
        instances[(int)Sound.BoostStart].start();
        FMODUnity.RuntimeManager.AttachInstanceToGameObject(instances[(int)Sound.BoostLoop], playerRb.transform, playerRb);
        instances[(int)Sound.BoostLoop].start();
    }

    public void BoostEnd() {
        instances[(int)Sound.BoostLoop].stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
    }

    public void GrappleReelOutStart() {
        FMODUnity.RuntimeManager.AttachInstanceToGameObject(instances[(int)Sound.GrappleLaunch], playerRb.transform, playerRb);
        instances[(int)Sound.GrappleLaunch].start();
        FMODUnity.RuntimeManager.AttachInstanceToGameObject(instances[(int)Sound.GrappleReelOut], playerRb.transform, playerRb);
        instances[(int)Sound.GrappleReelOut].start();
    }

    public void GrappleReelOutEnd() {
        instances[(int)Sound.GrappleReelOut].stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
    }

    public void GrappleHit(Vector3 grapplePoint) {
        instances[(int)Sound.GrappleHit].set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(grapplePoint));
        instances[(int)Sound.GrappleHit].start();
        instances[(int)Sound.EngageAlert].set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(playerRb.transform, playerRb));
        instances[(int)Sound.EngageAlert].start();
    }

    public void GrappleReelInStart (Vector3 grapplePoint) {
        FMODUnity.RuntimeManager.AttachInstanceToGameObject(instances[(int)Sound.GrappleReelIn], playerRb.transform, playerRb);
        instances[(int)Sound.GrappleReelIn].start();
        instances[(int)Sound.GrappleUngrasp].set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(grapplePoint));
        instances[(int)Sound.GrappleUngrasp].start();
        GrappleReelOutEnd();
    }

    public void GrappleReelInEnd() {
        instances[(int)Sound.GrappleReelIn].stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        FMODUnity.RuntimeManager.AttachInstanceToGameObject(instances[(int)Sound.GrappleRehouse], playerRb.transform, playerRb);
        instances[(int)Sound.GrappleRehouse].start();
    }

    public void PlayerDeath() {
        BoostEnd();
        GrappleReelInEnd();
        GrappleReelOutEnd();
        instances[(int)Sound.PlayerDeath].set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(playerRb.transform, playerRb));
        instances[(int)Sound.PlayerDeath].start();
    }

    public void SlowStart() {
        Debug.Log("Parachuting");
        FMODUnity.RuntimeManager.AttachInstanceToGameObject(instances[(int)Sound.Parachute], playerRb.transform, playerRb);
        instances[(int)Sound.Parachute].start();
    }

    public void SlowEnd() {
        instances[(int)Sound.Parachute].stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
    }

    public void HitSomething(Vector3 hitLocation, float hitspeed) {
        if (hitspeed > 20) {
            instances[(int)Sound.ImpactHard].set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(hitLocation));
            instances[(int)Sound.ImpactHard].start();
        } else if (hitspeed > 5) {
            instances[(int)Sound.ImpactSoft].set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(hitLocation));
            instances[(int)Sound.ImpactSoft].start();
        }
    }

    public void GibsHit(Vector3 hitLocation) {
        FMOD.Studio.EventInstance instance = FMODUnity.RuntimeManager.CreateInstance(audioEvents[(int)Sound.DeathGibs]);
        instance.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(hitLocation));
        instance.start();
        instance.release();
    }

    public void MovementInputStart() {
        FMODUnity.RuntimeManager.AttachInstanceToGameObject(instances[(int)Sound.MovementInput], playerRb.transform, playerRb);
        instances[(int)Sound.MovementInput].start();
    }

    public void MovementInputStop() {
        instances[(int)Sound.MovementInput].stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
    }

    public void RollingStart() {
        FMODUnity.RuntimeManager.AttachInstanceToGameObject(instances[(int)Sound.Rolling], playerRb.transform, playerRb);
        instances[(int)Sound.Rolling].start();
    }

    public void RollingStop() {
        instances[(int)Sound.Rolling].stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
    }
}