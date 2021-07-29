using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DitzelGames.FastIK;

public class Boss1Animator : MonoBehaviour {
    [Header("References")]
    [SerializeField] private Boss1Controller controller;
    [SerializeField] private Transform[] armEnds;
    [SerializeField] private Transform mid;
    [SerializeField] private Transform[] footEnds;

    [Header("Sheild Movement")]
    [SerializeField] private float armMoveSpeed;
    [SerializeField] private float armRotateSpeed;
    [SerializeField] private float armHeightOffset;
    [SerializeField] private float armRadiusMin;
    [SerializeField] private float armRadiusDelta;
    [SerializeField] private float armDisconnectForce;
    [SerializeField] private float armBitEjectForce;
    [SerializeField] private float downRot;
    [SerializeField] private float upRot;

    [Header("Walking Movement")]
    [SerializeField] private float footRadius;
    [SerializeField] private float footHeightOffset;
    [SerializeField] private float footMoveDistance;
    [SerializeField] private float footMoveSpeed;
    [SerializeField] private float footMoveHeight;
    [SerializeField] private AnimationCurve footMoveHeightCurve;
    [SerializeField] private float overstepPercent;
    private Transform armTargetAndPoleContainer;
    private List<Arm> arms;
    private class Arm {
        public int armNumber;
        public Vector3 homeDir;
        public Transform ikEnd;
        public Transform ikTarget;
        public Transform ikPole;
        public Transform shield;
        public FastIKFabric ikComponent;
        public float currentRot;
    }
    private List<Foot> feet;
    private class Foot {
        public Vector3 homeDir;
        public Transform ikEnd;
        public Transform ikTarget;
        public Transform ikPole;
        public Vector3 movementGoal;
        public float movementPercent;
        public FastIKFabric ikComponent;
        public bool isMoving;
    }

    private bool legInAir = false;


    void Start() {
        armTargetAndPoleContainer = new GameObject("Arm Target And Pole Container").transform;
        armTargetAndPoleContainer.SetParent(this.transform);
        arms = new List<Arm>();
        for (int i = 0; i < armEnds.Length; i++) {
            Arm newArm = new Arm();
            newArm.armNumber = i + 1;
            newArm.ikEnd = armEnds[i];
            newArm.shield = newArm.ikEnd.GetChild(0);
            Vector3 localPos = newArm.ikEnd.position - this.transform.position;
            newArm.homeDir = new Vector3(localPos.x, 0, localPos.z).normalized;
            newArm.ikComponent = newArm.ikEnd.gameObject.AddComponent<FastIKFabric>();
            newArm.ikTarget = newArm.ikComponent.Target;
            newArm.ikTarget.SetParent(armTargetAndPoleContainer);
            newArm.ikPole = new GameObject("IK Pole " + i).transform;
            newArm.ikPole.SetParent(armTargetAndPoleContainer);
            newArm.ikComponent.Pole = newArm.ikPole;
            newArm.ikPole.position = this.transform.position + Vector3.up * 50f;
            newArm.ikComponent.ChainLength = 2;
            newArm.ikComponent.SnapBackStrength = 0;
            arms.Add(newArm);
        }
    }

    public void UpdateArms(Vector3 playerPos, Boss1Controller.State state) {
        switch (state) {
            case Boss1Controller.State.facingOut:
                foreach(Arm arm in arms) {
                    float angle = Vector3.Angle(arm.homeDir, this.transform.position - playerPos);
                    float angleInfluence = Mathf.Clamp(UtilityFunctions.Remap(angle, 0, 180, 0, 1), 0, 1);
                    RaycastHit hit;
                    int layermask = 1 << 6;
                    if (!Physics.Raycast(arm.ikEnd.position + Vector3.up * 100, Vector3.down, out hit, Mathf.Infinity, layermask)) {
                        throw new System.Exception("Boss arm raycast didn't hit terrain");
                    }
                    float height = armHeightOffset + Mathf.Clamp(playerPos.y * angleInfluence, hit.point.y, 100f);
                    float armDistance = armRadiusMin + (arm.armNumber % 2) * 10f;
                    Vector3 targetPos = UtilityFunctions.VectorTo2D(this.transform.position) + arm.homeDir * armDistance + Vector3.up * height;
                    arm.ikTarget.position = Vector3.Lerp(arm.ikTarget.position, targetPos, Time.deltaTime * armMoveSpeed);
                    if (arm.currentRot < 67) arm.currentRot = Mathf.Min(arm.currentRot + Time.deltaTime * armRotateSpeed, 67);
                    else if (arm.currentRot > 67) arm.currentRot = Mathf.Max(arm.currentRot - Time.deltaTime * armRotateSpeed, 67);
                    arm.shield.localRotation = Quaternion.Euler(0, arm.shield.localRotation.eulerAngles.y, arm.currentRot);
                }
                break;
            case Boss1Controller.State.facingDown:
                foreach(Arm arm in arms) {
                    Vector3 targetPos = this.transform.position + arm.homeDir * (armRadiusMin);
                    arm.ikTarget.position = Vector3.Lerp(arm.ikTarget.position, targetPos, Time.deltaTime * armMoveSpeed);
                    if (arm.currentRot < downRot) arm.currentRot = Mathf.Min(arm.currentRot + Time.deltaTime * armRotateSpeed, downRot);
                    else if (arm.currentRot > downRot) arm.currentRot = Mathf.Max(arm.currentRot - Time.deltaTime * armRotateSpeed, downRot);
                    arm.shield.localRotation = Quaternion.Euler(0, arm.shield.localRotation.eulerAngles.y, arm.currentRot);
                }
                break;
            case Boss1Controller.State.facingUp:
                foreach(Arm arm in arms) {
                    Vector3 targetPos = this.transform.position + arm.homeDir * (armRadiusMin);
                    arm.ikTarget.position = Vector3.Lerp(arm.ikTarget.position, targetPos, Time.deltaTime * armMoveSpeed);
                    if (arm.currentRot < upRot) arm.currentRot = Mathf.Min(arm.currentRot + Time.deltaTime * armRotateSpeed, upRot);
                    else if (arm.currentRot > upRot) arm.currentRot = Mathf.Max(arm.currentRot - Time.deltaTime * armRotateSpeed, upRot);
                    arm.shield.localRotation = Quaternion.Euler(0, arm.shield.localRotation.eulerAngles.y, arm.currentRot);
                }
                break;
            case Boss1Controller.State.walking:
                if (legInAir) {
                    foreach (Foot foot in feet) {
                        if (foot.isMoving) {
                            foot.movementPercent += Time.deltaTime * footMoveSpeed;
                            foot.ikTarget.transform.position = Vector3.Lerp(foot.ikTarget.transform.position, foot.movementGoal, foot.movementPercent) + Vector3.up * footMoveHeightCurve.Evaluate(foot.movementPercent) * footMoveHeight;
                            if (foot.movementPercent >= 1) {
                                foot.isMoving = false;
                                legInAir = false;
                            }
                        }
                    }
                } else {
                    float maxDist = 0;
                    Foot footToMove = null;
                    Vector3 goal = Vector3.zero;
                    foreach (Foot foot in feet) {
                        RaycastHit hit;
                        int layermask = 1 << 6;
                        Physics.Raycast(this.transform.position + foot.homeDir.normalized * footRadius + Vector3.up * 50, Vector3.down, out hit, Mathf.Infinity, layermask);
                        Vector3 goalPos = hit.point + Vector3.up * footHeightOffset;
                        float dist = Vector3.Distance(goalPos, foot.ikTarget.position);
                        if (dist > maxDist) {
                            maxDist = dist;
                            footToMove = foot;
                            goal = goalPos;
                        }
                    }
                    if (maxDist > footMoveDistance) {
                        footToMove.movementGoal = (goal - footToMove.ikTarget.position) * overstepPercent + footToMove.ikTarget.position;
                        footToMove.isMoving = true;
                        legInAir = true;
                        footToMove.movementPercent = 0;
                    }
                }
                break;
            default:
                Debug.LogError("Unhandled boss1 state");
                break;
        }
    }

