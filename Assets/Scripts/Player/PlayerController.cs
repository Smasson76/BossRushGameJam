using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {
    
    [Header("References")]
    [SerializeField] private GameObject player;

    [Header("Movement")]
    [SerializeField] private float acceleration;
    [SerializeField] private float boostForce;
    [SerializeField] private float megaboostForce;
    [SerializeField] private float slowForce;
    [SerializeField] private float rotationSlowAmount;
    [SerializeField] private float minForceToExplode;

    [Header("Boost Recharge Variables")]
    public float boostDescreaseAmount = 0.5f;
    public float megaboostDescreaseAmount = 20f;
    public float boostIncreaseAmount = 0.1f;
    public bool canRegenerateBoost = false;

    [Header("Walking Movement")]
    [SerializeField] private float groundedHeight;
    [SerializeField] private float targetHeight;
    [SerializeField] private float springStrength;
    [SerializeField] private float springDamper;
    [SerializeField] private float rotationStrength;
    [SerializeField] private float rotationDamper;
    [SerializeField] private float velocitySlow;

    [System.NonSerialized] public PlayerInput playerInput;
    private PlayerAnimator playerAnimator;
    private PlayerAudio playerAudio;
    private Vector3 goalBoostDirection = Vector3.up;
    private Rigidbody playerRb;
    private CameraController cameraController;
    private Vector3 grapplePoint;
    private SpringJoint grappleSpring; 
    private bool isGrappling;
    private bool isBoosting;
    private bool isBraking;
    private bool isMoving;
    private bool isRolling;
    private bool isDead;
    private float boostPercent = 100f;
    private Vector3 walkingCurrentUp;

    void Start() {
        cameraController = this.GetComponent<CameraController>();
        playerAnimator = this.GetComponent<PlayerAnimator>();
        playerAudio = this.GetComponent<PlayerAudio>();
        playerRb = player.GetComponent<Rigidbody>();
        GameObject lookTargetGameObject = new GameObject("Look Target");
        playerAudio.InitInstances(playerRb);
    }
    
    // FixedUpdate for physics changes
    void FixedUpdate() {
        PlayerMove();
        PlayerBoost();
        PlayerSlow();
        if (isGrappling) {
            UpdateRope();
        }
        if (isOnGround() && playerRb.velocity.magnitude > 1f) {
            if (!isRolling) {
                isRolling = true;
                playerAudio.RollingStart();
            }
        } else if (isRolling) {
            isRolling = false;
            playerAudio.RollingStop();
        }
    }

    void PlayerMove() {
        Vector2 val = playerInput.GetPlayerMovement();
        if (isOnGround() && val.magnitude != 0) {
            if (!isMoving) {
                isMoving = true;
                playerAudio.MovementInputStart();
            }
            Vector3 forceDirection = (cameraController.GetCameraHorizontalFacing() * new Vector3(val.x, 0, val.y)).normalized;
            playerRb.AddForce(forceDirection * acceleration, ForceMode.Force);
            goalBoostDirection = forceDirection;
        } else if (isMoving) {
            isMoving = false;
            playerAudio.MovementInputStop();
        }
        
    }

    void PlayerBoost() {
        if (isBoosting) {
            goalBoostDirection = GetCurrentBoostDir();
            playerRb.AddForce(goalBoostDirection * boostForce, ForceMode.Force);
        }

        if (boostPercent < 100 && canRegenerateBoost) {
            boostPercent += boostIncreaseAmount;
        }
    }
    
    public void BoostStart() {
        isBoosting = true;
        playerAnimator.isBoosting = true;
        playerAudio.BoostStart();
    }
    
    public void BoostEnd() {
        isBoosting = false;
        playerAnimator.isBoosting = false;
        playerAudio.BoostEnd();
    }
    public void Megaboost () {
        if (boostPercent > megaboostDescreaseAmount) {
            goalBoostDirection = GetCurrentBoostDir();
            boostPercent -= megaboostDescreaseAmount;
            playerRb.AddForce(goalBoostDirection * megaboostForce, ForceMode.Force);
        }
    }

    private Vector3 GetCurrentBoostDir() {
        Vector2 dirInput = playerInput.GetPlayerMovement();
        if (dirInput.magnitude == 0) {
            return playerRb.velocity.normalized;
        } else {
            return (cameraController.GetCameraHorizontalFacing() * new Vector3(dirInput.x, 0, dirInput.y)).normalized;
        }
    }

    public float GetBoostRemaining() {
        return boostPercent;
    }

    public void SlowStart() {
        if (!isOnGround()) {
            isBraking = true;
            playerAnimator.isBraking = true;
            playerAudio.SlowStart();
        } else {
            isBraking = true;
            walkingCurrentUp = playerAnimator.walkStarted();
        }
    }

    void PlayerSlow() {
        if (isBraking) {
            if (isOnGround()) {
                playerAnimator.walkStarted();
                playerAnimator.isBraking = false;

                RaycastHit hit;
                int layermask = ~(1 << 2);
                Physics.Raycast(playerRb.position, Vector3.down, out hit, Mathf.Infinity, layermask);
                float height = targetHeight - hit.distance;
                float springForce = (height * springStrength) - (playerRb.velocity.y * springDamper);
                playerRb.AddForce(Vector3.up * springForce);

                Quaternion goal = Quaternion.LookRotation(walkingCurrentUp);
                Quaternion diff = UtilityFunctions.ShortestRotation(goal, playerRb.rotation);
                Vector3 rotAxis;
                float rotDegrees;
                diff.ToAngleAxis(out rotDegrees, out rotAxis);
                rotAxis.Normalize();
                float rotRadians = rotDegrees * Mathf.Deg2Rad;
                playerRb.AddTorque((rotAxis * (rotRadians * rotationStrength)) - (playerRb.angularVelocity * rotationDamper));

                playerRb.velocity = new Vector3(playerRb.velocity.x * velocitySlow, playerRb.velocity.y, playerRb.velocity.z * velocitySlow);
            } else { 
                Vector3 force = -playerRb.velocity * slowForce;
                playerRb.AddForce(force, ForceMode.Force);
                playerRb.AddTorque(-playerRb.angularVelocity * rotationSlowAmount);
            }
        } else {
            playerAnimator.walkEnded();
        }
    }

    public void SlowEnd() {
        isBraking = false;
        playerAnimator.isBraking = false;
        playerAudio.SlowEnd();
    }

    public void GrappleStart() {
        RaycastHit hit;
        Vector2 mousePos = playerInput.GetPointerPos();
        Ray ray = Camera.main.ScreenPointToRay(mousePos);
        if (Physics.Raycast(ray, out hit)) {
            isGrappling = true;
            playerAnimator.walkEnded();
            
            grapplePoint = hit.point;
            grappleSpring = player.AddComponent<SpringJoint>();
            Vector3 grappleSectionHome =  playerAnimator.GrappleStart(grapplePoint);
            grappleSpring.anchor = grappleSectionHome;
            grappleSpring.autoConfigureConnectedAnchor = false;
            grappleSpring.connectedAnchor = grapplePoint;
            float grappleDist = Vector3.Distance(player.transform.position, grapplePoint);
            grappleSpring.maxDistance = grappleDist;
            grappleSpring.minDistance = 0;
            grappleSpring.spring = 5f;
            grappleSpring.damper = 5f;

            playerAudio.GrappleReelOutStart();
        }   
    }

    public void GrappleEnd() {
        if (isGrappling) {
            isGrappling = false;
            Destroy(grappleSpring);
            playerAnimator.GrappleEnd();
            playerAudio.GrappleReelInStart(grapplePoint);
        }
    }

    public void UpdateRope() {
        float grapplelength = Vector3.Distance(player.transform.position, grapplePoint);
        if (grappleSpring.maxDistance > grapplelength) {
            grappleSpring.maxDistance = grapplelength;
        }
    }

    bool isOnGround() {
        RaycastHit hit;
        int layermask = ~(1 << 2);
        if (Physics.Raycast(player.transform.position, Vector3.down, out hit, groundedHeight, layermask)) {
            return true;
        }
        return false;
    }

    public void HitSomething(Collision collision) {
        float hitSpeed = Vector3.Dot(collision.relativeVelocity, collision.contacts[0].normal);
        playerAudio.HitSomething(collision.contacts[0].point, hitSpeed);
        if (hitSpeed > minForceToExplode && !isDead) {
            isDead = true;
            if (isGrappling) {
                GrappleEnd();
            }
            if (isBoosting) {
                BoostEnd();
            }
            if (isBraking) {
                SlowEnd();
            }
            isMoving = false;
            isRolling = false;
            playerAnimator.Pop();
            playerInput.PlayerDeath();
            playerAudio.PlayerDeath();
            GameManager.instance.PlayerDead();
        }
    }

    public void DisablePlayer() {
        playerAnimator.ClearEvidence();
        Destroy(this.gameObject);
    }

    public Vector3 GetPlayerVelocity() {
        return playerRb.velocity;
    }

    public Vector3 GetBoostDirection() {
        return goalBoostDirection;
    }

    public bool IsPlayerDead() {
        return isDead;
    }
}
