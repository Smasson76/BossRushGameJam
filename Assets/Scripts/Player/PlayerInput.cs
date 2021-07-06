using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerInput : MonoBehaviour {
    public PlayerController playerController;
    public Control controls;

    void Awake() {
        controls = new Control();
    }
    
    void Start() {
        if (GameManager.instance.currentScene.name == "MainMenu") {
            controls.PlayerMovement.Disable();
            controls.MainMenuControls.Enable();
            controls.MainMenuControls.ChangeDifficulty.performed += ctx => MainMenuManager.instance.ChargeStation();
        } else if (GameManager.instance.currentScene.name == "PlayerTestScene") {
            controls.PlayerMovement.Enable();
            controls.MainMenuControls.Disable();
            playerController = GameObject.Find("Player(Clone)").GetComponent<PlayerController>();
            controls.PlayerMovement.GrappleStart.performed += ctx => playerController.GrappleStart();
            controls.PlayerMovement.GrappleEnd.performed += ctx => playerController.GrappleEnd();
        }
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
