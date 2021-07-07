using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour {
    
    public static AudioManager instance;

    public AudioSource audioSourceQuickSounds;
    public AudioSource audioSourceMovement;
    public AudioSource audioSourceBooster;
    public AudioSource audioSourceMusic;
    public AudioSource audioSourceWind;
    public AudioSource audioSourceSteam;
    public AudioSource audioSourceHumBuz;

    public bool soundOn = true;

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

    [Header("Menu Sound Manager")]
    public AudioClip[] menuAudioClips;
    public float menuSoundVolume = 0.3f;

    [Header("Music Manager")]
    public AudioClip[] musicAudioClips;
    public float musicVolume = 0.3f;
    public bool musicOn = true;

    void Awake() {
        if (instance == null) {
            DontDestroyOnLoad(gameObject);
            instance = this;
        }
        else {
            Destroy(gameObject);
        }
    }

    void Start() {
        MusicSwitcher();
    }

    public void PlayGrappleSoundEffect() {
        if (soundOn) {
            audioSourceQuickSounds.clip = grappleAudioClips[Random.Range(0, grappleAudioClips.Length)];
            audioSourceQuickSounds.pitch = Random.Range(minGrapplePitch, maxGrapplePitch);
            audioSourceQuickSounds.volume = grappleVolume;
            audioSourceQuickSounds.Play();
        }
    }

    public void PlayReelReturnSoundEffect() {
        if (soundOn) {
            audioSourceQuickSounds.clip = reelReturnAudioClips[Random.Range(0, reelReturnAudioClips.Length)];
            audioSourceQuickSounds.pitch = Random.Range(minReelReturnPitch, maxReelReturnPitch);
            audioSourceQuickSounds.volume = reelReturnVolume;
            audioSourceQuickSounds.Play();
        }
    }

    public void PlayBoosterSoundEffect() {
        if (soundOn) StartCoroutine(PlayBoosterSound());
    }

    IEnumerator PlayBoosterSound() {
        audioSourceBooster.volume = boosterVolume;

        if (audioSourceBooster.isPlaying == false) {
            audioSourceBooster.clip = boosterBurnAudioClips[Random.Range(0, boosterBurnAudioClips.Length)];
            audioSourceBooster.Play();
            yield return new WaitForSeconds(boosterBurnAudioClips.Length);
        }
    }    

    public void PlayerMovementSoundEffect() {
        if (soundOn) StartCoroutine(PlayMovementSound());
    }

    IEnumerator PlayMovementSound() {
        audioSourceMovement.clip = movementAudioClips;
        audioSourceMovement.volume = movementVolume;
        

        if (audioSourceMovement.isPlaying == false) {
            audioSourceMovement.Play();
            yield return new WaitForSeconds(movementAudioClips.length);
        }
    }

    public void PlayImpactSoundEffect() {
        if (soundOn) {
            audioSourceQuickSounds.clip = impactAudioClips[Random.Range(0, impactAudioClips.Length)];
            audioSourceQuickSounds.pitch = Random.Range(minImpactPitch, maxImpactPitch);
            audioSourceQuickSounds.volume = impactVolume;
            audioSourceQuickSounds.Play();
        }
    }

    public void MusicSwitcher() {
        if (GameManager.instance.GetCurrentScene() == GameManager.SceneType.mainMenu && musicOn) {
            audioSourceMusic.clip = musicAudioClips[0];
            audioSourceMusic.Play();
            audioSourceWind.clip = musicAudioClips[1];
            audioSourceWind.Play();
            audioSourceSteam.clip = musicAudioClips[2];
            audioSourceSteam.Play();
            audioSourceHumBuz.clip = musicAudioClips[3];
            audioSourceHumBuz.Play();
        }
        else {
            audioSourceMusic.Stop();
            audioSourceWind.Stop();
            audioSourceSteam.Stop();
            audioSourceHumBuz.Stop();
        }
        
        if (GameManager.instance.GetCurrentScene() == GameManager.SceneType.playerTestScene && musicOn) {
            //audioSourceMusic.clip = musicAudioClips[1];
            //audioSourceMusic.Play();
        }
        else {
            //audioSourceMusic.Stop();
        }
    }

    public void MenuSounds(int soundSelection) {
        audioSourceQuickSounds.volume = menuSoundVolume;
        if (soundOn) {
            if (soundSelection == 1) {
                audioSourceQuickSounds.clip = menuAudioClips[0];
                audioSourceQuickSounds.Play();
            }
            else if (soundSelection == 2) {
                audioSourceQuickSounds.clip = menuAudioClips[1];
                audioSourceQuickSounds.Play();
            }
            else if (soundSelection == 3) {
                audioSourceQuickSounds.clip = menuAudioClips[2];
                audioSourceQuickSounds.Play();
            }
            else if (soundSelection == 4) {
                audioSourceQuickSounds.clip = menuAudioClips[3];
                audioSourceQuickSounds.Play();
            }
            else if (soundSelection == 5) {
                audioSourceQuickSounds.clip = menuAudioClips[4];
                audioSourceQuickSounds.Play();
            }
        }
    }
}
