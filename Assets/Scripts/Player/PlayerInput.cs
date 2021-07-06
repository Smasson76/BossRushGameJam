﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerInput : MonoBehaviour {
    private PlayerController playerController;
    private Control controls;

    void Awake() {
        controls = new Control();
        controls.MainMenuControls.ChangeDifficulty.performed += ctx => MainMenuManager.instance.ChargeStation();
        controls.PlayerMovement.GrappleStart.performed += ctx => GrappleStart();
        controls.PlayerMovement.GrappleEnd.performed += ctx => GrappleEnd();
        controls.PlayerMovement.Megaboost.performed += ctx => Megaboost();
    }
    
    void Start() {
        if (GameManager.instance.currentScene.name == "MainMenu") {
            controls.PlayerMovement.Disable();
            controls.MainMenuControls.Enable();
        } else if (GameManager.instance.currentScene.name == "PlayerTestScene") {
            controls.PlayerMovement.Enable();
            controls.MainMenuControls.Disable();
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
    }

    public Vector2 GetPointerPos() {
        return controls.CameraControl.CameraControl.ReadValue<Vector2>();
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
}
