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
        controls.MainMenuControls.Click.performed += ctx => MainMenuClick();

        controls.PlayerMovement.BoostStart.performed += ctx => BoostStart();
        controls.PlayerMovement.BoostEnd.performed += ctx => BoostEnd();
        controls.PlayerMovement.GrappleStart.performed += ctx => GrappleStart();
        controls.PlayerMovement.GrappleEnd.performed += ctx => GrappleEnd();
        controls.PlayerMovement.SlowStart.performed += ctx => SlowStart();
        controls.PlayerMovement.SlowEnd.performed += ctx => SlowEnd();
        controls.PlayerMovement.Megaboost.performed += ctx => Megaboost();

        controls.PlayerSpawnerControls.Spawning.performed += ctx => SpawnPlayer();
    }

    void OnEnable() {
        controls.Enable();
        Cursor.lockState = CursorLockMode.Confined;
        GameManager.OnSceneChange += SceneChange;
        GameManager.OnPlayerSpawn += NewPlayer;
    }

    void OnDisable() {
        controls.Disable();
        Cursor.lockState = CursorLockMode.None;
        GameManager.OnSceneChange -= SceneChange;
        GameManager.OnPlayerSpawn -= NewPlayer;
    }

    void SceneChange(GameManager.SceneType sceneType) {
        switch (sceneType) {
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
        controls.PlayerSpawnerControls.Disable();
        controls.PlayerMovement.Enable();
    }

    public void PlayerDeath() {
        controls.PlayerMovement.Disable();
        controls.PlayerSpawnerControls.Enable();
    }

    public Vector2 GetPointerPos() {
        return controls.CameraControl.CameraControl.ReadValue<Vector2>();
    }

    // Main Menu Controls:
    private void MainMenuClick() {
        MainMenuManager.instance.Click();
    }
    // Spawning Controls:
    private void SpawnPlayer() {
        GameManager.instance.SpawnPlayer();
    }

    // Player Movement Controls:
    private void BoostStart() {
        playerController.BoostStart();
    }
    private void BoostEnd() {
        playerController.BoostEnd();
    }
    private void GrappleStart() {
        playerController.GrappleStart();
    }

    private void GrappleEnd() {
        playerController.GrappleEnd();
    }

    private void SlowStart() {
        playerController.SlowStart();
    }

    private void SlowEnd() {
        playerController.SlowEnd();
    }

    public bool IsSlowing() {
        return controls.PlayerMovement.SlowStart.ReadValue<float>() != 0;
    }

    private void Megaboost() {
        playerController.Megaboost();
    }

    public Vector2 GetPlayerMovement() {
        return controls.PlayerMovement.PlayerMove.ReadValue<Vector2>().normalized;
    }

    public bool DirectCameraControl() {
        return controls.PlayerMovement.DirectCameraControl.ReadValue<float>() != 0;
    }

    public Vector2 DirectCameraControlDelta() {
        return controls.PlayerMovement.DirectCameraControlDelta.ReadValue<Vector2>();
    }
}
