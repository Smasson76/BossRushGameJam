using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour {
    
    public static AudioManager instance;

    public AudioSource audioSourceQuickSounds;
    public AudioSource audioSourceMovement;
    public AudioSource audioSourceBooster;

    [Header("Grapple Sound Manager")]
    public AudioClip[] grappleAudioClips;
    public int minGrapplePitch = 1;
    public int maxGrapplePitch = 3;
    public float grappleVolume = 0.3f;

    [Header("Reel Return Sound Manager")]
    public AudioClip[] reelReturnAudioClips;
    public int minReelReturnPitch = 1;
    public int maxReelReturnPitch = 3;
    public float reelReturnVolume = 0.3f;

    [Header("Booster Sound Manager")]
    public AudioClip[] boosterAudioClips;
    public AudioClip[] boosterBurnAudioClips;
    //public int minBoosterPitch = 1;
    //public int maxBoosterPitch = 3;
    public float boosterVolume = 0.3f;

    [Header("Player Movement Sound Manager")]
    public AudioClip movementAudioClips;
    public int minMovementPitch = 1;
    public int maxMovementPitch = 3;
    public float movementVolume = 0.3f;

    [Header("Impact Sound Manager")]
    public AudioClip[] impactAudioClips;
    public int minImpactPitch = 1;
    public int maxImpactPitch = 3;
    public float impactVolume = 0.3f;

    void Awake() {
        if (instance == null) {
            DontDestroyOnLoad(gameObject);
            instance = this;
        }
        else {
            Destroy(gameObject);
        }
    }

    public void PlayGrappleSoundEffect() {
        audioSourceQuickSounds.clip = grappleAudioClips[Random.Range(0, grappleAudioClips.Length)];
        audioSourceQuickSounds.pitch = Random.Range(minGrapplePitch, maxGrapplePitch);
        audioSourceQuickSounds.volume = grappleVolume;
        audioSourceQuickSounds.Play();
    }

    public void PlayReelReturnSoundEffect() {
        audioSourceQuickSounds.clip = reelReturnAudioClips[Random.Range(0, reelReturnAudioClips.Length)];
        audioSourceQuickSounds.pitch = Random.Range(minReelReturnPitch, maxReelReturnPitch);
        audioSourceQuickSounds.volume = reelReturnVolume;
        audioSourceQuickSounds.Play();
    }

    public void PlayBoosterSoundEffect() {
        StartCoroutine(PlayBoosterSound());
    }

    IEnumerator PlayBoosterSound() {
        audioSourceBooster.volume = boosterVolume;

        if (audioSourceBooster.isPlaying == false) {
            audioSourceBooster.clip = boosterBurnAudioClips[Random.Range(0, boosterBurnAudioClips.Length)];
            //audioSourceBooster.pitch = Random.Range(minBoosterPitch, maxBoosterPitch);
            audioSourceBooster.Play();
            yield return new WaitForSeconds(boosterBurnAudioClips.Length);
        }
    }    

    public void PlayerMovementSoundEffect() {
        StartCoroutine(PlayMovementSound());
    }

    IEnumerator PlayMovementSound() {
        audioSourceMovement.clip = movementAudioClips;
        audioSourceMovement.pitch = Random.Range(minMovementPitch, maxMovementPitch);
        audioSourceMovement.volume = movementVolume;

        if (audioSourceMovement.isPlaying == false) {
            audioSourceMovement.Play();
            yield return new WaitForSeconds(movementAudioClips.length);
        }
    }

    public void PlayImpactSoundEffect() {
        audioSourceQuickSounds.clip = impactAudioClips[Random.Range(0, impactAudioClips.Length)];
        audioSourceQuickSounds.pitch = Random.Range(minImpactPitch, maxImpactPitch);
        audioSourceQuickSounds.volume = impactVolume;
        audioSourceQuickSounds.Play();
    }
}
