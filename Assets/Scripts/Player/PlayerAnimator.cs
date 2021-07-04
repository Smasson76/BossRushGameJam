using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DitzelGames.FastIK;

public class PlayerAnimator : MonoBehaviour {
    [Header("References")]
    [SerializeField] private Transform player;
    [SerializeField] private Transform booster;
    [SerializeField] private ParticleSystem boosterParticles;
    [SerializeField] private Transform sectionParent;
    [SerializeField] private Transform[] sectionBones;
    [SerializeField] private GameObject[] ikEnds;
    [SerializeField] private Material ropeMaterial;

    [Header ("Animation")]
    [SerializeField] private float boosterLerpSpeed;
    [SerializeField] private float panelLerpSpeed;
    [SerializeField] private float grappleLerpSpeed;
    [SerializeField] private float panelDistMin;
    [SerializeField] private float panelDistMax;

    [Header ("Rope Spring Properties")]
    [SerializeField] int quality;
    [SerializeField] float damper;
    [SerializeField] float strength;
    [SerializeField] float waveCount;
    [SerializeField] float waveHeight;
    [SerializeField] AnimationCurve affectCurve;

    [System.NonSerialized] public bool isGrappling = false;
    [System.NonSerialized] public bool isBraking;
    [System.NonSerialized] public bool isBoosting;
    private PlayerController playerController;
    private SectionData [] sphereSections;
    private Vector3 grapplePoint;

    private struct SectionData {
        public Transform transform;
        public GameObject ikEnd;
        public Transform ikPole;
        public Vector3 homeLocation;
        public Quaternion homeRotation;
        public SectionStatus status;
        public LineRenderer rope;
        public float ropeSpringPos;
        public float ropeSpringVelocity;
    }

    private enum SectionStatus {
        standby,
        grappleLeaving,
        grappled,
        grappleReturning,
    }

    void Start() {
        playerController = this.gameObject.GetComponent<PlayerController>();
        sphereSections = new SectionData[sectionBones.Length];
        Transform poleContainer = new GameObject("IKPole Container").transform;
        poleContainer.SetParent(this.transform);
        for(int i = 0; i < sectionBones.Length; i++) {
            SectionData newSection = new SectionData();
            newSection.transform = sectionBones[i];
            newSection.homeLocation = newSection.transform.localPosition.normalized;
            newSection.homeRotation = newSection.transform.localRotation;

            newSection.ikEnd = ikEnds[i];
            Transform ikPole = new GameObject("IK Pole").transform;
            ikPole.SetParent(poleContainer);
            ikPole.localPosition = newSection.homeLocation * 2;
            newSection.ikPole = ikPole;
            FastIKFabric ikComponent = newSection.ikEnd.AddComponent<FastIKFabric>();
            ikComponent.ChainLength = 4;
            ikComponent.Target = newSection.transform;
            ikComponent.Pole = ikPole;

            newSection.status = SectionStatus.standby;
            sphereSections[i] = newSection;
        }
    }

    // LateUpdate for visual changes
    void LateUpdate() {
        UpdateBooster();
        UpdateSections();
    }

    void UpdateSections() {
        for (int i =0; i < sphereSections.Length; i++) {
            SectionData section = sphereSections[i];
            switch (section.status) {
            case SectionStatus.standby:
                if (isBraking) {
                    Vector3 playerVelocity = playerController.GetPlayerVelocity();
                    float angle = Vector3.Angle(playerVelocity, section.transform.parent.rotation * section.homeLocation);
                    if (angle < 100) {
                        Vector3 goalPos = section.homeLocation * panelDistMax;
                        section.transform.localPosition = Vector3.Lerp(section.transform.localPosition, goalPos, Time.deltaTime * panelLerpSpeed);
                        Quaternion goalRot = Quaternion.LookRotation(playerVelocity, section.transform.localPosition);
                        section.transform.rotation = Quaternion.Lerp(section.transform.rotation, goalRot, Time.deltaTime * panelLerpSpeed);
                    } else {
                        LerpToHome(section);
                    }
                } else if (isBoosting) {
                    Vector3 boostDir = playerController.GetBoostDirection();
                    Vector3 localPosNoRot = section.transform.parent.rotation * section.homeLocation;
                    float angle = Vector3.Angle(boostDir, localPosNoRot);
                    if (angle > 110) {
                        Vector3 boostDirVectorComponent = boostDir.normalized * Vector3.Dot(localPosNoRot, boostDir);
                        Vector3 perpToBoost = (localPosNoRot - boostDirVectorComponent);
                        Debug.DrawLine(section.transform.position, section.transform.position + boostDirVectorComponent, Color.blue);
                        Debug.DrawLine(section.transform.position, section.transform.position + perpToBoost, Color.red);

                        Vector3 goalPos = section.homeLocation * UtilityFunctions.Remap(angle, 110, 180, panelDistMin, panelDistMax);
                        section.transform.localPosition = Vector3.Lerp(section.transform.localPosition, goalPos, Time.deltaTime * panelLerpSpeed);

                        Quaternion goalRot = Quaternion.LookRotation(perpToBoost, localPosNoRot);
                        section.transform.rotation = Quaternion.Lerp(section.transform.rotation, goalRot, Time.deltaTime * panelLerpSpeed);
                    } else {
                        LerpToHome(section);
                    }
                } else {
                    LerpToHome(section);
                }
                break;
            case SectionStatus.grappleLeaving:
                section.transform.position += GetDiffToTarget(section.transform.position, grapplePoint);
                if (isGrappling == false) {
                    sphereSections[i].status = SectionStatus.grappleReturning;
                } else if (Vector3.Distance(section.transform.position, grapplePoint) < 1f) {
                    sphereSections[i].status = SectionStatus.grappled;
                }
                UpdateRope(i);
                break;
            case SectionStatus.grappled:
                if (isGrappling == false) {
                    sphereSections[i].status = SectionStatus.grappleReturning;
                }
                UpdateRope(i);
                break;
            case SectionStatus.grappleReturning:
                Vector3 goal = player.position + player.transform.rotation * section.homeLocation;
                section.transform.position += GetDiffToTarget(section.transform.position, goal);
                UpdateRope(i);
                if (Vector3.Distance(section.transform.position, goal) < 1f) {
                    section.transform.SetParent(sectionParent, true);
                    sphereSections[i].status = SectionStatus.standby;
                    Destroy(section.rope);
                }
                break;
            default:
                throw new System.Exception("Section status not caught");
            }
        }
    }

