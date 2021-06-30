using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInput : MonoBehaviour {

     [Header("References")]
    [SerializeField] private GameObject player;
    [SerializeField] private Transform playerCamera;

     [Header("Camera")]
    [SerializeField] private float lookTargetHeight;
    [SerializeField] private float cameraDistance;
    [SerializeField] private float cameraMinTilt;
    [SerializeField] private float cameraMaxTilt;
    [SerializeField] private float cameraMaxSpeed;
    [SerializeField] private float cameraDeadZone;
    [SerializeField] private float cameraGradientSize;

    [Header("Movement")]
    [SerializeField] private float acceleration;

    private Control controls;
    private Vector2 cameraPos = Vector2.zero;
    private Rigidbody playerRb;
    private Transform lookTarget;
    private Vector3 grapplePoint;
    private SpringJoint grappleSpring; 
    private LineRenderer rope;
    private bool isGrappling = false;

    void Awake() {
        controls = new Control();
        controls.PlayerMovement.GrappleStart.performed += ctx => GrappleStart(ctx);
        controls.PlayerMovement.GrappleEnd.performed += ctx => GrappleEnd(ctx);
    }
    void OnEnable() {
        controls.Enable();
        Cursor.lockState = CursorLockMode.Confined;
    }
    void OnDisable() {
        controls.Disable();
        Cursor.lockState = CursorLockMode.None;
    }
    void Start() {
        playerRb = player.GetComponent<Rigidbody>();
        GameObject lookTargetGameObject = new GameObject("Look Target");
        lookTarget = lookTargetGameObject.transform;
        rope = this.gameObject.GetComponent<LineRenderer>();
        rope.enabled = false;
        PositionLookTarget();
        PositionCamera();
    }
    void Update() {
        PositionLookTarget();
        CameraMove();
        if (isGrappling) {
            UpdateRope();
        }
        PlayerMove();
    }
    void PositionLookTarget() {
        lookTarget.position = player.transform.position + Vector3.up * lookTargetHeight;
    }

    void CameraMove() {
        Vector2 val = controls.PlayerMovement.CameraControl.ReadValue<Vector2>();
        // Camera movement horizontally
        val.x -= Screen.width/2;
        float maxSpeedLocation = cameraDeadZone + cameraGradientSize;
        if (val.x > 0) {val.x = Mathf.Clamp(UtilityFunctions.Remap(val.x, cameraDeadZone, maxSpeedLocation, 0, cameraMaxSpeed), 0, cameraMaxSpeed);}
        if (val.x < 0) {val.x = Mathf.Clamp(UtilityFunctions.Remap(val.x, -maxSpeedLocation, -cameraDeadZone, 0, cameraMaxSpeed), -cameraMaxSpeed, 0);}
        cameraPos.x += val.x;

        // Camera movement vertically
        cameraPos.y = UtilityFunctions.Remap(val.y, Screen.height, 0, cameraMinTilt, cameraMaxTilt);
        cameraPos.y = Mathf.Clamp(cameraPos.y, -cameraMaxTilt, cameraMaxTilt);
        PositionCamera();
    }

    void PositionCamera() {
        Vector3 offsetVector = Quaternion.Euler(cameraPos.y, cameraPos.x, 0) * new Vector3(0, 0, -cameraDistance);
        playerCamera.position = player.transform.position + offsetVector;
        playerCamera.LookAt(lookTarget);
    }

    void PlayerMove() {
        Vector2 val = controls.PlayerMovement.PlayerMove.ReadValue<Vector2>();
        Vector3 forceDirection = (Quaternion.Euler(0, cameraPos.x, 0) * new Vector3(val.x, 0, val.y)).normalized;
        playerRb.AddForce(forceDirection * acceleration, ForceMode.Force);
    }

    void GrappleStart(InputAction.CallbackContext ctx) {
        Debug.Log("Grapple started");
        RaycastHit hit;
        Vector2 mousePos = controls.PlayerMovement.CameraControl.ReadValue<Vector2>();
        Ray ray = Camera.main.ScreenPointToRay(mousePos);
        if (Physics.Raycast(ray, out hit)) {
            Debug.Log("hit something");
            isGrappling = true;
            
            grapplePoint = hit.point;
            grappleSpring = player.AddComponent<SpringJoint>();
            grappleSpring.autoConfigureConnectedAnchor = false;
            grappleSpring.connectedAnchor = grapplePoint;
            float grappleDist = Vector3.Distance(player.transform.position, grapplePoint);
            grappleSpring.maxDistance = grappleDist * 0.8f;
            grappleSpring.minDistance = grappleDist * 0.3f;
            grappleSpring.spring = 5;
            grappleSpring.damper = 5f;

            rope.enabled = true;
        }   
    }

    void GrappleEnd(InputAction.CallbackContext ctx) {
        Debug.Log("Grapple ended");
        isGrappling = false;
        Destroy(grappleSpring);
        rope.enabled = false;
    }

    void UpdateRope() {
        rope.positionCount = 2;
        rope.SetPosition(0, player.transform.position);
        rope.SetPosition(1, grapplePoint);
    }
}
