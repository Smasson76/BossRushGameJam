using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInput : MonoBehaviour {
    private PlayerController playerController;
    private Control controls;

    void Awake() {
        controls = new Control();
        controls.PlayerMovement.GrappleStart.performed += ctx => playerController.GrappleStart();
        controls.PlayerMovement.GrappleEnd.performed += ctx => playerController.GrappleEnd();
        controls.PlayerMovement.Boost.performed += ctx => playerController.BoostStarted();
    }
    
    void Start() {
        playerController = this.GetComponent<PlayerController>();
    }
    void OnEnable() {
        controls.Enable();
        Cursor.lockState = CursorLockMode.Confined;
    }

    void OnDisable() {
        controls.Disable();
        Cursor.lockState = CursorLockMode.None;
    }
    public Vector2 GetPointerPos() {
        return controls.PlayerMovement.CameraControl.ReadValue<Vector2>();
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
