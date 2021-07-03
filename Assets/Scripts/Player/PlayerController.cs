using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject player;
    [SerializeField] private Transform booster;
    [SerializeField] private ParticleSystem boosterParticles;
    [SerializeField] private Transform splitSphere;

    [Header("Movement")]
    [SerializeField] private float acceleration;
    [SerializeField] private float boostAmount;
    [SerializeField] private float slowAmount;
    [SerializeField] private float rotationSlowAmount;

    [Header ("Animation")]
    [SerializeField] private float boosterLerpSpeed;
    [SerializeField] private float panelLerpSpeed;
    [SerializeField] private float panelDistMin;
    [SerializeField] private float panelDistMax;
    
    private Rigidbody playerRb;
    private PlayerInput playerInput;
    private CameraController cameraController;
    private Vector3 grapplePoint;
    private SpringJoint grappleSpring; 
    private LineRenderer rope;
    private bool isGrappling = false;
    private Vector3 goalBoostDirection = Vector3.up;
    private bool isBraking;
    private bool isBoosting;
    private SectionData [] sphereSections;
    private SectionData grappleSection;

    private struct SectionData {
        public Transform transform;
        public Vector3 homeLocation;
        public SectionStatus status;
    }

    private enum SectionStatus {
        standby,
        isGrappling
    }

    void Start() {
        playerInput = this.GetComponent<PlayerInput>();
        cameraController = this.GetComponent<CameraController>();

        playerRb = player.GetComponent<Rigidbody>();
        GameObject lookTargetGameObject = new GameObject("Look Target");
        rope = this.gameObject.GetComponent<LineRenderer>();
        rope.enabled = false;

        sphereSections = new SectionData[splitSphere.childCount];
        for(int i = 0; i < splitSphere.childCount; i++) {
            SectionData newSection = new SectionData();
            newSection.transform = splitSphere.GetChild(i);
            newSection.homeLocation = newSection.transform.localPosition.normalized;
            newSection.status = SectionStatus.standby;
            sphereSections[i] = newSection;
        }
    }

    // LateUpdate for visual changes
    void LateUpdate() {
        UpdateBooster();
        UpdateSections();
        if (isGrappling) {
            UpdateRope();
        }
    }
    
    // FixedUpdate for physics changes
    void FixedUpdate() {
        PlayerMove();
        PlayerBoost();
        PlayerSlow();
    }

    void PlayerMove() {
        if (isOnGround()) {
            Vector2 val = playerInput.GetPlayerMovement();
            if (val.magnitude != 0) {
                Vector3 forceDirection = (cameraController.GetCameraHorizontalFacing() * new Vector3(val.x, 0, val.y)).normalized;
                playerRb.AddForce(forceDirection * acceleration, ForceMode.Force);
                goalBoostDirection = forceDirection;
            }
        }
    }

    void PlayerBoost() {
        if (playerInput.IsBoosting()) {
            isBoosting = true;
            boosterParticles.Play();
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
            isBoosting = false;
            boosterParticles.Stop();
        }
    }

    void PlayerSlow() {
        if (playerInput.IsSlowing() && !isOnGround()) {
            isBraking = true;
            Vector3 force = -playerRb.velocity * slowAmount;
            playerRb.AddForce(force, ForceMode.Force);
            playerRb.AddTorque(-playerRb.angularVelocity * rotationSlowAmount);
        } else {
            isBraking = false;
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
            grappleSection = GetClosestSection(grapplePoint - player.transform.position);
            grappleSection.status = SectionStatus.isGrappling;
            grappleSection.transform.gameObject.SetActive(false);
            grappleSpring.anchor = grappleSection.homeLocation;
            grappleSpring.autoConfigureConnectedAnchor = false;
            grappleSpring.connectedAnchor = grapplePoint;
            float grappleDist = Vector3.Distance(player.transform.position, grapplePoint);
            grappleSpring.maxDistance = grappleDist;
            grappleSpring.minDistance = 0;
            grappleSpring.spring = 5f;
            grappleSpring.damper = 5f;

            rope.enabled = true;
        }   
    }

    public void GrappleEnd() {
        isGrappling = false;
        grappleSection.transform.gameObject.SetActive(true);
        grappleSection.status = SectionStatus.standby;
        Destroy(grappleSpring);
        rope.enabled = false;
    }

    SectionData GetClosestSection(Vector3 dir) {
        SectionData closest = sphereSections[0];
        float smallestAngle = 180;
        foreach(SectionData section in sphereSections) {
            float angle = Vector3.Angle(dir, section.transform.parent.rotation * section.homeLocation);
            if (angle < smallestAngle) {
                closest = section;
                smallestAngle = angle;
            }
        }
        return closest;
    }

    void UpdateRope() {
        float grapplelength = Vector3.Distance(player.transform.position, grapplePoint);
        if (grappleSpring.maxDistance > grapplelength) {
            grappleSpring.maxDistance = grapplelength;
        }
        rope.positionCount = 2;
        rope.SetPosition(0, grappleSection.transform.position);
        rope.SetPosition(1, grapplePoint);
    }

    bool isOnGround() {
        RaycastHit hit;
        if (Physics.Raycast(player.transform.position, Vector3.down, out hit, 2f)) {
            return true;
        }
        return false;
    }

    void UpdateSections() {
        if (isBraking) {
            foreach(SectionData section in sphereSections) {
                float angle = Vector3.Angle(playerRb.velocity, section.transform.parent.rotation * section.homeLocation);
                if (angle > 45 && angle < 100) {
                    Vector3 goalPos = section.homeLocation * panelDistMax;
                    section.transform.localPosition = Vector3.Lerp(section.transform.localPosition, goalPos, Time.deltaTime * panelLerpSpeed);
                    Quaternion goalRot = Quaternion.FromToRotation(section.homeLocation, playerRb.velocity);
                    section.transform.rotation = Quaternion.Lerp(section.transform.rotation, goalRot, Time.deltaTime * panelLerpSpeed);
                } else {
                    Vector3 goalPos = section.homeLocation * panelDistMin;
                    section.transform.localPosition = Vector3.Lerp(section.transform.localPosition, goalPos, Time.deltaTime * panelLerpSpeed);
                    Quaternion goalRot = Quaternion.LookRotation(Vector3.forward, Vector3.up);
                    section.transform.localRotation = Quaternion.Lerp(section.transform.localRotation, goalRot, Time.deltaTime * panelLerpSpeed);
                }
            }
        } else if (isBoosting) {
            foreach(SectionData section in sphereSections) {
                Vector3 boostDir = goalBoostDirection;
                Vector3 localPosNoRot = section.transform.parent.rotation * section.homeLocation;
                float angle = Vector3.Angle(boostDir, localPosNoRot);
                if (angle > 100) {
                    Vector3 boostDirVectorComponent = boostDir.normalized * Vector3.Dot(localPosNoRot, boostDir);
                    Vector3 perpToBoost = (localPosNoRot - boostDirVectorComponent);

                    Vector3 goalPos = section.homeLocation * panelDistMax;
                    section.transform.localPosition = Vector3.Lerp(section.transform.localPosition, goalPos, Time.deltaTime * panelLerpSpeed);

                    Quaternion goalRot = Quaternion.FromToRotation(section.homeLocation, perpToBoost);
                    section.transform.rotation = Quaternion.Lerp(section.transform.rotation, goalRot, Time.deltaTime * panelLerpSpeed);
                } else {
                    Vector3 goalPos = section.homeLocation * panelDistMin;
                    section.transform.localPosition = Vector3.Lerp(section.transform.localPosition, goalPos, Time.deltaTime * panelLerpSpeed);
                    Quaternion goalRot = Quaternion.LookRotation(Vector3.forward, Vector3.up);
                    section.transform.localRotation = Quaternion.Lerp(section.transform.localRotation, goalRot, Time.deltaTime * panelLerpSpeed);
                }
            }
        } else {
            foreach(SectionData section in sphereSections) {
                Vector3 goalPos = section.homeLocation * panelDistMin;
                section.transform.localPosition = Vector3.Lerp(section.transform.localPosition, goalPos, Time.deltaTime * panelLerpSpeed);
                Quaternion goalRot = Quaternion.LookRotation(Vector3.forward, Vector3.up);
                section.transform.localRotation = Quaternion.Lerp(section.transform.localRotation, goalRot, Time.deltaTime * panelLerpSpeed);
            }
        }
    }

    void UpdateBooster() {
        Quaternion goalDir = Quaternion.LookRotation(goalBoostDirection, Vector3.up);
        booster.transform.rotation = Quaternion.Lerp(booster.transform.rotation, goalDir, Time.deltaTime * boosterLerpSpeed);
    }

    public void Pop() {
        foreach (SectionData section in sphereSections) {
            section.transform.SetParent(null, true);
            BoxCollider collider = section.transform.gameObject.AddComponent<BoxCollider>();
            collider.size = section.transform.gameObject.GetComponent<MeshFilter>().mesh.bounds.extents;
            Rigidbody rb = section.transform.gameObject.AddComponent<Rigidbody>();
            rb.interpolation = RigidbodyInterpolation.Interpolate;
            rb.collisionDetectionMode = CollisionDetectionMode.Continuous;
            rb.velocity = playerRb.velocity;
            rb.AddForce(section.homeLocation.normalized * 5);
        }
        sphereSections = new SectionData[0];
    }
}
