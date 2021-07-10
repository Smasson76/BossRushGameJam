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
    private float boostPercent = 100f;

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

    void PlayerSlow() {
        if (isBraking) {
            Vector3 force = -playerRb.velocity * slowForce;
            playerRb.AddForce(force, ForceMode.Force);
            playerRb.AddTorque(-playerRb.angularVelocity * rotationSlowAmount);
        }
        if (isOnGround()) {
            SlowEnd();
        }
    }

    public void SlowStart() {
        if (!isOnGround()) {
            isBraking = true;
            playerAnimator.isBraking = true;
            playerAudio.SlowStart();
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
        if (Physics.Raycast(player.transform.position, Vector3.down, out hit, 2f)) {
            return true;
        }
        return false;
    }

    public void HitSomething(Collision collision) {
        float hitSpeed = Vector3.Dot(collision.relativeVelocity, collision.contacts[0].normal);
        playerAudio.HitSomething(collision.contacts[0].point, hitSpeed);
        if (hitSpeed > minForceToExplode) {
            if (isGrappling) {
                GrappleEnd();
            }
            playerAnimator.Pop();
            playerInput.PlayerDeath();
            //Plays the impact sound effect here
            //Stops movement audio source when dead here
            //AudioManager.instance.DeathSound();
            GameManager.instance.PlayerDead();
        }
    }

    public void DisablePlayer() {
        Transform sphere = this.transform.Find("Sphere");
        sphere.SetParent(null);
        Destroy(sphere.GetComponent<CollisionDetection>());
        Destroy(this.gameObject);
    }

    public Vector3 GetPlayerVelocity() {
        return playerRb.velocity;
    }

    public Vector3 GetBoostDirection() {
        return goalBoostDirection;
    }
}
