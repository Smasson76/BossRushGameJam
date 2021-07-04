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
    public float grappleVolume = 0.3f;

    [Header("Reel Return Sound Manager")]
    public AudioClip[] reelReturnAudioClips;
    public int minReelReturnPitch = 1;
    public int maxReelReturnPitch = 3;
    public float reelReturnVolume = 0.3f;

    [Header("Booster Sound Manager")]
    public AudioClip[] boosterAudioClips;
    public int minBoosterPitch = 1;
    public int maxBoosterPitch = 3;
    public float boosterVolume = 0.3f;

    [Header("Player Movement Sound Manager")]
    public AudioClip movementAudioClips;
    public int minMovementPitch = 1;
    public int maxMovementPitch = 3;
    public float movementVolume = 0.3f;

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
        audioSource.volume = grappleVolume;
        audioSource.Play();
    }

    public void PlayReelReturnSoundEffect() {
        audioSource.clip = reelReturnAudioClips[Random.Range(0, reelReturnAudioClips.Length)];
        audioSource.pitch = Random.Range(minReelReturnPitch, maxReelReturnPitch);
        audioSource.volume = reelReturnVolume;
        audioSource.Play();
    }

    public void PlayBoosterSoundEffect() {
        audioSource.clip = boosterAudioClips[Random.Range(0, boosterAudioClips.Length)];
        audioSource.pitch = Random.Range(minBoosterPitch, maxBoosterPitch);
        audioSource.volume = boosterVolume;
        audioSource.Play();
    }

    public void PlayerMovementSoundEffect() {
        StartCoroutine(PlayMovementSound());
    }

    IEnumerator PlayMovementSound() {
        audioSource.clip = movementAudioClips;
        audioSource.pitch = Random.Range(minMovementPitch, maxMovementPitch);
        audioSource.volume = movementVolume;

        if (audioSource.isPlaying == false) {
            audioSource.Play();
            yield return new WaitForSeconds(movementAudioClips.length);
        }
    }
}
