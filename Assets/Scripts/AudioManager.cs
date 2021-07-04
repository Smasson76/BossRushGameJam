using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour {
    
    public static AudioManager instance;

    public AudioSource audioSource;

    [Header("Grapple Sound Manager")]
    public AudioClip[] grappleAudioClips;
    public int minGrapplePitch = 1;
    public int maxGrapplePitch = 3;

    [Header("Reel Return Sound Manager")]
    public AudioClip[] reelReturnAudioClips;
    public int minReelReturnPitch = 1;
    public int maxReelReturnPitch = 3;

    [Header("Booster Sound Manager")]
    public AudioClip boosterAudioClips;
    public int minBoosterPitch = 1;
    public int maxBoosterPitch = 3;

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
        audioSource.clip = grappleAudioClips[Random.Range(0, grappleAudioClips.Length)];
        audioSource.pitch = Random.Range(minGrapplePitch, maxGrapplePitch);
        audioSource.Play();
    }

    public void PlayReelReturnSoundEffect() {
        audioSource.clip = reelReturnAudioClips[Random.Range(0, reelReturnAudioClips.Length)];
        audioSource.pitch = Random.Range(minReelReturnPitch, maxReelReturnPitch);
        audioSource.Play();
    }

    public void PlayBoosterSoundEffect() {
        //audioSource.clip = boosterAudioClips[Random.Range(0, boosterAudioClips.Length)];
        //audioSource.pitch = Random.Range(minBoosterPitch, maxBoosterPitch);
        audioSource.PlayOneShot(boosterAudioClips, 10f);
    }
}