    Vector3 GetDiffToTarget(Vector3 currentPos, Vector3 targetPos) {
        float moveDist = Mathf.Min(grappleLerpSpeed, Vector3.Distance(targetPos, currentPos));
        return (targetPos - currentPos).normalized * moveDist;
    }

    void UpdateRope(int sectionIndex) {
        float pos = sphereSections[sectionIndex].ropeSpringPos;
        float velocity = sphereSections[sectionIndex].ropeSpringVelocity;
        velocity += (-pos * strength - velocity * damper) * Time.deltaTime;
        pos += velocity * Time.deltaTime;
        sphereSections[sectionIndex].ropeSpringPos = pos;
        sphereSections[sectionIndex].ropeSpringVelocity = velocity;
        SectionData section = sphereSections[sectionIndex];
        for (var i = 0; i < quality + 1; i++) {
            var delta = i / (float) quality;
            var offset = Vector3.up * waveHeight * Mathf.Sin(delta * waveCount * Mathf.PI) * pos * affectCurve.Evaluate(delta);
            section.rope.SetPosition(i, Vector3.Lerp(section.ikEnd.transform.position, section.transform.position, delta) + offset);
        }
    }

    void LerpToHome (SectionData section) {
        Vector3 goalPos = section.homeLocation * panelDistMin;
        section.transform.localPosition = Vector3.Lerp(section.transform.localPosition, goalPos, Time.deltaTime * panelLerpSpeed);
        Quaternion goalRot = section.homeRotation * Quaternion.LookRotation(Vector3.forward, Vector3.up);
        section.transform.localRotation = Quaternion.Lerp(section.transform.localRotation, goalRot, Time.deltaTime * panelLerpSpeed);
    }

    void UpdateBooster() {
        Quaternion goalDir = Quaternion.LookRotation(playerController.GetBoostDirection(), Vector3.up);
        booster.transform.rotation = Quaternion.Lerp(booster.transform.rotation, goalDir, Time.deltaTime * boosterLerpSpeed);
        if (isBoosting) {
            boosterParticles.Play();
        } else {
            boosterParticles.Stop();
        }
    }

    public Vector3 GrappleStart(Vector3 grappleHit) {
        grapplePoint = grappleHit;
        isGrappling = true;
        int grappleSectionIndex = GetClosestSection(grapplePoint);
        sphereSections[grappleSectionIndex].status = SectionStatus.grappleLeaving;
        Transform sectionTransform = sphereSections[grappleSectionIndex].transform;
        sectionTransform.SetParent(null);
        LineRenderer rope = sectionTransform.gameObject.AddComponent<LineRenderer>();
        rope.startWidth = 0.1f;
        rope.material = ropeMaterial;
        rope.positionCount = quality + 1;
        sphereSections[grappleSectionIndex].rope = rope;
        sphereSections[grappleSectionIndex].ropeSpringPos = 1;
        sphereSections[grappleSectionIndex].ropeSpringVelocity = 0;
        return sphereSections[grappleSectionIndex].homeLocation;
    }

    public void GrappleEnd() {
        isGrappling = false;
    }

    int GetClosestSection(Vector3 pos) {
        int closest = 0;
        float smallestDistance = 180;
        for (int i = 0; i < sphereSections.Length; i++) {
            float dist = Vector3.Distance(pos, sphereSections[i].transform.position);
            if (dist < smallestDistance && sphereSections[i].status == SectionStatus.standby) {
                closest = i;
                smallestDistance = dist;
            }
        }
        return closest;
    }

    /*
    public void Pop() {
        foreach (SectionData section in sphereSections) {
            section.transform.SetParent(null, true);
            BoxCollider collider = section.transform.gameObject.AddComponent<BoxCollider>();
            collider.size = section.transform.gameObject.GetComponent<MeshFilter>().mesh.bounds.extents;
            Rigidbody rb = section.transform.gameObject.AddComponent<Rigidbody>();
            rb.interpolation = RigidbodyInterpolation.Interpolate;
            rb.collisionDetectionMode = CollisionDetectionMode.Continuous;
            rb.velocity = playerController.GetPlayerVelocity();
            rb.AddForce(section.homeLocation.normalized * 5);
        }
        sphereSections = new SectionData[0];
    }
    */
}