    public void DestroyArm(Transform jointTransform, int armNumber) {
        Arm destroyedArm = GetArm(armNumber);
        if (destroyedArm == null) {
            throw new System.Exception("Destroyed arm not in arms list");
        }
        Destroy(destroyedArm.ikComponent.gameObject);
        Destroy(destroyedArm.ikPole.gameObject);
        Destroy(destroyedArm.ikTarget.gameObject);
        MeshCollider meshCollider = destroyedArm.ikEnd.GetChild(0).GetChild(0).GetComponent<MeshCollider>();
        meshCollider.convex = true;
        arms.Remove(destroyedArm);
        jointTransform.SetParent(null, true);
        Rigidbody rb = jointTransform.gameObject.AddComponent<Rigidbody>();
        rb.mass = 1000;
        rb.AddForce((destroyedArm.homeDir).normalized * armDisconnectForce, ForceMode.Force);

        //if (arms.Count == 0) {
            foreach (Arm arm in arms) {
                DestroyArm(arms[0].ikEnd.parent, arms[0].armNumber);
            }
            EjectArmBits();
            controller.AllArmsDestroyed();
            setupFeet();
        //}
    }

    Arm GetArm(int armNumber) {
        foreach(Arm arm in arms) {
            if (arm.armNumber == armNumber) {
                return arm;
            }
        }
        return null;
    }

    void EjectArmBits() {
        for (int i = 0; i < mid.childCount; i++) {
            if (mid.GetChild(i).childCount > 0) {
                Transform bit = mid.GetChild(i).GetChild(0);
                bit.SetParent(null);
                Rigidbody rb = bit.gameObject.AddComponent<Rigidbody>();
                rb.mass = 100;
                rb.AddForce(UtilityFunctions.VectorTo2D(this.transform.position - bit.position).normalized * armBitEjectForce);
            }
        }
        Transform cap = mid.GetChild(mid.childCount - 1);
        cap.SetParent(null);
        Rigidbody capRb = cap.gameObject.AddComponent<Rigidbody>();
        capRb.mass = 100;
        
        mid.SetParent(null);
        Rigidbody midRb = mid.gameObject.AddComponent<Rigidbody>();
        midRb.mass = 100;
        midRb.AddForce(Vector3.up * armBitEjectForce);
    }

    void setupFeet() {
        feet = new List<Foot>();
        for (int i = 0; i < footEnds.Length; i++) {
            Foot newFoot = new Foot();
            newFoot.ikEnd = footEnds[i];
            Vector3 localPos = newFoot.ikEnd.position - this.transform.position;
            newFoot.homeDir = new Vector3(localPos.x, 0, localPos.z).normalized;
            newFoot.ikComponent = newFoot.ikEnd.gameObject.AddComponent<FastIKFabric>();
            newFoot.ikTarget = newFoot.ikComponent.Target;
            newFoot.ikPole = new GameObject("IK Pole " + i).transform;
            newFoot.ikPole.SetParent(armTargetAndPoleContainer);
            newFoot.ikComponent.Pole = newFoot.ikPole;
            newFoot.ikPole.position = this.transform.position + Vector3.up * 50f;
            newFoot.ikComponent.ChainLength = 2;
            newFoot.ikComponent.SnapBackStrength = 0;
            feet.Add(newFoot);
        }
    }
}
