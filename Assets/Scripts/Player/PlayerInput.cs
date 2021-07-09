using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerInput : MonoBehaviour {
    private PlayerController playerController;
    private Control controls;

    void Awake() {
        controls = new Control();
        controls.MainMenuControls.ChangeDifficulty.performed += ctx => ChargeStation();
        controls.MainMenuControls.ChangeDifficulty.performed += ctx => SoundChanger();
        controls.MainMenuControls.ChangeDifficulty.performed += ctx => MusicChanger();
        controls.MainMenuControls.ChangeDifficulty.performed += ctx => PlayGame();
        controls.MainMenuControls.ChangeDifficulty.performed += ctx => ExitGame();

        controls.PlayerMovement.GrappleStart.performed += ctx => GrappleStart();
        controls.PlayerMovement.GrappleEnd.performed += ctx => GrappleEnd();
        controls.PlayerMovement.Megaboost.performed += ctx => Megaboost();

        controls.PlayerSpawnerControls.Spawning.performed += ctx => SpawnPlayer();
    }
    
    void Start() {
        switch (GameManager.instance.GetCurrentScene()) {
        case GameManager.SceneType.mainMenu:
            controls.PlayerMovement.Disable();
            controls.MainMenuControls.Enable();
            controls.PlayerSpawnerControls.Disable();
            break;
        case GameManager.SceneType.withPlayer:
            controls.PlayerMovement.Disable();
            controls.MainMenuControls.Disable();
            controls.PlayerSpawnerControls.Enable();
            break;
        }
    }
    public void NewPlayer(PlayerController player) {
        playerController = player;
        controls.PlayerMovement.Enable();
    }

    void OnEnable() {
        controls.Enable();
        Cursor.lockState = CursorLockMode.Confined;
    }

    void OnDisable() {
        controls.Disable();
        Cursor.lockState = CursorLockMode.None;
    }

    public void PlayerDeath() {
        controls.PlayerMovement.Disable();
        controls.PlayerSpawnerControls.Enable();
    }

    public Vector2 GetPointerPos() {
        return controls.CameraControl.CameraControl.ReadValue<Vector2>();
    }

    // Main Menu Controls:
    private void ChargeStation() {
        MainMenuManager.instance.ChargeStation();
    }

    private void SoundChanger() {
        MainMenuManager.instance.SoundChanger();
    }

    private void MusicChanger() {
        MainMenuManager.instance.MusicChanger();
    }

    private void ExitGame() {
        MainMenuManager.instance.ExitGame();
    }
    private void PlayGame() {
        MainMenuManager.instance.PlayGame();
    }

    // Spawning Controls:
    private void SpawnPlayer() {
        GameManager.instance.SpawnPlayer();
        controls.PlayerSpawnerControls.Disable();
    }

    // Player Movement Controls: 
    private void GrappleStart() {
        playerController.GrappleStart();
    }

    private void GrappleEnd() {
        playerController.GrappleEnd();
    }

    private void Megaboost() {
        playerController.Megaboost();
    }

    public Vector2 GetPlayerMovement() {
        return controls.PlayerMovement.PlayerMove.ReadValue<Vector2>().normalized;
    }

    public bool IsBoosting() {
        return controls.PlayerMovement.Boost.ReadValue<float>() != 0;
    }

    public bool IsSlowing() {
        return controls.PlayerMovement.Slow.ReadValue<float>() != 0;
    }

    public bool DirectCameraControl() {
        return controls.PlayerMovement.DirectCameraControl.ReadValue<float>() != 0;
    }

    public Vector2 DirectCameraControlDelta() {
        return controls.PlayerMovement.DirectCameraControlDelta.ReadValue<Vector2>();
    }
}
