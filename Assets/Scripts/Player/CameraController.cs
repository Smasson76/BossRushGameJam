using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {
    [Header("References")]
    [SerializeField] private GameObject player;
    [SerializeField] private Transform playerCamera;

    [Header("Camera")]
    [SerializeField] private float cameraLerpSpeed;
    [SerializeField] private float lookTargetHeightMin;
    [SerializeField] private float lookTargetHeightMax;
    [SerializeField] private float cameraMinDistance;
    [SerializeField] private float cameraMaxDistance;
    [SerializeField] private float minFOV;
    [SerializeField] private float maxFOV;
    [SerializeField] private float cameraDistanceVelocityLimit;
    [SerializeField] private float cameraMinTilt;
    [SerializeField] private float cameraMaxTilt;
    [SerializeField] private float cameraMaxSpeed;
    [SerializeField] private float cameraDeadZone;
    [SerializeField] private float cameraGradientSize;
    [SerializeField] private float cameraDirectControlSensitivity;

    private Transform lookTarget;
    private float lookTargetHeight;
    private float cameraDistance;
    private Rigidbody playerRb;
    private Vector2 cameraPos = Vector2.zero;
    public PlayerInput playerInput;
    public GameObject gameManager;

    void Start() {
        playerInput = this.GetComponent<PlayerInput>();
        gameManager = GameObject.Find("GameManager");
        playerInput = gameManager.GetComponent<PlayerInput>();
        playerRb = player.GetComponent<Rigidbody>();
        GameObject lookTargetGameObject = new GameObject("Look Target");
        lookTarget = lookTargetGameObject.transform;
        PositionLookTarget();
        PositionCamera();
    }

    void LateUpdate() {
        PositionLookTarget();
        if (playerInput.DirectCameraControl()) {
            CameraMoveDirect();
        } else {
            CameraMoveStandard();
        }
        PositionCamera();
        float targetFOV = Mathf.Clamp(UtilityFunctions.Remap(playerRb.velocity.magnitude, 0, cameraDistanceVelocityLimit, minFOV, maxFOV), minFOV, maxFOV);
        Camera.main.fieldOfView = Mathf.Lerp(Camera.main.fieldOfView, targetFOV, Time.deltaTime * cameraLerpSpeed);
    }

    void PositionLookTarget() {
        float goallookTargetHeight = Mathf.Clamp(UtilityFunctions.Remap(playerRb.velocity.magnitude, 0, cameraDistanceVelocityLimit, lookTargetHeightMin, lookTargetHeightMax), lookTargetHeightMin, lookTargetHeightMax);
        lookTargetHeight =  Mathf.Lerp(lookTargetHeight, goallookTargetHeight, Time.deltaTime * cameraLerpSpeed);
        lookTarget.position = player.transform.position + Vector3.up * lookTargetHeight;
    }

    void CameraMoveStandard() {
        Vector2 val = playerInput.GetPointerPos();
        // Camera movement horizontally
        val.x -= Screen.width/2;
        float maxSpeedLocation = cameraDeadZone + cameraGradientSize;
        if (val.x > 0) {val.x = Mathf.Clamp(UtilityFunctions.Remap(val.x, cameraDeadZone, maxSpeedLocation, 0, cameraMaxSpeed), 0, cameraMaxSpeed);}
        if (val.x < 0) {val.x = Mathf.Clamp(UtilityFunctions.Remap(val.x, -maxSpeedLocation, -cameraDeadZone, 0, cameraMaxSpeed), -cameraMaxSpeed, 0);}
        cameraPos.x += val.x * Time.deltaTime;

        // Camera movement vertically
        cameraPos.y = Mathf.Lerp(cameraPos.y, UtilityFunctions.Remap(val.y, Screen.height, 0, cameraMinTilt, cameraMaxTilt), Time.deltaTime * cameraLerpSpeed);
        cameraPos.y = Mathf.Clamp(cameraPos.y, -cameraMaxTilt, cameraMaxTilt);
    }

    void CameraMoveDirect() {
        Vector2 val = playerInput.DirectCameraControlDelta();
        cameraPos.x += val.x * cameraDirectControlSensitivity;
        cameraPos.y = Mathf.Clamp(cameraPos.y - val.y * cameraDirectControlSensitivity, cameraMinTilt, cameraMaxTilt);
    }

    void PositionCamera() {
        float goalCameraDistance = Mathf.Clamp(UtilityFunctions.Remap(playerRb.velocity.magnitude, 0, cameraDistanceVelocityLimit, cameraMinDistance, cameraMaxDistance), cameraMinDistance, cameraMaxDistance);
        cameraDistance = Mathf.Lerp(cameraDistance, goalCameraDistance, Time.deltaTime * cameraLerpSpeed);
        Vector3 offsetVector = Quaternion.Euler(cameraPos.y, cameraPos.x, 0) * new Vector3(0, 0, -cameraDistance);
        playerCamera.position = player.transform.position + offsetVector;
        playerCamera.LookAt(lookTarget);
    }

    public Quaternion GetCameraHorizontalFacing() {
        return Quaternion.Euler(0, cameraPos.x, 0);
    }
}
