using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject player;

    [Header("Movement")]
    [SerializeField] private float acceleration;
    [SerializeField] private float boostAmount;
    [SerializeField] private float slowAmount;
    [SerializeField] private float rotationSlowAmount;
    [SerializeField] private float minForceToExplode;

    private PlayerAnimator playerAnimator;
    private Vector3 goalBoostDirection = Vector3.up;
    private Rigidbody playerRb;
    private PlayerInput playerInput;
    private CameraController cameraController;
    private Vector3 grapplePoint;
    private SpringJoint grappleSpring; 
    private bool isGrappling;

    void Start() {
        playerInput = this.GetComponent<PlayerInput>();
        cameraController = this.GetComponent<CameraController>();
        playerAnimator = this.GetComponent<PlayerAnimator>();

        playerRb = player.GetComponent<Rigidbody>();
        GameObject lookTargetGameObject = new GameObject("Look Target");
    }
    
    // FixedUpdate for physics changes
    void FixedUpdate() {
        PlayerMove();
        PlayerBoost();
        PlayerSlow();
        if (isGrappling) {
            UpdateRope();
        }
    }

    void PlayerMove() {
        if (isOnGround()) {
            Vector2 val = playerInput.GetPlayerMovement();
            AudioManager.instance.PlayerMovementSoundEffect(); //Plays the movement sound effect
            if (val.magnitude != 0) {
                Vector3 forceDirection = (cameraController.GetCameraHorizontalFacing() * new Vector3(val.x, 0, val.y)).normalized;
                playerRb.AddForce(forceDirection * acceleration, ForceMode.Force);
                goalBoostDirection = forceDirection; 
            }
        }
        else {
            AudioManager.instance.audioSourceMovement.Stop(); //Stops the sound when player is not grounded
        }
    }

    //YOU CAN DELETE THIS METHOD WHEN YOU SEE THIS
    public void BoostStarted() {
        //AudioManager.instance.PlayBoosterSoundEffect(); //Plays the booster sound effect
    }
    
    void PlayerBoost() {
        if (playerInput.IsBoosting() && GameManager.instance.boostAmount > 0) {
            playerAnimator.isBoosting = true;
            AudioManager.instance.PlayBoosterSoundEffect(); //Plays the booster sound effect
            GameManager.instance.boostAmount -= GameManager.instance.boostDescreaseAmount; //Decreasing boost slider
            GameManager.instance.isBoosting = true; //Setting gamemanagers isBoosting to true
            Vector2 dirInput = playerInput.GetPlayerMovement();
            if (dirInput.magnitude == 0) {
                Vector3 forceDirection = playerRb.velocity.normalized;
                playerRb.AddForce(forceDirection * boostAmount, ForceMode.Force);
                goalBoostDirection = forceDirection;
            } else {
                Vector3 forceDirection = (cameraController.GetCameraHorizontalFacing() * new Vector3(dirInput.x, 0, dirInput.y)).normalized;
                playerRb.AddForce(forceDirection * boostAmount, ForceMode.Force);
                goalBoostDirection = forceDirection;
            }
        } else {
            playerAnimator.isBoosting = false;
            AudioManager.instance.audioSourceBooster.Stop(); //Stops the sound when player is no longer boosting
            GameManager.instance.isBoosting = false; //Setting gamemanagers isBoosting to false
        }
    }

    void PlayerSlow() {
        if (playerInput.IsSlowing() && !isOnGround()) {
            playerAnimator.isBraking = true;
            Vector3 force = -playerRb.velocity * slowAmount;
            playerRb.AddForce(force, ForceMode.Force);
            playerRb.AddTorque(-playerRb.angularVelocity * rotationSlowAmount);
        } else {
            playerAnimator.isBraking = false;
        }
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

            AudioManager.instance.PlayGrappleSoundEffect(); //Plays the grapple fire sound effect
        }   
    }

    public void GrappleEnd() {
        isGrappling = false;
        Destroy(grappleSpring);
        playerAnimator.GrappleEnd();
        AudioManager.instance.PlayReelReturnSoundEffect(); //Plays the reel return sound effect
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
        if (hitSpeed > minForceToExplode) {
            Debug.Log("Exploded");
            if (isGrappling) {
                GrappleEnd();
            }
            playerAnimator.Pop();
            playerInput.PlayerDeath();
            AudioManager.instance.PlayImpactSoundEffect(); //Plays the impact sound effect
            AudioManager.instance.audioSourceMovement.Stop(); //Stops movement audio source when dead
        }
    }

    public Vector3 GetPlayerVelocity() {
        return playerRb.velocity;
    }
    public Vector3 GetBoostDirection() {
        return goalBoostDirection;
    }
}
