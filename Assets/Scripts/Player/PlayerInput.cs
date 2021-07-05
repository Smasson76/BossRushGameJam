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
        controls.PlayerMovement.GrappleStart.performed += ctx => playerController.GrappleStart();
        controls.PlayerMovement.GrappleEnd.performed += ctx => playerController.GrappleEnd();
        controls.PlayerMovement.Boost.performed += ctx => playerController.BoostStarted();
        controls.MetaControls.Restart.performed += ctx => reloadScene();
        controls.MainMenuControls.ChangeDifficulty.performed += ctx => MainMenuManager.instance.ChargeStation();
    }
    
    void Start() {
        playerController = this.GetComponent<PlayerController>();
        Scene currentScene = SceneManager.GetActiveScene();
        string sceneName = currentScene.name;

        if (sceneName == "MainMenu") {
            controls.PlayerMovement.Disable();
            controls.MainMenuControls.Enable();
        }
        if (sceneName == "PlayerTestScene") {
            controls.PlayerMovement.Enable();
            controls.MainMenuControls.Disable();
        }
        Debug.Log(sceneName);
    }
    void OnEnable() {
        controls.Enable();
        Cursor.lockState = CursorLockMode.Confined;
    }

    void OnDisable() {
        controls.Disable();
        Cursor.lockState = CursorLockMode.None;
    }

    void reloadScene() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
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
